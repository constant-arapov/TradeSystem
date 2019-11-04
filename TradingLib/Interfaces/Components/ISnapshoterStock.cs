using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.Enums;



namespace TradingLib.Interfaces.Components
{
    public interface ISnapshoterStock
    {
        Dictionary<string, CSharedStocks> OutputStocks { get;}
		void UpdateInpStocksBothDir(string isin, ref CSharedStocks sourceStock, int precision);
        void UpdateInpStocksOneDir(string isin, ref CSharedStocks sourceStock, Direction dir, int precision);

        void UpdateOutStock(string instrument, bool bDoNotUpdateTraders);
    }
}
