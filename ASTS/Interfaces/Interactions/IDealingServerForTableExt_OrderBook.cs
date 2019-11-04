using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;



using Common.Interfaces;

namespace ASTS.Interfaces.Interactions
{
    public interface IDealingServerForTableExt_OrderBook : IAlarmable
    {
        ManualResetEvent EvStockOnline { get; set; }
        bool IsStockOnline { get; set; }

    }
}
