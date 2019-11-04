#ifndef _THREADING_CREATORS_
#define _THREADING_CREATORS_

#include <Windows.h>
void StartThread(LPVOID object, LPTHREAD_START_ROUTINE startingRoutine);

DWORD __stdcall  StartCP2ConnectorNativeNewThread(LPVOID arg);

DWORD __stdcall StartCStockProcessorNewThread(LPVOID arg);

DWORD _stdcall StartCNamedPipeClientThreadReader(LPVOID arg);

DWORD _stdcall StartCNamedPipeClientThreadWriter(LPVOID arg);

#endif