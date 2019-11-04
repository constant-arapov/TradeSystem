using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;

using Common.Interfaces;
using TradingLib;

using TradingLib.Interfaces.Components;
using TradingLib.GUI;

namespace TradingLib.Interfaces.Clients
{
    public interface IClientStockConverter : IAlarmable, IClientSharedStock
    {
        ISnapshoterStock SnapshoterStock { get; set; }
        IUserDealsPosBox UserDealsPosBox { get;  }
        CGlobalConfig GlobalConfig { get; }    
        CGUIBox GUIBox { get; set; }
        List<int> GetPricePrecisions();
        decimal GetMinStep(string instrument);
        int GetDecimals(string instrument);



    }
}
