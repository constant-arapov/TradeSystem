using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtoBuf;

using TradingLib.Interfaces.Keys;
using TradingLib.ProtoTradingStructs.TradeManager;

namespace TradingLib.ProtoTradingStructs
{
    [ProtoContract]
    public class CClientInfo  : CBaseTrdMgr_StockExch_BotId, IKey_TraderName
    {
        [ProtoMember(1)]
        public string Version { get; set; }


        //for future aims
        [ProtoMember(2)]
        public int Instance { get; set; }

        [ProtoMember(3)]
        public int ConId { get; set; }
    
        [ProtoMember(4)]
        public DateTime DtConnection { get; set; }

        [ProtoMember(5)]
        public string Ip { get; set; }



        public string TraderName { get; set; }



    }
}
