#include "MockSynchro.h"


void MockSynchro :: SetInstrumentLoaded()
{
	SetEvent(_evWaitInstrumentsLoaded);

}

