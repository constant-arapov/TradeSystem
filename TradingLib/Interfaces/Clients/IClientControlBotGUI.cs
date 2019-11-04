using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.GUI;

namespace TradingLib.Interfaces.Clients
{
	public interface IClientControlBotGUI : IClientWindowManualTrading, IBotOerations
	{
		CGUIBox GUIBox { set; get; }
	}
}
