using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Data.DB.Interfaces;


namespace TradingLib.Data.DB
{
    public class CDBAccountTrade : IAccountMoney
    {
        public int id { get; set; }
        public int accounts_money_id { get; set; }                   
        public int stock_exchange_id { get; set; }
        public Decimal money_sess_limit { get; set; }
        public Decimal money_avail { get; set; }
        public Decimal leverage {get; set;}
        public decimal proc_profit { get; set; }
        public Decimal proc_fee_dealing { get; set; }


        public string name { get; set; }

        public decimal proc_fee_turnover_limit { get; set; }

        public decimal proc_fee_turnover_market { get; set; }

        public int GetId()
        {
            return accounts_money_id;
        }

    }
}
