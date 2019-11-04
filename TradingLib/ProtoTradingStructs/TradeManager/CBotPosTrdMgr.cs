using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using ProtoBuf;

using TradingLib.Interfaces.Keys;


namespace TradingLib.ProtoTradingStructs.TradeManager
{
    [ProtoContract]   
    public class CBotPosTrdMgr : CBaseTrdMgr_StockExch_Bot_InstrId, IKey_TraderName
    {

             
        [ProtoMember(1)]
        public decimal Amount { get; set; }


        public string TraderName { get; set; }
    }
}
