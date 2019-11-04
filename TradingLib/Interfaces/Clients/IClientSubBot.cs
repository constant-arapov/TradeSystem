using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Enums;

namespace TradingLib.Interfaces.Clients
{
    public interface IClientSubBot
    {

        DateTime ServerTime { get; }
		string BotSubDir { get; }
        decimal GetMinOrderSize(string instrument);
        void ResetIsClosingPos(string instrument);
        bool AddMarketOrder(string instrument, EnmOrderDir dir, decimal amount);
        void ClosePositionOfInstrument(string instrument);
    }
}
