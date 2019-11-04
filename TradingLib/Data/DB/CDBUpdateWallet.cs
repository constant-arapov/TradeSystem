using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLib.Data.DB
{
    public class CDBUpdateWallet
    {
        public DateTime Dt { get; set; }
        public string Currency { get; set; }
        public decimal Balance { get; set; }
        public string Wallet_type { get; set;}
    }
}
