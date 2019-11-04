using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;



using Common;
using Common.Utils;

using TradingLib;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Interaction;
using TradingLib.Interfaces.Components;
using TradingLib.Common;
using TradingLib.Enums;
using TradingLib.Data.DB;
using TradingLib.Bots;

//using DBCommunicator;


//using Plaza2Connector.Interfaces;


namespace TradingLib
{
    public class CClearingProcessor : CBaseFunctional
    {

        IClearingProcessorClient _client;
        /*CDBCommunicator*//*IDBCommunicator*/
		IDBCommunicatorForClearingProcessor _dbCommunicator;
		
        /*CUserDealsPosBox*/IUserDealsPosBox _userDealPosBox;

        Dictionary<int, CDBAccountMoney> _accountsMoney { get; set; }
        public Dictionary<int, CDBAccountTrade> _accountsTrade { get; set; }

   

        public  CClearingProcessor (IClearingProcessorClient client)
            : base(client)
        {

            _client = client;
            _dbCommunicator = (IDBCommunicatorForClearingProcessor) client.DBCommunicator;
            _userDealPosBox = client.UserDealsPosBox;

            _accountsMoney = client.AccountsMoney;
            _accountsTrade = client.AccountsTrade;
            

         }



        /// <summary>
        /// Process automatic clearing/
        /// 
        /// Call from:
        /// CBaseSeccionBox.TaskCheckUnsavedSessionsAndClearing
        /// 
        /// </summary>
        public void ProcessAutomaticClearing()
        {
            try
            {

                LogMethEntry("ProcessAutomaticClearing");

                //Step 0 pre conditions - none writes to DB
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-us");
             

                int slInterval = 100;
                int numIntv = 10;
                int cnt = 0;
                while (true)
                {
                    if (!_dbCommunicator.IsQueueEmpty())
                        cnt = 0;
                    else
                        cnt++;

                    if (cnt > numIntv)
                        break;

                    Thread.Sleep(slInterval);
                }

                //Step 1 Select from DB not clearing  processed sessions
                Log("ProcessAutomaticClearing - Step1. Select from DB not clearing  processed sessions");
                var res = _dbCommunicator.GetSessionsNotClearingProcessed(_client.StockExchId);


                //Step 2 For each not clearing processed sessions Calculate VM for all of them
                //  
                Log("ProcessAutomaticClearing - Step2. For each not clearing processed sessions, select all userposlog not clearing processed");

                //Enumerate session from older ещ newer. For each session do process 
                //datablock - all not processed data till Date of session's end
                //
                foreach (var sess in  res)
                {

                    ProcessClearingDataBlock((long)sess["DtEnd_timestamp_ms"], (int)sess["id"], 1, (int) _client.StockExchId);
                    _dbCommunicator.SetClearingProcessSession((int)sess["id"]);

                }

                //_client.UpdateMoneyData();

                //2016-09-14 for update clients trade log data after clearing
                _client.LoadDataFromDB();

                LogMethExit("ProcessAutomaticClearing");
            }
            catch (Exception e)
            {
                Error("TaskProcessAutomaticClearing", e);

            }
        }


        /// <summary>
        /// Load all poslog data till DtEnd_timestamp_ms (end of session date for
        /// automatic clearing).
        /// 
        /// Call from:
        /// 
        /// this.ProcessAutomaticClearing
        /// this.ProcessManualClearing
        /// 
        /// </summary>
        /// <param name="DtEnd_timestamp_ms">Date untill do processing (end of session deate)</param>
        /// <param name="sessionId"></param>
        /// <param name="clearingMode"></param>
        /// <param name="stockExchId"></param>
        public void ProcessClearingDataBlock(long DtEnd_timestamp_ms, int sessionId, int clearingMode,
                                               int stockExchId)
        {

            LogMethEntry("ProcessClearingDataBlock");

            //retrieve all not clearing processed poslogdata earlier than DtEnd_timestamp_ms
            var lstPos = _dbCommunicator.LoadNotClearingProcessedTradeDataLogTimeFilt("poslog", "DtClose_timestamp_ms",
                                                                                       DtEnd_timestamp_ms,
                                                                                       stockExchId
                                                                                     );

            // Get Log with closed positions of bots
            Dictionary<int, Dictionary<string, List<CBotPos>>> dicBotPosLog =
                                    new Dictionary<int, Dictionary<string, List<CBotPos>>>();

            Log("ProcessClearingDataBlock - LoadTradeDataLog. Get multidemension structures botId - instrument - userposlog ");
            _dbCommunicator.LoadTradeDataLog(lstPos, dicBotPosLog);
           


            Dictionary<int, CDBClearingCalcedVM> dictDBClearingCalcedVM = new Dictionary<int, CDBClearingCalcedVM>();
            Dictionary<int, CDBPayout> dictDBPayout = new Dictionary<int, CDBPayout>();

            //changed 2017-10-24
          /*  decimal oplogMoney_BeforeCalcVM =0;
            decimal opLogMoney_ChangeCalcVM = 0;
            decimal opLogMoney_AfterCalcVM =0 ;


            decimal oplogMoney_BeforePayout =0;
            decimal opLogMoney_ChangePayout =0 ;
            decimal opLogMoney_AfterPayout = 0;
            */

            Dictionary<int,decimal> oplogMoney_BeforeCalcVM = new Dictionary<int,decimal>();
            Dictionary<int,decimal> opLogMoney_ChangeCalcVM = new Dictionary<int,decimal>();

            Dictionary<int,decimal> opLogMoney_AfterCalcVM = new Dictionary<int,decimal>();
            
            Dictionary<int,decimal> oplogMoney_BeforePayout = new Dictionary<int,decimal>();
            Dictionary<int,decimal> opLogMoney_ChangePayout = new Dictionary<int,decimal>();
            Dictionary<int, decimal> opLogMoney_AfterPayout = new Dictionary<int, decimal>();
          
             




            //prepare structs for insert
            //to clearing_calced_vm table
            Log("ProcessClearingDataBlock. Calculate VM and prepare structs for insert");

            foreach (var kvp in dicBotPosLog)
            {
                int id = kvp.Key;

                CDBAccountMoney accountMoney = new CDBAccountMoney();
              
              //  _accountsMoney.TryGetValue(id,out accountMoney);
                accountMoney = _accountsMoney[id];



                //prepare struct ClearingCalcedVM
                if (!dictDBClearingCalcedVM.ContainsKey(id))
                    dictDBClearingCalcedVM[id] = new CDBClearingCalcedVM
                    {
                        DtCalced = _client.ServerTime,
                        account_trade_Id = id, //note account_trade_id is realy account_trade.account_money_id
                        session_id = sessionId,
                        ClearingMode = clearingMode,
                        stock_exchange_id = (int)_client.StockExchId,
                        Money_before_calc = accountMoney.money_avail

                    };

                foreach (var kvp2 in kvp.Value) //all instruments
                    foreach (var v in kvp2.Value) //all poslogs
                    {
                        dictDBClearingCalcedVM[id].VM_RUB += v.VMClosed_RUB;
                        dictDBClearingCalcedVM[id].VM_RUB_clean += v.VMClosed_RUB_clean;

                    }

                

                dictDBClearingCalcedVM[id].VM_RUB_user = dictDBClearingCalcedVM[id].VM_RUB;
                opLogMoney_ChangeCalcVM[id] = dictDBClearingCalcedVM[id].VM_RUB_user;

                dictDBPayout[id] = new CDBPayout { Dt = _client.ServerTime };//create payout profile


                ///if profit positive do calculate payout
                if (dictDBClearingCalcedVM[id].VM_RUB_user > 0) 
                {
                //    dictDBClearingCalcedVM[id].VM_RUB_user *= (100 - _accountsTrade[id].proc_profit) / 100;
                    dictDBPayout[id].payout = -_accountsTrade[id].proc_profit/100 * dictDBClearingCalcedVM[id].VM_RUB_user;
                }
                
                dictDBClearingCalcedVM[id].Money_after_calc = dictDBClearingCalcedVM[id].Money_before_calc + dictDBClearingCalcedVM[id].VM_RUB_user;
               
                
                //--- prepatre data for accountOperationsLog
               oplogMoney_BeforeCalcVM[id] = accountMoney.money_avail;
               opLogMoney_ChangeCalcVM[id] = dictDBClearingCalcedVM[id].VM_RUB_user;
               opLogMoney_AfterCalcVM[id] = dictDBClearingCalcedVM[id].Money_after_calc;

               oplogMoney_BeforePayout[id] = opLogMoney_AfterCalcVM[id];
               opLogMoney_ChangePayout[id] = dictDBPayout[id].payout;
               opLogMoney_AfterPayout[id] = oplogMoney_BeforePayout[id] + opLogMoney_ChangePayout[id];
               //---
            }





            foreach (var kvp in dictDBClearingCalcedVM)
            {
                int userId = kvp.Key;

                //Step 3 Insert into clearing calced_vm calculated values. 
                Log("ProcessClearingDataBlock - Step 3. Insert into clearing calced_vm calculated values");
                int idOpUpdateCalcedVM = (int)_dbCommunicator.UpdateCalcedVM(kvp.Value);

                //Step 3.1 Insert into payouts values
                dictDBPayout[userId].clearing_calced_vm_id = idOpUpdateCalcedVM;
                int idOpUpdatePayOut =  (int)_dbCommunicator.InsertPayout(dictDBPayout[userId]);


                //Step 4 Add to trading account and money account calculated values

                Log("ProcessClearingDataBlock - Step 4. Add to trading account and money account calculated values");
                UpdateAccountMoney(kvp.Key, dictDBClearingCalcedVM[kvp.Key].VM_RUB_user, dictDBPayout[userId].payout);

                //Step 5 Mark trade data as clearing processed and update vmId
                Log("ProcessClearingDataBlock - Step 5. Mark trade data as clearing processed and update vmId");
                _dbCommunicator.SetClearingProcessedTradeData(DtEnd_timestamp_ms, userId, stockExchId, idOpUpdateCalcedVM);


                //Step 6 add record to accoubt operations log
                Log("ProcessClearingDataBlock - Step 6. Add record to account operations log");

                //insert ot accounts operation log calculated VM
                _dbCommunicator.InsertAccountOperationsLog(new CDBAccountsOperationsLog
                {
                    account_operation_name_id = GetStockalcVMOperationId(_client.StockExchId),                                   
                    Dt_operation =  _client.ServerTime,
                    account_trade_id = dictDBClearingCalcedVM[kvp.Key].account_trade_Id,
                    account_operation_id = idOpUpdateCalcedVM,
                    Money_before = oplogMoney_BeforeCalcVM[userId],
                    Money_after = opLogMoney_AfterCalcVM[userId],
                    Money_changed = opLogMoney_ChangeCalcVM[userId]
                }



                                                                   );

                //insert to accounts operation log comission
                _dbCommunicator.InsertAccountOperationsLog(new CDBAccountsOperationsLog
                {
                    account_operation_name_id  = (int)EnmAccountOperations._11_ProffitComission,
                    Dt_operation = _client.ServerTime.AddSeconds(1), //special hack cause current ver of MySQL has no millisec resolution
                    account_trade_id = dictDBClearingCalcedVM[kvp.Key].account_trade_Id,
                    account_operation_id = idOpUpdatePayOut,
                    Money_before = oplogMoney_BeforePayout[userId],
                    Money_after = opLogMoney_AfterPayout[userId],
                    Money_changed = opLogMoney_ChangePayout[userId]


                }
                );


            }

            LogMethExit("ProcessClearingDataBlock");

        }

        private int GetStockalcVMOperationId(int codeStockExch)
        {


            if (codeStockExch == CodesStockExch._01_MoexFORTS)
                return (int)EnmAccountOperations._01_CalcVM_FORTS;

            else if (codeStockExch == CodesStockExch._02_MoexSPOT)
                return (int)EnmAccountOperations._02_CalcVM_MOEX;

            else if (codeStockExch == CodesStockExch._03_MoexCurrency)
                return (int)EnmAccountOperations._03_CalcVM_Currency;

            else if (codeStockExch == CodesStockExch._04_CryptoBitfinex)
                return (int)EnmAccountOperations._04_CalcVM_Bitfinex;

            else return 0;
                
           


        }




        public void CleanPosAndDeals()
        {
            _userDealPosBox.CleanUserPosLog();


        }



        public void ProcessManualClearing()
        {
            LogMethEntry("ProcessManualClearing");
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-us");

            ProcessClearingDataBlock(CUtilTime.GetUnixTimestampMillis(_client.ServerTime), 0, 2, (int)_client.StockExchId);

            _client.UpdateMoneyData();
            CleanPosAndDeals();

            LogMethExit("ProcessManualClearing");
        }



        public void UpdateAccountMoney(int id, decimal VM_RUB_user, decimal payout)
        {

            LogMethEntry("UpdateAccountMoney");

            _accountsMoney[id].money_avail += VM_RUB_user;
            _accountsMoney[id].money_avail += payout;
            //2017-11-10 calculate trade account independent
            _accountsTrade[id].money_avail = _accountsMoney[id].money_avail * _accountsTrade[id].leverage;


            //_


            _dbCommunicator.UpdateMoney(_accountsMoney[id], "accounts_money", "id");
            _dbCommunicator.UpdateMoney(_accountsTrade[id], "accounts_trade", "accounts_money_id", 
                                            _accountsTrade[id].stock_exchange_id);

            LogMethExit("UpdateAccountMoney");

        }







    }
}
