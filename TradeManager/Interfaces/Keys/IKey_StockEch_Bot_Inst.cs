using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeManager.Interfaces.Keys
{
    public interface IKey_StockEch_Bot_Inst : IKey_StockExch_Bot
    {
        string Instrument { get; set; }
    }
}
