using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Data.DB
{
    public class CDBSession
    {

        public int stock_exchange_id  {get;set;}
        public DateTime DtBegin {get;set;}
        public DateTime DtEnd { get; set; }

        public int StockExchangedSessionId { get; set; }

        public int IsCompleted { get; set; }

        public int IsClearingProcessed { get; set; }








    }
}
