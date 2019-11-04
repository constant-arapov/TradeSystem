using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Enums;

namespace TradingLib.Bots
{
	public class COrderThrowData
	{
		public CBotPos PosPrev = new CBotPos();
		public decimal Price;
		public decimal Amount;
		public EnmOrderDir Dir;


	}
}
