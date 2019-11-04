using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Interfaces;

using Messenger;

using TradingLib.Interfaces.Components;
using TradingLib.Data;
using TradingLib.Data.DB;
using TradingLib.ProtoTradingStructs;
using TradingLib.Bots;


namespace TradingLib.Interfaces.Clients
{
    public interface IClientTradersDispatcher
    {
        bool IsStockOnline { get; set; }
        bool IsDealsOnline { get; set; }

        IMessenger Messenger { get; set; }
        IDBCommunicator DBCommunicator { get; set; }
        IDealBox DealBox { get;}
        CGlobalConfig GlobalConfig { get;}
        CListInstruments Instruments { get; set; }
        IUserDealsPosBox UserDealsPosBox { get; }
        Dictionary<string, CInstrument> DictInstruments { get; set; }
        Dictionary<int, CDBAccountMoney> AccountsMoney { get; set; }
        ISnapshoterStock SnapshoterStock { get; set; }
        List<CBotBase> ListBots { get; set; }
        Dictionary<int, CDBAccountTrade> AccountsTrade { get; set; }
		decimal GetSessionLimit(int botId);
        bool IsExistBotIdInstrument(int botId, string instrument);
        int StockExchId { get; set; }
        void AddClientInfo(CClientInfo clientInfo);
        void DeleteClientInfo(int conId);
        List<int> GetPricePrecisions();

    }
}
