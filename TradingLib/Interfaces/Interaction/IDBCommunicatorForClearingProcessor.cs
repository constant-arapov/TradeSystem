using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Data.DB;
using TradingLib.Data.DB.Interfaces;



namespace TradingLib.Interfaces.Interaction
{
	public interface IDBCommunicatorForClearingProcessor
	{
		bool IsQueueEmpty();
		List<Dictionary<string, object>> GetSessionsNotClearingProcessed(int stockExchID);
		void SetClearingProcessSession(int sesionId);
		List<Dictionary<string, object>> LoadNotClearingProcessedTradeDataLogTimeFilt(string tableName, string dtCol, long dtVal, int stockExchId);
		void LoadTradeDataLog<T>(List<Dictionary<string, object>> queryRes,
									Dictionary<int, Dictionary<string, List<T>>> data) where T : new();


		long UpdateCalcedVM(CDBClearingCalcedVM dbClearingCalcedVM);
		void SetClearingProcessedTradeData(long dtVal, int account_id, int stock_exch_Id, int calcVMId);
		void InsertAccountOperationsLog(CDBAccountsOperationsLog dbOperationsLog);
		long InsertPayout(CDBPayout dbPayout);
		void UpdateMoney(IAccountMoney dbAccMoney, string tableName, string stId, int stock_exchange_id = 0);

	}
}
