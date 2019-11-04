using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;
using System.IO;

using System.Text.RegularExpressions;
using System.Diagnostics;


using System.Collections.Concurrent;

using Common;
using Common.Interfaces;
using Common.Collections;
using Common.Logger;
using Common.Utils;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Enums;
using TradingLib.Data;

using TradingLib.BotEvents;


namespace TradingLib

{
    public class CTimeFrameAnalyzer : IAlarmable
    {
        private string m_isin;

        //private BlockingCollection<CRawDeal> m_blkColRowDeals = new BlockingCollection<CRawDeal>(1000000);


        private CBlockingQueue<CRawDeal> m_bqRawDeals = new CBlockingQueue<CRawDeal>();



        private CRawDeal m_prevDeal = null;
        private CRawDeal m_currDeal = null;

        List<CRawDeal> m_listRawDealStorage = new List<CRawDeal>();
        Mutex mxlistRawDealStorage = new Mutex();

        Mutex mxWrLastRD = new System.Threading.Mutex();
   


        CDictTFArray m_dictTFArray ;
        List<int> m_M1_scale = new List<int>();
        List<int> m_M5_scale = new List<int>();
        List<int> m_M15_scale = new List<int>();
        List<int> m_M30_scale = new List<int>();
        List<int> m_H_scale = new List<int>();
        List<int> m_D_scale = new List<int>();




      
        CTimeFrameArray ArrM1 = new CTimeFrameArray();


        public /*CPlaza2Connector*/IClientTimeFrameAnalyzer Plaza2Connector { get; set; }

        string m_rootDir;
        string m_dataTFDir;
      

        CTimer m_timerWriteFiles = null;
      
        
        public decimal LowDayPrice { get; set; }
        public decimal HighDayPrice { get; set; }



        private bool _wasStartedCreateTimeArray = false;

        public bool IsOnlineData
        {
            get;
            set;
        }

        private CLogger m_logger;




        
        public string IsinDir
        {
            get
            {

                return String.Format(@"{0}\{1}", m_dataTFDir, m_isin);

            }

        }


        List<string> m_listFiles = new List<string>();


        public void DelayIfLargMemUsage()
        {
        /*    const int MB = 1000000;

            long memUsage = Plaza2Connector.MemoryUsssage;
         */
/*
            if (memUsage > 2200 * MB)
                Thread.Sleep(10);//20
            else if (memUsage > 2000 * MB)
                Thread.Sleep(5);//10
            else if (memUsage > 1800 * MB)
                Thread.Sleep(2);//5
            
            */



        }
        public void AddNewDeal (CRawDeal rd)
        {
                     
            DelayIfLargMemUsage();
            //m_blkColRowDeals.Add(rd);
            m_bqRawDeals.Add(rd);

        }
        
        private void ThreadAnalyzeOnline()
        {

            const int MAX_COUNT = 10000;

           // foreach (CRawDeal rd in m_blkColRowDeals.GetConsumingEnumerable())
            while (true)
            {

                CRawDeal rd = m_bqRawDeals.GetElementBlocking();

              /*  if (m_blkColRowDeals.Count > MAX_COUNT)
                    Plaza2Connector.Error("ThreadAnalyzeOnline > MAX_COUNT. "+m_isin+
                        " count="+m_blkColRowDeals.Count);
                */

                AnalyzeOnlineData(rd);


            }
           
        }
        
        public void   TaskUpdateCandles (string stTF, /*SortedDictionary<DateTime, CTimeFrameArray> TFarr*/ List <DateTime> LstDict)
        {
            try
            {
               
                int i = 0;
                const int COUNT_DEEP = 2;
                foreach (DateTime dt in System.Linq.Enumerable.Reverse(LstDict))
                {


                    Plaza2Connector.GUIBox.GUICandleBox.AddTFArray(m_isin, stTF, dt.ToString(), m_dictTFArray[stTF][dt]);

                    int numCount = Math.Min(COUNT_DEEP, LstDict.Count);


                    if (++i == numCount)                        
                        Plaza2Connector.GUIBox.GUICandleBox.SetLastDataLoaded(m_isin, stTF);

                }

            }
            catch (Exception e)
            {
                Error("TaskUpdateCandles", e);

            }


        }
        private void AnalizeDiskData()
        {

            try
            {       //Load data from disk first
                if (Directory.Exists(IsinDir))
                {
                    string[] Files = Directory.GetFiles(IsinDir);
                    foreach (string fn in Files)
                    {
                        FileInfo f = new FileInfo(fn);
                        if (f.Length == 0)
                        {
                            File.Delete(fn);
                            Error("Remove empty file "+fn);
                            continue;
                        }

                        Regex newReg = new Regex(@"[\w\W]*([0-9]{4}_[0-9]{2}_[0-9]{2})_(M1|M5|M15|M30|H1|D1).xml");
                        Match m = newReg.Match(fn);

                        if (m.Groups.Count > 1)
                        {
                          
                            string stTF = m.Groups[2].ToString();
                            string stDate = m.Groups[1].ToString();
                            DateTime dt = CUtilTime.GetDateFromString(stDate);
                            CTimeFrameArray tfa = new CTimeFrameArray();
                            tfa.FileName = fn;
                            //CSerializator.Read<CTimeFrameArray>(ref tfa);
                            CSerializator.ReadSafe<CTimeFrameArray>(ref tfa);
                            
                            m_dictTFArray.AddTFArray(stTF, dt, tfa);
                          

                            string st="";
                            if (tfa.ListTimeFrameInfo.Count == 0)
                                st = "";


                          //  Plaza2Connector.GUIBox.ExecuteCandlesUpdate
                            //    (new Action(() => Plaza2Connector.GUIBox.GUICandleBox.AddTFArray(tfa.ListTimeFrameInfo[0].Isin, stTF, dt.ToString(), tfa)));

                          
                        }
                      
                    }

                    AnalyzeDiskTF(CUtilTF.IsClosed_M5_M15_M30_TF, m_M5_scale, EnmTF.M5, EnmTF.M1);
                    AnalyzeDiskTF(CUtilTF.IsClosed_M5_M15_M30_TF, m_M15_scale, EnmTF.M15, EnmTF.M5);
                    AnalyzeDiskTF(CUtilTF.IsClosed_M5_M15_M30_TF, m_M30_scale, EnmTF.M30, EnmTF.M15);
                    AnalyzeDiskTF(CUtilTF.IsClosed_H1, m_M30_scale, EnmTF.H1, EnmTF.M30);

                    AnalyzeDiskTFDays();


                    //CreateCandlesArrays();
                  
                }
            }
            
            catch (Exception e)
            {

                Error("AnalizeDiskData",e);

            }
        }

        public void Error(string description, Exception exception = null)
        {

             Plaza2Connector.Error( description, exception );
        }


        public void CreateCandlesArrays()
        {


            try
            {

                //create accepted flags first
                foreach (KeyValuePair<string, SortedDictionary<DateTime, CTimeFrameArray>> kvp in m_dictTFArray)
                    if (kvp.Value.Count > 0)
                        Plaza2Connector.GUIBox.GUICandleBox.CreateAcceptedFlag(m_isin, kvp.Key);





                foreach (KeyValuePair<string, SortedDictionary<DateTime, CTimeFrameArray>> kvp in m_dictTFArray)
                //(new Task(() => TaskUpdateCandles(kvp.Key,kvp.Value))).Start();
                {
                    string stTf = kvp.Key;
                    //SortedDictionary<DateTime, CTimeFrameArray> sd = kvp.Value;
                    List<DateTime> dtList = new List<DateTime>();
                    foreach (var d in kvp.Value)
                        dtList.Add(d.Key);

                    //if (st == "D1")                        
                    {
                        // TaskUpdateCandles(kvp.Key, kvp.Value);
                        (new Task(() => TaskUpdateCandles(stTf, dtList))).Start();
                    }
                }
                //  TaskUpdateCandles(kvp.Key, kvp.Value);


            }
            catch (Exception e)
            {


                Error("CreateCandlesArrays", e);


            }
                    

                    


       

        }

        private void AnalyzeDiskTFDays()
        {
            try
            {


                if (!m_dictTFArray.ContainsKey(EnmTF.H1.ToString()))
                    return;



                foreach (KeyValuePair<DateTime, CTimeFrameArray> kvp in m_dictTFArray[EnmTF.H1.ToString()]) //all dates of low TF array
                {

                    try
                    {
                        CTimeFrameArray M_low_TF_Array = kvp.Value;

                        DateTime dt = CUtilTime.NormalizeDay(M_low_TF_Array.ListTimeFrameInfo[0].Dt.Date);
                        if (m_dictTFArray.IsContainTimeFrameInfo(EnmTF.D1, dt))
                            continue;


                        CTimeFrameInfo ntfi = m_dictTFArray.GetNewTimeFrameInfo(m_isin, EnmTF.D1, dt);

                        Analyze_D1(dt);
                    }
                    catch (Exception e)
                    {


                    }

                }
            }
            catch (Exception e)
            {

                Error("AnalyzeDiskTFDays",e);
            }



        }
        private void Analyze_D1(DateTime dt)
        {
            try
            {


                CTimeFrameInfo ntfi = m_dictTFArray.GetNewTimeFrameInfo(m_isin, EnmTF.D1, dt);


                CTimeFrameArray M_low_TF_Array = m_dictTFArray[EnmTF.H1.ToString()][dt];


                for (int i = 1; i < M_low_TF_Array.ListTimeFrameInfo.Count; i++) //each TF array
                {
                    ntfi.OpenedPos = M_low_TF_Array.ListTimeFrameInfo[i].OpenedPos;

                    CTimeFrameInfo currtfi = M_low_TF_Array.ListTimeFrameInfo[i];
                    ntfi.numOfDeals += currtfi.numOfDeals;
                    ntfi.Volume += currtfi.Volume;

                    if (ntfi.HighPrice < currtfi.HighPrice) ntfi.HighPrice = currtfi.HighPrice;
                    if (ntfi.LowPrice > currtfi.LowPrice) ntfi.LowPrice = currtfi.LowPrice;

                }
            }
            catch (Exception e)
            {
                string st = e.Message;

            }

        }


        private void CheckTFIConsistent(CTimeFrameInfo ntfi)
        {
            //  Plaza2Connector.GUIBox.GUICandleBox.UpdateCandle(this.m_isin, TF_high.ToString(), dtcnd.ToString(), ntfi);
            if (ntfi.HighPrice < ntfi.LowPrice)
                Error("Not consistent data.ntfi.HighPrice < ntfi.LowPrice");


        }

        private void InsertToValidPlace(CTimeFrameInfo tfi,int ind, CTimeFrameArray arr)
        {
            int i = 0;
            for (i = 0; i < arr.ListTimeFrameInfo.Count - 1; i++)
            {

                if (arr.ListTimeFrameInfo[i].Dt > tfi.Dt)
                    break;


            }

            arr.ListTimeFrameInfo.Insert(i, tfi);
            arr.ListTimeFrameInfo.RemoveAt(ind);
        }

      


        //Function to trap error.
        //Must be disabled in production system
        private void TrapError(CTimeFrameArray arr)
        {
            for (int i = 0; i < arr.ListTimeFrameInfo.Count - 1; i++)
            {
                if (arr.ListTimeFrameInfo[i].Dt > arr.ListTimeFrameInfo[i+1].Dt)
                {

        //         InsertToValidPlace(arr.ListTimeFrameInfo[i+1],i+1, arr);
                    Error("TrapError TimeFrameArray.");
                    return;
                }

            }

        }



        private void AnalyzeDiskTF(ChangedTF changedTF, List<int> lstScale, EnmTF TF_high, EnmTF TF_low)
        {
            try
            {

                foreach (KeyValuePair<DateTime, CTimeFrameArray> kvp in m_dictTFArray[TF_low.ToString()]) //all dates of low TF array
                {
                    
                    if (CUtilTime.OlderThanTwoWorkDays(kvp.Key))
                        continue;

                    CTimeFrameArray M_low_TF_Array = kvp.Value;
                    for (int i = 1; i < M_low_TF_Array.ListTimeFrameInfo.Count; i++) //each TF array
                    {

                        DateTime dtPrev = M_low_TF_Array.ListTimeFrameInfo[i - 1].Dt;
                        DateTime dtCurr = M_low_TF_Array.ListTimeFrameInfo[i].Dt;



                        DateTime dtFrom = new DateTime(0);
                        DateTime dtTo = new DateTime(0);

                        if (changedTF(dtPrev, dtCurr, lstScale, ref dtFrom, ref dtTo))
                        {
                            //TO DO chek prev day etc

                            if (m_dictTFArray.IsContainTimeFrameInfo(TF_high, dtFrom))
                                continue; //if  already exists in high TF array than nothing to do and  continue

                            //exp logics
                            CTimeFrameInfo ntfi=null;
                            if (m_dictTFArray.GetLatestTFI(TF_high.ToString()) != null)
                            {
                                DateTime dtLast = (m_dictTFArray.GetLatestTFI(TF_high.ToString())).Dt;
                                if (dtLast > dtFrom)
                                {
                                    ntfi = m_dictTFArray.GetNewTFIPuttingtoWritePlace(m_isin, TF_high, dtFrom);
                                    //Error("Inserting data. TO DO check and remove");
                                }
                                else
                                    ntfi = m_dictTFArray.GetNewTimeFrameInfo(m_isin, TF_high, dtFrom);

                            }
                                else 
                                     ntfi = m_dictTFArray.GetNewTimeFrameInfo(m_isin, TF_high, dtFrom);
                           


                            int j = i - 1;

                            //note: the last pos
                            ntfi.OpenedPos = M_low_TF_Array.ListTimeFrameInfo[j].OpenedPos;
                            ntfi.ClosePrice = M_low_TF_Array.ListTimeFrameInfo[j].ClosePrice;


                            while (j >= 0 && M_low_TF_Array.ListTimeFrameInfo[j].Dt >= dtFrom)
                            {
                                CTimeFrameInfo currtfi = M_low_TF_Array.ListTimeFrameInfo[j];
                                ntfi.numOfDeals += currtfi.numOfDeals;
                                ntfi.Volume += currtfi.Volume;

                                if (ntfi.HighPrice < currtfi.HighPrice) ntfi.HighPrice = currtfi.HighPrice;
                                if (ntfi.LowPrice > currtfi.LowPrice) ntfi.LowPrice = currtfi.LowPrice;

                                ntfi.OpenPrice = M_low_TF_Array.ListTimeFrameInfo[j].OpenPrice;

                                j--; //backward
                            }

                            DateTime dtcnd = CUtilTime.NormalizeDay(dtFrom);

                            //no need ? check !
                            CheckTFIConsistent(ntfi);

                        }
                        
                        


                    }
                    //TrapError(kvp.Value);
                }

            }
            catch (Exception e)
            {
                Error("AnalyzeDiskTF",e);


            }


        }


     
    public void GenerateTFScales()
    {

        for (int i = 0; i <= 12; m_M5_scale.Add (i * 5), i++) ;
        for (int i = 0; i <= 4; m_M15_scale.Add(i * 15), i++) ;
        for (int i = 0; i <= 2; m_M30_scale.Add(i * 30), i++) ;
        for (int i = 0; i <= 60; m_M1_scale.Add(i),      i++) ;

    }
    // public  decimal UpdateCurrentDayLow()
    // {
    //   DateTime dt = CUtil.NormalizeDay(DateTime.Now);

    // return m_dictTFArray[EnmTF.D1.ToString()][dt].ListTimeFrameInfo[0].LowPrice;

    //}

    public bool IsChangedM5TF()
    {


        return false;
    }


    public CTimeFrameAnalyzer (string isin, /*CPlaza2Connector*/IClientTimeFrameAnalyzer plaza2Connector)
        {           
            m_isin = isin;
            Plaza2Connector = plaza2Connector;
            m_rootDir = plaza2Connector.GetDataPath();
            m_dataTFDir = m_rootDir +"\\TF";

            m_dictTFArray = new CDictTFArray(this);


            if (Plaza2Connector.GlobalConfig.AnalzyeTimeFrames)
            {
                GenerateTFScales();
                AnalizeDiskData();

                (new Thread(ThreadAnalyzeOnline)).Start();
            }

            m_logger = new CLogger("TimeFrameAnalyzer_"+m_isin, 
                                    flushMode:false,
                                    subDir:"TimeFrameAnalyzer");
        }


    public void Log(string msg)
    {
        m_logger.Log(msg);

    }

/* 
    public  string GetDirName_Isin_TF( EnmTF TF, DateTime dt)
    {
        return IsinDir + @"\" + TF.ToString();
    }

    public string GetDirName_Isin_TF_Date(EnmTF TF, DateTime dt)
    {
        return GetDirName_Isin_TF(TF, dt) + @"\" + CUtil.GeDateString(dt);    
    }

        */

    /*public string GetFileName(EnmTF TF, DateTime dt)
    {
       
        return String.Format(@"{0}\{1}_{2}.xml", IsinDir, CUtilTime.GeDateString(dt), TF.ToString());

    }
	*/
    public string GetFileName(string TF, DateTime dt)
    {

        return String.Format(@"{0}\{1}_{2}.xml", IsinDir, CUtilTime.GeDateString(dt), TF);

    }


    public void CreateDirectoriesIfNeed()
    {

        string dir = IsinDir;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string  bkpDir = CUtil.GenBackupFileName(dir);

        if (!Directory.Exists(bkpDir))
            Directory.CreateDirectory(bkpDir);

    }
     private void TaskWriteLastReplIDToFile(long revID)
     {

         string path = String.Format(@"{0}deals_{1}_last_replid.txt", Plaza2Connector.GetDataPath(), m_isin);

         mxWrLastRD.WaitOne();
         mxWrLastRD.ReleaseMutex();



      }



     delegate bool ChangedTF(DateTime dtPrev, DateTime dtCurr, List<int> lstScale,  ref DateTime dtFrom, ref DateTime dtTo);

     private void AnalyzeOnlineTF(ChangedTF changedTF,   List <int> lstScale, EnmTF TF_high, EnmTF TF_low, CRawDeal prevDeal, CRawDeal currDeal)
        {

      
            DateTime dtPrev = prevDeal.Moment;
            DateTime dtCurr = currDeal.Moment;


            //Get  last high TF
            CTimeFrameInfo TFinfo_low = m_dictTFArray.GetLastTimeFrameInfo(TF_low);
          /*  if (!TFinfo_low.bNotProcessedData)
                return;
         */


          DateTime dtFrom = new DateTime(0);
          DateTime dtTo = new DateTime(0);

      

          bool b_wasChangedTF = changedTF(prevDeal.Moment, currDeal.Moment, lstScale, ref dtFrom, ref dtTo);

        
     

          if (!m_dictTFArray.IsContainTimeFrameInfo(TF_high, /*dtFrom*/dtTo))
              m_dictTFArray.AddNewTimeFrameInfo(m_isin, TF_high, /*dtFrom*/dtTo, this);

          CTimeFrameInfo TFinfo_low_prev = m_dictTFArray.GetPrevLastTimeFrameInfo(TF_low);
          CTimeFrameInfo TFinfo_high_prev = m_dictTFArray.GetPrevLastTimeFrameInfo(TF_high);
     
                
          CTimeFrameInfo TFinfo_high = m_dictTFArray.GetLastTimeFrameInfo(TF_high);


          bool bChangedExtr = false;


          if (currDeal.ReplID > TFinfo_high.LastReplId)
          {
              
              TFinfo_high.numOfDeals++;
              TFinfo_high.Volume += currDeal.Amount;
              TFinfo_high.OpenedPos = currDeal.Pos;

              //changed 2016/08/01
              //only if changed extremum do recalc
              if (TFinfo_high.HighPrice < TFinfo_low.HighPrice)
              {
                  TFinfo_high.HighPrice = currDeal.Price;
                  bChangedExtr = true;
              }
              if (TFinfo_high.LowPrice > TFinfo_low.LowPrice)
              {
                  TFinfo_high.LowPrice = currDeal.Price;
                  bChangedExtr = true;
              }

              TFinfo_high.LastReplId = currDeal.ReplID;

              TFinfo_high.LastUpdate = DateTime.Now;

              if (!TFinfo_high.bProcessedData)
              {
                  TFinfo_high.OpenPrice = TFinfo_low.OpenPrice;
                  TFinfo_high.bProcessedData = true;
              }
              //KAA 2015-12-23
              //because low TF was already updated
             // if (!b_wasChangedTF)
                TFinfo_high.ClosePrice = TFinfo_low.ClosePrice;
             // else
               // TFinfo_high.ClosePrice = TFinfo_low_prev.ClosePrice; //TFinfo_low.ClosePrice;


              DateTime dtcnd = CUtilTime.NormalizeDay(dtFrom);
            //  Plaza2Connector.GUIBox.GUICandleBox.UpdateCandle(this.m_isin, TF_high.ToString(), dtcnd.ToString(), TFinfo_high);

              Plaza2Connector.GUIBox.GUICandleBox.QueueTFinfo(this.m_isin, TF_high.ToString(), dtcnd.ToString(), TFinfo_high);


              //changed 2016/08/01
              //only if changed extremum do recalc
              if (bChangedExtr)
                TriggerRecalcAllBotsUpdateTF(TF_high, TFinfo_high);

              if (b_wasChangedTF)
              {
                  Log(TF_high.ToString() + " TFinfo_high.Dt="+TFinfo_high.Dt.ToString()+" was closed prevDeal.Moment=" + prevDeal.Moment + " currDeal.Moment=" + currDeal.Moment + " TFinfo_high.ClosePrice=" + TFinfo_high.ClosePrice +
                      " TFinfo_low_prev.ClosePrice=" + TFinfo_low_prev.ClosePrice + " TFinfo_low_prev.Dt =" + TFinfo_low_prev.Dt);

                  if (Plaza2Connector.IsDealsOnline &&    Plaza2Connector.IsAnalyzerTFOnline)
                    TriggerRecalcAllBotsChangedTF(TF_high, TFinfo_high_prev);
              }
          }
          else
          {
             

          }
         

      //  TrapError(m_dictTFArray[TF_high.ToString()][CUtil.NormalizeDay(dtFrom)]);

        
          int tmp = 1;
        }



        private void AnalyzeOnlineM1(CRawDeal rd, CRawDeal prevDeal)
        {
          

            DateTime dtTill = rd.Moment.AddSeconds(-rd.Moment.Second);  
            dtTill = dtTill.AddMilliseconds(-dtTill.Millisecond);

          
            if (!m_dictTFArray.IsContainTimeFrameInfo(EnmTF.M1,dtTill))
                m_dictTFArray.AddNewTimeFrameInfo(m_isin, EnmTF.M1, dtTill, this);


          
            CTimeFrameInfo tfi = m_dictTFArray.GetLastTimeFrameInfo(EnmTF.M1);
           
            
        
                DateTime currMoment = rd.Moment; 
                CRawDeal currRd = rd;

                //changed 2016/08/01
                //only if changed extremum do recalc
                bool bChangedExtr = false;
              
                if (rd.ReplID > tfi.LastReplId)
                {
                    tfi.LastReplId = rd.ReplID;
                    tfi.OpenedPos = rd.Pos;
                    tfi.Volume += rd.Amount; 
                    tfi.numOfDeals++;
                    if (currRd.Price > tfi.HighPrice)
                    {
                        bChangedExtr = true; ;
                        tfi.HighPrice = currRd.Price;

                    }
                    if (currRd.Price < tfi.LowPrice)
                    {
                        bChangedExtr = true;
                        tfi.LowPrice = currRd.Price;
                    }
                 
                    tfi.LastUpdate = rd.Moment;

                    if (!tfi.bProcessedData)
                    {
                        tfi.OpenPrice = rd.Price;
                        tfi.bProcessedData = true;
                    }
                    tfi.ClosePrice = rd.Price;
              
                    DateTime dtcnd = CUtilTime.NormalizeDay(dtTill);
                    CheckTFIConsistent(tfi);
                   
                //    sw5.Stop();

                    //Plaza2Connector.GUIBox.GUICandleBox.UpdateCandle(this.m_isin, EnmTF.M1.ToString(), dtcnd.ToString(), tfi);
                    Plaza2Connector.GUIBox.GUICandleBox.QueueTFinfo(this.m_isin, EnmTF.M1.ToString(), dtcnd.ToString(), tfi);
                }
           
               // TrapError(m_dictTFArray["M1"][CUtil.NormalizeDay(tfi.Dt)]);


               

                CheckDataOnline(rd);

             

                if (Plaza2Connector.IsDealsOnline)
                {

                    //changed 2016/08/01
                    //only if changed extremum do recalc
                    if (bChangedExtr)
                        TriggerRecalcAllBotsUpdateTF(EnmTF.M1, tfi);

                    DateTime dtTmp = Plaza2Connector.ServerTime;
                    DateTime dtFrom = new DateTime(0);
                    DateTime dtTo = new DateTime(0);

                    CTimeFrameInfo tfiPrev = m_dictTFArray.GetPrevLastTimeFrameInfo(EnmTF.M1);


                    if (CUtilTF.WasClose_M1_TF(prevDeal.Moment, rd.Moment))
                    {
                        Log("M1 was closed prevDeal.Moment=" +prevDeal.Moment +" rd.Moment=" + rd.Moment);
                        if (Plaza2Connector.IsAnalyzerTFOnline)
                        TriggerRecalcAllBotsChangedTF(EnmTF.M1, tfiPrev);

                    }

                }

           
             

                AnalyzeOnlineTF(CUtilTF./*(IsClosed_M5_M15_M30_TF*/ WasClosed_M5_M15_M30_TF, m_M5_scale, EnmTF.M5, EnmTF.M1, prevDeal, rd);
              

                AnalyzeOnlineTF(CUtilTF./*IsClosed_M5_M15_M30_TF*/ WasClosed_M5_M15_M30_TF, m_M15_scale, EnmTF.M15, EnmTF.M5, prevDeal, rd);

          

                AnalyzeOnlineTF(CUtilTF./*IsClosed_M5_M15_M30_TF*/ WasClosed_M5_M15_M30_TF, m_M30_scale, EnmTF.M30, EnmTF.M15, prevDeal, rd);

            
              
            AnalyzeOnlineTF(CUtilTF./*IsClosed_H1*/ WasClosed_H1,          m_M30_scale, EnmTF.H1,   EnmTF.M30, prevDeal, rd);
          

            AnalyzeOnlineTF(CUtilTF./*IsClosed_D1*/WasClosed_D1, m_M30_scale, EnmTF.D1, EnmTF.H1, prevDeal, rd);
           

            CTimeFrameInfo TFinfo_D1 = m_dictTFArray.GetLastTimeFrameInfo(EnmTF.D1);
            LowDayPrice = TFinfo_D1.LowPrice;
            HighDayPrice = TFinfo_D1.HighPrice;

           
            
           
        }
        private void TriggerRecalcAllBotsUpdateTF(EnmTF TF, CTimeFrameInfo tfi)
        {
            if (!Plaza2Connector.IsDealsOnline || !IsOnlineData)
                return;

           BotEventTF mess =    new BotEventTF()
                             {
                               TFUpdate = TF,
                               TFI = tfi
                             };
           Plaza2Connector.TriggerRecalcAllBotsWithInstrument(m_isin, EnmBotEventCode.OnTFUpdate, mess);
        }

        private void TriggerRecalcAllBotsChangedTF(EnmTF TF, CTimeFrameInfo tfi)
        {
            if (!Plaza2Connector.IsDealsOnline || !IsOnlineData)
                return;

            BotEventTF mess = new BotEventTF()
            {
                TFUpdate = TF,
                TFI = tfi
                
            };
            Plaza2Connector.TriggerRecalcAllBotsWithInstrument(m_isin, EnmBotEventCode.OnTFChanged, mess);
        }


        public void CheckDataOnline(CRawDeal currRD)
        {
            if (!this.IsOnlineData  && Plaza2Connector.IsDealsOnline)
                if (currRD.ReplID == Plaza2Connector.DealBox.DealsStruct[m_isin].LastRcvRD.ReplID)
                    this.IsOnlineData = true;



        }



        /*
        public void WriteTFToFile(CTimeFrameArray tfa, EnmTF TF, DateTime dt)
        {

            CreateDirectoriesIfNeed();
            ArrM1.FileName = GetFileName(EnmTF.M1, dt); //TO DO split
            CSerializator.Write<CTimeFrameArray>(ref ArrM1);

        }
        */

        public void CheckTFAnalyzerOnline(CRawDeal rd)
        {

            if (!Plaza2Connector.IsAnalyzerTFOnline)
            {

                if ((Plaza2Connector.ServerTime - rd.Moment).TotalSeconds < 1)
                {
                    Plaza2Connector.IsAnalyzerTFOnline = true;
                   

                }
            }

            //first - the moment when we build candle array
            //and set IsTimeToInitCandle
            if (!Plaza2Connector.IsTimeToInitCandles)
            {
                if ((Plaza2Connector.ServerTime - rd.Moment).TotalHours < 1)
                    Plaza2Connector.IsTimeToInitCandles = true;

            }

            //now we trigger once load tf array
            if (Plaza2Connector.IsTimeToInitCandles &&!this._wasStartedCreateTimeArray)
            {
                _wasStartedCreateTimeArray = true;
                CreateCandlesArrays();

            }
        }



        public void AnalyzeOnlineData(CRawDeal rd)
        {

            try
            {
              

                if (m_timerWriteFiles == null)
                    m_timerWriteFiles = new CTimer(5000, new Action(m_dictTFArray.WriteAllDataToDisk), true);

                 CheckTFAnalyzerOnline(rd);

                if (m_prevDeal == null && m_currDeal == null)
                {
                    m_currDeal = rd;

                    CTimeFrameInfo tfi = m_dictTFArray.GetLatestTFI();
                    if (tfi != null)
                    {
                        m_prevDeal = new CRawDeal(tfi);

                    }
                    else
                    {
                        m_prevDeal = rd;
                        return;
                    }
                }

                m_prevDeal = m_currDeal;
                m_currDeal = rd;
             
                //if data old do not analyze it
                if (!m_dictTFArray.IsNewData(rd))
                    return;


                AnalyzeOnlineM1((CRawDeal)rd.Copy(), m_prevDeal);
               

                /*
                if (m_dictTFArray.IsTimeToWrite(EnmTF.M1))
                {
                                    
                    (new System.Threading.Tasks.Task(() => m_dictTFArray.WriteAllDataToDisk())).Start();                    
                }

            */

            }

            catch (Exception e)
            {
                Error("Error AnalyzeData",e);
                          
            }



        }

        public  CTimeFrameInfo GetTFIByDate(string stTF, DateTime dt)
        {

            //DateTime dt =   CUtil.NormalizeDay(Plaza2Connector.ServerTime);

            return m_dictTFArray[stTF][CUtilTime.NormalizeDay(dt)].ListTimeFrameInfo.GetTFIByDate(dt);
           
        }


    }





}
