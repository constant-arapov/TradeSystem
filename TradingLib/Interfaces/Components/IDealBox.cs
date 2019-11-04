using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Data;
using TradingLib.Bots;

namespace TradingLib.Interfaces.Components
{
    public interface IDealBox
    {
        Dictionary<string, CDealsStruct> DealsStruct { get;  }
		//void UpdateDeals (DEALS.deal dealInp);
		//void UpdateDeals(string instrument, AstsCCTrade.ALL_TRADES at);
		void UpdateAllDealStructLastData();
      
    }
}
