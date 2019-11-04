using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Collections.Concurrent;

using System.Diagnostics;

using Common;
using Common.Interfaces;
using Common.Collections;


using TradingLib.Data;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Enums;

namespace TradingLib.GUI.Candles
{

    public class CGUICandleBox : Dictionary<string, Dictionary <string, Dictionary<string, CGUICandleCollection>>>, IAlarmable
    {
        

      //  public delegate void CandleBoxUpdatedDelegate();

     //   public event CandleBoxUpdatedDelegate CandleBoxUpdated ;

        private  Dictionary  <string,Dictionary<string,bool>> LastDataAccepted { get; set; }
        private object _lockLastDataAccepted = new object();

        private Dictionary <string, Dictionary <string, Mutex>> m_dictTFMx = new Dictionary <string, Dictionary <string, Mutex>>();


        private Dictionary<string, Mutex> m_dictIsinMx = new Dictionary<string, Mutex>();
       


        private CGUIBox m_GUIBox;
        /*CPlaza2Connector*/
        IClientGUICandleBox m_plaza2Connector;


        Mutex mx = new Mutex();

        Dictionary<string, BlockingCollection<CStructTF>> m_dictStructTf = new Dictionary<string, BlockingCollection<CStructTF>>(1000000);

        Dictionary<string, CBlockingQueueForStructures<CStructTF>> m_dictbqStructTF = new Dictionary<string, CBlockingQueueForStructures<CStructTF>>();

        private Dictionary<string, object> m_dictStructTFlock = new Dictionary<string, object>();


        private List<CIsinBlockingQueue> _listBQ = new List<CIsinBlockingQueue>();


        private Dictionary<string, CGUICandleProcessor> _dictCandleProc = new Dictionary<string, CGUICandleProcessor>();



        public void SetLastDataLoaded(string isin,string tf)
        {
            lock (_lockLastDataAccepted)
            {
                LastDataAccepted[isin][tf] = true;

            }
        }

     
        public bool IsAllLastDataLoaded
        {
            get
            {
                lock (_lockLastDataAccepted)
                {
                    foreach (KeyValuePair<string, Dictionary<string, bool>> kvp in LastDataAccepted)
                        foreach (var b in kvp.Value)
                            if (!b.Value)
                                return false;

                    return true;
                }
            }
          
        }

        public CGUICandleBox(CGUIBox GUIBox)
        {
            m_GUIBox = GUIBox;
            m_plaza2Connector = (IClientGUICandleBox) m_GUIBox.Plaza2Connector;

            LastDataAccepted = new Dictionary<string,Dictionary<string,bool>>();;
   
            CreateDataStruct();
        }


        Dictionary<string, long> tmp = new Dictionary<string, long>();

        public void CreateDataStruct()
        {
            
           //foreach (var isin in m_plaza2Connector.GlobalConfig.ListIsins)
            foreach( var kvp in m_plaza2Connector.Instruments.DictInstrument_IsinId)
            {
                string isin = kvp.Key;

                this[isin] = new Dictionary<string, Dictionary<string, CGUICandleCollection>>();
           
                m_dictTFMx[isin] = new Dictionary<string, Mutex>();
                m_dictIsinMx[isin] = new Mutex();

                foreach (var enm in Enum.GetValues(typeof(EnmTF)))
                {
                    this[isin][enm.ToString()] = new Dictionary<string, CGUICandleCollection>();
                   
                    m_dictTFMx[isin][enm.ToString()] = new Mutex();
                }

                m_dictStructTf[isin] = new BlockingCollection<CStructTF>();
                m_dictbqStructTF[isin] = new CBlockingQueueForStructures<CStructTF>();

             

                _listBQ.Add(new CIsinBlockingQueue {Isin = isin });

              


                _dictCandleProc[isin] = new CGUICandleProcessor(isin, this);



            }

        }


        private  CBlockingQueueForStructures<string>  GetBQ(string isin)
        {
            for (int i = 0; i < _listBQ.Count; i++)
                if (_listBQ[i].Isin == isin)
                    return _listBQ[i].Queue;

                return null;
        }

        Dictionary<string, long> tmp_old = new Dictionary<string, long>();
        private void ThreadRowDealProcessor(string isin)
        {
            const int MAX_COUNT = 1000;

            tmp_old[isin] = 0;

           /* foreach (CStructTF sc in m_dictStructTf[isin].GetConsumingEnumerable())
                UpdateCandle(sc.Isin, sc.TF, sc.Dt, sc.TFinfo);
            */
            /*
            while (true)
            {
             //   CStructTF sc = (CStructTF) varSc.Copy();


                if (m_dictbqStructTF.ContainsKey(isin) && m_dictbqStructTF[isin] != null)
                {
              
                    
                        CStructTF sc = m_dictbqStructTF[isin].GetElementBlocking();
                        if (sc != null)
                        {
                            long val = Convert.ToInt64(sc.Isin);
                            if (val != 0)
                            {
                                if (val - tmp_old[isin] != 1)
                                    Thread.Sleep(0);
                                
                            }
                            tmp_old[isin] = val;
                           // UpdateCandle(sc.Isin, sc.TF, sc.Dt, sc.TFinfo);


                        }
                        else
                        {
                            Thread.Sleep(0);
                        }
                            ;// Error("ThreadRowDealProcessor. Anomaly  sc is null");
                    
                }
            //    if (m_dictStructTf[isin].Count > MAX_COUNT)
              //      Error("ThreadRowDealProcessor > MAX " + isin + " count=" + m_dictStructTf[isin].Count);
            }
        */

            CBlockingQueueForStructures<string> q = GetBQ(isin);
            while (true)
            {

                string sc = q.GetElementBlocking();
                if (sc != null)
                {
                    long val = Convert.ToInt64(/*sc.Isin*/sc);
                    if (val != 0)
                    {
                        if (val - tmp_old[isin] != 1)
                            Thread.Sleep(0);

                    }
                    tmp_old[isin] = val;
                    // UpdateCandle(sc.Isin, sc.TF, sc.Dt, sc.TFinfo);


                }
                else
                {
                    Thread.Sleep(0);
                }

            }


        }
            
     

        
        public void QueueTFinfo(string isin, string tf, string dt, CTimeFrameInfo tfi)
        {
            //after this time all data will (or already where) loaded
            //so it is possible to update


            if (m_plaza2Connector.IsTimeToInitCandles)                 
                _dictCandleProc[isin].Add(isin, tf, dt, tfi);

       
        }




        public void Error(string err, Exception e = null)
        {
            if (m_plaza2Connector != null)
                m_plaza2Connector.Error(err, e);

        }

        public void CreateAcceptedFlag(string isin, string tf)
        {
            try
            {
                lock (_lockLastDataAccepted)
                {
                    if (!LastDataAccepted.ContainsKey(isin))
                        LastDataAccepted[isin] = new Dictionary<string, bool>();

                    if (!LastDataAccepted[isin].ContainsKey(tf))
                        LastDataAccepted[isin][tf] = false;
                }
            }
            catch (Exception e)
            {
                Error("CGUICandleBox.CreateAcceptedFlag", e);

            }
        }

        public bool IsTFIsinLastDataAccepted(string isin)
        {
            lock (_lockLastDataAccepted)
            {
                if (!LastDataAccepted.ContainsKey(isin))
                    return false;
                try
                {
                    foreach (KeyValuePair<string, bool> kvp in LastDataAccepted[isin])
                        if (kvp.Value == false)
                            return false;
                }
                catch (Exception e)
                {

                    Error("IsTFIsinLastDataAccepted", e);

                }
            }

        return true;
        }


       private void CreateIfNeed(string isin)
        {

            try
            {
           /*
            if (!this.ContainsKey(isin))
            {
             
                this[isin] = new Dictionary<string, Dictionary<string, CGUICandleCollection>>();

            }
           */

           

          


      
            }
             

            catch (Exception e)
            {

                Error("CreateIfNeed", e);
            }

        }

        private void CreateIfNeed(string isin, string tf)
        {
           
                /*
                if (!this[isin].ContainsKey(tf))
                    this[isin][tf] = new Dictionary<string, CGUICandleCollection>();
                */
                    /*
                if (!LastDataAccepted[isin].ContainsKey(tf))
                    LastDataAccepted[isin][tf] = false;
                    */
                
            


        }
       


        private void CreateIfNeed(string isin, string tf,string dt)
        {
          m_dictTFMx[isin][tf].WaitOne();
            if (!this[isin][tf].ContainsKey(dt))
                this[isin][tf][dt] = new CGUICandleCollection(m_plaza2Connector,isin);

            m_dictTFMx[isin][tf].ReleaseMutex();

        }


        private void CreateIfNeedAll(string isin, string tf, string dt)
        {

            CreateIfNeed(isin);
            CreateIfNeed(isin, tf);
            CreateIfNeed(isin, tf, dt);
     
        }



        Stopwatch sw = new Stopwatch();
        Stopwatch sw1 = new Stopwatch();


        public void UpdateCandle(string isin, string tf, string dt, CTimeFrameInfo tfinfo)
        {
                          
            CreateIfNeedAll(isin, tf, dt);
           
            //TO DO normal
            //TO DO remove delay from deals thread
            sw.Start();
            m_dictTFMx[isin][tf].WaitOne();
            sw1.Start();
                this[isin][tf][dt].Update(tfinfo);
            sw1.Stop();
                m_dictTFMx[isin][tf].ReleaseMutex();
           sw.Stop();

           int tmp = 1; 
            /*
                if (m_GUIBox != null && CandleBoxUpdated!=null)
                m_GUIBox.ExecuteCandlesUpdate(new Action (()=>  CandleBoxUpdated()));
            else
                CandleBoxUpdated();
            */
                
               
        }

        public void AddTFArray(string isin, string tf, string dt, CTimeFrameArray tfa)
        {

            try
            {
              
                CreateIfNeedAll(isin, tf, dt);
             
                
               
             
                CListTimeFrameInfo lstf = (CListTimeFrameInfo) tfa.ListTimeFrameInfo.Clone();
                int i = 0;
                foreach (CTimeFrameInfo tfi in lstf)
                {
                                                     
                        m_dictTFMx[isin][tf].WaitOne();
                        this[isin][tf][dt].mx.WaitOne();
                        
                        this[isin][tf][dt].Update(tfi);

                        this[isin][tf][dt].mx.ReleaseMutex();
                        m_dictTFMx[isin][tf].ReleaseMutex();
                  
                        i++;
                }



             /*   if (CandleBoxUpdated != null)
                {
                    if (m_GUIBox != null)
                        m_GUIBox.ExecuteCandlesUpdate(new Action(() => CandleBoxUpdated()));
                    else
                        CandleBoxUpdated();
                }
               */

            }
            catch (Exception e)
            {

                //  System.Diagnostics.Debug.Assert(false, e.Message + " " +e.StackTrace);
                Error("AddTFArray", e);
            }
            finally
            {
             

            }

        }

        public bool IsTFAvailable(string isin, string tf,string dt)
        {
            if (!this.ContainsKey(isin))
                return false;

            if (!this[isin].ContainsKey(tf))
                return false;

            if (!this[isin][tf].ContainsKey(dt))
                return false;


            return true;
        }


    }
    public class CStructTF  : CClone
    {
        public CStructTF(string isin, string tf, string dt, CTimeFrameInfo tfinfo)
        {
            Isin = isin;
            TF = tf;
            Dt = dt;
            TFinfo = tfinfo;

        }
        public string Isin { get; set; }
        public string TF { get; set; }
        public string Dt { get; set; }
        public CTimeFrameInfo TFinfo {get; set;}

    }

    public class CIsinBlockingQueue
    {
        public string Isin { get; set; }
        public CBlockingQueueForStructures<string> Queue = new  CBlockingQueueForStructures<string>();
    }





}
