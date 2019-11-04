using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;


namespace TradingLib.ProtoTradingStructs
{
    [ProtoContract]
    public class CUserPos
    {        
        [ProtoMember(1)]
        public decimal Amount { get; set; }
        [ProtoMember(2)]
        public decimal AvPos { get; set; }


    }
}
