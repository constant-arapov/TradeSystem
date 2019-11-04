#include "stdafx.h"
#include <Windows.h>

#include "Clocker.h"


Clocker::Clocker()
{
	InitializeCriticalSection(&_criticalSect);
}

void Clocker::Lock()
{
	EnterCriticalSection(&_criticalSect);
}


void Clocker::Unlock()
{
	LeaveCriticalSection(&_criticalSect);
}

