using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TradeManager.Interfaces.Keys;


namespace TradeManager.Models
{
    public class ModelTrdAddFundsReq : IKey_Server
    {
        public int ServerId { get; set; }
        public string ShortNameDB { get; set; }
        public int id { get; set; }
        public string TraderName { get; set; }
        public DateTime dt_add { get; set; }
        public string currency {get;set;}
        public decimal  amount { get; set; }
        public int StockExchId { get; set; }

    }
}
