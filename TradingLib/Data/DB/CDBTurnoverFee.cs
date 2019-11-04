using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLib.Data.DB
{
    public class CDBTurnoverFee
    {
        public int account_money_id { get; set; }
        public decimal turnover { get; set; }
        public decimal proc_fee_turnover_limit { get; set; }
        public decimal proc_fee_turnover_market { get; set; }

        public CDBTurnoverFee Copy ()
        {
            return (CDBTurnoverFee)  this.MemberwiseClone();

        }

    }
}
