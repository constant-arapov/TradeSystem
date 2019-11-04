using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using Messenger;

using TradingLib.Interfaces.Components;
using TradingLib.Abstract;

using TradingLib.Bots;

namespace TradingLib.Interfaces.Clients
{
	public interface IClientTradeManagerServer : IAlarmable
	{
        IMessenger Messenger { get; set; }
        IDBCommunicator DBCommunicator { get; set; }
        CBasePosBox PosBoxBase { get; }
        int StockExchId { get; set; }
        int PortTradeManager { get; }
		void DisableBot(long id);
		void EnableBot(long id);
        CBotBase GetBotById(long id);

        void OnTrdMgrSentReconnect(int channelId);
      
	}
}
