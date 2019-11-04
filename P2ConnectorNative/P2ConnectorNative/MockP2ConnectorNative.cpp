#include "ThreadingCreators.h"

#include "MockP2ConnectorNative.h"
#include "CMapperGlobals.h"
#include "CSynchro.h"

#include "CP2ConnectorNative.h"

 MockP2ConnectorNative:: MockP2ConnectorNative (CMapperGlobals * pMapperGlobals, CSynchro * synchro) 
	 : CP2ConnectorNative(pMapperGlobals,synchro)
 {
	 
 }


void MockP2ConnectorNative::InitSimulate()
{

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
	//_listIsinId.push_back(1);
	 

	//itit  istruments
	_stockRouter.Init(_listIsinId);
	Log("Instrument init for StockRouter");
	
	StartThread(this, StartCP2ConnectorNativeNewThread);

}