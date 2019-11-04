using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Data.DB
{
    public  class CDBPayout
    {
        public DateTime Dt { get; set; }
        public decimal payout { get; set; }
        public int clearing_calced_vm_id { get; set; }


    }
}
