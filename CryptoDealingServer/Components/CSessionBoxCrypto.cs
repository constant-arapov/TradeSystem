using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using Common.Utils;

using TradingLib.Interfaces.Components;
using TradingLib.Common;
using TradingLib.Bots;
using TradingLib.Abstract;
using TradingLib.Data.DB;
using TradingLib.Interfaces.Clients;

using CryptoDealingServer.Interfaces;


namespace CryptoDealingServer.Components
{
    public class CSessionBoxCrypto : CBaseSessionBox, ISessionBox
    {
        public ISession _currentSession;





        public override ISession CurrentSession
        {
            get
            {
                return _currentSession;
            }
        }

        private CDBSession _currDBSession;





        private IClientSessionBoxCrypto _clientCrypto;



        private int _sessionBeginHours;
        private int _sessionBeginMinutes;

        private int _sessonEndHours;
        private int _sessionEndMinutes;
        private int _maxClearingDurMinut;

        CBotState<EnmStateSessBoxCrypt> _state;




        public CSessionBoxCrypto(IClientSessionBoxCrypto client)
            : base(client)
        {
            _clientCrypto = client;


            _sessionBeginHours = 0; //0
            _sessionBeginMinutes = 0; //0

            _sessonEndHours = 23;      //23
            _sessionEndMinutes = 55;  //55

            _maxClearingDurMinut = 4;

        }



        public void Process()
        {

            CUtil.ThreadStart(ThreadMain);


        }



        public void SetGUISessionNonActive()
        {
            if (_client.GUIBox.IsSessionActive)
                _client.GUIBox.IsSessionActive = false;

            if (_client.GUIBox.SessionState != CodessionState.NotActive)
                _client.GUIBox.SessionState = CodessionState.NotActive;

        }


        public void SetGUISessionActive()
        {
            if (!_client.IsSessionActive)
                _client.IsSessionActive = true;

            if (!_client.GUIBox.IsSessionActive)
                _client.GUIBox.IsSessionActive = true;

            if (_client.GUIBox.SessionState != CodessionState.Active)
                _client.GUIBox.SessionState = CodessionState.Active;




        }

        public void WaitTillAutomaticClearingProcessed()
        {
            while (!_client.IsAutomaticClearingProcessed)
                Thread.Sleep(10);

        }


        private void ValidateSessionParameters()
        {
            if (_sessionBeginHours > _sessonEndHours)
                throw new ApplicationException("SessionBoxCrypto _sessionBeginHours < _sessonEndHours");
        }






        public bool IsClearing()
        {
            DateTime dtCurr = _client.ServerTime;

            //DateTime tmpDt = new DateTime(2019, 01, 03, 13, 03, 00);

            return dtCurr > _currDBSession.DtEnd &&
                           (dtCurr - _currDBSession.DtEnd).TotalMinutes < _maxClearingDurMinut;
            //return dtCurr > tmpDt &&
              //          (dtCurr - tmpDt).TotalMinutes < _maxClearingDurMinut;
        }




        public void ThreadMain()
        {
            ValidateSessionParameters();
            _client.IsSessionActive = false;

            _state = new CBotState<EnmStateSessBoxCrypt>(EnmStateSessBoxCrypt._01_Initial, _logger);


            //On start dealing server triggers  
            //Automatic clearing processing so wait
            //till it finish.
            WaitTillAutomaticClearingProcessed();



            //_client.WaitDataLoadedFromDB();


            //Now generate new session
            GenerateFirstSession();

            while (true)
            {
                try
                {
                    //Thread.Sleep(30000);
                    DateTime dtCurr = _client.ServerTime;


                    if (IS(EnmStateSessBoxCrypt._02_IsTrading))
                    {    //clearing condition
                       
                         if (IsClearing())                       
                        {

                            SetGUISessionNonActive();
                            _client.IsSessionActive = false;
                            CUtil.TaskStart(TaskCheckUnsavedSessionsAndClearing);
                            SetState(EnmStateSessBoxCrypt._03_IsClearing);
                            OnSessionEnd();
                        }
                    }
                    else if (IS(EnmStateSessBoxCrypt._01_Initial))
                    {

                        UpdateTurnoverFeeCoefs();

                        if (IsClearing())
                        {
                            SetGUISessionNonActive();
                            _client.IsSessionActive = false;
                            SetState(EnmStateSessBoxCrypt._03_IsClearing);
                        }
                        else
                        {
                            //activate normal trading
                            SetState(EnmStateSessBoxCrypt._02_IsTrading);
                            _client.IsSessionActive = true;
                            SetGUISessionActive();
                        }
                    }
                    else if (IS(EnmStateSessBoxCrypt._03_IsClearing))
                    {
                        //wait till clearing processor completes clearing
                        WaitTillAutomaticClearingProcessed();
                        //Generate next session...
                        GenerateNextSession();


                        UpdateTurnoverFeeCoefs();

                        //... and wait till it is time for new session
                        while (_client.ServerTime < _currDBSession.DtBegin)
                            Thread.Sleep(10);

                        _clientCrypto.OnClearingProcessed();

                        SetState(EnmStateSessBoxCrypt._02_IsTrading);
                        _client.IsSessionActive = true;
                        SetGUISessionActive();

                    }

                }
                catch (Exception e)
                {
                    Error("CSessionBoxCrypto.ThreadMain", e);
                }
                Thread.Sleep(100);
            }


        }

        private void UpdateTurnoverFeeCoefs()
        {
            _clientCrypto.UpdateTurnOver();
            _clientCrypto.UpdateFeeTurnoverCoefs();


        }


       
		
        protected override void InsertUnsavedSessions()
        {
            //Not need at the moment. For capability
           // _lstSessions.ForEach(session => _client.DBCommunicator.InsertUnsavedSessionASTS(session));

        }

        private bool IS(EnmStateSessBoxCrypt state)
        {
            return _state.IS(state);
        }


        private void SetState(EnmStateSessBoxCrypt state)
        {
            _state.SetState(state);
        }

        private int GenSessionId()
        {
            //DateTime _dt = _client.ServerTime;
            long ls = CUtilTime.GetCurrentUnixTimestampSeconds();

            return (int)ls;
            
        }

		public void GenerateFirstSession()
		{

            _currDBSession = new CDBSession
			{
				DtBegin = DateTime.Now.Date.AddHours(_sessionBeginHours).AddMinutes(_sessionBeginMinutes),
				DtEnd = DateTime.Now.Date.AddHours(_sessonEndHours).AddMinutes(_sessionEndMinutes),
				stock_exchange_id = _client.StockExchId,
                 StockExchangedSessionId = GenSessionId()
			};


            _client.GUIBox.UpdateSessionString(_currDBSession.DtBegin,
                                               _currDBSession.DtEnd);

			
            //Insert new session to DB. 
            //If session already in DB it will not insert (as implemented in method).
            _client.DBCommunicator.InsertUnsavedSessionCrypto(_currDBSession);


			//DateTime dt = _client.ServerTime;
			//if (dt.)


			

		}
        


        private bool _bProduction = true;

        public void GenerateNextSession()
        {
         
            if (_bProduction) //use on real server
            {
                 _currDBSession = new CDBSession
                {   //2018-04-22
                    DtBegin = _currDBSession.DtBegin.Date.AddDays(1).AddHours(_sessionBeginHours).AddMinutes(_sessionBeginMinutes),
                    DtEnd = _currDBSession.DtEnd.Date.AddDays(1).AddHours(_sessonEndHours).AddMinutes(_sessionEndMinutes),
                    stock_exchange_id = _client.StockExchId,
                    StockExchangedSessionId = GenSessionId()
                };

            }
            else    //use for debugging
            {
            
                _currDBSession = new CDBSession
                {
                    DtBegin = _currDBSession.DtEnd.AddMinutes(4),
                    DtEnd =  _currDBSession.DtEnd.AddMinutes(10), 
                    stock_exchange_id = _client.StockExchId,
                    StockExchangedSessionId = GenSessionId()
                };
            }
            
          
           
            _client.DBCommunicator.InsertUnsavedSessionCrypto(_currDBSession);
        }
      

        public override  bool IsPossibleCancellOrders()
        {
            return true;
        }

       




    }


    enum EnmStateSessBoxCrypt
    {
        _01_Initial,
        _02_IsTrading,
        _03_IsClearing,
        _04_WaitNextSession

    }


}
