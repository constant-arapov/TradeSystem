using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Threading;



namespace zTestStopwatch
{
    class Program
    {
        static void Main(string[] args)
        {

            Test t = new Test();

        }

        

        public class Test
        {
            List<long> perf = new List<long>();
            bool b = false;

            public Test()
            {
                Stopwatch sw = new Stopwatch();
           
                while (true)
                {
                   
                    sw.Start();
                    if (!b) b = true;
                    else
                    Thread.Sleep(1000);
                    sw.Stop();

                    if (sw.ElapsedMilliseconds > 10)
                    {
                        int t = 1;

                    }
                    perf.Add(sw.ElapsedMilliseconds);
                   // sw.Reset();
                }

             //   Thread.Sleep(10);


            }
        }

    }
}
