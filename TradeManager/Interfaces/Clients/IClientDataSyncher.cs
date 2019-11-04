using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;


namespace TradeManager.Interfaces.Clients
{
    public interface IClientDataSyncher : IAlarmable
    {

        bool IsStockExchSelected(int stockExhId);

    }
}
