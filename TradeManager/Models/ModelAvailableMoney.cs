using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeManager.Models
{
	public class ModelAvailableMoney
	{
		public int ServerId { get; set; }
		public string ShortNameDB { get; set; }
		public int id { get; set; }
		public string name { get; set; }
        public string email { get; set; }
		public decimal money_avail { get; set; }
	}
}
