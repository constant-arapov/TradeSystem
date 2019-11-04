using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradeManager.Interfaces.Keys;


namespace TradeManager.Models
{
    public class ModelInstrument : IKey_Server
    {
		public int ServerId { get; set; }
        public string ShortNameDB { get; set; }
        public int stock_exch_id { get; set; } //as it in DB
		public int StockExchId { get; set; }
        public string instrument { get; set; }
		public long Isin_id { get; set; }
		public int IsInitialised { get; set; }
		public int IsViewOnly { get; set; }
		public int Is_GUI_monitoring { get; set; }
		public int Trade_disable_Code {get; set;}
	
    }
}
