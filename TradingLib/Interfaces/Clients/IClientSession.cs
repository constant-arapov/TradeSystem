using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Interfaces.Clients;
using TradingLib;


using TradingLib.GUI;

namespace TradingLib.Interfaces.Clients
{
    public interface IClientSession
    {
        CGUIBox GUIBox { get; set; }
    }
}
