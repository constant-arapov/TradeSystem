using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;


namespace TradingLib.ProtoTradingStructs.TradeManager
{
    [ProtoContract]
    public class CCloseBotPosTrdMgr
    {
     
        [ProtoMember(1)]
        public int BotId { get; set; }


        [ProtoMember(2)]
        public string Instrument { get; set; }


        public int StockExchId { get; set; }

    }
}
