using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using Common;
using Common.Interfaces;
using Common.Utils;

using TradingLib.TradeManagerServer;
using TradingLib.Interfaces.Clients;

using TradingLib.Data;
using TradingLib.ProtoTradingStructs;


using TCPLib;

using TradeManager.TCPCommu;

namespace zTest.TradeManager
{
   public class TestTradeManagerServer
    {

        public void Test()
        {
            IClientTradeManagerServer dealingServer = new MockTradeManagerServer();


                            

            CTradeManagerServer tradeManagerServer = new CTradeManagerServer(dealingServer);
            tradeManagerServer.Process();
            CUtil.TaskStart(TaskClient);
            

            Console.ReadKey();

        }


        public void TaskClient()
        {

            Thread.Sleep(7000);

            string ip = "127.0.0.1";
            int port = 6000;


            MockTradeMgr trdMgr = new MockTradeMgr();
            CCommuTradeManager commuTrdMgr = new CCommuTradeManager(trdMgr/*, ip, port*/);



			commuTrdMgr.OnUserTryConnectToServer(new UserConReq
			{
				AuthRequest = new CAuthRequest { User = "constant", Password = "ivanov" },
				ConnNum = 0
			});
													

            //_tcpCommu.TryToConnect();
           
           

        }




    }
}
