using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using System.Windows.Threading;

using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Interop;

using Common;
using Common.Interfaces;
using Common.Utils;

using Terminal.ViewModels;

using Terminal.Controls;
using Terminal.Controls.Market;
using Terminal.Common;
//using Terminal.Graphics;


using Terminal.Views;
using Terminal.Views.ChildWindows;
using Terminal.Conf;
using Terminal.Events;

using Terminal.Events.Data;
using TCPLib;


namespace Terminal.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseTerminalWindow, IGUIDispatcherable, IAlarmable
    {

       // List<Tick_Info> _arrTickSim = new List<Tick_Info>();

       

        //TO DO normal view model and etc
       
        CAlarmer Alarmer { get; set; }
        public Dispatcher GUIDispatcher { get; set; }
        object _lckForceRender = new object();
        private TerminalViewModel _terminalViewModel;

        private Border _borderWindow;

        public bool IsModeTopWindow { get; set; }

        private VersionWindow _windowVersion;



	




        public MainWindow()
        {
                           
            InitializeComponent();
            SetAlarmBoxVisibility();

            CUtil.TaskStart(TaskPreloadVisibility);
            


            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            this.ContentRendered += new EventHandler(MainWindow_ContentRendered);

            IsModeTopWindow = false;                                                        
            string version = CUtil.GetVersion();      
            this.Title = "Terminal "+version;
                 
            GUIDispatcher = Dispatcher.CurrentDispatcher;
            KernelTerminal.GUIDispatcher = GUIDispatcher;
           
            KernelTerminal.CreateAlarmer(this, ComboboxAlarm.AlarmComboBox, AlarmList_CollectionChanged);
            Alarmer = KernelTerminal.Alarmer;



            _terminalViewModel = KernelTerminal.ViewModelDispatcher.TerminalViewModel;
			KernelTerminal.SetDataFromConfigToTerminalViewModel();

            AddAllStocksFromConfig();
            Deactivated += new EventHandler(MainWindow_Deactivated);
            Activated += new EventHandler(MainWindow_Activated);
           // GridMarket.PreviewMouseMove += new MouseEventHandler(GridMarket_PreviewMouseMove);
			
            PreviewMouseDown += new MouseButtonEventHandler(MainWindow_PreviewMouseDown);
            PreviewMouseUp += new MouseButtonEventHandler(MainWindow_PreviewMouseUp);
           
        }

     

        /// <summary>
        ///Special hack. Some time, on some computers
        ///after startpicture doesn't redraws properly     
        /// So make little delay 
        /// </summary>
        public void TaskPreloadVisibility()
        {

            // Thread.Sleep(100);

            Dispatcher.Invoke(new Action(
                () =>
                     this.Visibility = System.Windows.Visibility.Visible
                 ));
        }









        void MainWindow_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
         
        }

        void MainWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           
               
        }


        /// <summary>
        /// For normal showing of child dialogue window
        /// when main window is in topmost mode 
        /// temporary "suspend" topmost.
        /// 
        /// 
        /// </summary>    
        public void SuspendTopMost(Window win)
        {
            win.Closed += ChildWindow_Closed;
            Topmost = false; //anyway no depend if IsModeTopWindow set or not
            

        }




        /// <summary>
        /// Call from child window whilch was finished
        /// Need for resume (bring back topmost flag)
        /// </summary>       
        public void ChildWindow_Closed(object sender, EventArgs e)
        {
                ResumeTopMost();
        }


        /// <summary>
        /// if window need be topmost
        /// bring it back.
        /// Call when child window finish it's work
        /// </summary>
        public void ResumeTopMost()
        {
            if (IsModeTopWindow && !Topmost)
                Topmost = true;
        }


       

        private void MainWindow_Deactivated(object sender, EventArgs e)
        {

          
            this.IsEnabled = false;
			/*if (IsModeTopWindow)
			{
				Topmost = true;
				return;

			}
            */
            foreach (var v in KernelTerminal.ViewDispatcher.LstControlMarket)
            {
                v.GridWindowControlButtons.Opacity = 0.5;
                v.GridClock.Opacity = 0.5;
                //v.ControlStockInstance.Opacity = 0.90;
               
                v.BorderMarket.Opacity = 0.5;
                v.BorderMarket.BorderThickness = new Thickness(0);
                v.ButtonDeleteStock.Opacity = 0.5;
                v.ButtonMarketSettings.Opacity = 0.5;
				v.ButtonKeyboardTrade.Opacity = 0.5;
              

            }
            SetBorderWindowUnActive();
             

            
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {

            IsEnabled = true;

            foreach (var v in KernelTerminal.ViewDispatcher.LstControlMarket)
            {
                v.GridWindowControlButtons.Opacity = 1.0;
                v.GridClock.Opacity = 1.0;
                v.ControlStockInstance.Opacity = 1.0;
                v.BorderMarket.Opacity = 1.0;
                //v.BorderMarket.BorderThickness = new Thickness(1);
                v.ButtonDeleteStock.Opacity = 1.0;
                v.ButtonMarketSettings.Opacity = 1.0;
				v.ButtonKeyboardTrade.Opacity = 1.0;
               
            }

            //SetBorderWindowActive();

        }

        private void SetBorderWindowActive()
        {
            if (_borderWindow != null)
            {
                _borderWindow.BorderThickness = new Thickness(2, 0, 0, 0);
                
            }
        }

        private void SetBorderWindowUnActive()
        {
            if (_borderWindow != null)
            {
                //_borderWindow.Opacity = 0.9;
                _borderWindow.BorderThickness = new Thickness(0, 0, 0, 0);
                
            }
        }




       



        private void BindTerminalProperties()
        {
            this.DataContext = _terminalViewModel;

            

        }







        private void SetAlarmBoxVisibility()
        {

            if (KernelTerminal.TerminalConfig.ShowAlarmBox)
                ComboboxAlarm.Visibility = Visibility.Visible;
            else
                ComboboxAlarm.Visibility = Visibility.Collapsed;

        }


        
        /// <summary>
		/// 1) Call KernelTerminal.AddAllStocksFromConfig,
		/// to create ControlMarkets and MarketViewModels
		/// after creating each one calling CreateStockCell callback perfoms
		/// 2) After creating manage Stock elements: WindowControlPannel and buttons,
		/// control width of Stocks
        /// </summary>
        private void AddAllStocksFromConfig()
        {

            try
            {

                KernelTerminal.AddAllStocksFromConfig(this, CreateStockCell);


                HideWindowControlPanels();
                CheckStocksForButtDelDisable();
                CheckStocksForDelEnable();
				CheckAddButtonVisibility();

				SetWidthOfStocks();

            }



            catch (Exception err)
            {

                KernelTerminal.Error("AddAllStocksFromConfig", err);


            }


        }





       public void ControlStock_ButtonClose_PreviewMouseUp(object sender, MouseButtonEventArgs e)
       {

           CloseWindow();

       }


       public void ControlStock_ButtonMaximizeNomalize_PreviewMouseUp(object sender, MouseButtonEventArgs e)
       {
           if (WindowState == WindowState.Normal)
               MaximizeWindow();
           else if (WindowState == WindowState.Maximized)
               WindowState = WindowState.Normal;

       }

       public void ControlStock_ButtonMinimize_PreviewMouseUp(object sender, MouseButtonEventArgs e)
       {

           MinimizeWindow();

       }


       private void MinimizeWindow()
       {

           WindowState = WindowState.Minimized;
       }



        /*
        private void MainWindow_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            _buttonClose.ProcessMouseUp(e.GetPosition(this), CloseWindow);
            _buttonMaximize .ProcessMouseUp(e.GetPosition(this), MaximizeWindow);
            _buttonMinimize.ProcessMouseUp(e.GetPosition(this), MinimizeWindow);

         

          _isInModeMoveWindow = false;
          Mouse.Capture(null);
          _pntPrev = new Point();
        }
        */
       

        private void MaximizeWindow()
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;

        }





               

        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
          //  ReOpenChildWindows();
        }



        private void RetriveBorderFromTemplate()
        {
            try
            {

                Style ob = (Style)FindResource("CustomTitleBar");
                foreach (Setter str in ob.Setters)
                {

                    if (str == null)
                        continue;
                    
                      if (str.Property.Name == "Template")
                        {

                          ControlTemplate tmpl = (ControlTemplate)str.Value;                         
                           if (tmpl != null)
                            {
                                var res = tmpl.FindName("BorderMainWindow", this);
                                if (res != null)
                                {
                                    _borderWindow = (Border)res;
                                    return;
                                }
                            }

                        }

                 }
                

            }
            catch (Exception e)
            {
                Error("RetriveBorderFromTemplate");
            }
        }


		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			// (new Thread(ForceRenderOnLoad)).Start();
			ReOpenChildWindows();
			RetriveBorderFromTemplate();
			
			if(_terminalViewModel.NeedAutoConnection)
				KernelTerminal.AutoConnection();



		}





        public void ReOpenChildWindows()
        {

         


            KernelTerminal.TerminalConfig.ListWindowSavedData.ForEach( a=>
                {
                    if (a.IsOpened)                   
                        KernelTerminal.ViewModelDispatcher.OpenViewChildOnStart(a.TypeOfWindow);                                                                     
                    

                 
                }



                );


        }






        public void Error(string msg, Exception e=null)
        {
            Alarmer.Error(msg, e);

        }


        /// <summary>
        /// Add columns to Grid and adds ControlMarket 
		/// and GridSplitter to Grid.
        /// 
        /// Call from:
        /// 1) KernelTerminal.AddAllStocksFromConfig
        /// 2) KernelTerminal.AddEmptyStockFromButton
        ///         
        /// </summary>
        /// <param name="VisualConf">Params from config, if null (call from button) 
        /// do use default params</param>
        private void CreateStockCell(int stockNum, string ticker, ControlMarket controlMarket, CStocksVisual visualConf=null) 
        {
            
            ColumnDefinition colDef = new ColumnDefinition();
          
            colDef.Tag = "ColStock_"+stockNum;
            GridMarket.ColumnDefinitions.Add(colDef);

            /*
            //2018-05-03
            //Use default params - called from button
            if (visualConf == null)
            {
                double sumWidthStock = 0;
               
                foreach (UIElement v in GridMarket.Children)
                {

                    if (v is ControlMarket)
                    {
                        ControlMarket currCM = (ControlMarket)v;
                        foreach (var cd in   currCM.GridControlMarket.ColumnDefinitions)
                        {
                            if (cd.Name == "ColControlStock")                            
                                sumWidthStock += cd.ActualWidth;
                            
                        }
                        

                    }

                }




                //Stock Cell
                if (stockNum != 0)
                {
                    controlMarket.GridControlMarket.ColumnDefinitions[4].Width =
                        new GridLength(sumWidthStock / (stockNum));

                    
                }

            }
            else //use specific params - called from initialization (params from filr)
            {
                //Clusters cell
                controlMarket.GridControlMarket.ColumnDefinitions[0].Width =
                   new GridLength(visualConf.WidthClusters);

                //Stock Cell
                controlMarket.GridControlMarket.ColumnDefinitions[4].Width = 
                    new GridLength(visualConf.WidthStock);
            }
                */                 
            Grid.SetRow(controlMarket, 0);
            Grid.SetColumn(controlMarket, 2 * stockNum); //KAA changed 2017-03 was stockNum
            GridMarket.Children.Add(controlMarket);
			

			//2017-03-17 add splitter
			ColumnDefinition colSplitter = new ColumnDefinition();
			
			colSplitter.Width = GridLength.Auto;
			
			GridMarket.ColumnDefinitions.Add(colSplitter);


			GridSplitter splitter = new GridSplitter();
			splitter.Width = 1.5;
			splitter.Background = new SolidColorBrush(Colors.Blue); //TODO DP
			splitter.HorizontalAlignment = HorizontalAlignment.Stretch;
			splitter.Tag = stockNum;
			Panel.SetZIndex(splitter, Int16.MaxValue);


			Grid.SetRow(splitter, 0);
			Grid.SetColumn(splitter, 2 * stockNum + 1);
			GridMarket.Children.Add(splitter);


          //  Canvas.SetZIndex(controlMarket, 10 - stockNum);

           
        }



		/// <summary>
		/// 1) Call KernelTerminal.AddEmptyStockFromButton to create ControlMarket and MarketViewModel.
		/// After creation perform Callback CreateStockCell (adds ControlMarket to Grid).		
		/// 2) After creating manage Stock elements: WindowControlPannel and buttons,
		/// control width of Stocks
		/// 		
		/// Call from EventHandler -  ButtonAddStock_PreviewMouseUp
		/// </summary>
        public void AddStockFromButton()
        {
            try
            {

           

                int stockNum = KernelTerminal.AddEmptyStockFromButton(this, CreateStockCell);

                HideWindowControlPanels();
                CheckStocksForButtDelDisable();
                CheckStocksForDelEnable();
				CheckAddButtonVisibility();

				SetWidthOfStocks();

            }
            catch (Exception exc)
            {
                Error("AddStockFromButton", exc);

            }



        }


        /// <summary>
        /// Find ControlMarket with given number, remove it
        /// from viusal tree creates new one then add it to
        /// visual tree.
        /// 
        /// </summary>
        /// <param name="stockNum"></param>
        /// <returns></returns>
        public ControlMarket ReplaceControlMarket(int stockNum)
        {

           
          
            ControlMarket mc = new ControlMarket(stockNum, this);
            Grid.SetRow(mc, 0);
            Grid.SetColumn(mc, 2*stockNum);
            //NOTE +1 because 0 is ControlAlarm


            GridMarket.Children.RemoveAt(2*stockNum);
            GridMarket.Children.Insert(2*stockNum, mc);

            HideWindowControlPanels();

			SetWidthOfStocks();

           // CKernelTerminal.GetViewDispatcherInstance().UpdateZIndexes();
           // Canvas.SetZIndex(mc, 10 - stockNum);
            return mc;


        }




		/// <summary>
		/// 1) Delete GridSplitter from visual tree and 
		/// GridSplitter's column from grid
		/// 2) Delete ControlMarket from visual tree and
		/// ControlMarket's column from grid
		/// Note. Each ControlMarket has StockNum property
		/// with enumeration  from 0 to N. And the same is 
		/// for GridSplitter but using tag.
		/// Enumeration in ColumnDefiniions and in 
		/// Child list is even (0,2,4) for ControlMarket,
		/// odd (1,3,5) for GridSplitter
		/// 
		/// Call from:
		/// KernerlTerminal.DeleteExistingStock
		/// </summary>		
        public void DeleteStock(int stockNum)
        {
			//Delete GridSplitter
			foreach (var child in GridMarket.Children)
			{
				if (child is GridSplitter)
				{
					GridSplitter gs = (GridSplitter)child;
					if ((int)gs.Tag == stockNum)
					{

						int col = Grid.GetColumn(gs);

						GridMarket.ColumnDefinitions.RemoveAt(col); 
						GridMarket.Children.Remove(gs);					
						break;                                        
					}

				}

			}

			//Delete ControlMarket
            foreach (var child in GridMarket.Children)
            {

                if (child is ControlMarket)
                {
                    ControlMarket cm = (ControlMarket)child;
					

                    if (cm.StockNum == stockNum)
                    {
                        
                        int col = Grid.GetColumn(cm);
						
                        GridMarket.Children.Remove(cm);
                        GridMarket.ColumnDefinitions.RemoveAt(col);					
                        break;

                    }
                }
            }

						
            CheckStocksForButtDelDisable();
			ShowControlPanelOnRightStock(stockNum);
			SetWidthOfStocks();

        }

		/// <summary>
		/// Disable button delete stock if stock is
		/// single
		/// </summary>
        public void CheckStocksForButtDelDisable()
        {


            int count = 0;

            ControlMarket cm = null;

            foreach (var child in GridMarket.Children)
            {
                if (child is ControlMarket)
                {
                    count++;
                    cm = (ControlMarket) child;                  
                }


            }
            if (count ==1)
                if (cm != null)
                {
                    cm.DisableDeleteStock();

                }

                                   
        }
		/// <summary>
		/// If Empty instrument disable ButtonAddStock
		/// </summary>
		public void CheckAddButtonVisibility()
		{
			foreach (var child in GridMarket.Children)
			{
				if (child is ControlMarket)
				{
					ControlMarket cm = (ControlMarket)child;
					if (cm.ControlStockInstance.TickerName == Literals.Undefind)
						cm.ButtonAddStock.Visibility = Visibility.Collapsed;
					else
						cm.ButtonAddStock.Visibility = Visibility.Visible;
				}
			}
		}





        public void CheckStocksForDelEnable()
        {


            int count = 0;

            ControlMarket cm = null;

            foreach (var child in GridMarket.Children)
            {
                if (child is ControlMarket)
                {

                    count++;
                    cm = (ControlMarket)child;

                }


            }
            if (count <= 1) //still single stock
                return;



            foreach (var child in GridMarket.Children)
            {
                if (child is ControlMarket)
                {
                    cm = (ControlMarket)child;
                    cm.EnableDeleteStock();
                }


            }

        }







        //TODO move this to control
        private bool _firstTime = true;
        private void AlarmList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (ComboboxAlarm.AlarmComboBox.Items.Count != 0)
            {
                ComboboxAlarm.AlarmComboBox.SelectedIndex = 0;


                ComboboxAlarm.AlarmComboBox.Background = System.Windows.Media.Brushes.Red;



                ComboboxAlarm.AlarmComboBox.Foreground = System.Windows.Media.Brushes.White;


                if (_firstTime)
                {
                    ComboboxAlarm.AlarmComboBox.Resources.Add(System.Windows.SystemColors.WindowBrushKey, Brushes.Red);
                    ComboboxAlarm.AlarmComboBox.Resources.Add(System.Windows.SystemColors.HighlightBrushKey, Brushes.Red);
                    _firstTime = false;
                }

            }


        }





       
        public void CloseWindow()
        {

            try
            {
                KernelTerminal.OnClose();


                //TODO normal exit

              //  Application.Current.Shutdown();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            catch (Exception exc)
            {

                Error("MainWindow.CloseWindow", exc);
            }

        }


        private void ShowVersionWindow()
        {

            if (_windowVersion == null)
            {
                _windowVersion = new VersionWindow();
                CUtilWin.ShowDialogOnCenter(_windowVersion, this);
                _windowVersion = null;
            }

        }



        private void ExecuteCommand(RoutedUICommand cmd, object data = null)
        {
            cmd.Execute(data, this);
        }

        private void CloseInstrumentPositions(ControlMarket controlMarket)
        {
           
            MarketViewModel mv = (MarketViewModel)controlMarket.DataContext;

            CDataCloseInstPos dataClose = new CDataCloseInstPos
            {
                Instrument = controlMarket.ControlStockInstance.TickerName,
                ConId = mv.ConnId
            };


            ExecuteCommand(EventsGUI.CloseInstrumentPositions, dataClose);
               
                    
            return;
            
        }

        private void CancellInstrOrder(ControlMarket controlMarket)
        {

            MarketViewModel mv = (MarketViewModel)controlMarket.DataContext;

            CDataCloseInstPos dataClose = new CDataCloseInstPos
            {
                Instrument = controlMarket.ControlStockInstance.TickerName,
                ConId = mv.ConnId
            };


            ExecuteCommand(EventsGUI.CancellInstrumentOrders, dataClose);


            return;


        }


        private void CloseAllPositions()
        {

            CDataCloseAllPos dataCloseAllPos = new CDataCloseAllPos();
            ExecuteCommand(EventsGUI.CloseAllPositions, dataCloseAllPos);

        }


        private void CancellAllOrders()
        {
            CDataCancellAllOrders cancellAllOrders = new CDataCancellAllOrders();
            ExecuteCommand(EventsGUI.CancellAllOrders, cancellAllOrders);

        }




        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
         
            try
            {
                //Only this window key pressing

                if (e.Key == Key.F1)
                {
                    ShowVersionWindow();
                    return;
                }
                if (e.Key == Key.F2)
                {
                    HotKeyWindow hkw = new HotKeyWindow();
                    CUtilWin.ShowDialogOnCenter(hkw, this);

                }


                if (e.Key == Key.RightCtrl || e.Key == Key.LeftCtrl)
                {
                    //If in stock zone do close only by instrument end get out.
                    //Else close all instrument - do send data closeall positions
                    //and cancell all orders commands
                    
                    foreach (var controlMarket in KernelTerminal.ViewDispatcher.LstControlMarket)                    
                        if (controlMarket.ControlStockInstance.IsInStockArea)
                        {
                            CloseInstrumentPositions(controlMarket);
                            CancellInstrOrder(controlMarket);
                            return;
                        }

                    //not in ControlStock - do cancell and close all
                    CloseAllPositions();
                    CancellAllOrders();
                    return;


                }
                if (e.Key == Key.Space)
                {
                    //All the same as for close (see above)
                    foreach (var controlMarket in KernelTerminal.ViewDispatcher.LstControlMarket)
                        if (controlMarket.ControlStockInstance.IsInStockArea)
                        {                           
                            CancellInstrOrder(controlMarket);
                            return;
                        }
                    CancellAllOrders();
                    return;
                }
                
                

                /** Cause on child control events not triggers (unknown reason) do it forcely. 
                 * 
                 */

                foreach (var v in KernelTerminal.ViewDispatcher.LstControlMarket)
                    v.ControlMarket_PreviewKeyDown(sender, e);
            }
            catch (Exception exc)
            {
                Error("MainWindow.MainWindow_PreviewKeyDown", exc);
            }



        }


        private void MainWindow_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            //  System.Threading.Thread.Sleep(0);
            try
            {
                foreach (var v in KernelTerminal.ViewDispatcher.LstControlMarket)
                    v.ControlMarket_PreviewKeyUp(sender, e);
            }
            catch (Exception exc)
            {
                Error("MainWindow.MainWindow_PreviewKeyUp", exc);
            }         

        }




        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
        //    System.Threading.Thread.Sleep(1000);
           
        }
       
        /*
        public void ClickMenuItemWindow<TChildWindow>() where TChildWindow : Window 
        {          
            KernelTerminal.ViewModelDispatcher.OpenChildWindow <TChildWindow>();
        }
       */


      

        /// <summary>
        /// On add stock do hide all WindowControlPanels, except the rightest stock (with largest StockNum)
		/// Call method after change stock configuration (add or replace)
		/// 
		/// Call from: 
		/// 1) Mainwindow.AddAllStocksFromConfig
		/// 2) Mainwindow.AddStockFromButton
		/// 3) MainWindow.ReplaceControlMarket
        /// </summary>       
        public void HideWindowControlPanels()
        {

            int cntOfStocks = KernelTerminal.MaxStockNum;

            foreach (var child in GridMarket.Children)
            {

                if (child is ControlMarket)
                {
                   ControlMarket cm = (ControlMarket)child;
                   if (cm.StockNum != cntOfStocks)
                   {
                       cm.HideWindowControlPannels();

                   }
                   else
                   {
                      
                       
                    /* MarketViewModel mvm =  CKernelTerminal.GetViewModelDispatcherInstance().GetMarketViewModel(cm.StockNum)
                         if (mvm!=null)
                         {
                             mvm.

                         }*/

                   }
                      

                   
                }


            }


        }

        /// <summary>
        /// Set the width of stock depend if stock the rightest 
        /// (with control menu) or not.
        /// For the rightest stock set width auto (width calculated,
        /// by content's width) and minimum width (to make all control 
        /// elements visible on resizing), for the other stocks
        /// use * and sensible minimum width (to protect agains "silly"
        /// small width). 
        /// Note, in grid using even number for ControlMarket and odd
        /// for the GridSplitter.		
        /// 
        /// Call from:
        /// 1) AddAllStocksFromConfig
        /// 2) AddStockFromButton
        /// 3) ReplaceControlMarket
        /// 4) DeleteStock
        /// </summary>
        public void SetWidthOfStocks()
        {

            int iStockVis = 0;

            int colCount = GridMarket.ColumnDefinitions.Count;
            //Set width of grid's columns
            //note, 0 - ControlMarket, 1 - splitter, etc, that's why increase by two
            for (int i = 0; i < GridMarket.ColumnDefinitions.Count; i += 2)
            {
               
                iStockVis++;

                var col = GridMarket.ColumnDefinitions[i];
                //for the rightest column
                if ((i == colCount - 2) && (colCount > 2)) //2017-03-22 upd if single stock nothing to do
                {
                    // col.Width = GridLength.Auto;
                    col.Width = new GridLength(1, GridUnitType.Star);//2018-05-03 change was auto
                    //col.Width = new GridLength(1, GridUnitType.Star);
                    // col.Width = new GridLength(widthOfControlMarket);//tempo 2018-05-02

                    //col.MinWidth = Math.Max(170, this.Width / (colCount / 2));
                    col.MinWidth = 100; //2018-05-03 change was calculated above
                    //col.MaxWidth = 1200;
                }
                else //for the other column
                {
                    col.Width = new GridLength(1, GridUnitType.Star);
                    //col.Width = new GridLength(widthOfControlMarket);//tempo 2018-05-02
                    col.MinWidth = 100;
                }


            }

            iStockVis = 0;
            int cntOfStocks = KernelTerminal.MaxStockNum;
            //Set width of ControlStocks
            foreach (var child in GridMarket.Children)
            {

                if (child is ControlMarket)
                {
                    ControlMarket cm = (ControlMarket)child;

                    double widthOfStock = KernelTerminal.StockVisualConf.ListStocksVisual[iStockVis].WidthStock;
                    double widthOfClusters = KernelTerminal.StockVisualConf.ListStocksVisual[iStockVis].WidthClusters;

                    //the rightest ControlMarket (with window contol buttons)
                    if ((cm.StockNum == cntOfStocks) && (colCount > 2)) //2017-03-22 upd if single stock- nothing to do
                    {
                        //2017-11-16 changed for Alexeev's request 
                        // - previously was 110
                        //cm.ColControlStock.MinWidth = 110;
                        cm.ColControlStock.MinWidth = 110; //2018-05-03 back 
                        //TODO - make specific setting


                        //cm.ColControlStock.Width = new GridLength(1.0000001, GridUnitType.Star);
                        //cm.ColControlStock.Width = GridLength.Auto;
                        cm.ColControlStock.Width = new GridLength(widthOfStock);//2018-05-02
                        cm.ColControlClusters.Width = new GridLength(widthOfClusters);

                        //cm.ColControlDeals.MinWidth = 30;
                        //cm.ColControlDeals.Width = GridLength.Auto;
                       
                    }
                    //the other ControlMarkets
                    else
                    {
                        cm.ColControlStock.MinWidth = 30;
                        //cm.ColControlStock.Width = new GridLength(1, GridUnitType.Star);
                        cm.ColControlStock.Width = new GridLength(widthOfStock);   //2018-05-02
                        cm.ColControlClusters.Width = new GridLength(widthOfClusters);
                        // cm.ColControlStock.Width = new GridLength(226);
                    }


                    iStockVis++;

                }
            }
            


            foreach (var child in GridMarket.Children)
            {

                if (child is ControlMarket)
                {
                    ControlMarket cm = (ControlMarket)child;
                    cm.ControlStockInstance.IsInitializeComplete = true;
                    cm.ControlClustersInstance.IsInitializeComplete = true;
                }

            }
        }

       

		/// <summary>
		/// Shows ControlPanel on the rightest ControlMarket
		/// after we delete the "old" rightest ControlStock
		/// 
		/// </summary>
		/// <param name="stockNumDelete"></param>
        public void ShowControlPanelOnRightStock(int stockNumDelete)
        {
			//only if we delete rightes ControlMarket
			if (stockNumDelete == KernelTerminal.StockVisualConf.ListStocksVisual.Count - 1)
			{
				//-2 cause 1)numeration from zero and 2)stock was not removed from StockVisualConf
				int num = KernelTerminal.StockVisualConf.ListStocksVisual.Count - 2;

				num = Math.Max(num, 0);
				//if (stockNumDelete == 0)
					//num = KernelTerminal.StockVisualConf.ListStocksVisual.Count - 1;


				foreach (var child in GridMarket.Children)
				{

					if (child is ControlMarket)
					{
						ControlMarket cm = (ControlMarket)child;
						if (cm.StockNum == num)
							cm.ShowWindowControlPannels();

					}


				}
			}
        }

        private void BaseTerminalWindow_PreviewMouseMove(object sender, MouseEventArgs e)
        {

        }





       
      
    }

   
    

}
