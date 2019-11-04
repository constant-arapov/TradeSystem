using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Data.DB
{
    public class CDBUpdateLate
    {
        public string Instrument { get; set; }
        public long ReplId { get; set; }
        public int BotId { get; set; }
        public decimal Fee { get; set; }
        public decimal Fee_Total { get; set; }
        public decimal Fee_Stock { get; set; }
        public decimal FeeDealing { get; set; }

      
        public decimal VMClosed_RUB { get; set; }
        public decimal VMClosed_RUB_user { get; set; }
        public decimal VMClosed_RUB_stock { get; set; }


        public int  IsFeeLateCalced { get; set; }


    }
}
