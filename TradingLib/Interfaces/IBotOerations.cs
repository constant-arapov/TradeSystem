using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Bots;


namespace TradingLib.Interfaces
{
	public interface IBotOerations
	{
		 void DisableBot(long id);
		 void EnableBot(long id);
		 void UnloadBot(long id);
		 void LoadBot(long id);
		 void Error(string description, Exception exception = null);
		 List<CBotBase> ListBots { get; set; }

	}
}
