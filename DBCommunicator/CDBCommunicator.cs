using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Threading;
using System.Threading.Tasks;



using Common;
using Common.Interfaces;
using Common.Collections;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Common;
using TradingLib.Data.DB;
using TradingLib.Data.DB.Interfaces;
using TradingLib.Interfaces.Interaction;
using TradingLib.ProtoTradingStructs;

//using DBCommunicator.DBData;
using DBCommunicator.Interfaces;
using DBCommunicator.Builders;


namespace DBCommunicator
{
    public class CDBCommunicator : CBaseFunctional,
                                    IDBCommunicator, IClientDatabaseConnector, IDBCommunicatorForClearingProcessor, IDBCommunicatorForReportDispatcher
    {


        CMySQLConnector _MySQLConnector;

        IClientDBCommunicator _client;

        CBlockingQueue<object> _bqQueriesQueue = new CBlockingQueue<object>();
        private ManualResetEvent _evDatabaseConnected = new ManualResetEvent(false);
        private ManualResetEvent _evReadyForOperations = new ManualResetEvent(false);

        private string _dataBaseName;

        private bool _isDatabaseConnected;

        public bool IsDatabaseConnected
        {
            get
            {
                return _isDatabaseConnected;

            }
            set
            {
                _isDatabaseConnected = value;
                _client.IsDatabaseConnected = value;
                if (value)
                    _evDatabaseConnected.Set();
                else
                    _evDatabaseConnected.Reset();

            }


        }

        private bool _isDatabaseReadyForOperations;



        public bool IsDatabaseReadyForOperations
        {
            get
            {
                return _isDatabaseReadyForOperations;

            }
            set
            {
                _isDatabaseReadyForOperations = value;
                _client.IsDatabaseReadyForOperations = value;
                if (value)
                    _evReadyForOperations.Set();
                else
                    _evDatabaseConnected.Reset();

            }



        }

        public DateTime DtLastExcute
        {
            get
            {
                if (_MySQLConnector != null)
                    return _MySQLConnector.DtLastExecute;
                else
                    return DateTime.Now;

            }

        }





        //========================================================================================================================================================================================================================================================

        public CDBCommunicator(string dataBaseName, IClientDBCommunicator client)
            : base(client)

        {
            _dataBaseName = dataBaseName;

            _client = client;

            //TODO read from config

            //_evDatabaseConnected = new AutoResetEvent(false);


            (new Task(TaskCreate)).Start();


            (new Thread(ThreadProcessQueriesQueue)).Start();
        }


        public void WaitDatabaseConnected()
        {
            _evDatabaseConnected.WaitOne();
        }

        public void WaitReadyForOperations()
        {
            _evReadyForOperations.WaitOne();
        }


        public void TaskCreate()
        {
            _MySQLConnector = new CMySQLConnector("localhost", _dataBaseName, "root", "profinvest", _client, this);

            //  _MySQLConnector = new CMySQLConnector("localhost", "atfs_production_2017_10_24", "root", "profinvest", _client, this);


            _MySQLConnector.Connect();
            _MySQLConnector.GenTablesSchemas();

            //   _MySQLConnector.Connect();           

        }


        public bool IsQueueEmpty()
        {
            //TODO chek if this Thread safe

            if (_bqQueriesQueue.Count > 0)
                return false;

            return true;
        }


        private void ThreadProcessQueriesQueue()
        {

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-us");
            //  Thread.CurrentThread.CurrentCulture.DateTimeFormat = System.Globalization.DateTimeFormatInfo.InvariantInfo; 
            while (true)
            {

                try
                {

                    object ob = _bqQueriesQueue.GetElementBlocking();
                    // string name = ob.GetType().Name;
                    Type type = ob.GetType();

                    if (type == typeof(CDBUserPosLog))
                        UpdateUserTradeDataLog(ob, "poslog");
                    else if (type == typeof(CDBUserDeal))
                        UpdateUserTradeDataLog(ob, "userdealslog");
                    else if (type == typeof(CDBSession))
                        UpdateSession(ob);
                    else if (type == typeof(CDBInstrument))
                        UpdateInstrument(ob);
                    else if (type == typeof(CDBUpdateLate))
                        UpdateFee(ob);
                    else if (type == typeof(CDBUpdateStepPrice))
                        UpdateStepPrice(ob);
                    else if (type == typeof(CDBUpdateWallet))
                        UpdateWalletLog(ob);
                    else if (type == typeof(CDBUpdateFeeUserDealsLog))
                        UpdateFeeUserDealsLog(ob);
                    else if (type == typeof(CDBMoneyTracking))
                        UpdateMoneyTracking(ob);
                    else if (type == typeof(CDBBindDealBotPos))
                        UpdateBindDbData(ob);
                    else if (type == typeof(CDBBfxOrder))
                        InsertBfxOrdersHistory(ob);
                    else if (type == typeof(CDBBfxTrades))
                        InsertBfxTrades(ob);
                    else if (type == typeof(CDBUpdVMOpenedClosedTot))
                        UpdateOpenedClosedTotal(ob);
                    else if (type == typeof(CDBUpdPosInstr))
                        UpdDBPosInstr(ob);



                    // else if (type == typeof(CDBInsertBfxTrades))
                    //   InsertBfxTrades(ob);

                        //else if ( == "CDBLastClosedPos") UpdateLastClosedPos((CDBLastClosedPos)ob);

                }
                catch (Exception e)
                {
                    Error("ThreadProcessQueriesQueue", e);

                }


                //ob.Invoke();

            }

        }

        public void UpdateMoneyTracking(object objMoneyTracking)
        {

            //CDBMoneyTracking moneyTracking = (CDBMoneyTracking)objMoneyTracking;
            LogMethEntry("UpdateMoneyTracking");

            string tableName = "money_tracking";

            string sql = new CSQLInsertBuilder()
                                             ._1_InsertIntoTable(tableName)
                                             ._2_SetColumnObj(objMoneyTracking, _MySQLConnector.TablesSchemas[tableName])
                                             .Build();


            _MySQLConnector.ExecuteInsertOrUpdate(sql);

            LogMethExit("UpdateMoneyTracking");



        }



        public void DummySelect()
        {

            string sql = "select 0";
            _MySQLConnector.ExecuteSelect(sql);


        }



        public List<CDBExceptDay> LoadExcpHolidays(int year)
        {
            return LoadExcp("except_holidays", year);
        }

        public List<CDBExceptDay> LoadExcpDayOff(int year)
        {

            return LoadExcp("except_dayoff", year);

        }






        private List<CDBExceptDay> LoadExcp(string table, int year)
        {
            string whereCriteria = String.Format("DT<{0} and DT>={1}",
                                                   CMySQLConv.ToMySQLFormat(new DateTime(year + 1, 1, 1)),
                                                    CMySQLConv.ToMySQLFormat(new DateTime(year, 1, 1))
                                                    );


            string sqlExc = new CSQLSelectBuilder()
                               ._2_FromTable(table)
                               ._3_Where(whereCriteria)
                               .Build();


            return _MySQLConnector.ExecuteSelectObject<CDBExceptDay>(sqlExc);




        }

        public void TransactAddInstrument(string instrument, int codeStockExchId)
        {

            CMySQLProcedureBuilder builder = new CMySQLProcedureBuilder("transact_add_instrument", _MySQLConnector);

            int out_result = 0;
            string out_error_message = "";


            var res = builder.Add("in_intrument", instrument)
                     .Add("in_stock_exch_id", codeStockExchId)
                     .Add("out_result", out_result, ParameterDirection.Output)
                     .Add("out_error_message", out_error_message, ParameterDirection.Output)
                     .Build();

        }




        public TimeSpan LoadTimeTradeDisable(int stockExchId)
        {
            string sql = new CSQLSelectBuilder()
                            ._2_FromTable("time_trade_disable")
                            ._3_Where("stock_exch_id=" + stockExchId)
                            .Build();

            var res = _MySQLConnector.ExecuteSelect(sql);
            if (res.Count != 1)
                throw new ApplicationException("CDBCommunicator.LoadTimeTradeDisable error ");

            return (TimeSpan)res[0]["time_disable"];


        }

        public string LoadStockExchPassword(int stock_exch_id, string login)
        {

            string where = String.Format(@" stock_exch_id={0} and login='{1}'", stock_exch_id, login);


            string sql = new CSQLSelectBuilder()
                            ._2_FromTable("stock_exch_accounts")
                            ._3_Where(where)
                            .Build();

            var res = _MySQLConnector.ExecuteSelect(sql);
            if (res.Count != 1)
                throw new ApplicationException("CDBCommunicator.LoadStockExchAccount");


            return res[0]["password"].ToString();

        }


        public void SaveNewPassword(int stock_exch_id, string login, string newPassword)
        {
            string where = String.Format(@" stock_exch_id={0} and login='{1}'", stock_exch_id, login);


            string sql = new CSQLUpdateBuilder()
                            ._1_UpdateTable("stock_exch_accounts")
                            ._3_Where(where)
                            ._2_SetColumnExpr("password", newPassword)
                            .Build();

            long cnt = _MySQLConnector.ExecuteInsertOrUpdate(sql);

        }





        public void QueueData(object data)
        {

            _bqQueriesQueue.Add(data);

        }

        public List<Dictionary<string, object>> GetCurrentMonthOperations(int accountTradeId, DateTime dt)
        {
            /*

            string sql = (new CSQLSelectBuilder())
                                   ._1_Select("Dt_operation, Money_changed, Money_before, Money_after,Operation_name")
                                   ._2_FromTable("accounts_operations_log, account_operations_names")  
                                                                           
                                   ._4_LinqTables("accounts_operations_log.account_operation_name_id=account_operations_names.id")
                                   //._3_Where("")
                                   .Build();
            return _MySQLConnector.ExecuteSelect(sql);
             */

            DateTime dtTo = dt.AddDays(1);

            string stDtBegin = String.Format("{0}-{1}-{2}",
                                             dt.Year.ToString("D4"),
                                             dt.Month.ToString("D2"), 1);

            string stDtEnd = String.Format("{0}-{1}-{2} {3}:{4}",
                                                dtTo.Year.ToString("D4"),
                                                dtTo.Month.ToString("D2"),
                                                dtTo.Day.ToString("D2"),
                                                dtTo.Hour.ToString("D2"),
                                                dtTo.Second.ToString("D2"));

            return _MySQLConnector.ExecuteProcedure("get_accounts_operations",
                                                    new Dictionary<string, object>
                                                    {
                                                        {"in_account_trade_id",accountTradeId},
                                                        {"in_Dt_Begin",  stDtBegin/*"2016-9-1"*/ },
                                                        {"in_Dt_End",    stDtEnd /*"2016-09-30"*/ }

                                                    });




        }

        public void LoadMoneyDataGeneric<T>(List<Dictionary<string, object>> queryRes, Dictionary<int, T> dictAcc) where T : IAccountMoney, new()
        {
            dictAcc.Clear();

            queryRes.ForEach(a =>
            {
                T dbMoney = new T();
                CMySQLConnector.FillClassFields(a, dbMoney);
                dictAcc[dbMoney.GetId()] = dbMoney;

            }
                                  );


        }

        public List<CDBTurnoverFee>  LoadTurnoverFeesCoef()
        {

            LogMethEntry("LoadTurnoverFeesCoef");


            string sql = new CSQLSelectBuilder()
                            ._1_Select("*")
                            ._2_FromTable("turnover_fees_coef")
                            .Build();



            var res = _MySQLConnector.ExecuteSelect(sql);

            List<CDBTurnoverFee> lst = new List<CDBTurnoverFee>();

            res.ForEach(el =>
           {
               CDBTurnoverFee trnOv = new CDBTurnoverFee();
               CMySQLConnector.FillClassFields(el,trnOv);
               lst.Add(trnOv);

           }
            );



            LogMethEntry("LoadTurnoverFeesCoef");

            return lst;


        }







        public bool LoginRequest(string user, string password)
        {

            LogMethEntry("LoginRequest");


            //string stWhereCriteria = String.Format("UCASE (`login`)=UCASE('{0}') AND `password`=SHA2(CONCAT(`salt`,'{1}'),256) and accounts_trade.user_id = users.id",
            //                                                  user, password);

            //TODO StockExchangeId
            string stWhereCriteria = String.Format("UCASE (`accounts_money_code`)=UCASE('{0}') AND users.password=SHA2(CONCAT(`salt`,'{1}'),256) and stock_exchange_id=1",
                                                                    user, password);


            string sql = (new CSQLSelectBuilder())
                                    //._1_Select("accounts_trade.id accounts_trade_id,  accounts_trade.user_id, users.id")
                                    ._2_FromTable("users, accounts_trade, accounts_money")
                                    ._3_Where(stWhereCriteria)
                                    ._4_LinqTables("accounts_trade.accounts_money_id = accounts_money.id and accounts_money.user_id = users.id")
                                    .Build();




            var t = _MySQLConnector.ExecuteSelect(sql);



            //   var t =  _MySQLConnector.ExecuteSelectSimple("users, accounts_trade",
            //                                                 stWhereCriteria);


            LogMethExit("LoginRequest");


            if (t.Count == 0)
                return false;
            else
                return true;


        }


        public bool LoginRequestTradeManager(string user, string password)
        {

            LogMethEntry("LoginRequestTradeManager");


            string stWhereCriteria = String.Format("UCASE (`login`)=UCASE('{0}') AND users.password=SHA2(CONCAT(`salt`,'{1}'),256) and is_trade_manager=1",
                                                                    user, password);


            string sql = (new CSQLSelectBuilder())
                                ._1_Select("login,is_trade_manager")
                                    ._2_FromTable("users")
                                    ._3_Where(stWhereCriteria)
                                    .Build();




            var t = _MySQLConnector.ExecuteSelect(sql);





            LogMethExit("LoginRequestTradeManager");


            if (t.Count == 0)
                return false;
            else
                return true;
        }




        public List<CDBInstrument> GetInstuments(int stockExchId)
        {

            Dictionary<string, object> paramList = new Dictionary<string, object>();
            paramList["in_stock_exch_id"] = stockExchId;

            return SelectObjectsFromProcedure<CDBInstrument>("get_instruments", paramList);

            //

        }


        public List<Dictionary<string, object>> GetAllAccountsMoneyCurrent()
        {
            return _MySQLConnector.ExecuteProcedure("get_all_accounts_money_current");
        }


        public List<Dictionary<string, object>> GetWalletChange()
        {

            return _MySQLConnector.ExecuteProcedure("get_wallet_change");

        }





        public List<T> SelectObjectsFromProcedure<T>(string procedureName, Dictionary<string, object> paramList = null) where T : new()
        {

            return _MySQLConnector.ExecuteSelectObjectProcedureName<T>(procedureName, paramList);
        }


        public void UpdateInstrument(object dbInstrument)
        {

            UpdateObjectWithId<CDBInstrument>((CDBInstrument)dbInstrument, "instruments");

        }

        public void UpdateFee(object dbFee)
        {

            Log("UpdateFee");

            CDBUpdateLate dbUpdFee = (CDBUpdateLate)dbFee;
            //TODO make using obj update
            var sql = new CSQLUpdateBuilder()
                        ._1_UpdateTable("poslog")
                        ._2_SetColumn("Fee", dbUpdFee.Fee)
                        ._2_SetColumn("Fee_Total", dbUpdFee.Fee_Total)
                        ._2_SetColumn("FeeDealing", dbUpdFee.FeeDealing)
                        ._2_SetColumn("Fee_Stock", dbUpdFee.Fee_Stock)
                        ._2_SetColumn("VMClosed_RUB", dbUpdFee.VMClosed_RUB)
                        ._2_SetColumn("VMClosed_RUB_user", dbUpdFee.VMClosed_RUB_user)                       
                        ._2_SetColumn("VMClosed_RUB_stock", dbUpdFee.VMClosed_RUB_stock)
                        ._2_SetColumn("IsFeeLateCalced", dbUpdFee.IsFeeLateCalced)
                        ._3_Where(String.Format("ReplId={0} and account_trade_Id={1}", dbUpdFee.ReplId, dbUpdFee.BotId))

                        .Build();




            _MySQLConnector.ExecuteInsertOrUpdate(sql);

        }




        public void UpdateObject<T>(T obj, string tableName, string whereCriteria)
        {

            var sql = new CSQLUpdateBuilder()
              ._1_UpdateTable(tableName)
              ._2_SetColumnObj(obj, _MySQLConnector.TablesSchemas[tableName])
              ._3_Where(whereCriteria)
              .Build();

            _MySQLConnector.ExecuteInsertOrUpdate(sql);

        }

        public void UpdateObjectWithId<T>(T obj, string tableName) where T : IObjectWithId
        {

            var sql = new CSQLUpdateBuilder()
              ._1_UpdateTable(tableName)
              ._2_SetColumnObj(obj, _MySQLConnector.TablesSchemas[tableName])
              ._3_Where("id=" + obj.id)
              .Build();

            _MySQLConnector.ExecuteInsertOrUpdate(sql);

        }




        public void LoadUserDealsLogData<T>(CDict_L2_List<int, string, T> dataLog, int stockExchId) where T : new()
        {
            LoadTradeDataLog<T>(LoadNotClearingProcessedTradeDataLog("userdealslog", "Moment_timestamp_ms", stockExchId),
                         dataLog);

        }



        public void LoadUserPosLogData<T>(CDict_L2_List<int, string, T> dataLog, int stockExchId) where T : new()
        {
            LoadTradeDataLog<T>(LoadNotClearingProcessedTradeDataLog("poslog", "DtClose_timestamp_ms", stockExchId),
                         dataLog);

        }




        //TODO move to separate class or to plaza2Connextor
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">userdealslog, userposlog</typeparam>
        /// <param name="queryRes">Results of query</param>
        /// <param name="data">Returns multidemension structure botId - instrumens of T  - </param>
        public void LoadTradeDataLog<T>(List<Dictionary<string, object>> queryRes,
                                    Dictionary<int, Dictionary<string, List<T>>> data) where T : new()
        {

            if (queryRes == null)
                return;


            //2016-09-14 for update clients trade log data after clearing
            data.Clear();


            foreach (var row in queryRes)
            {
                int id = Convert.ToInt32(row["account_trade_Id"]);
                if (!data.ContainsKey(id))
                    data[id] = new Dictionary<string, List<T>>();

                string instrument = row["Instrument"].ToString();
                if (!data[id].ContainsKey(instrument))
                    data[id][instrument] = new List<T>();
                T bp = new T();
                CMySQLConnector.FillClassFields<T>(row, bp);
                data[id][instrument].Add(bp);





            }





        }

        //public void GetLatest




        /// <summary>
        /// Get traders vm calculations with latest id which where not processed by reporter.
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetLatestCalcedVmData(int stockExchID)
        {

            return _MySQLConnector.ExecuteProcedure("get_latest_calced_vm_data", new Dictionary<string, object> { { "inp_stock_exch_id", stockExchID } });

        }


        public List<Dictionary<string, object>> GetPoslogClearingCalsSummary(int vmCalcId)
        {
            return _MySQLConnector.ExecuteProcedure("get_poslog_clearing_calcs_summary", new Dictionary<string, object> { { "calced_vm_id", vmCalcId } });


        }

        public List<Dictionary<string, object>> GetPoslogClearingCalsInstrumentsSummary(int vmCalcId)
        {
            return _MySQLConnector.ExecuteProcedure("get_poslog_clearing_calcs_instr_summary", new Dictionary<string, object> { { "calced_vm_id", vmCalcId } });


        }

        public List<Dictionary<string, object>> GetPoslogClearingCalsInstruments(int vmCalcId)
        {
            return _MySQLConnector.ExecuteProcedure("get_poslog_clearing_calcs_instr", new Dictionary<string, object> { { "calced_vm_id", vmCalcId } });


        }


        public void UpdateReportSent(int vmCalcId)
        {
            _MySQLConnector.ExecuteProcedure("update_report_sent", new Dictionary<string, object> { { "calced_vm_id", vmCalcId } });

        }

        public void UpdateReportBossSent(int sessionId)
        {
            _MySQLConnector.ExecuteProcedure("update_report_boss_sent", "in_session_id", sessionId);
        }


        public List<Dictionary<string, object>> GetSessionsBossReportNotSent(int sessionStockExchId)
        {
            return _MySQLConnector.ExecuteProcedure("get_session_boss_report_not_sent", "inSessionExchId", sessionStockExchId);

        }

        public List<Dictionary<string, object>> GetBossReportList()
        {
            return _MySQLConnector.ExecuteProcedure("get_need_boss_report");
        }

        public List<Dictionary<string, object>> LoadLatestUserTradeData(string storedProcedure, int stockExchId)
        {

            return _MySQLConnector.ExecuteProcedure(storedProcedure, new Dictionary<string, object> { { "in_stock_exch_id", stockExchId } });

        }


        public List<Dictionary<string, object>> GetMinMaxAccountOp(DateTime dtFrom)
        {

            return _MySQLConnector.ExecuteProcedure("get_min_max_accounts_op", "inDateTimeFrom", dtFrom);

        }


        public List<Dictionary<string, object>> LoadLatestSessionBeginTime()
        {


            //Note/ We accept that maximum db time has max stock session num
            string sqlSelect = new CSQLSelectBuilder()
                                   ._1_Select("Max(DtBegin_timestamp_ms) as DtBegin_timestamp_ms, Max(StockExchangedSessionId) as StockExchangedSessionId")
                                   ._2_FromTable("sessions")
                                   .Build();




            return _MySQLConnector.ExecuteSelect(sqlSelect);
        }


        public List<Dictionary<string, object>> GetAllAccountsSumBySession(int sessionId)
        {

            return _MySQLConnector.ExecuteProcedure("get_all_accounts_sums_by_session", "inpSessionId", sessionId.ToString());
        }



        public List<Dictionary<string, object>> LoadNotClearingProcessedTradeDataLog(string tableName, string dtColSortBy, int stockExchId)
        {

            LogMethEntry("LoadNotClearingProcessedTradeDataLog");
            Log("tableName=" + tableName + " dtCol=" + dtColSortBy);


            string sqlSelect = new CSQLSelectBuilder()
                                               ._2_FromTable(tableName)
                                               ._3_Where("IsClearingProcessed<>1 and stock_exch_id=" + stockExchId.ToString())
                                               ._6_OrderBy(dtColSortBy)
                                               .Build();


            LogMethExit("LoadNotClearingProcessedTradeDataLog");

            return _MySQLConnector.ExecuteSelect(sqlSelect);

        }

        /// <summary>
        /// 
        /// Generic method that  retrieves not cleaaring processed trade data (poslog and dealslog).
        /// earlier than dtVal for all traders and bots.
        /// Retrieves data from tables "userdealslog" and "userposlog"
        /// Note all previous sessions must be clearing processed for correct work
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dtCol"></param>
        /// <param name="dtVal"></param>
        /// <returns>List of rows from database    </returns>
        public List<Dictionary<string, object>> LoadNotClearingProcessedTradeDataLogTimeFilt(string tableName, string dtCol,
                                                                                                long dtVal, int stockExchId)
        {


            LogMethEntry("LoadNotClearingProcessedTradeDataLogTimeFilt");

            string whereCond = String.Format("IsClearingProcessed<>1 and {0}<{1} and stock_exch_id={2}",
                                               dtCol, dtVal, stockExchId);


            string sqlSelect = new CSQLSelectBuilder()
                                               ._2_FromTable(tableName)
                                               ._3_Where(whereCond)
                                               ._6_OrderBy(dtCol)
                                               .Build();

            LogMethExit("LoadNotClearingProcessedTradeDataLogTimeFilt");

            return _MySQLConnector.ExecuteSelect(sqlSelect);

        }


        /*public void GetData(int clearingCa)
        {


        }
        */


        public List<Dictionary<string, object>> GetSessionsNotClearingProcessed(int stockExchId)
        {
            LogMethEntry("GetSessionsNotClearingProcessed");
            string whereCond = "IsClearingProcessed<>1 and IsCompleted=1 and stock_exchange_id=" + stockExchId;


            string sqlSelect = new CSQLSelectBuilder()
                                               ._2_FromTable("sessions")
                                               ._3_Where(whereCond)
                                               ._6_OrderBy("DtBegin_timestamp_ms")
                                               .Build();


            LogMethExit("GetSessionsNotClearingProcessed");


            return _MySQLConnector.ExecuteSelect(sqlSelect);
        }





        public void InsertSessionDefaultSchedule(CDBSessionDefaultSchedule sessDefSched)
        {

            string sql = new CSQLInsertBuilder()
                               ._1_InsertIntoTable("session_default_schedule")
                               ._2_SetColumnObj(sessDefSched, _MySQLConnector.TablesSchemas["session_default_schedule"])
                               .Build();

            _MySQLConnector.ExecuteSelect(sql);
        }

        public CDBSessionDefaultSchedule GetSessionSchedule(int stockExchId)
        {


            string sql = new CSQLSelectBuilder()
                            ._2_FromTable("session_default_schedule")
                            ._3_Where("StockExchanged_id =" + stockExchId)
                            .Build();

            CDBSessionDefaultSchedule dbSessSched = new CDBSessionDefaultSchedule();

            var res = _MySQLConnector.ExecuteSelect(sql);


            CMySQLConnector.FillClassFields<CDBSessionDefaultSchedule>(res[0], dbSessSched);

            return dbSessSched;



        }





        public void InsertClearing(CDBClearing dbClearing)
        {
            LogMethEntry("InsertClearing");

            string sql = new CSQLInsertBuilder()
                            ._1_InsertIntoTable("clearing")
                            ._2_SetColumnObj(dbClearing, _MySQLConnector.TablesSchemas["clearing"])
                            .Build();

            _MySQLConnector.ExecuteInsertOrUpdate(sql);

            LogMethExit("InsertClearing");

        }



        private void UpdateUserTradeDataLog(/*CDBUserPosLog dbUserPosLog*/object obj, string table)
        {

            LogMethEntry("UpdateUserTradeDataLog");

            Log("UpdateUserTradeDataLog " + table);

            string sql = new CSQLInsertBuilder()
                                                      ._1_InsertIntoTable(table)
                                                      ._2_SetColumnObj(obj, _MySQLConnector.TablesSchemas[table])
                                                      .Build();

            _MySQLConnector.ExecuteInsertOrUpdate(sql);

            LogMethExit("UpdateUserTradeDataLog");

        }


        public void UpdateRoundTo(string instrument, int stockExchId, int newRoundTo)
        {
            LogMethEntry("UpdateRoundTo");

            string sql = new CSQLUpdateBuilder()
                        ._1_UpdateTable("instruments")
                        ._2_SetColumn("RoundTo", newRoundTo)
                        ._3_Where(String.Format(@"stock_exch_id={0} and instrument='{1}'", stockExchId, instrument))
                        .Build();

            Log(sql);

            _MySQLConnector.ExecuteInsertOrUpdate(sql);

            LogMethEntry("UpdateRoundTo");

        }


        public void UpdateMinStep(string instrument, int stockExchId, decimal newMinStep)
        {
            LogMethEntry("UpdateRoundTo");

            string sql = new CSQLUpdateBuilder()
                        ._1_UpdateTable("instruments")
                        ._2_SetColumn("Min_step", newMinStep)
                        ._3_Where(String.Format(@"stock_exch_id={0} and instrument='{1}'", stockExchId, instrument))
                        .Build();

            Log(sql);

            _MySQLConnector.ExecuteInsertOrUpdate(sql);

            LogMethExit("UpdateRoundTo");

        }



        public void UpdateStepPrice(object inDbUpdStepPrice)
        {

            LogMethEntry("UpdateStepPrice");

            CDBUpdateStepPrice updStepPrice = (CDBUpdateStepPrice)inDbUpdStepPrice;

            string sql = new CSQLUpdateBuilder()
                        ._1_UpdateTable("instruments")
                        ._2_SetColumn("Step_price", updStepPrice.NewStepPrice)
                        ._3_Where(String.Format(@"stock_exch_id={0} and instrument='{1}'", updStepPrice.StockExchId, updStepPrice.Instrument))
                        .Build();

            Log(sql);

            _MySQLConnector.ExecuteInsertOrUpdate(sql);


            LogMethExit("UpdateStepPrice");
        }






        /// <summary>
        /// Find session with requested by specific criteria (different for P2 and ASTS) . 
        /// If session is not exist
        /// insert session record.
        /// </summary>
        /// <param name="dbSession"></param>
        private void InsertUnsavedSession(CDBSession dbSession, string whereCriteriaCpecific)
        {
            LogMethEntry("InsertUnsavedSession");


            string whereCriteria = String.Format("stock_exchange_id={0} and {1}",
                                                  dbSession.stock_exchange_id,
                                                  whereCriteriaCpecific);



            string sql = new CSQLSelectBuilder()
                                 ._2_FromTable("sessions")
                                 ._3_Where(whereCriteria)
                                 .Build();


            var res = _MySQLConnector.ExecuteSelect(sql);
            if (res.Count != 0)
            {
                Log("No unsaved sessions");
                LogMethExit("InsertUnsavedSession");
                return; //session is already saved - get out

            }
            string sqlIns = new CSQLInsertBuilder()
                                ._1_InsertIntoTable("sessions")
                                ._2_SetColumnObj(dbSession, _MySQLConnector.TablesSchemas["sessions"])
                                .Build();

            _MySQLConnector.ExecuteInsertOrUpdate(sqlIns);

            LogMethExit("InsertUnsavedSession");

        }

        /// <summary>
        /// On P2 use unique session number
        /// </summary>
        /// <param name="dbSession"></param>
        public void InsertUnsavedSessionP2(CDBSession dbSession)
        {
            string whereCriteria = "StockExchangedSessionId=" + dbSession.StockExchangedSessionId;
            InsertUnsavedSession(dbSession, whereCriteria);

        }


        /// <summary>
        /// On ASTS use "Session Begin" as criteria
        /// </summary>
        /// <param name="dbSession"></param>
        public void InsertUnsavedSessionASTS(CDBSession dbSession)
        {
            // string whereCriteria = "DtBegin= '" + dbSession.DtBegin.ToString() + "'";

            string whereCriteria = "DtBegin="
                                    + CMySQLConv.ToMySQLFormat(dbSession.DtBegin); ;

            InsertUnsavedSession(dbSession, whereCriteria);

        }

        public void InsertUnsavedSessionCrypto(CDBSession dbSession)
        {


            string whereCriteria = "DtBegin="
                                    + CMySQLConv.ToMySQLFormat(dbSession.DtBegin); ;

            InsertUnsavedSession(dbSession, whereCriteria);

        }



        public List<Dictionary<string, object>> GetUnCompletedSessions(int stockExchId)
        {
            LogMethEntry("GetUnCompletedSessions");

            string whereCond = "IsCompleted!=1 and stock_exchange_id=" + stockExchId;


            string sql = new CSQLSelectBuilder()
                            ._2_FromTable("sessions")
                            ._3_Where(whereCond)
                            .Build();

            LogMethExit("GetUnCompletedSessions");

            return _MySQLConnector.ExecuteSelect(sql);
        }




        public void SetCompletedSessions(List<int> needSetCompletedSession)
        {

            LogMethEntry("SetCompletedSessions");

            string whereStateMent = "";
            needSetCompletedSession.ForEach(a => whereStateMent += "StockExchangedSessionId =" + a + " or ");


            whereStateMent = whereStateMent.Remove(whereStateMent.Length - 3, 3);


            string sql = new CSQLUpdateBuilder()
                            ._1_UpdateTable("sessions")
                            ._2_SetColumn("IsCompleted", 1)
                            ._3_Where(whereStateMent)
                            .Build();

            _MySQLConnector.ExecuteInsertOrUpdate(sql);


            LogMethExit("SetCompletedSessions");
        }




        public void UpdateSession(object dbSession)
        {

            LogMethEntry("UpdateSession");

            //why do we need check ? may be remove it ?
            /*  string sqlSelect = new CSQLSelectBuilder()
                                  ._2_FromTable("sessions")
                                  ._3_Where("DtBegin_timestamp_ms=" + CUtilTime.GetUnixTimestampMillis(((CDBSession)dbSession).DtBegin))
                                  .Build();

               var res =  _MySQLConnector.ExecuteSelect(sqlSelect);

                if (res.Count !=0 || res[0]["DtBegin"]!= DBNull.Value)
                    return;
             */

            string sqlInsert = new CSQLInsertBuilder()
                          ._1_InsertIntoTable("sessions")
                          ._2_SetColumnObj(dbSession, _MySQLConnector.TablesSchemas["sessions"])
                          .Build();

            _MySQLConnector.ExecuteInsertOrUpdate(sqlInsert);

            LogMethExit("UpdateSession");

        }


        public void UpdateCryptoInstrumentData(string instrument, int codeStockExchId, decimal minimumOrderSize, int decimalVolume)
        {

            string sqlUpdate = new CSQLUpdateBuilder()
                                    ._1_UpdateTable("instruments")
                                    ._2_SetColumnExpr("minimum_order_size", minimumOrderSize.ToString())
                                    ._2_SetColumnExpr("DecimalVolume", decimalVolume.ToString())
                                    ._2_SetColumnExpr("IsInitialised", "1")
                                    ._3_Where(string.Format(@"instrument='{0}' and stock_exch_id={1}", instrument, codeStockExchId))
                                    .Build();




            _MySQLConnector.ExecuteInsertOrUpdate(sqlUpdate);

            LogMethExit("UpdateCryptoInstrumentData");


        }


        public List<Dictionary<string, object>> LoadAccountsMoney()
        {
            LogMethEntry("LoadAccountsMoney");

            string sql = new CSQLSelectBuilder()
                              ._2_FromTable("accounts_money")
                              .Build();

            LogMethExit("LoadAccountsMoney");

            return _MySQLConnector.ExecuteSelect(sql);

        }



        public List<Dictionary<string, object>> LoadAccountsTrade(int stockExchagesId)
        {

            LogMethEntry("LoadAccountsTrade");

            string selectStmnt = "accounts_money_id, stock_exchange_id, accounts_trade.money_avail as money_avail, leverage, proc_profit, money_sess_limit, name, proc_fee_dealing,proc_fee_turnover_limit,proc_fee_turnover_market ";

            string linqStm = "accounts_money_id = accounts_money.id and stock_exchanges.id = accounts_trade.stock_exchange_id";

            string sql = new CSQLSelectBuilder()
                              ._1_Select(selectStmnt)
                              ._2_FromTable("accounts_trade, accounts_money, stock_exchanges")
                              ._4_LinqTables(linqStm)
                              ._3_Where("stock_exchange_id=" + stockExchagesId)
                              .Build();


            LogMethExit("LoadAccountsTrade");

            return _MySQLConnector.ExecuteSelect(sql);

        }


        public void InsertAccountOperationsLog(CDBAccountsOperationsLog dbOperationsLog)
        {

            LogMethEntry("InsertAccountOperationsLog");

            string sql = new CSQLInsertBuilder()
                                ._1_InsertIntoTable("accounts_operations_log")
                                ._2_SetColumnObj(dbOperationsLog, _MySQLConnector.TablesSchemas["accounts_operations_log"])

                                .Build();


            _MySQLConnector.ExecuteInsertOrUpdate(sql);

            LogMethExit("InsertAccountOperationsLog");

        }




        public void UpdateMoney(IAccountMoney dbAccMoney, string tableName, string stId, int stock_exchange_id = 0)
        {


            LogMethEntry("UpdateMoney");


            string whereCond = stId + "=" + dbAccMoney.GetId();

            if (tableName == "accounts_trade")
                whereCond += " and stock_exchange_id=" + stock_exchange_id;


            string sql = new CSQLUpdateBuilder()
                            ._1_UpdateTable(tableName)
                            ._2_SetColumnObj(dbAccMoney, _MySQLConnector.TablesSchemas[tableName])
                            ._3_Where(whereCond)
                            .Build();



            _MySQLConnector.ExecuteInsertOrUpdate(sql);

            LogMethExit("UpdateMoney");


            //  UpdateAccountsMoney(money_change, id);
            //  UpdateMoneyData(money_change, id);


        }

        /*
        public void UpdateMoneyData(decimal money_change, int id)
        {

            LogMethEntry("UpdateMoneyData");

            //TODO for diffenert tables

            string sql = new CSQLUpdateBuilder()
                                ._1_UpdateTable("accounts_trade")
                                ._2_SetColumnExpr("money_avail", "money_avail" + money_change)
                                ._3_Where("accounts_money_id=" + id)
                                .Build();



           // string sql2 = "update accounts_trade set  money_avail= @m:= money_avail-41.817  where accounts_money_id=100 ;select @m";

           // _MySQLConnector.ExecuteSelect(sql2);

            Dictionary<string, object> ls = new Dictionary<string, object>();
            ls["@vm_changed"] = money_change;
            ls["@id_in"] = id;
            ls["@stock_exch_id_in"] = 1;
                                          

             var res =  _MySQLConnector.ExecuteProcedure("update_money_data",ls);




            if (res != null)
                Thread.Sleep(0);
            //_MySQLConnector.ExecuteInsertOrUpdate(sql);

        }
         */







        public void UpdateAccountsMoney(decimal money_change, int id)
        {


            LogMethEntry("UpdateAccountsMony");

            //TODO for diffenert tables

            string sql = new CSQLUpdateBuilder()
                                ._1_UpdateTable("accounts_money")
                                ._2_SetColumnExpr("money_avail", "money_avail" + money_change)
                                ._3_Where("id=" + id)
                                .Build();


            _MySQLConnector.ExecuteInsertOrUpdate(sql);

            LogMethExit("UpdateAccountsMony");

        }






        public void UpdateAccountsTradeLog(int id)
        {



        }


        public long UpdateCalcedVM(CDBClearingCalcedVM dbClearingCalcedVM)
        {
            LogMethEntry("UpdateCalcedVM");

            string sql = new CSQLInsertBuilder()
                                ._1_InsertIntoTable("clearing_calced_vm")
                                ._2_SetColumnObj(dbClearingCalcedVM, _MySQLConnector.TablesSchemas["clearing_calced_vm"])
                                .Build();


            LogMethExit("UpdateCalcedVM");

            return _MySQLConnector.ExecuteInsertOrUpdate(sql);

        }



        //public long Inser


        public long InsertObject(object obj, string tableName)
        {

            string sql = new CSQLInsertBuilder()
                                   ._1_InsertIntoTable(tableName)
                                   ._2_SetColumnObj(obj, _MySQLConnector.TablesSchemas[tableName])
                                   .Build();


            return _MySQLConnector.ExecuteInsertOrUpdate(sql);


        }


        public long InsertPayout(CDBPayout dbPayout)
        {

            return InsertObject(dbPayout, "payouts");
        }

        public void SetClearingProcessedTradeData(long dtVal, int account_id, int stock_exch_Id, int calcVMId)
        {

            LogMethEntry("SetClearingProcessedTradeData");


            string commCriteria = string.Format("{0}={1} and {2}={3} and ",
                                                "account_trade_Id", account_id,
                                                 "stock_exch_id", stock_exch_Id);

            string specCriteria = commCriteria + string.Format("{0}<{1}", "DtClose_timestamp_ms", dtVal);
            SetClearingProcessedWithSpecialCriteria("poslog", specCriteria, calcVMId);

            specCriteria = commCriteria + string.Format("{0}<{1}", "Moment_timestamp_ms", dtVal);
            SetClearingProcessedWithSpecialCriteria("userdealslog", specCriteria, calcVMId);

            LogMethExit("SetClearingProcessedTradeData");


        }

        public void SetClearingProcessSession(int sesionId)
        {
            LogMethEntry("SetClearingProcessSession");

            string specCriteria = "id=" + sesionId;

            string sql = new CSQLUpdateBuilder()
                                              ._1_UpdateTable("sessions")
                                              ._2_SetColumn("IsClearingProcessed", 1)
                                              ._3_Where(specCriteria)
                                              .Build();


            _MySQLConnector.ExecuteInsertOrUpdate(sql);

            LogMethExit("SetClearingProcessSession");
        }



        private void SetClearingProcessedWithSpecialCriteria(string tableName, string specialCriteria, int calcedVMId)
        {


            LogMethEntry("SetClearingProcessedWithSpecialCriteria");

            string whereCond = "IsClearingProcessed<>1 and " + specialCriteria;


            string sql = new CSQLUpdateBuilder()
                                               ._1_UpdateTable(tableName)
                                               ._2_SetColumn("IsClearingProcessed", 1)
                                               ._2_SetColumn("calced_vm_id", calcedVMId)
                                               ._3_Where(whereCond)
                                               .Build();


            _MySQLConnector.ExecuteInsertOrUpdate(sql);

            LogMethExit("SetClearingProcessedWithSpecialCriteria");

        }



        public List<Dictionary<string, object>> LoadBotsConfig(int stockExchId)
        {
            string sql = new CSQLSelectBuilder()
                                ._2_FromTable("bots_config")
                                ._3_Where("stockExchId=" + stockExchId)
                                .Build();


            List<Dictionary<string, object>> res = _MySQLConnector.ExecuteSelect(sql);

            return res;

        }

        public List<Dictionary<string, object>> LoadBotInstrumentConfig(int stockExchId)
        {
            string sql = new CSQLSelectBuilder()
                            ._2_FromTable("bots_instrument_config")
                            ._3_Where("stockExchId=" + stockExchId)
                            .Build();


            List<Dictionary<string, object>> res = _MySQLConnector.ExecuteSelect(sql);

            return res;

        }



        public void InsertExceptHol(CDBExceptDay excpHol)
        {
            _MySQLConnector.InsertObj("except_holidays", excpHol);
        }

        public void InsertExceptDayOff(CDBExceptDay excpHol)
        {
            _MySQLConnector.InsertObj("except_dayoff", excpHol);
        }


        public void UpdateWalletLog(DateTime dt, string walletType, string currency, decimal balance)
        {
            CDBUpdateWallet dBUpdateWallet = new CDBUpdateWallet
            {
                Wallet_type = walletType,
                Currency = currency,
                Balance = balance,
                Dt = dt

            };

            QueueData(dBUpdateWallet);


        }

        private void UpdateWalletLog(object objWalletUpd)
        {

            LogMethEntry("UpdateWalletLog");

            CDBUpdateWallet dBUpdateWallet = (CDBUpdateWallet)objWalletUpd;



            string tableName = "walletlog";

            string sql = new CSQLInsertBuilder()
                            ._1_InsertIntoTable(tableName)
                            ._2_SetColumnObj(objWalletUpd,
                                        _MySQLConnector.TablesSchemas[tableName])
                            .Build();


            _MySQLConnector.ExecuteInsertOrUpdate(sql);


            LogMethExit("UpdateWalletLog");

        }


        private void UpdateFeeUserDealsLog(object objUpdateFeeUserDealsLog)
        {
            CDBUpdateFeeUserDealsLog dBUpdateFeeUserDealsLog =
                (CDBUpdateFeeUserDealsLog)objUpdateFeeUserDealsLog;

            LogMethEntry("UpdateFeeUserDealsLog");

            string tableName = "userdealslog";

            string sql = new CSQLUpdateBuilder()
                        ._1_UpdateTable(tableName)
                        ._2_SetColumn("Fee", dBUpdateFeeUserDealsLog.Fee)                        
                        ._2_SetColumn("FeeDealing", dBUpdateFeeUserDealsLog.FeeDealing)
                        ._2_SetColumn("Fee_Stock", dBUpdateFeeUserDealsLog.FeeStock)
                        ._3_Where("DealId=" + dBUpdateFeeUserDealsLog.DealId)
                        .Build();

            _MySQLConnector.ExecuteInsertOrUpdate(sql);

            LogMethExit("UpdateFeeUserDealsLog");

        }


        public List<Dictionary<string, object>> GetWalletEndPrevDay()
        {

            return _MySQLConnector.ExecuteProcedure("get_wallet_end_prev_date");

        }

        private void UpdateBindDbData(object objBindDealBotPos)
        {
            LogMethEntry("UpdateBindDbData");

            CDBBindDealBotPos bindDealBotPos = (CDBBindDealBotPos)objBindDealBotPos;


            string tableName = "userdealslog";

            string sql = new CSQLUpdateBuilder()
                        ._1_UpdateTable(tableName)
                        ._2_SetColumn("BP_DtOpen_timestamp_ms", bindDealBotPos.BP_DtOpen_timestamp_ms)
                        ._3_Where("DealId=" + bindDealBotPos.DealId)
                        .Build();

            _MySQLConnector.ExecuteInsertOrUpdate(sql);

            LogMethExit("UpdateBindDbData");

        }

        private void UpdateOpenedClosedTotal(object objUpdOpenedClosedTot)
        {
            LogMethEntry("UpdateOpenedClosedTotal");

            CDBUpdVMOpenedClosedTot dbUpd = (CDBUpdVMOpenedClosedTot)objUpdOpenedClosedTot;

            string table = "vm_opened_closed_total";

            string sql = new CSQLUpdateBuilder()
                            ._1_UpdateTable(table)
                            ._2_SetColumn("VMAllInstrOpenedAndClosed", dbUpd.VMAllInstrOpenedAndClosed)
                            ._3_Where(String.Format("account_id={0} and stock_exch_id={1}",
                                         dbUpd.AccountId, dbUpd.StockExchId))
                            .Build();


            _MySQLConnector.ExecuteInsertOrUpdate(sql);

            LogMethExit("UpdateOpenedClosedTotal");

        }

        private void UpdDBPosInstr(object objUpdDBPosInstr)
        {
            LogMethEntry("UpdDBPosInstr");

            CDBUpdPosInstr dbUpdPosInstr = (CDBUpdPosInstr)objUpdDBPosInstr;
        
            string table = "pos_opened";
            string sql = new CSQLInsertBuilder()
                        ._1_InsertIntoTable(table)
                        ._2_SetColumnObj(objUpdDBPosInstr, _MySQLConnector.TablesSchemas[table])
                        ._3_OnDuplicateKeyEnytry()
                        ._3_OnDuplicateKeyEnytryUpd("amount", dbUpdPosInstr.amount)
                        ._3_OnDuplicateKeyEnytryUpd("AvPos",dbUpdPosInstr.AvPos)
                        ._3_OnDuplicateKeyEnytryUpd("Dt_upd", CMySQLConv.ToMySQLFormat(dbUpdPosInstr.Dt_upd))
                        
                        .Build();

            




          _MySQLConnector.ExecuteInsertOrUpdate(sql);


          LogMethExit("UpdDBPosInstr");

      }






      public void InsertBfxTrades(object objInsertBfxTrades)
      {

          LogMethEntry("InsertBfxTrades");

          string tableName = "bfx_trades_history";

          string sql = new CSQLInsertBuilder()
                      ._1_InsertIntoTable(tableName)
                      ._2_SetColumnObj(objInsertBfxTrades, _MySQLConnector.TablesSchemas[tableName])
                      .Build();

          _MySQLConnector.ExecuteInsertOrUpdate(sql);

          LogMethExit("InsertBfxTrades");

      }


      public void InsertBfxOrdersHistory(object objBfxOrder)
      {
          LogMethEntry("InsertBfxOrdersHistory");


          string tableName = "bfx_orders_history";

          string sql = new CSQLInsertBuilder()
                      ._1_InsertIntoTable(tableName)
                      ._2_SetColumnObj(objBfxOrder, _MySQLConnector.TablesSchemas[tableName])
                      .Build();

          _MySQLConnector.ExecuteInsertOrUpdate(sql);


          LogMethExit("InsertBfxOrdersHistory");

      }

      public void InsertBfxTradesHistory(object objBfxTrades)
      {
          LogMethEntry("InsertBfxTradesHistory(");

          string tableName = "bfx_trades_history";

          string sql = new CSQLInsertBuilder()
                      ._1_InsertIntoTable(tableName)
                      ._2_SetColumnObj(objBfxTrades, _MySQLConnector.TablesSchemas[tableName])
                      .Build();

          _MySQLConnector.ExecuteInsertOrUpdate(sql);


          LogMethExit("InsertBfxTradesHistory(");
      }




      public List<CDBBfxOrder> GetBfxOrdersHistory(string stDtFrom)
      {

          LogMethEntry("GetBfxOrdersHistory");

          string sql = new CSQLSelectBuilder()
                          ._1_Select("*")
                          ._2_FromTable("bfx_orders_history")
                          ._3_Where(String.Format(@"DtUpdate>'{0}'", stDtFrom))
                          .Build();


          List<CDBBfxOrder> _lst = _MySQLConnector.ExecuteSelectObject<CDBBfxOrder>(sql);

          LogMethExit("GetBfxOrdersHistory");
          return _lst;

      }


      public List<CDBBfxTrades> GetBfxTradesHistory(string stDtFrom)
      {

          LogMethEntry("GetBfxTradesHistory");

          string sql = new CSQLSelectBuilder()
                          ._1_Select("*")
                          ._2_FromTable("bfx_trades_history")
                          ._3_Where(String.Format(@"DtCreate>'{0}'", stDtFrom))
                          .Build();


          List<CDBBfxTrades> _lst = _MySQLConnector.ExecuteSelectObject<CDBBfxTrades>(sql);

          LogMethExit("GetBfxTradesHistory");
          return _lst;

      }




      public List<CDBTurnOver>  GetTradersTurnover(DateTime dtFrom)
      {

          LogMethEntry("GetTradersTurnover");


          string whereCriteria = String.Format(@"Moment >= '{0}-{1}-{2} {3}:{4}:{5}' and userdealslog.stock_exch_id = 4",
                                                  dtFrom.Year.ToString(),
                                                  dtFrom.Month.ToString(),
                                                  dtFrom.Day.ToString(),
                                                  dtFrom.Hour.ToString(),
                                                  dtFrom.Minute.ToString(),
                                                  dtFrom.Second.ToString()

                                                  );


          string sql = new CSQLSelectBuilder()
                      ._1_Select("account_trade_Id, sum(Amount*Price) as turnover")
                      ._2_FromTable("userdealslog")
                      ._3_Where(whereCriteria)
                      ._5_GroupBy("account_trade_Id")
                      .Build();

          List <CDBTurnOver> res = _MySQLConnector.ExecuteSelectObject<CDBTurnOver>(sql);


          LogMethExit("GetTradersTurnover");

          return res;

      }





      public void UpdateTradersTurnover(List <CDBTurnOver> lstDbTurnover)
      {

          LogMethEntry("UpdateTradersTurnover");

          string sql = new CSQLUpdateMultipleBuilder()
                        ._1_UpdateTable("accounts_trade")
                        ._2_SetCaseList(lstDbTurnover,
                                         objNameCond:"account_trade_Id",
                                         objNameValue:"turnover",
                                         dbNameCond: "accounts_money_id", 
                                         dbNameValue: "turnover")
                        ._3_Where("stock_exchange_id=4")
                        .Build();

          _MySQLConnector.ExecuteInsertOrUpdate(sql);

          LogMethExit("UpdateTradersTurnover");



      }


      public void UpdateTradersFeeProc(List <CDBTurnoverFee> lstDbTurnOverFee)
      {

          LogMethEntry("UpdateTradersFeeProc");

          string sql = new CSQLUpdateMultipleBuilder_2Val()
                      ._1_UpdateTable("accounts_trade")
                      ._2_SetCaseList(lstDbTurnOverFee,
                                       objNameCond1: "account_money_id",
                                       objNameValue1: "proc_fee_turnover_limit",
                                       objNameCond2: "account_money_id",
                                       objNameValue2: "proc_fee_turnover_market",
                                       dbNameCond1: "accounts_money_id",
                                       dbNameValue1: "proc_fee_turnover_limit",
                                       dbNameCond2: "accounts_money_id",
                                       dbNameValue2:"proc_fee_turnover_market")
                      ._3_Where("stock_exchange_id=4")
                      .Build();

          _MySQLConnector.ExecuteInsertOrUpdate(sql);

          LogMethExit("UpdateTradersFeeProc");
      }










  }







}
