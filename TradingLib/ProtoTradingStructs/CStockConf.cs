using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtoBuf;

namespace TradingLib.ProtoTradingStructs
{
    [ProtoContract]
    public class CStockConf
    {
        public CStockConf()
        {

        }
            
        [ProtoMember(1)]
        public int PrecissionNum { get; set; }

        [ProtoMember(2)]
        public decimal MinStep { get; set; }

        [ProtoMember(3)]
        public int DecimalsPrice { get; set; }
    }
}
