using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;
using TradingLib.Enums;

namespace TradingLib.ProtoTradingStructs
{
    [ProtoContract]
    public class CSetOrder
    {

        [ProtoMember(1)]
        public string Instrument { get; set; }


        [ProtoMember(2)]
        public EnmOrderTypes OrderType { get; set; }

      
        [ProtoMember(3)]
        public decimal Price {get;set;}


        [ProtoMember(4)]
        public decimal Amount { get; set; }



    }
}
