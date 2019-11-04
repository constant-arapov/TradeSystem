using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

namespace TradeManager.Interfaces.Clients
{
    public interface IClientDataSource : IAlarmable
    {
        void SaveConfig();
        string GetStockExchName(int stockExchId);
        void FatalError(string message);
    }
}
