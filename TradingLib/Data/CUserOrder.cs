using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Enums;

namespace TradingLib.Data
{
	public class CUserOrder
	{

		public int BotId { get; set; }

		public int StockExchId { get; set; }

		public long OrderId { get; set; }

		public string Instrument { get; set; }

		//TODO enum
		public  EnmOrderAction OrderActionLast { get; set; }


		public string Status { get; set; }


		public decimal Amount { get; set; }

		public int AmountOriginal { get; set; }


		public int AmountRest { get; set; }

		//TODO status id etc.....

		public CUserOrder()
		{


		}

		




	}
}
