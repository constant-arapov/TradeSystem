using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;


using ASTS.Interfaces;
using ASTS.DealingServer;


namespace ASTS.Interfaces.Interactions
{
    public interface IDealingServerForTrades : IAlarmable
    {
        bool IsOnlineUserDeals { get; set; }

        CUserDealsPosBoxASTS UserDealsPosBoxInp { get;  }

		bool IsDealsPosLogLoadedFromDB { get; set; }


    }
}
