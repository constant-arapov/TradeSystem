using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeManager.Interfaces.Keys
{
    public interface IKey_StockExch_Inst : IKey_StockExch
    {
        string Instrument { get; set; }

    }
}
