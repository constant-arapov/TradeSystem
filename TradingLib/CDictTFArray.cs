using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;


using Common;
using Common.Interfaces;
using Common.Utils;

using TradingLib.Enums;
using TradingLib.Data;
using TradingLib.GUI;

namespace TradingLib
{

    class CDictTFArray : Dictionary<string, SortedDictionary<DateTime, CTimeFrameArray>>, ICloneable, IAlarmable
    {
        CTimeFrameAnalyzer m_timeFrameAnalyzer;
        CGUIBox m_GUIBox;
        public Mutex mx = new Mutex();

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public CDictTFArray(CTimeFrameAnalyzer timeFrameAnalyzer)
        {

            m_timeFrameAnalyzer = timeFrameAnalyzer;
            m_GUIBox = m_timeFrameAnalyzer.Plaza2Connector.GUIBox;

        }

        public void Error(string st, Exception e = null)
        {


            m_timeFrameAnalyzer.Plaza2Connector.Error (st,e);

        }



        public CTimeFrameInfo GetLatestTFI()
        {
            if (this.ContainsKey("M1"))
            {
                if (this["M1"].Count > 0)
                {
                    DateTime dt = this["M1"].Keys.Last();
                    if (this["M1"][dt].ListTimeFrameInfo.Count > 0)
                    {

                        return this["M1"][dt].ListTimeFrameInfo.Last();

                    }

                }
            }
            return null;
        }

        public CTimeFrameInfo GetLatestTFI(string stTF)
        {
            if (this.ContainsKey(stTF))
            {
                if (this[stTF].Count > 0)
                {
                    DateTime dt = this[stTF].Keys.Last();
                    if (this[stTF][dt].ListTimeFrameInfo.Count > 0)
                    {

                        return this[stTF][dt].ListTimeFrameInfo.Last();

                    }

                }
            }
            return null;

        }

        private void IfNotLastInsert(CTimeFrameInfo tfi)
        {



        }








        public void AddTFArray(string TF, DateTime dt, CTimeFrameArray ar)
        {
            mx.WaitOne();
            if (!this.ContainsKey(TF))
                this[TF] = new SortedDictionary<DateTime, CTimeFrameArray>();


  
            this[TF][dt] = ar;

            mx.ReleaseMutex();
           // string isin = ar.ListTimeFrameInfo[0].Isin;
            
            
          
          //  m_GUIBox.ExecuteWindowsUpdate(new Action (()=> m_GUIBox.AddCandleCollection (TF, dt, ar )  ));

            



        }


        public void AddNewTimeFrameInfo(string isin, EnmTF TF,  DateTime date, CTimeFrameAnalyzer tfa)
        {
            mx.WaitOne();
            if (!this.ContainsKey(TF.ToString()))            
                this[TF.ToString()] = new SortedDictionary<DateTime, CTimeFrameArray>();

            if (!this[TF.ToString()].ContainsKey(date.Date))
            {
                this[TF.ToString()][date.Date] = new CTimeFrameArray();
            
            }

            this[TF.ToString()][date.Date].ListTimeFrameInfo.Add(new CTimeFrameInfo(isin, date/*, tfa*/));
            mx.ReleaseMutex();
        }

        //TO DO check. dangerous
        public CTimeFrameInfo GetLastTimeFrameInfo(EnmTF TF)
        {
            CTimeFrameInfo tfi = null;
            try
            {
                DateTime dtMax = new DateTime(0);
                foreach (KeyValuePair<DateTime, CTimeFrameArray> kvp in this[TF.ToString()])
                    if (kvp.Key > dtMax && kvp.Value.ListTimeFrameInfo.Count>0)
                        dtMax = kvp.Key;

                 tfi = this[TF.ToString()][dtMax].ListTimeFrameInfo.Last();
                 return tfi;
            }
            catch (Exception e)
            {
                Error("GetLastTimeFrameInfo",e);
                return tfi;
            }
            
         
        }




        //TO DO check. dangerous
        public CTimeFrameInfo GetPrevLastTimeFrameInfo(EnmTF TF)
        {
            CTimeFrameInfo tfi = null;
            try
            {
                DateTime dtMax = new DateTime(0);
                foreach (KeyValuePair<DateTime, CTimeFrameArray> kvp in this[TF.ToString()])
                    if (kvp.Key > dtMax && kvp.Value.ListTimeFrameInfo.Count > 0)
                        dtMax = kvp.Key;

                int cnt=this[TF.ToString()][dtMax].ListTimeFrameInfo.Count;
                if (cnt >= 2)
                    tfi = this[TF.ToString()][dtMax].ListTimeFrameInfo[cnt-2];
                else
                    tfi = this[TF.ToString()][dtMax].ListTimeFrameInfo.Last();
                return tfi;
            }
            catch (Exception e)
            {
                Error("GetLastTimeFrameInfo", e);
                return tfi;
            }


        }















        public CTimeFrameInfo GetNewTFIPuttingtoWritePlace(string isin, EnmTF TF, DateTime dt)
        {
            CListTimeFrameInfo arr = null;
            int i = 0;

            try
            {
                arr = this[TF.ToString()][dt.Date].ListTimeFrameInfo;
              
                for (i = 0; i < arr.Count; i++)
                {
                    if (arr[i].Dt > dt)
                        break;

                }
            }
            catch (Exception e)
            {
                Error("GetNewTFIPuttingtoWritePlace",e);

            }


            mx.WaitOne();
            if (arr!=null)            
                arr.Insert(i, new CTimeFrameInfo(isin, dt/*, m_timeFrameAnalyzer*/));            
            mx.ReleaseMutex();
            return arr[i];

            //return null;
        }
        public CTimeFrameInfo GetNewTimeFrameInfo(string isin,EnmTF TF, DateTime dt)
        {
              if (!this.ContainsKey(TF.ToString())) this[TF.ToString()] = new SortedDictionary<DateTime, CTimeFrameArray>();
             if (!this[TF.ToString()].ContainsKey(dt.Date)) this[TF.ToString()][dt.Date] = new CTimeFrameArray();




             mx.WaitOne();
           this[TF.ToString()][dt.Date].ListTimeFrameInfo.Add(new CTimeFrameInfo(isin,dt/*,m_timeFrameAnalyzer*/));
           mx.ReleaseMutex();

            

        
           return this[TF.ToString()][dt.Date].ListTimeFrameInfo.Last();
        }



        public bool IsContainTimeFrameInfo(EnmTF TF, DateTime dtFind)
        {

            if (!this.ContainsKey(TF.ToString()))
                return false;

            if (!this[TF.ToString()].ContainsKey(dtFind.Date))
                return false;
            foreach (CTimeFrameInfo ctfi in this[TF.ToString()][dtFind.Date].ListTimeFrameInfo)
            {
                if (ctfi.Dt == dtFind)
                    return true;


            }

            return false;
        }
        public bool IsNewData(CRawDeal rd)
        {

            DateTime rd_date = CUtilTime.NormalizeDay(rd.Moment.Date);

            if (!this.ContainsKey("M1"))
                return true;

            if (!this["M1"].ContainsKey(rd_date))
                return true;

            if (this["M1"][rd_date].ListTimeFrameInfo.Count == 0)
                return true;


            DateTime dt = this["M1"][rd_date].ListTimeFrameInfo.Last().Dt;//.AddMinutes(1);
            if (rd.Moment > dt)
                return true;
           

            return false;
        }

        public void TaskCopyBackup(string fname)
        {
            
            System.Threading.Thread.Sleep(1000);

            const int NUM_CNT = 10;
            int i = 0;
            string bkfn =CUtil.GenBackupFileName(fname);
            if (!CUtil.IsValidXML(fname))
                return;




            for ( i = 0; i < NUM_CNT; i++ )
            {
                try
                {
                    string bkFn = CUtil.GenBackupFileName(fname);
                    File.Copy(fname, bkfn, true);
                    return;
                }
                catch (System.UnauthorizedAccessException ea)
                {
                    //nothing to do with for a while
                    //maybe in the future will do more complex
                    string st = ea.Message;

                    System.Threading.Thread.Sleep(20);

                }
                catch (Exception e)
                {
                    string err = e.Message;
                    

                }

                System.Threading.Thread.Sleep(200);

            }
            if (i == NUM_CNT)
            {
                //string st = "problem";
                System.Threading.Thread.Sleep(0);
            }


        }



        private void CopyDictTFArray(ref Dictionary<string, SortedDictionary<DateTime, CTimeFrameArray>> DictTFDest)
        {
            try
            {

                mx.WaitOne();

                foreach (KeyValuePair<string, SortedDictionary<DateTime, CTimeFrameArray>> dict in this)
                {


                    string dtTF = dict.Key;
                    SortedDictionary<DateTime, CTimeFrameArray> SourceSDA = dict.Value;

                    if (!DictTFDest.ContainsKey(dtTF))
                        DictTFDest[dtTF] = new SortedDictionary<DateTime, CTimeFrameArray>();

                    foreach (KeyValuePair<DateTime, CTimeFrameArray> dictTFA in SourceSDA)
                    {
                        DateTime dt = dictTFA.Key;
                        CTimeFrameArray sourceTFarr = dictTFA.Value;
                        if (!DictTFDest[dtTF].ContainsKey(dt))
                            DictTFDest[dtTF][dt] = new CTimeFrameArray()
                             {
                                 FileName = sourceTFarr.FileName,
                                 NeedSelfInit = sourceTFarr.NeedSelfInit,
                                 // ListTimeFrameInfo = new CListTimeFrameInfo()
                             };

                        if (DictTFDest[dtTF][dt].ListTimeFrameInfo == null)
                            DictTFDest[dtTF][dt].ListTimeFrameInfo = new CListTimeFrameInfo();
                        //foreach (CTimeFrameInfo destTFI in sourceTFarr.ListTimeFrameInfo)

                        for (int i = 0; i < sourceTFarr.ListTimeFrameInfo.Count; i++)
                        {
                            //   DictTFDest[dtTF][dt].ListTimeFrameInfo.Add((CTimeFrameInfo)  destTFI.Copy());
                            //if (!DictTFDest[dtTF][dt].ListTimeFrameInfo.Contains(destTFI))
                            //  DictTFDest[dtTF][dt].ListTimeFrameInfo.Add((CTimeFrameInfo)destTFI.Copy());
                            if (i > DictTFDest[dtTF][dt].ListTimeFrameInfo.Count - 1)
                                DictTFDest[dtTF][dt].ListTimeFrameInfo.Add((CTimeFrameInfo)sourceTFarr.ListTimeFrameInfo[i].Copy());

                            if (DictTFDest[dtTF][dt].ListTimeFrameInfo[i].LastUpdate < sourceTFarr.ListTimeFrameInfo[i].LastUpdate)
                                DictTFDest[dtTF][dt].ListTimeFrameInfo[i] = (CTimeFrameInfo)sourceTFarr.ListTimeFrameInfo[i].Copy();

                        }

                    }

                }
            }
            catch (Exception e)
            {
                Error("CopyDictTFArray",e);
            }
            finally 
            {
                mx.ReleaseMutex();


             }




        }

        Dictionary<string, SortedDictionary<DateTime, CTimeFrameArray>> CopyDict =
                new Dictionary<string, SortedDictionary<DateTime, CTimeFrameArray>>();
        bool bCopyIsBusy = false;

        public void WriteAllDataToDisk()
        {
          //TO DO older 2 days

          //  if (bCopyIsBusy)
           //     return;

            
           
            bCopyIsBusy = true;
           
           
            CopyDictTFArray(ref CopyDict);
          
            //m_timeFrameAnalyzer.Plaza2Connector.Log("ElapsedMilliseconds=" + sw.ElapsedMilliseconds + " " + m_timeFrameAnalyzer.IsinDir); 
        //    CDictTFArray cp = (CDictTFArray)  this.MemberwiseClone();



            try
            {

                foreach (KeyValuePair<string, SortedDictionary<DateTime, CTimeFrameArray>> kv in CopyDict)// this)
                {
                    string tf = kv.Key;

                    foreach (KeyValuePair<DateTime, CTimeFrameArray> kv2 in kv.Value)
                    {
                        DateTime dt = kv2.Key;
                        CTimeFrameArray tfa = kv2.Value;
                        if (!CUtilTime.OlderThanTwoWorkDays(dt))
                        {
                            tfa.FileName = m_timeFrameAnalyzer.GetFileName(tf, dt);

                            //   tfa.mxLockFile.WaitOne();
                            this.m_timeFrameAnalyzer.CreateDirectoriesIfNeed();
                            CSerializator.Write<CTimeFrameArray>(ref tfa);

                            tfa.DtLastWrite = DateTime.Now;

                        //    TaskCopyBackup(tfa.FileName);



                            //   tfa.mxLockFile.ReleaseMutex();
                        }
                    }

                }

            }
            catch (Exception e)
            {
                Error("WriteAllDataToDisk",e);

            }

            bCopyIsBusy = false;

        }
        

        public bool IsTimeToWrite(EnmTF TF)
        {

            //TO DO from config
            if (CopyDict.ContainsKey(TF.ToString()) && !bCopyIsBusy)
            {
                DateTime dt = /*this*/CopyDict[TF.ToString()].Last().Value.DtLastWrite;
                if ((DateTime.Now - dt).TotalSeconds > 20)
                    return true;
            }
            return false;

        }
    }
}
