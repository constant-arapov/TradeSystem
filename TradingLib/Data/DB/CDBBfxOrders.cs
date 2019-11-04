using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLib.Data.DB
{
    public class CDBBfxOrder
    {
        public long Id { get; set; }
        public long Gid { get; set; }        
        public string Symbol { get; set; }
        public long MtsCreate { get; set; }
        public long MtsUpdate { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountOrig { get; set; }
        public string OrderStatus { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAvg { get; set; }
        
        public DateTime DtCreate { get; set; }
        public DateTime DtUpdate { get; set; }

    }
}
