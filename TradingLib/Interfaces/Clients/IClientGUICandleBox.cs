using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib;
using TradingLib.Data.DB;

using TradingLib.GUI;

namespace TradingLib.Interfaces.Clients
{
    public interface IClientGUICandleBox : IAlarmable
    {
        CListInstruments Instruments { get; set; }
        bool IsTimeToInitCandles { get; set; }
        CGUIBox GUIBox { get; set; }
    }
}
