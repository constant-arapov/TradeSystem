using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Data;
using TradingLib.Data.DB;
using TradingLib.ProtoTradingStructs;


namespace TradingLib.Data.DB
{
    public class CDBUserDeal : CUserDeal
    {

        public string Instrument { get; set; }
        public int account_trade_Id { get; set; }
        public int stock_exch_id { get; set; }
        public long DealId { get; set; }


    }
}
