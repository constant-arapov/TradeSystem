using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TradingLib.Interfaces.Components;


using Common.Interfaces;

namespace CryptoDealingServer.Interfaces
{
    public interface IClientTradeHistStorV2 : IAlarmable
    {
        long GetBotIdBfxRest(long id);
        IDBCommunicator DBCommunicator { get; set; }
    }
}
