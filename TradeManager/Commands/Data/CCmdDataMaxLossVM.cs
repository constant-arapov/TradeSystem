using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradeManager.ViewModels;

namespace TradeManager.Commands.Data
{
	public class CCmdDataMaxLossVM
	{
		public int ServerId { get; set; }
		public VMTradersLimits TradersLims { get; set; }
		public decimal NewLim { get; set; }
	}
}
