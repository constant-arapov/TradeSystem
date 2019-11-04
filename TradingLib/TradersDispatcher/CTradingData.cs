using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Enums;



namespace TradingLib.TradersDispatcher
{
    public class CTradingData
    {
        public enmTradingEvent Event { get; set; }
        public object Data { get; set; }
        public int ConnId { get; set; }


    }
}
