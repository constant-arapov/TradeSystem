using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;


namespace Common.Interfaces
{
    public interface IDataBaseStatus
    {

        bool IsDatabaseConnected { get; set; }
        bool IsDatabaseReadyForOperations { get; set; }
        //AutoResetEvent EvDatabaseConnected { get; set; }


    }
}
