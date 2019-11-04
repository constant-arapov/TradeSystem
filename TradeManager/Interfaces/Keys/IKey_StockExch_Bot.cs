using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeManager.Interfaces.Keys
{
    public interface IKey_StockExch_Bot : IKey_StockExch
    {
        int BotId { get; set; }
    }
}
