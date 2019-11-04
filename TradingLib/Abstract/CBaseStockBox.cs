using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common.Logger;

using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Clients;
using TradingLib.Data;




namespace TradingLib.Abstract
{
	public abstract class CBaseStockBox
	{

		//protected abstract void CreateStocksStructDict(int stockDepth);




		protected IClientStockBox _client;
		CLogger m_logger;



		public CBaseStockBox(IClientStockBox client, int stockDepth)
		{
			_client = client;


			m_logger = new CLogger("Stockbox");
		}


		public abstract CBaseStockConverter GetStockConverter(string instrument);






		public void Log(string message)
		{
			m_logger.Log(message);

		}


		public void Error(string description, Exception exception = null)
		{

			_client.Error(description, exception);

		}




	}
}
