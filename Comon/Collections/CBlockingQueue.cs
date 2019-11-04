using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.Concurrent;
using System.Threading;

using System.Diagnostics;

using Common.Interfaces;

namespace Common.Collections
{

    //note this not working when dictionary of CBlockingQueue


    public class CBlockingQueue<T> : List<T>, IQueue<T>
    {

      

        private object _locker = new object();


        private int _addCnt = 0;
        private DateTime _lastAlarm = new DateTime(0);

        private bool _isTerminated = false;



        /// <summary>
        /// If true queue termination starting.
        /// </summary>
        public bool IsTerminated
        {
            get
            {
                return _isTerminated;
            }
            set
            {
                _isTerminated = true;
            }



        }

    

        public new void Add(T el)
        {
            _addCnt++;
          

       
            lock (_locker)
            {              

                base.Add(el);

                                  
            }
       

        }



        public void CheckLimit(int countLim,   bool bOutAlarm =true, int secRepeatAlarm = 20, IAlarmable alarmer =null, string queueName="Unknown")
        {
            if (this.Count > countLim)
            {
                if ((DateTime.Now - _lastAlarm).TotalSeconds > secRepeatAlarm && bOutAlarm)
                {
                    if (alarmer !=null)
                        alarmer.Error(queueName + " quee more than max =" + this.Count);                 
                      _lastAlarm = DateTime.Now;
                }

            }

        }

        public T GetElementBlocking()
        {

          


            T el = default(T);



            //if waiting ends because of "IsTerminated" signal -
            //do return default value(null) else do
            //continue processing
            if (!WaitTillQueueIsNotEmpty())
                return el; // default value (usually null)

          

         

            lock (_locker)
            {

                try
                {
                    //   if (this.Count > 0)                  
                    {
                        el = this.First();                      
                        this.RemoveAt(0);
                    }
                  


                   
                }
                catch (Exception e)
                {

                    throw;
                }
            }
         

            return el;

        }
		//for IQueue <T> capability
		public T Get()
		{
			return GetElementBlocking();

		}



        /// <summary>
        /// Wait till QueuesIsNotEmpty or
        /// terminated. If queue become not empty returns true.
        /// If queue terminated returns false.
        /// </summary>
        /// <returns>
        /// </returns>
        private bool WaitTillQueueIsNotEmpty()
        {
            //2018-05-20 added termination condition
            while (this.Count == 0 && !IsTerminated)
                Thread.Sleep(1);

            if (IsTerminated)
                return false;
            else
                return true;


        }

    }





}
