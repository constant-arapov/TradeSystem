using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Enums;

namespace TradeManager.Commands.Data
{
	public class CCmdDataAddWithdrawMoney
	{
		public int ServerId { get; set; }
		public EnmAccountOperations Operation_code { get; set; }
		public int BotId { get; set; }
		public decimal MoneyChanged { get; set; }
	}
}
