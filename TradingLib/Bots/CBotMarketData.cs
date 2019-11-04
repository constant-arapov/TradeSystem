using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Bots
{
    public class CBotMarketData
    {
        public decimal LowDayPrice {get;set;}
        public decimal HighDayPrice {get;set;}
        public long    CurrentVolume { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }

    }
}
