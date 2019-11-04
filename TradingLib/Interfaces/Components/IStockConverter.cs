using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Interfaces.Components
{
	public interface IStockConverter
	{
		decimal GetBid();
		decimal GetAsk();

	}
}
