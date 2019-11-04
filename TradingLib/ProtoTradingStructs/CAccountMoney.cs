using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using ProtoBuf;

namespace TradingLib.ProtoTradingStructs
{

     [ProtoContract]
    public class CAccountMoney
    {
        [ProtoMember(1)]
        public Decimal money_avail { get; set; }
        [ProtoMember(2)]
        public Decimal money_sess_limit { get; set; }

    }
}
