using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terminal.TradingStructs
{
    public class CStockPosition
    {        

        public int Amount { get; set; }
        public bool IsEmpty { get; set; }
        public double Price { get; set; }
    }
}
