using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.BotEvents;

using TradingLib.Bots;
using TradingLib.GUI;

using DBCommunicator;
using DBCommunicator.Interfaces;

using CryptoDealingServer.Components;
using CryptoDealingServer.Interfaces;

namespace zTest.Crypto
{
    public class TestSessionBox : IClientSessionBoxCrypto, IClientDBCommunicator
    {

        public bool IsDatabaseConnected { get; set; }
        public bool IsDatabaseReadyForOperations { get; set; }

        public void WaitDataLoadedFromDB() { }
        public bool IsAutomaticClearingProcessed { get; set; }
        public bool IsPossibleToCancelOrders { get; set; }
        public bool IsSessionActive { get; set; }

        public bool IsSessionOnline { get; set; }

        public bool UseRealServer { get; set; }


        public int SessionCurrent { get; set; }

        public CGUIBox GUIBox { get; set; }

        public DateTime ServerTimeLocal()
        {
            throw new NotImplementedException();
        }
        public void OnSessionDeactivate()
        {
            throw new NotImplementedException();
        }


        public void OnNightStarted()
        {
            throw new NotImplementedException();
        }



        public DateTime ServerTime
        {
            get
            {

                return DateTime.UtcNow;
            }
            set { }

        }




        public List<CBotBase> ListBots { get; set; }

        public CClearingProcessor ClearingProcessor { get; set; }

        public IDBCommunicator DBCommunicator { get; set; }

        public void OnSessionActivate()
        {
            throw new NotImplementedException();
        }

        public void OnNightEnded()
        {
            throw new NotImplementedException();
        }


        public void OnIntradeyClearingBegin()
        {
            throw new NotImplementedException();
        }



        public void OnEnableCancellOrders()
        {
            throw new NotImplementedException();
        }




        public void OnDisableCancellOrders()
        {
            throw new NotImplementedException();
        }

        public void OnDaySessionExpired()
        {
            throw new NotImplementedException();
        }

        public void FillDBClassField<T>(Dictionary<string, object> row, T outObj)
        {
            throw new NotImplementedException();
        }

        public void SendReports()
        {
            throw new NotImplementedException();
        }
        public int StockExchId { get; set; }

        public List<string> GetInsruments()
        {
            throw new NotImplementedException();
        }

        public void WaitServerTimeAvailable()
        {
            throw new NotImplementedException();
        }


        public void Error(string msg, Exception e)
        {
            throw e;
        }


        private CSessionBoxCrypto _sessionBox;


        public void Test()
        {

            StockExchId = 4;

            _sessionBox = new CSessionBoxCrypto(this);


            DBCommunicator = new CDBCommunicator("atfs", this);

            DBCommunicator.WaitReadyForOperations();


            _sessionBox.Process();

            _sessionBox.GenerateFirstSession();



            //	var el = DBCommunicator.GetSessionsNotClearingProcessed(StockExchId);
            //TODO generate session

        }


        public void OnClearingProcessed()
        {


        }

        public void TriggerRecalcAllBots(EnmBotEventCode evnt, object data)
        {

        }

        public void UpdateTurnOver()
        {


        }
        public void UpdateFeeTurnoverCoefs()
        { }



    }
}
