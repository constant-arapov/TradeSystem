using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Enums;

namespace TradingLib.Interfaces.Components
{
	public interface IStockConnector : ITradeOperations
	{
        void WaitConnectionClosed();
        void DisconnectFromServer();
        string Account { get; }
        string Password { get; }
        bool IsConnectedToServer { get; }

        
	}
}
