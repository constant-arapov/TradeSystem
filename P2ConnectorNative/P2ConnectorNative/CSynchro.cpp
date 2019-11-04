
#include "CSynchro.h"


CSynchro :: CSynchro ()
{
	//TODO check if we need it remove if not
	CreateEventW(NULL , false,false,  TEXT("ATFS_Instument_loaded"));
	_evWaitInstrumentsLoaded  =  OpenEventW(EVENT_ALL_ACCESS,true,TEXT("ATFS_Instument_loaded"));
	_evSignalConnectorReady =  OpenEventW(EVENT_ALL_ACCESS,true,TEXT("ATFS_wait_P2NativeConnectorInitialized"));
	//_evSignalConnectorReady
}



void CSynchro ::WaitInstrLoadedFromDealingServer()
{
	 WaitForSingleObject(_evWaitInstrumentsLoaded, INFINITE); 
}


void CSynchro ::SetConnectorReadyForDealingServer()
{
	 SetEvent(_evSignalConnectorReady);

}