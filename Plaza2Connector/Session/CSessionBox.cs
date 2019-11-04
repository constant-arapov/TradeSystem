using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using Common;
using Common.Interfaces;
using Common.Logger;
using Common.Utils;

using TradingLib;
using TradingLib.Data.DB;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Interaction;
using TradingLib.Interfaces.Components;
using TradingLib.BotEvents;
using TradingLib.Abstract;

using DBCommunicator;
//using DBCommunicator.DBData;

//using Plaza2Connector.Session;
using TradingLib.Bots;


using Plaza2Connector.Interfaces;

namespace Plaza2Connector
{
	public class CSessionBox : CBaseSessionBox, ISessionBox, ISessionBoxForP2Connector, ILogable, IAlarmable 
    {
        List<CRawSession> m_listRawSession = new List<CRawSession>();

       // /*CPlaza2Connector*/IClientSessionBox m_plaza2Connector;

        CRawSession m_curentSessionRaw;      
        public CRawSession CurrentRawSession { get { return m_curentSessionRaw; } }



        int _sessionCurrentNum;
        DateTime _sessionCurrentBegin;


        ISession m_currSession; 
        public override ISession CurrentSession { get { return m_currSession; } }



        public FUTINFO.session LatestRcvdSess;

        private EnmFORTSInerClearingSate _interClearingState;

        private IClientSessionBoxP2 _clientP2;


        public CSessionBox(/*CPlaza2Connector*/IClientSessionBoxP2 client)
            : base(client)
        {
            _client = client;
            _clientP2 = client;
            m_currSession = new CSession( (IClientSession) _client);

            
      
            
        }


      
      

        public override bool IsPossibleCancellOrders()
        {

            //2017-10-23
            if (m_curentSessionRaw == null)
                return false;

            if (m_curentSessionRaw.State == (int)EnmFORTSSessionState.S1_SessionActiveTradeEnabled
                || m_curentSessionRaw.State == (int)EnmFORTSSessionState.S0_SessionWasSet)
            {


                return true;
            }

            return false;
        }



       

        public void Update(FUTINFO.session sess)
        {


           // UpdateLatestRcvdSess(sess);
          

                
            m_listRawSession.Add(new CRawSession(sess));
            
            // Session is active. Possible to add and cancel orders
            DateTime SrvTmLocal = _client.ServerTimeLocal();
          

            int timeTolMS = 1000;




           Log("Update SrvTmLocal=" + SrvTmLocal + " begin=" + sess.begin + " end=" + sess.end + " state=" + sess.state +                
                " eve_begin=" + sess.eve_begin + " eve_end=" + sess.eve_end + " eve_on"+sess.eve_on +
                " mon_begin="+sess.mon_begin+ " end="+sess.mon_end+ " mon_on="+ sess.mon_on+
                " IsSessionActive=" + _client.IsSessionActive);





           _interClearingState = (EnmFORTSInerClearingSate) sess.inter_cl_state;


            if (sess.state == (int)EnmFORTSSessionState.S1_SessionActiveTradeEnabled)
            {

                _client.GUIBox.SessionState = 1;

               Log("S1_SessionActiveTradeEnabled");

                //check time also
                if ( CUtilTime.InTmInterval (SrvTmLocal, sess.begin, sess.end, timeTolMS) ||
                     (sess.eve_on == 1 &&  CUtilTime.InTmInterval(SrvTmLocal, sess.eve_begin, sess.eve_end, timeTolMS)) ||
                     (sess.mon_on == 1 && CUtilTime.InTmInterval(SrvTmLocal, sess.mon_begin, sess.mon_end, timeTolMS)) 
                    )
                {
                   Log("Is in interval");
                    m_curentSessionRaw = new CRawSession(sess);
                    
                    m_currSession.SetCurrentSession(sess, SrvTmLocal, timeTolMS);
                    

                    if (!_client.IsSessionActive)
                     {                       
                        _client.OnSessionActivate();
                        _client.IsSessionActive = true;
                        _client.SessionCurrent = sess.sess_id;
                        //m_plaza2Connector.DtSessionCurrentBegin = sess.begin;

                        _sessionCurrentNum = sess.sess_id;
                        _sessionCurrentBegin = sess.eve_begin;
                       
                       Log("IsSessionActive = true;");


                        if ( CUtilTime.InTmInterval (SrvTmLocal, sess.inter_cl_end , sess.end,timeTolMS))
                            _clientP2.OnIntradayClearingEnd();

                        if (CUtilTime.InTmInterval (SrvTmLocal, sess.end, sess.eve_begin, timeTolMS))
                            _clientP2.OnEveningClearingEnd();

                        if ((CUtilTime.InTmInterval (SrvTmLocal, sess.mon_begin, sess.mon_end,timeTolMS) && sess.mon_on == 1) ||
                           (CUtilTime.InTmInterval ( SrvTmLocal, sess.begin,  sess.end, timeTolMS) && sess.mon_on == 0))                                                                                                                
                           _client.OnNightEnded();
                        

                    }

                    if (!_client.IsPossibleToCancelOrders)
                    {
                        _client.IsPossibleToCancelOrders = true;
                        _client.OnEnableCancellOrders();
                    }


                }

            }

            else     //session is not active
            {
                if (sess.sess_id < m_currSession.SessionNumber)
                {
                    Log("sess.sess_id < m_currSession.SessionNumber exiting");
                    return;
                }


                _client.GUIBox.SessionState = 0;

                int s1 = _client.SessionCurrent;
                int s2 = sess.sess_id;


               Log("else...");
                //not possible to add orders
                if (_client.IsSessionActive)
                {
                    _client.OnSessionDeactivate();
                    _client.IsSessionActive = false;                 

                }



                //... but possible to cancel orders
                if (sess.state == (int)EnmFORTSSessionState.S0_SessionWasSet || sess.state == (int)EnmFORTSSessionState.S2_SessionNotActiveTradeDisabled)
                {
                   Log("EnmSessionState.S0_SessionWasSet || sess.state == S2_SessionNotActiveTradeDisabled");

                    if (!_client.IsPossibleToCancelOrders)
                    {
                        _client.IsPossibleToCancelOrders = true;
                        _client.OnEnableCancellOrders();
                    }



                    if (CUtilTime.InTmInterval ( SrvTmLocal, sess.inter_cl_begin, sess.inter_cl_end, timeTolMS))
                        _client.OnIntradeyClearingBegin();


                    if (CUtilTime.InTmInterval(SrvTmLocal, sess.end, sess.eve_begin, timeTolMS))
                    {
                        _clientP2.OnEveningClearingBegin();
                       
                     
                       

                    }

                    if ( (SrvTmLocal > sess.eve_end && SrvTmLocal < sess.mon_begin && sess.mon_on==1) ||
                         (SrvTmLocal > sess.eve_end && SrvTmLocal < sess.begin && sess.mon_on == 0))
                        _client.OnNightStarted();

                   
                }
                else if (sess.state == (int)EnmFORTSSessionState.S4_SessionExpired)
                {
                    if (CUtilTime.InTmInterval(SrvTmLocal, sess.end, sess.eve_begin, timeTolMS))                    
                        _client.OnDaySessionExpired();

                    //if session online end expired than day session finished
                    if (_client.IsSessionOnline)
                        OnSessionExpired(sess);
                      
                    
                }

                else //not possible cancell orders
                {
                   Log("not possible cancell orders");
                    if (_client.IsPossibleToCancelOrders)
                    {
                        _client.IsPossibleToCancelOrders = false;
                        _client.OnDisableCancellOrders();

                    }

                }
            }


            foreach (CBotBase bt in _client.ListBots)
                bt.Recalc("", EnmBotEventCode.OnSessionUpdate, null);



            UpateLstSessions(sess);
           // UpdateDBSession(sess);
    }

        protected override void InsertUnsavedSessions()
        {
          
                _lstSessions.ForEach(session => _client.DBCommunicator.InsertUnsavedSessionP2(session));
            
        }




        public void OnSessionExpired(FUTINFO.session sess)
        {


            //first update lstSessions 
            UpateLstSessions(sess);
            //after update start clering process
            int tol = 1000;
            //advance check that session expired
            if (_client.ServerTime > sess.end.AddMilliseconds(-tol)) 
                (new Task(TaskCheckUnsavedSessionsAndClearing)).Start();



        }



        /// <summary>
        /// Add session to session list if it is not already in this list.
        /// 
        /// Call from 
        /// 1) CSessionBox.Update
        /// 2) On session expired
        /// </summary>     
        public void UpateLstSessions(FUTINFO.session sess)
        {
            
            if (!_lstSessions.Exists(a => a.StockExchangedSessionId == sess.sess_id))
            {
                int isCompleted = 0;

                int tol = 1000;
                if (_client.ServerTime > sess.end.AddMilliseconds(-tol))
                    isCompleted = 1;

                _lstSessions.Add(new CDBSession
                {
                    DtBegin = sess.eve_begin,
                    DtEnd = sess.end,
                    StockExchangedSessionId = sess.sess_id,
                    IsCompleted = isCompleted,
                    stock_exchange_id = _client.StockExchId

                });

                Log("Added new session id="+sess.sess_id);
            }




        }


       




    }
    enum EnmFORTSSessionState : int
    {
        S0_SessionWasSet= 0,
        S1_SessionActiveTradeEnabled = 1,
        S2_SessionNotActiveTradeDisabled =2,
        S3_SessionAborted =3,
        S4_SessionExpired =4

    }
    enum EnmFORTSInerClearingSate : int
    {
        S00_IsUndefined,      //Possible add and cancell orders
        S01_IsScheduledToday, //Possible add and cancell orders
        S02_IsCanceled,       //Possible add and cancell orders
        S04_IsCurrent,        //NOT Possible add cancell orders
        S08_IsCurrentButOver, //Not possible add but possible cancell
        S10_IsFinished        //Possible add and cancell orders




    }

}
