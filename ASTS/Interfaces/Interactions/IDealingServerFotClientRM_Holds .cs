using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib.Interfaces.Components;
using TradingLib.GUI;

using ASTS.DealingServer;

namespace ASTS.Interfaces
{
    public interface IDealingServerFotClientRM_Holds  : IAlarmable
    {

        CPosistionsBoxASTS PosistionsBoxInp { get; }
        int StockExchId { get; }
    }
}
