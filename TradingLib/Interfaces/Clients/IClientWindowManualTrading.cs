using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib.Data.DB;
using TradingLib.Bots;

namespace TradingLib.Interfaces.Clients
{
	public interface IClientWindowManualTrading : IAlarmable
	{
		 Dictionary<long, CBotBase> DictBots { get; set; }
	     CListInstruments Instruments { get; set; }
		 int GetGUITradeSystemVolume(string instrument, string inpVolume);



	}
}
