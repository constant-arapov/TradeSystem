using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib;
using TradingLib.GUI;
using TradingLib.Data;


namespace TradingLib.Interfaces.Clients
{
    public interface IClientGUIBot
    {

        bool IsOnlineUserDeals { get; set; }
       bool IsOnlineUserOrderLog { get; set; }

        CGUIBox GUIBox { get; set; }
        Dictionary<string, CInstrument> DictInstruments { get; set; }
      
    }
}
