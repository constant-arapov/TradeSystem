#ifndef _CSTOPWATCH_
#define _CSTOPWATCH_

#include <Windows.h>


class CStopwatch
{
	private:
		_int64 _frequency;
		long double _res;
		_int64 _counterStart;
		_int64 _counterStop;

		void Reset();
	public:
		CStopwatch();
		void Restart();
		void Stop();
		void Update();
		
		_int64 Cnt;
		
		double Sec;
		double MiliSec;
		double MicroSec;
		double NanoSec;
};


#endif