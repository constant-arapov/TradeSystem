using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Enums;

using ProtoBuf;


namespace TradingLib.ProtoTradingStructs
{
    [ProtoContract]

    public class CAddOrder
    {

        [ProtoMember(1)]
        public int IdTrader { get; set; }
        
        [ProtoMember(2)]
        public string Isin { get; set; }

        [ProtoMember(3)]
        public EnmOrderDir Dir { get; set; }


        [ProtoMember(4)]
        public decimal Price { get; set; }


        [ProtoMember(5)]
        public decimal Amount { get; set; }


    }
       
}
