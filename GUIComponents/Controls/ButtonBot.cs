using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;

using TradingLib.Interfaces;


namespace GUIComponents.Controls
{
	public class ButtonBot : Button
	{


		public int Id { get; set; }
		public IBotOerations BotOperations;

	}
}
