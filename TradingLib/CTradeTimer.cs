using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Timers;

using Common.Interfaces;
using TradingLib.BotEvents;
using TradingLib.Bots;


namespace TradingLib
{
    public class CTradeTimer : ILogable
    {
        //public bool IsExpired { get; set; }

        public bool IsStarted = false;
     

        string m_name;
        public bool IsStopped = true;
        Timer m_timer;

        CBotBase m_bot;
        EnmBotEventCode m_evBot;

        public CTradeTimer(string name, CBotBase bot, EnmBotEventCode evCode,  double expTimeMS)
        {
            m_timer = new Timer(expTimeMS);
            m_bot = bot;
            m_name = name;

            m_timer.Elapsed += new ElapsedEventHandler(OnTimerExpired);

            m_evBot = evCode;
            
        }

        public void Log(string message)
        {
            m_bot.Logger.Log(message);

        }

        public void OnTimerExpired(Object obj, EventArgs ev)
        {
            m_timer.Stop();
            Log("Timer " + m_name + " was expired");
            //IsExpired = true;
            IsStopped = true;
            IsStarted = false;
            

            m_bot.Recalc("", m_evBot, m_name);
             



        }

       

        public void Set()
        {
            m_timer.Start();
            //IsExpired = false;
            Log ("Timer " + m_name + " was started");
            IsStopped = false;
            IsStarted = true;

        }


        public void Reset()
        {
            m_timer.Stop();
            //IsExpired = false;
            Log("Timer " + m_name + " was stopped");
            IsStopped = false;
            IsStarted = false;

        }



    }
}
