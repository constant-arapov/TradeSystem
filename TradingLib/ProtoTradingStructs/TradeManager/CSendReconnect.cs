using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtoBuf;


namespace TradingLib.ProtoTradingStructs.TradeManager
{

    [ProtoContract]
    public class CSendReconnect
    {

        public int StockExchId { get; set; }

        [ProtoMember(1)]
        public int ConnectionId { get; set; }

    }
}
