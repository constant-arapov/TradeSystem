using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Common;
using Common.Logger;

using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Clients;
using TradingLib.Data.DB;

using TradingLib.BotEvents;

namespace TradingLib.Abstract
{
    public abstract class CBaseSessionBox : CBaseFunctional, ISessionBox
    {
     
        public abstract ISession CurrentSession { get; }
        public abstract bool IsPossibleCancellOrders();          

        protected List<CDBSession> _lstSessions = new List<CDBSession>();

        protected IClientSessionBox _client { get; set; }

        


        public CBaseSessionBox(IClientSessionBox client) : base(client)
        {
            _client = client;
          //  m_logger = new CLogger("SessionBox");
            
        }


        protected abstract void InsertUnsavedSessions();

      

        /// <summary>             
        /// 1)Inserts unstored sessions.
        /// 2)Check session for completion. If session is completed 
        /// updates session's status    
        /// 3)Process Automatic clearing
        /// 4)Send reports
        /// 
        /// /// Call when:
        /// 1)Session become online for P2( assume at start)
        /// 2)When session ended (expired). Assume after day session
        /// (and whole session)  ended  
        /// 
        /// Call from:        
        /// 1) Plaza2Connector.SessionBox.OnSessionExpired
        /// 2) Plaza2Connector.IsSessionOnline
        /// 3) CBaseDealingServer.Process
        /// </summary>
        public void TaskCheckUnsavedSessionsAndClearing()
        {
            Log("TaskCheckUnsavedSessionsAndClearing entry");

         
            _client.WaitDataLoadedFromDB();
            _client.WaitServerTimeAvailable();
                       
            
            Log("Step 1 insert unstored sessions");
            InsertUnsavedSessions();

            Log("Step 2 Get stored sessions and check if sessions completed. If completed update it");
            //Gen list of "Not completed sessions"
            var notCompletedSess = _client.DBCommunicator.GetUnCompletedSessions(_client.StockExchId);
            int tol = 1000;

            List<int> needSetCompletedSession = new List<int>();
            //if session already ended (check it using server time) do add to 
            //"needSetCompletedSession" list 
            notCompletedSess.ForEach(a =>
            {
                CDBSession dbSess = new CDBSession();
                _client.FillDBClassField(a, dbSess);
                if (dbSess.IsCompleted == 0)
                    if (_client.ServerTime > dbSess.DtEnd.AddMilliseconds(-tol))
                        needSetCompletedSession.Add(dbSess.StockExchangedSessionId);

            }
                                     );


            //... set that session completed
            if (needSetCompletedSession.Count != 0)
                _client.DBCommunicator.SetCompletedSessions(needSetCompletedSession);


            _client.ClearingProcessor.ProcessAutomaticClearing();
            _client.IsAutomaticClearingProcessed = true;
            //2018-06-14 - after clearing - do force update TradeManagers
            //to load new data

            _client.TriggerRecalcAllBots(EnmBotEventCode.OnForceUpdTotalVM, null);
            _client.TriggerRecalcAllBots(EnmBotEventCode.OnForceUpdTrdMgr, null);

            _client.SendReports();



            Log("TaskCheckUnsavedSessionsAndClearing exit");

        }

        protected void OnSessionEnd()
        {
            _client.TriggerRecalcAllBots(EnmBotEventCode.OnSessionEnd, null);
        }


    

        public void Error(string msg, Exception e = null)
        {

            _client.Error(msg, e);

        }

    }
}
