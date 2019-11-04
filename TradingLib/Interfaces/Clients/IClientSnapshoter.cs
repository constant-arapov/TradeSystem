using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Interfaces;

using TradingLib.Data.DB;
using TradingLib.Bots;

namespace TradingLib.Interfaces.Clients
{
    public interface IClientSnapshoter : IAlarmable, IClientSharedStock
    {
        bool IsSessionOnline { get; }
        bool UseRealServer { get; }
        bool IsSessionActive { get; }
        bool IsStockOnline { get; }
                        


        CListInstruments Instruments { get; set; }
        CGlobalConfig GlobalConfig { get; }
		List<CBotBase> ListBots { get; set; }


        void UpdateTradersDeals(string instrument);
        void UpdateTradersStocks(string instrument);

       
    }
}
