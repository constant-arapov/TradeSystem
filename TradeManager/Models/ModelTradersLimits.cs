using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using TradeManager.Interfaces.Keys;

namespace TradeManager.Models
{
    public class ModelTradersLimits : IKey_Server
    {
        public int ServerId { get; set; }
        public string ShortNameDB { get; set; }
        public int StockExchId { get; set; }
        public int number { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public decimal MaxLossVMClosedTotal { get; set; }
        public decimal proc_profit { get; set; }
        public decimal proc_fee_dealing { get; set; }



    }
}
