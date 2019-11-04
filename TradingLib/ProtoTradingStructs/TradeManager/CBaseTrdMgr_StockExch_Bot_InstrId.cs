using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;


namespace TradingLib.ProtoTradingStructs.TradeManager
{   
    [ProtoContract]
    //Add include for all StockExchId-BotId-Instument child classes here
    //end user classes
    [ProtoInclude(401, typeof(CBotPosTrdMgr))]
    public class CBaseTrdMgr_StockExch_Bot_InstrId : CBaseTrdMgr_StockExch_BotId
    {

        [ProtoMember(1)]
        public string Instrument { get; set; }

    }
}
