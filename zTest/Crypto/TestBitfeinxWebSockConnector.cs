using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using System.Threading;

using Common.Interfaces;


using TradingLib;
using TradingLib.Enums;
using TradingLib.Data;


using BitfinexCommon.Messages.Response;

using BitfinexWebSockConnector;
using BitfinexWebSockConnector.Interfaces;

using BitfinexWebSockConnector.Messages.Response;


namespace zTest.Crypto
{
    public class TestBitfeinxWebSockConnector : IClientBfxWebSockCon
	{


		public void Test()
		{
            TestAuthResp();


			CBitfenixWebSockConnector bfxWebSock = new CBitfenixWebSockConnector(this,                            
                                                                                 GenListInstrument(),
                                                                                 true,
                                                                                 "fVgyf0Rk4hDDDdXAzys7yN0vnGcRPUVYRTLoOxTyDIL",
                                                                                 "MDqroztPvZzFIaKKspozdyeAD274OFAZnEZy2nv3eUE");
            bfxWebSock.Process();
			Thread.Sleep(300000000);
           // bfxWebSock.AddOrder(101,"tEDOUSD", TradingLib.Enums.EnmOrderDir.Buy, 4M, 1.2M);

            //bfxWebSock.AddMarketOrder(100, "tIOTUSD", TradingLib.Enums.EnmOrderDir.Buy, 6M, 1.69M);

          //  bfxWebSock.AddMarketOrder(100, "tIOTUSD", TradingLib.Enums.EnmOrderDir.Sell, 6M, 1.29M);
            //bfxWebSock.CancellOrder(7984743425);


            /*
            while (!bfxWebSock.IsConnected)
                Thread.Sleep(100);


            for (int i = 0; i < 30; i++)
            {
                bfxWebSock.SendPing(i);
               // Thread.Sleep(10);

            }*/
            Thread.Sleep(10000);

		}

        private void TestAuthResp()
        {
            string msg = "{\"event\":\"auth\",\"status\":\"OK\",\"chanId\":0,\"userId\":1781587,\"auth_id\":\"b3654b77-d30a-4832-9f3f-cbd42bb4dc50\",\"caps\":{\"orders\":{\"read\":1,\"write\":1},\"account\":{\"read\":1,\"write\":0},\"funding\":{\"read\":1,\"write\":1},\"history\":{\"read\":1,\"write\":0},\"wallets\":{\"read\":1,\"write\":1},\"withdraw\":{\"read\":0,\"write\":1},\"positions\":{\"read\":1,\"write\":1}}}";
            var obj = CBitfinexJsonSerializer.DeserializeObject<ResponseAuth>(msg);
            if (obj != null)
                System.Threading.Thread.Sleep(0);

        }


        //TODO get from dealing server
        private List<CCryptoInstrData> GenListInstrument()
        {

            List<CCryptoInstrData> lstInstruments = new List<CCryptoInstrData>();

            lstInstruments.Add(new CCryptoInstrData { Instrument = "BTCUSD", DecimalVolume = 3 });
            lstInstruments.Add(new CCryptoInstrData { Instrument = "LTCUSD", DecimalVolume = 2 });
            /*lstInstruments.Add("tLTCUSD");
            lstInstruments.Add("tETHUSD");
            lstInstruments.Add("tZECUSD");
            lstInstruments.Add("tXMRUSD");
            //_lstInstruments.Add("tDASHUSD");
            //_lstInstruments.Add("tIOTAUSD");
            lstInstruments.Add("tEOSUSD");
            lstInstruments.Add("tSANUSD");
            lstInstruments.Add("tOMGUSD");
            lstInstruments.Add("tBCHUSD");
            lstInstruments.Add("tNEOUSD");
            //  _lstInstruments.Add("tUTPUSD");
            //_lstInstruments.Add("tQTUMUSD");
            lstInstruments.Add("tEDOUSD");
            lstInstruments.Add("tAVTUSD");*/

            return lstInstruments;


        }

        public void UpdateStockBothDir(string instrument,int precision, CSharedStocks stock)
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

        public void  UpdatePos(ResponsePositions respPos)
        {

        }

        public void PeriodicActBfxAuth()
        {


        }

        public void UpdateWallet(string walletType, string currency, decimal balance)
        {
        }

        public List <int> GetPricePrecisions()
        {
            return new List<int> { 0, 1, 2, 3 };
        }

        public int GetStockDepth(int prec)
        {
            throw new NotImplementedException();
        }



    }
}
