using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Timers;


namespace Common
{
    public class CTimer
    {
        public bool IsExpired { get; set; }
        public bool IsStopped = true;

        Timer m_timer;

        Action m_actionOnExpiration;

        public CTimer(double expTimeMS, Action Act, bool startOnCreation =false  )
        {

            m_timer = new Timer(expTimeMS);

            m_timer.Elapsed += new ElapsedEventHandler(OnTimerExpired);
            m_actionOnExpiration = Act;

            if (startOnCreation)
                this.Set();


        }



        public void OnTimerExpired(Object obj, EventArgs ev)
        {
            m_timer.Stop();
           
            IsExpired = true;
            IsStopped = true;

            m_actionOnExpiration.Invoke();

            m_timer.Start();


        }




        public void Set()
        {
            m_timer.Start();
            IsExpired = false;
            
            IsStopped = false;


        }


        public void Reset()
        {
            m_timer.Stop();
            IsExpired = false;
          
            IsStopped = false;


        }











    }
}
