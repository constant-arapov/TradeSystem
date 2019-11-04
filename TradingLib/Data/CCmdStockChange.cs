using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtoBuf;

using TradingLib.Enums;

namespace TradingLib.Data
{
    [ProtoContract]
    public class CCmdStockChange
    {      
        [ProtoMember(1)]
        public EnmStockChngCodes  Code {get;set;}

        [ProtoMember(2)]
        public decimal Price { get; set; }

        [ProtoMember(3)]
        public long Volume { get; set; }

    }
}
