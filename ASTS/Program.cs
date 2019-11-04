using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Common.Interfaces;
using Common;
using Common.Utils;

using TradingLib.Enums;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Interaction;

using ASTS.Common;
using ASTS.Interfaces.Interactions;


using ASTS.Connector;
using ASTS.DealingServer;

namespace ASTS
{
	class Program
	{
		static void Main(string[] args)
		{
			TestASTSDealingServer();
			//TestASTSSingleConnection();
			//TestASTSDoubleConnection();
		}
		static void TestASTSDealingServer()
		{
			IAlarmable alarmer = new StubAlarmer();
			CASTSDealingServer dealingServer = new CASTSDealingServer(/*alarmer*/);
            dealingServer.Process();



			while (true)
				Thread.Sleep(100);
		}

		static void TestASTSSingleConnection()
		{
			IAlarmable alarmer = new StubAlarmer();
			IDealingServerForASTSConnector dealingServer = new CASTSDealingServer(/*alarmer*/);
			IStockConnector stockConnector = new CASTSConnectorSingle(dealingServer);
			int botId = 100;

			//Thread.Sleep(10000);
			//stockConnector.AddOrder(botId, "AFLT", 157.70M, EnmOrderDir.Buy, 1);
			//stockConnector.CancelOrder(2064814799, botId);
			//stockConnector.CancelOrder(2064814740, botId);
            
            CUtil.ThreadStart(new Action(() => 
            {                
                    Thread.Sleep(20000);
                   stockConnector.DisconnectFromServer();                               
            }
            ));


            while (true)
            {
                Thread.Sleep(60000);
             

            }


         


         

		}




		static void TestASTSDoubleConnection()
		{
			IAlarmable alarmer = new StubAlarmer();
			IDealingServerForASTSConnector dealingServer = new CASTSDealingServer(/*alarmer*/);
			IStockConnector stockConnector = new CASTSConnectorDouble(dealingServer);
			int botId = 100;

			Thread.Sleep(10000);
			//stockConnector.AddOrder(botId, "AFLT", 157.70M, EnmOrderDir.Buy, 1);
			//stockConnector.AddOrder(botId, "AFLT", 157.80M, EnmOrderDir.Buy, 1);
			stockConnector.CancelOrder(2063436210, botId);
			stockConnector.CancelOrder(2063436211, botId);


			while (true)
				Thread.Sleep(0);


		}



	}
}
