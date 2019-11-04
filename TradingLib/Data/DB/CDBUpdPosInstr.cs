using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLib.Data.DB
{
    public class CDBUpdPosInstr
    {
        public int account_id { get; set; }
        public int stock_exch_id { get; set; }
        public string instrument { get; set; }
        public decimal amount { get; set; }
        public decimal AvPos { get; set; }
        public DateTime Dt_upd { get; set; }

    }
}
