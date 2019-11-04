using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitfinexCommon.Enums
{
    public enum EnmBfxOrderType
    {
        Undefined,
        Limit,
        Market,
        Stop,
        TrailingStop,
        ExchangeMarket,
        ExchangeLimit,
        ExchangeStop,
        ExchangeTrailingStop,
        Fok,
        ExchangeFok,
        StopLimit,
        ExchangeStopLimit
    }
}
