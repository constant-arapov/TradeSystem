using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Data.DB.Interfaces;

namespace TradingLib.Data.DB
{
    public class CDBInstrument : IObjectWithId
    {
        
        public int id { get; set; }
        public string instrument { get; set; }
        public string SecName { get; set; }
        public string ShortName { get; set; }
        public long Isin_id { get; set; }
        public int stock_exch_id { get; set; }
        public int IsSubscribed { get; set; }
        public int IsInitialised { get; set; }
        public decimal Min_step { get; set; }
        public decimal Step_price { get; set; }
        public int RoundTo { get; set; }
        public int Is_GUI_monitoring {get;set;}
        public long LotSize { get; set; }
        public int IsShortable { get; set; }
        public long Trade_disable_Code { get; set; }
		public int IsViewOnly { get; set; }
        public decimal minimum_order_size { get; set; }
        public int DecimalVolume { get; set; }
    }
}
