using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Collections.Specialized;

using Common;
using Common.Interfaces;
using Common.Collections;
using Common.Logger;
using Common.Utils;

using TradingLib.Interfaces.Interaction;



using ASTS.Interfaces;
using ASTS.Interfaces.Clients;
using ASTS.Interfaces.Interactions;
using ASTS.Common;
using ASTS.Connector.State;

namespace ASTS.Connector
{
	public class CASTSConnectorSingle : CBaseASTSConnector, ICleintState
	{

        private DateTime _dtLastSnapshotUpdate = DateTime.Now;
        private int _parPeriodicUpdateSnapshotMs = 1000;


        private int _parRequestPeriod = 100;


		public CBaseStateConnector StateDisconnected;
		public CBaseStateConnector StateConnected;
        public CBaseStateConnector StateOff;


		private CBaseStateConnector _state;
       


        private ILogable _loggerStates;

        public bool RequestDisconnect = false;



        public override  string Password
        {
            get
            {
                return _astsConnectionMain.Password;
            }

        }




        public override bool IsConnectedToServer { get; set; }
       




		public CASTSConnectorSingle(IDealingServerForASTSConnector dealingServer)
                                    
			: base(dealingServer)
		{
            IsConnectedToServer = false;
            CreateStates();
            _state = StateDisconnected;

			_queueTransactions = new CMutualQueue<Action>();

            _parRequestPeriod = _confASTSConnector.RequestPeriod;

			CUtil.ThreadStart(Process);		
		}


        private void CreateStates()
        {
            _loggerStates = new CLogger("Connector_States", flushMode: true);

            StateDisconnected = new CStateConnector_Disconnected(this);
            StateConnected = new CStateConnector_Connected(this);
            StateOff = new CStateConnector_Off(this);
           

        }

        public void LogState(string msg)
        {
            _loggerStates.Log(msg);

        }


		private void Process()
		{
            try
            {
                _astsConnectionMain = new CASTSConnection(_confASTSConnector ,  _dealingServer, true);
            }
            catch (Exception e)
            {         
                //Bad situation. It is better to stop
                //TODO close all
                Error("CASTSConnectorSingle. Unable create ASTSConnection", e);
                return;
            }





			try
			{
               
                while (!RequestDisconnect)
                {
                    try
                    {
                      /*  _astsConnectionMain.Connect();
                        MainLoop();
					   */
						_state.Process();


                    }
                    catch (Exception e)
                    {
                        Error("",e);
                    }
                }

				//_astsConnectionMain.Disconnect();
			}
			catch (Exception e)
			{
				Error("Error CASTSConnectorSingle.Process ", e);

			}
          //  RequestDisconnect = false;

            Log("Exit connection process");

            _evConnectionClosed.Set();
		}


    



        /*
        public void ProcessDisconnect()
        {
            _astsConnectionMain.Disconnect();
        }
        */

		public void SetState  (CBaseStateConnector stateNew) 
		{
           
			_state = stateNew;

		}


		public override void Connect()
		{
			_astsConnectionMain.Connect();
		}

        //Start disconnect here
        public override void DisconnectFromServer()
        {
         
            _state.RequestDisconnect();
            
        }


        public override void Disconnect()
        {
            //disconnect main connection
            try
            {
                _astsConnectionMain.Disconnect();
            }
            catch (Exception e)
            {
                Error("Error disconnect main ASTS connection",e);

            }
            Log("Disconnect main ASTS connection successfull");

        }



        

        private void UpdateSnapshot()
        {
            if ((DateTime.Now - _dtLastSnapshotUpdate).TotalMilliseconds > _parPeriodicUpdateSnapshotMs)
            {
                _astsConnectionMain.UpdateSnapshot();
                _dtLastSnapshotUpdate = DateTime.Now;
            }

        }



		public void MainLoop()
		{
            DateTime dtStart = DateTime.Now;
			DateTime dtStartIteration;
			
			int parMaxLength = 30000 * 1024; //30 kB
		
          
            while (!RequestDisconnect)
			{

                

                //remember start server request time
				dtStartIteration = DateTime.Now;
              
               


				int len = 0;
				//if there is no transaction, request data
                //if there is transaction do not request data
				if (!ProcessQueue())
					len = _astsConnectionMain.RequestData();
                
                UpdateSnapshot();

                //must be not more than parSleep
				int dt = _parRequestPeriod - (DateTime.Now - dtStartIteration).Milliseconds;

				
				if (dt > 0)
					if (len <= parMaxLength)
						Thread.Sleep(dt);
                Log("RequestData. dt=" + dt + " len=" + len);
           
			}


		}
       
		/// <summary>
		/// 
		/// </summary>
		/// <returns>if transaction processed, return true</returns>
		private bool ProcessQueue()
		{
			Action transaction = _queueTransactions.Get();
			
			//no transaction inq queue
			if (transaction == null)
				return false;

			ProcessTransaction(transaction);


			//transaction processed
			return true;
		}

		



	}
}
