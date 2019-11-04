using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;



namespace TradingLib.ProtoTradingStructs.TradeManager
{
    
    [ProtoContract]
    //Add include for all StockExchId-BotId child classes here
    //next "child base class"
    [ProtoInclude(201, typeof(CBaseTrdMgr_StockExch_Bot_InstrId))]
    //end user-classes
    [ProtoInclude(202, typeof(CBotStatus))]
	[ProtoInclude(203, typeof(CTraderInfoSummary))]
    [ProtoInclude(204, typeof(CClientInfo))]


    public class CBaseTrdMgr_StockExch_BotId : CBaseTrdMgr_StockExchId
    {
        [ProtoMember(1)]
        public int  BotId { get; set; }
    }
}
