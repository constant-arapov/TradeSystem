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

using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Controls;


using Common;
using Common.Interfaces;
using Common.Collections;
using Common.Utils;

using GUIComponents;
using GUIComponents.Controls;
using TradingLib;
using TradingLib.Bots;

using GUIComponents.ViewModel;

using Plaza2Connector;

using TradeSystem.View;


namespace TradeSystem
{
    public class MainWindowViewModel : CBaseProppertyChanged, IAlarmable
    {


        MainWindow m_mainWindow;


        public CPlaza2Connector Plaza2Connector { set; get; }
        Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

        SettingsTableViewModel _vmSTab;


        CGUIConfig m_GUICOnfig;

        Dictionary<string, DateTimeIntervalType> dictIntervalTypes = new Dictionary<string, DateTimeIntervalType>()
                {
                    {"M1",  DateTimeIntervalType.Minutes},
                    {"M5",  DateTimeIntervalType.Minutes},
                    {"M15", DateTimeIntervalType.Minutes},
                    {"M30", DateTimeIntervalType.Minutes},
                    {"H1", DateTimeIntervalType.Hours},
                    {"D1", DateTimeIntervalType.Days}

                };
        Dictionary<string, int> dictIntervals = new Dictionary<string, int>()
                {
                    {"M1",  1},
                    {"M5",  5},
                    {"M15", 15},
                    {"M30", 30},
                    {"H1",  1},
                    {"D1",  1}


                };



     
         
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

                string path = CUtil.GetConfigDir()+"\\GUIConfig.xml";
                    
                m_GUICOnfig = new CGUIConfig(path);
                CSerializator.Read<CGUIConfig>(ref m_GUICOnfig);


                if (m_mainWindow != null)
                    if (!DesignerProperties.GetIsInDesignMode(m_mainWindow)) 
                (new Thread(ThreadFunc)).Start();

            }
            catch (Exception e)
            {

                Error(" MainWindowViewModel. ", e);

            }



        }



        public void Error(string description, Exception exception = null)
        {
            Plaza2Connector.Error(description, exception);

        }


        private void CorrectMinMax(DateTimeIntervalType  dtintType, int interv,   ref DateTime dtMinCorr, ref DateTime dtMaxCorr)
        {
            
           

            if (DateTimeIntervalType.Minutes == dtintType)
            {
                dtMinCorr = dtMinCorr.AddMinutes(-interv);
                dtMaxCorr = dtMaxCorr.AddMinutes(interv);
            }
            else if (DateTimeIntervalType.Hours == dtintType)
            {
                dtMinCorr = dtMinCorr.AddHours(-interv);
                dtMaxCorr= dtMaxCorr.AddHours(interv);

            }
            else if (DateTimeIntervalType.Days == dtintType)
            {
                dtMinCorr = dtMinCorr.AddDays(-interv);
                dtMaxCorr = dtMaxCorr.AddDays(interv);


            }

        }
        public DateTime GetMinDate(DateTime maxDate,  DateTimeIntervalType dtintType, int dtOffset )
        {
            if (DateTimeIntervalType.Minutes == dtintType)
                return maxDate.AddMinutes(-dtOffset);
            else if (DateTimeIntervalType.Hours == dtintType)
                return maxDate.AddHours(-dtOffset);
            else if (DateTimeIntervalType.Days == dtintType)
                return maxDate.AddHours(-dtOffset);


            return maxDate;
        }



       


      
        public void OnTFButtonClick(object sender,  RoutedEventArgs e)
        {


            try
            {
                string isin = "", tf = "", dt = "";
                DataButton dbut = (DataButton)sender;
                dbut.IsEnabled = false;
                m_mainWindow.DummyButton.Focus();
                if (RetrieveInstTF(dbut.ID, ref isin, ref  tf, ref dt))
                {
                    Thread thread = new Thread(() =>
                        {                            
                            WindowCandleChart wcch = new WindowCandleChart(isin + " " + tf);
                            wcch.CandleChartFrame.Bind(Plaza2Connector, isin, tf, dt, dbut, _dispatcher);
                            wcch.Show();
                            wcch.Closed += new EventHandler(wcch.CandleChartFrame.OnClosed);
                            wcch.CandleChartFrame.IsDispatcherRun = true;
                            System.Windows.Threading.Dispatcher.Run();
                          
                        }
                        );
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
               
               
                
               }                              
                 else
                    MessageBox.Show("Unable to get TF data");
            }
            catch (Exception exc)
            {

                Plaza2Connector.Alarmer.Error("OnTFButtonClick", exc);

            }


        }

      

        private bool _firstTime = true;
        private void AlarmList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {


         if (m_mainWindow.Alarm.AlarmComboBox.Items.Count!=0)
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
                            (ObservableCollection<string>)Plaza2Connector.Alarmer.AlarmList;

            Plaza2Connector.Alarmer.AlarmList.CollectionChanged +=new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AlarmList_CollectionChanged);

               m_mainWindow.Alarm.AlarmComboBox.DropDownOpened +=new EventHandler(AlarmComboBox_DropDownOpened);

;

        }

        private void TaskBindAlarm()
        {
            while (true)
            {
                if (Plaza2Connector.Alarmer != null && Plaza2Connector.Alarmer.AlarmList != null)
                {

                    try
                    {
                        _dispatcher.Invoke(new Action (()=>BindAlarm()));
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

     




        private void OnBotLoad(ControlBotGUI botGUI,long  botId)
        {
            CBotBase bot = Plaza2Connector.GetBotById(botId);
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
         
            botGUI.DealingServer = Plaza2Connector;
            bot.GUIBot.MonitorPos.CollectionChanged += botGUI.OnMonitorPosCollectionChanged;

            botGUI.Orders.ItemsSource = bot.GUIBot.Orders;
            bot.GUIBot.Orders.CollectionChanged += botGUI.OnMonitorOrdersCollectionChanged;

            bot.GUIBot.DisposeGUIBotEvent += botGUI.OnDisposeGUIBot;
                                                                     

            botGUI.ButtonPosLog.Id = bot.BotId;
            botGUI.ButtonPosLog.BotOperations = Plaza2Connector;

            botGUI.ButtonDisableBot.Id = bot.BotId;
            botGUI.ButtonDisableBot.BotOperations = Plaza2Connector;

            botGUI.ButtonEnableBot.Id = bot.BotId;
            botGUI.ButtonEnableBot.BotOperations = Plaza2Connector;
            
            botGUI.ButtonLoadBot.Id = bot.BotId;
            botGUI.ButtonLoadBot.BotOperations = Plaza2Connector;

            botGUI.ButtonManualControl.Id = bot.BotId;
            botGUI.ButtonManualControl.BotOperations = Plaza2Connector;

          



            BindBotButtons(ref botGUI.ButtonPosLog, bot);
            BindBotButtons(ref botGUI.ButtonDisableBot, bot);
            BindBotButtons(ref botGUI.ButtonEnableBot, bot);
            BindBotButtons(ref botGUI.ButtonUnloadBot, bot);

            //botGUI.StopBot


            _vmSTab = new SettingsTableViewModel(bot, botGUI);


            CUtil.SetBinding(bot.GUIBot, "BotState", botGUI.BotSate, ControlSettingsDataBlock.SettingValueTextProperty);
            botGUI.EvntLoadBot += OnBotLoad;
                                                         
        }

        private void BindBotButtons(ref ButtonBot butBot, CBotBase bot)
        {

            butBot.Id = bot.BotId;
            butBot.BotOperations = Plaza2Connector;



        }



        private void TaskBindClock()
        {
            try
            {
                while (!Plaza2Connector.IsServerTimeAvailable)
                    Thread.Sleep(100);
             

                _dispatcher.BeginInvoke(new Action( () => 
                   CUtil.SetBinding(Plaza2Connector.GUIBox, "ServerTime", m_mainWindow.ClockBox.TextWin.TextBlockLabel,
                                    TextBlock.TextProperty, "HH:mm:ss")
                   ));


            }
            catch (Exception e)
            {
                Error("TaskBindClock",e);

            }
                

        }

        private void TaskBindTFButtons()
        {

            try
            {

                while (true)
                {


                    //TO DO use OS events
                    while (Plaza2Connector == null || Plaza2Connector.GUIBox == null || Plaza2Connector.GUIBox.GUICandleBox == null ||
                            !m_mainWindow.GridData.IsButtonsLoaded || !Plaza2Connector.IsServerTimeAvailable ||
                            !Plaza2Connector.IsDealsOnline || !Plaza2Connector.IsAnalyzerTFOnline )
                        Thread.Sleep(1000);

                    bool b_allEnabled = true;
                    foreach (var cb in m_mainWindow.GridData.DictionaryControllButtonTF)
                    {

                        string id = ""; ;
                        
                        _dispatcher.Invoke
                            (new Action(
                                () =>id= cb.Value.TFButton.Tag.ToString()
                                        )
                              );
                        //string id = cb.Value.TFButton.Tag.ToString();
                        string isin = "";
                        string tf = "";
                        string dt = "";

                        if (RetrieveInstTF(id, ref isin, ref  tf, ref dt)
                            && Plaza2Connector.GUIBox.GUICandleBox.IsTFAvailable(isin, tf, dt))
                            _dispatcher.Invoke(new Action(() => cb.Value.TFButton.IsEnabled = true));
                        else
                        {
                            if (id.IndexOf("HST") <= 0) 
                                b_allEnabled = false;


                            _dispatcher.Invoke(new Action(() => cb.Value.TFButton.IsEnabled = false));
                           

                        }
                        Thread.Sleep(100);



                    }

                    if (b_allEnabled)
                        return;



                }
            }
            catch (Exception e)
            {
                Error("TaskBindTFButtons", e);

            }
            
        }
        //bind all data


        

        private void CreateControlBotGUIs()
        {
            //TO DO generate it dinamically

            try
            {
              BindBotControls(m_mainWindow.Bot1, Plaza2Connector.GetBotById(m_GUICOnfig.ListPositionId[0]));
              BindBotControls(m_mainWindow.Bot2, Plaza2Connector.GetBotById(m_GUICOnfig.ListPositionId[1]));
              BindBotControls(m_mainWindow.Bot3, Plaza2Connector.GetBotById(m_GUICOnfig.ListPositionId[2]));
              BindBotControls(m_mainWindow.Bot4, Plaza2Connector.GetBotById(m_GUICOnfig.ListPositionId[3]));


             //   BindBotControls(m_mainWindow.Bot2, Plaza2Connector.ListBots[1]);
             //   BindBotControls(m_mainWindow.Bot3, Plaza2Connector.ListBots[2]);
             //   BindBotControls(m_mainWindow.Bot4, Plaza2Connector.ListBots[3]);
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
                    if (Plaza2Connector != null &&
                        Plaza2Connector.GUIBox != null && Plaza2Connector.IsAllBotLoaded)
                        
                        break;

                    Thread.Sleep(SLEEP_DUR);


                }
                if (i == TRIAL_COUNT)
                {
                    if (Plaza2Connector == null || Plaza2Connector.Alarmer == null)
                        Debug.Assert(false, "TaskBindData Timeout !");
                    else
                        Error("TaskBindData Timeout !");

                }


                Plaza2Connector.GUIBox.GUIDispatcher = _dispatcher;

                _dispatcher.UnhandledException += new DispatcherUnhandledExceptionEventHandler(Plaza2Connector.Alarmer.GUIdispatcher_UnhandledException); 



                (new Task(TaskBindAlarm)).Start();
                (new Task(TaskBindClock)).Start();
                (new Task(TaskBindTFButtons)).Start();

                CreateControlBotGUIs();
                


           //   Plaza2Connector.GUIBox.GUICandleBox.CandleBoxUpdated += OnGUICandleBoxUpdated;

                m_mainWindow.GridData.DealingServer = Plaza2Connector;
                m_mainWindow.GridData.GUIDispatcher = _dispatcher;

                m_mainWindow.FORTSBox.AutoBind((object)Plaza2Connector.GUIBox);
                m_mainWindow.ButtonAnalyzeStatus.AutoBind((object)Plaza2Connector.GUIBox);
                m_mainWindow.ButtonDataBaseStatus.AutoBind((object)Plaza2Connector.GUIBox);


                m_mainWindow.FORTSBox.ButtOkOrNot.Click += new RoutedEventHandler(ButtOkOrNot_Click);

                m_mainWindow.SessionTable.DataContext = Plaza2Connector.GUIBox;

      
              
            }
            catch (Exception e)
            {
                Debug.Assert(false, "TaskBindData Error !"+e.Message);
            }

        }

        void ButtOkOrNot_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            FORTSStatusWindow wfs = new FORTSStatusWindow();
            wfs.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            wfs.ControlFORTSStatus.BindButtons( (object) Plaza2Connector.GUIBox);
            m_mainWindow.DummyButton.Focus();
            wfs.ShowDialog();

        }

        private bool RetrieveInstTF(string buttName, ref string  isin, ref  string tf, ref string stDt )
        {

               Regex reg = new Regex(@"([\w0-9\-\.]+)_([\w0-9]+)");
                Match m = reg.Match(buttName);
                if (m.Groups.Count > 2)
                {
                    isin = Convert.ToString(m.Groups[1]);
                    tf = Convert.ToString(m.Groups[2]);

                    DateTime dt = Plaza2Connector.ServerTime;
                    
                    stDt = CUtilTime.NormalizeDay(dt).ToString();
                    return true;
                }

            return false;
        }

     
        

            

        


        private void ThreadFunc()
        {
            Plaza2Connector = new CPlaza2Connector();

     
            _dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(BindData));
          
        

            Plaza2Connector.Process();

            


        }
      








    }
}
