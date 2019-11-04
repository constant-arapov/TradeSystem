using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;


using Common;
using Common.Interfaces;

using TradingLib.Interfaces.Clients;

using DBCommunicator.Interfaces;
using DBCommunicator;

using TradingLib.Data.DB;

namespace zTest
{
    public class TestDBCommunicator :    IClientDBCommunicator , IAlarmable //IDatabaseConnectorClient, IAlarmable
    {
       
        CDBCommunicator _dbCommunicator;

        private bool _isDatabaseReadyForOperations;

        //public AutoResetEvent EvUpdate { get; set; }



        public bool IsDatabaseReadyForOperations
        {
            get
            {
                return _isDatabaseReadyForOperations;
            }
            set
            {
                _isDatabaseReadyForOperations = value;
            }

        }

        public int StockExchId { get; set; }

        
        public TestDBCommunicator() 
        {





       

            _dbCommunicator = new CDBCommunicator("atfs",this);


            while (!_dbCommunicator.IsDatabaseConnected || !_dbCommunicator.IsDatabaseReadyForOperations)
                Thread.Sleep(5);

             bool logedIn =       _dbCommunicator.LoginRequest("100", "ivanov");

          
                


             var res = _dbCommunicator.GetInstuments(1);

             res[0].IsInitialised = 1;


             _dbCommunicator.UpdateInstrument(res[0]);

             //var res = _dbCommunicator.SelectObjects<CDBInstrument>();


             if (res!=null)
                 Thread.Sleep(5);




            //_dbCommunicator.SelectObjects<CDBI





            /*
             _dbCommunicator.UpdatePosLog( new CUserPosLog
                                 {
                                PriceOpen = 100,
                                PriceClose = 200,
                                BuySell = botPos.BuySell,
                                DtOpen = botPos.DtOpen,
                                DtClose = botPos.DtClose,
                                CloseAmount = (int)botPos.CloseAmount,
                                VMClosed_Points = botPos.VMClosed_Points,
                                VMClosed_RUB = botPos.VMClosed_RUB,
                                Fee = botPos.Fee

                                 }   );
            */

          ///CMySQLConnector mysqlConn = new CMySQLConnector("localhost", "atfs", "root", "profinvest", this, this);

           //List<Dictionary<string, object>> lstResult = mysqlConn.ExecuteSelectSimple("stock_exchanges");


       



        //   mysqlConn.Disconnect();
         }

        public bool IsDatabaseConnected { get; set; }




        public void Error(string err, Exception e )
        {

        }

    }
}
