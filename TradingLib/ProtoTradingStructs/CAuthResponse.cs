using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace TradingLib.ProtoTradingStructs
{
    [ProtoContract]
    public class CAuthResponse
    {

      
        [ProtoMember(1)]
        public string ErrorMessage { get; set; }
    
        [ProtoMember(2)]
        public bool IsSuccess { get; set; }

        public CAuthResponse()
        {


        }


    }
}
