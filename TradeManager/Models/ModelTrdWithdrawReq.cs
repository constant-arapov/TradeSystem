using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using TradeManager.Interfaces.Keys;

namespace TradeManager.Models
{
    public class ModelTrdWithdrawReq 
    {
        public int id { get; set; }

        public int ServerId { get; set; }
        public string ShortNameDB { get; set; }

        public int StockExchId { get; set; }
        public string TraderName { get; set; }

        public int account_trade_id { get; set; }
        public DateTime dt_add { get; set; }
        public decimal amount { get; set; }
        public string walletId { get; set; }
    }
}
