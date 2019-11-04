using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

using TradingLib.ProtoTradingStructs.TradeManager;


namespace TradingLib.ProtoTradingStructs.TradeManager
{
    [ProtoContract]
    public class CPositionInstrTotal : CBaseTrdMgr_StockExch_InstrId
    {
    

        [ProtoMember(1)]
        public decimal Pos { get; set; }

    }
}
