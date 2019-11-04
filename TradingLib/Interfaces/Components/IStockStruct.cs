using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Interfaces.Components
{
	public interface IStockStruct
	{
		IStockConverter StockConverter { get; set; }
		bool NeedReInitStock { get; set; }

	}
}
