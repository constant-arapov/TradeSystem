using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Common;
using Common.Interfaces;

using TradingLib.Interfaces.Components;
using TradingLib.Data.DB;

namespace TradingLib.Interfaces.Clients
{
    public interface IClientInstrumentGrid : IAlarmable
    {
         IDealBox DealBox { get; }
         IStockBox StockBox { get; }
         IPositionBox PositionBox { get; }
         bool IsStockAvailable(string instrument);
         ManualResetEvent EvStockOnline { get; set; }
         ManualResetEvent EvDealsOnline { get; set; }
         ManualResetEvent EvPosOnline { get; set; }
         bool IsStockOnline { get; set; }
         CGlobalConfig GlobalConfig { get; }
         CListInstruments Instruments { get; set; }
         bool IsGlobalConfigAvail { get; }
         bool NeedHistoricalDeals { get; set; } 
    }
}
