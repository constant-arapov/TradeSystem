using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Common.Interfaces;


using TradingLib.Interfaces.Components;

namespace CryptoDealingServer.Interfaces
{
    public interface IClientMoneyTracker : IAlarmable
    {
        IDBCommunicator DBCommunicator { get; }
        bool IsAllUserPosClosed();
        bool IsAllStockExchClosed();

        decimal MoneyCurrentStockExch { get; }
        DateTime DtLastWalletUpdate { get; }
        decimal GetSessionProfit();
    }
}
