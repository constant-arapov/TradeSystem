#ifndef _SYNCHRO_
#define _SYNCHRO_

#include <Windows.h>


class CSynchro
{
	protected:
		HANDLE _evWaitInstrumentsLoaded;
		HANDLE _evSignalConnectorReady;
	public:
		CSynchro();

		void WaitInstrLoadedFromDealingServer();
		void SetConnectorReadyForDealingServer();

};
#endif