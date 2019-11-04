#include <Windows.h>
#include <list>
#include <string>


#include "aggr.h"


#include "ThreadingCreators.h"
#include "CStopWatch.h"

#include "EnmTransactCodes.h"

#include "CStockProcessor.h"

#include "CLogger.h"


using namespace std;





CStockProcessor:: CStockProcessor(long isinId)
{
	//sizeof_orders_aggr
	_isinId =isinId;
	

	_sizeStockOneDir = sizeof_orders_aggr * STOCK_SIZE;
	memset(_bufferRaw,0,STOCK_BUFFER_SIZE);
	currBufferRowElCount = 0;

	InitializeCriticalSection(&_critSect);
	_evForceQueueProc =  CreateEvent(NULL,false, false, NULL);
	 StartThread(this, StartCStockProcessorNewThread);
	
	 _mapper = new CMapperStock(_isinId);
	 _mapper->Init();
}


/// <summary>
/// Entry point of class.
///
/// Accumulate data in _queueEntry
/// </summary>
void CStockProcessor :: AddStocStruct(TStockStruct stockStruct) 
{		
	EnterCriticalSection(&_critSect);
		_queueEntry.push(stockStruct);
	LeaveCriticalSection(&_critSect);
	
}

/// <summary>
/// Process _queueProcessing if "transaction end"
/// message found do process transaction
///
/// See algo desc in class header.
/// </summary>
void CStockProcessor :: ThreadProcess()
{
	size_t sz=0;
	DWORD msWait = 1; 
	bool bNeedProcessTransact;

	char buff[255];
	sprintf(buff, "stockProc  %d", _isinId);

	 _logger = new CLogger (string(buff));
	_logger->Log("test");

	while (true)
	{
		bNeedProcessTransact = false;
		//was last elemnt on prev operation now must be empty
		if (sz<=1)
			WaitForSingleObject(_evForceQueueProc, msWait);

		EnterCriticalSection(&_critSect);
		sz = _queueEntry.size();

		if (sz != 0)
		{
			TStockStruct stockStruct = _queueEntry.front();
			if (stockStruct.code == EnmTransactCodes_End)
			{
				bNeedProcessTransact = true;
			}
			else 
				_queueProcessing.push(stockStruct);
			
			_queueEntry.pop();
		}
		LeaveCriticalSection(&_critSect);
	
		if (bNeedProcessTransact)
			ProcessTransaction();
			
		
	}


}


/// <summary>
/// Process each element in transaction and call ProcessElement
/// (which is update _rawBuffer).
///  After transaction processed call ProcessWorkBuffer(which is build workbuffer)
///
/// Called from ThreadProcess
/// </summary>
void CStockProcessor :: ProcessTransaction()
{
	//update row buffer first
	while(!_queueProcessing.empty())
	{
		TStockStruct stockStruct = _queueProcessing.front();
		ProcessElement(stockStruct.orders_aggr_inst);
		_queueProcessing.pop();

	}
	   	
	 ProcessWorkBuffer();
}


/// <summary>
/// Update row buffer. Each element has replId. If element 
/// found do update it. If not do add element to the end of buff.
///
/// Called from ProcessTransaction.
/// </summary>
void CStockProcessor :: ProcessElement(const orders_aggr &  rcvdOrdersAggr)
{


	  orders_aggr* orderAggr  =  (orders_aggr*)   _bufferRaw;
	  
	  int i =0 ;
	  int currSize = 0;
	  //while not last element in buffer
	  while (orderAggr->replID != 0)
	  {
		  //element found, update it
		  if (rcvdOrdersAggr.replID == orderAggr->replID)
		  {
			 *orderAggr = rcvdOrdersAggr;
			 break;
		  }

		  i++;		 
		  currSize = i * sizeof_orders_aggr;
		  if (currSize<STOCK_BUFFER_SIZE)
		  {
			
			  orderAggr = (orders_aggr* ) _bufferRaw +i;
		  }
		  else
		  {
			  //TODO ERROR
			  break;
		  }
	  }

	  //element not found add it
	  if (currBufferRowElCount == i)
	  {		//2018-04-13 no volume - no need to add as it for remove value we've not recieved
		  if (rcvdOrdersAggr.volume !=0)
		  {


			*((orders_aggr* ) _bufferRaw +i) = rcvdOrdersAggr;
			i++;
			currBufferRowElCount = i;

		    char buff[255]; 
			sprintf(buff,"====CStockProcessor :: ProcessElement _bufferRaw  ===== Adding==== i=%d ===============",i);
			_logger->Log(string(buff));
		  }
	  }

	
	 
	 	  
}




/// <summary>
/// 1. Put element to _bufferWork. If direction is "buy"
///   do put to the first half of buffer if "sell" do
///    copy to the second half.
/// 2. Sort elements by price. "Buy" half sort desc, 
///	   "Sell" half do sort inc.
/// 3. Copy workBuffer to _stockMapper
//
/// Called from ProcessTransaction.
/// </summary>
void CStockProcessor ::  ProcessWorkBuffer()
{

	
	 //TODO for debugging, not use in production
	 memset(_bufferWork,0, STOCK_BUFFER_SIZE);
	// memcpy(localBuffer,bufferWork,STOCK_BUFFER_SIZE);
	//copy data to workBuffer
	  orders_aggr* orderAggr  =  (orders_aggr*)   _bufferRaw;
	  
	  int i =0 ;
	  int currSize = 0;

	

	  int iBid =0;
	  int iAsk =0;
	  
																

	  do 
	  {
		  		  		 		  		 		  			
		  orderAggr = (orders_aggr* ) _bufferRaw +i;

		  if (orderAggr->volume == 0)
		  {
			  //2018-04-12 if with no increment will be infinite cycle
			  i++; 
			  continue;
		  }
		  
		  if (orderAggr->dir == Buy)
		  {
			  //copy to bid area
			  *((orders_aggr* )_bufferWork + iBid) = *orderAggr;
			  iBid++;
		  }
		  else
		  {	
			  //copy to ask area	
			  //sizeof_orders_aggr 84*50 = 4200 
			  *((orders_aggr* )_bufferWork + STOCK_SIZE + iAsk) = *orderAggr;
			  iAsk++;
		  }
		 


		  currSize = ++i * sizeof_orders_aggr;
	  }
	  while (currSize < STOCK_BUFFER_SIZE && 
		  ((orders_aggr* ) _bufferRaw + i)->replID !=0 );
	 
																			
																			//CStopwatch sw;	
																			//sw.Restart();
		
		
																			//sw.Stop();
	   SortDir(0, iBid-1, Buy);
	   SortDir(STOCK_SIZE, STOCK_SIZE+iAsk-1, Sell );


	   //list<string> lstAll;	  
	   //DbgSort(0, STOCK_SIZE+iAsk, lstAll);

	   _mapper->Write(_bufferWork);
	   DbgPrintWorkBuff();
   //    DbgSort(STOCK_SIZE, STOCK_SIZE+iAsk ,lstAsks);


}


void CStockProcessor::DbgPrintWorkBuff()
{
	  int i =0 ;
	  int currSize = 0;


	  orders_aggr* orderAggr  =  (orders_aggr*)   _bufferWork;

	  char buff[255];
	  _logger->Log("=========================begin=========================================================");


	  do 
	  {
		  		  		 		  		 		  			
		  orderAggr = (orders_aggr* ) _bufferWork +i;

		  char msgLog[255];
		   int64_t value=0;
	       int8_t scale=0;
	      int res = cg_bcd_get(orderAggr->price, &value,&scale);
		  double dblPrice= (double) value/pow(10.0, scale);
		  
		  sprintf(buff,"i=%d replId=%d replRev=%d dir=%d price=%f volume=%d", 
			      i, 
			      orderAggr->replID,
				  orderAggr->replRev,
				  orderAggr->dir,
				  dblPrice,
				  orderAggr->volume
				  
				  );

		  _logger->Log(string(buff));
		 
		 


		  currSize = ++i * sizeof_orders_aggr;
	  }
	  while (currSize < STOCK_BUFFER_SIZE /*&& 
		  ((orders_aggr* ) _bufferWork + i)->replID !=0*/ );

   _logger->Log("=========================end=========================================================");
  



}



/// <summary>
/// Sorts elements in WorkBuffer. Implementing "bulk sort" algo.
///
/// Call from  ProcessWorkBuffer
/// </summary>
/// <param name="first">First element in workbuffer to process. Zero for bids. 
///						STOCK_SIZE for asks</param>
/// <param name="last">Last element in workbuffer to process. STOCK_SIZE for bids. 
///						STOCK_SIZE+iAsk for asks</param>
///<param name="dir">Direction "Buy" or "Sell". For "Buy" sort desc. 
///      			 For "Asc" sort desc	</param>
void CStockProcessor::SortDir(int first, int last, EnumBuySell dir)
{
	  for (int i=first; i<last; i++)
	  {		
		  for (int j=first; j< first + last - i; j++)
		  {			
				orders_aggr* currOrdJ = (orders_aggr* )_bufferWork + j;
				orders_aggr* currOrdJ_plus_1 = (orders_aggr* )_bufferWork + j+1;

				double priceJ = GetDoublePrice(currOrdJ->price);
				double priceJ_plus_1 = GetDoublePrice(currOrdJ_plus_1->price);

				if (priceJ_plus_1 > priceJ && dir == Buy ||
					priceJ_plus_1 < priceJ && dir == Sell )
				{

					orders_aggr tmp = *currOrdJ;
					*currOrdJ = *currOrdJ_plus_1;
					*currOrdJ_plus_1 = tmp;
				}				
			}

		  }

	  

}


//For debugging do not call in production
void CStockProcessor::DbgSort(long firstEl, long lastEl, list<string> &lst )
{
	 char buff[255];

	 
	 for (int i=firstEl; i<lastEl; i++)
	 {
		 orders_aggr* currOrd = (orders_aggr* )_bufferWork + i;
					sprintf(buff, "%d  %4.2f",currOrd->replID, GetDoublePrice(currOrd->price));
					lst.push_back(string(buff));
			

	 }

}


double CStockProcessor::GetDoublePrice(void * pPrice)
{
	int64_t value;
	int8_t scale;

	int res = cg_bcd_get(pPrice, &value,&scale);

	double dblPrice= (double) value/pow(10.0, scale);
				//char[11]

	return dblPrice;
}










