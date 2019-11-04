using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeManager.Models
{
	public class ModelServer
	{
        public int ConId { get; set; }

		public string Ip { get; set; }
		public long Port { get; set; }
		public bool IsAvailable { get; set; }
        public string Name { get; set; }


	}
}
