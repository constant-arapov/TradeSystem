using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace Common
{
    //TODO REMOVE
    [ProtoContract]
    public class CTestMessageClass
    {
        [ProtoMember(1)]
        public DateTime DtCurrentTime { get; set; }

        [ProtoMember(2)]
        public byte[] BuffDummy { get; set; }


        public CTestMessageClass()
        {


            BuffDummy = new byte[1];
        }
    }
}
