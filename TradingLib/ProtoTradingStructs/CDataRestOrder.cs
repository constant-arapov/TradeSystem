using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

using TradingLib.Enums;




namespace TradingLib.ProtoTradingStructs
{
    [ProtoContract]
    public class CDataRestOrder
    {
        [ProtoMember(1)]
        public decimal Price { get; set; }

        [ProtoMember(2)]
        public EnmOrderDir Dir { get; set; }

        [ProtoMember(3)]
        public string Instrument { get; set; }




    }
}
