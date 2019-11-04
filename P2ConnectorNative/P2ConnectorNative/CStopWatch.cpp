#include <Windows.h>

#include "CStopWatch.h"


CStopwatch::CStopwatch()
{

	 QueryPerformanceFrequency((LARGE_INTEGER *)&_frequency);
	 _res = 1.0/_frequency;
	 Reset();
}


void CStopwatch::Restart()
{

	Reset();
	QueryPerformanceCounter((LARGE_INTEGER *)&_counterStart);

}


void  CStopwatch::Stop()
{

	Update();

}

void  CStopwatch::Update()
{
	  QueryPerformanceCounter((LARGE_INTEGER *)&_counterStop);

	  Cnt = _counterStop - _counterStart ;
	 
	  Sec = Cnt  * _res ;

	  MiliSec = Sec  * 1000;
	  MicroSec = Sec * 1000000;
	  NanoSec = Sec * 1000000000;

}



void CStopwatch::Reset()
{	
	 _counterStart =0;
	 _counterStop = 0;

	 Cnt = 0;
	 
	 Sec = 0;
	 MiliSec = 0;
	 MicroSec = 0;
	 NanoSec = 0;

}

