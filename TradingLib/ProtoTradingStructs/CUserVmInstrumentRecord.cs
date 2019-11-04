using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace TradingLib.ProtoTradingStructs
{

    [ProtoContract]
    public class CUserVmInstrumentRecord
    {

        [ProtoMember(1)]
        public string Isin { get; set; }

        [ProtoMember(2)]
        public decimal VM { get; set; }



    }
}
