using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Windows;

using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Threading;

using System.Collections.ObjectModel;
using System.IO;


using ProtoBuf;


using Common;
using Common.Interfaces;
using Common.Utils;


using TCPLib;

using TradingLib;
using TradingLib.Enums;
using TradingLib.ProtoTradingStructs;
using TradingLib.Data;

//using Visualizer;
using Terminal.Common;
using Terminal.Conf;
using Terminal.Events;
using Terminal.Events.Data;

using Terminal.Views;
using Terminal.Views.ChildWindows;
using Terminal.ViewModels;
using Terminal.Controls;
using Terminal.Controls.Market;

using Terminal.DataBinding;
using Terminal.Graphics;
using Terminal.Communication;
using Terminal.TradingStructs;

using System.Reflection;
using System.Windows.Input;



namespace Terminal
{

    //Responce: call members from one class to another
    //Like a mediator pattern



    public class CKernelTerminal : DependencyObject, IAlarmable
    {
        CAlarmer _alarmer;
        public CViewModelDispatcher ViewModelDispatcher { get; set; }
        public CViewDispatcher ViewDispatcher { get; set; }
        public CCommunicator Communicator { get; set; }

        private CPasswordSaver _passwordSaver ;



        private DateTime _dtLastSave;

        private ConnectionTrialEventHandler ConnectionTrial;
		private ConnectedSuccessEventHandler AuthSuccess;

        private CPreRunCondChecker _condChecker = new CPreRunCondChecker();


        private CGlobalConfig _globalConfig = new CGlobalConfig();
        public CGlobalConfig GlobalConfig
        {
            get
            {
                return _globalConfig;
            }

        }
        




        
        /*
        // == SHARED SETTINGS == Step 2 create DP in kernel terminal

        public int StringHeight
        {
            get { return (int)GetValue(StringHeightProperty); }
            set { SetValue(StringHeightProperty, value); }
        }

   
        // Using a DependencyProperty as the backing store for StringHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StringHeightProperty =
            DependencyProperty.Register("StringHeight", typeof(int), typeof(CKernelTerminal), new UIPropertyMetadata(1));

       

        public int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }


  
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(int), typeof(CKernelTerminal), new UIPropertyMetadata(1));
        */



        private CTerminalConfig _terminalConfig;

        public CTerminalConfig TerminalConfig
        {
            get
            {
                return _terminalConfig;
            }
            set
            {
                          
                _terminalConfig = value;

            }

        }

        //private 



        public Dispatcher GUIDispatcher { get; set; }

        private CEvntDispTerminal _evntGUIDispatcher;// = new CEvntGUIDispatcher();

        private CStocksVisualConf _stocksVisualConf;

        public CStocksVisualConf StockVisualConf
        {
            get
            {
                return _stocksVisualConf;

            }

        }




        public CAlarmer Alarmer
        {
            get
            {
                return _alarmer;
            }

        }



        /// <summary>
        /// Note: StockVisualConf.ListStocksVisual must be not empty
        /// </summary>
        public int MaxStockNum 
        {
            get
            {
                return StockVisualConf.ListStocksVisual.Count - 1;

            }
        }

        private CColorList _colorList;
        

        public CColorList ColorList
        {
            get
            {
                return _colorList;

            }
           

        }



        private object _guiLocker = new object();
        
        public object GUIILocker
        {
            get
            {
                return _guiLocker;
            }

        }


        private void CheckPreRunCondProcessed()
        {
            _condChecker.Check();
           
        }



        private void ThreadWatchDog()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(5);

            }


        }





        public CKernelTerminal()
        {

            CUtil.ThreadStart(ThreadWatchDog);

            //2018-03-28 moved up as it was after ViewDispatcher create line
            _evntGUIDispatcher = new CEvntDispTerminal(this);

            if (!IsUniqueProcess())
                return;

            CheckPreRunCondProcessed();

            ReadGlobalConfig();

            ReadTerminalConfig();

            ReadVisualStockConfig();
            SetMinThreads();

            Communicator = new CCommunicator(this);
            ViewModelDispatcher = new CViewModelDispatcher(this);
            ViewDispatcher = new CViewDispatcher(this);
           
			_passwordSaver = new CPasswordSaver(this);


            LoadColorList();
            
            //Bind connection trial and auth success events
            //for manual (GUI) and aftomatic(passwordsave) auth requesters. 
			ConnectionTrial += Communicator.OnUserTryConnectToServer;
            ConnectionTrial += _passwordSaver.OnConnectionTrial;

			AuthSuccess += _passwordSaver.OnConnectedSuccess;
			AuthSuccess += SubscribeTickersFromConfig;
            AuthSuccess += SendTeminalInfoForDealingServer;


        }

        private void SetMinThreads()
        {
            int numEl = _stocksVisualConf.ListStocksVisual.Count;
            int threadsPerStock = 14;//was 14
            int threadsNeed = numEl * threadsPerStock;

            System.Threading.ThreadPool.SetMinThreads(threadsNeed,threadsNeed);
         //   System.Threading.Thread.Sleep(10000);
            int thr1 = 0;
            int thr2 = 0;

            System.Threading.ThreadPool.GetMinThreads(out thr1, out thr2);

        }



		public void SetDataFromConfigToTerminalViewModel()
		{

			lock (TerminalConfig)
			{
				foreach(var propertyFromList in _terminalConfig.TerminalProperties.TerminalGlobalProperties.GetType().GetProperties())
				{


					PropertyInfo VMProperty = ViewModelDispatcher.TerminalViewModel.GetType().GetProperty(propertyFromList.Name);
					if (VMProperty !=null)
					{

						CUtilReflex.SetPropertyValue(ViewModelDispatcher.TerminalViewModel,
							VMProperty,
							propertyFromList.GetValue(_terminalConfig.TerminalProperties.TerminalGlobalProperties,null));

					}
					//if (property.Name == 



				}


			}



		}


        private void ReadGlobalConfig()
        {
            string pathFile = CUtil.GetConfigDir() + @"\globalSettings.xml";
            _globalConfig = new CGlobalConfig(pathFile);
            CSerializator.Read<CGlobalConfig>(ref _globalConfig);
        }



		private void ReadTerminalConfig()
		{
			CUtil.ReadConfig<CTerminalConfig>(ref _terminalConfig, "terminal");
			TerminalConfig = _terminalConfig; //for initial values of shared settings
		}

     



        /// <summary>
        /// Automatic connection sequence entry point.
        /// 
        /// Call from MainWindow.MainWindow_Loaded
        /// 
        /// </summary>
		public void AutoConnection()
		{
			List<CServer> servers = Communicator.ServersConf.Servers;
			for (int i = 0; i < servers.Count; i++)
			{
                string serverName = Communicator.ServersConf.Servers[i].Name;
				//UserConReq req =  _passwordSaver.GetUserConReq(i);
                UserConReq req = new UserConReq
                {
                    AuthRequest = _passwordSaver.GetUserAuthReq(i),
                    ConnNum = i,
                    ServerName = serverName
                };
                

				if (req == null)
					 continue;
				//System.Threading.Thread.Sleep(20000);
				ConnectionTrial(req);
								
			}


		}







        public void OnUserTryConnectToServer(object sender, ExecutedRoutedEventArgs e)
        {

            var UserConReq = (UserConReq)e.Parameter;
           
            //Communicator.OnUserTryConnectToServer(authReq);
            //_passwordSaver.OnConnectionTrial(authReq);
            ConnectionTrial(UserConReq);

        }


        public void CloseInstrumentPostions(object sender, ExecutedRoutedEventArgs e)
        {
            CDataCloseInstPos closeInstrPos = (CDataCloseInstPos)e.Parameter;

           
            CCloseAllPositionsByIsin closeByIsin = new CCloseAllPositionsByIsin { Isin = closeInstrPos.Instrument };
            enmTradingEvent ev = enmTradingEvent.CloseAllPositionsByIsin;
            Communicator.SendDataToServer(closeInstrPos.ConId, closeByIsin, ev);

        }


        public void CancellInstrumentOrders(object sender, ExecutedRoutedEventArgs e)
        {
            CDataCloseInstPos closeInstrPos = (CDataCloseInstPos)e.Parameter;

            CCancellOrderByIsin cancellAllOrders = new CCancellOrderByIsin { Isin = closeInstrPos.Instrument };
            enmTradingEvent ev = enmTradingEvent.CancellOrdersByIsin;
            Communicator.SendDataToServer(closeInstrPos.ConId, cancellAllOrders, ev);

        }

        public void CloseAllPositions(object sender, ExecutedRoutedEventArgs e)
        {
            CDataCloseAllPos dataCloseAllPos = (CDataCloseAllPos)e.Parameter;
            CCloseAllPositions closeAllPos = new CCloseAllPositions();
   
            List<int> lst =  ViewModelDispatcher.GetConIdLstWithOpenedPos();

            foreach (var conId in lst)
                Communicator.SendDataToServer(conId, closeAllPos, enmTradingEvent.CloseAllPositions);
        }




        public void CancellAllOrders(object sender, ExecutedRoutedEventArgs e)
        {
            CDataCancellAllOrders cancellAllOrders = (CDataCancellAllOrders)e.Parameter;
            CCancellAllOrders cao = new CCancellAllOrders();

            List<int> lst = ViewModelDispatcher.GetConIdWithOrders();

            foreach (var conId in lst)
                Communicator.SendDataToServer(conId, cao, enmTradingEvent.CancellAllOrders);
        }





        public void OnErrorMessageFromGUI(object sender, ExecutedRoutedEventArgs e)
        {
            CDataErrorMessage errMsg = (CDataErrorMessage)e.Parameter;

            Error(errMsg.Message, errMsg.Excptn);

        }


		public CAuthRequest GetAuthReqById(int id)
		{
			return _passwordSaver.GetUserAuthReq(id);

		}


        public void LoadColorList()
        {
            _colorList = new CColorList(needSelfInit: false);


            string path = CUtil.GetConfigDir() + @"\colorlist.xml";
            _colorList.FileName = path;
            CSerializator.Read(ref _colorList);


        }



        public bool IsUniqueProcess()
        {

            if (!CUtil.IsSingleProcWithPath())
            {


                string message = "Вы пытаетесь запустить второй экземпляр программы из одной и той же папки. Возможно после прошлого запуска программа не была  корректно завершена. " +
                                    "Вы желаете завершить \"старый\" экземпляр программы и запустить \"новый\" ?";

                
                AllertWindowYesNo win = new AllertWindowYesNo(message);
                win.ShowWindowOnCenter();

                //((Window)win).ShowDialog();

                if (win.YesClicked)
                {
                    //kill dup process with same names in same dir
                    CUtil.KillAllDupProcSamePath();
                    return true;
                }
                else//kill our proc and exit
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    return false;
                }
            }

            return true;
        }


        public static CKernelTerminal GetKernelTerminalInstance()
        {     

            return ((App)Application.Current).KernelTerminal;

        }

        public static CViewDispatcher GetViewDispatcherInstance()
        {

            return GetKernelTerminalInstance().ViewDispatcher;

        }

        public static CViewModelDispatcher GetViewModelDispatcherInstance()
        {

            return GetKernelTerminalInstance().ViewModelDispatcher;

        }



        public  void OnConnectionSuccess(int connId, CAuthResponse authResponse)
        {

			AuthSuccess(connId);

            //SubscribeTickersFromConfig(connId);
            //SavePassword(connId, authResponse.
                       
        }

        private void SavePassword(int connID, string pwd)
        {



        }


        /// <summary>
        /// When auth past success. Read data from config (StockVisualConf)
        /// and send request to server for instruments we need
        /// </summary>
        /// <param name="conID"></param>
        public void SubscribeTickersFromConfig(int conID)
        {

            lock (StockVisualConf)
            {
                var listStockVisConf = StockVisualConf.ListStocksVisual.FindAll(a => a.ConNumm == conID);

                CSubscribeTicker subscrTicker = new CSubscribeTicker();

                if (listStockVisConf != null)
                    listStockVisConf.ForEach(stockVisConf => 
                                    {
                                        if (stockVisConf.Ticker != null)
                                        {
                                            subscrTicker.ListSubscribeCommands.Add(
                                                                                new CCommandSubscribeTickers
                                                                                {
                                                                                    Ticker = stockVisConf.Ticker,
                                                                                    Action = EnmSubsrcibeActions.Subscribe
                                                                                }
                                                                                    );





                                            //assume Step is valid and was already saved in conf
                                            Communicator.GetDataReciever(conID).AddStructuresForSubscribedTicker(stockVisConf.Ticker, 
                                                                                                                (decimal)stockVisConf.Step);
                                        }
                                    });


                Communicator.SendSubscribeTickerList(conID, subscrTicker);
            }
        }



        public void SendTeminalInfoForDealingServer(int conID)
        {

            CClientInfo termInfo = new CClientInfo
            {
                Version = CUtil.GetVersion(),
                Instance = 1
            };

            Communicator.SendDataToServer(conID, termInfo, enmTradingEvent.ClientInfo);



        }




        public void ReReadSharedSettings()
        {
            //TODORefact using reflection
            // == SHARED SETTINGS == Step 3
            try
            {
               // _terminalConfig.StringHeight = StringHeight;
                //_terminalConfig.FontSize = FontSize;
            }
            catch (Exception e)
            {
                if (e != null)
                    System.Threading.Thread.Sleep(0);


            }

        }


        public static void SaveTerminalProperties()
        {
            CKernelTerminal kernTeminal = CKernelTerminal.GetKernelTerminalInstance();

            lock (kernTeminal.TerminalConfig)
            {
                CDataBinder.UpdateTerminalConfig(kernTeminal.TerminalConfig.TerminalProperties, CViewModelDispatcher.GetTerminalViewModel());


            }

            CUtil.TaskStart(kernTeminal.SaveTerminalSettings);

        }

        /// <summary>
        /// Saves terminalc config to file
        /// 
        /// Call from:
        /// 1. KernelTerminal.SaveWindowPosition (task called before)
        /// 2. SettingsTerminalWindow.SaveTerminalSettings (task)
        /// 
        /// </summary>
        public void SaveTerminalSettings()
        {

            double parSecsWriteBackup = 60;

            lock (TerminalConfig)
            {
              
                //GUIDispatcher.Invoke(new Action(()=>ReReadSharedSettings()));
                CSerializator.Write<CTerminalConfig>(ref _terminalConfig);

                //Write backup if time to do it
                if ((DateTime.Now - _dtLastSave).TotalSeconds > parSecsWriteBackup)
                {
                    CTerminalConfig terminalConfigBackup = _terminalConfig;

                    terminalConfigBackup = (CTerminalConfig) _terminalConfig.Copy();
                    terminalConfigBackup.FileName = CUtil.GetConfigBackupDir() + @"\terminal.xml";
                    CSerializator.Write<CTerminalConfig>(ref terminalConfigBackup);

                    _dtLastSave = DateTime.Now;

                }

                

            }
        }






        public void GetMainWindowPosition(ref CGeomWindow geomWindow)
        {

            lock (TerminalConfig)
            {
                string stTypeOfWindow = geomWindow.TypeOfWindow.ToString();
                var res = _terminalConfig.ListWindowSavedData.FirstOrDefault
                           (a => a.TypeOfWindow == stTypeOfWindow);


                if (res != null)
                {
                    geomWindow.Left = res.Left;
                    geomWindow.Top = res.Top;
                    geomWindow.Height = res.Height;
                    geomWindow.Width = res.Width;
                }

            }
        }



        /// <summary>
        /// 1. Updates config data for specific window
        /// 2. Call saving of terminal config
        /// 
        /// Called from BaseTerminalWindow.TaskSaveWindowData() (task)
        /// </summary>
        /// <param name="newGeomWindow"></param>

        public void SaveWindowPosition(CGeomWindow newGeomWindow )
        {
            lock (TerminalConfig)
            {
                                                                                
                var res = _terminalConfig.ListWindowSavedData.FirstOrDefault
                            (a => a.TypeOfWindow == newGeomWindow.TypeOfWindow.ToString());

                if (res == null)
                    _terminalConfig.ListWindowSavedData.Add(newGeomWindow);
                else
                {
                    res.Left = newGeomWindow.Left;
                    res.Top = newGeomWindow.Top;
                    res.Height = newGeomWindow.Height;
                    res.Width = newGeomWindow.Width;
                    res.IsOpened = newGeomWindow.IsOpened;

                }
               


           


            }
            SaveTerminalSettings();

        }


        public void ReadVisualStockConfig()
        {
            string pathToConfig = CUtil.GetConfigDir() + @"\StockVisual.xml";
            _stocksVisualConf = new CStocksVisualConf(pathToConfig, false);

            CSerializator.Read<CStocksVisualConf>(ref _stocksVisualConf);



        }



       
        



     



        public bool IsSynchronised { get; set; }
        //removed 2018-05-27
    //    public List<CUserPosMonitorUpdate> LstUserPosMonUpd { get => _lstLateUserPosMonUpd; set => _lstLateUserPosMonUpd = value; }
        public bool IsWaitTaskStarted { get => _isWaitTaskStarted; set => _isWaitTaskStarted = value; }

        long tmpCnt = 0;
        public void Synchronise(string stTime)
        {
            DateTime dtTm = Convert.ToDateTime(stTime);
            DateTime dtNow = DateTime.Now;
            double delta = (dtNow - dtTm).TotalMilliseconds;
            if (delta > 100 && tmpCnt >300)
            {
                System.Threading.Thread.Sleep(1);
            }

            tmpCnt++;

        }

        public void CreateAlarmer(IGUIDispatcherable guiDisp, ComboBox alarmCombobox, System.Collections.Specialized.NotifyCollectionChangedEventHandler evHand)
        {
            _alarmer = new CAlarmer(guiDisp);
            alarmCombobox.ItemsSource = (ObservableCollection<string>)_alarmer.AlarmList;
            _alarmer.AlarmList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(evHand);
            guiDisp.GUIDispatcher.UnhandledException += new DispatcherUnhandledExceptionEventHandler(_alarmer.GUIdispatcher_UnhandledException);
        }


        /// <summary>
        /// Using ListStocksVisual from config file create controllmarket
        /// </summary>
        /// <param name="mainWin"></param>
        /// <param name="OnStockAdded">Callback after adding controlMarket in MainWindow</param>
        public void AddAllStocksFromConfig(MainWindow mainWin, Action<int , string, ControlMarket, CStocksVisual> OnStockAdded)
        {           
            for (int stockNum = 0; stockNum < StockVisualConf.ListStocksVisual.Count; stockNum++)
            {
                  ControlMarket controlMarket = new ControlMarket(stockNum, mainWin);
                  OnStockAdded(stockNum, 
                                StockVisualConf.ListStocksVisual[stockNum].Ticker, 
                                controlMarket, 
                                StockVisualConf.ListStocksVisual[stockNum]);

                  AddOneStockFromConfig(stockNum, controlMarket, StockVisualConf.ListStocksVisual[stockNum]);


               
            }

        }

       
        /// <summary>
        /// Loads instrument config from config file. Using loaded parameters 
        /// call methods of ViewModelDispatcher and ViewDispatcher
        /// </summary>  
        public void AddOneStockFromConfig(int stockNum ,ControlMarket controlMarket, 
                        CStocksVisual stockVisual )
        {
           // CInstrumentConfig  instrConfig =   OpenInstrumentsConfig(tickerName);


            CInstrumentConfig instrumentConfig = LoadInstrumentConfig(stockVisual.Ticker);
            ViewModelDispatcher.AddMarketViewModelNew(stockNum,controlMarket,  stockVisual, instrumentConfig);
            ViewDispatcher.AddControlMarket(controlMarket, ViewModelDispatcher.GetMarketViewModel(/*tickerName*/ stockVisual.Ticker));

        }


        private double GetWidthOfAddedControlMarket(MainWindow mainWindow, int countExistingControlMarkets)
        {
            double widthNew = mainWindow.ActualWidth / countExistingControlMarkets;

            return widthNew;
        }






        public void UpdateStockVisualWidth(MainWindow mainWindow, int stockNum, 
                                            CStocksVisual stocksVisual)
        {
            double sumWidthStock = 0;
            double sumWithControlClusters = 0;
            stocksVisual.WidthStock = 50; //default minimum
            

            foreach (UIElement v in  mainWindow.GridMarket.Children)
            {

                if (v is ControlMarket)
                {
                    ControlMarket currCM = (ControlMarket)v;
                    foreach (var cd in currCM.GridControlMarket.ColumnDefinitions)
                    {
                        if (cd.Name == "ColControlStock")
                            sumWidthStock += cd.ActualWidth;
                        if (cd.Name == "ColControlClusters")
                            sumWithControlClusters += cd.ActualWidth;


                    }


                }

            }

            //Stock Cell
            if (stockNum != 0)
            {
                stocksVisual.WidthStock = Math.Max(stocksVisual.WidthStock, 
                                                    sumWidthStock / stockNum);

                stocksVisual.WidthClusters = Math.Max(stocksVisual.WidthClusters, 
                                                sumWithControlClusters / stockNum);
            }


            
        }





        /// <summary>
        /// Adds stock when button "Add stock" clicked.
        /// Creates new ControlMarket instance. 
        /// Note: Accepted contract below.
        /// Whesn stock creates it is become "rightest" on window form.
        /// And also become the latest elemnt of ListStocksVisual 
        /// </summary>
        /// <param name="mainWin"></param>
        /// <param name="OnStockAdded">Callback of MainWindow after ControllMarket created</param>
        /// <returns>StockNum - in fact number of element in ListStockVisual</returns>
        public int AddEmptyStockFromButton(MainWindow mainWin, Action<int, string, ControlMarket, CStocksVisual> OnStockAdded)
        {
            var lstStockVisual = StockVisualConf.ListStocksVisual;
            int cntStocks;

          



            CStocksVisual stockVisual = new CStocksVisual()
            {
                Ticker = Literals.Undefind,
                
            };


            UpdateStockVisualWidth(mainWin, 
                                   StockVisualConf.ListStocksVisual.Count, 
                                   stockVisual);


            lock (lstStockVisual)
            {
                lstStockVisual.Add(stockVisual);
                cntStocks = lstStockVisual.Count;
            }

            int stockNum = cntStocks - 1;

            ControlMarket controlMarket = new ControlMarket(stockNum, mainWin);
            OnStockAdded(stockNum, stockVisual.Ticker, controlMarket,null);

            
            
            mainWin.Width += mainWin.Width / cntStocks;


            AddEmptyStock(stockNum, controlMarket, stockVisual);

            SaveVisualConf();//2018-05-03
            
            return stockNum;

        }


     
        


		/// <summary>
		/// 1) Loads empty instrument config
		/// 2) Call ViewModelDispatcher.AddMarketViewModelNew (creates 
		/// and register MarketViewModel).
		/// 
		/// Note. ControlMarket was previously created in 
		/// AddEmptyStockFromButton
		/// 
		/// 
		/// Call from AddEmptyStockFromButton
		/// </summary>	
        public void AddEmptyStock(int stockNum,   ControlMarket controlMarket, CStocksVisual stockVisual)
        {
            CInstrumentConfig instrumentConfig = LoadInstrumentConfig(Literals.Undefind);
          
            ViewModelDispatcher.AddMarketViewModelNew(stockNum, controlMarket,  stockVisual, instrumentConfig);
            MarketViewModel mvm = ViewModelDispatcher.GetMarketViewModel(stockNum);
            ViewDispatcher.AddControlMarket(controlMarket, mvm);

        }



       /// <summary>
	   /// 1) Unsubscribe ticker if subscribed
	   /// 2) Call method MainWindow.DeleteStock
	   /// to remove ControlMarket from MainWindow
	   /// 3) Call delete from ViewModelDispatcher
	   /// 4) Call delete from ViewDispacher
	   /// 5) Call shift StockNumbers
	   /// 6) Remove from StockVisualConfl
	   /// 7) Save VisualConf
	   /// 
       /// Call from MarketViewModel.DeleteInstrument
       /// </summary>  
        public void DeleteExistingStock(int stockNum, string ticker)
        {

            //Unsubscribe from ticker if subscribed
            if (ticker != Literals.Undefind)
            {
                int conid = ViewModelDispatcher.GetConnectionIdOfMarketViewModel(ticker);

                if (Communicator.IsConnected(conid))
                    Communicator.SendUnsubscribeOneTicker(conid, ticker);

            }

            //remove from main window graphical stock
            MainWindow mw = (MainWindow) CUtilWin.FindWindow<MainWindow>();
            mw.DeleteStock(stockNum);
           // ViewModelDispatcher.ShiftMarketViewModelStockNumber(stockNum);
            

            ViewModelDispatcher.DeleteMarketViewModel(ticker);
            ViewDispatcher.DeleteControlMarket(stockNum);
            ViewDispatcher.ShiftStockNumber(stockNum);

            StockVisualConf.ListStocksVisual.RemoveAt(stockNum);

            SaveVisualConf();
        }



        /// <summary>
        /// Call from 
        ///           1) DeleteExistingStock
        ///           2) ChangeMarketInstrument
        ///           3) AddEmptyStockFromButton
        ///           4) UpdateStockVisualInstrumentParams
        ///           5) StockWidthChanged
        /// </summary>
        public void SaveVisualConf()
        {
            (new Task(TaskSaveVisualConf)).Start();

        }


        public void TaskSaveVisualConf()
        {
            try
            {

                lock (StockVisualConf)
                    CSerializator.Write<CStocksVisualConf>(ref _stocksVisualConf);

             
            }
            catch (Exception e)
            {
                Error("CKernalTerminal.TaskWriteVisualConf",e);

            }


        }

        public static string GetInstruemntPath(string instrument)
        {
            return CUtil.GetConfigDir() + "\\instruments\\" + instrument + ".xml";                         
        }



        public CInstrumentConfig LoadInstrumentConfig(string instrument)
        {

        
            string pathToConfig = GetInstruemntPath(instrument);

            //2017-08-17 fix "No parameters stock problem"
            //if config file exist load instrument from config
            //else load undefined.xml. Market data (decimals etc)
            //will fill later.
            
            CInstrumentConfig instrumentConfig;
            if (File.Exists(pathToConfig))
            {
            

                instrumentConfig = new CInstrumentConfig(instrument, pathToConfig);
                CSerializator.Read<CInstrumentConfig>(ref instrumentConfig);
            }
            else
            {

                string undefPath = GetInstruemntPath(Literals.Undefind);

                instrumentConfig = new CInstrumentConfig(instrument, undefPath);
                CSerializator.Read<CInstrumentConfig>(ref instrumentConfig);
                instrumentConfig.FileName = pathToConfig;
                instrumentConfig.MarketProperties.StockProperties.TickerName = instrument;
                instrumentConfig.MarketProperties.DealsProperties.TickerName = instrument;
                instrumentConfig.MarketProperties.ClusterProperties.TickerName = instrument;
               CSerializator.Write(ref instrumentConfig);


            }

            return instrumentConfig;
        
        }



        public void SaveInstrumentConfig(ref CInstrumentConfig instrumentConfig)
        {
          CSerializator.Write<CInstrumentConfig>(ref instrumentConfig);
        }




        //2018-02-25 KAA for encapsulate work with stockVisual
        /// <summary>
        /// Call from:
        ///        1) UpdateInstrParamsOnConnection
        ///        2) UpdateInstrumentParamsOnline
        /// </summary>
        /// <param name="stockNum"></param>
        /// <param name="sv"></param>
        public void UpdateStockVisualInstrumentParams(int stockNum, CStocksVisual sv)
		{
            //2018-05-02 protect of overriding width etc

            //StockVisualConf.ListStocksVisual[stockNum] = sv;
            lock (StockVisualConf)
            {
                StockVisualConf.ListStocksVisual[stockNum].ConNumm = sv.ConNumm;
                StockVisualConf.ListStocksVisual[stockNum].Decimals = sv.Decimals;
                StockVisualConf.ListStocksVisual[stockNum].Step = sv.Step;
                StockVisualConf.ListStocksVisual[stockNum].Ticker = sv.Ticker;
            }

            SaveVisualConf();

		}


        /// <summary>
        /// Case when ControlMarket is allready exists. 
        /// 1)Replace existing control market with new one on window.
        /// 2)Replace market view model.
        /// 3)Replace structures elements from ViewmMdelDispactcher and 
        /// ViewDispatcher. 
        /// 
        /// Calling from ChangeMarketInstrument
        /// </summary>       
        public void EditConnectedStock(int stockNum, int newConId, CTIckerData newTickerData)
        {
                
            //remove on main window graphical stock with new one
            MainWindow mainWindow = (MainWindow)CUtilWin.FindWindow<MainWindow>();
            ControlMarket controlMarket =  mainWindow.ReplaceControlMarket(stockNum);
           

            string newTicker = newTickerData.TickerName;
            //add ticker as subscribed
            Communicator.GetDataReciever(newConId).AddStructuresForSubscribedTicker(newTicker, newTickerData.Step);


            string oldTicker = StockVisualConf.ListStocksVisual[stockNum].Ticker;


            //create stock visual
            CStocksVisual stockVisual = new CStocksVisual
                {
                    Ticker = newTicker,
                    ConNumm = newConId,
                    Decimals = newTickerData.Decimals,
                    Step = (double)newTickerData.Step
                };

          
            //add it to config
            StockVisualConf.ListStocksVisual[stockNum] = stockVisual;

            //load instrument config
            CInstrumentConfig instumentConfig = LoadInstrumentConfig(stockVisual.Ticker);

            //replace market view model
            ViewModelDispatcher.ReplaceMarketViewModel(stockNum, oldTicker, stockVisual, controlMarket, instumentConfig);
            
           //get previously created ViewModel
            MarketViewModel marketViewModel = ViewModelDispatcher.GetMarketViewModel(newTicker);
           //note: could be that CntrolMarket exists but not connected to server
            marketViewModel.IsConnectedToServer = true; 


            //replace view
            ViewDispatcher.ReplaceControlMarket(stockNum, ref controlMarket, marketViewModel);
            //2017-06-02
            //Bring back parameters instrumant paramers (step, conNum etc) that where previously overided
            //by ReplacaControlMarket(during binding)
            marketViewModel.SetInstrumentParams(stockVisual);
            //2018-04-30 bring back decimal volume too
            marketViewModel.DecimalVolume = newTickerData.DecimalVolume;
               

            ViewDispatcher.UpdateZIndexes();


            //subscribe new tick
            CSubscribeTicker subscrTick = new CSubscribeTicker();
            subscrTick.ListSubscribeCommands.Add(new CCommandSubscribeTickers 
             {
                 Ticker = newTicker,
                 Action = EnmSubsrcibeActions.Subscribe
              }
                                        );

            //unsubscribe old tick
            subscrTick.ListSubscribeCommands.Add(new CCommandSubscribeTickers
            {
                Ticker = oldTicker,
                Action = EnmSubsrcibeActions.UnSubscribe
            }
                                       );


            //update subscribed ticker lis send - it to server
            Communicator.SendSubscribeTickerList(newConId, subscrTick);

          //  SaveVisualConf();

        }

        /// <summary>
        /// Edit stock.  ViewModel of CntrolMarket is "undefined" at this point. That case is after added new stock. 
        /// On that case  ViewModel is empty ("dummy"), not connected, no data receieving. So here we: 
        ///  
		/// 1) Insert instrument data ensteed of "dummy" data
		/// 2) Set as Connected to server
		/// 2) Register in ViewModelDispatcher
		/// 4) Trigger send subcribe message and set as subscribed
		/// 
        /// Calling from ChangeMarketInstrument     
        public void EditNonConnectedStock(int stockNum, int conNum, CTIckerData newTickerData, MarketViewModel marketViewModel)
        {

			
            // edit instrument parameters for empty viewmodel
            marketViewModel.SetEmptyVewModelInstrParams(conNum, newTickerData);
			//note: we can set instrument when only if we connected to server
			marketViewModel.IsConnectedToServer = true;
			

            //add existing view model
            ViewModelDispatcher.MakeEmptyViewModelNonEmpty(newTickerData.TickerName, marketViewModel);
          

          


            var dataReciever = Communicator.GetDataReciever(conNum);

            //add ticker as "subscribed"
            dataReciever.AddStructuresForSubscribedTicker(newTickerData.TickerName, newTickerData.Step);
            //subscribe new ticker
            Communicator.SendSubscribeOneTicker(conNum, newTickerData.TickerName);

            /* StockVisualConf.ListStocksVisual[stockNum] = new CStocksVisual
             {
                 ConNumm = conNum,
                 Ticker = newTickerData.TickerName,
                 Decimals = newTickerData.Decimals,
                 Step = (double)newTickerData.Step
             };
             */
            //changed 2018-05-03
            lock (StockVisualConf)
            {
                StockVisualConf.ListStocksVisual[stockNum].ConNumm = conNum;
                StockVisualConf.ListStocksVisual[stockNum].Ticker = newTickerData.TickerName;
                StockVisualConf.ListStocksVisual[stockNum].Decimals = newTickerData.Decimals;
                StockVisualConf.ListStocksVisual[stockNum].Step = (double)newTickerData.Step;
            }




        }





        /// <summary>
        /// Change all data when user selected another instrument.
        /// Create new ControlMarket, MArketViewModel, make binding etc.
        /// 
        /// Call from ViewModelAvailableTickers of window AvailalableTickersWindow.
        /// 
        /// </summary>
        /// <param name="conNum"></param>
        /// <param name="tickerData">Parameters of instrument coming from GUI Datagrid's selected item.</param>
        /// <param name="currMarketViewModel">Attached MarketViewModel of ControlMarket, from which user 
        /// edit instrument. Uses it calling methods  methods</param>  
        public void ChangeMarketInstrument(int conNum,  CTIckerData tickerData, MarketViewModel currMarketViewModel)
        {


            int currStockNum = currMarketViewModel.ControlMarket.StockNum;
            string tickerName = tickerData.TickerName;


            /*
             * Two cases:  
             * 1) ViewModel of CntrolMarket is "undefined". That case is after added new stock. 
             *    On that case  ViewModel is empty ("dummy"), not connected, no data receieving. 
             *    On that case - set  ViewModel parameters, replace new ControlMarket than                  
             *     subscribe  instrument on server.
             *    
             * 2) ViewModel of CntrolMarket is connected to Server, so we need to disconnect
             *    from server, change ViewModel parameters, replace new ControlMarket, unsubscribe 
             *    "old" instrument, subscribe  "new" instrument
             */
            if (currMarketViewModel.TickerName == Literals.Undefind)//"empty" view model 
                EditNonConnectedStock(currStockNum, conNum,
                                                      tickerData, currMarketViewModel);
            else
                EditConnectedStock(currStockNum, conNum, tickerData);//" not empty" view model

            MainWindow mw = (MainWindow)CUtilWin.FindWindow<MainWindow>();
			mw.CheckAddButtonVisibility();
           

            //save changed config
            SaveVisualConf();



        }



        public void ConnectToServer()
        {
            //TO DO normal
            //Communicator.ConnectToServer(0);

        }

      /*  public void AuthRequest(int conId)
        {

            CAuthRequest ar = new CAuthRequest { User =  _terminalConfig.User, Password   = _terminalConfig.Password };
            enmTradingEvent ev = enmTradingEvent.AuthRequest;
            Communicator.SendDataToServer(conId,ar,ev);
            
        }
        */
        public void UpdateUserOrders(CUserOrdersUpdate  userOrdersUpdate)
        {
            if (userOrdersUpdate.MonitorOrders != null)
            {
                foreach (var v in userOrdersUpdate.MonitorOrders)
                {
                    string isin = v.Key;
                  
                    MarketViewModel mvm = ViewModelDispatcher.GetMarketViewModel(isin);
					if (mvm != null)
					{
										
							mvm.UpdateUsersOrders(v.Value);
							mvm.RepaintDeals();//for redraw orders

					}
                }
            }
          
        }

        //2018-05-27
        private Dictionary<string, CUserPos> _savedUserPosMonUpd = new Dictionary<string, CUserPos>();
        bool _isWaitTaskStarted = false;


        /// <summary>
        /// Call from:
        /// 
        /// DataReciever.ProcessUserUpdatePositionMonitor
        /// </summary>
        /// <param name="userPosUpdate"></param>
        public void UpdateUserMonitorPos(CUserPosMonitorUpdate userPosUpdate)
        {
            bool bSavedUserPos = false;

            if (userPosUpdate.MonitorUserPos != null)
            {
               

                foreach (var v in userPosUpdate.MonitorUserPos)
                {
                    string isin = v.Key;
                    MarketViewModel mvm = ViewModelDispatcher.GetMarketViewModel(isin);
                    if (mvm != null)
                        mvm.UpdateUserPos(v.Value);
                    else
                    {
                        //2018-05-27 special logics for update posmon
                        //if mvm is not loaded on start yet
                        //do add to list
                        lock (_savedUserPosMonUpd)
                        {
                            if (v.Value.Amount != 0)
                            {
                                _savedUserPosMonUpd[isin] = v.Value;
                                bSavedUserPos = true;
                            }

                        }
                    }
                }

               if (bSavedUserPos)
                    if (!_isWaitTaskStarted)
                    {
                         lock (_savedUserPosMonUpd)
                        {   //found not updated element
                            if (_savedUserPosMonUpd.Count>0)
                            {
                                _isWaitTaskStarted = true;
                                CUtil.TaskStart(TaskLateUpdPosMon);
                            // (new Task(TaskLateUpdPosMon)).Start();

                            }


                        }


                }
                

            }
        }


        private void TaskLateUpdPosMon()
        {
            DateTime dtTaskStarted = DateTime.Now;
            int parSecSinceSatrted = 30;


            List<string> lstInstrToRemove = new List<string>();

            while((DateTime.Now - dtTaskStarted).TotalSeconds <parSecSinceSatrted )
            {
                lock (_savedUserPosMonUpd)
                {
                    if (_savedUserPosMonUpd.Count > 0)
                    {
                       foreach (var el in _savedUserPosMonUpd)
                        {
                            
                            MarketViewModel mvm = ViewModelDispatcher.GetMarketViewModel(el.Key);
                            if (mvm != null)
                            {
                                mvm.UpdateUserPos(el.Value);
                                lstInstrToRemove.Add(el.Key);
                            }
                        }

                       foreach(var instr in lstInstrToRemove)                       
                            _savedUserPosMonUpd.Remove(instr);

                        lstInstrToRemove.Clear();


                    
                    }

                }

                System.Threading.Thread.Sleep(100);


            }
     


        }





		public void AcceptStopLossTakeProfit(CSetOrder stopLossTakeProfit)
		{

			string instrument = stopLossTakeProfit.Instrument;

			MarketViewModel mvvm = ViewModelDispatcher.GetMarketViewModel(instrument);
			if (mvvm != null)
				mvvm.AcceptStopLossTakeProfit(stopLossTakeProfit);


		}



        /// <summary>
        /// Call when instrument parameter changed (for example Decimals)
        /// during trading.
        /// 
        /// Call from CDataReciever.ProcessUpdInstrParamsOnilne
        /// 
        /// </summary>
        /// <param name="updInstrParams"></param>
		public void UpdateInstrParamsOnilne(CUpdateInstrumentParams updInstrParams)
		{
			   MarketViewModel mvm = ViewModelDispatcher.GetMarketViewModel(updInstrParams.Instrument);

			   if (mvm != null)			   
				   mvm.UpdateInstrumentParamsOnline(updInstrParams);
			  
		}

        /// <summary>
        /// Call when connection with server esteblished, auth past and 
        /// server sent ticker data
        /// 
        /// Call from  CDataReciever.ProcessUserUpdateAvailableTickers
        /// 
        /// </summary>
        /// <param name="avTickers"></param>
		public void UpdateInstrParamsOnConnection(CAvailableTickers avTickers)
		{

			avTickers.ListAvailableTickers.ForEach(tickData =>
			{

				MarketViewModel mvm = ViewModelDispatcher.GetMarketViewModel(tickData.TickerName);

				if (mvm != null)
					mvm.UpdateInstrParamsOnConnection(tickData);

			});


		}


		


    
        public void UpdateUserDealsLog(CUserDealsLogUpdate userDealsLog)
        {
            ViewModelDispatcher.UpdateUserDealsLog(userDealsLog);


        }
      


        public void Error(string msg,Exception e=null)
        {
            if (_alarmer !=null)
                _alarmer.Error(msg, e);
        }

        public static void ErrorStatic(string msg, Exception e = null)
        {

            GetKernelTerminalInstance().Error(msg,e);

        }

        /// <summary>
        /// If window already opened activates it.
        /// If not than request ViewModelDispathcer create and activate it.
        /// 
        /// Call from 
        /// 
        /// ControlMarket.ButtonConnection_PreviewMouseDown
        /// WindowMenu.OpenChildWindow (openes trading result windows)
        /// </summary>    
		public static Window OpenChildWindow<TChildWindow>() where TChildWindow : Window
        {


            Window win = CUtilWin.FindWindowOrNull<TChildWindow>();
            //changed 2018-03-28
            try
            {
                if (win == null)
                {

                    win = CKernelTerminal.GetViewModelDispatcherInstance().
                                         OpenChildWindow<TChildWindow>();


                }
                else
                {
                    win.Activate();

                }
            }
            catch (Exception e)
            {
                ErrorStatic("CKernelTerminal.OpenChildWindow Type=" + typeof(TChildWindow).ToString(),
                             e);
            }


			return win;
        }

        public void OnClose()
        {

            ViewModelDispatcher.OnClose();

        }

    }
}
