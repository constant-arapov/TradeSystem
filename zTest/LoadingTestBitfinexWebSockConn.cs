using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.IO;

using WebSocketSharp;

using Common.Utils;

using TradingLib;
using TradingLib.Enums;
using TradingLib.Data;

using BitfinexCommon.Messages.Response;

using BitfinexWebSockConnector;
using BitfinexWebSockConnector.Interfaces;


namespace zTest
{
    public class LoadingTestBitfinexWebSockConn : IClientBfxWebSockCon
    {
        public void Test()
        {

            CBitfenixWebSockConnector bfxWebSock = new CBitfenixWebSockConnector(this,
                                                                                 GenListInstrument(),
                                                                                 true,
                                                                                 "fVgyf0Rk4hDDDdXAzys7yN0vnGcRPUVYRTLoOxTyDIL",
                                                                                 "MDqroztPvZzFIaKKspozdyeAD274OFAZnEZy2nv3eUE");


            ThreadPool.SetMinThreads(100, 100);
            //  System.Threading.Thread.Sleep(60000);

            //string path = @"e:\ATFS\Logs\TradeSystemCrypto\2018_06_27\BfxRaw_public___07_32_41.txt";
            string path = @"e:\temp993\1.txt";

            string [] lines = File.ReadAllLines(path);

            DateTime dtStart = DateTime.Now;
            Console.WriteLine("started"+ CUtilTime.GetDateTimeString(dtStart));
            for (int i=0; i<lines.Length; i++)
            {
                if (lines[i].Contains("== STARTED ==="))
                    continue;
                string msg = lines[i].Remove(0, 27);



                bfxWebSock.ProcessData(msg);

            }

            DateTime dtEnd = DateTime.Now;
            double sec = (dtEnd - dtStart).TotalSeconds;

            Console.WriteLine("Sec="+sec);


            Console.ReadKey();
        }

        private List<CCryptoInstrData> GenListInstrument()
        {

            List<CCryptoInstrData> lstInstruments = new List<CCryptoInstrData>();

            lstInstruments.Add(new CCryptoInstrData { Instrument = "BTCUSD", DecimalVolume = 3 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "LTCUSD", DecimalVolume = 1 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "ETHUSD", DecimalVolume = 2 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "ETCUSD", DecimalVolume = 1 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "RRTUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "ZECUSD", DecimalVolume = 2 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "XMRUSD", DecimalVolume = 1 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "DSHUSD", DecimalVolume = 2 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "XRPUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "IOTUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "EOSUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "SANUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "OMGUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "BCHUSD", DecimalVolume = 2 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "NEOUSD", DecimalVolume = 1 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "ETPUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "QTMUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "AVTUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "EDOUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "BTGUSD", DecimalVolume = 1 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "DATUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "QSHUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "YYWUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "GNTUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "SNTUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "BATUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "MNAUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "FUNUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "ZRXUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "TNBUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "SPKUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "TRXUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "RCNUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "RLCUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "AIDUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "SNGUSD", DecimalVolume = 0 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "REPUSD", DecimalVolume = 1 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "ELFUSD", DecimalVolume = 0 });

            return lstInstruments;
        }

       public void UpdateStockBothDir(string instrument, int precision, CSharedStocks stock)
       {

       }


        public void UpdateStockOneDir(string instrument, Direction dir, int precision, CSharedStocks stock)
        {

        }



        public void UpdateDeal(string instrument, CRawDeal rd)
        {
        }



        public void Error(string msg, Exception exc = null)
        {


        }

        public void ProcessOrder(ResponseOrders respOrders, EnmOrderAction ordAction)
        {


        }
        public void UpdateUserDeals(ResponseTrades rt)
        {


        }

        public void UpdateUserDealsLateUpd(ResponseTrades rt)
        {

        }

        public void UpdatePos(ResponsePositions respPos)
        {

        }

        public void PeriodicActBfxAuth()
        {


        }

        public void UpdateWallet(string walletType, string currency, decimal balance)
        {
        }

        public List<int> GetPricePrecisions()
        {
            return new List<int> { 0, 1, 2, 3 };
        }

        public int GetStockDepth(int precission)
        {
            throw new NotImplementedException();
        }




    }
}
