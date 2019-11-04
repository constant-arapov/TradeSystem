namespace Visualizer
{
    using IntelliTradeComplex2.Modules;
    using IntelliTradeComplex2.Trade;
    using Model;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Timers;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Threading;
    using System.Xml;
    using Xceed.Wpf.Toolkit;

    public class MainControl : UserControl, IIntelliThinkEventsReceiver, IComponentConnector
    {
        private bool _contentLoaded;
        internal Button Buttn_RemoveChild;
        internal Button Buttn_Settings;
        internal Grid Grid_OrderFailedReason;
        internal GridSplitter LeftGridSplitter;
        private byte m_ActiveWorkAmount;
        private bool m_bIgnoreKeyHolding;
        private bool m_bLayerNull = true;
        private bool m_bMouseMode = true;//KAA tempo
        private IIntelliThink m_Bot;
        private double m_dStep = 1.0;
        private int m_iDecimals;
        private int m_iElapsedCounter;
        private long m_LastRoundTripTime;
        private List<TextBlock> m_lRoundTripTxtBlocks = new List<TextBlock>();
        private List<Stop_Order> m_lStopOrders = new List<Stop_Order>();
        private List<int> m_lWorkAmounts = new List<int>();
        private List<TextBlock> m_lWorkAmountTxtBlocks = new List<TextBlock>();
        private string m_sAssignedContractName = string.Empty;
        private XmlDocument m_Settings_XML = new XmlDocument();
        private Contract_Settings m_SettingsObject;
        public SettingsWindow m_SettingsWindow;
        private string m_sOrderFailedText = string.Empty;
        private string m_sWorkingDirectory = string.Empty;
        private Tick_Info m_Ticks;
        private System.Timers.Timer m_Timer = new System.Timers.Timer(500.0);
        private System.Timers.Timer m_Timer_CheckCoord = new System.Timers.Timer(60000.0);
        private System.Timers.Timer m_Timer_LayerUnNull = new System.Timers.Timer(100.0);
        internal Grid MainControl_Grid;
        internal Image MouseActiveIcon;
        internal Grid PositionGrid;
        internal Grid RoundTripGrid;
        /*internal*/ public  ClusterPanel Run_ClusterPanel;
        /*internal*/public DOM Run_DOM;//KAA tempo
        /*internal*/public TickPanel Run_TickPanel;//KAA
        internal TextBlock TextBlock_OrderFailedReason;
        internal Grid ToolsGrid;
        internal TextBlock TxtBlck_Amount;
        internal TextBlock TxtBlck_Amount1;
        internal TextBlock TxtBlck_Amount2;
        internal TextBlock TxtBlck_Amount3;
        internal TextBlock TxtBlck_ChangeInstrument;
        internal TextBlock TxtBlck_Price;
        internal TextBlock TxtBlck_Profit;
        internal TextBlock TxtBlck_RTP1;
        internal TextBlock TxtBlck_RTP2;
        internal TextBlock TxtBlck_RTP3;
        internal TextBlock TxtBlck_TimeNow;
        internal Grid WorkAmountsGrid;

        public event Action<MainControl> AddLeaderRequest;

        public event Action DoAllFocused;

        public event Action DoNotSendNextOrderAsClose;

        public event Action<MainControl> SettingsWindow_SaveToXML;

        public event Action<MainControl> TryingTo_ChangeInstrument;

        public event Action<MainControl> TryingTo_RemoveInstrument;

        public event Action<bool> UnBlockViewModel;


        public static readonly DependencyProperty TicksProperty = DependencyProperty.Register("Ticks", typeof(Tick_Info), typeof(MainControl));

        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(double), typeof(MainControl), new UIPropertyMetadata(0.0));
        
        

        private ITradeOperations _tradeOperation;


        private int _stockNum;


        public MainControl(int stockNum)
        {
            this.InitializeComponent();

            _stockNum = stockNum;

           // if (stockNum == 0)
             //   HideRemoveChildButton();

            base.Loaded += new RoutedEventHandler(this.MainControl_Loaded);
            this.Run_DOM.PriceCoordChanged += new EventHandler(this.PriceCoord_ToControls);
            this.Run_DOM.DOMScrolledEvent += new EventHandler(this.DOMScroll_ToControls);
            if (this.m_bMouseMode)
            {
                this.MouseActiveIcon.Visibility = Visibility.Visible;
            }
            else if (!this.m_bMouseMode)
            {
                this.MouseActiveIcon.Visibility = Visibility.Collapsed;
            }
            List<TextBlock> list = new List<TextBlock> {
                this.TxtBlck_Price,
                this.TxtBlck_Amount,
                this.TxtBlck_Profit
            };
            this.Run_DOM.PositionGrid = this.PositionGrid;
            this.Run_DOM.PositionTextBlocks = list;
            this.TxtBlck_Amount1.Tag = 1;
            this.TxtBlck_Amount2.Tag = 2;
            this.TxtBlck_Amount3.Tag = 3;
            this.m_lWorkAmountTxtBlocks.Add(this.TxtBlck_Amount1);
            this.m_lWorkAmountTxtBlocks.Add(this.TxtBlck_Amount2);
            this.m_lWorkAmountTxtBlocks.Add(this.TxtBlck_Amount3);
            this.m_lRoundTripTxtBlocks.Add(this.TxtBlck_RTP1);
            this.m_lRoundTripTxtBlocks.Add(this.TxtBlck_RTP2);
            this.m_lRoundTripTxtBlocks.Add(this.TxtBlck_RTP3);
            this.Run_TickPanel.TenLevels = this.Run_DOM.TenLevels;
            this.Run_TickPanel.FiftyLevels = this.Run_DOM.FiftyLevels;
            this.Run_TickPanel.Dict_KPriceVCoordin = this.Run_DOM.Dict_KPriceVCoordin;
            this.Run_ClusterPanel.Dict_KPriceVCoordin = this.Run_DOM.Dict_KPriceVCoordin;
            this.m_Timer_CheckCoord.Elapsed += new ElapsedEventHandler(this.m_Timer_CheckCoord_Elapsed);
            this.m_Timer_CheckCoord.Start();
            this.m_Timer.Elapsed += new ElapsedEventHandler(this.m_Timer_Elapsed);
            this.m_Timer_LayerUnNull.Elapsed += new ElapsedEventHandler(this.m_Timer_LayerUnNull_Elapsed);
            //this.Step = 1.0;
            //KAA onfocus event
            DoAllFocused += ToFocus;
          //  this.KeyDown += new KeyEventHandler(MainControl_KeyDown);
            //this.PreviewKeyDown += new KeyEventHandler(MainControl_PreviewKeyDown);

            this.PreviewKeyDown += new KeyEventHandler(MainControl_PreviewKeyDown);
        }


       




        public void MainControl_KeyDown(object sender, KeyEventArgs e)
        {
            
        }


        public void BindToSystem( ITradeOperations tradeOperation)
        {

            _tradeOperation = tradeOperation;


        }

       
       

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        internal Delegate _CreateDelegate(Type delegateType, string handler)
        {
            return Delegate.CreateDelegate(delegateType, this, handler);
        }

        public void BlockPaint45msec()
        {
            this.Run_DOM.BlockPaint = true;
            this.Run_TickPanel.BlockPaint = true;
            this.Run_ClusterPanel.BlockPaint = true;
            this.UnBlockViewModel(true);
            this.m_Timer.Start();
        }

        public void Buttn_RemoveChild_Click(object sender, RoutedEventArgs e)
        {
            //this.TryingTo_RemoveInstrument(this);

            _tradeOperation.DeleteInstrument();

        }

        private void Buttn_Settings_Click(object sender, RoutedEventArgs e)
        {
            if ((this.m_SettingsWindow == null) && (this.SettingsObject != null))
            {
                this.m_SettingsWindow = new SettingsWindow();
                this.m_SettingsWindow.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.m_SettingsWindow_IsVisibleChanged);
                this.SettingsWinInit();
                this.m_SettingsWindow.Buttn_FirstWorkAmount.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
                this.m_SettingsWindow.Buttn_SecondWorkAmount.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
                this.m_SettingsWindow.Buttn_ThirdWorkAmount.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
                this.m_SettingsWindow.Buttn_FirstIncreaseAmount.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
                this.m_SettingsWindow.Buttn_SecondIncreaseAmount.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
                this.m_SettingsWindow.Buttn_DecreaseAmount.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
                this.m_SettingsWindow.Buttn_MaxPosition.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
                this.m_SettingsWindow.Buttn_BackAmount.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
                this.m_SettingsWindow.Buttn_ThrowLimitTo.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
                this.m_SettingsWindow.Buttn_StopLossSteps.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
                this.m_SettingsWindow.Buttn_TakeProfitSteps.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
                this.m_SettingsWindow.Buttn_AveragingMethod.SelectionChanged += new SelectionChangedEventHandler(this.SettingsChanged_ComboBox);
                this.m_SettingsWindow.Buttn_AddLeader.Click += new RoutedEventHandler(this.SettingsChanged_Button);
                this.m_SettingsWindow.Buttn_RemoveLeader.Click += new RoutedEventHandler(this.SettingsChanged_Button);
                this.m_SettingsWindow.Buttn_ShowLeadersNames.Click += new RoutedEventHandler(this.SettingsChanged_Button);
                this.m_SettingsWindow.Buttn_ShowClusterPanel.Click += new RoutedEventHandler(this.SettingsChanged_Button);
                this.m_SettingsWindow.Buttn_StringHeight.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
                this.Run_DOM.SettingsWin = this.m_SettingsWindow;
                this.Run_TickPanel.SettingsWin = this.m_SettingsWindow;
                this.Run_ClusterPanel.SettingsWin = this.m_SettingsWindow;
                Window window = Window.GetWindow(this);
                this.m_SettingsWindow.Owner = window;
            }
            if (this.m_SettingsWindow != null)
            {
                this.SettingsWinInit();
                Point point = new Point(Mouse.GetPosition(this).X, Mouse.GetPosition(this).Y);
                Point point2 = base.PointToScreen(point);
                this.m_SettingsWindow.Top = point2.Y;
                this.m_SettingsWindow.Left = point2.X - (this.m_SettingsWindow.Width / 2.0);
                this.m_SettingsWindow.Title = "Настройки - " + this.m_sAssignedContractName;
                this.m_SettingsWindow.ShowDialog();
            }
        }

        public void Do_AllSpreadCentering()
        {
            this.Run_DOM.FocusAsk = 0;
        }

        private void DOMScroll_ToControls(object sender, EventArgs e)
        {
            this.Run_TickPanel.ScrollDelta = this.Run_DOM.DOMScrollDelta;
            this.Run_ClusterPanel.ScrollDelta = this.Run_DOM.DOMScrollDelta;
        }

        public void HideRemoveChildButton()
        {
            this.Buttn_RemoveChild.IsEnabled = false;
            this.Buttn_RemoveChild.Visibility = System.Windows.Visibility.Hidden;

        }

        public void UnhideRemoveChildButton()
        {
            this.Buttn_RemoveChild.IsEnabled = true;
            this.Buttn_RemoveChild.Visibility = System.Windows.Visibility.Visible;

        }




        public void HideSettingsWindow()
        {
            if (this.m_SettingsWindow != null)
            {
                this.m_SettingsWindow.Hide();
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/Visualizer;component/maincontrol.xaml", UriKind.Relative);
              
            
                //KAA
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void Layer_OrderFailedReason(string ReasonCode)
        {
            this.m_sOrderFailedText = string.Empty;
            if (ReasonCode == "-6")
            {
                this.m_sOrderFailedText = "Заявка отклонена сервером биржи";
            }
            else if (ReasonCode == "-5")
            {
                this.m_sOrderFailedText = "Не хватает средств или превышена максимально допустимая просадка";
            }
            else if (ReasonCode == "-4")
            {
                this.m_sOrderFailedText = "Торги инструментом запрещены";
            }
            else if (ReasonCode == "-3")
            {
                this.m_sOrderFailedText = "Не верная цена заявки";
            }
            else if (ReasonCode == "-2")
            {
                this.m_sOrderFailedText = "Не верный объём заявки";
            }
            if (!this.m_Timer_LayerUnNull.Enabled)
            {
                this.m_iElapsedCounter = 0;
                this.m_Timer_LayerUnNull.Start();
            }
        }

        private void m_SettingsWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!((bool) e.NewValue))
            {
                this.SaveInstrumentSettings();
            }
        }

        private void m_Timer_CheckCoord_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Run_ClusterPanel.ScrollDelta = this.Run_DOM.DOMScrollDelta;
            this.SaveClusters();
        }

        private void m_Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action method = new Action(this.UnBlockPaint);
            base.Dispatcher.Invoke(DispatcherPriority.Send, method);
        }

        private void m_Timer_LayerUnNull_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.m_sOrderFailedText != string.Empty)
            {
                Action method = new Action(this.ShowOrderFailed);
                base.Dispatcher.Invoke(DispatcherPriority.Render, method);
            }
            else
            {
                this.m_Timer_LayerUnNull.Stop();
            }
        }

        private void MainControl_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            if (window != null)
            {
                window.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.W_LostKeyboardFocus);
                //KAA
               // base.MouseDown += new MouseButtonEventHandler(this.MainControl_MouseDown);
                base.PreviewMouseDown += new MouseButtonEventHandler(this.MainControl_MouseDown);
            }
            this.Run_DOM.MC_Grid = this.MainControl_Grid;
        }

        private void MainControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!base.IsKeyboardFocused)
            {
                this.DoAllFocused();
            }
            
        }

        public void OnChildAdded(IIntelliThink Sender, IIntelliThink AddedChild)
        {
        }

        public void OnChildRemoved(IIntelliThink Sender, IIntelliThink RemovedChild)
        {
        }

        public void OnLayerCleared(IIntelliThink Sender)
        {
            if (Sender.Layer != null)
            {
                //KAA
               // Sender.Layer.remove_OrderFailedReason(new Action<string>(this.Layer_OrderFailedReason));
               // Sender.Layer.remove_OrderFailedReason(new Action<string>(this.Layer_OrderFailedReason));
            }
        }

        public void OnLayerCreated(IIntelliThink Sender, ILayer Layer)
        {
            //KAA
            //Layer.add_OrderFailedReason(new Action<string>(this.Layer_OrderFailedReason));
        }

        public void MainControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            var t = sender.GetType();
            //TODO normal
            if (t.Name != "MainWindow")
                return;
                //Thread.Sleep(0);


            

            
            
            this.Run_DOM.LastClickCoord = 3.0;


            if (e.Key == Key.RightCtrl || e.Key == Key.LeftCtrl)
            {
                //Thread.Sleep(0);
                if (_tradeOperation != null)
                {

                    //_tradeOperation.AddOrder(
                    _tradeOperation.CancellAllOrders();
                    _tradeOperation.CloseAllPositions();
                    
                }
                

            }
            else if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                if (_tradeOperation != null)
                {

                    this.Do_AllSpreadCentering();
                }

            }

            else if (e.Key == Key.Space)
            {

               
                _tradeOperation.CancellAllOrders();

            }


        }

        public void PreviewKeyUp(object sender, KeyEventArgs e)
        {
            this.Run_DOM.CanClick_ChangeStopOrTake = false;
            this.m_bIgnoreKeyHolding = false;
        }

        private void PriceCoord_ToControls(object sender, EventArgs e)
        {
        }

        public void SaveClusters()
        {
            this.Run_ClusterPanel.SaveClusters_OuterCommand();
        }

        public void SaveInstrumentSettings()
        {
            this.SettingsWindow_SaveToXML(this);
        }

        public void SendNextOrderAsClose(bool b)
        {
            if (b)
            {
                this.Run_TickPanel.SendNextOrderAsClose = true;
                this.Run_TickPanel.TicksFilled = true;
            }
            else
            {
                this.Run_TickPanel.SendNextOrderAsClose = false;
                this.Run_TickPanel.TicksFilled = true;
            }
        }

        public void SendOrder_ByBestPrice(bool _ChooseOrderAction)
        {
            if (((!this.m_bIgnoreKeyHolding && this.m_bMouseMode) && (((this.Bot != null) && (this.Bot.Layer != null)) && ((this.Run_DOM.Asks != null) && (this.Run_DOM.Bids != null)))) && ((this.Run_DOM.Asks[0] != null) && (this.Run_DOM.Bids[0] != null)))
            {
                this.m_bIgnoreKeyHolding = true;
                OrderAction unknow = OrderAction.Unknow;
                double price = 0.0;
                int num2 = 0;
                if (this.ActiveWorkAmount == 1)
                {
                    num2 = this.SettingsObject.Trading_Settings.First_WorkAmount;
                }
                if (this.ActiveWorkAmount == 2)
                {
                    num2 = this.SettingsObject.Trading_Settings.Second_WorkAmount;
                }
                if (this.ActiveWorkAmount == 3)
                {
                    num2 = this.SettingsObject.Trading_Settings.Third_WorkAmount;
                }
                if (_ChooseOrderAction)
                {
                    unknow = OrderAction.Buy;
                    if (Math.Round((double) ((this.Run_DOM.Asks[0].Price - this.Run_DOM.Bids[0].Price) / this.Step), 0) != 1.0)
                    {
                        price = Math.Round((double) (this.Run_DOM.Bids[0].Price + this.Step), this.Decimals);
                    }
                    else
                    {
                        price = Math.Round(this.Run_DOM.Bids[0].Price, this.Decimals);
                    }
                }
                else if (!_ChooseOrderAction)
                {
                    unknow = OrderAction.Sell;
                    if (Math.Round((double) ((this.Run_DOM.Asks[0].Price - this.Run_DOM.Bids[0].Price) / this.Step), 0) != 1.0)
                    {
                        price = Math.Round((double) (this.Run_DOM.Asks[0].Price - this.Step), this.Decimals);
                    }
                    else
                    {
                        price = Math.Round(this.Run_DOM.Asks[0].Price, this.Decimals);
                    }
                }
                this.Bot.Layer.SendOrder(unknow, price, (double) num2, OrderFlags.Limit);
            }
        }

        private void SetActiveWorkAmount()
        {
            foreach (TextBlock block in this.m_lWorkAmountTxtBlocks)
            {
                if (((int) block.Tag) == this.m_ActiveWorkAmount)
                {
                    block.FontWeight = FontWeights.Bold;
                    block.Background = Brushes.WhiteSmoke;
                }
                else
                {
                    block.FontWeight = FontWeights.Normal;
                    block.Background = Brushes.LightGray;
                }
            }
        }

        private void SetMouseMode()
        {
            if (this.m_bMouseMode)
            {
                this.MouseActiveIcon.Visibility = Visibility.Visible;
            }
            else if (!this.m_bMouseMode)
            {
                this.MouseActiveIcon.Visibility = Visibility.Collapsed;
            }
        }

        private void SetRoundTrips()
        {
            this.TxtBlck_RTP1.Text = this.TxtBlck_RTP2.Text;
            this.TxtBlck_RTP2.Text = this.TxtBlck_RTP3.Text;
            this.TxtBlck_RTP3.Text = this.m_LastRoundTripTime.ToString();
            this.TxtBlck_RTP1.Background = this.TxtBlck_RTP2.Background;
            this.TxtBlck_RTP2.Background = this.TxtBlck_RTP3.Background;
            Brush lightGray = Brushes.LightGray;
            if (this.m_LastRoundTripTime < 250L)
            {
                lightGray = Brushes.LightGreen;
            }
            else if (this.m_LastRoundTripTime <= 650L)
            {
                lightGray = Brushes.Yellow;
            }
            else if (this.m_LastRoundTripTime > 650L)
            {
                lightGray = Brushes.LightCoral;
            }
            this.TxtBlck_RTP3.Background = lightGray;
        }

        private void SettingsChanged_Button(object sender, RoutedEventArgs e)
        {
            if ((sender != null) && (this.SettingsObject != null))
            {
                Button button = (Button) sender;
                if (button.Tag.ToString() == "Buttn_ShowClusterPanel")
                {
                    if (this.Run_ClusterPanel.Visibility == Visibility.Visible)
                    {
                        this.Run_ClusterPanel.Visibility = Visibility.Collapsed;
                        this.LeftGridSplitter.Visibility = Visibility.Collapsed;
                        if (this.MainControl_Grid.ColumnDefinitions.Count >= 1)
                        {
                            this.MainControl_Grid.ColumnDefinitions[0].MinWidth = 0.0;
                            this.MainControl_Grid.ColumnDefinitions[0].Width = GridLength.Auto;
                        }
                        button.Content = "Нет";
                        this.SettingsObject.ClusterPanel_Settings.ShowClusterPanel = false;
                    }
                    else
                    {
                        this.Run_ClusterPanel.Visibility = Visibility.Visible;
                        this.LeftGridSplitter.Visibility = Visibility.Visible;
                        if (this.MainControl_Grid.ColumnDefinitions.Count >= 1)
                        {
                            this.MainControl_Grid.ColumnDefinitions[0].MinWidth = 15.0;
                            this.MainControl_Grid.ColumnDefinitions[0].Width = new GridLength(15.0, GridUnitType.Star);
                        }
                        button.Content = "Да";
                        this.SettingsObject.ClusterPanel_Settings.ShowClusterPanel = true;
                    }
                }
                if (button.Tag.ToString() == "Buttn_SaveClusters")
                {
                    bool flag = button.Content.ToString() != "Да";
                    this.SettingsObject.ClusterPanel_Settings.SaveClusters = flag;
                    button.Content = (button.Content.ToString() == "Да") ? "Нет" : "Да";
                    if (this.TradingSettingsList != null)
                    {
                        ModuleProperty property = null;
                        property = this.TradingSettingsList.Find(a => a.Text == "Сохранять кластеры");
                        if (property != null)
                        {
                            property.Value = flag;
                        }
                    }
                }
                else if (button.Tag.ToString() == "Buttn_AddLeader")
                {
                    if (this.TradingSettingsList != null)
                    {
                        ModuleProperty property2 = null;
                        property2 = this.TradingSettingsList.Find(a => a.Text == "Добавить поводыря");
                        if (property2 != null)
                        {
                            this.AddLeaderRequest(this);
                            if (this.AddedLeader != null)
                            {
                                property2.Value = this.AddedLeader;
                            }
                            this.AddedLeader = null;
                        }
                    }
                }
                else if (button.Tag.ToString() == "Buttn_RemoveLeader")
                {
                    if (this.TradingSettingsList != null)
                    {
                        ModuleProperty property3 = null;
                        property3 = this.TradingSettingsList.Find(a => a.Text == "Поводыри - удалить последнего");
                        if (property3 != null)
                        {
                            object obj2 = new object();
                            property3.Value = obj2;
                        }
                    }
                }
                else if ((button.Tag.ToString() == "Buttn_ShowLeadersNames") && (this.TradingSettingsList != null))
                {
                    ModuleProperty property4 = null;
                    property4 = this.TradingSettingsList.Find(a => a.PropertyID == "ShowChildsNames");
                    if (property4 != null)
                    {
                        object obj3 = new object();
                        property4.Value = obj3;
                        button.Content = property4.Text;
                    }
                }
            }
        }

        private void SettingsChanged_ComboBox(object sender, SelectionChangedEventArgs s)
        {
            if (((sender != null) && (this.SettingsObject != null)) && (this.TradingSettingsList != null))
            {
                ComboBox box = (ComboBox) sender;
                int selectedIndex = box.SelectedIndex;
                string str = box.Tag.ToString();
                ModuleProperty property = null;
                if (str == "Buttn_AveragingMethod")
                {
                    property = this.TradingSettingsList.Find(a => a.Text == "Метод усреднения");
                    if (property != null)
                    {
                        switch (selectedIndex)
                        {
                            case 1:
                                property.Value = selectedIndex;
                                this.SettingsObject.Trading_Settings.AveragingMethod = "Stack";
                                return;

                            case 0:
                                property.Value = selectedIndex;
                                this.SettingsObject.Trading_Settings.AveragingMethod = "Queque";
                                break;
                        }
                    }
                }
            }
        }

        private void SettingsChanged_NumericUpDown(object sender, RoutedPropertyChangedEventArgs<object> o)
        {
            if (((sender != null) && (o.NewValue != null)) && (this.SettingsObject != null))
            {
                IntegerUpDown down = (IntegerUpDown) sender;
                int newValue = (int) o.NewValue;
                string str = down.Tag.ToString();
                ModuleProperty property = null;
                if (this.TradingSettingsList != null)
                {
                    switch (str)
                    {
                        case "Buttn_FirstWorkAmount":
                            property = this.TradingSettingsList.Find(a => a.Text == "Рабочий объём 1");
                            if (property != null)
                            {
                                property.Value = newValue;
                                this.SettingsObject.Trading_Settings.First_WorkAmount = newValue;
                                property = null;
                                property = this.TradingSettingsList.Find(a => a.Text == "Макс. позиция");
                                if (property != null)
                                {
                                    this.m_SettingsWindow.Buttn_MaxPosition.Value = new int?(Convert.ToInt32(property.Value));
                                    this.SettingsObject.Trading_Settings.MaxPosition = Convert.ToInt32(property.Value);
                                }
                            }
                            break;

                        case "Buttn_SecondWorkAmount":
                            property = this.TradingSettingsList.Find(a => a.Text == "Рабочий объём 2");
                            if (property != null)
                            {
                                property.Value = newValue;
                                this.SettingsObject.Trading_Settings.Second_WorkAmount = newValue;
                                property = null;
                                property = this.TradingSettingsList.Find(a => a.Text == "Макс. позиция");
                                if (property != null)
                                {
                                    this.m_SettingsWindow.Buttn_MaxPosition.Value = new int?(Convert.ToInt32(property.Value));
                                    this.SettingsObject.Trading_Settings.MaxPosition = Convert.ToInt32(property.Value);
                                }
                            }
                            break;

                        case "Buttn_ThirdWorkAmount":
                            property = this.TradingSettingsList.Find(a => a.Text == "Рабочий объём 3");
                            if (property != null)
                            {
                                property.Value = newValue;
                                this.SettingsObject.Trading_Settings.Third_WorkAmount = newValue;
                                property = null;
                                property = this.TradingSettingsList.Find(a => a.Text == "Макс. позиция");
                                if (property != null)
                                {
                                    this.m_SettingsWindow.Buttn_MaxPosition.Value = new int?(Convert.ToInt32(property.Value));
                                    this.SettingsObject.Trading_Settings.MaxPosition = Convert.ToInt32(property.Value);
                                }
                            }
                            break;

                        case "Buttn_FirstIncreaseAmount":
                            property = this.TradingSettingsList.Find(a => a.Text == "Объём увеличения 1");
                            if (property != null)
                            {
                                property.Value = newValue;
                                this.SettingsObject.Trading_Settings.First_IncreaseAmount = newValue;
                            }
                            break;

                        case "Buttn_SecondIncreaseAmount":
                            property = this.TradingSettingsList.Find(a => a.Text == "Объем увеличения 2");
                            if (property != null)
                            {
                                property.Value = newValue;
                                this.SettingsObject.Trading_Settings.Second_IncreaseAmount = newValue;
                            }
                            break;

                        case "Buttn_DecreaseAmount":
                            property = this.TradingSettingsList.Find(a => a.Text == "Объём уменьшения");
                            if (property != null)
                            {
                                property.Value = newValue;
                                this.SettingsObject.Trading_Settings.DecreaseAmount = newValue;
                            }
                            break;

                        case "Buttn_MaxPosition":
                            property = this.TradingSettingsList.Find(a => a.Text == "Макс. позиция");
                            if (property != null)
                            {
                                property.Value = newValue;
                                this.SettingsObject.Trading_Settings.MaxPosition = newValue;
                                property = null;
                                property = this.TradingSettingsList.Find(a => a.Text == "Рабочий объём 1");
                                if (property != null)
                                {
                                    this.m_SettingsWindow.Buttn_FirstWorkAmount.Value = new int?(Convert.ToInt32(property.Value));
                                    this.SettingsObject.Trading_Settings.First_WorkAmount = Convert.ToInt32(property.Value);
                                }
                                property = null;
                                property = this.TradingSettingsList.Find(a => a.Text == "Рабочий объём 2");
                                if (property != null)
                                {
                                    this.m_SettingsWindow.Buttn_SecondWorkAmount.Value = new int?(Convert.ToInt32(property.Value));
                                    this.SettingsObject.Trading_Settings.Second_WorkAmount = Convert.ToInt32(property.Value);
                                }
                                property = null;
                                property = this.TradingSettingsList.Find(a => a.Text == "Рабочий объём 3");
                                if (property != null)
                                {
                                    this.m_SettingsWindow.Buttn_ThirdWorkAmount.Value = new int?(Convert.ToInt32(property.Value));
                                    this.SettingsObject.Trading_Settings.Third_WorkAmount = Convert.ToInt32(property.Value);
                                }
                            }
                            break;

                        case "Buttn_BackAmount":
                            property = this.TradingSettingsList.Find(a => a.Text == "Опорный объём");
                            if (property != null)
                            {
                                property.Value = newValue;
                                this.SettingsObject.Trading_Settings.BackAmount = newValue;
                            }
                            break;

                        case "Buttn_ThrowLimitTo":
                            property = this.TradingSettingsList.Find(a => a.Text == "Дальность застрела ");
                            if (property != null)
                            {
                                property.Value = newValue;
                                this.SettingsObject.Trading_Settings.ThrowLimitTo = newValue;
                            }
                            break;

                        case "Buttn_StopLossSteps":
                            this.Run_DOM.StopLoss = newValue;
                            this.SettingsObject.Trading_Settings.StopLoss_Steps = newValue;
                            break;

                        case "Buttn_TakeProfitSteps":
                            this.Run_DOM.TakeProfit = newValue;
                            this.SettingsObject.Trading_Settings.TakeProfit_Steps = newValue;
                            break;
                    }
                    if (str == "Buttn_StringHeight")
                    {
                        //KAA 2016-05-31
                       // this.Run_DOM.StringHeight = newValue;
                       // this.Run_TickPanel.StringHeight = newValue;
                       // this.Run_ClusterPanel.StringHeight = newValue;
                        this.SettingsObject.DOM_Settings.StringHeight = newValue;
                    }
                }
            }
        }

        public void SettingsSharing()
        {
            this.Run_DOM.StopLoss = this.SettingsObject.Trading_Settings.StopLoss_Steps;
            this.Run_DOM.TakeProfit = this.SettingsObject.Trading_Settings.TakeProfit_Steps;
            if (this.TradingSettingsList != null)
            {
                foreach (ModuleProperty property in this.TradingSettingsList)
                {
                    switch (property.Text)
                    {
                        case "Рабочий объём 1":
                            property.Value = this.SettingsObject.Trading_Settings.First_WorkAmount;
                            break;

                        case "Рабочий объём 2":
                            property.Value = this.SettingsObject.Trading_Settings.Second_WorkAmount;
                            break;

                        case "Рабочий объём 3":
                            property.Value = this.SettingsObject.Trading_Settings.Third_WorkAmount;
                            break;

                        case "Объём увеличения 1":
                            property.Value = this.SettingsObject.Trading_Settings.First_IncreaseAmount;
                            break;

                        case "Объем увеличения 2":
                            property.Value = this.SettingsObject.Trading_Settings.Second_IncreaseAmount;
                            break;

                        case "Объём уменьшения":
                            property.Value = this.SettingsObject.Trading_Settings.DecreaseAmount;
                            break;

                        case "Опорный объём":
                            property.Value = this.SettingsObject.Trading_Settings.BackAmount;
                            break;

                        case "Дальность застрела ":
                            property.Value = this.SettingsObject.Trading_Settings.ThrowLimitTo;
                            break;

                        case "Макс. позиция":
                            property.Value = this.SettingsObject.Trading_Settings.MaxPosition;
                            break;

                        case "Метод усреднения":
                        {
                            int num = 1;
                            int num2 = 0;
                            if (this.SettingsObject.Trading_Settings.AveragingMethod == "Stack")
                            {
                                property.Value = num;
                            }
                            if (this.SettingsObject.Trading_Settings.AveragingMethod == "Queque")
                            {
                                property.Value = num2;
                            }
                            break;
                        }
                        case "Сохранять кластеры":
                            property.Value = this.SettingsObject.ClusterPanel_Settings.SaveClusters;
                            break;
                    }
                }
            }
            if (!this.SettingsObject.ClusterPanel_Settings.ShowClusterPanel)
            {
                this.Run_ClusterPanel.Visibility = Visibility.Collapsed;
                this.LeftGridSplitter.Visibility = Visibility.Collapsed;
                if (this.MainControl_Grid.ColumnDefinitions.Count >= 1)
                {
                    this.MainControl_Grid.ColumnDefinitions[0].MinWidth = 0.0;
                    this.MainControl_Grid.ColumnDefinitions[0].Width = GridLength.Auto;
                }
            }
            else if ((this.Run_ClusterPanel.Visibility != Visibility.Visible) || (this.LeftGridSplitter.Visibility != Visibility.Visible))
            {
                this.Run_ClusterPanel.Visibility = Visibility.Visible;
                this.LeftGridSplitter.Visibility = Visibility.Visible;
                if (this.MainControl_Grid.ColumnDefinitions.Count >= 1)
                {
                    this.MainControl_Grid.ColumnDefinitions[0].MinWidth = 15.0;
                    this.MainControl_Grid.ColumnDefinitions[0].Width = new GridLength(15.0, GridUnitType.Star);
                }
            }

            //KAA 2016-05-31 commented
            //this.Run_DOM.StringHeight = this.SettingsObject.DOM_Settings.StringHeight;
            //this.Run_TickPanel.StringHeight = this.SettingsObject.DOM_Settings.StringHeight;
            //this.Run_ClusterPanel.StringHeight = this.SettingsObject.DOM_Settings.StringHeight;
        }

        private void SettingsWinInit()
        {
            if ((this.m_SettingsWindow != null) && (this.SettingsObject != null))
            {
                this.m_SettingsWindow.Buttn_AsksColor.SelectedColor = this.SettingsObject.DOM_Settings.AsksColor;
                this.m_SettingsWindow.Buttn_BidsColor.SelectedColor = this.SettingsObject.DOM_Settings.BidsColor;
                this.m_SettingsWindow.Buttn_VolumesFilledAt.Value = new int?(this.SettingsObject.DOM_Settings.FilledAt);
                this.m_SettingsWindow.Buttn_AutoScroll.Content = this.SettingsObject.DOM_Settings.AutoScroll ? "Да" : "Нет";
                this.m_SettingsWindow.Buttn_RenewSpeed_DOM.Value = new int?(this.SettingsObject.DOM_Settings.RenewSpeed);
                this.m_SettingsWindow.Buttn_StringHeight.Value = new int?(this.SettingsObject.DOM_Settings.StringHeight);
                this.m_SettingsWindow.Buttn_ClustersFilledAt.Value = new int?(this.SettingsObject.ClusterPanel_Settings.FilledAt);
                this.m_SettingsWindow.Buttn_PercentsForColorGradient.Value = new int?(this.SettingsObject.ClusterPanel_Settings.PercentsForColorGradient);
                switch (this.SettingsObject.ClusterPanel_Settings.TimeFrame)
                {
                    case 1:
                        this.m_SettingsWindow.Buttn_ClustersTF.SelectedIndex = 0;
                        break;

                    case 5:
                        this.m_SettingsWindow.Buttn_ClustersTF.SelectedIndex = 1;
                        break;

                    case 10:
                        this.m_SettingsWindow.Buttn_ClustersTF.SelectedIndex = 2;
                        break;

                    case 15:
                        this.m_SettingsWindow.Buttn_ClustersTF.SelectedIndex = 3;
                        break;

                    case 30:
                        this.m_SettingsWindow.Buttn_ClustersTF.SelectedIndex = 4;
                        break;

                    case 60:
                        this.m_SettingsWindow.Buttn_ClustersTF.SelectedIndex = 5;
                        break;

                    case 0x5a0:
                        this.m_SettingsWindow.Buttn_ClustersTF.SelectedIndex = 6;
                        break;
                }
                bool saveClusters = this.SettingsObject.ClusterPanel_Settings.SaveClusters;
                if (saveClusters)
                {
                    this.m_SettingsWindow.Buttn_SaveClusters.Content = "Да";
                }
                if (!saveClusters)
                {
                    this.m_SettingsWindow.Buttn_SaveClusters.Content = "Нет";
                }
                if (!this.SettingsObject.ClusterPanel_Settings.ShowClusterPanel)
                {
                    this.m_SettingsWindow.Buttn_ShowClusterPanel.Content = "Нет";
                }
                else
                {
                    this.m_SettingsWindow.Buttn_ShowClusterPanel.Content = "Да";
                }
                switch (this.SettingsObject.ClusterPanel_Settings.ClusterStyleText)
                {
                    case "Summ":
                        this.m_SettingsWindow.Buttn_ClusterStyleText.SelectedIndex = 0;
                        break;

                    case "Delta1":
                        this.m_SettingsWindow.Buttn_ClusterStyleText.SelectedIndex = 1;
                        break;

                    case "Delta2":
                        this.m_SettingsWindow.Buttn_ClusterStyleText.SelectedIndex = 2;
                        break;
                }
                string clusterStyleColor = this.SettingsObject.ClusterPanel_Settings.ClusterStyleColor;
                if (clusterStyleColor == "WhiteBalance")
                {
                    this.m_SettingsWindow.Buttn_ClusterStyleColor.SelectedIndex = 0;
                }
                else if (clusterStyleColor == "BlackBalance")
                {
                    this.m_SettingsWindow.Buttn_ClusterStyleColor.SelectedIndex = 1;
                }
                else if (clusterStyleColor == "Mix")
                {
                    this.m_SettingsWindow.Buttn_ClusterStyleColor.SelectedIndex = 2;
                }
                clusterStyleColor = this.SettingsObject.ClusterPanel_Settings.VolumeStyleText;
                if (clusterStyleColor == "Summ")
                {
                    this.m_SettingsWindow.Buttn_VertVolumeStyle.SelectedIndex = 0;
                }
                else if (clusterStyleColor == "Delta1")
                {
                    this.m_SettingsWindow.Buttn_VertVolumeStyle.SelectedIndex = 1;
                }
                else if (clusterStyleColor == "Delta2")
                {
                    this.m_SettingsWindow.Buttn_VertVolumeStyle.SelectedIndex = 2;
                }
                this.m_SettingsWindow.Buttn_ShowTicksFrom.Value = new int?(this.SettingsObject.TickPanel_Settings.ShowTicksFrom);
                this.m_SettingsWindow.Buttn_FilterTicksFrom.Value = new int?(this.SettingsObject.TickPanel_Settings.FilterTicksFrom);
                this.m_SettingsWindow.Buttn_TicksWeight.Value = new int?(this.SettingsObject.TickPanel_Settings.TicksWeight);
                this.m_SettingsWindow.Buttn_RenewSpeed_Ticks.Value = new int?(this.SettingsObject.TickPanel_Settings.RenewSpeed);
                clusterStyleColor = this.SettingsObject.TickPanel_Settings.TicksStyle;
                if (clusterStyleColor == "Dots")
                {
                    this.m_SettingsWindow.Buttn_TicksStyle.SelectedIndex = 0;
                }
                else if (clusterStyleColor == "Lines")
                {
                    this.m_SettingsWindow.Buttn_TicksStyle.SelectedIndex = 1;
                }
                else if (clusterStyleColor == "DotsLines")
                {
                    this.m_SettingsWindow.Buttn_TicksStyle.SelectedIndex = 2;
                }
                this.m_SettingsWindow.Buttn_FirstWorkAmount.Value = new int?(this.SettingsObject.Trading_Settings.First_WorkAmount);
                this.m_SettingsWindow.Buttn_SecondWorkAmount.Value = new int?(this.SettingsObject.Trading_Settings.Second_WorkAmount);
                this.m_SettingsWindow.Buttn_ThirdWorkAmount.Value = new int?(this.SettingsObject.Trading_Settings.Third_WorkAmount);
                this.m_SettingsWindow.Buttn_FirstIncreaseAmount.Value = new int?(this.SettingsObject.Trading_Settings.First_IncreaseAmount);
                this.m_SettingsWindow.Buttn_SecondIncreaseAmount.Value = new int?(this.SettingsObject.Trading_Settings.Second_IncreaseAmount);
                this.m_SettingsWindow.Buttn_DecreaseAmount.Value = new int?(this.SettingsObject.Trading_Settings.DecreaseAmount);
                this.m_SettingsWindow.Buttn_MaxPosition.Value = new int?(this.SettingsObject.Trading_Settings.MaxPosition);
                this.m_SettingsWindow.Buttn_BackAmount.Value = new int?(this.SettingsObject.Trading_Settings.BackAmount);
                this.m_SettingsWindow.Buttn_ThrowLimitTo.Value = new int?(this.SettingsObject.Trading_Settings.ThrowLimitTo);
                this.m_SettingsWindow.Buttn_StopLossSteps.Value = new int?(this.SettingsObject.Trading_Settings.StopLoss_Steps);
                this.m_SettingsWindow.Buttn_TakeProfitSteps.Value = new int?(this.SettingsObject.Trading_Settings.TakeProfit_Steps);
                string averagingMethod = this.SettingsObject.Trading_Settings.AveragingMethod;
                if (averagingMethod == "Stack")
                {
                    this.m_SettingsWindow.Buttn_AveragingMethod.SelectedIndex = 1;
                }
                else if (averagingMethod == "Queque")
                {
                    this.m_SettingsWindow.Buttn_AveragingMethod.SelectedIndex = 0;
                }
            }
        }

        private void SetWorkAmounts()
        {
            if (this.m_lWorkAmounts.Count >= 3)
            {
                this.TxtBlck_Amount1.Text = this.m_lWorkAmounts[0].ToString();
                this.TxtBlck_Amount2.Text = this.m_lWorkAmounts[1].ToString();
                this.TxtBlck_Amount3.Text = this.m_lWorkAmounts[2].ToString();
            }
        }

        private void ShowOrderFailed()
        {
            this.m_iElapsedCounter += 100;
            double num = 1.0;
            if (this.m_iElapsedCounter == 100)
            {
                num = 2.0;
            }
            else
            {
                num = 1.0;
            }
            this.Grid_OrderFailedReason.Height = this.Run_DOM.LastClickCoord + (this.TextBlock_OrderFailedReason.ActualHeight * num);
            if (this.m_iElapsedCounter == 100)
            {
                this.TextBlock_OrderFailedReason.Text = this.m_sOrderFailedText;
                this.Grid_OrderFailedReason.Visibility = Visibility.Visible;
                this.TextBlock_OrderFailedReason.Background = Brushes.Red;
                this.TextBlock_OrderFailedReason.Opacity = 0.8;
            }
            else if (this.m_iElapsedCounter == 200)
            {
                this.TextBlock_OrderFailedReason.Background = Brushes.Gray;
            }
            else if (((this.m_iElapsedCounter > 0x6a4) && (this.m_iElapsedCounter <= 0x7d0)) && (this.TextBlock_OrderFailedReason.Opacity >= 0.2))
            {
                this.TextBlock_OrderFailedReason.Opacity -= 0.2;
            }
            else if (this.m_iElapsedCounter > 0x7d0)
            {
                this.Grid_OrderFailedReason.Visibility = Visibility.Hidden;
                this.TextBlock_OrderFailedReason.Text = "";
                this.m_iElapsedCounter = 0;
                this.m_Timer_LayerUnNull.Stop();
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.MainControl_Grid = (Grid) target;
                    return;

                case 2:
                    this.Grid_OrderFailedReason = (Grid) target;
                    return;

                case 3:
                    this.TextBlock_OrderFailedReason = (TextBlock) target;
                    return;

                case 4:
                    this.Run_ClusterPanel = (ClusterPanel) target;
                    return;

                case 5:
                    this.LeftGridSplitter = (GridSplitter) target;
                    return;

                case 6:
                    this.Run_TickPanel = (TickPanel) target;
                    return;

                case 7:
                    this.Run_DOM = (DOM) target;
                    return;

                case 8:
                    this.ToolsGrid = (Grid) target;
                    return;

                case 9:
                    this.Buttn_RemoveChild = (Button) target;
                    this.Buttn_RemoveChild.Click += new RoutedEventHandler(this.Buttn_RemoveChild_Click);
                    return;

                case 10:
                    this.Buttn_Settings = (Button) target;
                    this.Buttn_Settings.Click += new RoutedEventHandler(this.Buttn_Settings_Click);
                    return;

                case 11:
                    this.MouseActiveIcon = (Image) target;
                    return;

                case 12:
                    this.TxtBlck_TimeNow = (TextBlock) target;
                    return;

                case 13:
                    this.TxtBlck_ChangeInstrument = (TextBlock) target;
                    this.TxtBlck_ChangeInstrument.MouseEnter += new MouseEventHandler(this.TxtBlck_ChangeInstrument_MouseEnter);
                    this.TxtBlck_ChangeInstrument.MouseLeave += new MouseEventHandler(this.TxtBlck_ChangeInstrument_MouseLeave);
                    this.TxtBlck_ChangeInstrument.MouseDown += new MouseButtonEventHandler(this.TxtBlck_ChangeInstrument_Click);
                    return;

                case 14:
                    this.WorkAmountsGrid = (Grid) target;
                    return;

                case 15:
                    this.TxtBlck_Amount1 = (TextBlock) target;
                    this.TxtBlck_Amount1.MouseDown += new MouseButtonEventHandler(this.TxtBlck_Amount1_MouseDown);
                    return;

                case 0x10:
                    this.TxtBlck_Amount2 = (TextBlock) target;
                    this.TxtBlck_Amount2.MouseDown += new MouseButtonEventHandler(this.TxtBlck_Amount2_MouseDown);
                    return;

                case 0x11:
                    this.TxtBlck_Amount3 = (TextBlock) target;
                    this.TxtBlck_Amount3.MouseDown += new MouseButtonEventHandler(this.TxtBlck_Amount3_MouseDown);
                    return;

                case 0x12:
                    this.RoundTripGrid = (Grid) target;
                    return;

                case 0x13:
                    this.TxtBlck_RTP1 = (TextBlock) target;
                    return;

                case 20:
                    this.TxtBlck_RTP2 = (TextBlock) target;
                    return;

                case 0x15:
                    this.TxtBlck_RTP3 = (TextBlock) target;
                    return;

                case 0x16:
                    this.PositionGrid = (Grid) target;
                    return;

                case 0x17:
                    this.TxtBlck_Price = (TextBlock) target;
                    return;

                case 0x18:
                    this.TxtBlck_Amount = (TextBlock) target;
                    return;

                case 0x19:
                    this.TxtBlck_Profit = (TextBlock) target;
                    return;
            }
            this._contentLoaded = true;
        }

        public void ToFocus()
        {
            this.Run_DOM.FocusedByClick = true;
        }

        private void TxtBlck_Amount1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((this.ChangeWorkAmountSource != null) && (this.ChangeActiveWorkAmount != null))
            {
                this.ChangeActiveWorkAmount.IndexUsedAmount = 1;
                this.ChangeWorkAmountSource.SendInteraction(this.ChangeActiveWorkAmount);
            }
        }

        private void TxtBlck_Amount2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((this.ChangeWorkAmountSource != null) && (this.ChangeActiveWorkAmount != null))
            {
                this.ChangeActiveWorkAmount.IndexUsedAmount = 2;
                this.ChangeWorkAmountSource.SendInteraction(this.ChangeActiveWorkAmount);
            }
        }

        private void TxtBlck_Amount3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((this.ChangeWorkAmountSource != null) && (this.ChangeActiveWorkAmount != null))
            {
                this.ChangeActiveWorkAmount.IndexUsedAmount = 3;
                this.ChangeWorkAmountSource.SendInteraction(this.ChangeActiveWorkAmount);
            }
        }

        private void TxtBlck_ChangeInstrument_Click(object sender, MouseButtonEventArgs e)
        {
            //this.TryingTo_ChangeInstrument(this);
            
            //if marketviewModel exist 
            if (_tradeOperation!=null)
                _tradeOperation.ChangeInstrument();
           
        }

        private void TxtBlck_ChangeInstrument_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock block = (TextBlock) sender;
            block.Background = block.Foreground;
            block.Foreground = Brushes.WhiteSmoke;
            block.Opacity = 0.8;
        }

        private void TxtBlck_ChangeInstrument_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock block = (TextBlock) sender;
            block.Background = Brushes.Transparent;
            block.Foreground = Brushes.Gray;
            block.Opacity = 1.0;
        }

        private void UnBlockPaint()
        {
            if (Mouse.PrimaryDevice.LeftButton == MouseButtonState.Released)
            {
                this.Run_DOM.BlockPaint = false;
                this.Run_DOM.ChangeOrdersWidth();
                this.Run_TickPanel.BlockPaint = false;
                this.Run_TickPanel.TicksFilled = true;
                this.Run_ClusterPanel.BlockPaint = false;
                this.Run_ClusterPanel.CanRepaintAll();
                this.UnBlockViewModel(false);
                this.m_Timer.Stop();
            }
        }

        private void W_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.Run_DOM.FocusedByClick = false;
        }

        public byte ActiveWorkAmount
        {
            get
            {
                return this.m_ActiveWorkAmount;
            }
            set
            {
                if (value != this.m_ActiveWorkAmount)
                {
                    this.m_ActiveWorkAmount = value;
                    this.Run_DOM.ActiveWorkAmount = this.m_ActiveWorkAmount;
                    Action method = new Action(this.SetActiveWorkAmount);
                    base.Dispatcher.Invoke(DispatcherPriority.Normal, method);
                }
            }
        }

        public TargetContractInfoValue AddedLeader { get; set; }

        public string AssignedContractName
        {
            get
            {
                return this.m_sAssignedContractName;
            }
            set
            {
                this.m_sAssignedContractName = value;
                this.Run_ClusterPanel.AssignedContractName = value;
            }
        }

        public IIntelliThink Bot
        {
            get
            {
                return this.m_Bot;
            }
            set
            {
                this.m_Bot = value;
                this.Run_DOM.Bot = value;
                this.m_Bot.AddIntelliThinkEventsReceiver(this);
            }
        }

        public ChangeWorkAmountInteraction ChangeActiveWorkAmount { get; set; }

        public GraphicDataExternalSource ChangeWorkAmountSource { get; set; }

        public double ClusterPanelWidth
        {
            get
            {
                return this.Run_ClusterPanel.ActualWidth;
            }
            set
            {
                int column = Grid.GetColumn(this.Run_ClusterPanel);
                this.MainControl_Grid.ColumnDefinitions[column].Width = new GridLength(value, GridUnitType.Star);
            }
        }

        public int Decimals
        {
            get
            {
                return this.m_iDecimals;
            }
            set
            {
                this.m_iDecimals = value;
                this.Run_ClusterPanel.Decimals = this.m_iDecimals;
                this.Run_TickPanel.Decimals = this.m_iDecimals;
            }
        }

        public Dictionary<string, string> Dictionary_HotKeys_IDKey { get; set; }

        public double DOMWidth
        {
            get
            {
                return this.Run_DOM.ActualWidth;
            }
            set
            {
                int column = Grid.GetColumn(this.Run_DOM);
                this.MainControl_Grid.ColumnDefinitions[column].Width = new GridLength(value, GridUnitType.Star);
            }
        }

        public long LastRoundTripTime
        {
            get
            {
                return this.m_LastRoundTripTime;
            }
            set
            {
                this.m_LastRoundTripTime = value;
                Action method = new Action(this.SetRoundTrips);
                base.Dispatcher.Invoke(DispatcherPriority.Normal, method);
            }
        }

        public bool MouseMode
        {
            get
            {
                return this.m_bMouseMode;
            }
            set
            {
                this.m_bMouseMode = value;
                this.Run_DOM.MouseMode = value;
                this.Run_TickPanel.MouseMode = value;
                this.Run_DOM.ShowOrderFocus(value);
                Action method = new Action(this.SetMouseMode);
                base.Dispatcher.Invoke(DispatcherPriority.Normal, method);
            }
        }

        public Contract_Settings SettingsObject
        {
            get
            {
                return this.m_SettingsObject;
            }
            set
            {
                this.m_SettingsObject = value;
                this.Run_DOM.SettingsObject = value;
                this.Run_TickPanel.SettingsObject = value;
                this.Run_ClusterPanel.SettingsObject = value;
                if (this.m_SettingsWindow != null)
                {
                    Action action = new Action(this.HideSettingsWindow);
                    base.Dispatcher.Invoke(DispatcherPriority.Input, action);
                }
                Action method = new Action(this.SettingsSharing);
                base.Dispatcher.Invoke(DispatcherPriority.Input, method);
                Action action3 = new Action(this.Run_DOM.SettingsSharing);
                base.Dispatcher.Invoke(DispatcherPriority.Input, action3);
                Action action4 = new Action(this.Run_TickPanel.SettingsSharing);
                base.Dispatcher.Invoke(DispatcherPriority.Input, action4);
                Action action5 = new Action(this.Run_ClusterPanel.SettingsSharing);
                base.Dispatcher.Invoke(DispatcherPriority.Input, action5);
            }
        }

        public SettingsWindow SettingsWin
        {
            get
            {
                return this.m_SettingsWindow;
            }
        }

        //KAA 2016-04-22

        public double Step
        {
            get
            {
                return this.m_dStep;
            }
            set
            {
                this.m_dStep = value;
               
              //  this.Run_TickPanel.Step = this.m_dStep;
            }
        }
        
        public double TickPanelWidth
        {
            get
            {
                return this.Run_TickPanel.ActualWidth;
            }
            set
            {
                int column = Grid.GetColumn(this.Run_TickPanel);
                this.MainControl_Grid.ColumnDefinitions[column].Width = new GridLength(value, GridUnitType.Star);
            }
        }

        public Tick_Info Ticks
        {
            get
            {
                return this.m_Ticks;
            }
            set
            {
                this.Run_TickPanel.Ticks = value;
             //   this.Run_ClusterPanel.Ticks = value;
            }
        }


        public Tick_Info  TickRcvr
        {
            get
            {
                return (Tick_Info)base.GetValue(TicksProperty);
            }
            set
            {
                base.SetValue(TicksProperty, value);
            }
        }








        public List<ModuleProperty> TradingSettingsList { get; set; }

        public List<int> WorkAmounts
        {
            get
            {
                return this.m_lWorkAmounts;
            }
            set
            {
                if (value.Count != this.m_lWorkAmounts.Count)
                {
                    this.m_lWorkAmounts = value;
                    Action method = new Action(this.SetWorkAmounts);
                    base.Dispatcher.Invoke(DispatcherPriority.Normal, method);
                }
                else if (((this.m_lWorkAmounts[0] != value[0]) || (this.m_lWorkAmounts[1] != value[1])) || (this.m_lWorkAmounts[2] != value[2]))
                {
                    this.m_lWorkAmounts = value;
                    Action action2 = new Action(this.SetWorkAmounts);
                    base.Dispatcher.Invoke(DispatcherPriority.Normal, action2);
                }
            }
        }

        public string WorkingDirectory
        {
            get
            {
                return this.m_sWorkingDirectory;
            }
            set
            {
                this.m_sWorkingDirectory = value;
                this.Run_ClusterPanel.WorkingDirectory = this.WorkingDirectory;
            }
        }
    }
}

