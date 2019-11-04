using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using System.Threading;
using System.Diagnostics;


using Common;
using Common.Interfaces;
using Common.Utils;



using TradeManager.Interfaces.Clients;
using TradeManager.DataSource;
using TradeManager.Commands;
using TradeManager.Views;

namespace TradeManager
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	/// 
	public partial class App : Application,  IClientDataSource	, IGUIDispatcherable
	{
       
        //====================== PUBLIC PROPERTIES ==========================================
        public CDataSource DataSource
        {
            get
            {
                return _dataSource;
            }
        }


		public  System.Windows.Threading.Dispatcher GUIDispatcher { set; get; }
		//====================== END PUBLIC PROPERTIES ==========================================


		//===================== PRIVATE FIELDS   =================================================
		private CDataSource _dataSource;		
		private CCommandProcessor _commandProcessor;
        private CAlarmer _alarmer;
        private CTradeManagerConfig _tradeManagerConf;

		private MainWindow _mainWindow;


		//======================END PRIVATE FIELDS =================================================


		public static bool IsRunning;

        //now just for validation
  

        private Dictionary<int, string> _exchName = new Dictionary<int, string>()
        {
           {1, "FORTS"},
           {2, "SPOT"},
           {3, "CURR"},
           {4, "BFX"}
        };
        

        public CTradeManagerConfig TradeManagerConfig
        {
            get
            {
              return  _tradeManagerConf;
            }
        }

		public App()
		{
			IsRunning = true;
            LoadConfig();
			_dataSource = new CDataSource(this, 
                                            System.Windows.Threading.Dispatcher.CurrentDispatcher,
                                            _tradeManagerConf.IP_DB, _tradeManagerConf.Port_DB, 
                                            _tradeManagerConf.LstStockExhId,
                                             _tradeManagerConf.LstModelStockExchState,
											 _tradeManagerConf.LstModelDBCon);

			_commandProcessor = new CCommandProcessor(_dataSource);

			GUIDispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
			_alarmer = new CAlarmer(this);
			CUtil.TaskStart(TaskBindAlarm);
		
	

		}

        public string GetStockExchName(int stockExchId)
        {
            if (!_exchName.ContainsKey(stockExchId))
                throw new ApplicationException("App.GetStockExchName unknown stockExch");
            
            return _exchName[stockExchId];

        }


		public void OnClose()
		{
			IsRunning = false;
			GUIDispatcher.BeginInvokeShutdown(System.Windows.Threading.DispatcherPriority.Normal);
			//GUIDispatcher.InvokeShutdown();
			//while (!GUIDispatcher.HasShutdownFinished)
				//Thread.Sleep(100);

			Thread.Sleep(1000);//wait till all finishable threads ends
			Application.Current.Shutdown();
			Thread.Sleep(1000);
			Process.GetCurrentProcess().Kill();
		
		}

		public static void ErrorStatic(string message, Exception e = null)
		{
			App instance = (App)Application.Current;
			instance.Error(message, e);

		}

		public void Error(string description, Exception exception = null)
		{
			_alarmer.Error(description, exception);

		}





		public void OnMainWindowLoaded(MainWindow mainWin)
		{
			_mainWindow = mainWin;

			_mainWindow.BindDataSource();

			CUtil.TaskStart(TaskConnectToDatabase);
			CUtil.TaskStart(TaskConnectToDealingServers);
		}


		private void TaskConnectToDatabase()
		{
			try
			{
				//TODO if not connected ????????????
				_dataSource.ConnectToDatabase();

				GUIDispatcher.Invoke(new Action(() =>
										_mainWindow.BindDBDataSource()));
			}
			catch (Exception e)
			{
				Error("App.TaskConnectToDatabase",e);
			}

		}



		private void TaskConnectToDealingServers()
		{
			try
			{

				GUIDispatcher.Invoke(new Action(() =>
									_mainWindow.BindTCPDataSource()));

				_dataSource.ConnectToDealingServers();

			
			

			}

			catch (Exception e)

			{
				Error("App.TaskConnectToDealingServers",e);
			}

		
		}
       
     

        private void LoadConfig()
        {
		   /*string path = CUtil.GetConfigDir() + @"\TradeManagerConfig.xml";

			
			_tradeManagerConf = new CTradeManagerConfig() { NeedSelfInit = true, IsValid = true, FileName = path};
		
			CSerializator.Read<CTradeManagerConfig>(ref _tradeManagerConf);
			*/
            CUtil.ReadConfig<CTradeManagerConfig>(ref _tradeManagerConf, "TradeManagerConfig");
     
     
           
        }

		private void TaskBindAlarm()
		{
			while (IsRunning)
			{
				if (_alarmer !=null && _alarmer.AlarmList !=null)
				{
					GUIDispatcher.Invoke(new Action(() => BindAlarm()));
					return;
				}


				Thread.Sleep(100);
			}

		}


		private void BindAlarm()
		{
			MainWindow mainWindows = (MainWindow)CUtilWin.FindWindowOrNull<MainWindow>();
			if (mainWindows == null || mainWindows.ComboBoxAlarm == null )
				return;

			mainWindows.ComboBoxAlarm.ItemsSource = _alarmer.AlarmList;
			_alarmer.AlarmList.CollectionChanged += 
				new NotifyCollectionChangedEventHandler(mainWindows.AlarmList_CollectionChanged);


		}



        public  void SaveConfig()
        {
            CSerializator.Write<CTradeManagerConfig>(ref _tradeManagerConf);
        }


        public void FatalError(string message)
        {
            GUIDispatcher.Invoke(new Action(() =>
                            {
                             
                                AllertWindow win = new AllertWindow(message);
                                win.ShowWindowOnCenter();
                             //  
                            }
                            ));

         
        }



    }

	


}
