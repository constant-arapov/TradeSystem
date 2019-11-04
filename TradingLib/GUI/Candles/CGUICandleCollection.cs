using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows.Threading;

using System.Diagnostics;



using Common;
using Common.Interfaces;
using Common.Logger;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Data;

namespace TradingLib.GUI.Candles
{
    public class CGUICandleCollection : ObservableCollection <GUICandleObject>, IAlarmable
    {
        GUICandleObject _previousCandle = null;
        public string Isin { get; set; }

        CGUIBox m_GUIBox;
        public delegate void CandleCollecionUpdateDelegate();
        public System.Threading.Mutex mx = new System.Threading.Mutex();


        CLogger m_loggerTMP;

       // public delegate void DelegUpdateGUI(Action act);
       // public event DelegUpdateGUI EvntUpdateGUI;


        public event CandleCollecionUpdateDelegate CandleCollectionUpdate;
        /*CPlaza2Connector*/
        IClientGUICandleBox m_plaza2Connector;
        Dispatcher m_subScribedDispatcher;
        
        public void SetSubsribedDispatcher(Dispatcher disp)
        {
            mx.WaitOne();
            m_subScribedDispatcher = disp;
            mx.ReleaseMutex();

        }
        public void UnSetSubsribedDispatcher(Dispatcher disp)
        {
           
            m_subScribedDispatcher = null;
           
        }

        public CGUICandleCollection(/*CPlaza2Connector*/IClientGUICandleBox plaza2Connector, string isin)
        {
            m_GUIBox = plaza2Connector.GUIBox;
            m_plaza2Connector = plaza2Connector;
            Isin = isin;

            m_loggerTMP = new CLogger("Condlecollect_"+isin);


          /*  Add(new GUICandleObject { Date = new DateTime(2008, 8, 4), Open = 25, High = 26, Low = 24, Close = 25, Volume = 3002342 });
            Add(new GUICandleObject { Date = new DateTime(2008, 8, 5), Open = 30, High = 31, Low = 28, Close = 29, Volume = 2000342 });


            Add(new GUICandleObject { Date = new DateTime(2008, 8, 6), Open = 26, High = 28, Low = 25, Close = 27, Volume = 2000342 });
            Add(new GUICandleObject { Date = new DateTime(2008, 8, 7), Open = 26, High = 27, Low = 23, Close = 25, Volume = 2003342 });
            Add(new GUICandleObject { Date = new DateTime(2008, 8, 8), Open = 26, High = 26, Low = 24, Close = 26, Volume = 1000323 });
            Add(new GUICandleObject { Date = new DateTime(2008, 8, 9), Open = 25, High = 25, Low = 24, Close = 24, Volume = 2040389 });

            Add(new GUICandleObject { Date = new DateTime(2008, 8, 10), Open = 23, High = 23, Low = 20, Close = 20, Volume = 2030341 });
           * */
           // CandleCollectionUpdate += new CandleCollecionUpdateDelegate(Dummy);


        }
        public  void Error(string description, Exception exception = null)
        {
            m_plaza2Connector.Error(description,exception);
        }
          private void GUIUpdate(Action act)
          {
              //if (m_GUIBox.GUIDispatcher != null)
                //  m_GUIBox.GUIDispatcher.BeginInvoke(act);

              /*if (m_subScribedDispatcher!=null)
                    m_subScribedDispatcher.BeginInvoke(act);
              else
                  act.Invoke();
              */





          }

          private void DebugPrint(int ind)
          {
              m_loggerTMP.Log("============== Update  this[ind]==== ind="+ind+"============================================================");
              m_loggerTMP.Log("Date=" + this[ind].Date);
              m_loggerTMP.Log("Open="+this[ind].Open);
              m_loggerTMP.Log("Close=" + this[ind].Close);
              m_loggerTMP.Log("High=" + this[ind].High);
              m_loggerTMP.Log("Low=" + this[ind].Low);
              m_loggerTMP.Log("WickHeight = High - Low = " + this[ind].WickHeight);
              m_loggerTMP.Log("BodyHeight = Math.Abs(Open - Close) = " + this[ind].BodyHeight);
              m_loggerTMP.Log("WickCenter = (High + Low) / 2 = " + this[ind].WickCenter);
              m_loggerTMP.Log("scaleY = BodyHeight / WickHeight = " + this[ind].ScaleY);
              m_loggerTMP.Log("origin.Y =  High - top + (BodyHeight / 2)) / WickHeight = " + this[ind].Origin.Y);
             


            


              



          }


          public new void Add(GUICandleObject currentCandle, bool bNeedGUIUpd)
        {
            try
            {

                if (_previousCandle == null)
                {
                    _previousCandle = currentCandle;
                    //KAA we have no previous candle on that day, but we need to fill
                    //so use one of base color
                    if (currentCandle.Open < currentCandle.Close)
                        currentCandle.CandleColor = Colors.Green;
                    else
                        currentCandle.CandleColor = Colors.Red;
                }
                else
                {
                    currentCandle.CandleColor = currentCandle.Close < _previousCandle.Close ? Colors.Red : Colors.Green;
                    _previousCandle = currentCandle;

                }
                base.Add(currentCandle);
                currentCandle.UpdateCandle(bNeedGUIUpd);
            }
            catch (Exception e)
            {
                Error("CGUICandleCollection.Add", e);


            }



        }


          const int NOT_FOUND = -1;

          public int ContainsDate(DateTime dt)
          {
              try
              {
                  int i = 0;

                  foreach (var v in this)
                  {
                      if (v.Date == dt)
                          return i;
                      i++;
                  }
                  return NOT_FOUND;
                 /* for (int i = 0; i < this.Count; i++)
                      if (this[i].Date == dt)
                          return i;
                  return NOT_FOUND;*/
              }
              catch (Exception e)
              {
                  Error("ContainsDate", e);
                  return NOT_FOUND;

              }

          }

          Stopwatch sw1 = new Stopwatch();
          Stopwatch sw11 = new Stopwatch();
          Stopwatch sw2 = new Stopwatch();
          Stopwatch sw3 = new Stopwatch();
          Stopwatch sw4 = new Stopwatch();

          Stopwatch sw5 = new Stopwatch();
          Stopwatch sw6 = new Stopwatch();


          public void UpdateCandle(CTimeFrameInfo tfinfo, bool bNeedGUIUpd)
          {
              int ind = NOT_FOUND;
              try
              {
                  sw1.Start();
                  sw11.Start();

                  sw2.Start();
                  sw3.Start();
                  sw4.Start();
                  sw5.Start();
                  sw6.Start();


                  mx.WaitOne();
                  sw11.Stop();
                  
                  
                 // TrapError();
                 
                   ind = this.ContainsDate(tfinfo.Dt);
                   sw6.Stop();
                  if (ind == NOT_FOUND)
                  {
                     

                      //if (this.Count > 0 && tfinfo.Dt < this.Last().Date)
                        //  System.Threading.Thread.Sleep(0);


                      this.Add(new GUICandleObject
                      {
                          Date = tfinfo.Dt,
                          Open = (double)tfinfo.OpenPrice,
                          Close = (double)tfinfo.ClosePrice,
                          High = (double)tfinfo.HighPrice,
                          Low = (double)tfinfo.LowPrice,
                          Volume = (int)tfinfo.Volume
                      }, bNeedGUIUpd);

                      sw5.Stop();
                      this.Last().UpdateCandle(bNeedGUIUpd);
                      sw4.Stop();
                  }
                  else
                  {
                    

                      GUICandleObject curr = this[ind];
                      /*
                      this[ind] = new GUICandleObject {
                      Date = tfinfo.Dt,
                      Open = (double)tfinfo.OpenPrice,
                      Close = (double)tfinfo.ClosePrice,
                      High = (double)tfinfo.HighPrice,
                      Low = (double)tfinfo.LowPrice,
                      Volume = (int)tfinfo.Volume,
                      };
                      */

                      this[ind].Date = tfinfo.Dt;
                      this[ind].Open = (double)tfinfo.OpenPrice;
                      this[ind].Close = (double)tfinfo.ClosePrice;
                      this[ind].High = (double)tfinfo.HighPrice;
                      this[ind].Low = (double)tfinfo.LowPrice;
                      this[ind].Volume = (int)tfinfo.Volume;
                      sw5.Stop();
                      this[ind].UpdateCandle(bNeedGUIUpd);
                      DebugPrint(ind);
                      sw4.Stop();
                    
                  }

                  sw3.Stop();           

                //  TrapError();

              
                 //tempo uncomment
                //  NotifyCollectionChanged();
                  if (CandleCollectionUpdate != null)
                      CandleCollectionUpdate();


                
              }
              catch (Exception e)
              {

                  Error("CGUICandleCollecion.UpdateCandle", e);

              }
              finally
              {
                  sw2.Stop();
                  mx.ReleaseMutex();

              }

              sw1.Stop();
              int tmp = 1;

          }

          private void TrapError()
          {

              for (int i =0; i <  this.Count-1; i++)
              {
                  if  (this[i].Date> this[i+1].Date)
                  {
                      Error("TrapError CGUICandleCollecion.");
                      return;
                  }

              }

          }

          Stopwatch sw = new Stopwatch();

          public void Update(CTimeFrameInfo tfinfo)
          {              

            //  mx.WaitOne();
              /*if (EvntUpdateGUI == null)
                  UpdateCandle(tfinfo);

              else
                  EvntUpdateGUI(new Action(()=> UpdateCandle(tfinfo)));
              */
              sw.Start();
              if (m_subScribedDispatcher != null)
                  m_subScribedDispatcher.BeginInvoke(new Action(() => UpdateCandle(tfinfo, true)));
              else
                  UpdateCandle(tfinfo, false);

              sw.Stop();
              int tmp = 0;
             // mx.ReleaseMutex();

          }
      
          public void NotifyCollectionChanged()
          {
                         
              if (CandleCollectionUpdate != null)
                  CandleCollectionUpdate();
              
          }


          public double GetMaximum(int first=0)
          {
              int i = 0;
              double max= 0;
              foreach (var v in this)
              {
                  if (i++ >= first)
                  {
                      if (v.High > max) max = v.High;

                  }

              }
              return max;              
          }

          public double GetMinimum(int first=0)
          {
              double min = double.MaxValue;
              int i = 0;
              foreach (var v in this)
              {
                  if (i++ >= first)
                  {
                      if (v.Low < min) min = v.Low;
                  }
              }

              return min;
          }

          private int GetInd(DateTime dt)
          {
              int i = 0;
            for (i = 0; i < this.Count && this[i].Date < dt; i++) ;
             
              return i;
          }

          public double GetMinimumFromDate(DateTime dt)
          {
              int i = GetInd(dt);
              return GetMinimum(i);
              
          }

          public double GetMaximumFromDate(DateTime dt)
          {

                int i = GetInd(dt);
              return  GetMaximum(i);

          }






    }
}
