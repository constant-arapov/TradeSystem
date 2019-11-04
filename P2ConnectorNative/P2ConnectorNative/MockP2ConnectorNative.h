#ifndef _MockP2ConnectorNative_
#define _MockP2ConnectorNative_

#include "CP2ConnectorNative.h"
#include "CMapperGlobals.h"
#include "CSynchro.h"



class MockP2ConnectorNative :  public CP2ConnectorNative
{
	public:
		MockP2ConnectorNative (CMapperGlobals * pMapperGlobals, CSynchro * synchro) ;
		
		void InitSimulate();

};




#endif