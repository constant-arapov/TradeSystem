using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.Concurrent;
using System.Threading;

using System.Diagnostics;



namespace Common.Collections
{

    //note this not working when dictionary of CBlockingQueue


    public class CBlockingQueueWithPriority<T> : List<T>
    {

        //private Mutex _mx = new Mutex();

        private object _locker = new object();


        private int _addCnt = 0;


        public new void Add(T el)
        {
            _addCnt++;


            bool bWasSet = false;

            //_mx.WaitOne();
            lock (_locker)
            {

                base.Add(el);


                /*  if (this.Count >= 1)
                  {
                   //   _ev.Set();
                      bWasSet = true;
                  }*/

            }
            //  _mx.ReleaseMutex();


            int tmp = 1;

        }




        public T GetElementBlocking()
        {




            T el = default(T);



            //_ev.WaitOne();
            //_ev.Wait();



            WaitTillQueueIsNotEmpty();



            //     _mx.WaitOne();

            lock (_locker)
            {

                try
                {
                    //   if (this.Count > 0)                  
                    {
                        el = this.First();
                        this.RemoveAt(0);
                    }


                    //    if (this.Count == 0)
                    //  _ev.Reset();


                }
                catch (Exception e)
                {

                    throw e;
                }
            }
            //   _mx.ReleaseMutex();




            return el;

        }


        private void WaitTillQueueIsNotEmpty()
        {
            while (this.Count == 0)
                Thread.Sleep(50);

        }

    }





}
