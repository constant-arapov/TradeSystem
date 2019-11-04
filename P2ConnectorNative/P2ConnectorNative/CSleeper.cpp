#include "CSleeper.h"


CSleeper::CSleeper ()
{
	_waiSinceLastDataMs = 1;
	_parSleep = 20;
	_bAnyDatRcved = false;
	_bSleepPerFinished = false;
}



void CSleeper::OnDataRecieved()
{
	_bAnyDatRcved = true;
	//Restart counters on data recieved
	_swSinceLastDataRcv.Restart(); 
	_bSleepPerFinished = false;

}


void CSleeper::OnTimeOut()
{
	//refresh stopwatch counters
	_swSinceLastDataRcv.Update();

	//Data already recieved and if we not where in sleep yet
	if  (_bAnyDatRcved && !_bSleepPerFinished )
	{
		//If during given timespan (1 ms) we have not recieved data
		//do sleep.
		if (_swSinceLastDataRcv.MiliSec > _waiSinceLastDataMs)
		{
			Sleep(_parSleep);
			//after sleep was finished do permanent request
			//till data received again
			_bSleepPerFinished = true; 
		}
	}


}

