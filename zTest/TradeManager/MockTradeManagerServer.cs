using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common.Interfaces;

using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Abstract;
using TradingLib.Bots;

using Messenger;

using DBCommunicator;
using DBCommunicator.Interfaces;


using TradeManager.Views;

namespace zTest.TradeManager
{
    class MockTradeManagerServer :  IClientTradeManagerServer, IClientDBCommunicator
    {
        public int StockExchId { get; set; }
        public int PortTradeManager 
        {
            get 
            {
                return 6001;
            } 
        }

        CBasePosBox _posBox;
        public CBasePosBox PosBoxBase
        {
            get
            {
                return _posBox;
            }
        
        }

        public MockTradeManagerServer()
        {
            Messenger = new CMessenger();
            DBCommunicator = new CDBCommunicator("atfs", this);
            StockExchId = 1;
        }



        public IMessenger Messenger { get; set; }

        public IDBCommunicator DBCommunicator { get; set; }


        public void Error(string st, Exception exc=null)
        {

        }

        public bool IsDatabaseConnected { get; set; }
        public bool IsDatabaseReadyForOperations { get; set; }


		public void DisableBot(long id) { }
		public void EnableBot(long id) { }

        public CBotBase GetBotById(long id)
        {
            return null;
        }

        public void OnTrdMgrSentReconnect(int channelId)
        {

        }





    }

    

}
