using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xml;

using Common;
using Common.Utils;

using TradingLib.Enums;

using Terminal.Interfaces;
using Terminal.TradingStructs;
using Terminal.Events;
using Terminal.Views;
using Terminal.Views.ChildWindows;

using Terminal.ViewModels;

using Terminal.Controls.Market.Settings;


namespace Terminal.Controls.Market
{
    /// <summary>
    /// Логика взаимодействия для ControlMarketInstrument.xaml
    /// </summary>
    public partial class ControlMarket : UserControl, IStockNumerable
    {


       
       
        private bool _modeMouse = true;//KAA tempo
      
        private double _step = 1.0;
        private int _decimals;   

       
    
      
  
      
       
		//TODO remove
        private System.Timers.Timer _timerCheckCoord = new System.Timers.Timer(60000);
     
      
     

        public event Action DoAllFocused;

          


        //public static readonly DependencyProperty TicksProperty = DependencyProperty.Register("Ticks", typeof(CDeal), typeof(ControlMarket));

        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(double), typeof(ControlMarket), new UIPropertyMetadata(0.0));


        //for window movement control
        private Point _pntPrev = new Point();
        private bool _isInModeMoveWindow = false;

        private MainWindow _mainWindow;

        private CKernelTerminal _kernelTerminal;


        private GridLength _widthSaved;


        private double _widthDealsSavedPxls;

       // private ITradeOperations _tradeOperation;




        // public int StockNum { get; set; }



        public int StockNum
        {
            get { return (int)GetValue(StockNumProperty); }
            set { SetValue(StockNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StockNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StockNumProperty =
            DependencyProperty.Register("StockNum", typeof(int), typeof(ControlMarket), new UIPropertyMetadata(0));









        public CSelectionMode SelectionMode
        {
            get { return (CSelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionModeProperty =
            DependencyProperty.Register("SelectionMode", typeof(CSelectionMode), typeof(ControlMarket), new UIPropertyMetadata(new CSelectionMode()));




		public bool IsModeKeyboardTrading
		{
			get { return (bool)GetValue(IsModeKeyboardTradingProperty); }
			set { SetValue(IsModeKeyboardTradingProperty, value); }
		}

		// Using a DependencyProperty as the backing store for IsModeKeyboardTrading.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IsModeKeyboardTradingProperty =
			DependencyProperty.Register("IsModeKeyboardTrading", typeof(bool), typeof(ControlMarket), new UIPropertyMetadata(false));



		

		





        private bool _isMaximized = false;
        

        private WindowMenu _windowMenu;
        private Dispatcher _mainWinDispatcher;
        



        private List<string> _dpList = new List<string>();


        public double ClusterPanelWidth
        {
            get
            {
                return ControlClustersInstance.ActualWidth;
            }
            set
            {
                int column = Grid.GetColumn(ControlClustersInstance);
                GridControlMarket.ColumnDefinitions[column].Width = new GridLength(value, GridUnitType.Star);
            }
        }

        public int Decimals
        {
            get
            {
                return _decimals;
            }
            set
            {
                _decimals = value;
                ControlClustersInstance.Decimals = _decimals;
                ControlClustersInstance.Decimals = _decimals;
            }
        }

      
      

        public bool MouseMode
        {
            get
            {
                return _modeMouse;
            }
            set
            {
                _modeMouse = value;
                ControlStockInstance.MouseMode = value;
                ControlDealsInstance.MouseMode = value;
                ControlStockInstance.ShowOrderFocus(value);
                Action method = new Action(SetMouseMode);
                base.Dispatcher.Invoke(DispatcherPriority.Normal, method);
            }
        }

      

     


        //KAA 2016-04-22

        public double Step
        {
            get
            {
                return _step;
            }
            set
            {
                _step = value;

            }
        }

      


        public Brush FontColor
        {
            get { return (Brush)GetValue(FontColorProperty); }
            set { SetValue(FontColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FontColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontColorProperty =
            DependencyProperty.Register("FontColor", typeof(Brush), typeof(ControlMarket), new UIPropertyMetadata(Brushes.Black));



        private bool _isMouseInZone = false;

       




        public ControlMarket(int stockNum, MainWindow mainWindow)
        {
            InitializeComponent();
            //StockNum = stockNum;

            _mainWindow = mainWindow;

            _kernelTerminal = CKernelTerminal.GetKernelTerminalInstance();

            

            StockNum = stockNum;
            Loaded += new RoutedEventHandler(ControlMarket_Loaded);
            ControlStockInstance.PriceCoordChanged += new EventHandler(OnPriceCoordChanged);
            ControlStockInstance.PriceCoordChanged += new EventHandler(ControlDealsInstance.OnPriceCoordChanged);
            ControlStockInstance.PriceCoordChanged += new EventHandler(ControlClustersInstance.OnPriceCoordChanged);


            //note: subscribe ControlMarket to MouseWheel event of ControlStock
            ControlStockInstance.DoScrollAllControls += new EventHandler(OnControllStockScrolled);
          /*  if (_modeMouse)
            {
                MouseActiveIcon.Visibility = Visibility.Visible;
            }
            else if (!_modeMouse)
            {
                MouseActiveIcon.Visibility = Visibility.Collapsed;
            }
            */
            ControlStockInstance.PositionGrid = PositionGrid;
          
           
        
        
            ControlDealsInstance.LstLevel2 = ControlStockInstance.TenLevels;
            ControlDealsInstance.LstLevel1 = ControlStockInstance.FiftyLevels;
			ControlDealsInstance.DictPriceY = ControlStockInstance.DictPriceY;
			ControlClustersInstance.DictPriceY = ControlStockInstance.DictPriceY;

            ControlClustersInstance.GetAvailableSpaceForScrollViewer = GetFreeSpaceForControlCluster;


            _timerCheckCoord.Elapsed += new ElapsedEventHandler(TimerCheckCoord_Elapsed);
            _timerCheckCoord.Start();
         
            
  
            //KAA onfocus event
            DoAllFocused += ToFocus;
          

            PreviewKeyDown += new KeyEventHandler(ControlMarket_PreviewKeyDown);
            PreviewKeyUp += new KeyEventHandler(ControlMarket_PreviewKeyUp);
            //_mainWindow = (MainWindow)CUtilWin.FindWindow<MainWindow>();
            _mainWinDispatcher = Dispatcher.CurrentDispatcher;

            GenDPList();

            MouseDown += new MouseButtonEventHandler(ControlMarket_MouseDown);

          
        }

       

        void ControlMarket_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {

                Point pnt = e.GetPosition(ControlClustersInstance);

                double delta = 10;

                if (pnt.X > 0 /*ControlClustersInstance.ActualWidth - delta*/ &&
                    pnt.X < ControlClustersInstance.ActualWidth /* + delta*/ &&
                    pnt.Y > 0 &&
                    pnt.Y < ControlClustersInstance.ActualHeight)
                {

                    if (ControlDealsInstance.Visibility == Visibility.Visible)
                        CollapseDealsInstance();
                    else if (ControlDealsInstance.Visibility == Visibility.Collapsed)
                    {
                        UnCollapseDealsInstance();
                        ControlClustersInstance.ScrollViewerClusters.ScrollToRightEnd();
                    }
                }

            }
        }

     

        private void GenDPList()
        {
            var props = Type.GetType("Terminal.DataBinding.CTerminalCommonProperties").GetProperties();
            props.ToList().ForEach(property => _dpList.Add(property.Name));
        }


        //private List<string> _tmp = new List<string>();

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {

           // _tmp.Add(e.Property.Name.ToString());
            if (e.Property.Name == "StockClock")
            {
                Thread.Sleep(0);
               
            }

            if (e.Property.Name == "Background")
            {
                if (TextBlockChangeInstrument!=null)
                    TextBlockChangeInstrument.Background = (Brush)e.NewValue;

                 if (BorderEmpty!=null)
                     BorderEmpty.Background = (Brush)e.NewValue;

            }

         


            base.OnPropertyChanged(e);

        }





      

        public void ButtonDeleteStock_Click(object sender, RoutedEventArgs e)
        {
            //this.TryingTo_RemoveInstrument(this);

            //_tradeOperation.DeleteInstrument();
            //_tradeOperation.DeleteInstrument();
            ExecuteCommand(EventsViewModel.CmdDeleteInstrument);


        }


   

    

        public void TriggerCenteringStock()
        {
            ControlStockInstance.FocusAsk = 0;
        }


        /// <summary>
        /// When ControlStock's MouseWheel event raises
        /// this Event raises as well (it was subscribed to Controlstock's event).
        /// This event set delta to another two controls: ControlClusters and ControlDeals.
        /// These controls using ScrollDelta value recalc coordinates.           
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnControllStockScrolled(object sender, EventArgs e)
        {
            ControlDealsInstance.ScrollDelta = ControlStockInstance.ScrollDelta;
            ControlClustersInstance.ScrollDelta = ControlStockInstance.ScrollDelta;
        }

        public void DisableDeleteStock()
        {
            ButtonDeleteStock.IsEnabled = false;
            ButtonDeleteStock.Visibility = System.Windows.Visibility.Hidden;

        }

        public void EnableDeleteStock()
        {
            ButtonDeleteStock.IsEnabled = true;
            ButtonDeleteStock.Visibility = System.Windows.Visibility.Visible;

        }



             

        private void TimerCheckCoord_Elapsed(object sender, ElapsedEventArgs e)
        {
            ControlClustersInstance.ScrollDelta = ControlStockInstance.ScrollDelta;
            SaveClusters();
        }

      

      

        private void ControlMarket_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            if (window != null)
            {
                window.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.W_LostKeyboardFocus);
             
                PreviewMouseDown += new MouseButtonEventHandler(ControlMarket_PreviewMouseDown);
            }
            ControlStockInstance.GridControlMarket = GridControlMarket;
            ControlClustersInstance.GridControlMarket = GridControlMarket;
            DrawMaximizeButton();
            DrawOffButtonWinControl();
            //DrawTopMostButton();
			DrawEmpyTopMostButton();
        }

        private void ControlMarket_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!base.IsKeyboardFocused)
                {
                    DoAllFocused();
                }

                
            }
            catch (Exception exc)
            {

                CKernelTerminal.ErrorStatic("ControlMarket_PreviewMouseDown", exc);
            }

        }
     
        private void CollapseDealsInstance()
        {
            //changed 2018-05-08
            int indColClusters = 0;
            int indColCtrolDeals = 2;


            ControlDealsInstance.Visibility = Visibility.Collapsed;
            ToolsGrid.Visibility = Visibility.Collapsed;
         
           
            _widthSaved = GridControlMarket.ColumnDefinitions[indColCtrolDeals].Width;
            _widthDealsSavedPxls = GridControlMarket.ColumnDefinitions[indColCtrolDeals].ActualWidth;

            GridControlMarket.ColumnDefinitions[indColCtrolDeals].Width = GridLength.Auto;

            GridControlMarket.ColumnDefinitions[indColCtrolDeals].MinWidth = 0;

            double _widthActGridClust = GridControlMarket.ColumnDefinitions[indColClusters].ActualWidth;
           
            GridControlMarket.ColumnDefinitions[indColClusters].Width = new GridLength(_widthActGridClust + _widthDealsSavedPxls);
           
        }

        private void UnCollapseDealsInstance()
        {
            //changed 2018-05-08
            int indColClusters = 0;
            int indColCtrolDeals = 2;

            ControlDealsInstance.Visibility = Visibility.Visible;
            ToolsGrid.Visibility = Visibility.Visible;


            //GridControlMarket.ColumnDefinitions[indColCtrolDeals].Width = new GridLength(1, GridUnitType.Star);

            //restore and than set *
            GridControlMarket.ColumnDefinitions[indColCtrolDeals].Width = new GridLength(_widthDealsSavedPxls);   // _widthSaved;
            GridControlMarket.ColumnDefinitions[indColCtrolDeals].MinWidth = 15;
            GridControlMarket.ColumnDefinitions[indColCtrolDeals].Width = new GridLength(1, GridUnitType.Star);



            double _widthActGridClust = GridControlMarket.ColumnDefinitions[indColClusters].ActualWidth;
            GridControlMarket.ColumnDefinitions[indColClusters].Width = new GridLength(_widthActGridClust - _widthDealsSavedPxls);


        }

        private void DrawMaximizeButton()
        {
            ButtonMinimizeNormalizePath.Data = Geometry.Parse("M0,0 H8 V7 H0 V0 M0,1 H8");
        }

        private void DrawNormilizeButton()
        {
            ButtonMinimizeNormalizePath.Data = Geometry.Parse("M0,4 V8 H5 V3 H0 M2,4 V1 H7 V6 H4");
           
        }

        private void DrawOnButtonWinControl()
        {

            PathButtonShowButtonsWinControl.Data = Geometry.Parse("M 6,1 L 10,8 L 0,8 L 5,-1");

        }

        private void DrawOffButtonWinControl()
        {

            PathButtonShowButtonsWinControl.Data = Geometry.Parse("M 5,10 L 10,2 L 0,2 L 5,10");

        }


        private void DrawTopMostButton()
        {

            PathButtonTopMost.Data = Geometry.Parse("M1,3 L2,8 M 2,8 L8,0");

            //PathButtonTopMost.Data = Geometry.Parse("M0,0 M 1,1");
        }

		private void DrawEmpyTopMostButton()
		{

			PathButtonTopMost.Data = Geometry.Parse("");
		}


        public void ButtonMaximizeNmormilize_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {


            if (_isMaximized)
           {
               
               
               DrawMaximizeButton();
                _isMaximized = !_isMaximized;
            }
            else
            {
                DrawNormilizeButton();
                _isMaximized = !_isMaximized;

            }

        }


        private void ExecuteCommand(RoutedUICommand cmd, object data=null)
        {
            cmd.Execute(data, this);

        }

      /*  private bool IsMouseInControlMarket()
        {

           Point pnt =  Mouse.GetPosition(this);

           if (pnt.X > 0 && pnt.Y > 0)
               return true;

            return false;
        }
        */
        private bool IsMouseInControlStock()
        {

            Point pnt = Mouse.GetPosition(ControlStockInstance);

            if (pnt.X > 0 && pnt.Y > 0 &&
                pnt.X< ControlDealsInstance.ActualWidth && pnt.Y <ControlDealsInstance.ActualHeight)
                return true;

            return false;
        }

        private bool IsMouseInControlMarket()
        {
            Point pnt = Mouse.GetPosition(this);

            if (pnt.X > 0 && pnt.Y > 0 &&
                pnt.X < this.ActualWidth && pnt.Y < this.ActualHeight)
                return true;


            return false;
        }



        private void SendThrowOrder(Key key)
		{
			if (IsModeKeyboardTrading)
			{

				EnmOrderDir dir = (key == Key.Up) ? EnmOrderDir.Buy : EnmOrderDir.Sell;
			
				ExecuteCommand(EventsViewModel.CmdSendOrderThrow,dir);
			}

		}


		private void InvertPosition()
		{
			if (IsModeKeyboardTrading)
			{
				ExecuteCommand(EventsViewModel.CmdInvertPosition);
			}
		}

        


        public void ControlMarket_PreviewKeyDown(object sender, KeyEventArgs e)
        {

                  
            var t = sender.GetType();
            //TODO normal
            if (t.Name != "MainWindow")
                return;
            //Thread.Sleep(0);

            //ControlStockInstance.LastClickCoord = 3.0;

            //TODO get key's list from config
            if (e.Key == Key.RightCtrl || e.Key == Key.LeftCtrl)
            {
                ExecuteCommand(EventsViewModel.CmdCancellAllOrders);
                ExecuteCommand(EventsViewModel.CmdCloseAllPositions);
            }
            else if (e.Key == Key.NumPad0)
                InvertPosition();
            else if (e.Key == Key.Up || e.Key == Key.Down)
                SendThrowOrder(e.Key);
            else if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                TriggerCenteringStock();

            else if (e.Key == Key.Space)
                ExecuteCommand(EventsViewModel.CmdCancellAllOrders);

            else if (e.Key == Key.L && IsMouseInControlStock())
            {
                SelectionMode.IsModeDrawLevel = true;
                ControlStockInstance.IsNeedRepaintDeals = true;
            }
            else if (e.Key == Key.Z && IsMouseInControlStock())
            {
                SelectionMode.IsModeStopLossTakeProfit = true;
                ControlStockInstance.IsNeedRepaintDeals = true;
            }
            else if (e.Key == Key.Q && IsMouseInControlStock())
            {
                SelectionMode.IsModeStopLossInvert = true;
                ControlStockInstance.IsNeedRepaintDeals = true;
            }
            else if (e.Key == Key.S && IsMouseInControlStock())
            {
                SelectionMode.IsModeStopOrder = true;
                ControlStockInstance.IsNeedRepaintDeals = true;
            }
            else if (((e.SystemKey == Key.LeftAlt) || e.SystemKey == Key.RightAlt)
                && IsMouseInControlStock())
            {
                SelectionMode.IsModeRestOrder = true;
                ControlStockInstance.IsNeedRepaintDeals = true;
            }
            else if (e.Key == Key.Add && IsMouseInControlMarket())
                ExecuteCommand(EventsViewModel.CmdIncreaseMinStep);

            else if (e.Key == Key.Subtract && IsMouseInControlMarket())
                ExecuteCommand(EventsViewModel.CmdDecreaseMinStep);
           

            ControlDealsInstance.UpdateLevelsAndCondOrders();
            

        }




        public void ControlMarket_PreviewKeyUp(object sender, KeyEventArgs e)
        {




            if (e.Key == Key.L && IsMouseInControlStock())
            {
                SelectionMode.IsModeDrawLevel = false;
                ControlStockInstance.IsNeedRepaintDeals = true;
            }
            else if (e.Key == Key.Z && IsMouseInControlStock())
            {
                SelectionMode.IsModeStopLossTakeProfit = false;
                ControlStockInstance.IsNeedRepaintDeals = true;
            }
            else if (e.Key == Key.Q && IsMouseInControlStock())
            {
                SelectionMode.IsModeStopLossInvert = false;
                ControlStockInstance.IsNeedRepaintDeals = true;
            }
            else if (e.Key == Key.S && IsMouseInControlStock())
            {
                SelectionMode.IsModeStopOrder = false;
                ControlStockInstance.IsNeedRepaintDeals = true;
            }
            else if (((e.SystemKey == Key.LeftAlt) || e.SystemKey == Key.RightAlt)
                        && IsMouseInControlStock())
                SelectionMode.IsModeRestOrder = false;
            {
                ControlDealsInstance.UpdateLevelsAndCondOrders();
                ControlStockInstance.IsNeedRepaintDeals = true;
            }
        }




      

        private void OnPriceCoordChanged(object sender, EventArgs e)
        {


           


        }

        public void SaveClusters()
        {
            
        }

     

            

        private void SetMouseMode()
        {
          /*  if (_modeMouse)
            {
                MouseActiveIcon.Visibility = Visibility.Visible;
            }
            else if (!_modeMouse)
            {
                MouseActiveIcon.Visibility = Visibility.Collapsed;
            }*/
        }

       

      

                      
       

        public void ToFocus()
        {
            ControlStockInstance.FocusedByClick = true;
        }




        private void ChangeIstrument()
        {
           





        }

        private void TxtBlck_ChangeInstrument_Click(object sender, MouseButtonEventArgs e)
        {

      
           

            Point locationFromScreen = PointToScreen(new Point(0, 0));
            PresentationSource source = PresentationSource.FromVisual(this);
            System.Windows.Point targetPoints = source.CompositionTarget.TransformFromDevice.Transform(locationFromScreen);
            // if (_tradeOperation != null)
              //   _tradeOperation.ShowChangeInstrumentWindow(targetPoints);

            ExecuteCommand(EventsViewModel.CmdShowChangeInstrumentWindow, targetPoints);


        }

        private void TxtBlck_ChangeInstrument_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock block = (TextBlock)sender;

            //TODO color from settings

            block.Background = Brushes.Blue;
            block.Foreground = Brushes.WhiteSmoke;  //Brushes.WhiteSmoke;
            block.FontWeight = FontWeights.Bold;
            block.Opacity = 0.8;
        }

        private void TxtBlck_ChangeInstrument_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock block = (TextBlock)sender;
            block.Background = Background;
            block.Foreground = Foreground; 
            block.FontWeight = FontWeights.Normal;
            block.Opacity = 1.0;
        }


        private void W_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ControlStockInstance.FocusedByClick = false;
        }


        /// <summary>
        /// Moving main window here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMove_PreviewMouseMove(object sender, MouseEventArgs e)
        {

            if (!_isInModeMoveWindow)
                return;

            Point pnt = e.GetPosition(_mainWindow);

            Point pntScreen = PointToScreen(pnt);


            const double parMinDeltaX = 1;
            const double parMinDeltaY = 1;


            if (_isInModeMoveWindow)
                if ((Math.Abs(pntScreen.X - _pntPrev.X) > parMinDeltaX) ||
                    Math.Abs(pntScreen.Y - _pntPrev.Y) > parMinDeltaY)
                {
                    if (_pntPrev.X != 0 && _pntPrev.Y != 0)
                    {

                        double leftOld = _mainWindow.Left;
                        _mainWindow.Left += pntScreen.X - _pntPrev.X;
                        _mainWindow.Top += pntScreen.Y - _pntPrev.Y;

                    }
                    _pntPrev = pntScreen;
                }


           

        }

        private void ButtonMove_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _isInModeMoveWindow = true;
            Mouse.Capture(ButtonMove);
            
        }

        private void ButtonMove_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            _isInModeMoveWindow = false;
            _pntPrev = new Point();
            Mouse.Capture(null);
        }

     


     

       
        
        private void TradeButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Button button = (Button) sender;
            Point coord = button.TransformToAncestor(_mainWindow).Transform(new Point(0,0));
        

            double left = _mainWindow.Left + coord.X;
            double top = _mainWindow.Top + coord.Y+button.Height;


            
            
           // CKernelTerminal.GetViewModelDispatcherInstance().OpenChildWindow


            _mainWindow.Topmost = false;

            Thread thread = new Thread(() =>
                {

                    _windowMenu = new WindowMenu(_mainWindow);
                    _windowMenu.GUIDispMainWindow = _mainWinDispatcher;
                    _windowMenu.Left = left;
                    _windowMenu.Top = top;                    
                    //_windowMenu.
                    Window win = (Window)_windowMenu;
                    _windowMenu.Show();
                    System.Windows.Threading.Dispatcher.Run();

                }

            );

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

         //   (new Thread(ThreadTrackMouse)).Start();


            //CUtilWin.ShowActivated(ref win);
            //win.Focus();
          // wm.Show();

        }


        public void HideWindowControlPannels()
        {

            GridWindowControlButtons.Visibility = Visibility.Collapsed;
            GridClock.Visibility = Visibility.Collapsed;

        }

        public void ShowWindowControlPannels()
        {

            GridWindowControlButtons.Visibility = Visibility.Visible;
            GridClock.Visibility = Visibility.Visible;

        }


        private void ButtonShowButtonsWinControl_Click(object sender, RoutedEventArgs e)
        {
            if (GridWindowControlButtons.Visibility == Visibility.Visible)
            {
                GridWindowControlButtons.Visibility = Visibility.Collapsed;
                DrawOffButtonWinControl();
                ButtonShowButtonsWinControl.ToolTip = "Скрыть панель управления окном";
            }
            else if (GridWindowControlButtons.Visibility == Visibility.Collapsed)
            {
                GridWindowControlButtons.Visibility = Visibility.Visible;
                DrawOnButtonWinControl();
                ButtonShowButtonsWinControl.ToolTip = "Показать панель управления окном";

            }
        }



        private void ButtonConnection_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

			



            
            //Window winConn =   CKernelTerminal.OpenChildWindow<ConnectionsWindow>();

			Window winConn = CKernelTerminal.GetViewModelDispatcherInstance().
				OpenChildWindow<ConnectionsWindow>(showAtStartup:false);

			if (winConn != null)
			{
				_mainWindow.SuspendTopMost(winConn);
				winConn.ShowDialog();
			}
			
        }

        private void ButtonAddStock_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {            
            if (_mainWindow != null)
            {
                _mainWindow.AddStockFromButton();
            }


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonMarketSettings_Click(object sender, RoutedEventArgs e)
        {
            MarketSettingsWindow msw = new MarketSettingsWindow(StockNum);

            
            msw.DataContext = this.DataContext;
            msw.SetComboboxTimeFrame(ControlClustersInstance.TimeFrame);


            _mainWindow.SuspendTopMost(msw);    
            CUtilWin.ShowDialogOnCenter(windowToShow: msw, windowRoot: _mainWindow);
         
        }



        private void ButtonTerminalSettings_Click(object sender, RoutedEventArgs e)
        {
            Window winSet = (Window)new SettingsTerminalWindow();


            CViewModelDispatcher.BindTerminalViewModel(winSet);


            
            Point pnt =  ButtonTerminalSettings.TransformToAncestor(_mainWindow).Transform(new Point(0, 0));

            double xOffset = 10;
            double yOffset = 30;

            winSet.Left = _mainWindow.Left + pnt.X - winSet.Width - xOffset;
            winSet.Top = _mainWindow.Top + pnt.Y + yOffset;

            //winSet.Left =  thi

            _mainWindow.SuspendTopMost(winSet);
            
           // winSet.ShowDialog();
            CUtilWin.ShowDialogOnCenter(winSet, _mainWindow);
            


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

          //  TextBlockChangeInstrument.Background = Background; 


        }

        private void ControlMarket_MouseLeave(object sender, MouseEventArgs e)
        {
          //  SelectionMode.ResetAllModes();

           /* AllertWindow win = new AllertWindow("Mouse leave");


            win.Topmost = true;

            Window wn = (Window)win;


            CUtilWin.ShowActivated(ref wn);
            */

          //  ControlDealsInstance.IsNeedRepaintDeals = true;
            
        }

		private void ButtonTopMost_Click(object sender, RoutedEventArgs e)
		{
			if (_mainWindow.IsModeTopWindow)
			{
				DrawEmpyTopMostButton();
				_mainWindow.Topmost = false;
			}
			else
			{
				DrawTopMostButton();
				_mainWindow.Topmost = true;
			}

			_mainWindow.IsModeTopWindow = !_mainWindow.IsModeTopWindow;
		}

        private void StockClock_TargetUpdated(object sender, DataTransferEventArgs e)
        {
           // Thread.Sleep(0);
        }

		private void ButtonKeyboardTrade_Click(object sender, RoutedEventArgs e)
		{
			ExecuteCommand(EventsViewModel.CmdSetKeyboardTrading);
		}

        double _prevWidth;

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //2018-05-08
            //If ControlDeals is collapsed we have a problem: when user
            //tries to change ControlMarket width, width of stock column doesn't
            //change so we need to change it manually
           if (ControlDealsInstance.Visibility == Visibility.Collapsed)
           {
                double dWidth = this.ActualWidth - _prevWidth;
                int indColClust = 0; 

                double actClustWidth = GridControlMarket.ColumnDefinitions[indColClust].ActualWidth;
                GridControlMarket.ColumnDefinitions[indColClust].Width = new GridLength(actClustWidth + dWidth);

           }
            _prevWidth = this.ActualWidth;
        }



        public double GetFreeSpaceForControlCluster(ref bool isValid)
        {

            if (ActualWidth > 0 &&
               ControlStockInstance.ActualWidth > 0 &&
               ControlDealsInstance.ActualWidth >= 0)
            {
               
                isValid = true;

                //2017-11-20 changed to fix negaive free space problem
                double freeSpace = ActualWidth - ControlStockInstance.ActualWidth - ControlDealsInstance.ActualWidth -2;
                if (freeSpace  < 0)                                    
                    isValid = false;


                return freeSpace;
                //return ActualWidth - ControlStockInstance.ActualWidth - ControlDealsInstance.ActualWidth;
            }
            else
            {
                isValid = false;
                return 0;
            }

           
        }

      

    

    }
}
