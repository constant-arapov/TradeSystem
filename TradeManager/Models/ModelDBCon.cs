using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeManager.Models
{
	public class ModelDBCon
	{
		public int ServerId { get; set; }
		public string Host { get; set; }
		public long Port { get; set; }

        public string DatabaseName { get; set; }
	
		public string ShortNameDB { get; set; }
		public bool IsSelected { get; set; }
		public List<int> LstAvailStockExh { get; set; }
	}
}
