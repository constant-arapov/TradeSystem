using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeManager.ViewModels
{
    public class VMStockExch : CBasePropertyChangedAuto
    {
        [Magic]
        public int StockExchId { get; set; }


        [Magic]
        public string StockExchName { get; set; }

    }
}
