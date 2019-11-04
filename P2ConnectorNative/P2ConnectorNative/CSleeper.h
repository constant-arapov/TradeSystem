#ifndef _CSleeper_
#define _CSleeper_

#include "CStopWatch.h"


class CSleeper
{

private:
	CStopwatch _swSinceLastDataRcv;
	int _waiSinceLastDataMs;
	int _parSleep;
	bool _bAnyDatRcved;
	bool _bSleepPerFinished;

public:
	CSleeper();
	void OnDataRecieved();
	void OnTimeOut();


};

#endif