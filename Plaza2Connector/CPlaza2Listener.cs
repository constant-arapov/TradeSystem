using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;


using ru.micexrts.cgate;
using System.Runtime.InteropServices;
using ru.micexrts.cgate.message;

using Common;
using Common.Interfaces;
using Common.Collections;
using Common.Logger;
using Common.Utils;

using TradingLib;
using TradingLib.Enums;
using TradingLib.Common;
using TradingLib.Data;
using TradingLib.Data.DB;


//using DBCommunicator.DBData;
using TradingLib.Bots;
using TradingLib.BotEvents;

using System.Diagnostics;


namespace Plaza2Connector
{
    class CPlaza2Listener :  IAlarmable, ILogable

    {

        Listener m_listener;
        Connection m_conn;
        public string Name { set; get; }
        //CLogger m_logger;
        CLoggerBuffered m_logger;


        //private List<string> m_lstChk = new List<string>();
        //private List<int> m_IsinIds = new List<int>();

        private CPlaza2Connector m_plaza2Connector;
        private CConnection m_connParent;

        public bool Disabled { set; get; }

        public CTimer TimerRetry { set; get; }

        
        public bool IsConnected { set; get; }

        private bool _needDataLogging = true;
        //private long _lifeNum = 0;



        public void CheckOnOff()
        {
            if (StateIS == State.Closed && IsConnected == true)
            {
                IsConnected = false;
                Log("============================ Switch <IsConnected> from TRUE to FALSE");
            }

            if (StateIS == State.Active && IsConnected == false)
            {
                IsConnected = true;
                Log("=========================== Switch <IsConnected> from FALSE to TRUE");
            }

        }
        public  CPlaza2Listener(Connection con, CSettingsListener SL, CPlaza2Connector p2conn, CConnection parCon )
        {

            try
            {


                Disabled = false;
                m_connParent = parCon;

                Name = SL.Name;
                bool flushMode = false;


                _needDataLogging = SL.NeedDataLogging;


                if (Name == "ListenerUserDeals" || Name == "ListenerUserOrderLog"
                    || Name == "ListenerPosition" ||
                    Name == "ListenerPart" || Name == "ListenerVM")
                    flushMode = true;

                if (Name == "ListenerStock")
                    (new Thread(ThreadproduceLogStock)).Start();


                m_plaza2Connector = p2conn;

                //m_logger = new CLogger(Name,flushMode,"",true);
                //2017-10-23
                //m_logger = new CLoggerBuffered(Name, m_plaza2Connector.LogServer);
                m_logger = CreateLoggerBuffered(Name, m_plaza2Connector.LogServer);

                m_conn = con;
                
                m_listener = new Listener(m_conn, SL.SettingsString);


                //note: create new instance but not link

                //m_lstChk = SL.ListIsins;
                //foreach (var v in SL.ListIsins)
                  //  m_lstChk.Add(v);

                m_listener.Handler += new Listener.MessageHandler(MessageHandlerClient);


                int m_parRetryIntervalMS = 1000;
                TimerRetry = new CTimer(m_parRetryIntervalMS, OnTimerRetryExpired);

                _needDataLogging = SL.NeedDataLogging;

                Log("");
                Log("++++++++++++++++++++++++++++++++++++++++++++  Listener created++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Log("");
            }
            catch (Exception e)
            {
                string err = "Error create listener " + this.Name;
                Log(err+e.Message+" "+e.StackTrace);
                Error(err,e);

            }

        }

        private CLoggerBuffered CreateLoggerBuffered(string Name, CLogServer  logserver)
        {
            
            if (Name.Contains("ListenerOrderControlReply"))
                
            {
                int ind = Name.IndexOf("_");
                if (ind > 0)
                {
                    string subDir = Name.Substring(0,ind);
                    return new CLoggerBuffered(Name, logserver, 
                                                flushMode: true, subDir: subDir);

                }

            }

            return new CLoggerBuffered(Name, logserver);
        }



        public void Log(string message)
        {
            m_logger.Log(message);

        }

        public void Error(string description, Exception exception = null)
        {

            m_plaza2Connector.Alarmer.Error(description, exception);

        }

        public void Close()
        {
            try
            {
                if (m_listener.State != State.Closed)
                {
                    m_listener.Close();
                   // m_listener.Dispose();
                }
            }
            catch (Exception e)
            {
                string err = "Unable to close or dispose listener. " + this.Name;                    
                Log(err + e.Message);
                Error(err,e);

            }
        }


        public void Open(string settings)
        {
            try
            {

                m_listener.Open(settings);
            }
            catch (Exception e)
            {
                Error("Error open listener "+ this.Name, e);
            }
        }

        public State StateIS
        {
            get 
            {
                  return m_listener.State;
            }

        }

        public  void OnError()
        {
            if (Name == "ListenerSession")
            {}//  m_plaza2Connector.IsSessionOnline = false;
            else if (Name == "ListenerPosition")
                m_plaza2Connector.IsPositionOnline = false;
            else if (Name == "ListenerVM")
                m_plaza2Connector.IsOnlineVM = false;
            else if (Name == "ListenerDeals")
                m_plaza2Connector.IsDealsOnline = false;
            else if (Name == "ListenerUserOrderLog")
                m_plaza2Connector.IsOnlineUserOrderLog = false;
            else if (Name == "ListenerHeartBeat")
                m_plaza2Connector.IsServerTimeAvailable = false;
            else if (Name == "ListenerUserDeals")
                m_plaza2Connector.IsOnlineUserDeals = false;
            else if (Name == "ListenerStock")
                m_plaza2Connector.IsStockOnline = false;
            else if (Name == "ListenerOrderControlReply")
                m_plaza2Connector.IsOrderControlAvailable = false;

        }
        public void OnTimerRetryExpired()
        {

            Disabled = false;


        }


        public void ProcessFutInstruments(StreamDataMessage replmsg)
        {

            FUTINFO.fut_instruments fbr = new FUTINFO.fut_instruments(replmsg.Data);
            Log("isin=" + fbr.isin + " id=" + fbr.isin_id);

            //removed 2016-05-10 as new class Instruments was created

            /*
            for (int i = 0; i < m_lstChk.Count; i++ )
            {

                if (fbr.isin == m_lstChk[i])
                {
                   // m_plaza2Connector.DictIsin.Add(fbr.isin, fbr.isin_id);
                  //  m_plaza2Connector.DictIsin_id.Add(fbr.isin_id, fbr.isin);
                 //   m_plaza2Connector.DictInstruments.Add(fbr.isin, new CInstrument(fbr));

                  //  m_IsinIds.Add(fbr.isin_id);
                    m_lstChk.Remove(fbr.isin);
                    break;
                }

            }
            if (m_lstChk.Count == 0 && !m_plaza2Connector.IsDictIsinAvailable) 
                m_plaza2Connector.IsDictIsinAvailable = true;
            */
            m_plaza2Connector.Instruments.ProcessRecievedInstrument(
                                                                     new CDBInstrument { 
                                                                                        instrument = fbr.isin,
                                                                                        stock_exch_id =  m_plaza2Connector.StockExchId, /*CodesStockExch._01_MoexFORTS*/ 
                                                                                        Isin_id = fbr.isin_id,
                                                                                        Min_step = fbr.min_step,
                                                                                        Step_price = fbr.step_price, //bugfix 2018-05-21
                                                                                        RoundTo = fbr.roundto 
                                                                                            }
                                                                     
                                                                     
                                                                     
                                                                     );

        }

        /// <summary>
        /// 
        /// FORTS_INFO_REPL - additional dictin
        /// futures_params - futures parameters
        /// 
        /// 
        /// </summary>
        /// <param name="replmsg"></param>

        public void ProcessFuturesParams(StreamDataMessage replmsg)
        {
            INFO.futures_params fp = new INFO.futures_params(replmsg.Data);

            Log("isin=" + fp.isin + " id=" + fp.isin_id + " step_price=" + fp.step_price +" settl_price="+ fp.settl_price);


            //removed 2016-10-05 as not used
            /*
            for (int i = 0; i < m_lstChk.Count; i++)
            {

                if (fp.isin == m_lstChk[i])
                {
                    m_plaza2Connector.DictIsin.Add(fp.isin, fp.isin_id);
                    m_plaza2Connector.DictIsin_id.Add(fp.isin_id, fp.isin);
                    m_IsinIds.Add(fp.isin_id);

                    m_lstChk.Remove(fp.isin);
                    break;
                }

            }
           
            if (m_lstChk.Count == 0 && !m_plaza2Connector.DictIsinIsAvailable)
                m_plaza2Connector.DictIsinIsAvailable = true;
              */






        }

       

        public static T DeepClone<T>(T obj)
        {

            using (var ms = new System.IO.MemoryStream())
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(ms,obj);
                ms.Position  = 0 ;

                return (T) formatter.Deserialize(ms);



            }



        }
        private CBlockingQueue<CStockLogStruct> _bqStockLog = new CBlockingQueue<CStockLogStruct>();


        private void ThreadproduceLogStock()
        {

            while (true)
            {

                try
                {
                    CStockLogStruct sls = _bqStockLog.GetElementBlocking();
                    CRawStock rs = sls.RawStock;
                    Log("replID=" + rs.ReplID + "  replRev=" + rs.ReplRev + " ticks=" + sls.Ticks + " gdTick=" + sls.GetDataTick + " lgTck=" + sls.LogTick + "  isin_id= " + rs.Isin_id + " dir=" + rs.Dir +
                                 " price=" + rs.Price + "  volume= " + rs.Volume + " moment=" + CUtilTime.TmWithMs(rs.Moment));

                }
                catch (Exception e)
                {
                    Error("ThreadproduceLogStock",e);

                }
            }

        }

        private void LogStock(CStockLogStruct sls)
        {

            _bqStockLog.Add(sls);
           

        }


        Stopwatch swAggr = new Stopwatch();
        Stopwatch swGd = new Stopwatch();

        Stopwatch swAggrLog = new Stopwatch();
        int tmp = 0;

        public void ProcessOrdersAggr(StreamDataMessage replmsg)
        {
            swAggr.Reset();
            swAggr.Start();


            swGd.Restart();
            swGd.Start();

            AGGR.orders_aggr oa = new AGGR.orders_aggr( replmsg.Data );
            swGd.Stop();
           
            CRawStock rs = new CRawStock(oa,tmp++);
         
            
               // m_plaza2Connector.m_lastStockRevId = oa.replRev;
                                                      
              
            
                if (m_plaza2Connector.Instruments.IsMarketInstrumentsAvailable(CodesStockExch._01_MoexFORTS))
                //if (m_plaza2Connector.IsAllInstrAllMarketsAvailable)
                {
                   if (m_plaza2Connector.Instruments.DictInstrument_IsinId.ContainsValue(rs.Isin_id))
                    {

                   //     sw1.Start();
                       m_plaza2Connector.StockBoxInp.UpdateStock(rs);
                       swAggr.Stop();
                       long swAggrLogtk = swAggrLog.ElapsedTicks;
                       swAggrLog.Reset(); swAggrLog.Start();
                      /* Log("replID=" + oa.replID + "  replRev=" + oa.replRev + " ticks="+swAggr.ElapsedTicks + " lgTck="+swAggrLogtk+ "  isin_id= " + oa.isin_id + " dir=" + oa.dir +
                            " price=" + oa.price + "  volume= " + oa.volume + " moment=" + CUtil.TmWithMs(oa.moment));*/
                      /* Log("replID=" + oa.replID + "  replRev=" + oa.replRev+" ticks=" + swAggr.ElapsedTicks + " lgTck=" + swAggrLogtk +"  isin_id= " + oa.isin_id + " dir=" + oa.dir +
                            " price=" + oa.price + "  volume= " + oa.volume + " moment=" + CUtil.TmWithMs(oa.moment));*/
                       LogStock(new CStockLogStruct { RawStock=rs, Ticks = swAggr.ElapsedTicks, LogTick = swAggrLog.ElapsedTicks, GetDataTick = swGd.ElapsedTicks } );



                       swAggrLog.Stop();
                     //   sw1.Stop();
                     //   sw1.Reset();
             ///           if (sw1.ElapsedMilliseconds > 10)
                        {
                //            int tmp = 1;
                        }


                    }

                }                                                          
             
        }

       

        public void ProcessSysEvents(StreamDataMessage replmsg)
        {
            FUTINFO.sys_events se = new FUTINFO.sys_events(replmsg.Data);
            Log("SYS_EVENT event_id="+se.event_id  + " sess_id="+ se.sess_id+ " type="+se.event_type+ " message" +se.message);            

        }


        public void ProcessUserDeal(StreamDataMessage replmsg)
        {
              USER_DEAL.user_deal deal = new  USER_DEAL.user_deal  (replmsg.Data);

              m_plaza2Connector.LastUserDealRevId = deal.replRev;

              Log("replID=" + deal.replID + " replRev=" + deal.replRev + " id_deal=" + deal.id_deal + " isin_id=" + deal.isin_id + " moment=" +  CUtilTime.TmWithMs(deal.moment)  + " price=" + deal.price + " amount=" + deal.amount
                             + " id_ord_buy=" + deal.id_ord_buy + " ord_sell=" + deal.id_ord_sell + " nosystem=" + deal.nosystem + " ext_id_buy="+deal.ext_id_buy + " ext_id_sell="+deal.ext_id_sell + " code_buy=" + deal.code_buy 
                             +" code_sell="+ deal.code_sell + " code_rts_buy=" + deal.code_rts_buy +" code_rts_sell=" + deal.code_rts_sell +" fee_buy=" + deal.fee_buy+" fee_sell=" + deal.fee_sell);
                             
                              // /* +" status_buy="+ deal.status_buy + " status_sell="+deal.status_sell
                             //+ " xstatus_buy=" + deal.xstatus_buy + " xstatus_sell=" + deal.xstatus_sell*/);


            m_plaza2Connector.UserDealsPosBoxInp.Update(deal);





        }



        DateTime dt = new DateTime(0);
        bool bIsWorkingModeDeals = false;
        Stopwatch swDeal = new Stopwatch();
        Stopwatch swLgPrev = new Stopwatch();
        Stopwatch swDb = new Stopwatch();

        public void ProcessDeal(StreamDataMessage replmsg)
        {
            swDeal.Reset();
            swDeal.Start();

            DEALS.deal deal = new DEALS.deal(replmsg.Data);
            m_plaza2Connector.LastDealRevId = deal.replRev;
          
      
          //if (m_plaza2Connector.Instruments.DictInstrument_IsinId.ContainsValue(deal.isin_id))
            if (m_plaza2Connector.Instruments.IsContainsIsinId(deal.isin_id))
              if (deal.nosystem ==0) //if no system - not analyze it
            {





                string msg = "replID=" + deal.replID + " replRev=" + deal.replRev + " id_deal=" + deal.id_deal + " isin_id=" + deal.isin_id + " moment=" + CUtilTime.TmWithMs(deal.moment) + " price=" + deal.price + " amount=" + deal.amount
                           + " id_ord_buy=" + deal.id_ord_buy + " ord_sell=" + deal.id_ord_sell + " pos=" + deal.pos;// +" nosystem=";
                string marker = "";

                if (!bIsWorkingModeDeals)
                    if (m_plaza2Connector.IsDealsOnline)
                    {
                        dt = DateTime.Now;
                        bIsWorkingModeDeals = true;
                    }

                if (bIsWorkingModeDeals)
                {
                   long parDeltaStart = 3000;
                   long parDeltaDeal = m_plaza2Connector.GlobalConfig.MaxTmDiffDeal; 

                   if ((DateTime.Now - dt).TotalMilliseconds > parDeltaStart)
                    {

                        DateTime mscDt = DateTime.Now.AddHours(-2);
                         double dtDeal = (mscDt - deal.moment).TotalMilliseconds;

                         marker = " dt="+dtDeal;


                        if (dtDeal > parDeltaDeal)
                        {

                            marker += " <===";
                            Error("Old data for deal dtDeal=" + dtDeal);
                        }

                    }

                }

                swDeal.Stop();
                long prevLg = swLgPrev.ElapsedTicks;
                swLgPrev.Reset();
                swLgPrev.Start();
                
                Log(msg + " swDeal=" + swDeal.ElapsedTicks + " prevLg=" + prevLg + " swDb=" +swDb.ElapsedTicks + marker);
                swLgPrev.Stop();

                swDb.Reset();
                swDb.Restart();
                m_plaza2Connector.DealBoxInp.UpdateDeals(deal);
                swDb.Stop();

            }
      
        }

        private void WriteStreamStructure(StreamDataMessage replmsg)
        {
            Value[] v = ((ru.micexrts.cgate.message.AbstractDataMessage)(replmsg)).Fields;
            string st = "STREAM STRUCTURE "; 
              foreach   (Value field in  replmsg.Fields)
              {
                  st += field.ToString() + " ";   
                  
              }
           
        }

        private string GetOrderstatusString(int status)
        {

          /*  if (status == 0x01)
                return "Котировочная";
            else if (status == 0x02)
                return "Встречная";
            else if (status == 0x04)
                return "Внесистемная";
            else if (status == 0x1000)
                return "Последняя в транзакции";
            else if (status == 0x100000)
                return "Рез-т перемещения заявки";
            else if (status == 0x200000)
                return "Рез-т удаления заявки";
            else if (status == 0x400000)
                return "Рез-т группового удаления";
            else if (status == 0x20000000)
                return "Уд-е остатка,  кросс-сделка";
            else if (status == 0x00080000)
                return "Заявка  Fill-or-kill";

            */



            return status.ToString();
        }


      



        private void ProcessCommonOrdersLog(StreamDataMessage replmsg)
        {
            ORDLOG.orders_log ol = new ORDLOG.orders_log(replmsg.Data);

            if (m_plaza2Connector.Instruments.IsMarketInstrumentsAvailable(CodesStockExch._01_MoexFORTS))
            //if (m_plaza2Connector.IsAllInstrAllMarketsAvailable)
            {
                //if (m_plaza2Connector.DictInstr_IsinId.ContainsValue(ol.isin_id)
                if (m_plaza2Connector.Instruments.IsContainsIsinId(ol.isin_id))
                {

                    /*
                    Log(" replRev=" + ol.replRev + " moment=" + CUtilTime.TmWithMs(ol.moment) + " isin_id=" + ol.isin_id + " id_ord=" + ol.id_ord + " sess_id=" + ol.sess_id + " price=" + ol.price +
                                    " dir=" + ol.dir + " action=" + m_plaza2Connector.GetActionString(ol.action) + " amount=" + ol.amount + " amount_rest=" + ol.amount_rest + " id_deal=" + ol.id_deal +
                                          " deal_price=" + ol.deal_price + " status=" + GetOrderstatusString(ol.status));//+" xstatus=" + ol.xstatus);
                    */
                }
            }


        }

        private void ProcessOrdersLog(StreamDataMessage replmsg)
        {

            if (Name == "ListenerUserOrderLog") ProcessUserOrdersLog(replmsg);
            else if (Name == "ListenerOrderLog") ProcessCommonOrdersLog(replmsg);
            else Log("Error ! Unknown order_log.");



        }


        private void ProcessUserOrdersLog(StreamDataMessage replmsg)
        {
            FUTTRADE.orders_log ol = new FUTTRADE.orders_log(replmsg.Data);

            m_plaza2Connector.LastUserOrderLogRevId = ol.replRev;
            Log("replId=" + ol.replID + " replRev=" + ol.replRev + " ext_id=" + ol.ext_id + " moment=" + CUtilTime.TmWithMs(ol.moment) + " isin_id=" + ol.isin_id + " id_ord=" + ol.id_ord + " sess_id=" + ol.sess_id + " price=" + ol.price +
                                    " dir=" + ol.dir + " action=" + m_plaza2Connector.GetActionString(ol.action) + " amount=" + ol.amount + " amount_rest=" + ol.amount_rest + " id_deal=" + ol.id_deal  + " deal_price=" + ol.deal_price + 
                                    " status=" + GetOrderstatusString(ol.status) );//+" xstatus=" + ol.xstatus);      


                    m_plaza2Connector.UserOrderBoxInp.Update(ol);

        }

        int cnt = 0; long  sum;
        private void ProcessHeartBeat(StreamDataMessage replmsg)
        {
            if (this.Name != "ListenerHeartBeat") return;
       
            FUTTRADE.heartbeat hb = new FUTTRADE.heartbeat(replmsg.Data);
            m_plaza2Connector.ServerTime = hb.server_time;

            m_plaza2Connector.SetServerTimeAvailable();


            //TO DO check this time is sensible
            m_plaza2Connector.IsServerTimeAvailable = true;

            DateTime tm = m_plaza2Connector.ServerTimeLocal();
            long dtMs = (long) (tm - hb.server_time).TotalMilliseconds;

            string marker = "";

            int parMaxTmDiff = m_plaza2Connector.GlobalConfig.MaxTmDiffHeartBeat; //  200;// 1000;

            if (m_plaza2Connector.IsTimeSynchronized)
                if (dtMs > parMaxTmDiff)
                {
                marker = " <============";
                Error("Time difference from server is more than max "+ dtMs);
                }
            
            string msg = "replID="+hb.replID + " replRev="+ hb.replRev + " local_time=" + DateTime.Now + "." + DateTime.Now.Millisecond + " server_time=" + hb.server_time + "." + hb.server_time.Millisecond + " server_time_local=" + tm + "." + tm.Millisecond + " delta_ms=" + dtMs +marker;

            Log(msg);

            m_plaza2Connector.GUIBox.ServerTime = hb.server_time;//.ToString();

            if (!m_plaza2Connector.IsTimeSynchronized)
            {

                cnt++;
                sum += dtMs;
                double avDelta = sum / cnt;

                if (cnt > 10)
                {

                    CTimeChanger th = new CTimeChanger((int)-avDelta);
                    m_plaza2Connector.IsTimeSynchronized = true;

                    Log("Time was synchronised");


                }


            }
          


        }

        private void ProcessPart(StreamDataMessage replmsg)
        {
            PART.part prt = new PART.part(replmsg.Data);
            Log("PART. replID="+prt.replID + " replRev="+prt.replRev +  " client_code=" + prt.client_code + " money_amount="+prt.money_amount + 
                            " money_free="+prt.money_free +  " money_old="+prt.money_old +  " money_blocked=" + prt.money_blocked + " balance_money=" +prt.balance_money + " vm_reserve=" + prt.vm_reserve+
                              " vm_intercl=" + prt.vm_intercl + " fee=" + prt.fee + " coef_go=" + prt.coeff_go);
                           

            m_plaza2Connector.Part = new CPart(prt);
            m_plaza2Connector.GUIBox.UpdatePart((PART.part)prt.Copy());
        }

        private void ProcessPosition(StreamDataMessage replmsg)
        {
           
            try
            {
                POS.position pos = new POS.position(replmsg.Data);
               
                Log("POS replID=" + pos.replID + " replRev=" + pos.replRev + " client_code=" + pos.client_code + " isin_id=" + pos.isin_id + " pos =" + pos.pos +
                                " open_qty=" + pos.open_qty + " buys_qtty=" + pos.buys_qty + " sells_qtty=" + pos.sells_qty + " net_volume_rur=" + pos.net_volume_rur +
                                " waprice=" + pos.waprice.ToString() + " last_deal_id=" + pos.last_deal_id);
                
                m_plaza2Connector.PositionBoxInp .Update(pos);

                string isin = m_plaza2Connector.Instruments.GetInstrumentByIsinId(pos.isin_id);
            
               m_plaza2Connector.GUIBox.ExecuteWindowsUpdate(new Action(() => m_plaza2Connector.GUIBox.UpdatePos(isin, new CRawPosition(pos))));

            }
            catch (Exception e)
            {
                string err = "Unable process position "; ;
                Log(err + e.Message+ " "+e.StackTrace);
                Error(err,e);



            }
            
        }


        private void ProcessSession(StreamDataMessage replmsg)
        {

            FUTINFO.session  ses  = new FUTINFO.session  (replmsg.Data);

            Log("sess_id=" + ses.sess_id + " begin=" + ses.begin + " end=" + ses.end + " state=" + ses.state + " inter_cl_begin=" + ses.inter_cl_begin + " inter_cl_end=" + ses.inter_cl_end + " ses.inter_cl_state=" + ses.inter_cl_state + 
                           " eve_on=" + ses.eve_on +" eve_begin="+ ses.eve_begin + " eve_end="+ses.eve_end + " mon_on="+ses.mon_on + " mon_begin="+ses.mon_begin+" mon_end="+ses.mon_end+ " transfer_begin="+ses.pos_transfer_begin +
                            " transfer_end="+ses.pos_transfer_end);
            Log("");
            m_plaza2Connector.SessionBoxInp.Update(ses);



        }
        private void ProcessInvestr(StreamDataMessage replmsg)
        {
            FUTINFO.investr inv = new FUTINFO.investr(replmsg.Data);
            Log("investr replID="+inv.replID + " replRev="+inv.replRev  +" replAct=" +inv.replAct+ " client_code="+inv.client_code + " name="+ inv.name + " status="+inv.status);




            m_plaza2Connector.UpdateClientCode(inv.client_code);
        }

        /// <summary>
        /// FORTS_FUTINFO_REPL - futures dictionaries and session info
        /// fut_sess_contents tradinf instrument dictionaties
        /// 
        /// </summary>
        /// <param name="replmsg"></param>
        private void ProcessFutSessContents(StreamDataMessage replmsg)
        {

            FUTINFO.fut_sess_contents cont = new FUTINFO.fut_sess_contents (replmsg.Data);


          Log("replID="+ cont.replID + " replRev=" + cont.replRev +   " sess_id="+ cont.sess_id + " isin_id="+ cont.isin_id + " isin="+ cont.isin +
              " step_price=" + cont.step_price +   " step_price_clr="+ cont.step_price_clr+     " step_price_interclr=" + cont.step_price_interclr + 
              " step_price_curr=" + cont.step_price_curr +" step_price_scale=" + cont.step_price_scale + 
              " is_limited=" + cont.is_limited              + " limit_up=" + cont.limit_up + " limit_down=" + cont.limit_down + " last_cl_quote=" + cont.last_cl_quote);

            string isin  = cont.isin;
            // note that we recieve it two times for different sessions
            //if (m_plaza2Connector.DictInstr_IsinId.ContainsKey(isin))
            if (m_plaza2Connector.Instruments.IsContainsInstrument(isin))
            {
                m_plaza2Connector.DictFutLims[isin] = new CFutLims(cont.last_cl_quote, cont.limit_up, cont.limit_down);
                m_plaza2Connector.DictStepPrice[isin] = cont.step_price;//cont.step_price_curr;
                m_plaza2Connector.DictMinStep[isin] = cont.min_step;

            }
           
        }


        private void ProcessFutVm(StreamDataMessage replmsg)
        {

            try
            {
                FORTS_VM_REPL.fut_vm fut_vm = new FORTS_VM_REPL.fut_vm(replmsg.Data);

                Log(" replID=" + fut_vm.replID + " replRev=" + fut_vm.replRev + " session_id=" + fut_vm.sess_id + " fut_vm.isin_id=" + fut_vm.isin_id + " vm=" + fut_vm.vm + " vm_real=" + fut_vm.vm_real);

                //if (m_plaza2Connector.DictIsin_id.ContainsKey(fut_vm.isin_id))
                if (m_plaza2Connector.Instruments.IsContainsIsinId(fut_vm.isin_id))
                {
                    //string isin = m_plaza2Connector.DictIsin_id[fut_vm.isin_id];
                    string isin = m_plaza2Connector.Instruments.GetInstrumentByIsinId(fut_vm.isin_id);

                    lock (m_plaza2Connector.DictVM)
                    {
                        m_plaza2Connector.DictVM[isin] = fut_vm.vm;
                    }

                    m_plaza2Connector.GUIBox.ExecuteWindowsUpdate(new Action(() => m_plaza2Connector.GUIBox.UpdateVM(isin, (FORTS_VM_REPL.fut_vm)fut_vm.Copy())));
                }

            }
            catch (Exception e)
            {
                Error("Error ProcessFutVm",e);

            }


        }

        private void ProcessUsdOnline(StreamDataMessage replmsg)
        {

            FUTINFO.usd_online usd = new FUTINFO.usd_online(replmsg.Data);

            Log("replUD=" + usd.replID + " replRev=" + usd.replRev + " moment=" + CUtilTime.TmWithMs(usd.moment) + " id=" + usd.id + " rate=" + usd.rate);


            m_plaza2Connector.USDRate = usd.rate;



        }

        private bool IsUsedBoard(string board)
        {
            //TODO check AETS
            if (board == "TQBR" || board == "TQDE" ||
                board == "CETS")
                return true;

            return false;
        }



        private void ProcessASTSCurrOrderBook(StreamDataMessage replmsg)
        {

          

         AstsCCAggr.ORDERBOOK ob = new AstsCCAggr.ORDERBOOK(replmsg.Data);
      
         if (IsUsedBoard(ob.SECBOARD))
           if (m_plaza2Connector.Instruments.IsMarketInstrumentsAvailable(CodesStockExch._03_MoexCurrency))
               if (m_plaza2Connector.Instruments.IsContainsInstrument(ob.SECCODE))
                   if (m_plaza2Connector.StockBox != null)
                 

           {  
             
            if (_needDataLogging)
               Log("replID=" + ob.replID + " replRev=" + ob.replRev + " SECCODE=" + ob.SECCODE + " BUYSELL=" + ob.BUYSELL + 
                   " PRICE=" + ob.PRICE + " QUANTITY=" + ob.QUANTITY + " SECBOARD=" + ob.SECBOARD);

                   m_plaza2Connector.StockBoxInp.UpdateStock(ob.SECCODE,
                                    new CRawStock 
                                        {
                                            Dir = ob.BUYSELL == "B" ? (sbyte)Direction.Down : (sbyte)Direction.Up,
                                             Price = ob.PRICE,
                                              Volume = ob.QUANTITY,
                                              ReplID = ob.replID,
                                              ReplRev = ob.replRev
                                        }

                                                        );                         
                                             
                    

            }
         
        }


        private void ProcessASTSSpotOrderBook(StreamDataMessage replmsg)
        {


            AstsSpotAggr.ORDERBOOK ob = new AstsSpotAggr.ORDERBOOK(replmsg.Data);


            if (IsUsedBoard(ob.SECBOARD))
             
            {
                Log("replID=" + ob.replID + " replRev=" + ob.replRev + " SECCODE=" + ob.SECCODE + " BUYSELL=" + ob.BUYSELL + " PRICE=" + ob.PRICE + " QUANTITY=" + ob.QUANTITY + " SECBOARD=" + ob.SECBOARD);
            }

              //= new AstsCCAggr.ORDERBOOK(replmsg.Data);

            


        }


      

        private void ProcessASTCurrAllTrades(StreamDataMessage replmsg)
        {
            AstsCCTrade.ALL_TRADES at = new AstsCCTrade.ALL_TRADES(replmsg.Data);


            m_plaza2Connector.LastASTSCurrDealRevId = at.replRev;


            if (IsUsedBoard (at.SECBOARD))
                if (m_plaza2Connector.Instruments.IsMarketInstrumentsAvailable(CodesStockExch._03_MoexCurrency))
                    if (m_plaza2Connector.Instruments.IsContainsInstrument(at.SECCODE))
            {
            if (_needDataLogging)
            Log("replID=" + at.replID + " replRev=" + at.replRev + " TRADETIME=" + at.TRADETIME + " MICROSEC=" + at.MICROSECONDS + " SECCODE=" + at.SECCODE + " BUYSELL=" + at.BUYSELL + " PRICE=" + at.PRICE + " QUANTITY=" + at.QUANTITY + " VALUE=" + at.VALUE
                  + " SETTLEDATE=" + at.SETTLEDATE  + " SECBOARD=" + at.SECBOARD);


               if (m_plaza2Connector.DealBox!=null)
                    m_plaza2Connector.DealBoxInp.UpdateDeals(at.SECCODE,at);
            }

        }


        private void ProcessASTSSpotAllTrades(StreamDataMessage replmsg)
        {

            

            
            AstsSpotTrade.ALL_TRADES at = new AstsSpotTrade.ALL_TRADES(replmsg.Data);

            if (IsUsedBoard(at.SECBOARD))
            {
                
                Log("replID=" + at.replID + " replRev=" + at.replRev + " TRADETIME=" + at.TRADETIME + " MICROSEC=" + at.MICROSECONDS + " SECCODE=" + at.SECCODE + " BUYSELL=" + at.BUYSELL + " PRICE=" + at.PRICE + " QUANTITY=" + at.QUANTITY + " VALUE=" + at.VALUE
                     + " SETTLEDATE=" + at.SETTLEDATE + " SECBOARD=" + at.SECBOARD);




            }
           /*
            if (at.PRICE < 0 && at.SECCODE.Contains("TATN "))
            {
                System.Threading.Thread.Sleep(0);
               
            //    if (t != 0)
                    System.Threading.Thread.Sleep(0);


            }
            */
            //Log("replID=" + ob + " replRev=" + ob.replRev + " SECCODE=" + ob.SECCODE + " BUYSELL=" + ob.BUYSELL + " PRICE=" + ob.PRICE + " QUANTITY=" + ob.QUANTITY + " SECBOARD=" + ob.SECBOARD);


        }





        private void ProcessASTSCurrSecurities(StreamDataMessage replmsg)
        {


            AstsCCInfo.SECURITIES sec = new AstsCCInfo.SECURITIES(replmsg.Data);


            if (IsUsedBoard(sec.SECBOARD))
            {

                Log("replUD=" + sec.replID + " replRev=" + sec.replRev + " SECCODE =" + sec.SECCODE + " SECNAME=" + sec.SECNAME + " MARKETCODE=" +
                    sec.MARKETCODE + " LOTSIZE=" + sec.LOTSIZE + " MINSTEP=" + sec.MINSTEP + " DECIMALS=" + sec.DECIMALS + " SECBOARD=" + sec.SECBOARD);



                m_plaza2Connector.Instruments.ProcessRecievedInstrument(new CDBInstrument
                                                                            {
                                                                                stock_exch_id =  m_plaza2Connector.StockExchId,
                                                                                instrument = sec.SECCODE,
                                                                                //stock_exch_id = CodesStockExch._03_MoexCurrency,
                                                                                Min_step = sec.MINSTEP,
                                                                                RoundTo = sec.DECIMALS




                                                                            }

                                                                                        );

            }

            //System.Threading.Thread.Sleep(0);


        }

        private void ProcessASTSSPOTSecurities(StreamDataMessage replmsg)
        {
            AstsSpotInfo.SECURITIES sec = new AstsSpotInfo.SECURITIES(replmsg.Data);
            if (IsUsedBoard(sec.SECBOARD))
            {
                Log("replID=" + sec.replID + " replRev=" + sec.replRev + " ISIN=" + sec.ISIN + " SECCODE =" + sec.SECCODE + " SECNAME=" + sec.SECNAME + " MARKETCODE=" + sec.MARKETCODE + " LOTSIZE=" + sec.LOTSIZE);
            }

        }



        private void ProcessOrderAddReply(DataMessage dm, int botId)
        { 
            MESSAGE.FORTS_MSG101 msg101 = new MESSAGE.FORTS_MSG101(dm.Data); 
            Log("code="+msg101.code + " message="+msg101.message + " order_id="+msg101.order_id);
            long order_id = msg101.order_id;

            if (msg101.code == 0)
            {

                foreach (CBotBase bb in m_plaza2Connector.ListBots)
                {
                    if (bb.BotId == botId)
                        bb.Recalc("", EnmBotEventCode.OnOrderAdded, (object)msg101);
                  


                }

            }
            else if (msg101.code == 31)
            {
                Error("Cross order found botId="+botId);
                
                m_plaza2Connector.OnCrossOrderReply(botId);

            }
            else
            {
                //TO DO remove - nothing to do
                System.Threading.Tasks.Task tsk = new System.Threading.Tasks.Task(() => TaskCheckIdInUserOrdLog(order_id));
                tsk.Start();

            }

        }


     


        private void TaskCheckIdInUserOrdLog(long id_ord)
        {

            Log("TaskCheckIdInUserOrdLog started. id_ord=" + id_ord);
        
            const int TRIAL_COUNT = 100;
            const int TIME_SLEEP = 30;
            for (int i = 0; i < TRIAL_COUNT; i++)
            {
                lock (m_plaza2Connector.UserOrderBox.ListRawOrdersStruct)
                {
                    try
                    {
                        for (int j = m_plaza2Connector.UserOrderBox.ListRawOrdersStruct.Count - 1; j > 0; j--)
                        {
                            if (m_plaza2Connector.UserOrderBox.ListRawOrdersStruct[j].Id_ord == id_ord)
                            {

                           

                               int ext_id =m_plaza2Connector.UserOrderBox.ListRawOrdersStruct[j].Ext_id;
                               long isin_id = m_plaza2Connector.UserOrderBox.ListRawOrdersStruct[j].Isin_Id;
                               string isin = m_plaza2Connector.Instruments.GetInstrumentByIsinId(isin_id);
                               decimal price = m_plaza2Connector.UserOrderBox.ListRawOrdersStruct[j].Price;
                               sbyte dir = m_plaza2Connector.UserOrderBox.ListRawOrdersStruct[j].Dir;
                               decimal amount = m_plaza2Connector.UserOrderBox.ListRawOrdersStruct[j].Amount;

                            if (m_plaza2Connector.UserOrderBox.ListRawOrdersStruct[j].Action == (sbyte)EnmOrderAction.Added)                               
                                {

                                        foreach (CBotBase  bt in m_plaza2Connector.ListBots)
                                     {
                                         if (bt.BotId == ext_id)
                                         {
                                        /*     BotEventOrderAccepted ao = new BotEventOrderAccepted(id_ord, price, dir, amount); 
                                             bt.Recalc(isin, EnmBotEventCode.OnOrderAccepted, ao);
                                         */

                                         }


                                     }



                                   
                                    Log("id_ord=" + id_ord + " was found in OrdersLog. Trigger order added");
                                    //TO DO trigger add order action
                                }


                                return;
                            }


                        }
                    }
                    catch (Exception e)
                    {
                        string err = "Error in  TaskCheckIdInUserOrdLog. ";
                        Log(err+e.Message);
                        Error(err,e);

                    }



                }

                System.Threading.Thread.Sleep(TIME_SLEEP);
            }

            Log("Error ! Order id was not added to UserOrdersLog");

        }

       
        private void ProcessOrderDelReply(DataMessage dm)
        {
            MESSAGE.FORTS_MSG102 msg102 = new MESSAGE.FORTS_MSG102(dm.Data);
            Log("code="+msg102.code + " message=" + msg102.message + " amount="+msg102.amount);


        }

        private void ProcessOrderUserDelReply(DataMessage dm)
        {
            MESSAGE.FORTS_MSG103 msg103 = new MESSAGE.FORTS_MSG103(dm.Data);
            Log("code=" + msg103.code + " message=" + msg103.message + " num_orders=" + msg103.num_orders);


        }

        private void TriggerSynchronizeBotsOrders()
        {
            foreach (CBotBase bt in m_plaza2Connector.ListBots)            
                bt.Recalc("", EnmBotEventCode.OnUserOrdersOnline, null);
            
        }

        private void TriggerSynchronizeBotsPositions()
        {

            foreach (CBotBase bt in m_plaza2Connector.ListBots)
                bt.Recalc("", EnmBotEventCode.OnPositionOnline, null);
                           

        }

        private void TriggerSynchronizeDeals()
        {
            foreach (CBotBase bt in m_plaza2Connector.ListBots)
                bt.Recalc("", EnmBotEventCode.OnUserDealOnline, null);


        }


        




        public int MessageHandlerClient(Connection conn, Listener listener, Message msg)
        {
            try
            {
             
                    //   m_logger.Write("msg.Type=" + msg.Type);

                    switch (msg.Type)
                    {
                        case MessageType.MsgStreamData:
                            {
                                
                                StreamDataMessage replmsg = (StreamDataMessage)msg;
                                //   m_logger.Write(String.Format("Stream DATA message SEQ={0} [table:[idx={1}, id={2}, name={3}], dataSize:{4}]", replmsg.Rev, replmsg.MsgIndex, replmsg.MsgId, replmsg.MsgName, msg.Data.Length));
                                WriteStreamStructure(replmsg);
                                string st = msg.ToString();

                                switch (replmsg.MsgName)
                                {
                                    // -- FORTS
                                    case "fut_instruments": ProcessFutInstruments(replmsg); break;
                                    case "fut_sess_contents": 
                                        ProcessFutSessContents(replmsg); break;
                                    case "fut_vm": ProcessFutVm(replmsg) ; break;
                                    case "futures_params": ProcessFuturesParams(replmsg); break;
                                    case "orders_aggr": ProcessOrdersAggr(replmsg); break;
                                    case "sys_events": ProcessSysEvents(replmsg); break;
                                    case "deal": ProcessDeal(replmsg); break;
                                    case "user_deal": ProcessUserDeal(replmsg); break;
                                    case "orders_log": ProcessOrdersLog(replmsg); break;
                                    case "part": 
                                        ProcessPart(replmsg); break;
                                    case "position": ProcessPosition(replmsg); break;
                                    case "investr": ProcessInvestr(replmsg); break;
                                    case "session": ProcessSession(replmsg); break;
                                    case "heartbeat": ProcessHeartBeat(replmsg); break;
                                    case "usd_online": ProcessUsdOnline(replmsg); break;                                    

                                    //--ASTS
                                    case "SECURITIES": if (Name == ConstP2.ListenerASTSCurrSECURITIES)
                                                        ProcessASTSCurrSecurities(replmsg);

                                        else if (Name == ConstP2.ListenerASTSSpotSECURITIES)
                                                        ProcessASTSSPOTSecurities(replmsg);                    
                                                
                                                        break;

                                    case "ORDERBOOK":   if (Name == ConstP2.ListenerASTSCurrOrerbook)
                                                            ProcessASTSCurrOrderBook(replmsg);
                                                        else if (Name == ConstP2.ListenerASTSSpotOrerbook)
                                                            ProcessASTSSpotOrderBook(replmsg);


                                                        break;


                                    case "ALL_TRADES": if (Name == ConstP2.ListenerASTSCurrALL_TRADES)
                                                            ProcessASTCurrAllTrades(replmsg);
                                                        else if (Name == ConstP2.ListenerASTSSpotALL_TRADES)
                                                            ProcessASTSSpotAllTrades(replmsg);

                                                        break;
                                    default:
                                        Thread.Sleep(0);
                                    break;
                                }

                               


                                


                                break;
                            }
                        case MessageType.MsgData:
                            {
                                DataMessage dm = (DataMessage)msg;

                                switch (dm.MsgName)
                                {
                                    case "FORTS_MSG101":
                                       
                                        ProcessOrderAddReply(dm, m_connParent.BotId); 
                                        
                                        break;
                                    case "FORTS_MSG102": ProcessOrderDelReply(dm); break;
                                    case "FORTS_MSG103": ProcessOrderUserDelReply(dm); break;
                                }

                                break;
                            }

                        case MessageType.MsgP2ReplOnline:
                            {
                                Log("======================================== ONLINE");

                                if (Name == "ListenerUserOrderLog")
                                {
                                    if (!m_plaza2Connector.IsOnlineUserOrderLog)
                                    {
                                        TriggerSynchronizeBotsOrders();
                                        m_plaza2Connector.IsOnlineUserOrderLog = true;
                                    }
                                    
                                }
                                if (Name == "ListenerOrderControlReply")
                                {

                                 //   m_plaza2Connector.IsOrderControlAvailable = true;
                                }
                                if (Name == "ListenerPosition")
                                {
                                 
                                    TriggerSynchronizeBotsPositions();
                                    m_plaza2Connector.IsPositionOnline = true;
                                    m_plaza2Connector.EvPosOnline.Set();

                                }
                                if (Name == "ListenerUserDeals")
                                {
                                    if (!m_plaza2Connector.IsOnlineUserDeals)
                                    {

                                        TriggerSynchronizeDeals();
                                        m_plaza2Connector.IsOnlineUserDeals = true;

                                        

                                    }

                                }
                                if (Name == "ListenerStock")
                                {                           
                                    m_plaza2Connector.IsStockOnline = true;                                 
                                 //  m_plaza2Connector.StockBox.UpdateAllBidAsk();
                                    m_plaza2Connector.EvStockOnline.Set();
                                   
                                }
                                if (Name == "ListenerSession")
                                {
                                    m_plaza2Connector.IsSessionOnline = true;
                                }

                                if (Name == "ListenerVM")
                                {
                                    m_plaza2Connector.IsOnlineVM = true;

                                }
                                if (Name == "ListenerDeals")
                                {

                                    m_plaza2Connector.IsDealsOnline = true;
                                    m_plaza2Connector.DealBox.UpdateAllDealStructLastData();
                                    m_plaza2Connector.EvDealsOnline.Set();
                                    //m_plaza2Connector.DealBox.
                                }
                                if (Name == "ListenerFutInfo")
                                {

                                    m_plaza2Connector.IsFutInfoOnline = true;
                                    
                                    //m_plaza2Connector.DealBox.
                                }




                                break;
                            }
                        case MessageType.MsgTnBegin:
                            {
                                if (Name == "ListenerStock" || Name == "ListenerDeals" || Name == "ListenerASTSCurrOrerbook")
                                    Log("====================================== TN Begin");
                                //    m_logger.Write ("====================================== TN Begin");
                                break;
                            }
                        case MessageType.MsgTnCommit:
                            {
								if (Name == "ListenerStock" || Name == "ListenerDeals" || Name == "ListenerASTSCurrOrerbook")
                                   Log("====================================== TN Commit");
                                //    Console.WriteLine("===================================== TN Commit");
                                break;
                            }
                        case MessageType.MsgOpen:
                            {

                                if (Name == "ListenerStock" || Name == "ListenerDeals")
                                        Log("====================================== Msg.Open");

                                /*
                              //  Console.WriteLine("=====================================OPEN");
                                {
                                    SchemeDesc schemeDesc = listener.Scheme;
                                    if (schemeDesc != null)
                                    {
                                        foreach (MessageDesc messageDesc in schemeDesc.Messages)
                                        {
                                            Console.WriteLine(String.Format("Message {0}, block size = {1}", messageDesc.Name, messageDesc.Size));
                                            foreach (FieldDesc fieldDesc in messageDesc.Fields)
                                            {
                                                Console.WriteLine(String.Format("Field {0} = {1} [size={2}, offset={3}]", fieldDesc.Name, fieldDesc.Type, fieldDesc.Size, fieldDesc.Offset));
                                            }
                                        }
                                    }
                                }*/
                                break;
                            }
                        case  MessageType.MsgP2ReplClearDeleted:
                            {


                                P2ReplClearDeletedMessage dm = (P2ReplClearDeletedMessage)msg;
                           //     m_logger.Write("MsgP2ReplClearDeleted: TableIdx=" + dm.TableIdx + " TableRev=" + dm.TableRev);



                                //long rev = ((P2ReplClearDeletedMessage)msg).TableRev;



                            }
                            break;

                        case MessageType.MsgClose:
                            {
                               // Console.WriteLine("CLOSE");
                                Log("====================================== Msg.Close");
                                break;
                            }
                        case MessageType.MsgP2ReplLifeNum:
                            {
                                if (this.Name == "ListenerDeals")                                
                                    m_plaza2Connector.LastDealLifeNum = ((P2ReplLifeNumMessage)msg).LifeNumber;
                                else if (this.Name == "ListenerASTSCurrALL_TRADES")
                                    m_plaza2Connector.LastASTSCurrDealLifeNum = ((P2ReplLifeNumMessage)msg).LifeNumber;
                                
                                //Log(String.Format("MsgP2ReplLifeNum:  Life number changed to: {0}", ((P2ReplLifeNumMessage)msg).LifeNumber));
                                break;
                            }
                        case MessageType.MsgP2ReplReplState:
                            {
                               

                             //   Log("====================================== Msg.ReplState");

                             //   m_logger.Write(String.Format("MsgP2ReplReplState: Message {0}", ((P2ReplStateMessage)msg).ReplState));
                                break;
                            }




                        default:
                            {
                                Console.WriteLine(String.Format("Message {0}", msg.Type));
                                break;
                            }

                    }
                    //System.Threading.Thread.Sleep(1);
                   // m_connParent.mxMessage.ReleaseMutex();
                    return 0;
                }
          
            catch (CGateException e)
            {
                Error("MessageHandler",e);

                return (int)e.ErrCode;
            }
        }






    }
}
