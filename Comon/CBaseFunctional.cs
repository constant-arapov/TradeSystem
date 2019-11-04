using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;
using Common.Logger;

namespace Common
{
    public class CBaseFunctional : ILogable
    {
        protected ILogable _logger;
        protected IAlarmable _alarmer;
        protected string _name;

        public CBaseFunctional(IAlarmable alarmer, string name=null)
        {
           
           //string name;
           if (name == null)
               _name = this.GetType().Name;
           else
               _name = name;
           _logger = new CLogger(_name);
           _alarmer = alarmer;

           PrintStartUpBanner();
         

        }

        public CBaseFunctional(string name,IAlarmable alarmer, ILogable logger)
        {
            _alarmer = alarmer;
            _logger = logger;
            PrintStartUpBanner();
            _name = name;

        }

        private void PrintStartUpBanner()
        {
            Log("==========================================" + _name + " started ==================================================================================");
        }


        public void Log(string msg)
        {
            if (_logger !=null)
                _logger.Log(msg);

        }
        public void Error(string msg, Exception e=null)
        {
            Log("Error ! " + msg);

            string msgAlarm = String.Format(@"=={0}== {1}", _name, msg);

            if (_alarmer != null)
                _alarmer.Error(msgAlarm, e);

            
        }

         public void LogMethEntry(string methName)
        {
            Log(methName+" - Entry");
        }

         public void LogMethExit(string methName)
         {
             Log(methName + " - Exit");
         }

    }
}
