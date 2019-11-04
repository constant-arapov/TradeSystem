using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Collections.Concurrent;
using System.Threading;
using System.Diagnostics;



using Common;
using Common.Logger;
using Common.Collections;





namespace zTestBlockingCollection
{
    class Program
    {
        static void Main(string[] args)
        {


          //  Tester t = new Tester();

            TestBlockingQueue q = new TestBlockingQueue();
        }

      
    }

    public class Tester
    {

        BlockingCollection<string> _bc = new BlockingCollection<string>();


        public Tester()
        {
            (new Thread(ThreadProducer)).Start();
            (new Thread(ThreadConsumer)).Start();

        }





        private void ThreadProducer()
        {
            CLogger log = new CLogger("bcTestProducer");

            long i = 0;
            while (true)
            {
                string st = i++.ToString();
                log.Log(st);
                _bc.Add(st);
                
            }


        }

        private void ThreadConsumer()
        {
            CLogger log = new CLogger("bcTestConsumer");

            foreach (string st in _bc.GetConsumingEnumerable())
                log.Log(st);

        }


    }

    public class TestBlockingQueue
    {
        CBlockingQueue<string> _queue = new CBlockingQueue<string>();
     
        public TestBlockingQueue ()
        {
            (new Thread(ThreadProducer)).Start();
          /*  (new Thread(ThreadProducer)).Start();
            (new Thread(ThreadProducer)).Start();
            (new Thread(ThreadProducer)).Start();*/

            (new Thread(ThreadConsumer)).Start();



        }

        Stopwatch sw = new Stopwatch();

       private void ThreadProducer()
        {
           
            CLogger log = new CLogger("qProducer");
           
           long i=0;
            while (true)
            {
                string st = i++.ToString();
                sw.Start();
                _queue.Add(st);
                sw.Stop();
               // log.Log(st);
                if (sw.ElapsedMilliseconds > 20)
                {
                    int tmp = 1;
                }

                Thread.Sleep(1);
            }


        }

     private void ThreadConsumer()
        {

            CLogger log = new CLogger("qConsumer");
            long prev=0;
            while (true)
            {
                string st = _queue.GetElementBlocking();
                long curr = Convert.ToInt64(st);

                /*
                if (curr > 0)
                {
                    if (curr-prev != 1)
                        Thread.Sleep(0);

                }
                prev = curr;
                */


                //log.Log(st);
                //process element here
                Thread.Sleep(1);
            }
        }


    }




   

    




}
