using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLib.Data.DB
{
    public class CDBMoneyTracking
    {
        public DateTime Dt { get; set; }
        public decimal MoneyCurrent { get; set; }
        public decimal DltMoney { get; set; }
        public decimal SessProfit { get; set; }
        public decimal Diff { get; set; }

    }
}
