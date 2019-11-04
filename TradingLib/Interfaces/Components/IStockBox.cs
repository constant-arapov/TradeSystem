using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Abstract;
using TradingLib.Enums;
using TradingLib.Data;

namespace TradingLib.Interfaces.Components
{
    public interface IStockBox
    {
        //Dictionary<string, CStockStruct> StocksStructDict { get; }
		bool IsStockAvailable(string instrument);
		CBaseStockConverter GetStockConverter(string instrument);

        decimal GetBestPice(string instrument, EnmOrderDir ordDir);
		decimal GetBid(string instrument);
		decimal GetAsk(string instrument);
		
		
    }
}
