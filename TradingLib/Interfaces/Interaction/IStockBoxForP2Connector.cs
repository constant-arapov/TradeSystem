using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Enums;
using TradingLib.Data;

namespace TradingLib.Interfaces.Interaction
{
	public interface IStockBoxForP2Connector
	{
		void UpdateStock(CRawStock rs);
		void UpdateStock(string instrument, CRawStock rs);
		void SimUpdateStock(CRawStock rs);

		void ReInitAllStocks();
	}
}
