using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib.Interfaces.Clients;
using TradingLib.GUI;
using TradingLib.Data.DB;
using TradingLib.BotEvents;





namespace ASTS.Interfaces.Clients
{
    public interface IClientPositionsBoxASTS :  IClientPosBox,   IAlarmable
    {

        string Account { get; }
        CListInstruments Instruments { get; set; }
        CGUIBox GUIBox { get; set; }
        void TriggerRecalcAllBots(EnmBotEventCode evnt, object data);

        int StockExchId { get; set; }



    }
}
