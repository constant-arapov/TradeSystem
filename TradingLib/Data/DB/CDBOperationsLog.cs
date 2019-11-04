using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Data.DB
{
    public class CDBAccountsOperationsLog
    {

        public int account_operation_name_id { get; set; }
        public int account_operation_id {get;set;}
                   
        public DateTime Dt_operation { get; set; }
        public int account_trade_id { get; set; }

        public decimal Money_changed {get;set;}
        public decimal   Money_before {get;set;}
        public decimal Money_after { get; set; }


    }
}
