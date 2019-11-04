using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using ProtoBuf;


namespace TradingLib.ProtoTradingStructs
{
    [ProtoContract]
    public class CUserVMStockRecord
    {
        [ProtoMember(1)]
        public string StockName { get; set; }

        [ProtoMember(2)]
        public decimal TotalVM { get; set; }


    }
}
