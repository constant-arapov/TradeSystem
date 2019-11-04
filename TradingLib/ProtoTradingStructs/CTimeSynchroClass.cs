using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;



namespace TradingLib.ProtoTradingStructs
{
    [ProtoContract]
    public class CTimeSynchroClass
    {
        [ProtoMember(1)]
        public DateTime DtCurrentTime { get; set; }


    }


}
