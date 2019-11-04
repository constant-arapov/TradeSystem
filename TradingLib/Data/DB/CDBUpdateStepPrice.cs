using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLib.Data.DB
{
    public class CDBUpdateStepPrice
    {
        public int StockExchId { get; set; }
        public string Instrument { get; set; }
        public decimal NewStepPrice { get; set; }
        


    }
}
