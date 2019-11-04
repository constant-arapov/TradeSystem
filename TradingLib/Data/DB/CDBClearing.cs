using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Data.DB
{
    public class CDBClearing
    {
        public int stock_exchange_id { get; set; }
        public int IsAutomatic { get; set; }
        public int SessionId { get; set; }
        public DateTime DtClearingBegin { get; set; }
        public DateTime DtClearingEnd { get; set; }


    }
}
