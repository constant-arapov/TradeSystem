using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Interfaces;
using Common.Logger;

namespace ASTS
{
    public class StubAlarmer : IAlarmable
    {


       private ILogable _logger;

       public StubAlarmer()
       {
           _logger = new CLogger("Errors");

       }

       public void Error(string description, Exception exception = null)
       {
           string msg = description;
           if (exception !=null)
            msg += " " + exception.Message + " " + exception.StackTrace;
           _logger.Log(msg);
       }

    }
}
