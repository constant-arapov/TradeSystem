using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using ProtoBuf;

namespace TradingLib.ProtoTradingStructs
{
    [ProtoContract]
    public class CUserPosLogUpdLate
    {
        [ProtoMember(1)]
        public string Instrument { get; set; }

        [ProtoMember(2)]
        public decimal Fee { get; set; }

        [ProtoMember(3)]
        public decimal Fee_Total { get; set; }
        // public decimal Fee_Stock { get; set; }     

        [ProtoMember(4)]
        public decimal VMClosed_RUB { get; set; }

        [ProtoMember(5)]
        public decimal VMClosed_RUB_user { get; set; }



    }
}
