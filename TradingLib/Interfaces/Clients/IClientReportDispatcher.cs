using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

namespace TradingLib.Interfaces.Clients
{
    public interface IClientReportDispatcher : IAlarmable
    {
        int GetDecimalVolume(string instrument);
    }
}
