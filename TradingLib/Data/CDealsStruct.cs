using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;


using Common;
using Common.Interfaces;
using Common.Collections;
using Common.Utils;



using TradingLib.Interfaces.Components;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;

using TradingLib.Bots;
using TradingLib.BotEvents;

namespace /*Plaza2Connector*/ TradingLib.Data
{
    public class CDealsStruct : IAlarmable, INotifyPropertyChanged
    {

        Thread ThreadProcessing;
    

        CBlockingQueue<CRawDeal> m_bqRawDeal = new CBlockingQueue<CRawDeal>();


        List<CBotBase> ListBot;


        List<string> ListSubscribersBotNames = new List<string>();
        System.Threading.Mutex mxListSubscribersBotNames = new System.Threading.Mutex();

        /*CPlaza2Connector*/
        IClientDealsStruct m_plaza2Connector;


        private string m_isin;
        //private long m_isin_id;


        private CTimeFrameAnalyzer m_TimeFrameAnalyzer;

        public List<CRawDeal> ListDeals { get; set; }


        public CTimeFrameAnalyzer TimeFrameAnalyzer
        {
            get
            {
                return m_TimeFrameAnalyzer;
            }
        }
       

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }


        private DateTime _timeLastDeal;
        public DateTime TimeLastDeal
        {

            get
            {
                return _timeLastDeal;

            }
            set
            {
                _timeLastDeal = value;
                RaisePropertyChanged("TimeLastDeal");
            }

        }


        private decimal _lastSellPrice;
        public decimal LastSellPrice
        {

            get
            {
                return _lastSellPrice;


            }
            set
            {
                _lastSellPrice = value;
                RaisePropertyChanged("LastSellPrice");

            }

        }

        private decimal _lastBuyPrice;
        public decimal LastBuyPrice
        {

            get
            {
                return _lastBuyPrice;


            }
            set
            {
                _lastBuyPrice = value;
                RaisePropertyChanged("LastBuyPrice");

            }

        }


        public decimal _guiOpenedInterest;
        public decimal GUIOpenedInterest
        {

            get
            {
                return _guiOpenedInterest;


            }
            set
            {
                if (_guiOpenedInterest != value)
                {
                    _guiOpenedInterest = value;
                    RaisePropertyChanged("GUIOpenedInterest");
                }
            }

        }






        private void UpdateTimeLastDeal(DateTime dt)
        {
            try
            {
               if (m_plaza2Connector.IsDealsOnline)
                    if (!CUtilTime.IsEqualTimesSecondsAcc(dt, TimeLastDeal))
                       // m_plaza2Connector.GUIBox.ExecuteWindowsUpdate(new Action(() => TimeLastDeal = dt));
                        m_plaza2Connector.GUIBox.ExecuteWindowsDealsUpdate(new Action(() => TimeLastDeal = dt));
                       // TimeLastDeal = dt;
                
            }
            catch (Exception e)
            {
                Error("UpdateTimeLastDeal",e);


            }



        }


        private void FillSubscribedBotNames()
        {

            foreach (CBotBase bb in m_plaza2Connector.ListBots)
            {
                if (bb.SettingsBot.ListIsins.Contains(m_isin))
                    ListSubscribersBotNames.Add(bb.Name);


            }



        }



        public CDealsStruct(/*long isin_id,*/ string isin, List<CBotBase> listBot, /*CPlaza2Connector*/IClientDealsStruct plaza2Connector)
        {
            m_isin = isin;
            //m_isin_id = plaza2Connector.Instruments.DictInstrument_IsinId[isin];
            ThreadProcessing = new Thread(ThreadFunc);
            ThreadProcessing.Start();
            ListBot = listBot;

            m_plaza2Connector = plaza2Connector;


            FillSubscribedBotNames();
            if (plaza2Connector.AnalzyeTimeFrames)
                m_TimeFrameAnalyzer = new CTimeFrameAnalyzer(m_isin, (IClientTimeFrameAnalyzer) m_plaza2Connector );

            ListDeals = new List<CRawDeal>();



        }
        private Dictionary<int, CRawDeal> m_RawDeals = new Dictionary<int, CRawDeal>();

        public void Error(string description, Exception exception = null)
        {
            m_plaza2Connector.Error(description, exception);            

        }


        public void Update(CRawDeal rd)
        {

            m_bqRawDeal.Add(rd);
        //    m_lastUpdate = rd.Moment;
            TimeLastDeal = rd.Moment;
        }
     

        public void Update(DEALS.deal dl)
        {
         
            //TO DO trigger recalc bots
            //TO DO update levels and candles
            LastRcvRD = new CRawDeal(dl);
        //    m_dealQqueue.Add(new CRawDeal(dl));
         
           
            m_bqRawDeal.Add(new CRawDeal(dl));          
            int tmp = 1;
        }

        public void Update(AstsCCTrade.ALL_TRADES at)
        {
            LastRcvRD = new CRawDeal(at);

            m_bqRawDeal.Add(new CRawDeal(at));

        }


        DateTime m_lastUpdate = new DateTime(0);



        private void UpdateLastPrice(CRawDeal rd)
        {
            try
            {
                if ((rd.Id_ord_buy > rd.Id_ord_sell) && (LastBuyPrice != rd.Price))
                {
                
                   // m_plaza2Connector.GUIBox.ExecuteWindowsUpdate(new Action(() => LastBuyPrice = rd.Price));
                    m_plaza2Connector.GUIBox.ExecuteWindowsDealsUpdate(new Action(() => LastBuyPrice = rd.Price));
                    //LastBuyPrice = rd.Price;
                  
                }
                else if ((rd.Id_ord_buy < rd.Id_ord_sell) && (LastSellPrice != rd.Price))
                     //m_plaza2Connector.GUIBox.ExecuteWindowsUpdate(new Action(() => LastSellPrice = rd.Price));
                      m_plaza2Connector.GUIBox.ExecuteWindowsDealsUpdate(new Action(() => LastSellPrice = rd.Price));
                  //  LastSellPrice = rd.Price;
            }
            catch (Exception e)
            {
                Error("UpdateLastPrice",e);
            }

        }
        private void UpdateOpedenedInterest(CRawDeal rd)
        {
            try
            {
                //tempo 
                // for decrease dispatcher.invoke execution
                //TODO remove in the future
                const decimal par_deltaOpenedInterest = 100;
                decimal delta  = Math.Abs( GUIOpenedInterest - rd.Pos)  ;
                if (delta > par_deltaOpenedInterest)
                 //   m_plaza2Connector.GUIBox.ExecuteWindowsUpdate(new Action(() => GUIOpenedInterest = rd.Pos));
                    m_plaza2Connector.GUIBox.ExecuteWindowsDealsUpdate(new Action(() => GUIOpenedInterest = rd.Pos));
                   // GUIOpenedInterest = rd.Pos;
            }
            catch (Exception e)
            {
                Error("UpdateOpedenedInterest", e);
            }

        }











        private CRawDeal m_lastRD { get; set; }
        public CRawDeal LastRcvRD { get; set; }
       
        private void ThreadFunc(object objisin_id)
        {
            const int MAX_COUNT = 50000;


      
            while (true)
            {
                try
                {
                    CRawDeal rd = m_bqRawDeal.GetElementBlocking();
                    //      if (m_dealQqueue.Count > MAX_COUNT)
                    {

                        //        m_plaza2Connector.Error("m_dealQqueue more than max."+m_isin+" Count=" + m_dealQqueue.Count);

                    }

                    if (m_plaza2Connector.AnalzyeTimeFrames)                        
                        m_TimeFrameAnalyzer.AddNewDeal(rd);


                    if (m_plaza2Connector.IsDealsOnline)
                    {

                        TriggerBotsDealsUpdate(rd);
                        UpdateTradersDeals(rd);
                      

                    }





                    if ((DateTime.Now - m_lastUpdate).TotalMinutes > 2)
                    {
                        try
                        {
                            (new System.Threading.Tasks.Task(() => TaskWriteLastReplIDToFile(rd.ReplID, rd.Isin_id))).Start();
                            m_lastUpdate = DateTime.Now;
                        }
                        catch (Exception e)
                        {
                            Error("CDealsStruct.ThreadFunc." + e.Message);
                        }
                    }

                    m_lastRD = (CRawDeal)rd.Copy();

                    //TO DO from config
                    int m_parMin = 5;

                    //only for "fresh data"
                    if ((m_plaza2Connector.ServerTime - rd.Moment).TotalMinutes < m_parMin)
                    {
                        if (m_plaza2Connector.IsDealsOnline)
                        {
                            UpdateAllLastDealData();
                        }
                    }
                }
                catch (Exception e)
                {
                    Error("CDealsStruct.ThreadFunc",e);

                }

            }

           
        }

        public void TriggerBotsDealsUpdate(CRawDeal rd)
        {

            foreach (CBotBase bb in m_plaza2Connector.ListBots)
            {
                if (ListSubscribersBotNames.Contains(bb.Name))
                {
                    string instrument = m_plaza2Connector.Instruments.GetInstrumentByIsinId(rd.Isin_id);
                    BotEventDeal bed = new BotEventDeal(rd.Isin_id, instrument , rd.Amount, rd.Price);

                    //2017-11-13 no need this data for bots but it's increase bot queue

                    //bb.Recalc(m_isin, EnmBotEventCode.OnDeals, (object)bed);
                }
            }

        }

        public void UpdateAllLastDealData()
        {
            if (m_lastRD != null)
            {
                UpdateTimeLastDeal(m_lastRD.Moment);
                UpdateLastPrice(m_lastRD);
                UpdateOpedenedInterest(m_lastRD);
            }
            else
            {
                //m_plaza2Connector.Log("Tempo debug m_lastRD==null");

            }

        }

        public void UpdateTradersDeals(CRawDeal rd)
        {
            if (m_plaza2Connector.GlobalConfig.IsTradingServer && m_plaza2Connector.IsDealsOnline && m_plaza2Connector.IsStockOnline)
            {
                lock (ListDeals)
                {
                    ListDeals.Add((CRawDeal)rd.Copy());
                }
               // m_plaza2Connector.UpdateTradersDeals(m_isin);
            }

        }
        private void TaskWriteLastReplIDToFile(long revID, long isin_id)
        {
            try
            {
                string isin = m_plaza2Connector.Instruments.GetInstrumentByIsinId(isin_id);

                 string rootDir = m_plaza2Connector.GetDataPath();   //System.Windows.Forms.Application.StartupPath;
                string path = String.Format(@"{0}\\deals_{1}_last_replid.txt", rootDir, m_isin);



                System.IO.File.WriteAllText(path, revID.ToString());
            }
            catch (Exception e)
            {

                string errr = e.Message;
            }
         
        }


     
       

    }
}
