

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;

using System.Xml;
using System.Threading;
using System.Threading.Tasks;

using Common;
using Common.Interfaces;
using Common.Logger;
using Common.Utils;

using GUIComponents;


using TradingLib;
using TradingLib.Enums;
using TradingLib.ProtoTradingStructs;


//using Visualizer;

using TCPLib;
using Messenger;

using Terminal;
using Terminal.Conf;
using Terminal.Controls;
using Terminal.Controls.Market;
using Terminal.Controls.Market.ChildElements;


using Terminal.Interfaces;
using Terminal.TradingStructs;
using Terminal.DataBinding;
using Terminal.Communication;

using Terminal.Events;
using Terminal.Events.Data;


using Terminal.Views.ChildWindows;



namespace Terminal.ViewModels
{
    
  public partial class MarketViewModel :  CBasePropertyChangedAuto, /*ViewModelBase ,*/ /*CBaseProppertyChanged,*/  IGUIDispatcherable, IAlarmable/*,  ITradeOperations*/
    {
       private long _parMaxVolumeBar;


        private CStockPosition[] m_Asks;
        private CStockPosition[] m_Bids;
      
              
      


        private ControlMarket _controlMarket;

        public ControlMarket ControlMarket
        {

            get
            {
                return _controlMarket;
            }



        }

        public delegate void DelegForceRepaintControlStock();
        public delegate void DelegForceRepaintDeals();


        public DelegForceRepaintControlStock ForceRepaintControlStock;
        //public DelegForceRepaintDeals ForceRepaintDeals;



        private CKernelTerminal _kernelTerminal;

      

       

        
        private Thread _threadDataReciever;

        /// <summary>
        /// Number of connections in connections list
        /// </summary>
        public int ConnId {get;set;}


        public bool IsAlive {get; set;}
        private bool IsDead { get; set; }


        private int _parSleep;



       private CInstrumentConfig _instrumentConfig;

       public CInstrumentConfig InstrumentConfig
       {
           get
           {
               return _instrumentConfig;
           }



       }

       
   



    


      
        public Dictionary<long, COrder> MonitorUserOrders  {get;set;}


      

 


        

        public string _test = "test";

        public string Test
        {
            get
            {
                return this._test;

            }


        }
      



     
       
      
         
        public System.Windows.Threading.Dispatcher GUIDispatcher {get;set;}


        const int NumAmounts = 5;


    
        
        


        private CLogger _logger;


        //public int StockNum { get; set; }


        CDataReciever _dataReciever = null;


	   

        CClusterProcessor _clusterProcessor;
		public CClusterProcessor ClusterProcessor
		{
			get
			{
				return _clusterProcessor;

			}


		}


		ViewModelUserPos _vmUserPos;// = new ViewModelUserPos();
        public ViewModelUserPos VMUserPos
       {
           get
           {
               return _vmUserPos;
           }
       }



        CUserLevels _userLevels;
        public CUserLevels UserLevels
        {
            get
            {
                return _userLevels;
            }


        }

      

        private CEventRouterViewModel _eventRouterViewModel;



		private TerminalViewModel _terminalViewModel;
		private CCommunicator _communicator;


        private List<CStockConf> _lstStockConf = new List<CStockConf>();



        public MarketViewModel(int stockNum,  ControlMarket controlMain, CKernelTerminal kernelTerminal, 
                                CStocksVisual stockVisual, CInstrumentConfig instrumentConfig)
        {
            StockNum = stockNum;
            string tickerName = stockVisual.Ticker;


            IsAlive = true;
            IsDead = false;


            SelectionMode = new CSelectionMode();
            string tf = instrumentConfig.MarketProperties.ClusterProperties.TimeFrame;
            _clusterProcessor = new CClusterProcessor(tickerName, tf);
            _userLevels =   new CUserLevels(tickerName);


            _eventRouterViewModel = new CEventRouterViewModel(this);

           // int conId = stockVisual.ConNumm;
          


            _orders = new OrderData[Params.MaxUserOrdersPerInstr];



		    _terminalViewModel =  CViewModelDispatcher.GetTerminalViewModel();




            int parUpdateStockPerSec = kernelTerminal.TerminalConfig.UpdateStockPerSec;
            _parSleep = (int)(1 / (double)parUpdateStockPerSec * 1000);

            _instrumentConfig = instrumentConfig;

        //    (new Thread(TestPar)).Start();


            GUIDispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;

            _controlMarket = controlMain;
            _kernelTerminal = kernelTerminal;
			_communicator = kernelTerminal.Communicator;

            //TextPriceSize = "10";
          //  StringHeight = "10";

            CurrAmountNum = "0";
            ListWorkAmount = new List<CWorkAmount>();
            
            SetInstrumentParams(stockVisual);
            _vmUserPos = new ViewModelUserPos(Decimals, Step);


           
            _parMaxVolumeBar = Int32.MaxValue;

            //RaisePropertyChanged("Contract_ShortName");


            ConnId = stockVisual.ConNumm;// conId;
            


            MonitorUserOrders = new Dictionary<long, COrder>();


            if (tickerName!=null)
                _logger = new CLogger("MarketViewModel_"+ tickerName);

             Log("Start logging__________________________________________");
           

          

            InitBidsAsks();

           

            //TODO remove
            this.InitializeStockData();

            SetMouseMode(true);

            _threadDataReciever = new Thread(ThreadDataReciever);
            _threadDataReciever.Start();
      

            //BindWorkAmount();
			
       
        }
        public void RouteEvent(object sender, ExecutedRoutedEventArgs e)
        {
            _eventRouterViewModel.RouteEvent(sender, e);

        }




        /// <summary>
        /// Call from:
        /// 
        /// 1)MarketViewModel Constructor
        /// 2)CKernelTerminal.EditConnectedStock
        /// </summary>
        /// <param name="stockVisual"></param>
        public void SetInstrumentParams(CStocksVisual stockVisual)
        {

            //m_sContract_ShortName = stockVisual.Ticker;

            

            TickerName = stockVisual.Ticker;
            //_tickerName = stockVisual.Ticker;

            Decimals = stockVisual.Decimals;
            Step = stockVisual.Step;
        }
   



        public void Log(string msg)
        {
            if (_logger != null)
                _logger.Log(msg);

        }

        private bool _bUpdInstrParamsFromGrid = true;

        /// <summary>
        /// Set instrument parameters for  Empty 
        /// viewmodel. Calling when set real 
        /// instrument selected.
        /// 
        /// If instrument was trade previously 
        /// settings file must be exists, load data from it.
        /// 
        ///  If instrument was not trade previously
        ///  settings file is not exist use ticker data receieved 
        ///  from the server
        ///  
        /// Calling from CKernelTerminal.EditNonConnectedStock
        /// </summary>
        /// <param name="conId"></param>
        /// <param name="ticker">ticker data(instrument, step, decimals, etc)  
        ///                      recieved from the server
        /// </param>
        public void SetEmptyVewModelInstrParams(int conId, CTIckerData ticker)
        {

            if (_logger == null)   
                _logger = new CLogger("MarketViewModel_"+ ticker.TickerName);



      
            string pathToConfig = CKernelTerminal.GetInstruemntPath(ticker.TickerName);
            
            //Config file for instrument is exists
            if (File.Exists(pathToConfig))
            {

                _instrumentConfig = _kernelTerminal.LoadInstrumentConfig(ticker.TickerName);
                CDataBinder.LoadMarketConfig(_instrumentConfig, this);

                //added 2018-02-27 for bitfinix capability
                if (_bUpdInstrParamsFromGrid)
                {
                   
                    Step = (double)ticker.Step;
                    DecimalVolume = ticker.DecimalVolume;
                    Decimals = ticker.Decimals;
                }


            }
            //Config file for instrument is not exists            
            else
            {
                TickerName = ticker.TickerName;
                Step = (double)ticker.Step;
                DecimalVolume = ticker.DecimalVolume;
                Decimals = ticker.Decimals;
              
              
                _instrumentConfig.Name = ticker.TickerName;
                _instrumentConfig.MarketProperties.StockProperties.TickerName = ticker.TickerName;
                _instrumentConfig.MarketProperties.StockProperties.Step = ticker.Step.ToString();
                _instrumentConfig.MarketProperties.StockProperties.Decimals = ticker.Decimals.ToString(); 


                _instrumentConfig.MarketProperties.DealsProperties.Step = ticker.Step.ToString();        
                _instrumentConfig.FileName = CKernelTerminal.GetInstruemntPath(ticker.TickerName);


               
                (new Task(SaveInstrumentConfig)).Start();
            }


            //note: ConId use for both cases
            ConnId = conId;


           
           
        }


        public void ShowChangeInstrumentWindow(Point pnt)
        {
            if (_kernelTerminal.Communicator.IsConnectedSomething()) 
            {
                AvailableTickersWindow atw =  (AvailableTickersWindow) _kernelTerminal.ViewModelDispatcher.OpenChildWindow<AvailableTickersWindow>();          
                atw.CurrentMarketViewModel = this;



               // double left = Canvas.GetLeft(this._controlMarket);
                //atw.Left = left;
               // Point locationFromScreen = pnt;// _controlMarket.PointToScreen(new Point(0, 0));
                //PresentationSource source = PresentationSource.FromVisual(_controlMarket);
                System.Windows.Point targetPoints = pnt;// source.CompositionTarget.TransformFromDevice.Transform(locationFromScreen);
                atw.Left = targetPoints.X;
                atw.Top = targetPoints.Y;
            }
            //SetStockNum(

        }

		/// <summary>
		/// Wait untill thread that recieves data from netwotk 
		/// connection (ThreadDataReciever) terminates
		/// than call method _kernelTerminal.DeleteExistingStock
		/// 
		/// Call from
		/// Eventhandler OnDeleteInstrument
		/// </summary>
        public void DeleteInstrument()
        {

            IsAlive = false;
            while (!IsDead && _threadDataReciever.IsAlive)
                Thread.Sleep(1);

            _kernelTerminal.DeleteExistingStock(ControlMarket.StockNum,  _tickerName);

                                           
        }




      /// <summary>
      ///1. Copies data from marketviewModel to config
      ///2. Call saving config to disk
      /// 
      /// Call from  
      /// 1. MarketViewModel.SetEmptyVewModelInstrParams - when create empty view model (task)
      /// 2. MarketViewModel.OnClose - when closing MainWindow (i.e close application) (synchronous, GUI thread)
        /// 3. OnSaveInstrumentConfig - from Views : MarketSettingsWindow.Close, from ControlAmountTextblock    (task)
      /// </summary>
      public void SaveInstrumentConfig()
      {

            for (int i = 0; i < ListWorkAmount.Count; i++)
                _instrumentConfig.WorkAmounts[i] = Convert.ToDecimal(ListWorkAmount[i].TextAmountValue);


            CDataBinder.SaveMarketConfig(_instrumentConfig, this);

            _kernelTerminal.SaveInstrumentConfig(ref _instrumentConfig);
            

          
            
        }





		public void AcceptStopLossTakeProfit(CSetOrder setOrder)
		{
			if (setOrder.OrderType == EnmOrderTypes.TakePofit)			
				TakeProfitPrice = (double)setOrder.Price;

            if (setOrder.OrderType == EnmOrderTypes.StopLoss)
                StopLossPrice = (double)setOrder.Price;

            if (setOrder.OrderType == EnmOrderTypes.StopLossInvert)
                StopLossInvertPrice = (double) setOrder.Price;

            if (setOrder.OrderType == EnmOrderTypes.BuyStop)
            {
                BuyStopPrice = (double)setOrder.Price;
                BuyStopAmount = setOrder.Amount;

            }

            if (setOrder.OrderType == EnmOrderTypes.SellStop)
            {
                SellStopPrice = (double)setOrder.Price;
                SellStopAmount = setOrder.Amount;

            }



			
		}

        /// <summary>
        /// Call from 
        /// 
        /// 1) CKernelTerminal.UpdateUserMonitorPos (from DataReciever)
        /// 2) CKernelTerminal.TaskLateUpdPosMon (on start when not loaded yet)
        /// </summary>
        /// <param name="userPos"></param>
        public void UpdateUserPos(CUserPos userPos)
        {        
            _vmUserPos.Update(userPos.Amount, userPos.AvPos, Bids[0].Price, Asks[0].Price,
                                Step, Decimals); 

        }
        

        public void CancellOrderbyId(long id)
        {
            CCancellOrderById ordById = new CCancellOrderById { Id = id };
            enmTradingEvent ev = enmTradingEvent.CancellOrderById;
            _kernelTerminal.Communicator.SendDataToServer(ConnId,ordById, ev);
        }

        public void CancellAllOrders()
        {

            if (_tickerName == null)
                return;



            CCancellOrderByIsin cancellAllOrders = new CCancellOrderByIsin { Isin = _tickerName };
            enmTradingEvent ev = enmTradingEvent.CancellOrdersByIsin;
            _kernelTerminal.Communicator.SendDataToServer(ConnId,cancellAllOrders,ev);
            Log("CancellAllOrders");

        }

        public void CloseAllPositions()
        {
            if (_tickerName == null)
            {
                Log("Try CloseAllPositions but _tickerName is null");
                return;
            }
           
            if (VMUserPos!=null && VMUserPos.Amount !=0)
            //if (UserPos != null && UserPos.Amount != 0)
            {

                CCloseAllPositionsByIsin closeByIsin = new CCloseAllPositionsByIsin { Isin = _tickerName };
                enmTradingEvent ev = enmTradingEvent.CloseAllPositionsByIsin;
                _kernelTerminal.Communicator.SendDataToServer(ConnId,closeByIsin, ev);
                Log("CloseAllPositions _tickerName=" + _tickerName);
            }
            else
            {
                string msg = "Try CloseAllPositions but";
                if (VMUserPos == null)
                    msg += " VMUserPos is null";
                else
                    msg += " VMUserPos.Amount ==0";

                Log(msg);
            }


        }
        //deprecated
        public void CloseAllPositionsByIsin(string isin)
        {
            if (_tickerName == null)
                return;


            CCloseAllPositionsByIsin closeByIsin = new CCloseAllPositionsByIsin { Isin = isin };
            enmTradingEvent ev = enmTradingEvent.CloseAllPositionsByIsin;
            _kernelTerminal.Communicator.SendDataToServer(ConnId,closeByIsin, ev);
            Log("CloseAllPositionsByIsin isin="+isin);
        }

    

        public void OnClose ()
        {

           
           SaveInstrumentConfig();

        }



        public void AddOrder(string isin, decimal amount, EnmOrderDir dir, decimal price )
        
        {
         
            int id = Convert.ToInt16(_kernelTerminal.TerminalConfig.User);

            CAddOrder ord = new CAddOrder {IdTrader= id, Amount = amount, Dir = dir, Isin = _tickerName, Price = price };

            enmTradingEvent ev = enmTradingEvent.AddOrder;
            
           
            _kernelTerminal.Communicator.SendDataToServer(ConnId,ord,ev);


            Log("AddOrder IdTrader="+id+" Amount="+amount+" Dir="+dir+" isin="+ _tickerName+" price="+price);

        }



        public void Error(string msg, Exception e =null )
        {
            _kernelTerminal.Error(msg, e);

        }



        private bool IsDataUnavailable(CDataReciever dataReciever )
        {
            if (dataReciever == null || _tickerName == null ||
                    !dataReciever.OutpStockClass.ContainsKey(_tickerName) ||
                    !dataReciever.OutpListAggrDeals.ContainsKey(_tickerName) ||
                    !dataReciever.OutpListRawDeals.ContainsKey(_tickerName))
                    return true;


            return false;
        }



        /// <summary>
        /// Call from:
        /// 
        /// ThreadDataReciever
        /// </summary>
        private void ProcessStocks()
        {
            var dictStockClass = _dataReciever.OutpStockClass[_tickerName];


            if (dictStockClass.LstStockConf ==null)
                return; //get out

            lock (_lstStockConf)
            {
                _lstStockConf.Clear();
                dictStockClass.LstStockConf.ForEach(el => _lstStockConf.Add(el));

            }


            var stockConf = dictStockClass.LstStockConf.Find(el => el.MinStep == (decimal) Step);
            if (stockConf == null)
                return; //get out

            int prec = stockConf.PrecissionNum;


            //process stocks
            if (dictStockClass.StockListAsks != null && dictStockClass.StockListBids != null)
            {

                lock (_dataReciever.OutpStockClass)
                {

                    lock (dictStockClass.Locker)
                    {
                      
                        try
                        {
                            bool bStockChanged = false;

                            if (dictStockClass.StockListAsks.ContainsKey(prec))
                            {
                                List<CStock> asks = dictStockClass.StockListAsks[prec];

                                if (asks != null)
                                {
                                    for (int i = 0; i <  Asks.Length; i++)
                                    {
                                        if (i >= asks.Count)
                                        {
                                            Asks[i].Price = 0;
                                            Asks[i].Amount = 0;
                                            continue;
                                        }


                                        if (Asks[i].Price != (double)asks[i].Price ||
                                            Asks[i].Amount != (int)asks[i].Volume

                                            )
                                        {


                                            Asks[i].Price = (double)asks[i].Price;
                                            Asks[i].Amount = (int)asks[i].Volume;

                                            bStockChanged = true;

                                            //debugging
                                           // if (Bids[0].Price!=0 &&Asks[i].Price <= Bids[0].Price)
                                             //   Thread.Sleep(0);

                                        }

                                    }
                                    //2018-07-09
                                    


                                }
                            }

                            if (dictStockClass.StockListBids.ContainsKey(prec))
                            {

                                List<CStock> bids = dictStockClass.StockListBids[prec];

                                if (bids !=null)
                                {

                                    for (int i = 0; i < Bids.Length; i++)
                                    {

                                        if (i >= bids.Count)
                                        {
                                            Bids[i].Price = 0;
                                            Bids[i].Amount = 0;
                                            continue;
                                        }


                                        if (Bids[i].Price != (double)bids[i].Price ||
                                          Bids[i].Amount != (int)bids[i].Volume)
                                        {
                                            Bids[i].Price = (double)bids[i].Price;
                                            Bids[i].Amount = (int)bids[i].Volume;

                                            bStockChanged = true;

                                            //if (Asks[0].Price!=0 && Bids[i].Price >= Asks[0].Price)
                                              //  Thread.Sleep(0);

                                        }
                                    }

                                }

                            }
                                    /*
                                    for (int k = 0; k < Asks.Length; k++)
                                    {


                                         if (Bids[0].Price!=0 && Asks[k].Price!=0 &&  Asks[k].Price <= Bids[0].Price)
                                               Thread.Sleep(0);
                                    }

                                     for (int j = 0; j < Bids.Length; j++)
                                     {
                                     

                                         if ((Asks[0].Price != 0)  && (Bids[j].Price >= Asks[0].Price))
                                            Thread.Sleep(0);
                                    }
                                    */
                           



                            if (bStockChanged)
                        {
                                _vmUserPos.Recalc((double)Bids[0].Price, (double)Asks[0].Price, Step, Decimals);
                                 ForceRepaintControlStock();

                                GUIDispatcher.BeginInvoke(new Action(() =>
                            {

                                try
                                {
                                    //note: put it here because of synchronizing problems (pforit calc on graphics is faster !)
                                   
                                   
                                }
                                catch (Exception e)
                                {
                                    Error("MarketViewModel process stocks GUI", e);
                                }



                            }
                            ));


                        }


                        }
                        catch (Exception e)
                        {
                            Error("", e);
                        }

                        // else
                        //  Thread.Sleep(0);



                    }

                }

            }


        }


        private bool IsPossibleSetSLTP()
        {
            if (Bids == null || Asks == null ||
                Bids.Length == 0 || Asks.Length ==0 ||
                VMUserPos.Amount ==0

                )
                return false;

            return true;
        }

        private bool IsPossibleSetStopOrder()
        {
			//enable stoporder on opended pos
            if (Bids == null || Asks == null ||
                Bids.Length == 0 || Asks.Length == 0  /*||
                VMUserPos.Amount != 0*/
                    )
                return false;

            return true;
        }


        private void SendOrderType(EnmOrderTypes orderType, decimal price, decimal amount=0)
        {

            _kernelTerminal.Communicator.SendOrderType(ConnId, TickerName, orderType, price, amount);
            Log("SendOrderType. TcikerName="+TickerName+" orderType="+orderType+" price="+price+" amount="+amount);
        }




        public void SetStopLossTakeProfit(object sender, ExecutedRoutedEventArgs e)
        {


            if (!IsPossibleSetSLTP())
                return;

          decimal bid = (decimal) Bids[0].Price;
          decimal ask = (decimal) Asks[0].Price;
          decimal selectedPrice = (decimal)  (double) e.Parameter;
                                             
          if (VMUserPos.Amount > 0) //buy
          {
              if ((selectedPrice > VMUserPos.AvPos) &&
                  (selectedPrice > bid))
              {
                  if (selectedPrice == (decimal)TakeProfitPrice)
                    SendOrderType(EnmOrderTypes.TakePofit, 0);
                  else 
                    SendOrderType(EnmOrderTypes.TakePofit, selectedPrice);

              }

              if (/*(selectedPrice < VMUserPos.AvPos) &&*/
                  (selectedPrice < bid))
              {
                  if (selectedPrice == (decimal)StopLossPrice)
                    SendOrderType(EnmOrderTypes.StopLoss, 0);  
                  else
                    SendOrderType(EnmOrderTypes.StopLoss, selectedPrice);


              }


          }
          else //<0 sell
          {
              if ((selectedPrice < VMUserPos.AvPos) &&
                  (selectedPrice < ask))
              {
                  if (selectedPrice == (decimal)TakeProfitPrice)
                      SendOrderType(EnmOrderTypes.TakePofit, 0);
                  else
                      SendOrderType(EnmOrderTypes.TakePofit, selectedPrice);
              }

              if (/*(selectedPrice > VMUserPos.AvPos) &&*/
                  (selectedPrice > ask))
              {
                  if (selectedPrice == (decimal)StopLossPrice)
                      SendOrderType(EnmOrderTypes.StopLoss, 0);
                  else
                      SendOrderType(EnmOrderTypes.StopLoss, selectedPrice);

              }   

           } 

        }




        public void SetStopLossInvert(object sender, ExecutedRoutedEventArgs e)
        {


            if (!IsPossibleSetSLTP())
                return;

            decimal bid = (decimal)Bids[0].Price;
            decimal ask = (decimal)Asks[0].Price;
            decimal selectedPrice = (decimal)(double)e.Parameter;

            if (VMUserPos.Amount > 0) //buy
            {
               
				//2017-03-10
                //if (selectedPrice < bid)
                {
                    if (selectedPrice == (decimal) StopLossInvertPrice)
                        SendOrderType(EnmOrderTypes.StopLossInvert, 0);
                    else
                        SendOrderType(EnmOrderTypes.StopLossInvert, selectedPrice);


                }


            }
            else //<0 sell
            {

                //if (selectedPrice > ask)
                {
                    if (selectedPrice == (decimal)StopLossInvertPrice)
                        SendOrderType(EnmOrderTypes.StopLossInvert, 0);
                    else
                        SendOrderType(EnmOrderTypes.StopLossInvert, selectedPrice);

                }

            } 


         

        }

        public void AddDelUserLevel(object sender, ExecutedRoutedEventArgs e)
        {
            _userLevels.AddDelUserLevel((double)e.Parameter);
        }

		public void InvertPosition(object sender, ExecutedRoutedEventArgs e)
		{
			
			_communicator.InvertPosition(ConnId,_tickerName);
		}

		public void SendOrderThrow(object sender, ExecutedRoutedEventArgs e)
		{

			int num = Convert.ToInt16(CurrAmountNum);
			decimal amount = Convert.ToDecimal(ListWorkAmount[num].TextAmountValue.Replace(@".",@","));//changed 2018-03-21
			//int throwSteps = 0;

			_communicator.SendOrderThrow(ConnId, _tickerName, amount, (EnmOrderDir) e.Parameter, ThrowSteps);			
		}

        public void SendRestOrder(object sender, ExecutedRoutedEventArgs e)
        {
            _communicator.SendOrderRest(ConnId, (CDataRestOrder)  e.Parameter);
			Log("Send restOrder");

        }

        public void StockWidthChanged(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
               double newWidth = (double)e.Parameter;

               lock (_kernelTerminal.StockVisualConf)
                    _kernelTerminal.StockVisualConf.ListStocksVisual[StockNum].WidthStock = newWidth;

                _kernelTerminal.SaveVisualConf();

            }
            catch (Exception exc)
            {
                Error("MarketViewModel.StockWidthChanged", exc);
            }
            
        }

        public void ClusterWidthChanged(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                double newWidth = (double)e.Parameter;

                lock (_kernelTerminal.StockVisualConf)
                    _kernelTerminal.StockVisualConf.ListStocksVisual[StockNum].WidthClusters = newWidth;

                _kernelTerminal.SaveVisualConf();

            }
            catch (Exception exc)
            {
                Error("MarketViewModel.ClusterWidthChanged", exc);
            }


        }




		public void SetKeyboardTradingMode(object sender, ExecutedRoutedEventArgs e)
		{
			if (!IsModeKeyboardTrading)
			{
				IsModeKeyboardTrading = true;
				_kernelTerminal.ViewModelDispatcher.ResetAllKeyBoardTradingExcpt(StockNum);
			}
			else
			{
				IsModeKeyboardTrading = false;
			}

		}


		public void ResetKeyboardTradingMode()
		{
			IsModeKeyboardTrading = false;			
		}




        public void SetStopOrder(object sender, ExecutedRoutedEventArgs e)
        {
            if (!IsPossibleSetStopOrder())
                return ;

            decimal bid = (decimal)Bids[0].Price;
            decimal ask = (decimal)Asks[0].Price;

			CDataStopOrder dso = (CDataStopOrder)e.Parameter;
			decimal amount = dso.Amount;

            decimal selectedPrice = dso.Price;

            if (selectedPrice > ask)
            {
                if (selectedPrice == (decimal) BuyStopPrice)
                    SendOrderType(EnmOrderTypes.BuyStop, 0, 0);
                else
                    SendOrderType(EnmOrderTypes.BuyStop, selectedPrice, amount);
            }

            if (selectedPrice < bid)
            {

                if (selectedPrice == (decimal)SellStopPrice)
                    SendOrderType(EnmOrderTypes.SellStop, 0, 0);
                else
                    SendOrderType(EnmOrderTypes.SellStop, selectedPrice, amount);

            }
        }





        public void OnChangeTimeFrame(object sender, ExecutedRoutedEventArgs e)
        {

            string tf = (string) e.Parameter;

            if (tf != TimeFrame)
            {
                TimeFrame = tf;

                _clusterProcessor.ChangeIntervals(tf);
            }
        }


        private void ProcessAggrDeals()
        {
            //process deals
            lock (_dataReciever.OutpListAggrDeals)
            {
                lock (_dataReciever.OutpListAggrDeals[_tickerName])
                {


                    //get data from deal queue
                    while (_dataReciever.OutpListAggrDeals[_tickerName].Count != 0)
                    {
                     
                        {


                            CDeal deal = new CDeal();

                            deal.Amount = (int)_dataReciever.OutpListAggrDeals[_tickerName][0].Amount;
                            deal.Price = Math.Round((double)_dataReciever.OutpListAggrDeals[_tickerName][0].Price, this.Decimals);

                            deal.Direction = (EnmDealDirection)_dataReciever.OutpListAggrDeals[_tickerName][0].DirDeal;
                            deal.DateTime = _dataReciever.OutpListAggrDeals[_tickerName][0].DtTm;

                            //TODO try to do with dependency property
                            //changed 2016_12_13
                         
                            AddNewDeal(deal);

                         

                        }

                        //note we remove it from queue after read
                        _dataReciever.OutpListAggrDeals[_tickerName].RemoveAt(0);

                    }

                }
            }

        }

        public void RepaintDeals()
        {
            IsNeedRepaintDeals = true;


        }
        private void AddNewDeal(CDeal deal)
        {

            const double parMaxSize = 1000;

            lock (_ticks)
            {
                _ticks.Add(deal);
                if (_ticks.Count > parMaxSize)
                    _ticks.RemoveAt(0);

            }


            RepaintDeals();

       

        }

        private void ProcessRowDeals()
        {
            lock (_dataReciever.OutpListRawDeals)
            {
                lock (_dataReciever.OutpListRawDeals[_tickerName])
                {
                    while (_dataReciever.OutpListRawDeals[_tickerName].Count != 0)
                    {

                        CDeal tk = new CDeal();

                        tk.Amount = (int)_dataReciever.OutpListRawDeals[_tickerName][0].Amount;
                        tk.Price = Math.Round((double)_dataReciever.OutpListRawDeals[_tickerName][0].Price, this.Decimals);

                        tk.Direction = (EnmDealDirection)_dataReciever.OutpListRawDeals[_tickerName][0].DirDeal;
                        tk.DateTime = _dataReciever.OutpListRawDeals[_tickerName][0].DtTm;

                        //TODO try to do with dependency property
                        //_controlMarket.ControlClustersInstance.Ticks = tk;
                        _dataReciever.OutpListRawDeals[_tickerName].RemoveAt(0);
                        _clusterProcessor.Update(tk);
                    }
                }

            }
        }



        private int GetParSleepBeetwenStockUpd()
        {

            double par = 200; //default value

            double maxValue = 200; //5 per sec
            double minValue = 30; //33 per sec

         


            if (_terminalViewModel != null)
            {
                double updPerSec = (double) _terminalViewModel.StockUpdatePerSec;
                if (updPerSec != 0)
                {

                    par = 1 / updPerSec * 1000;

                 
                }

            }

            par = Math.Min(maxValue, par);
            par = Math.Max(minValue, par);


            return (int) par;

        }


    
      /// <summary>
        /// Thread gets market data from DataRecieve
        /// and copy it to the MarketViewModel       
      /// </summary>   
        private void ThreadDataReciever()
        {
            CCommunicator communicator = _kernelTerminal.Communicator;
        

           
     
            while (IsAlive)
            {
             try
             {

                //whait till data is available
                while (IsDataUnavailable(_dataReciever))                                        
                {
                    _dataReciever =  communicator.GetDataReciever(ConnId);
                    Thread.Sleep(100);

                    if (!IsAlive)
                        return;
                 
                }
                DateTime dtBegin = DateTime.Now;
                ProcessStocks();
                ProcessAggrDeals();
                ProcessRowDeals();


                int parSleep = GetParSleepBeetwenStockUpd();

              


              //  int minSleePeriod = 50;


                double dt = parSleep - (DateTime.Now - dtBegin).TotalMilliseconds;
              //  dt = Math.Max(dt,minSleePeriod);

               if (dt>0)
                 Thread.Sleep((int)dt);

            }
              
                 catch (Exception e)
                {
                   Error("MarketViewModel.ThreadDataReciever", e);
                }

            }


            IsDead = true;

        }

      

        public void InitBidsAsks()
        {
            //2018-07-04 changesd 50->100
            m_Bids = new CStockPosition[100];
            m_Asks = new CStockPosition[100];

            for (int i = 0; i < 100; i++)
            {
                m_Bids[i] = new CStockPosition();
                m_Asks[i] = new CStockPosition();

            }
           ;
        }


	   //TODO make normal
        private void SetMouseMode(bool p)
        {
            //KAA
            //this.m_ControlOwner.MouseMode = p;
            this._controlMarket.MouseMode = p;
        }


        public void CancellOrdersWithPrice(double price)
        {
           //KAA 2017_01_24
           /* foreach (var v in MonitorUserOrders)            
                if ((double)v.Value.Price == price)
                    CancellOrderbyId(v.Key);
            */
            foreach (var v in Orders)            
                if (v.Price == price)
                  CancellOrderbyId(v.OrderID);
            


            Log("CancellOrdersWithPrice");
        }


        public void CleanOrders()
        {
            
           for (int i=0; i< Orders.Length; i++)            
                Orders[i] = new OrderData();
            



        }

        public void UpdateUsersOrders(Dictionary <long, COrder> orders)
        {

			//KAA 2016-01-23

			if (orders == null)
				orders = new Dictionary<long, COrder>();

            int i = 0;
         //   if (MonitorUserOrders != null)            
             lock (Orders)
            {
                CleanOrders();
              foreach (var v in orders)
              {
                 long id = v.Key;
                 COrder ord = v.Value;

                //MonitorUserOrders[i]. = id;
                 Orders[i].OrderID = id;
                 Orders[i].Price = (double)ord.Price;
                 Orders[i].Amount = ord.Amount;
                 Orders[i].Action = (ord.Dir == (sbyte)EnmOrderDir.Buy) ? EnmOrderAction.Buy : EnmOrderAction.Sell;
                 Orders[i].State = PortfolioOwnedOrderState.Active;
                 i++;
              }
                    //m_Orders
                    
                    //MonitorUserOrders = orders;
           }            
          //  else
              //  MonitorUserOrders = orders;


            /*
            if (orders != null)
            {            

             
                this.m_Orders = new OrderData[orders.Count];
                int i = 0;
                foreach (var v in orders)
                {
                    long id = v.Key;
                    COrder ord = v.Value;

                    m_Orders[i].OrderID = id;
                    m_Orders[i].Price = (double)ord.Price;
                    m_Orders[i].Amount = ord.Amount;
                    m_Orders[i].Action = (ord.Dir == (sbyte)EnmOrderDir.Buy) ? EnmOrderAction.Buy : EnmOrderAction.Sell;
                    m_Orders[i].State = PortfolioOwnedOrderState.Active;
                    i++;
                }
                



            }
            else
            {
                this.m_Orders = new OrderData[0];

            }
            RaisePropertyChanged("Orders");
        */
        }

		/// <summary>
        /// Call when instrument parameter changed (for example Decimals)
        /// during trading. Mostly for bitfinex.
        /// 
		/// Call from  CKernelTerminal.ProcessUserUpdateAvailableTickers
		/// </summary>
		/// <param name="updInstrParams"></param>
		public void UpdateInstrumentParamsOnline(CUpdateInstrumentParams updInstrParams)
		{
            //removed 2018-06-25
            //if (updInstrParams.Decimals != Decimals)
				//Decimals = updInstrParams.Decimals;

			//if ((double) updInstrParams.Min_step !=  Step)
				//Step = (double) updInstrParams.Min_step;

            if (updInstrParams.DecimalVolume != DecimalVolume)
                DecimalVolume = updInstrParams.DecimalVolume;



			CStocksVisual sv = new CStocksVisual
			{
				Decimals = Decimals,
				ConNumm = ConnId,
				Step = Step,
				Ticker = TickerName
			};
													  

			_kernelTerminal.UpdateStockVisualInstrumentParams(StockNum,sv);
			SaveInstrumentConfig();

		}


      /// <summary>
      /// Call when connection with server established, auth past and 
      /// server sent ticker's data.
      /// </summary>
      /// <param name="tickerData"></param>
		public void UpdateInstrParamsOnConnection(CTIckerData tickerData)
		{
            
            //removed 2018-06-25
		/*	if (tickerData.Decimals != Decimals)
				Decimals = tickerData.Decimals;

			if ((double) tickerData.Step != Step)
				Step = (double)tickerData.Step;
                */
            if (tickerData.DecimalVolume != DecimalVolume)
                DecimalVolume = tickerData.DecimalVolume;



			CStocksVisual sv = new CStocksVisual
			{
				Decimals = Decimals,
				ConNumm = ConnId,
				Step = Step,
				Ticker = TickerName
			};


			_kernelTerminal.UpdateStockVisualInstrumentParams(StockNum, sv);
			SaveInstrumentConfig();


		}



        public void InitializeStockData()
        {
            Random random = new Random();
            int num = 40;
          //  this.m_dStep = 10.0;
            if (this.m_Asks == null)
            {
                this.m_Asks = new CStockPosition[num];
            }
            if (this.m_Bids == null)
            {
                this.m_Bids = new CStockPosition[num];
            }

           
              
                  
        }
      
         private void m_Timer_Elapsed(object sender, ElapsedEventArgs e)
         {
             try
             {
                 this.InitializeStockData();
             }
             catch (Exception exc)
             {

                 Error("MarketViewModel.m_Timer_Elapsed", exc);
             }

         }


         public void OnSizeChanged(object sender, ExecutedRoutedEventArgs e)
         {
           
           
         }

         public void OnSaveInstrumentConfig(object sender, ExecutedRoutedEventArgs e)
         {
             try
             {
                 CUtil.TaskStart(SaveInstrumentConfig);
             }
             catch (Exception exc)
             {
                 Error("MarketViewModel.OnSaveInstrumentConfig",exc);
             }
         }

         public void OnAddOrder(object sender, ExecutedRoutedEventArgs e)
         {

             CDataAddOrder dao = (CDataAddOrder)e.Parameter;
             AddOrder(dao.Instrument, dao.Amount, dao.Dir, dao.Price);
             
         }

         public void OnCancellOrdersWithPrice(object sender, ExecutedRoutedEventArgs e)
         {
             CancellOrdersWithPrice((double)e.Parameter);             
         }

         public void OnCancellAllOrders(object sender, ExecutedRoutedEventArgs e)
         {
             CancellAllOrders();
         }

         public void OnCloseAllPositions(object sender, ExecutedRoutedEventArgs e)
         {

             CloseAllPositions();
         }

         public void OnCloseAllPositionsByIsin(object sender, ExecutedRoutedEventArgs e)
         {

             //CloseAllPositionsByIsin(
         }

         public void  OnShowChangeInstrumentWindow(object sender, ExecutedRoutedEventArgs e)
         {

             ShowChangeInstrumentWindow((Point) e.Parameter);

         }

         public void OnDeleteInstrument(object sender, ExecutedRoutedEventArgs e)
         {
             DeleteInstrument();
         }

        private void SaveInstrumentParams()
        {
            CUtil.TaskStart(SaveInstrumentConfig);

            CStocksVisual sv = new CStocksVisual
            {
                Decimals = Decimals,
                ConNumm = ConnId,
                Step = Step,
                Ticker = TickerName
            };


            _kernelTerminal.UpdateStockVisualInstrumentParams(StockNum, sv);

        }



        public void IncreaseMinStep(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                lock (_lstStockConf)
                {
                    int ind = -1;
                    for (int i = 0; i < _lstStockConf.Count; i++)
                    {
                        if (_lstStockConf[i].MinStep == (decimal)Step)
                        {
                            ind = i;
                            break;
                        }
                    }

                    if (ind >= 1)
                    {
                        ind--;
                        Step = (double)_lstStockConf[ind].MinStep;
                        Decimals = _lstStockConf[ind].DecimalsPrice;
                        IsNeedRepaintDeals = true;
                        SaveInstrumentParams();
                     //   ClusterProcessor.RebuildCurrentClusters();
                    }

                }
            }
            catch (Exception exc)
            {
                Error("MarketViewModel.IncreaseMinStep", exc);

            }

           
        }


        public void DecreaseMinStep(object sender, ExecutedRoutedEventArgs e)
        {

            try
            {
                lock (_lstStockConf)
                {
                    int ind = -1;
                    for (int i = 0; i < _lstStockConf.Count; i++)
                    {
                        if (_lstStockConf[i].MinStep == (decimal)Step)
                        {
                            ind = i;
                            break;
                        }
                    }

                    if (ind >= 0 && ind < _lstStockConf.Count - 1)
                    {
                        ind++;
                        Step = (double)_lstStockConf[ind].MinStep;
                        Decimals = _lstStockConf[ind].DecimalsPrice;
                        IsNeedRepaintDeals = true;

                        SaveInstrumentParams();

                    }

                }
            }
            catch (Exception exc)
            {
                Error("MarketViewModel.DecreaseMinStep", exc);

            }


        }




        /* public void OnChangeTimeFrame(object sender, ExecutedRoutedEventArgs e)
         {
       


         }
      */


    }
}
