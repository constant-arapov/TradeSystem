#include <windows.h>
#include <WinCon.h>
#include <stdio.h>



#include <functional>
#include <algorithm>
#include <list>
#include <string>
#include <queue>



#define CG_SHORT_NAMES 
#include "cgate.h"
#include "cgate_callback.h"



#include "utils.h"
#include "CLogger.h"


#include "DEALS.h"
#include "aggr.h"
#include "TStockStruct.h"
#include "EnmTransactCodes.h"
#include "ThreadingCreators.h"

#include "CP2ConnectorNative.h"
#include "CAlarmer.h"
#include "CSynchro.h"


/*

	CONSTRUCTOR

*/
CP2ConnectorNative::CP2ConnectorNative (CMapperGlobals * pMapperGlobals, CSynchro * synchro)
	:_logger("ConnectorStockAggr", false)
{
	


	_logger.LogStartBanner();

	_sizeOrdersAggr = sizeof(orders_aggr);

	_synchro = synchro;



	_evWaitForStockProcess = CreateEvent(NULL,false, false, NULL);

	InitializeCriticalSection(&_critsectQueueTransact);


#ifdef _WIN32
	SetConsoleCtrlHandler(InterruptHandler, 1);
#endif//_WIN32
		
	

}
//================================ END OF CONSUCTOR ====================================================================================
/// <summary>
/// Entry point to class.
///
/// 1) Creates main components, signals/waits of global server
/// 2) Create cgate objects
/// 3) Manages cgates connection/listeners (opening if need etc ....)
/// </summary>
void CP2ConnectorNative ::Process()
{
	
	Log("Starting process");

	
	//create global files data - instruments etc
	_mapperGlobals.CreateGlobals();
	Log("Create globals success");

	_synchro->SetConnectorReadyForDealingServer();
	Log("Send dealing server ready state");
	

	 //wait till dealing server loads instruments
	 //WaitForSingleObject(_evWaitInstrumentsLoaded, INFINITE); 
	_synchro->WaitInstrLoadedFromDealingServer();
	Log("Rcv responce deaing server loaded instruments");

	 
	_listIsinId = _mapperGlobals.GetInstrumentList();
	 

	//itit  istruments
	_stockRouter.Init(_listIsinId);
	Log("Instrument init for StockRouter");
	
	StartThread(this, StartCP2ConnectorNativeNewThread);

	

	const char* conn_str = "p2tcp://127.0.0.1:4001;app_name=BotsSystem_ConnectionStocksData";

	
	const char* lsn_str = "p2repl://FORTS_FUTAGGR50_REPL";

	char buff[MAX_PATH];
	
	char * useRealPlaza2 =  getenv("USE_REAL_PLAZA2_SERVER");
	char *key = NULL;
	if (useRealPlaza2 == NULL)
	{
		Error("USE_REAL_PLAZA2_SERVER not found");
		return;
	}
	else if (useRealPlaza2[0] == '1') //TODO from global settings	
		key = "dfTR6WasPsXBFOJ6nELg07Nf0UfH2Bz";	
	else //demo	
		key="11111111";


	char * confPath = getenv("CONFIG_PATH");
	if (confPath == NULL)
	{
		Error("Env CONFIG_PATH not found");
		return;
	}

	
	sprintf(buff,"ini=%s\\P2ConnectorNative\\repl.ini;key=%s",
					confPath,key);
	
	
	CG_RESULT res  = cg_env_open(buff);
	if (res != CG_ERR_OK)
	{
		Error("Unable to open cgate invironment");
	}
	Log("Cgate env init success");


	conn_t* conn = 0;
	listener_t* listener = 0;
	res = cg_conn_new (conn_str, &conn);


	if (res != CG_ERR_OK)
	{
		Error("Unable to open cgate connection");
		return;
	}
	

	Log("Con create success");

	 res=cg_lsn_new(conn,lsn_str,&MessageCallback,0 ,&listener);

	 if (res != CG_ERR_OK)
	 {
		Error("Unable to create listener");
		return;
	 }
	Log("Listener create success");


	bool bExit = false;


	while (!bExit)
	{

		uint32_t state;
		cg_conn_getstate(conn,&state);
		if (state == CG_STATE_ERROR)
		{
			cg_conn_close(conn);
		}
		else if (state == CG_STATE_CLOSED)
		{
			res =cg_conn_open(conn,0);			
		}
		else if (state == CG_STATE_ACTIVE)
		{

		   uint32_t result = cg_conn_process(conn,0, 0);

		  if (result == CG_ERR_OK)
		   {
			   _sleeper.OnDataRecieved();
		   }
		   if (result == CG_ERR_TIMEOUT)
		   {
			   _sleeper.OnTimeOut();
		   }
		  
		   
		  if (result != CG_ERR_OK && result != CG_ERR_TIMEOUT) 
			{
				log_info("Warning: connection state request failed: %X", result);
				Error("Warning: connection state request failed");
			}

		 result =  cg_lsn_getstate(listener, &state);

				
		 switch (state)
			{
			case CG_STATE_CLOSED: /* closed */

					// listener is CLOSED so open it
					//lsn_open(listener, 0);
				    //cg_lsn_open(listener,"mode=online");
				res = lsn_open(listener,0);
					/**/
					break;
			case CG_STATE_ERROR: /* error */
					// listener is in ERROR state so close it
				res =lsn_close(listener);
					break;						
			}
		}
	}


	
}



/// <summary>
/// Call from cgate callback when data recieved
/// Create element with "body" of orders_aggr and head of
/// EnmTransactCodes (type of elements: begin, commit or data).
/// Accumulates these data elements in _queueProcessing.
/// See algo desc in class header.
/// </summary>
void CP2ConnectorNative::ProcessOrdersAggr(orders_aggr * ordersAggr)
{
	
	
	//process only subscribed instruments
	if (!IsInSubscribedList(ordersAggr->isin_id))
		return;


	


	TStockStruct stockStruct = { EnmTransactCodes_Data, * ordersAggr };
	
	

	_queueTransact.push(stockStruct);

	LogStock(ordersAggr);
	
			

}
void CP2ConnectorNative::LogStock(orders_aggr * ordersAggr)
{
	char msgLog[255];
	int64_t value=0;
	int8_t scale=0;
	int res = cg_bcd_get(ordersAggr->price, &value,&scale);

	double dblPrice= (double) value/pow(10.0, scale);
	
	//short form
	//sprintf(msgLog,"replID=%llu replRev=%llu isin_id=%d" , 
		//		   ordersAggr->replID, ordersAggr->replRev, ordersAggr->isin_id);

	sprintf(msgLog,"replID=%llu replRev=%llu isin_id=%d dir=%d volume=%d price=%.4f" , 
					ordersAggr->replID,
					ordersAggr->replRev, 					
					ordersAggr->isin_id, 
					ordersAggr->dir,
					ordersAggr->volume,
					dblPrice);
				   
	
	Log(msgLog);
}

void CP2ConnectorNative ::Log(string message)
{
	_logger.Log(message);
}

void CP2ConnectorNative::LogBanner(string message)
{
	_logger.LogBanner(message);

}

void CP2ConnectorNative::LogStartBanner(string message)
{
	_logger.LogStartBanner();
}


bool  CP2ConnectorNative ::IsInSubscribedList(long isinId)
{


	auto itr = std::find(_listIsinId.begin(), _listIsinId.end(), isinId);
	if (itr == _listIsinId.end())
	  return	false;

	return true;
}

/// <summary>
/// Thread that Pops data from _queueProcessing and push it to stock router
/// See algo desc in class header.
/// </summary>
void CP2ConnectorNative :: ThreadRouteOrdersAggr()
{
	int sizeQueue=0;
	DWORD msWait =1; //initialy wait 1 ms

	while (true)
	{
		//was last elemnt on prev operation now must be empty
		if (sizeQueue<=1)
			WaitForSingleObject( _evWaitForStockProcess,  msWait);
			


		TStockStruct stockStruct;

		EnterCriticalSection(&_critsectQueueTransact);
		//remember for the last iteration
		sizeQueue = _queueProcessing.size();
		if (sizeQueue!=0)
		{
			stockStruct= _queueProcessing.front();
			_queueProcessing.pop();
			
			
		}
		LeaveCriticalSection(&_critsectQueueTransact);

		//after leave critical section, if data exist (sizeQueue!=0) 
		// delegate router processing
		if (sizeQueue!=0)
			_stockRouter.Route(stockStruct);


	}

}

/// <summary>
/// Call from cgate callback when begin message recieved.
/// </summary>
void CP2ConnectorNative::ProcessBegin()
{

	TStockStruct stockStruct = {EnmTransactCodes_Begin , NULL };
	_queueTransact.push(stockStruct);

	LogBanner("BEGIN");
}

void CP2ConnectorNative::ProcessOpen()
{
	LogBanner("OPEN");
}


void CP2ConnectorNative::ProcessOnline()
{
	LogBanner("ONLINE");
	//tempo debug
	_logger.Flush();
}

/// <summary>
/// Call from cgate callback on cimmit message recieved.
/// Copies data from  _queueTransact 
///  to _queueProcessing.
/// See algo desc in class header.
/// </summary>
void CP2ConnectorNative::ProcessCommit()
{

	TStockStruct stockStruct = {EnmTransactCodes_End , NULL };
	_queueTransact.push (stockStruct);
	


	bool bNeedProcessing =true;

	EnterCriticalSection(&_critsectQueueTransact);


	//assumption  - only two elemnts - begin and end no need to process more
	if (_queueTransact.size() == 2)
		bNeedProcessing = false;	




	while(!_queueTransact.empty())
	{
		//if need processing - copy to external queue
		if (bNeedProcessing)		
			_queueProcessing.push(_queueTransact.front());
		//if not just pop
					
		_queueTransact.pop();
	}

	LeaveCriticalSection(&_critsectQueueTransact);
	
	if (bNeedProcessing)
		SetEvent(_evWaitForStockProcess);


	LogBanner("COMMIT");
}










