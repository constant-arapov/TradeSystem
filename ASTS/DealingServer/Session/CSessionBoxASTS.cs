using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using Common.Interfaces;
using Common.Logger;
using Common.Utils;


using TradingLib.Abstract;
using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Clients;
using TradingLib.Common;
using TradingLib.Data.DB;


using ASTS.Interfaces.Clients;
using ASTS.Common;
using ASTS.Tables;
using ASTS.DealingServer.Session.State;

namespace ASTS.DealingServer.Session
{
    public class CSessionBoxASTS : CBaseSessionBox, IClientSessionState
    {
		private CBaseStateSession _state;

        public CStateSess_NonActive StateSess_NonActive;
        public CStateSess_NormalTrading StateSess_NormalTrading;
        public CStateSess_Clearing StateSess_Clearing;
        public CStateSess_OpenPeriod StateSess_OpenPeriod;

        private Dictionary<string, string> _dicTymeTypesDesc = new Dictionary<string, string>();
        private CDictTimeStatus _dictTimeStatus = new CDictTimeStatus();

        //TODO remove using for capability
        public ISession _currentSession;

        private ILogable _loggerStates;
        private List<CSessionEventASTS> _eventsFuture = new List<CSessionEventASTS>();
        private List<CSessionEventASTS> _eventsPast = new List<CSessionEventASTS>();


        private Dictionary<string, bool> _instrTradingEnbl = new Dictionary<string, bool>();


        private CDBSession _currSession;

    

        //TODO remove using for capability
        public override ISession CurrentSession 
        {
            get
            {
                return _currentSession;

            }
                
                
         }

        public override bool IsPossibleCancellOrders()
        {
            return true;
        }


        public CSessionBoxASTS(IClientSessionBox client) : base(client)
        {
            _loggerStates = new CLogger("SessionBox_states", flushMode: true);
            _client = client;
            CreateStates();
        }


        public void OnInstrumentLoaded()
        {
            _client.GetInsruments().ForEach
                (instrument =>
                {
                    _instrTradingEnbl[instrument] = false;
                });




        }



        protected override void InsertUnsavedSessions()
        {

            _lstSessions.ForEach(session => _client.DBCommunicator.InsertUnsavedSessionASTS(session));

        }







		public void CreateStates()
		{
            StateSess_NonActive = new CStateSess_NonActive(this);
            StateSess_NormalTrading = new CStateSess_NormalTrading(this);
            StateSess_Clearing = new CStateSess_Clearing(this);
            StateSess_OpenPeriod = new CStateSess_OpenPeriod(this);

            _state = StateSess_NonActive;

		}




        public void OnSessionActive()
        {
            _client.IsSessionActive = true;
            Log("Session is active");




            DateTime dtBegin = new DateTime(0);
            DateTime dtEnd = new DateTime(0);


            GetDtBeginEndSession(ref dtBegin, ref dtEnd);

            /*
            if (! _lstSessions.Exists (a =>a.StockExchangedSessionId == sessionId))
            {
                _lstSessions.Add(new CDBSession
                {
                    DtBegin = dtBegin,
                    DtEnd = dtEnd,
                    StockExchangedSessionId = sessionId,
                    stock_exchange_id = _client.StockExchId
                    
                });
                                             
            }
            InsertUnsavedSessions();
            */


        }


        private void GetDtBeginEndSession(ref DateTime dtBegin, ref DateTime dtEnd)
        {
            //TODO depend on test or not test
            int mskOffset = -2;


            bool bIsTestServer = true;


            if (!bIsTestServer)
            {



            }



            DateTime dtDaySessionStart = DateTime.Now.Date.AddHours(12).AddMinutes(15).AddHours(mskOffset);
            DateTime dtDaySessionEnd = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddHours(mskOffset);


            if (_client.ServerTime > dtDaySessionStart)
            {
                dtBegin = dtDaySessionStart;
                dtEnd = dtDaySessionEnd;


            }


        }








        public void SetSessionNonActive()
        {
            _client.IsSessionActive = false;
            Log("Session is non active");
        }


        public void UpdateTimeTypes(string type, string desc)
        {

            _dicTymeTypesDesc[type] = desc;
        }





        public void OnUppdateTradeEvent(CTableRecrd record)
        {
            
            string eventType = record.values["TYPE"].ToString();
            char status = Convert.ToChar(record.values["STATUS"]);
            string statusStr = _dictTimeStatus[status];
            string stTime = GetSixCharTime(record);


            string eventDesc = "";
            if (_dicTymeTypesDesc.TryGetValue(eventType, out eventDesc))
            {
                string msg = String.Format("{0} {1} {2} {3} {4} {5}",
                                            FormatTime(record),
                                            GetKeyValue(record, "MARKETID"),
											GetKeyValue(record,"BOARDID"),
                                            GetKeyValue(record, "SECCODE"),
											 "[" + statusStr + "]",
                                             eventDesc);
                Log(msg);

                
                var instrumentRec = record.values["SECCODE"];

                //process here only COMMON events
                if (instrumentRec == null)
                {

                    UpdateSessionEvent(record, eventType, eventDesc, status, statusStr);
                    ProcessCompleteEventsCommon(stTime, eventDesc, statusStr);
                }
                //process here instrument specific (mostly for currency
                else
                {
                    ProcessEventsForInstrument(instrumentRec.ToString(), eventDesc, statusStr);
                }

            }
            else
            {
                Error("Unknown tradeevent " + eventType);
            }


        }


        private void UpdateSessionEvent(CTableRecrd record, string eventType, string eventDesc, char status, string statusStr)
        {



            CSessionEventASTS evnt = new CSessionEventASTS 
            {
                StTime = GetSixCharTime(record),
                EventType = eventType,
                EventDesc = eventDesc,
                Status = status,
                StatusStr = statusStr
                
            };




            if (statusStr == LSessionStatus.Actual)
            {
                _eventsFuture.Add(evnt);

            }
            else if (statusStr == LSessionStatus.Complete)
            {
                _eventsPast.Add(evnt);

            }

        }

        /// <summary>
        /// For now (19-July-2017) this is mostly for CURRENCY.
        /// On currency usually personal event for each instrument 
        /// send
        /// </summary>
        /// <param name="instrument"></param>
        /// <param name="eventDesc"></param>
        /// <param name="statusStr"></param>
        private void ProcessEventsForInstrument(string instrument, string eventDesc, string statusStr)
        {
            //process only complete event
            if (statusStr != LSessionStatus.Complete)
                return;

            if (_instrTradingEnbl.ContainsKey(instrument))
            {
                if (eventDesc == LSessionEvents.NormalTradingStart)
                {

                    if (!_instrTradingEnbl[instrument])
                    {
                        _instrTradingEnbl[instrument] = true;
                        Log("Enable trading for instrument " + instrument);

                    }



                }
                else if (eventDesc == LSessionEvents.NormalTradingEnd)

                    if (_instrTradingEnbl[instrument])
                    {
                        _instrTradingEnbl[instrument] = false;
                        Log("Disable trading for instrument " + instrument);
                    }

            }
      






        }

        public bool IsTradingEnabledForInstrument(string instrument)
        {
            if (!_instrTradingEnbl.ContainsKey(instrument))
                return false;

            return _instrTradingEnbl[instrument];


            //return false;

        }







		private void ProcessCompleteEventsCommon(string stTime,string eventDesc, string statusStr)
		{
         




            if (eventDesc == LSessionEvents.NormalTradingStart)
            {
                _state.OnNormalTradingStart();
                CheckForNewSessions(stTime);

            }
            else if (eventDesc == LSessionEvents.NormalTradingEnd)
            {
                _state.OnNormalTradingEnd();
                CheckForSessionComplete();
             

            }
            //else if (eventDesc == LSessionEvents.TradingClosed)
              //  _state.OnNormalTradingClosed();
           // else if (eventDesc == LSessionEvents.ClearingStart)
             //   _state.OnClearingStart();
          //  else if (eventDesc == LSessionEvents.ClearingEnd)
         //       _state.OnClearingEnd();
        //    else if (eventDesc == LSessionEvents.OpenPeriodStart)
          //      _state.OnOpenPeriodStart();
         //   else if (eventDesc == LSessionEvents.OpenPeriodEnd)
          //      _state.OnOpenPeriodEnd();
          //  else if (eventDesc == LSessionEvents.OpenClosePeriod)
            //    _state.OnOpenClosePeriod(); // open "Close period" :)

			
              

		}

        private void CheckForNewSessions(string stTime)
        {
           //TODO from global variable, from config
           //int mscOffset = -2;

           
           DateTime dt = CASTSConv.ASTSTimeToDateTime(stTime);

           DateTime dtBegin = GetSessionStartNormalTime();
           DateTime dtEnd = GetSessionEndNormalTime();

            int tolMs = 2000;
            if (CUtilTime.IsEqual(dt, dtBegin, tolMs))
            {

               _currSession    = new CDBSession
                {
                    DtBegin = dtBegin,
                    DtEnd = dtEnd,
                    stock_exchange_id = _client.StockExchId
                                      
                };
               _lstSessions.Add(_currSession);
            }

            CUtil.TaskStart(InsertUnsavedSessions);


        }

        private void CheckForSessionComplete()
        {

            if (_client.ServerTime > _currSession.DtEnd)
            {
                Thread.Sleep(0);
            }

        }



        private DateTime GetSessionStartNormalTime()
        {
            //TODO also for test/real server
            DateTime dt = DateTime.Now.Date;

            if (_client.UseRealServer)
            {

                dt.AddHours(10); //12:00
            }
            else
            {
                if (_client.StockExchId ==  CodesStockExch._02_MoexSPOT)    
                    dt = dt.AddHours(7); //7:00
                else //Currency
                    dt = dt.AddHours(7); //7:00
            }
               


            //demo                     
            return dt;
        }

        private DateTime GetSessionEndNormalTime()
        {
            //TODO also for test/real server
            //6:59 on next day



            //DateTime dt = DateTime.Now.Date.AddDays(1).AddHours(6).AddMinutes(59);
            //DateTime dt = DateTime.Now.Date.AddHours(12).AddMinutes(57);
            DateTime dt = DateTime.Now.Date;

            if (_client.UseRealServer)
            {
                dt.AddHours(18).AddSeconds(39).AddSeconds(59);
            }
            else 
            {
                //for normal
                //dt = dt.AddDays(1).AddHours(6).AddMinutes(59);
                //tempo for debug
                if (_client.StockExchId == CodesStockExch._02_MoexSPOT)
                    dt = dt.AddHours(19).AddMinutes(00);
                else //Currency
                    dt = dt.AddHours(19).AddMinutes(00);

            }


            //demo                     
            return dt;
        }




        private string GetSixCharTime(CTableRecrd record)
        {

            string rowTime = record.values["TIME"].ToString();
            if (rowTime.Length == 5)
                rowTime = "0" + rowTime;
            return rowTime;
        }


        private string FormatTime(CTableRecrd record)
        {

            string rowTime = GetSixCharTime(record);



            string formatedTime = rowTime.Substring(0, 2) + ":" + rowTime.Substring(2, 2) +
                                     ":" + rowTime.Substring(4, 2); ;

            return formatedTime;
        }

		public void SetState(CBaseStateSession newState)
		{
            _state = newState;
		}

		public void LogState(string msg)
		{
         
            _loggerStates.Log(msg);
		}


        private string GetKeyValue(CTableRecrd record, string fieldName)
        {
            string st = fieldName;
            object obj = record.values[fieldName];

            if (obj != null)
                st += "=" + obj.ToString();

            st += "; ";

            return st;
        }



        public void UpdateLstSession(int sessionId)
        {


          
            
        }

      
        

    }
}
