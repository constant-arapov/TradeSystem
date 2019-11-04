using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using System.Threading;

using System.ComponentModel;

using System.Threading.Tasks;

using System.Diagnostics;
using System.Windows.Threading;
using System.Collections.ObjectModel;


using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;

using System.Text.RegularExpressions;

//using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Controls;
using System.Windows.Input;



using Common;
using Common.Interfaces;
using Common.Collections;
using Common.Utils;


using TradingLib;
using TradingLib.Bots;

using GUIComponents;
using GUIComponents.Controls;
using GUIComponents.ViewModel;

using CryptoDealingServer;


using TradeSystemCrypto;


namespace TradeSystemCrypto.ViewModel
{
    public class MainWindowViewModel : CBaseProppertyChanged, IAlarmable
    {

        CCryptoDealingServer _dealingServer;


        public CCryptoDealingServer DealingServer
        {
            get
            {
                return _dealingServer;
            }

        }
        MainWindow m_mainWindow;


        //public CPlaza2Connector Plaza2Connector { set; get; }

        Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

        SettingsTableViewModel _vmSTab;


        CGUIConfig m_GUICOnfig;





        public bool Enabled
        {
            get;

            set;

        }


        public MainWindowViewModel()
        {
            try
            {


                m_mainWindow = (MainWindow)App.Current.Windows[0];

                string path = CUtil.GetConfigDir() + "\\GUIConfig.xml";

                m_GUICOnfig = new CGUIConfig(path);
                CSerializator.Read<CGUIConfig>(ref m_GUICOnfig);


                if (m_mainWindow != null)
                    if (!DesignerProperties.GetIsInDesignMode(m_mainWindow))
                        (new Thread(ThreadFunc)).Start();



                //TODO normal, command, etc...

                m_mainWindow.Closed += new EventHandler(m_mainWindow_Closed);


            }
            catch (Exception e)
            {

                Error(" MainWindowViewModel. ", e);

            }


        }

        void m_mainWindow_Closed(object sender, EventArgs e)
        {
            //_dealingServer.Disconnect();

            //_dealingServer.WaitConnectorDisconnected();


            Process.GetCurrentProcess().Kill();

        }



        public void Error(string description, Exception exception = null)
        {
            _dealingServer.Error(description, exception);

        }
      


        private bool _firstTime = true;
        private void AlarmList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            
            if (m_mainWindow.Alarm.AlarmComboBox.Items.Count != 0)
            {
                m_mainWindow.Alarm.AlarmComboBox.SelectedIndex = 0;


                m_mainWindow.Alarm.AlarmComboBox.Background = System.Windows.Media.Brushes.Red;



                m_mainWindow.Alarm.AlarmComboBox.Foreground = System.Windows.Media.Brushes.White;


                if (_firstTime)
                {
                    m_mainWindow.Alarm.AlarmComboBox.Resources.Add(System.Windows.SystemColors.WindowBrushKey, Brushes.Red);
                    m_mainWindow.Alarm.AlarmComboBox.Resources.Add(System.Windows.SystemColors.HighlightBrushKey, Brushes.Red);
                    _firstTime = false;
                }

            }
            
        }

        private void AlarmComboBox_DropDownOpened(object sender, EventArgs e)
        {

            // m_mainWindow.Alarm.AlarmComboBox.Background = System.Windows.Media.Brushes.Red;

        }


        private void BindAlarm()
        {
            
            m_mainWindow.Alarm.AlarmComboBox.ItemsSource =
                            (ObservableCollection<string>)_dealingServer.Alarmer.AlarmList;

            _dealingServer.Alarmer.AlarmList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AlarmList_CollectionChanged);

            m_mainWindow.Alarm.AlarmComboBox.DropDownOpened += new EventHandler(AlarmComboBox_DropDownOpened);
            
        }




        private void TaskBindAlarm()
        {
            while (true)
            {
                if (_dealingServer.Alarmer != null && _dealingServer.Alarmer.AlarmList != null)
                {

                    try
                    {
                        _dispatcher.Invoke(new Action(() => BindAlarm()));
                    }
                    catch (Exception e)
                    {

                        string st = e.Message;
                    }

                    return;
                }
                System.Threading.Thread.Sleep(100);
            }
        }




        private void OnBotLoad(ControlBotGUI botGUI, long botId)
        {

            CBotBase bot = DealingServer.GetBotById(botId);
            if (bot != null)
                BindBotControls(botGUI, bot);



        }

        private void BindBotControls(ControlBotGUI botGUI, CBotBase bot)
        {


            bot.GUIBot.DisposeGUIBotEvent += botGUI.OnDisposeGUIBot;
            botGUI.GUIBot = bot.GUIBot;

            botGUI.DataContext = bot;
            botGUI.EllipseEnabled.DataContext = bot;
            botGUI.TextEnabled.DataContext = bot;

            //botGUI.EllipseReady.DataContext = bot.GUIBot;
            botGUI.EllipseReady.DataContext = bot;
            botGUI.TextReady.DataContext = bot;

            botGUI.ButtonEnableBot.DataContext = bot;
            botGUI.ButtonDisableBot.DataContext = bot;
            botGUI.ButtonManualControl.DataContext = bot;

            botGUI.MonitorPos.ItemsSource = bot.GUIBot.MonitorPos;

            botGUI.DealingServer = DealingServer;
            bot.GUIBot.MonitorPos.CollectionChanged += botGUI.OnMonitorPosCollectionChanged;

            botGUI.Orders.ItemsSource = bot.GUIBot.Orders;
            bot.GUIBot.Orders.CollectionChanged += botGUI.OnMonitorOrdersCollectionChanged;

            bot.GUIBot.DisposeGUIBotEvent += botGUI.OnDisposeGUIBot;


            botGUI.ButtonPosLog.Id = bot.BotId;
            botGUI.ButtonPosLog.BotOperations = DealingServer;

            botGUI.ButtonDisableBot.Id = bot.BotId;
            botGUI.ButtonDisableBot.BotOperations = DealingServer;

            botGUI.ButtonEnableBot.Id = bot.BotId;
            botGUI.ButtonEnableBot.BotOperations = DealingServer;

            botGUI.ButtonLoadBot.Id = bot.BotId;
            botGUI.ButtonLoadBot.BotOperations = DealingServer;

            botGUI.ButtonManualControl.Id = bot.BotId;
            botGUI.ButtonManualControl.BotOperations = DealingServer;





            BindBotButtons(ref botGUI.ButtonPosLog, bot);
            BindBotButtons(ref botGUI.ButtonDisableBot, bot);
            BindBotButtons(ref botGUI.ButtonEnableBot, bot);
            BindBotButtons(ref botGUI.ButtonUnloadBot, bot);




            _vmSTab = new SettingsTableViewModel(bot, botGUI);


            CUtil.SetBinding(bot.GUIBot, "BotState", botGUI.BotSate, ControlSettingsDataBlock.SettingValueTextProperty);
            botGUI.EvntLoadBot += OnBotLoad;



        }

        private void BindBotButtons(ref ButtonBot butBot, CBotBase bot)
        {

            butBot.Id = bot.BotId;
            butBot.BotOperations = DealingServer;



        }



        private void TaskBindClock()
        {
            try
            {
                while (!_dealingServer.IsServerTimeAvailable)
                    Thread.Sleep(100);


                _dispatcher.BeginInvoke(new Action(() =>
                   CUtil.SetBinding(_dealingServer.GUIBox, "ServerTime", m_mainWindow.ClockBox.TextWin.TextBlockLabel,
                                    TextBlock.TextProperty, "HH:mm:ss")
                   ));

               
            }
            catch (Exception e)
            {
                Error("TaskBindClock", e);

            }


        }
       

        private void CreateControlBotGUIs()
        {
            //TO DO generate it dinamically

            try
            {
                BindBotControls(m_mainWindow.Bot1, DealingServer.GetBotById(m_GUICOnfig.ListPositionId[0]));
                BindBotControls(m_mainWindow.Bot2, DealingServer.GetBotById(m_GUICOnfig.ListPositionId[1]));
                BindBotControls(m_mainWindow.Bot3, DealingServer.GetBotById(m_GUICOnfig.ListPositionId[2]));
                BindBotControls(m_mainWindow.Bot4, DealingServer.GetBotById(m_GUICOnfig.ListPositionId[3]));
                

         
            }
            catch (Exception e)
            {
                Error("CreateControlBotGUIs", e);

            }



        }




        private void BindData()
        {

            try
            {

                const int TRIAL_COUNT = 2000;
                const int SLEEP_DUR = 100;

                const int BOTS_COUNT = 3;

                //Plaza2Connector

                int i;
                for (i = 0; i < TRIAL_COUNT; i++)
                {
                    //TO DO number of bots
                    if (_dealingServer != null &&
                        _dealingServer.GUIBox != null &&
                        _dealingServer.IsAllBotLoaded)

                        break;

                    Thread.Sleep(SLEEP_DUR);


                }
                if (i == TRIAL_COUNT)
                {
                    if (_dealingServer == null || _dealingServer.Alarmer == null)
                        Debug.Assert(false, "TaskBindData Timeout !");
                    else
                        Error("TaskBindData Timeout !");

                }


                _dealingServer.GUIBox.GUIDispatcher = _dispatcher;

                _dispatcher.UnhandledException += new DispatcherUnhandledExceptionEventHandler(_dealingServer.Alarmer.GUIdispatcher_UnhandledException);



                (new Task(TaskBindAlarm)).Start();
                (new Task(TaskBindClock)).Start();
                

                CreateControlBotGUIs();

                CreateCommandBindings();

               

                m_mainWindow.GridData.DealingServer = _dealingServer;
                m_mainWindow.GridData.InteractDataFormatProvider = _dealingServer;
                m_mainWindow.GridData.GUIDispatcher = _dispatcher;

                //remod 2018-05-19
                //m_mainWindow.ButtonDataBaseStatus.AutoBind((object)_dealingServer.GUIBox);


                m_mainWindow.SessionTable.DataContext = _dealingServer.GUIBox;
             

            }
            catch (Exception e)
            {
                Debug.Assert(false, "TaskBindData Error !" + e.Message);
            }

        }
     


        private void ThreadFunc()
        {
            //Plaza2Connector = new CPlaza2Connector();
            _dealingServer = new CCryptoDealingServer();
          
            _dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(BindData));

            _dealingServer.Process();

            Thread.Sleep(100000);
            // Plaza2Connector.Process();




        }

      

     

        private void CreateCommandBindings()
        {
            // <=== add new commands here
            //BindCommandAndEvent(CommandsMainWindowViewModel.CmdPasswordChange, OnPasswordChange, DefaultCanDo);


        }

        private void BindCommandAndEvent(RoutedUICommand command, ExecutedRoutedEventHandler execute,
                                            CanExecuteRoutedEventHandler canExecute)
        {
            var binding = new CommandBinding(command,
                                             execute,
                                           canExecute
                                             );



            // Register CommandBinding for all windows.
            CommandManager.RegisterClassCommandBinding(typeof(Window), binding);


        }

        private static void DefaultCanDo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;



        }









    }
}
