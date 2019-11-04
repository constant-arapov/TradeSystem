using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;

using Common;
using Common.Interfaces;
using Common.Collections;


using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Data;
using TradingLib.GUI.Candles;


namespace TradingLib.GUI
{
   

    public class CGUIBox : CBaseProppertyChanged, IAlarmable, IGUIDispatcherable
    {
     

        public Dispatcher GUIDispatcher {set; get;}

        public CObservableIdCollection<VMInst,string> VM { set; get; }
        public CObservableIdCollection<PosisionInst,string> Position { set; get; }
        public CObservableIdCollection<OrderInst,long> Orders { set; get; }


     

        public CGUICandleBox GUICandleBox { get; set; }



        System.Collections.Concurrent.BlockingCollection<Action> m_blkColUpdateWindowsActions = new System.Collections.Concurrent.BlockingCollection<Action>();
        CBlockingQueue<Action> m_bqActions = new CBlockingQueue<Action>();
        CBlockingQueue<Action> m_bqDealsActions = new CBlockingQueue<Action>();

        CBlockingQueue<Action> m_bqStockActions = new CBlockingQueue<Action>();



       




      //  System.Collections.Concurrent.BlockingCollection<Action> m_blkColUpdateCanldles = new System.Collections.Concurrent.BlockingCollection<Action>();



        public CGUICandleCollection M1_collection { get; set; }




        private DateTime _serverTime;

        public DateTime ServerTime 
        { 
         get
          {
              return _serverTime;
          }
             
         set 
         {
             _serverTime = value;
             RaisePropertyChanged("ServerTime");
         } 
        }


        private decimal _bid;
        public decimal Bid
        {

            get
            {
                return _bid;

            }
            set
            {
                _bid = value;
                RaisePropertyChanged("Bid");

            }
        }


        private decimal _ask;
        public decimal Ask
        {
            get
            {
                return _ask;

            }
            set
            {
                _ask = value;       
                RaisePropertyChanged("Ask");

            }
        }





        private bool _readyForCreatePublishers = false;
        public bool IsReadyForCreatePublishers
        {
            get
            {
                return _readyForCreatePublishers;
            }
            set
            {
                _readyForCreatePublishers = value;

                RaisePropertyChanged("IsReadyForCreatePublishers");

            }

        }




        private bool _readyForSendOrder = false;
        public bool IsReadyForSendOrder
        {
            get
            {
                return _readyForSendOrder;
            }
            set
            {
                _readyForSendOrder = value;
                RaisePropertyChanged("IsReadyForSendOrder");

            }

        }




        public bool _onlineUserOrderLog = false;
        public bool IsOnlineUserOrderLog
        {
            get
            {
                return _onlineUserOrderLog;
            }
            set
            {
                _onlineUserOrderLog = value;

                RaisePropertyChanged("IsOnlineUserOrderLog");

            }

        }


        private bool _orderControlAvailable = false;
        public bool IsOrderControlAvailable
        {
            get
            {
                return _orderControlAvailable;
            }
            set
            {
                _orderControlAvailable = value;
                RaisePropertyChanged("IsOrderControlAvailable");

            }

        }



        private bool _onlineUserDeals = false;
        public bool IsOnlineUserDeals
        {
            get
            {
                return _onlineUserDeals;
            }
            set
            {
                _onlineUserDeals = value;

                RaisePropertyChanged("IsOnlineUserDeals");

            }

        }

        private bool _sessionOnline = false;
        public bool IsSessionOnline
        {
            get
            {
                return _sessionOnline;
            }
            set
            {
                _sessionOnline = value;

                RaisePropertyChanged("IsSessionOnline");

            }

        }
      


        private bool _positionOnline = false;
        public bool IsPositionOnline
        {
            get
            {
                return _positionOnline;
            }
            set
            {
                _positionOnline = value;

                RaisePropertyChanged("IsPositionOnline");

            }

        }



        private bool _stockOnline = false;

        public bool IsStockOnline
        {
            get
            {
                return _stockOnline;
            }
            set
            {
                _stockOnline = value;

                RaisePropertyChanged("IsStockOnline");

            }

        }



        private bool _dealsOnline;
        public bool IsDealsOnline
        {
            get
            {
                return _dealsOnline;
            }

            set
            {
                _dealsOnline = value;
                RaisePropertyChanged("IsDealsOnline");
            }
        }

        private bool _sessionActive = false;
        public bool IsSessionActive
        {
            get
            {
                return _sessionActive;
            }
            set
            {
                _sessionActive = value;
                RaisePropertyChanged("IsSessionActive");

            }

        }


        private bool _possibleToCancelOrders = false;
        public bool IsPossibleToCancelOrders
        {
            get
            {
                return _possibleToCancelOrders;
            }
            set
            {
                _possibleToCancelOrders = value;
                RaisePropertyChanged("IsPossibleToCancelOrders");

            }

        }


        private bool _sessionInClearing = false;
        public bool IsSessionInClearing
        {
            get
            {
                return _sessionInClearing;
            }
            set
            {
                _sessionInClearing = value;
                RaisePropertyChanged("IsPossibleToCancelOrders");

            }

        }




        private bool _serverTimeAvailable = false;
        public bool IsServerTimeAvailable
        {
            get
            {
                return _serverTimeAvailable;
            }
            set
            {
                _serverTimeAvailable = value;
                RaisePropertyChanged("IsServerTimeAvailable");

            }

        }




        private bool _onlineVM = false;
        public bool IsOnlineVM
        {
            get
            {
                return _onlineVM;
            }
            set
            {
                _onlineVM = value;
                RaisePropertyChanged("IsOnlineVM");

            }

        }






        private bool _fortsOnline;
        public bool IsFORTSOnline
        {
            get
            {
                return _fortsOnline;
            }

            set
            {
                _fortsOnline = value;
                RaisePropertyChanged("IsFORTSOnline");
            }
        }

        private bool _analyzerTFOnline = false;
        public bool IsAnalyzerTFOnline
        {
            get
            {
                return _analyzerTFOnline;
            }
            set
            {
                _analyzerTFOnline = value;
                RaisePropertyChanged("IsAnalyzerTFOnline");
                                      
            }

        }


        private bool _isDatabaseConnected = false;
        public bool IsDataBaseConnected
        {
            get
            {
                return _isDatabaseConnected;
            }
            set
            {
                _isDatabaseConnected = value;
                RaisePropertyChanged("IsDataBaseConnected");

            }

        }



        private int _sessState;
        public int SessionState
        {
            get
            {
                return _sessState;

            }
            set
            {
                _sessState = value;
                RaisePropertyChanged("SessionState");

            }


        }

        private string _sessionString;
        public string SessionString
        {

            get
            {
                return _sessionString;

            }
            set
            {
                _sessionString = value;
                RaisePropertyChanged("SessionString");



            }



        }


        public bool IsOnlineUserDealsDelayed = false;
       public bool IsOnlineUserOrderLogDelayed = false;


        public void UpdateSessionString(DateTime sessBegin, DateTime sessEnd)
        {

            SessionString = sessBegin.Hour.ToString("D2") + ":" + sessBegin.Minute.ToString("D2") +" - "+ sessEnd.Hour.ToString("D2") + ":"+sessEnd.Minute.ToString("D2");

        }



    

        public ObservableCollection<PART.part> Part { set; get; }

        public /*CPlaza2Connector*/ IClientGUIBox Plaza2Connector;

        public void StartGUICandleBox()
        {
            while (!Plaza2Connector.IsAllInstrAllMarketsAvailable)
                System.Threading.Thread.Sleep(100);


            if (Plaza2Connector.AnalzyeTimeFrames)
                GUICandleBox = new CGUICandleBox(this);




        }


        public CGUIBox(/*CPlaza2Connector*/IClientGUIBox plaza2Connector  )
        {

            Plaza2Connector = plaza2Connector;
            (new Thread(StartGUICandleBox)).Start();

            (new Thread(ThreadWindowsUpdate)).Start();
           // (new Thread(ThreadCandlesUpdate)).Start();

           
            (new Thread(ThreadWindowsDealsUpdate)).Start();


            (new Thread(ThreadWindowsStockUpdate)).Start();
       

            Part = new ObservableCollection<PART.part>();
           

            VM = new CObservableIdCollection<VMInst,string>();
            Position = new CObservableIdCollection<PosisionInst, string>();
            Orders = new CObservableIdCollection<OrderInst, long>();


          



         }



        
       

     

        public void UpdatePart(PART.part prt)
        {


            if (GUIDispatcher != null)
                ExecuteWindowsUpdate(new Action(() => ActionUpdPart(prt)));
               
               
            
           

            
        }

        public void ActionUpdPart(PART.part prt)
        {
            if (Part.Count == 0)
                Part.Add(prt);
            else
            Part[0] = prt;


        }



        public void UpdateOrders(string isin, CRawOrdersLogStruct rols)
        {
           ExecuteWindowsUpdate(new Action(() =>
            Orders.UpdateWithId(new OrderInst(isin, rols))
            ));
          
           
       }

        public void RemoveOrder(long ordId)
        {
            ExecuteWindowsUpdate(new Action(() =>
             Orders.RemoveWithId(ordId)
             ));
          

        }




        public void UpdateVM(string isin,FORTS_VM_REPL.fut_vm vm)
        {


            ExecuteWindowsUpdate(
                new Action(() =>
                    {
                     //for catch noisy error !
                    try 
                    {

                        VM.UpdateWithId(new VMInst(isin, vm));
                    }
                    catch (Exception e)
                    {

                        Error("UpdateVM",e);

                    }


                    }

            ));
        
        }
        public void UpdatePos(string isin, /*POS.position*/CRawPosition pos)
        {

            if (/*pos.pos*/pos.Pos == 0)
                ExecuteWindowsUpdate(new Action(() =>
                Position.RemoveWithId(isin)
                ));
            else
                ExecuteWindowsUpdate(new Action(() =>
                Position.UpdateWithId(new PosisionInst(isin, pos))
                ));

        }


     

        public void UpdateBidAsk(decimal bid, decimal ask)
        {


            try
            {
                if (Bid != bid)
                    //ExecuteWindowsUpdate(new Action (()=> Bid =bid));
                    //ExecuteWindowsStockUpdate(new Action(() => Bid = bid));
                    Bid = bid;


                if (Ask != ask)
                    //ExecuteWindowsUpdate(new Action(() => Ask = ask));
                  //  ExecuteWindowsStockUpdate(new Action(() => Ask = ask));
                    Ask = ask;
            }
            catch (Exception e)
            {
                Error("UpdateBidAsk",e);

            }

         
        }



        public void ExecuteWindowsStockUpdate(Action act)
        {

            m_bqStockActions.Add(act);

        }

        public void ThreadWindowsStockUpdate()
        {

            ProcessBlockingQueue("ThreadWindowsStockUpdate", ref m_bqStockActions, 20, true);


         

        }
    
       public void ExecuteWindowsDealsUpdate(Action act)
        {
        
            m_bqDealsActions.Add(act);
         
        }


        public void ThreadWindowsDealsUpdate()
        {
          
            ProcessBlockingQueue("ThreadWindowsDealsUpdate", ref m_bqDealsActions, 500, false);


        }

        private void ProcessBlockingQueue(string threadName, ref CBlockingQueue<Action> blockingQueue, int countLimit, bool bOutAlarm, int secRepeatAlarm=20)
        {
             DateTime lastAlarm = new DateTime(0);
            while (true)
            {
                Action act = blockingQueue.GetElementBlocking();

               

                if (blockingQueue.Count > countLimit)
                {
                    if ((DateTime.Now - lastAlarm).TotalSeconds > secRepeatAlarm && bOutAlarm)
                    {
                        //2018-04-15 removed to make cleaner alarm list/file
                        //Error(threadName+ "quee more than max skip messages. Count="+blockingQueue.Count);
                        lastAlarm = DateTime.Now;
                    }
                }
                else
                    if (GUIDispatcher!=null)
                    GUIDispatcher.BeginInvoke(act);

            }

        }


        //This thread not removes
        //deals from queue
        public void ThreadWindowsUpdate()
        {
            while (true)
            {

               Action value = m_bqActions.GetElementBlocking();

                while (GUIDispatcher == null)
                    System.Threading.Thread.Sleep(100);

             GUIDispatcher.BeginInvoke(value, DispatcherPriority.Normal, null);
              
            }
                        

        }

        Stopwatch sw = new Stopwatch();
        public void ExecuteWindowsUpdate(Action act)
        {


            try
            {
                sw.Start();
                // m_blkColUpdateWindowsActions.Add(act);

                m_bqActions.Add(act);


                sw.Stop();

              //  if (sw.ElapsedMilliseconds > 0)
                //    System.Threading.Thread.Sleep(0);

            }
            catch (Exception e)
            {
                Error("ExecuteWindowsUpdate", e);
            }

        }

         
        
      /*

        public void ThreadCandlesUpdate()
        {


            while (GUIDispatcher == null)
                System.Threading.Thread.Sleep(100);


            foreach (Action value in m_blkColUpdateCanldles.GetConsumingEnumerable())
            {

                try
                {

                    GUIDispatcher.Invoke(DispatcherPriority.Normal, value);

                }

                catch (Exception e)
                {

                    Error("ThreadCandlesUpdate", e);

                }
                

               


            }

        }
       */ 
       

        public void Error(string description, Exception exception = null)
        {

            Plaza2Connector.Alarmer.Error(description, exception);


        }




      /*  public void ExecuteCandlesUpdate(Action act)
        {
                     
            m_blkColUpdateCanldles.Add(act);
           
        }
        */


        public class Foo
        {

            public Foo(string a, string b)
            {
                A = a;
                B = b;

            }


            public string A { get; set; }
            public string B { get; set; }

        }

        public ObservableCollection<Foo> obsGUI
        {
            get;
            set;
        }




    }

    



}
