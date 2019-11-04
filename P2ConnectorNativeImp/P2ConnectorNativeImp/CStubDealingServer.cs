using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using TradingLib;

using P2ConnectorNativeImp.Interfaces;

namespace P2ConnectorNativeImp
{
	public class CStubDealingServer : IP2ConnectorNativeClient
	{

        public bool IsStockOnline { get; set; }

		public void Error(string description, Exception exception = null)
		{
			throw new ApplicationException(description);
		}

      



		public CStubDealingServer()
		{

			
		}

		public void UpdateInpStocks(long isinId, ref CSharedStocks sourceStock)
		{

		}


		public void WaitInstrumentLoaded()
		{
			//no wait just return
		}

		public List<string> GetInsruments()
		{
			throw new ApplicationException("Not impelmented in stub");
		}

		public long GetIsinIdByInstrument(string instrument)
		{
			throw new ApplicationException("Not impelmented in stub");
		}


		public void Process()
		{
			try
			{
				CP2ConnectorNative p = new CP2ConnectorNative(this);
				p.Process();

			}
			catch (Exception e)
			{

			}

		}



	}
}
