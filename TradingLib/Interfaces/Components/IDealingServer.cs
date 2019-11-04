using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Interfaces;

using Messenger;

using TradingLib.Abstract;
using TradingLib.Bots;
using TradingLib.GUI;
using TradingLib.Data;
using TradingLib.Data.DB;
using TradingLib.ProtoTradingStructs;
using TradingLib.ProtoTradingStructs.TradeManager;
using TradingLib.TradersDispatcher;


namespace TradingLib.Interfaces.Components
{
	public interface IDealingServer : ITradeOperations
	{

        bool IsSessionActive { get; set; }
        bool IsStockOnline { get; set; }
		bool IsAllInstrAllMarketsAvailable { get; }
        bool IsOnlineUserDeals { get; set; }
        bool IsOnlineUserOrderLog { get; set; }
        bool IsPositionOnline { get; set; }
        bool IsOrderControlAvailable { get; set; }
        bool IsFutInfoOnline { get; set; }
        bool IsSessionOnline { get; set; }
        bool IsDealsOnline { get; set; }
        bool IsTimeSynchronized { get; set; }
        bool IsAnalyzerTFOnline { get; set; }
        bool AnalzyeTimeFrames { get; set; }
        bool IsOnlineVM { get; set; }
		bool IsAllBotLoaded { get; set; }


        bool IsSimulateMode { get; }


        DateTime ServerTime { get; set; }
        int StockExchId { get; set; }

		bool UseRealServer { get; }
		Dictionary<long, CBotBase> DictBots { get; set; }
		IUserDealsPosBox
		/*CBaseUserDealsPosBox*/ UserDealsPosBox { get; }
        ISessionBox SessionBox { get; }
        IStockBox StockBox { get;  }
        IPositionBox PositionBox { get; }
        ISnapshoterStock SnapshoterStock { get; set; }
        IUserOrderBox UserOrderBox { get; }
        IDealBox DealBox { get; }
        IDBCommunicator DBCommunicator { get; set; }
        CTradersDispatcher TradersDispatcher { get;  }
		IReportDispatcher ReportDispatcher { get; set; }
		IMessenger Messenger { get; set; }


        CGUIBox GUIBox { get; set; }
                         
        CListInstruments Instruments { get; set; }

        CSounder Sounder { get; set; }
        
        CAlarmer Alarmer { get; set; }

        Dictionary<string, decimal> DictVM { get; set; }
        Dictionary<int, CDBAccountMoney> AccountsMoney { get; set; }
        Dictionary<int, CDBAccountTrade> AccountsTrade { get; set; }
        Dictionary<string, CFutLims> DictFutLims { get; set; }
                  		
        Dictionary<string, CInstrument> DictInstruments { get; set; }

        List<CBotBase> ListBots { get; set; }

        IGUIBot CreateGUIBot(IDealingServer dealingServer, long botId);
        CGlobalConfig GlobalConfig { get; }
        string GetDataPath();

        CBotBase GetBotById(long id);
        bool IsPriceInLimits(string isin, decimal price);


        bool IsPossibleEmptyInstrCancellOrders { get; set; }
        decimal GetMaxPrice(string instrument);
        decimal GetMinPrice(string instrument);

        decimal GetMinOrderSize(string instrument);



        bool IsPossibleToAddOrder(string instrument);


        bool IsReadyForRecalcBots();
        void WaitBotConfigLoaded();

        List<Dictionary<string, object>> LstBotsConfig { get; }


        List<Dictionary<string, object>> LstBotsInstrumentsConfig { get; }

        bool IsExistBotIdInstrument(int botId, string instrument);

        void UpdateBotStatusTrdMgr(CBotStatus botStatus);
        void UpdateBotPosTrdMgr(int botId, CBotPos botPos);

        bool IsActualSessionNumber(int sessNum);
        int  NumOfStepsForMarketOrder { get; }
        decimal GetAccountTradeMoney(int botId);

        bool IsPossibleNativeCancellOrdByInstr { get; set; }
        decimal GetOrdersBacking(string instrument, decimal price, decimal amount);
        void UpdDBVMOpenedClosedTot(int accountId, decimal vmAllInstrOpenedAndClosed);
        void UpdDBPosInstr(int stockExchId, string instrument, decimal amount, decimal avPos);


    }
}
