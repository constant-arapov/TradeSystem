using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeManager.Commands.Data
{
    public class CCmdDataAddInstrument
    {
        public int ServerId { get; set; }
        public int StockExchId { get; set; }
        public string Instrument { get; set; }
    }
}
