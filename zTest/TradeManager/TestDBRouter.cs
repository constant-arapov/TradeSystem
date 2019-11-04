using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib.Common;


using TradeManager.Interfaces.Clients;
using TradeManager.DataSource;

using TradeManager.Models;

namespace zTest.TradeManager
{
    class TestDBRouter : IClientDbRouter
    {

        public void Test()
        {
            List<ModelDBCon> lstDBConnections = new List<ModelDBCon>
            {
                new ModelDBCon 
                { Host = "127.0.0.1", Port=3306, 
                    LstAvailStockExh = new List<int>(){ CodesStockExch._01_MoexFORTS } },
                new ModelDBCon { Host = "127.0.0.1", Port=3306,
                    LstAvailStockExh = new List<int>(){ CodesStockExch._04_CryptoBitfinex }}

            };

            List<int> lstStockExch = new List<int> { 
                                                    CodesStockExch._01_MoexFORTS,
                                                    CodesStockExch._04_CryptoBitfinex
                                                    };
                    




            CDBRouter dbRouter = new CDBRouter(this, lstDBConnections);

            dbRouter.Connect();

            var el = dbRouter.GetTradersLimits();


            if (el != null)
                System.Threading.Thread.Sleep(0);

            var el2 = dbRouter.GetInstruments();

            if (el2 != null)
                System.Threading.Thread.Sleep(0);

            var el3 = dbRouter.GetAvailableMoney();
            
            if (el3 != null)
                System.Threading.Thread.Sleep(0);
        }


        public void Error(string msg, Exception e = null)
        {
            throw e;
        }


        public bool IsStockExchSelected(int stockExhId)
        {
            return true;

        }


       



    }
}
