#ifndef _CLOCKER_
#define _CLOCKER_

//#include "stdafx.h"
#include <Windows.h>

class Clocker
{
private:
		CRITICAL_SECTION _criticalSect;

public:
		Clocker();
		void Lock();
		void Unlock();


};






#endif
