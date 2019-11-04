using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace TradingLib.ProtoTradingStructs.TradeManager
{
    [ProtoContract]
    //Add include for all StockExchId-BotId-Instument child classes here
    //end user classes 301
    [ProtoInclude(301, typeof(CPositionInstrTotal))]
    public class CBaseTrdMgr_StockExch_InstrId : CBaseTrdMgr_StockExchId
    {
        [ProtoMember(1)]
        public string Instrument { get; set; }
    }
}
