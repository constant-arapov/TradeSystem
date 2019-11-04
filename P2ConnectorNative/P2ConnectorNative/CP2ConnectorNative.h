#ifndef _CP2ConnectorNative_
#define _CP2ConnectorNative_

#define CG_SHORT_NAMES 

#include <list>
#include <string>
#include <queue>


#include "cgate.h"

#include "CLogger.h"

#include "aggr.h"
#include "TStockStruct.h"
#include "CStockRouter.h"
#include "CMapperGlobals.h"
#include "CSynchro.h"
#include "CSleeper.h"

using namespace std;

/// <summary>
/// Main class of module.
/// 
/// Responsible for:
/// 1) Manage connection with cgate
/// 2) Recieving data from cgate callback
/// 4) Processing recieved data in transaction-oriented way. 
///		
///    DATA PROCESSING ALGO DESCRIPTION:
////
///    Recieveing data is preprocessed: add to original struct (orders_aggr)
///		header with data type (transaction begin, transaction end or data).Then
///		perform processing using steps bellow:
///		1. Accumulate data of transaction in _queueTransact
///		2. When transaction commited do copy data from  _queueTransact 
///			to _queueProcessing.
///		3. Thread ThreadRouteOrdersAggr process _queueProcessing
///			and push data to Stock router (see stockRouter description next)
/// </summary>
class CP2ConnectorNative
{
	private:
	
		CLogger _logger;


		int tmp;
		
		queue<TStockStruct> _queueTransact;
		queue<TStockStruct> _queueProcessing;


		

		int _sizeOrdersAggr;
		int _cntrOrderAggr;

		HANDLE _evWaitForStockProcess;	
		//HANDLE _evSignalConnectorReady;
		
	
		CSleeper _sleeper;

		CRITICAL_SECTION _critsectQueueTransact;
		void ProcessRouteEntry();
	   
		

		
		void LogBanner(string msessage);
		void LogStartBanner(string message);
		void LogStock(orders_aggr * ordersAggr);
	
	protected:
	
		CSynchro * _synchro;
		
		CStockRouter _stockRouter;
		CMapperGlobals _mapperGlobals;
		list<long> _listIsinId;

	



	public: 
		
		CP2ConnectorNative(CMapperGlobals * pMapperGlobals, CSynchro * synchro);
	
       //CG_RESULT  MessageCallback(cg_conn_t* conn, cg_listener_t* listener, struct cg_msg_t* msg, void* data);
	
		
	    void ProcessBegin();
	    void ProcessCommit();
		void ProcessOpen();
		void ProcessOnline();

		void ProcessOrdersAggr(orders_aggr * ordersAggr);
		void Process ();
	
	
		void ThreadRouteOrdersAggr();

		//for debug/testing
	    void DBGSetInstrumentLoaded();
		bool IsInSubscribedList(long isinId);
		
		
		void Log(string message);
};		

#endif