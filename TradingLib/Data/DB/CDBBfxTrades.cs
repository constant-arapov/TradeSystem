using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLib.Data.DB
{
    public class CDBBfxTrades 
    {
        public long Id { get; set; }
        public string Pair { get; set; }
        public long MtsCreate { get; set; }
        public long OrderId { get; set; }
        public decimal ExecAmount { get; set; }
        public decimal ExecPrice { get; set; }
   
        public decimal OrderPrice { get; set; }
        public int Maker { get; set; }
        public decimal Fee { get; set; }
        public string FeeCurrency { get; set; }

        public long BotId { get; set; }
        public DateTime DtCreate { get; set; }

    }
}
