using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib;
using TradingLib.Interfaces.Clients;
using TradingLib.Enums;

namespace BitfinexWebSockConnector.Interfaces
{
    public interface IClientBfxStockStor : IAlarmable, IClientSharedStock
    {
        void UpdateCleintStockBothDir(string instrument, int precision, CSharedStocks stock);
        void UpdateClientStockOneDir(string instrument, Direction dir,int precision,
                                        CSharedStocks stock);
    }
}
