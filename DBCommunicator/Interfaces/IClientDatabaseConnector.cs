using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;


namespace DBCommunicator.Interfaces
{
    public interface IClientDatabaseConnector
    {

        bool IsDatabaseConnected { get; set; }

        bool IsDatabaseReadyForOperations { get; set; }

      //  long UpdateProcProfit(int accountId, int stockExchId, decimal value);


    }
}
