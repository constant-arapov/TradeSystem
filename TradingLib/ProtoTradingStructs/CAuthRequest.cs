using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;


namespace TradingLib.ProtoTradingStructs
{

    [ProtoContract]
    public class CAuthRequest
    {
        [ProtoMember(1)]
        public string User { get; set; }


        //TODO hash
        [ProtoMember(2)]
        public string Password { get; set; }







    }
}
