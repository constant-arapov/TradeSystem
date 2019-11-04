using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Threading;



namespace Common.Collections
{
    public class CBlockingQueueForStructures<T> : List<T>
    {

        //private Mutex _mx = new Mutex();

        private object _locker = new object();


          private ManualResetEventSlim _ev = new ManualResetEventSlim(false);



        private int _addCnt = 0;


        public new void Add(T el)
        {
            _addCnt++;
          

            bool bWasSet = false;

            //_mx.WaitOne();


          
                lock (_locker)
                {
                    base.Add(el);

               
                      if (this.Count >= 1)
                      {
                          _ev.Set();
                       //   bWasSet = true;
                      }
                  

            }
            //  _mx.ReleaseMutex();

         
            int tmp = 1;

        }





        public T GetElementBlocking()
        {

        


            T el = default(T);

        

            //_ev.WaitOne();
            //_ev.Wait();


           
          //  WaitTillQueueIsNotEmpty();



            //     _mx.WaitOne();

           

                try
                {

                 _ev.Wait();
                    lock (_locker)
                     {
                     //   if (this.Count > 0)                  
                        {
                            el = this.First();
                      

                            this.RemoveAt(0);

                            if (this.Count == 0)
                                _ev.Reset();


                          
                        }
                    }
   

                   
                }
                catch (Exception e)
                {

                    throw;
                }
            
            //   _mx.ReleaseMutex();

        


            return el;

        }


        private void WaitTillQueueIsNotEmpty()
        {
            while (this.Count == 0)
                Thread.Sleep(1);

        }

    }
}
