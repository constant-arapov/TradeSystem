using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using System.Diagnostics;
using System.Threading;


using Common.Logger;

namespace zTestMem
{
    public class TestSleep
    {

        public TestSleep()
        {
            CLogger lg = new CLogger("TestSleep");

            lg.Log("start");



            Stopwatch sw = new Stopwatch();
            long currMs =0;
            long prevMs = 0;
            long delta = 0;
           

            bool bIsFirst = true;
            sw.Start();

            List<long> lstDelta = new List<long>();

            for (int i = 0; i < 100; i++)
            {
                currMs = sw.ElapsedMilliseconds;
                if (bIsFirst)
                    bIsFirst = false;
                else
                {                    
                    delta = currMs - prevMs;
                    lstDelta.Add(delta);
                  
                }
                prevMs = currMs;
                Thread.Sleep(1);
            }
            sw.Stop();


        }
        


    }
}
