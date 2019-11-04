using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;


using Common.Interfaces;

using TradingLib.Data.DB;
using TradingLib.Bots;


namespace TradingLib.Interfaces.Clients
{
	public interface IClientStockBox : IClientStockStruct, IClientStockConverter, IAlarmable
	{
		CListInstruments Instruments { get; set; }
		List<CBotBase> ListBots { get; set; }
        ManualResetEvent EvStockOnline { get; set; }
        bool IsStockOnline { get; set; }
	}
}
