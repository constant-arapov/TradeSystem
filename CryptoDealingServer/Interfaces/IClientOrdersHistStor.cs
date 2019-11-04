using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Interfaces;

using TradingLib.Interfaces.Components;

namespace CryptoDealingServer.Interfaces
{
    public interface IClientOrdersHistStor : IAlarmable
    {

       IDBCommunicator DBCommunicator { get; set; }
    }
}
