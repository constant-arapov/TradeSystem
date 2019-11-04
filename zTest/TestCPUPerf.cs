using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Utils;

using System.Threading;



namespace zTest
{
    public class TestCPUPerf
    {

        private int COUNT = 300;
        private Semaphore _sm;

      //  private List<bool> _lst = new List<bool>();

        List<Thread> threads = new List<Thread>();

        AutoResetEvent ev= new AutoResetEvent(false);

        public void DoTest()
        {
            

           // _sm = new Semaphore(0, COUNT);

            DateTime dt = DateTime.Now;

            for (int i = 0; i < COUNT; i++)
            {
                Thread nt = new Thread(WokingThread);
                nt.Start();
                threads.Add(nt);
                //CUtil.ThreadStart(WokingThread);

            }

           

         /*   for (int i = 0; i < COUNT; i++)
            {
                ev.Set();
            }
            */
            foreach (var thread in threads)
                thread.Join();

           

            double sec = (DateTime.Now - dt).TotalSeconds;
            double min = sec / 60;

            if (min != 0)
                Thread.Sleep(0);

        }

        public void WokingThread()
        {
          //  ev.WaitOne();

            double b = 0;
            for (double i = 0; i < 3000000000; i++)
                b += Math.Sqrt(i);

            if (b != 4)
                System.Threading.Thread.Sleep(0);

        }




    }
}
