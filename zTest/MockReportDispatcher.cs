using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common;
using Common.Interfaces;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Common;

using DBCommunicator;
using DBCommunicator.Interfaces;


using ReportDispatcher;






namespace zTest
{
    public class MockReportDispatcher : MockBase, IClientDBCommunicator, IDataBaseStatus, IClientReportDispatcher
    {

        public bool IsDatabaseConnected { get; set; }
        public bool IsDatabaseReadyForOperations { get; set; }

        public int StockExchId { get; set; }



        public MockReportDispatcher()
        {
             CDBCommunicator dc = new CDBCommunicator("atfs",this);

             dc.WaitDatabaseConnected();
             StockExchId = CodesStockExch._01_MoexFORTS;

             CReportDispatcher _repDisp = new CReportDispatcher(this, dc, false, StockExchId);

            _repDisp.GenReports();


            System.Threading.Thread.Sleep(10000000);
            //_repDisp.GenReports();



        }

        public int GetDecimalVolume(string instrument)
        {

            return 0;
        }


    }
}
