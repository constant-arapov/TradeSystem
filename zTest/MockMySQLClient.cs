using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DBCommunicator;
using DBCommunicator.Interfaces;

using Common.Interfaces;


namespace zTest
{
    public class MockMySQLClient : IClientDatabaseConnector, IAlarmable
    {

      public bool IsDatabaseConnected { get; set; }

      public bool IsDatabaseReadyForOperations { get; set; }


     public void Error(string description, Exception exception = null)
     {


     }

    }
}
