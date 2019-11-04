using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace TradingLib.ProtoTradingStructs
{
    [ProtoContract]
    public class CUserPosMonitorUpdate
    {
        [ProtoMember(1)]
        public Dictionary<string, CUserPos> MonitorUserPos { get; set; }

    }
}
