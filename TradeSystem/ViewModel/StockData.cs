using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeSystem.ViewModel
{
    public class StockData
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public int Volume { get; set; }
    }
}
