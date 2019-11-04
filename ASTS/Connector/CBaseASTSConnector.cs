using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;

using Common;
using Common.Interfaces;
using Common.Collections;
using Common.IO;
using Common.Utils;



using TradingLib.Enums;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Interaction;

using ASTS.Interfaces;
using ASTS.Interfaces.Clients;
using ASTS.Interfaces.Interactions;
using ASTS.Common;
using ASTS.Conf;


namespace ASTS.Connector
{
	public abstract class CBaseASTSConnector : CBaseFunctional, IStockConnector
	{


        public abstract bool IsConnectedToServer { get; set; }
		protected CASTSConnection _astsConnectionMain;
		protected IQueue<Action> _queueTransactions;

		protected IDealingServerForASTSConnector _dealingServer;

        protected ConfASTSConnector _confASTSConnector;

        protected ManualResetEvent _evConnectionClosed = new ManualResetEvent(false);

        


        public string Account
        {
            get
            {
                return _confASTSConnector.Account;

            }


        }


        public abstract string Password { get; }


		public CBaseASTSConnector(IDealingServerForASTSConnector dealingServer)
            : base(dealingServer)
		{
			_dealingServer = dealingServer;
             ReadConfASTSConnector();
			_queueTransactions = new CMutualQueue<Action>();
		}


        public void WaitConnectionClosed()
        {
            _evConnectionClosed.WaitOne();


        }



		protected void ProcessTransaction(Action transaction)
		{
			transaction.Invoke();
			
		}
	

		public void AddOrder(int botId, string instrument, decimal price, EnmOrderDir dir, decimal amount)
		{

			_queueTransactions.Add(new Action( () => _astsConnectionMain.AddOrder(botId,instrument,price,dir,amount)));				
			//_queueTransactions.Add(botId);		
		}

        public void ChangePassword(string currPassword, string newPassword)
        {

            _queueTransactions.Add(new Action(() => _astsConnectionMain.ChangePassword(currPassword,newPassword)));
        }



        public void CancelOrder(long orderId, int botId)
		{
			_queueTransactions.Add(new Action(() => _astsConnectionMain.CancelOrder(orderId,botId)));
		}

        public void CancelAllOrders(int buy_sell, int ext_id, string isin, int botId)
        {
            _queueTransactions.Add(new Action(() => _astsConnectionMain.CancellAllOrders(buy_sell,ext_id,isin, botId)));

        }


		public virtual void Connect()
		{



		}

        public virtual void DisconnectFromServer()
        {


        }

        public virtual void Disconnect()
        {



        }

        private void ReadConfASTSConnector()
        {
            string path = CUtil.GetConfigDir() + @"\ConfASTSConnector.xml";

            _confASTSConnector = new ConfASTSConnector()
            {
                NeedSelfInit = false,
                FileName = path
				
			
            };


            CSerializator.Read(ref _confASTSConnector);
           
        }


       



      




	}
}
