using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.Data;

namespace TradingLib.Interfaces.Interaction
{
	public interface IUserOrdersBoxForP2Connector
	{
		void Update(FUTTRADE.orders_log ol);
	}
}
