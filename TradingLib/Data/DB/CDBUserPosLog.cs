using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.ProtoTradingStructs;


namespace TradingLib.Data.DB
{
    public class CDBUserPosLog : CUserPosLog 
    
    {
        public string Instrument { get; set; }
        public int account_trade_Id { get; set; }
        public int stock_exch_id { get; set; }
        public long ReplId { get; set; }


       // public long UserId { get; set; }

       // public int broker_account_id { get; set; }
       // public int stock_exchange_id { get; set; }

    }
}
