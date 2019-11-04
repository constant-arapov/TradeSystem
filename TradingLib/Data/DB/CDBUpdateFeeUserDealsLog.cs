using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLib.Data.DB    
{
    public class CDBUpdateFeeUserDealsLog
    {
        public long DealId { get; set; }
        public decimal Fee { get; set; }
        public decimal FeeDealing { get; set; }
        public decimal FeeStock { get; set;}
     

    }
}
