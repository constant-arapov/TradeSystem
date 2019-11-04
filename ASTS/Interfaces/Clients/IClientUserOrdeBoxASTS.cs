using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;


using TradingLib.BotEvents;

using ASTS.DealingServer;


namespace ASTS.Interfaces.Clients
{
    public interface IClientUserOrdeBoxASTS : IAlarmable
    {

        void TriggerRecalculateBot(int botId, string isin, EnmBotEventCode code, object data);
        void TriggerRecalcAllBots(EnmBotEventCode evnt, object data);
        bool IsOnlineUserOrderLog { get; set; }

    }
}
