using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeManager.Interfaces.Keys
{
	public interface IKey_Server
	{
		int ServerId { get; set; }
        string ShortNameDB { get; set; }

	}
}
