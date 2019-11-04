using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace TradingLib.ProtoTradingStructs
{
    [ProtoContract]
    public class CUserOrdersUpdate
    {
    
        [ProtoMember(1)]
        public Dictionary<string, Dictionary<long, COrder>>   MonitorOrders {get;set;}


    }
}
