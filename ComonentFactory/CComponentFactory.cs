using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Interfaces;

using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Data.DB;
using TradingLib.GUI;

using DBCommunicator;
using DBCommunicator.Interfaces;

using ReportDispatcher;
using Messenger;


namespace ComonentFactory
{
	public class CComponentFactory
	{
		private ILogable _logger;


		public CComponentFactory(ILogable logger)
		{
			_logger = logger;				
		}

   
		public void Create(string databaseName, IClientComponentFactory dealingServer)
		{
			Log("Create Messenger");
			dealingServer.Messenger = new CMessenger();


			Log("Create DBCommunicator");
            //note: DBCommunicator creates parrallely
			dealingServer.DBCommunicator = new CDBCommunicator(databaseName, (IClientDBCommunicator)dealingServer);

            //2017-07-01 Why do we need use UseRealServer for create ReportDispatcher ?
			//if (dealingServer.GlobalConfig.UseRealServer)
			{
				Log("Create ReportDispatcher");
				dealingServer.ReportDispatcher =
						new CReportDispatcher(dealingServer,                                             
												dealingServer.DBCommunicator,
												dealingServer.GlobalConfig.UseRealServer,
                                                ((IClientDBCommunicator)dealingServer).StockExchId);

			}

            Log("Create Instruments");
			dealingServer.Instruments = new CListInstruments(dealingServer, dealingServer.GlobalConfig);

//            Log("Create GUIBox");
  //          dealingServer.GUIBox = new CGUIBox((IClientGUIBox)  dealingServer);


			
		}

		public void Log(string message)
		{
			_logger.Log(message);
		}


	}
}
