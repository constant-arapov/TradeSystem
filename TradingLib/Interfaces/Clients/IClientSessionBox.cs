using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib.BotEvents;
using TradingLib.Bots;
using TradingLib.GUI;
using TradingLib.Interfaces.Components;


namespace TradingLib.Interfaces.Clients
{
    public interface IClientSessionBox : IAlarmable, IClientSession
    {
       
        void WaitDataLoadedFromDB();
         bool IsAutomaticClearingProcessed { get; set; }
         bool IsPossibleToCancelOrders { get; set; }
         bool IsSessionActive { get; set; }

         bool IsSessionOnline { get; set; }

         bool UseRealServer { get; set; }


         int SessionCurrent { get; set; }
         DateTime ServerTimeLocal();
         void OnSessionDeactivate();
         void OnNightStarted();
         DateTime ServerTime { get; set; }
         List<CBotBase> ListBots { get; set; }
        
         CClearingProcessor ClearingProcessor { get; set; }

         IDBCommunicator DBCommunicator { get; set; }

         void OnSessionActivate();

         void OnNightEnded();

         void OnIntradeyClearingBegin();

        

         void OnEnableCancellOrders();

        

         
         void OnDisableCancellOrders();

         void OnDaySessionExpired();

         void FillDBClassField<T>(Dictionary<string, object> row, T outObj);

         void SendReports();
         int StockExchId { get; set; }

         List<string> GetInsruments();

         void WaitServerTimeAvailable();

        void TriggerRecalcAllBots(EnmBotEventCode evnt, object data);
    }
}
