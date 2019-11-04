using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using System.Threading;



namespace Plaza2Connector.Common
{    
    public class CSleeper
    {
        private Stopwatch _swSinceLastDataRcv;
        int _waiSinceLastDataMs;
        int _parSleep;
        bool _bAnyDatRcved;
        bool _bSleepPerFinished;


        public  CSleeper()
        {
            _waiSinceLastDataMs = 1;
            _parSleep = 15;
            _bAnyDatRcved = false;
            _bSleepPerFinished = false;
            _swSinceLastDataRcv = new Stopwatch();
        }


        public void OnDataRecieved()
        {
            _bAnyDatRcved = true;
            //Restart counters on data recieved
            _swSinceLastDataRcv.Restart();
            _bSleepPerFinished = false;
        }


        public void OnTimeOut()
        {
          

            //Data already recieved and if we not where in sleep yet
            if (_bAnyDatRcved && !_bSleepPerFinished)
            {
                //If during given timespan (1 ms) we have not recieved data
                //do sleep.
                //if (_swSinceLastDataRcv.ElapsedTicks > 100)
                {
                    Thread.Sleep(_parSleep);
                    //after sleep was finished do permanent request
                    //till data received again
                    _bSleepPerFinished = true;
                }
            }


        }

    }
}
