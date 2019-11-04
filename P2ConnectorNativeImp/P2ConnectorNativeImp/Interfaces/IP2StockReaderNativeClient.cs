using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib;

namespace P2ConnectorNativeImp.Interfaces
{
	public interface IP2StockReaderNativeClient
	{
		void UpdateStockReieved(long isinId);
     
	}
    
}
