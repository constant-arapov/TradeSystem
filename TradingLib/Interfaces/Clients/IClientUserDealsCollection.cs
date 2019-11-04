using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Bots;

namespace TradingLib.Interfaces.Clients
{
    public interface IClientUserDealsCollection
    {
        Dictionary<long, CBotBase> DictBots { get; set; }
    }
}
