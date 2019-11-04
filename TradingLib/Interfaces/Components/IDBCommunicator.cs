using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Collections;

using TradingLib.Interfaces.Interaction;
using TradingLib.Data.DB;
using TradingLib.Data.DB.Interfaces;
using TradingLib.Common;

namespace TradingLib.Interfaces.Components
{
    public interface IDBCommunicator : IDBCommunicatorForReportDispatcher
    {


        List<Dictionary<string, object>> LoadAccountsMoney();
        List<Dictionary<string, object>> LoadAccountsTrade(int StockExchId);

        string LoadStockExchPassword(int stock_exch_id, string login);



        void LoadMoneyDataGeneric<T>(List<Dictionary<string, object>> queryRes, Dictionary<int, T> dictAcc) where T : IAccountMoney, new();

        void WaitDatabaseConnected();
        void WaitReadyForOperations();
        void QueueData(object data);

        void LoadUserPosLogData<T>(CDict_L2_List<int, string, T> dataLog, int stockExchId) where T : new();
        void LoadUserDealsLogData<T>(CDict_L2_List<int, string, T> dataLog, int stockExchId) where T : new();

        TimeSpan LoadTimeTradeDisable(int stockExchId);


        /*void LoadLatestTradeData(List<Dictionary<string, object>> queryRes, string dtColName,
											CDict_L2<int, string, CLatestTradeData> data);

		*/
        List<Dictionary<string, object>> LoadLatestUserTradeData(string storedProcedure, int stockExchId);
        /*List<Dictionary<string, object>> GetLatestCalcedVmData(int stockExchID);
		void UpdateReportSent(int vmCalcId);

		List<Dictionary<string, object>> GetPoslogClearingCalsSummary(int vmCalcId);
		List<Dictionary<string, object>> GetPoslogClearingCalsInstrumentsSummary(int vmCalcId);
		List<Dictionary<string, object>> GetPoslogClearingCalsInstruments(int vmCalcId);
		List<Dictionary<string, object>> GetCurrentMonthOperations(int accountTradeId, DateTime dt);*/
        List<CDBInstrument> GetInstuments(int stockExchId);

        bool LoginRequest(string user, string password);
        void SetCompletedSessions(List<int> needSetCompletedSession);

        List<Dictionary<string, object>> GetUnCompletedSessions(int stockExchID);

        void InsertUnsavedSessionP2(CDBSession dbSession);
        void InsertUnsavedSessionASTS(CDBSession dbSession);

        void InsertUnsavedSessionCrypto(CDBSession dbSession);

        void SaveNewPassword(int stock_exch_id, string login, string newPassword);


        List<Dictionary<string, object>> LoadBotsConfig(int stockExchId);
        List<Dictionary<string, object>> LoadBotInstrumentConfig(int stockExchId);


        bool LoginRequestTradeManager(string user, string password);

        void TransactAddInstrument(string instrument, int codeStockExchId);


        void UpdateCryptoInstrumentData(string instrument, int codeStockExchId, decimal minimumOrderSize, int decimalVolume);

        void UpdateRoundTo(string instrument, int stockExchId, int newRoundTo);

        void UpdateMinStep(string instrument, int stockExchId, decimal newMinStep);
        List<Dictionary<string, object>> GetSessionsNotClearingProcessed(int stockExchId);

        DateTime DtLastExcute { get; }

        void DummySelect();


        void UpdateWalletLog(DateTime dt, string walletType, string currency, decimal balance);

        List<Dictionary<string, object>> GetWalletEndPrevDay();

        List<CDBBfxOrder> GetBfxOrdersHistory(string stDtFrom);
        List<CDBBfxTrades> GetBfxTradesHistory(string stDtFrom);
        List<CDBTurnOver> GetTradersTurnover(DateTime dtFrom);

        void UpdateTradersTurnover(List<CDBTurnOver> lstDbTurnover);

        List<CDBTurnoverFee> LoadTurnoverFeesCoef();
        void UpdateTradersFeeProc(List<CDBTurnoverFee> lstDbTurnOverFee);

    }
        
}
