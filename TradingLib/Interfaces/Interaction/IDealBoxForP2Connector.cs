using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Data;

namespace TradingLib.Interfaces.Interaction
{
	public interface IDealBoxForP2Connector
	{
		void UpdateDeals(DEALS.deal dealInp);
		void UpdateDeals(string instrument, AstsCCTrade.ALL_TRADES at);
		void SimUpdateDeals(CRawDeal dealInp);
	}
}
