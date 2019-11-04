using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib.Bots;


namespace TradingLib.Interfaces.Clients
{
    public interface IClientBotRiskManager : IAlarmable, ILogable
    {
        int BotId { get; set; }
        void SelfTerminate(string reason = null);
        decimal VMAllInstrClosed { get; set; }
        decimal VMAllInstrOpenedAndClosed { get; set; }
        CSettingsBot SettingsBot { get; set; }
        Dictionary<string, decimal> DictVMOpenedAndClosed { get; }
        Dictionary<string, CBotPos> MonitorPositionsAll { get; set; }
        
    }
}
