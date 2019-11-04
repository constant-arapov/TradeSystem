using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib;


namespace P2ConnectorNativeImp.Interfaces
{
	public interface IP2ConnectorNativeClient  : IAlarmable
	{

		void WaitInstrumentLoaded();
		List<string> GetInsruments();
		long GetIsinIdByInstrument(string instrument);
        void UpdateInpStocks(long isinId, ref CSharedStocks sourceStock);
        bool IsStockOnline { get; set; }

	}
}
