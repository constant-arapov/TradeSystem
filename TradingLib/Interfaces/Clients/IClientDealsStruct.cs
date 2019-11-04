using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;


using Common;
using Common.Interfaces;

using TradingLib.GUI;
using TradingLib.Bots;
using TradingLib.Data.DB;


namespace TradingLib.Interfaces.Clients
{
    public interface IClientDealsStruct  : IAlarmable
    {

       bool AnalzyeTimeFrames { get; set; }
       CGlobalConfig GlobalConfig { get; }
       CGUIBox GUIBox { get; set; }
       DateTime ServerTime { get; set; }
       bool IsStockOnline { get; set; }
       bool IsDealsOnline { get; set; }
       ManualResetEvent EvDealsOnline { get; set; }

       List<CBotBase> ListBots { get; set; }
       CListInstruments Instruments { get; set; }

       string GetDataPath();
    }
}
