using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common;
using Common.Interfaces;

namespace TradingLib.Interfaces.Clients
{
    public interface IClientStockStruct : IAlarmable
    {
        bool IsStockOnline { get; set; }
        CGlobalConfig GlobalConfig { get; }
       // string GetPath();
        string GetDataPath();
    }
}
