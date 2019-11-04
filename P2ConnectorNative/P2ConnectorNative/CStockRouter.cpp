#include <map>


#include "CStockRouter.h"
#include "EnmTransactCodes.h"

using namespace std;


void  CStockRouter:: Init(const list<long>  & listIsinIds)
{
	_instrumentCount = listIsinIds.size();

	
	
	//*Create StockProcessors	
	for (list<long>::const_iterator iterator = listIsinIds.begin(), end = listIsinIds.end(); 
																iterator != end; ++iterator) 
	{
		long isinId = *iterator;
		_mapStockProcessors[isinId] =  new CStockProcessor(isinId);
		

		_mapInstrNeedFlush[isinId] = false;
	}

		
}

/// <summary>
/// Entry point of class.
///
/// Called from CP2ConnectorNative
/// </summary>
void CStockRouter::Route(const TStockStruct & stockStruct)
{
		
		if (stockStruct.code == EnmTransactCodes_Begin)
		{
			//nothing to do at the moment

		}		
		else if (stockStruct.code == EnmTransactCodes_End)
		{
			// transaction end - do process accumulated data
			ProcessTransaction();					
		}
		
		else if (stockStruct.code == EnmTransactCodes_Data)
		{	//new data - accumulate it
			_queueRouteEntry.push(stockStruct);
		}
	
}

/// <summary>
/// Process transaction. Copy data from  _queueRouteEntry
/// to coresponded _mapStockProcessor. Also if at least
/// one data element of instrument was processed, add extra
/// "transaction-end" message.
//
/// See algo desc in class header.
/// Called from Route method
/// </summary>
void CStockRouter::ProcessTransaction()
{
	while (!_queueRouteEntry.empty())
	{
		TStockStruct stockStruct = _queueRouteEntry.front();
		_queueRouteEntry.pop();
		long isinId =stockStruct.orders_aggr_inst.isin_id;

		
		
		_mapInstrNeedFlush[isinId] = true;				
		(_mapStockProcessors[isinId])->AddStocStruct(stockStruct );	
	}


	for (map<long,bool>::iterator it = _mapInstrNeedFlush.begin(), end = _mapInstrNeedFlush.end(); 
																it != end; ++it) 
	{
		//if data sent
		if (it->second == true)
		{	
			//switch off
			it->second = false;

			//and add transaction end message
			TStockStruct  stockStructEmpty = { EnmTransactCodes_End, NULL };
			(_mapStockProcessors[it->first])->AddStocStruct(stockStructEmpty );
			
		}
	}

}







