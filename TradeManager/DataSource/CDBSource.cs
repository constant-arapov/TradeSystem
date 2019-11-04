using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TradeManager.Models;
using DBCommunicator;
using DBCommunicator.Interfaces;
using DBCommunicator.Builders;

using Common.Interfaces;

using TradeManager.Interfaces.Keys;


namespace TradeManager.DataSource
{
    public class CDBSource : IClientDatabaseConnector, IAlarmable
    {
        private ModelDBCon _conf;
        public ModelDBCon Conf
        {
            get
            {
                return _conf;
            }

        }


        public bool IsDatabaseConnected { get; set; }

        public bool IsDatabaseReadyForOperations { get; set; }

        private IAlarmable _client;


        private CMySQLConnector _mysqlConnector;

        public CMySQLConnector MySQLConnector
        {
            get
            {
                return _mysqlConnector;
            }

        }



        public CDBSource(ModelDBCon conf, IAlarmable client)
        {
            _conf = conf;
            _client = client;
            _mysqlConnector = new CMySQLConnector(_conf.Host, _conf.DatabaseName, "root", "profinvest", this, this);
        }


        public List<T> GetObject<T>(string procedureName, Dictionary<string, object> _dictParams = null) where T : IKey_Server, new()
        {
            return _mysqlConnector.ExecuteSelectObjectProcedureName<T>(procedureName, _dictParams);
        }



        public void Connect()
        {
            _mysqlConnector.Connect();
        }

        public void UpdateConnectionState()
        {
            _mysqlConnector.UpdateConnectionState();
        

        }




        public void Error(string err, Exception e)
        {
            _client.Error(err, e);
        }

        public long UpdateProcProfit(int accountId, int stockExchId, decimal value)
        {

            string sql = new CSQLUpdateBuilder()
                            ._1_UpdateTable("accounts_trade")
                            ._2_SetColumn("proc_profit", value)
                            ._3_Where(String.Format("accounts_money_id={0} and stock_exchange_id={1}", accountId, stockExchId))
                            .Build();


            long id = _mysqlConnector.ExecuteInsertOrUpdate(sql);

            return id;
        }



        public long UpdateProcFeeDealing(int accountId, int stockExchId, decimal value)
        {

            string sql = new CSQLUpdateBuilder()
                            ._1_UpdateTable("accounts_trade")
                            ._2_SetColumn("proc_fee_dealing", value)
                            ._3_Where(String.Format("accounts_money_id={0} and stock_exchange_id={1}", accountId, stockExchId))
                            .Build();


            long id = _mysqlConnector.ExecuteInsertOrUpdate(sql);

            return id;
        }


        public void UpdTrdAddFundsReq(int id)
        {
            string sql = new CSQLUpdateBuilder()
                        ._1_UpdateTable("traders_request_add_funds")
                        ._2_SetColumnExpr("dt_complete","Now()")
                        //._2_SetColumn("dt_complete", DateTime.Now)
                        ._3_Where(String.Format("id={0}",id))
                        .Build();


            _mysqlConnector.ExecuteInsertOrUpdate(sql);
                

        }


        public void UpdTrdWithDrawReq(int id)
        {
            string sql = new CSQLUpdateBuilder()
                        ._1_UpdateTable("traders_request_withdraw")
                        ._2_SetColumnExpr("dt_complete", "Now()")
                        //._2_SetColumn("dt_complete", DateTime.Now)
                        ._3_Where(String.Format("id={0}", id))
                        .Build();


            _mysqlConnector.ExecuteInsertOrUpdate(sql);


        }





        public List<ModelTrdWithdrawReq> GetWthdrawRequests()
        {
            // select* from traders_request_withdraw where dt_sms_confirm is not NULL and
            //dt_email_confirm is not null  and dt_complete is null

            string whereCond = "dt_sms_confirm is not NULL and     dt_email_confirm is not null  and dt_complete is null";


            string sql = new CSQLSelectBuilder()
                         ._1_Select("traders_request_withdraw.id, fnct_get_name_short(first_name,second_name,third_name) as TraderName, " +
                                      "StockExchId,account_trade_id,dt_add, amount,walletId, email" )
                         ._2_FromTable("traders_request_withdraw, accounts_money, users")
                         ._3_Where(whereCond)
                         ._4_LinqTables("accounts_money.id=traders_request_withdraw.account_trade_id and +" +
                                        "accounts_money.user_id = users.id    ")
                         ._6_OrderBy("id desc")
                         .Build();
           
          
            
           List<ModelTrdWithdrawReq> res =   _mysqlConnector.ExecuteSelectObject<ModelTrdWithdrawReq>(sql);


            return res;
        }






    }
}
