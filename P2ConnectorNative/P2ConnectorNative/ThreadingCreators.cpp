#include <Windows.h>

#include "CP2ConnectorNative.h"
#include "CStockProcessor.h"

#include "CNamedPipeClient.h"



void StartThread(LPVOID object, LPTHREAD_START_ROUTINE startingRoutine)
{

	CreateThread(NULL,4096,startingRoutine, object,0, NULL);
}




//TODO factory with parameters
DWORD __stdcall StartCP2ConnectorNativeNewThread(LPVOID arg)
{

	CP2ConnectorNative * p2ConnectorNative = (CP2ConnectorNative *) arg;

	p2ConnectorNative->ThreadRouteOrdersAggr();
	return 1;

}


DWORD __stdcall StartCStockProcessorNewThread(LPVOID arg)
{
	CStockProcessor * stockProcessor = (CStockProcessor *) arg;	
	stockProcessor->ThreadProcess();

	return 1;

}

DWORD _stdcall StartCNamedPipeClientThreadReader(LPVOID arg)
{
	CNamedPipeClient * namedPipeClient = (CNamedPipeClient *) arg;
	namedPipeClient->ThreadReader();

	return 1;
}


DWORD _stdcall StartCNamedPipeClientThreadWriter(LPVOID arg)
{
	CNamedPipeClient * namedPipeClient = (CNamedPipeClient *) arg;
	namedPipeClient->ThreadWriter();

	return 1;
}




