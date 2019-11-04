using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Threading;

namespace Common.Interfaces
{
     public interface IAlarmable
    {    
      void Error(string description, Exception exception = null);
     // void GUIdispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e);
    }
     
}
