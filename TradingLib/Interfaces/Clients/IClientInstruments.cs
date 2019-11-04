using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;

using TradingLib.Interfaces.Components;
using TradingLib.Data;

//using DBCommunicator;

namespace TradingLib.Interfaces.Clients
{
    public interface IClientInstruments
    {

        /*CDBCommunicator*/ IDBCommunicator DBCommunicator { get; set; }
       // Dictionary<string, long> DictInstr_IsinId { get; set; }
        //Dictionary<long, string> DictIsin_id {get; set;}
       // bool IsDictIsinAvailable { get; set; }
        int StockExchId { get; set; }


        Dictionary<string, CInstrument> DictInstruments { get; set; }
        // CGlobalConfig GlobalConfig { get; set; }

        void UpdateStepPrice(string instrument, decimal newStepPrice);

    }
}
