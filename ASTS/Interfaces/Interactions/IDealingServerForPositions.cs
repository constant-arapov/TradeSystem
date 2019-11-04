using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;


using ASTS.DealingServer;


namespace ASTS.Interfaces.Interactions
{
    public interface IDealingServerForPositions : IAlarmable
    {

     
        int StockExchId { get; }
        CPosistionsBoxASTS PosistionsBoxInp { get; }

    }
}
