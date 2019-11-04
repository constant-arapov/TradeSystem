using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;


using Common;

namespace TradingLib.ProtoTradingStructs
{
    [ProtoContract]
    public class CStock : CClone
    {
        [ProtoMember(1)]
        public decimal Price { get; set; }

        [ProtoMember(2)]
        public long Volume { get; set; }
        //  public long ReplId { get; set; }


        public CStock(decimal price, long volume)
        {
            Price = price;
            Volume = volume;

        }

        public CStock()
        {

        }

    }
}
