using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Common.Interfaces;

using TradingLib.Data.DB;

using ASTS.DealingServer;



namespace ASTS.Interfaces.Interactions
{
    public interface IDealingServerForASTSAllTrades : ILogable, IAlarmable
    {
        CDealBoxASTS DealBoxInp { get; }
        bool IsDealsOnline { get; set; }
        CListInstruments Instruments { get; set; }

        ManualResetEvent EvDealsOnline { get; set; }
    }
}
