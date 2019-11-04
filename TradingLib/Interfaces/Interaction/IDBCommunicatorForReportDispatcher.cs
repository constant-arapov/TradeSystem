using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Interfaces.Interaction
{
    public interface IDBCommunicatorForReportDispatcher
    {

        List<Dictionary<string, object>> GetLatestCalcedVmData(int stockExchID);
        List<Dictionary<string, object>> GetCurrentMonthOperations(int accountTradeId, DateTime dt);
        List<Dictionary<string, object>> GetPoslogClearingCalsInstrumentsSummary(int vmCalcId);
        List<Dictionary<string, object>> GetPoslogClearingCalsInstruments(int vmCalcId);
        List<Dictionary<string, object>> GetPoslogClearingCalsSummary(int vmCalcId);
        void UpdateReportSent(int vmCalcId);
        List<Dictionary<string, object>> GetMinMaxAccountOp(DateTime dt);
        List<Dictionary<string, object>> GetSessionsBossReportNotSent(int sessionStockExchId);
        List<Dictionary<string, object>> GetBossReportList();
        void UpdateReportBossSent(int sessionId);
		List<Dictionary<string, object>> GetAllAccountsMoneyCurrent();
        List<Dictionary<string, object>> GetAllAccountsSumBySession(int sessionId);

        List<Dictionary<string, object>> GetWalletChange();

    }
}
