using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Components;
using TradingLib.Enums;
using TradingLib.Data.DB;


//using DBCommunicator;
//using DBCommunicator.DBData;
using Common.Interfaces;


namespace TradingLib.Interfaces.Clients
{
    public interface IClearingProcessorClient : IAlarmable
    {

        IDBCommunicator DBCommunicator { get; set; }
		IUserDealsPosBox UserDealsPosBox { get; }
        DateTime ServerTime { get; set; }
        Dictionary<int, CDBAccountMoney> AccountsMoney { get; set; }
        Dictionary<int, CDBAccountTrade> AccountsTrade { get; set; }
        int StockExchId { get; set; }

        void UpdateMoneyData();

        void LoadDataFromDB();


    }
}
