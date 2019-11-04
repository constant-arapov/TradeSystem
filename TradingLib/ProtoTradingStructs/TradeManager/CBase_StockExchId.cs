using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace TradingLib.ProtoTradingStructs.TradeManager
{
    
    [ProtoContract]
    //Add include for all StockExchId child classes here
    //next "child base class"
    [ProtoInclude(101, typeof(CBaseTrdMgr_StockExch_BotId))]
    [ProtoInclude(102, typeof(CBaseTrdMgr_StockExch_InstrId))]
    
    //end user-classes
    [ProtoInclude(110, typeof(CListPositionInstrTotal))]
    [ProtoInclude(111, typeof(CListBotPosTrdMgr))]
    [ProtoInclude(112, typeof(CListBotStatus))]
    [ProtoInclude(113, typeof(CListClientInfo))  ]
  

    public class CBaseTrdMgr_StockExchId
    {

        [ProtoMember(1)]
        public int StockExchId { get; set; }


    }
}
