using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Interfaces.Interaction
{
	public interface IUserdealsPosBoxForP2Connector
	{
		void Update(USER_DEAL.user_deal deal);
	}
}
