using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common;
using Common.Interfaces;

using TradingLib;
using TradingLib.Data;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Components;
using TradingLib.GUI;
using TradingLib.BotEvents;


namespace TradingLib.Interfaces.Clients
{
    public interface IClientTimeFrameAnalyzer : IAlarmable
    {
        bool IsDealsOnline { get; set; }
        bool IsAnalyzerTFOnline { get; set; }
        CGUIBox GUIBox { get; set; }
        void TriggerRecalcAllBotsWithInstrument(string instrument, EnmBotEventCode evnt, object data);
        DateTime ServerTime { get; }
        bool IsTimeToInitCandles { get; set; }
        CGlobalConfig GlobalConfig { get; }
        string GetDataPath();
        IDealBox DealBox { get; }
        
    }
}
