using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common.Interfaces;


using TradingLib;
using TradingLib.Interfaces.Components;
using TradingLib.Data;
using TradingLib.Data.DB;
using TradingLib.BotEvents;
using TradingLib.Bots;
using TradingLib.ProtoTradingStructs;

//using Common.Interfaces;

//using DBCommunicator;
//using DBCommunicator.DBData;


namespace TradingLib.Interfaces.Clients
{
    public interface IClientUserDealsPosBox : IAlarmable
    {

        decimal USDRate { set; get; }

       

        decimal BrokerFeeCoef { get;  }
        decimal InternalFeeCoef { get;  }


        /*CDBCommunicator*/IDBCommunicator DBCommunicator { get; set; }


        void UpdateGUIDealCollection(CRawUserDeal rd);

        //bool IsOrderFromPrevSession(CRawUserDeal rd);
       // bool IsPossibleCalculateBotPos(CRawUserDeal rd);

        bool IsReadyRefreshBotPos();


        bool IsDealsPosLogLoadedFromDB { get; set; }


        bool IsStockOnline { get; set; }
		bool IsOnlineUserDeals { get; set; }

       void UpdateTradersPosLog (int extId);

       void UpdateDBPosLog(long userId, int stockExchId,string Instrument,
                                     CBotPos botPos);



       void UpdateDBUserDealsLog( CDBUserDeal userDeal);

       decimal GetBid(string ticker);
       decimal GetAsk(string ticker);


       decimal GetStepPrice(string ticker);

       decimal GetMinStep(string ticker);
       long GetLotSize(string instrument);

      // void GetFeeParams(out decimal brokerFeeCoef, out decimal internalFeeCoef);


       void TriggerRecalculateBot(int botId, string isin, EnmBotEventCode code, object data);

       void TriggerRecalcAllBots(EnmBotEventCode evnt, object data);

       string GetTicker(long id);

       void GUIBotUpdateMonitorPos(CBotPos bp, string isin, int botId);

       void GUIBotUpdatePosLog(CBotPos BotPos, int extId);

       CBotBase GetBotById(long id);
       int StockExchId { get; set; }

       bool IsInstrumentExist(string instrument);

	   int GetDecimalVolume(string instrument);

        void UpdDBPosInstr(int botId, string instrument, decimal amount, decimal avPos);
    }
}
