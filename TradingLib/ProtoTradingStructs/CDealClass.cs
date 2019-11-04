using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;
using TradingLib.Enums;

namespace TradingLib.ProtoTradingStructs
{

  [ProtoContract]

    public class CDealClass
    {
       [ProtoMember(1)]
        public long Amount { get; set; }
       
       [ProtoMember(2)]
        public decimal Price { get; set; }

        [ProtoMember(3)]
        public EnmDealDir DirDeal { get; set; }

        [ProtoMember(4)]
        public DateTime DtTm { get; set; }

     /* //TO DO move to parent class (fix problem with char encoding)
        [ProtoMember(5)]
        public string  Isin { get; set; }

        [ProtoMember(6)]
        public long Id { get; set; }
      */
    }
}
