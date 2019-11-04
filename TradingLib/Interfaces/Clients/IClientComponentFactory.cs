using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Interfaces;

using TradingLib.Data.DB;
using TradingLib.Interfaces.Components;


namespace TradingLib.Interfaces.Clients
{
    public interface IClientComponentFactory : IClientDBCommunicator, IClientInstruments, IClientReportDispatcher, IAlarmable
    {
       CListInstruments Instruments { get; set; }
       IReportDispatcher ReportDispatcher { get; set; }
       IMessenger Messenger { get; set; }

      

       CGlobalConfig GlobalConfig { get;  }
       



    }
}
