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
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Timers;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;
    using Xceed.Wpf.Toolkit;

    using Common;
    using Common.Interfaces;
    using TradingLib;
    using TradingLib.ProtoTradingStructs;
   

    public class DOM : UserControl, IComponentConnector
    {
       // public string Isin { get; set; }
        private CLogger _logger;
        private string _tickerName;
        private IAlarmable _alarmer;
        CPerfAnlzr _perfAnlzr ;//= new CPerfAnlzr();

        private bool _contentLoaded;

        private Brush B;
       
        internal Canvas DOMCanvas;
        internal Image DOMImage;
        internal Image DOMImage_AntiSpread;
        internal Image DOMImage_Prices;
        private OrderFocus DOMOrderFocus;

        private Brush m_BackAskBrush = new SolidColorBrush(Colors.MistyRose);
        private Pen m_BackAskPen;
        private Brush m_BackBidBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE0FFE2"));   // new SolidColorBrush(Colors.LightGreen);
        private Pen m_BackBidPen;
        private bool m_bAutoScroll = false;
        private Brush m_BestAskBrush = new SolidColorBrush(Colors.Red);
        private Pen m_BestAskPen;
        private Brush m_BestBidBrush = new SolidColorBrush(Colors.Green);
        private Pen m_BestBidPen;
        private Brush m_BlackBrush = new SolidColorBrush(Colors.Black);
        private Pen m_BlackPen;
        private bool m_bMouseEnter;
        private RenderTargetBitmap m_Bmp;
        private RenderTargetBitmap m_Bmp_Prices;
        private bool m_bNeedToReadd;
        private bool m_bNeedToRecount = true;
        private bool m_bRepaint = true;
        private bool m_bRepaint_Prices = true;
        private bool m_bSettingsShared;
        private bool m_bStopLossSended;
        private bool m_bStopTake_ON;
        private bool m_bTakeProfitSended;
        private bool m_bTextShown;
        private bool m_bTryingToRemoveOrder;
        private OrderAction m_CancelledOrderAction;
        private ManageOrderInteraction m_CancelOrderInteraction;
        private CultureInfo m_CultureInfo = new CultureInfo("en-US");
        private double m_dCheckCoord;
        private double m_dCheckPrice;
        private double m_dCheckX;
        private double m_dFocusSpeed = 0.08;
        private Dictionary<long, PortfolioOwnedOrder> m_Dict_KOrderIDVOwnedOrder = new Dictionary<long, PortfolioOwnedOrder>();
        private Dictionary<double, double> m_Dict_KPriceVCoordin = new Dictionary<double, double>();
        private Dictionary<double, double> m_Dict_KPriceVCoordin_LastPainted = new Dictionary<double, double>();
        private Dictionary<double, OrderStatic> m_Dict_KPriceVOrder = new Dictionary<double, OrderStatic>();
        private double m_dPosPrice;
        private double m_dPrevPrice;
        private double m_dPriceWidth;
        private DrawingContext m_DrawCont;
        private DrawingContext m_DrawCont_Prices;
        private DrawingVisual m_DrawVis = new DrawingVisual();
        private DrawingVisual m_DrawVis_Prices = new DrawingVisual();
        private double m_dStep = 10;//KAA
        private double m_dStopPrice;
        private double m_dTakePrice;
        private double m_dTxtWidth;
        private double m_dVolWidth;
        private System.Timers.Timer m_FocusTimer;
        private GraphicDataExternalSource m_GraphicDataSource;
        private Brush m_GridBrush = new SolidColorBrush(Colors.Black);
        private Pen m_GridPen;
        private int m_iFocusAsk;
        private int m_iFocusBid;
       // private double m_iFontWeight = 20;//kaa
        private int m_iHowFarSendOrder = 50;
        private double m_iMaxAmount = 100.0;
        private int m_iMouseDelta = 130;
        private Brush m_InLossBrush = new SolidColorBrush(Colors.Red);
        private Pen m_InLossPen;
        private Brush m_InProfitBrush = new SolidColorBrush(Colors.Green);
        private Brush m_InProfitBrush_Light = new SolidColorBrush(Color.FromArgb(/*240*/255, 0x80, 0xff, 0x80)); //KAA 2016-05-04
        private Pen m_InProfitPen;
        private int m_iPrevFocusAsk;
        private int m_iPriceDecimals= 0;//KAA
        private int m_iProfitSteps;
        private int m_iRightBorder = 30;
        private int m_iScrollDelta;
        private int m_iStopLoss;
        private int m_iTakeProfit;
        private int m_iVolumeRectH = 14;
        private List<TextBlock> m_lAsksTxtBlocks = new List<TextBlock>();
        private List<TextBlock> m_lBidsTxtBlocks = new List<TextBlock>();
        private List<OrderStatic> m_lOrderStatic = new List<OrderStatic>();
        private List<TextBlock> m_lPositionTxtBlocks = new List<TextBlock>();
        private List<TextBlock> m_lPricesTxtBlocks = new List<TextBlock>();
        private List<PortfolioOwnedOrder> m_lSettedOrders = new List<PortfolioOwnedOrder>();
        private List<double> m_lSW = new List<double>();
        private PortfolioManagerGraphicDataInteractionContext m_ManageOrderInteractionContext;
        private GraphicDataExternalSource m_ManagePortfolioSource;
        private Stock_Position[] m_OldAsks;
        private Stock_Position[] m_OldBids;
        private Grid m_PositionGrid;
        private RotateTransform m_RotateTransform = new RotateTransform();
        private string m_sAmountStyle = "Limited";
        private string m_sContractShortName = string.Empty;
        private ManageOrderInteraction m_SendOrderInteraction;
        private SettingsWindow m_SettingsWin;
        private string m_sShortName = string.Empty;
        private Stopwatch m_SW = new Stopwatch();
        private System.Timers.Timer m_Timer;
        private FormattedText m_Txt;
        private TextBlock m_TxtBlck_Amount;
        private TextBlock m_TxtBlck_Price;
        private TextBlock m_TxtBlck_Profit;
        private FontFamily m_TxtFontFam = new FontFamily("Verdana");
        private Typeface m_TypeFace;
        private Brush m_VolumeBrush = new SolidColorBrush(Colors.Orange);
        private Pen m_VolumePen;

        private Pen P;


        public static readonly DependencyProperty GraphicDataSourceProperty = DependencyProperty.Register("GraphicDataSource", typeof(GraphicDataExternalSource), typeof(DOM));


        public static readonly DependencyProperty OrdersProperty = DependencyProperty.Register("Orders", typeof(PortfolioOwnedOrder[]), typeof(DOM));
        public static readonly DependencyProperty UserPosProperty = DependencyProperty.Register("UserPos", typeof(CUserPos), typeof(DOM));


       
        public static readonly DependencyProperty PortfolioProperty = DependencyProperty.Register("Portfolio", typeof(PortfolioGraphicData), typeof(DOM));
        public static readonly DependencyProperty ShortNameProperty = DependencyProperty.Register("ShortName", typeof(string), typeof(DOM));
        public static readonly DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(double), typeof(DOM));

        public static readonly DependencyProperty AsksProperty = DependencyProperty.Register("Asks", typeof(Stock_Position[]), typeof(DOM));

        public static readonly DependencyProperty BidsProperty = DependencyProperty.Register("Bids", typeof(Stock_Position[]), typeof(DOM));
        public static readonly DependencyProperty DecimalsProperty = DependencyProperty.Register("Decimals", typeof(int), typeof(DOM));




        public static readonly DependencyProperty TestProperty = DependencyProperty.Register("Test", typeof(string), typeof(DOM),
            new PropertyMetadata("", new PropertyChangedCallback(TmpCallback))
            );









        
        //public static readonly DependencyProperty OrdersProperty = DependencyProperty.Register("Orders", typeof(PortfolioOwnedOrder[]), typeof(DOM));



        //public static readonly DependencyProperty Work = DependencyProperty.Register("ListWorkAmount", typeof(List<TradingLib.CWorkAmount>), typeof(DOM));

        

        public event EventHandler DOMScrolledEvent;

        public event EventHandler PriceCoordChanged;


        private ITradeOperations _tradeOperation;


        public static readonly DependencyProperty ListWorkAmountProperty = DependencyProperty.Register("ListWorkAmount", 
                                                                                typeof(List<CWorkAmount>), typeof(DOM));

        public static readonly DependencyProperty CurrAmountNumProperty = DependencyProperty.Register("CurrAmountNum",
                                                                               typeof(string), typeof(DOM), 
                                                                               new PropertyMetadata(""));

        // == SHARED SETTINGS == Step 5.1 create DP in separate control

        public int StringHeight
        {
            get { return (int)GetValue(StringHeightProperty); }
            //set { SetValue(StringHeightProperty, value); }
        }
       


        // Using a DependencyProperty as the backing store for StringHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StringHeightProperty =
            DependencyProperty.Register("StringHeight", typeof(int), typeof(DOM), new UIPropertyMetadata(1));



        public int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            //set { SetValue(FontSizetProperty, value); }
        }



        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(int), typeof(DOM), new UIPropertyMetadata(1));

     





     //   public static readonly DependencyProperty CurrAmountNumProperty = GUIComponents.ControlWorkAmount.CurrAmountNumProperty.AddOwner(typeof(DOM));


        private long _maxRepaintTimeMS;



        private static void TmpCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            

        }


      




        public DOM()
        {
          
            this.InitializeComponent();
            this.InitializeBrushes();
            this.FiftyLevels = new List<double>();
            this.TenLevels = new List<double>();
           // this.StringHeight = 10;//was 13 2016-May-20
            if (this.m_TypeFace == null)
            {
                this.m_TypeFace = new Typeface(this.m_TxtFontFam, FontStyles.Normal, FontWeights.Normal, new FontStretch());
            }
            this.m_FocusTimer = new System.Timers.Timer(45.0);
            this.m_FocusTimer.Elapsed += new ElapsedEventHandler(this.FocusTimerElapsed);
            this.m_Timer = new System.Timers.Timer(100); //KAA was 20
            this.m_Timer.Elapsed += new ElapsedEventHandler(this.m_Timer_Elapsed);
            this.m_Timer.Start();

            this.KeyDown += new KeyEventHandler(DOM_KeyDown);
            this.PreviewKeyDown += new KeyEventHandler(DOM_PreviewKeyDown);


            RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
            RenderOptions.SetBitmapScalingMode(this.DOMImage, BitmapScalingMode.NearestNeighbor);
            TextOptions.SetTextRenderingMode(this.DOMImage, TextRenderingMode.ClearType);
            TextOptions.SetTextFormattingMode(this.DOMImage, TextFormattingMode.Display);
            RenderOptions.SetBitmapScalingMode(this.DOMImage_Prices, BitmapScalingMode.NearestNeighbor);
            TextOptions.SetTextRenderingMode(this.DOMImage_Prices, TextRenderingMode.ClearType);
            TextOptions.SetTextFormattingMode(this.DOMImage_Prices, TextFormattingMode.Display);
          

          //  _logger.Log("DOM_"+);

           
        }

        public void DOM_PreviewKeyDown(object sender, KeyEventArgs e)
        {
          //  throw new NotImplementedException();
        }

        public void DOM_KeyDown(object sender, KeyEventArgs e)
        {
      //      throw new NotImplementedException();
        }


        private void Error(string msg, Exception e=null)
        {
            if (_alarmer != null)            
                _alarmer.Error(msg,e);
         
        }


        public void BindToSystem(ITradeOperations tradeOperation, IAlarmable alarmer, string tickername, long maxRepaintTimeMS)
        {

            _tradeOperation = tradeOperation;
            _alarmer = alarmer;
            _tickerName = tickername;

            _perfAnlzr = new CPerfAnlzr(_alarmer);

            _maxRepaintTimeMS = maxRepaintTimeMS;




            //tempo TODO normal
            
            
                     
        }

        private void Log(string message)
        {
            if (_logger == null)
            {
                if (_tradeOperation != null && _tickerName != null)
                {
                    _logger = new CLogger("DOM " + _tickerName + "____" + DateTime.Now.ToString("yyy_MM_dd__hh_mm_ss"));
                    _logger.Log(message);
                }
            }
            else
                _logger.Log(message);


        }
        public void ChangeOrdersWidth()
        {
            this.m_bNeedToRecount = true;
            foreach (KeyValuePair<double, OrderStatic> pair in this.m_Dict_KPriceVOrder)
            {
                pair.Value.Width = base.ActualWidth;
            }
            this.m_bRepaint_Prices = true;
        }

        public void ChangeStopOrTakePrice()
        {
        }

        private void DoFocus()
        {
            double length = 0.0;
            double y = 0.0;
            y = Mouse.GetPosition(this.DOMImage).Y;
            foreach (KeyValuePair<double, double> pair in this.m_Dict_KPriceVCoordin)
            {
                if ((y >= pair.Value) && (y < (pair.Value + this.StringHeight)))
                {
                    length = pair.Value;
                }
            }
            if (this.DOMOrderFocus != null)
            {
                Canvas.SetLeft(this.DOMOrderFocus, 0.0);
                Canvas.SetTop(this.DOMOrderFocus, length);
            }
        }

        private void FocusTimerElapsed(object sender, ElapsedEventArgs e)
        {
            double num = (this.m_iFocusAsk + this.m_iFocusBid) / 2;
            if (num < (0.3 * base.ActualHeight))
            {
                this.m_iFocusAsk += 3 * ((int) (((0.5 * base.ActualHeight) - this.m_iFocusAsk) * this.m_dFocusSpeed));
            }
            else if (num > (0.7 * base.ActualHeight))
            {
                this.m_iFocusAsk -= 3 * ((int) ((this.m_iFocusBid - (0.5 * base.ActualHeight)) * this.m_dFocusSpeed));
            }
            if ((((0.3 * base.ActualHeight) - this.m_iFocusAsk) < 1.0) && (this.m_iFocusAsk < (0.7 * base.ActualHeight)))
            {
                this.m_FocusTimer.Stop();
            }
            else if (((this.m_iFocusBid - (0.7 * base.ActualHeight)) < 1.0) && (this.m_iFocusBid > (0.3 * base.ActualHeight)))
            {
                this.m_FocusTimer.Stop();
            }
            this.m_bNeedToRecount = true;
            this.m_bRepaint = true;
            Action method = new Action(this.OnPaint);
            base.Dispatcher.Invoke(DispatcherPriority.Render, method); //KAA was Render
        }

        private void InitializeBrushes()
        {
            this.m_GridPen = new Pen(this.m_GridBrush, 0.75);
            this.m_VolumePen = new Pen(this.m_VolumeBrush, 1.0);
            this.m_BestAskPen = new Pen(this.m_BestAskBrush, 1.0);
            this.m_BackAskPen = new Pen(this.m_BackAskBrush, 1.0);
            this.m_BestBidPen = new Pen(this.m_BestBidBrush, 1.0);
            this.m_BackBidPen = new Pen(this.m_BackBidBrush, 1.0);
            this.m_InProfitPen = new Pen(this.m_InProfitBrush, 1.0);
            this.m_InLossPen = new Pen(this.m_InLossBrush, 1.0);
            this.m_BlackPen = new Pen(this.m_BlackBrush, 2.0);
            this.m_InProfitBrush.Opacity = 0.7;
            this.m_InLossBrush.Opacity = 0.5;
            this.m_GridBrush.Opacity = 0.2;
        }

       [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/Visualizer;component/dom.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
         
            

        }

        private void m_Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action method = new Action(this.OnPaint);
            base.Dispatcher.Invoke(DispatcherPriority.Render, method); //KAA was Render
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            string tmp2 = ListWorkAmount[0].TextAmountValue;


           


            //focus was set by mouse click
            if (this.FocusedByClick && (Mouse.GetPosition(this.DOMImage).X > 0.0))
            {
                //something to change stop or take
                if (this.CanClick_ChangeStopOrTake)
                {
                    if (((this.DOMOrderFocus != null) && (this.Portfolio != null)) && ((this.Asks != null) && (this.Bids != null)))
                    {
                        if ((this.Asks[0] == null) || (this.Bids[0] == null))
                        {
                            return;
                        }
                        double top = 0.0;
                        double key = 0.0;
                        top = Canvas.GetTop(this.DOMOrderFocus);
                        foreach (KeyValuePair<double, double> pair in this.m_Dict_KPriceVCoordin)
                        {
                            if (pair.Value == top)
                            {
                                key = pair.Key;
                            }
                        }
                        if (this.CanClick_ChangeStopOrTake)
                        {
                            if (this.Portfolio.CurrentPositionAmount > 0)
                            {
                                int num3 = (int) Math.Round((double) ((this.Portfolio.CurrentPositionPrice - key) / this.Step), 0);
                                if (this.Bids[0].Price >= key)
                                {
                                    this.m_dStopPrice = key;
                                    if (this.m_iStopLoss == num3)
                                    {
                                        this.m_iStopLoss = 0;
                                    }
                                    else
                                    {
                                        this.m_iStopLoss = num3;
                                    }
                                }
                                else if (this.Asks[0].Price <= key)
                                {
                                    this.m_dTakePrice = key;
                                    if (this.m_iTakeProfit == -num3)
                                    {
                                        this.m_iTakeProfit = 0;
                                    }
                                    else
                                    {
                                        this.m_iTakeProfit = -num3;
                                    }
                                }
                            }
                            else if (this.Portfolio.CurrentPositionAmount < 0)
                            {
                                int num4 = (int) Math.Round((double) ((this.Portfolio.CurrentPositionPrice - key) / this.Step), 0);
                                if (this.Asks[0].Price <= key)
                                {
                                    this.m_dStopPrice = key;
                                    if (this.m_iStopLoss == -num4)
                                    {
                                        this.m_iStopLoss = 0;
                                    }
                                    else
                                    {
                                        this.m_iStopLoss = -num4;
                                    }
                                }
                                else if (this.Bids[0].Price >= key)
                                {
                                    this.m_dTakePrice = key;
                                    if (this.m_iTakeProfit == num4)
                                    {
                                        this.m_iTakeProfit = 0;
                                    }
                                    else
                                    {
                                        this.m_iTakeProfit = num4;
                                    }
                                }
                            }
                            this.Repaint = true;
                        }
                    }
                    return;
                }

                //if left or right mouse button was click
                if ((e.ChangedButton == MouseButton.Left) || (e.ChangedButton == MouseButton.Right))
                {
                    //if user clicked not in area with order than exit                    
                    if (Mouse.GetPosition(this.DOMImage).X > (base.ActualWidth - this.m_iRightBorder))
                    {   //for order remove there is a special text block so we exit method here
                        this.m_bTryingToRemoveOrder = false;
                        return;
                    }
                    if (/*(this.m_ManageOrderInteractionContext != null) && */ !this.m_bTryingToRemoveOrder)
                    {
                      /*  if (this.m_SendOrderInteraction == null)
                        {
                            this.m_SendOrderInteraction = (ManageOrderInteraction) this.m_ManageOrderInteractionContext.InvokeInteraction(GraphicDataInteractionType.ManageOrders);
                        }
                        if (this.m_SendOrderInteraction == null)
                        {
                            return;
                        }
                       */
                        OrderAction act = new OrderAction();
                        EnmOrderDir dir = new EnmOrderDir();
                      //  this.m_SendOrderInteraction.ManageAction = ManageOrdersAction.SendOrder;
                        if (e.ChangedButton == MouseButton.Left)
                        {
                           // this.m_SendOrderInteraction.OrderAction = OrderAction.Buy;
                            act = OrderAction.Buy;
                            dir = EnmOrderDir.Buy;
                        }
                        else if (e.ChangedButton == MouseButton.Right)
                        {
                            //this.m_SendOrderInteraction.OrderAction = OrderAction.Sell;
                            act = OrderAction.Sell;
                            dir = EnmOrderDir.Sell;
                        }
                        else
                        {
                            //this.m_SendOrderInteraction.OrderAction = OrderAction.Unknow;
                            act = OrderAction.Unknow;
                        }
                    //    this.m_SendOrderInteraction.BySpecifiedAmount = false;




                        if (/*this.m_SendOrderInteraction.OrderAction*/act != OrderAction.Unknow)
                        {
                            //get PositionY and than get price
                           
                            double posY = -10000.0;
                            double price = 0.0;


                            if (this.DOMOrderFocus != null)
                            {
                                posY = Canvas.GetTop(this.DOMOrderFocus);
                            }
                            foreach (KeyValuePair<double, double> pair2 in this.m_Dict_KPriceVCoordin)
                            {
                                if (pair2.Value == posY)
                                {
                                    price = pair2.Key;
                                }
                            }

                            

                            this.LastClickCoord = posY;

                           


                           


                            //int amount = 1;

                            int currNum = Convert.ToInt16(CurrAmountNum);

                            //if (tmp != 0)
                              //  System.Threading.Thread.Sleep(0);


                            int amount = Convert.ToInt16(ListWorkAmount[currNum].TextAmountValue);




                            _tradeOperation.AddOrder(_tickerName, amount, dir, (decimal) price);
                            Log("Order sent _tickerName=" + _tickerName + " Amount=" + amount + " dir=" + dir);

                           /* this.m_SendOrderInteraction.OrderPrice = price;
                            if (this.m_ManagePortfolioSource != null)
                            {
                                this.m_ManagePortfolioSource.SendInteraction(this.m_SendOrderInteraction);
                            }
                            */
                        }
                    }
                }
            }
            this.m_bTryingToRemoveOrder = false;
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            this.m_bMouseEnter = true;
            if (this.m_FocusTimer.Enabled)
            {
                this.m_FocusTimer.Stop();
            }
            if (this.MouseMode && (this.DOMOrderFocus == null))
            {
                this.ShowOrderFocus(true);
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            this.m_bMouseEnter = false;
            this.ShowOrderFocus(false);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (this.MouseMode)
            {
                if (this.DOMOrderFocus == null)
                {
                    this.ShowOrderFocus(true);
                }
                else
                {
                    this.DoFocus();
                }
            }
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                this.m_iFocusAsk += this.m_iMouseDelta;
            }
            else if (e.Delta < 0)
            {
                this.m_iFocusAsk -= this.m_iMouseDelta;
            }
            this.m_bNeedToRecount = true;
            this.DOMScrollDelta = e.Delta;
            this.Repaint = true;
        }

        public void TriggerRepaintStock()
        {
            m_bRepaint = true;

        }
        public void BindToViewModel()
        {
            CUtil.SetBinding(DataContext, "UserPos", this, UserPosProperty);



        }

        public double GetCurrenPrice()
        {
            if (UserPos.Amount > 0)
                return Bids[0].Price;
            else
                return Asks[0].Price;
        }

        Stopwatch sw = new Stopwatch();

        Stopwatch sw1 = new Stopwatch();
        Stopwatch sw11 = new Stopwatch();


        Stopwatch sw2 = new Stopwatch();
        Stopwatch sw3 = new Stopwatch();
        Stopwatch sw4 = new Stopwatch();
        Stopwatch sw5 = new Stopwatch();
        Stopwatch sw6 = new Stopwatch();
        Stopwatch sw7 = new Stopwatch();

        public void OnPaint()
        {

            Log("Onpaint begin");

            //KAA tempo
          //  PortfolioGraphicData p = Portfolio;
            //PortfolioOwnedOrder ord;

          //  if (Orders !=null)
            //     ord = Orders[0];

           
            //SetValue(TestProperty, "ttt");

         //   string tst;
           // if (Test != "")
             //   tst = Test;

            sw.Stop(); sw.Reset();sw.Start();

            sw1.Stop(); sw1.Reset(); sw1.Start();
            sw11.Stop(); sw11.Reset(); sw11.Start();

            sw2.Stop(); sw2.Reset(); sw2.Start();
            sw3.Stop(); sw3.Reset(); sw3.Start();
            sw4.Stop(); sw4.Reset(); sw4.Start();
            sw5.Stop(); sw5.Reset(); sw5.Start();
            sw6.Stop(); sw6.Reset(); sw6.Start();
            sw7.Stop(); sw6.Reset(); sw7.Start();



            if ((this.m_ManagePortfolioSource != this.GraphicDataSource) && (this.GraphicDataSource != null))
            {
                this.m_ManagePortfolioSource = this.GraphicDataSource;
                this.m_ManageOrderInteractionContext = (PortfolioManagerGraphicDataInteractionContext) this.GraphicDataSource.InteractionContext;
            }
            if (((this.Step != this.m_dStep) || (this.Decimals != this.m_iPriceDecimals)) || (this.ShortName != this.m_sShortName))
            {
                this.m_dStep =  this.Step;
                this.m_iPriceDecimals = this.Decimals;
                this.m_sShortName = this.ShortName;
                this.m_iFocusAsk = 0;
                this.m_dPrevPrice = 0.0;
                
                //KAA
                this.m_bSettingsShared = true;
                this.BlockPaint = false;

                DummyStock();
            }
            if ((this.m_bSettingsShared && !this.BlockPaint) && ((this.Asks != null) && (this.Bids != null)))
            {
                if (this.Orders != null)
                {   //Draw user order rectangles here
                    this.ShowOrders(this.Orders);
                    if (this.Orders.Length == this.m_Dict_KOrderIDVOwnedOrder.Count)
                    {
                        for (int i = 0; i < this.Orders.Length; i++)
                        {
                            if (this.m_Dict_KOrderIDVOwnedOrder.ContainsKey(this.Orders[i].OrderID) && (this.m_Dict_KOrderIDVOwnedOrder[this.Orders[i].OrderID].Amount != this.Orders[i].Amount))
                            {
                                this.m_bNeedToReadd = true;
                            }
                        }
                    }
                    if ((this.Orders.Length != this.m_Dict_KOrderIDVOwnedOrder.Count) || this.m_bNeedToReadd)
                    {
                        foreach (KeyValuePair<double, OrderStatic> pair in this.m_Dict_KPriceVOrder)
                        {
                            if (this.DOMCanvas.Children.Contains(pair.Value))
                            {
                                this.DOMCanvas.Children.Remove(pair.Value);
                            }
                            for (int j = 0; j < this.DOMCanvas.Children.Count; j++)
                            {
                                if (this.DOMCanvas.Children[j] is OrderStatic)
                                {
                                    this.DOMCanvas.Children.Remove(this.DOMCanvas.Children[j]);
                                }
                            }
                        }
                        this.m_Dict_KPriceVOrder.Clear();
                        this.m_Dict_KOrderIDVOwnedOrder.Clear();
                        this.ShowOrders(this.Orders);
                        this.m_bNeedToReadd = false;
                    }
                    foreach (KeyValuePair<double, OrderStatic> pair2 in this.m_Dict_KPriceVOrder)
                    {
                        OrderStatic element = pair2.Value;
                        if (this.m_Dict_KPriceVCoordin.ContainsKey(pair2.Key))
                        {
                            Canvas.SetTop(element, this.m_Dict_KPriceVCoordin[pair2.Key]);
                        }
                        else if (((OrderAction) element.Tag) == OrderAction.Sell)
                        {
                            Canvas.SetTop(element, 0.0);
                        }
                        else if (((OrderAction) element.Tag) == OrderAction.Buy)
                        {
                            Canvas.SetTop(element, this.DOMImage.ActualHeight - element.Height);
                        }
                    }
                }
                sw7.Stop();
                if ((this.m_dStep != 0.0) && (this.NewOrderbook || this.m_bRepaint))
                {
                    //initialy use half of height
                    if (this.m_iFocusAsk == 0)
                    {
                        this.m_iFocusAsk = (int) (base.ActualHeight * 0.5);
                    }
                    int num3 = (int) (Math.Round((double) (Math.Abs((double) (this.Asks[0].Price - this.m_dPrevPrice)) / this.m_dStep), 0) * this.StringHeight);
                    if ((this.m_dPrevPrice != 0.0) && (this.Asks[0].Price > this.m_dPrevPrice))
                    {
                        this.m_iFocusAsk -= num3;
                    }
                    else if ((this.m_dPrevPrice != 0.0) && (this.Asks[0].Price < this.m_dPrevPrice))
                    {
                        this.m_iFocusAsk += num3;
                    }
                    //ask plus delta steps scaled by StringHeight
                    this.m_iFocusBid = this.m_iFocusAsk + (((int) Math.Round((double) ((this.Asks[0].Price - this.Bids[0].Price) / this.m_dStep), 0)) * this.StringHeight);
                    this.m_dPrevPrice = this.Asks[0].Price;  
                    //KAA changed 2016-06-01                                                                          
                    this.m_Txt = new FormattedText(this.Asks[0].Price.ToString("N0" + this.m_iPriceDecimals.ToString()), this.m_CultureInfo, FlowDirection.LeftToRight, this.m_TypeFace, /*this.m_iFontWeight*/ this.FontSize, Brushes.Black);
                    this.m_dTxtWidth = this.m_Txt.Width;
                    this.m_dPriceWidth = this.m_dTxtWidth;
                    this.m_BestAskBrush.Opacity = 0.7;
                    this.m_BestBidBrush.Opacity = 0.7;
                    lock (this.m_Dict_KPriceVCoordin)
                    {
                        this.m_Dict_KPriceVCoordin.Clear();
                        double iCoordPrice = this.m_iFocusAsk;
                        double price = this.Asks[0].Price;

                        //first find bottom of price and coord
                        //
                        while (iCoordPrice < base.ActualHeight)
                        {
                            iCoordPrice += this.StringHeight;
                            price -= this.m_dStep;
                        }
                        //than fill dictionary till 
                        //top
                        while (iCoordPrice > 0.0)
                        {
                            price += this.m_dStep;
                            iCoordPrice -= this.StringHeight;
                            price = Math.Round(price, this.m_iPriceDecimals);
                            if (this.m_Dict_KPriceVCoordin.Count < 1000)
                            {
                                this.m_Dict_KPriceVCoordin.Add(price, iCoordPrice);
                            }
                            else
                            {
                                this.m_iFocusAsk = 0;
                                this.m_dPrevPrice = 0.0;
                                return;
                            }
                        }
                    }
                    sw6.Stop();
                    this.PriceCoordChanged(this, null);
                    if ((this.m_Dict_KPriceVCoordin.Count != 0) && this.m_bNeedToRecount)
                    {
                        if (this.DOMScrolledEvent != null)
                        {
                            this.DOMScrolledEvent(this, null);
                        }
                        this.m_bNeedToRecount = false;
                    }
                    if ((!this.m_bMouseEnter && ((this.m_iFocusAsk < (0.1 * base.ActualHeight)) || (this.m_iFocusBid > (0.9 * base.ActualHeight)))) && (this.m_bAutoScroll && (this.m_FocusTimer != null)))
                    {
                        this.m_FocusTimer.Start();
                    }
                    if (this.m_Dict_KPriceVCoordin.Count > 0)
                    {
                        double num6 = this.m_Dict_KPriceVCoordin.Keys.ElementAt<double>(0);
                        double num7 = this.m_Dict_KPriceVCoordin.Values.ElementAt<double>(0);
                        double num8 = (base.ActualWidth - this.m_iRightBorder) - (this.m_dTxtWidth * 1.1);
                        if (((num6 != this.m_dCheckPrice) || (num7 != this.m_dCheckCoord)) || ((this.m_dCheckX != num8) || this.m_bRepaint_Prices))
                        {
                            //Draw prices
                            this.OnPaint_Prices();
                        }
                    }
                    int length = 0;
                    if (this.Asks.Length == this.Bids.Length)
                    {
                        length = this.Asks.Length;
                    }
                    else if (this.Asks.Length < this.Bids.Length)
                    {
                        length = this.Bids.Length;
                    }
                    else if (this.Asks.Length > this.Bids.Length)
                    {
                        length = this.Asks.Length;
                    }
                    double num10 = (base.ActualWidth - this.m_iRightBorder) - (this.m_dTxtWidth * 1.2);
                    double actualWidth = base.ActualWidth;

                    sw5.Stop();
                    this.m_DrawCont = this.m_DrawVis.RenderOpen();
                    for (int k = 0; k < length; k++)
                    {
                        double num12;
                        if ((k < this.Asks.Length) && (this.Asks[k] != null))
                        {
                            double width = (((double) this.Asks[k].Amount) / this.m_iMaxAmount) * num10;
                            if (width > num10)
                            {
                                width = num10;
                            }
                            if (width > 0.0)
                            {   //first element - best ask
                                if (k == 0)
                                {   //use brush for best ask
                                    this.B = this.m_BestAskBrush;
                                    this.P = this.m_BestAskPen;
                                }
                                else
                                {  //use default brush for ask
                                    this.B = this.m_BackAskBrush;
                                    this.P = this.m_BackAskPen;
                                }
                                //Draw volumes for asks
                                if (this.m_Dict_KPriceVCoordin.TryGetValue(this.Asks[k].Price, out num12))
                                {
                                    this.m_DrawCont.DrawRectangle(this.B, this.P, new Rect(0.0, num12, (double) (((int) base.ActualWidth) - this.m_iRightBorder), (double) this.StringHeight));
                                    this.m_DrawCont.DrawRectangle(this.m_VolumeBrush, this.m_VolumePen, new Rect(0.0, num12, width, (double) this.StringHeight));
                                    ////KAA changed 2016-06-01 
                                    this.m_Txt = new FormattedText(this.Asks[k].Amount.ToString(), this.m_CultureInfo, FlowDirection.LeftToRight, this.m_TypeFace, this.FontSize, Brushes.Black);
                                    this.m_DrawCont.DrawText(this.m_Txt, new Point(5.0, num12));
                                }
                                if (this.m_Txt.Width > this.m_dVolWidth)
                                {
                                    this.m_dVolWidth = this.m_Txt.Width;
                                }
                            }
                        }
                        if ((k < this.Bids.Length) && (this.Bids[k] != null))
                        {
                            double num14 = (((double) this.Bids[k].Amount) / this.m_iMaxAmount) * num10;
                            if (num14 > num10)
                            {
                                num14 = num10;
                            }
                            if (num14 > 0.0)
                            {
                                if (k == 0)
                                {
                                    //use brush for best bid
                                    this.B = this.m_BestBidBrush;
                                    this.P = this.m_BestBidPen;
                                }
                                else
                                {  //use default brush for bid
                                    this.B = this.m_BackBidBrush;
                                    this.P = this.m_BackBidPen;
                                }
                                //Draw volumes for bids
                                if (this.m_Dict_KPriceVCoordin.TryGetValue(this.Bids[k].Price, out num12))
                                {
                                    this.m_DrawCont.DrawRectangle(this.B, this.P, new Rect(0.0, num12, (double) (((int) base.ActualWidth) - this.m_iRightBorder), (double) this.StringHeight));
                                    this.m_DrawCont.DrawRectangle(this.m_VolumeBrush, this.m_VolumePen, new Rect(0.0, num12, num14, (double) this.StringHeight));
                                    //KAA changed 2016-06-01 
                                    this.m_Txt = new FormattedText(this.Bids[k].Amount.ToString(), this.m_CultureInfo, FlowDirection.LeftToRight, this.m_TypeFace, /*this.m_iFontWeight*/this.FontSize , Brushes.Black);
                                    this.m_DrawCont.DrawText(this.m_Txt, new Point(5.0, num12));
                                }
                                if (this.m_Txt.Width > this.m_dVolWidth)
                                {
                                    this.m_dVolWidth = this.m_Txt.Width;
                                }
                            }
                        }
                    }
                    if (this.MC_Grid != null)
                    {
                        this.MC_Grid.ColumnDefinitions[4].MinWidth = ((this.m_dVolWidth + this.m_iRightBorder) + this.m_dPriceWidth) + 10.0;
                    }

                    sw4.Stop();
                    //draw levels here
                    lock (this.m_Dict_KPriceVCoordin)
                    {
                        this.FiftyLevels.Clear();
                        this.TenLevels.Clear();
                        double num27 = base.ActualWidth;
                        foreach (KeyValuePair<double, double> pair3 in this.m_Dict_KPriceVCoordin)
                        {   //draw  levels 10
                            double level10 = (10.0 * this.m_dStep) * Math.Pow(10.0, (double) this.m_iPriceDecimals);
                            if ((Math.Round((double) (pair3.Key * Math.Pow(10.0, (double) this.m_iPriceDecimals)), 2) % level10) == 0.0)
                            {
                                this.m_DrawCont.DrawLine(this.m_GridPen, new Point(0.0, pair3.Value + 7.0), new Point(base.ActualWidth, pair3.Value + 7.0));
                                this.TenLevels.Add(pair3.Value + 7.0);
                            }
                            //draw levels 50
                            double level50 = (50.0 * this.m_dStep) * Math.Pow(10.0, (double) this.m_iPriceDecimals);
                            if ((Math.Round((double) (pair3.Key * Math.Pow(10.0, (double) this.m_iPriceDecimals)), 2) % level50) == 0.0)
                            {
                                this.m_DrawCont.DrawRectangle(this.m_GridBrush, this.m_GridPen, new Rect(0.0, pair3.Value + 5.0, base.ActualWidth, 3.0));
                                this.FiftyLevels.Add(pair3.Value + 5.0);
                            }
                        }
                    }

                 
                    this.m_DrawCont.DrawLine(this.m_GridPen, new Point((double) ((int) ((base.ActualWidth - this.m_iRightBorder) - (this.m_dTxtWidth * 1.2))), 0.0), new Point((double) ((int) ((base.ActualWidth - this.m_iRightBorder) - (this.m_dTxtWidth * 1.2))), (double) ((int) base.ActualHeight)));
                    this.m_DrawCont.DrawLine(this.m_GridPen, new Point((double) (((int) base.ActualWidth) - this.m_iRightBorder), 0.0), new Point((double) (((int) base.ActualWidth) - this.m_iRightBorder), (double) ((int) base.ActualHeight)));

                    
                    //KAA TEST
                    this.m_TxtBlck_Price.Text = "100";
                    this.m_TxtBlck_Amount.Text = "4";
                    this.m_TxtBlck_Profit.Text = "10";
                    this.m_TxtBlck_Price.Background = this.m_InProfitBrush_Light;

                 
                   


                 

                    if (UserPos !=null)
                    //if (this.Portfolio != null)
                    {

                       

                        //if ((this.Portfolio.CurrentPositionAmount != 0) && (this.m_lPositionTxtBlocks != null))
                        if (UserPos.Amount != 0)
                        {

                            //KAA 2016-05-04
                            if (this.m_PositionGrid.Opacity !=1)
                                this.m_PositionGrid.Opacity = 1.0;


                            this.m_dPosPrice = 0.0;
                            this.m_dPosPrice = Math.Round(this.Portfolio.CurrentPositionPrice, this.m_iPriceDecimals);
                            this.m_dPosPrice = (this.m_dPosPrice * Math.Pow(10.0, (double) this.m_iPriceDecimals)) / this.m_dStep;
                            this.m_dPosPrice = (Math.Round(this.m_dPosPrice, 0) * this.m_dStep) / Math.Pow(10.0, (double) this.m_iPriceDecimals);
                            this.m_dPosPrice = Math.Round(this.m_dPosPrice, this.Decimals);

                            if (UserPos != null)
                            {

                                m_dPosPrice = (double)UserPos.AvPos;

                            }


                            //KAA
                            //this.m_dPosPrice = 72000;
                           // double currentPrice = Math.Round(this.Portfolio.TargetPrice, this.m_iPriceDecimals);
                            double currentPrice = (double) GetCurrenPrice(); //UserPos.AvPos;
                            
                            int currentPositionAmount = this.Portfolio.CurrentPositionAmount;
                            int num19 = (int) ((base.ActualWidth - this.m_iRightBorder) - (this.m_dTxtWidth * 1.2));
                            if (this.m_PositionGrid.Visibility == Visibility.Collapsed)
                            {
                                this.m_PositionGrid.Visibility = Visibility.Visible;
                            }
                            this.m_TxtBlck_Price.Text = this.m_dPosPrice.ToString();
                            //this.m_TxtBlck_Amount.Text = this.Portfolio.CurrentPositionAmount.ToString() + " к.";
                            this.m_TxtBlck_Amount.Text = UserPos.Amount.ToString() + " к.";
                            //Draw rectangle on price of position opened
                            if (this.m_Dict_KPriceVCoordin.ContainsKey(this.m_dPosPrice))
                            {
                                this.m_DrawCont.DrawRectangle(this.m_GridBrush, this.m_GridPen, new Rect(0.0, this.m_Dict_KPriceVCoordin[this.m_dPosPrice], base.ActualWidth, (double) this.StringHeight));
                            }
                           
                            //if (this.Portfolio.CurrentPositionAmount > 0)
                            //When opened Long (buy) pos
                            if( UserPos.Amount>0)
                            {
                                this.m_TxtBlck_Price.Background = this.m_InProfitBrush_Light;
                                this.m_TxtBlck_Amount.Background = this.m_InProfitBrush_Light;
                                //text for profit in points
                                this.m_TxtBlck_Profit.Text = Math.Round((double) ((currentPrice - this.m_dPosPrice) * Math.Pow(10.0, (double) this.m_iPriceDecimals)), this.m_iPriceDecimals).ToString() + " п.";
                                //background color for profit label                                
                                if (currentPrice > this.m_dPosPrice) //if profit
                                {
                                    this.m_TxtBlck_Profit.Background = this.m_InProfitBrush_Light;
                                }                                    
                                else if (currentPrice < this.m_dPosPrice) //if loss
                                {
                                    this.m_TxtBlck_Profit.Background = Brushes.LightCoral;
                                }
                                //calc profit in steps
                                this.m_iProfitSteps = (int) Math.Round((double) ((this.Bids[0].Price - this.m_dPosPrice) / this.m_dStep), 0);
                                //draw profit/loss area
                                if (this.m_iProfitSteps > 0) //if profit
                                {
                                    this.m_DrawCont.DrawRectangle(this.m_InProfitBrush, this.m_InProfitPen, new Rect((double) num19, (double) this.m_iFocusBid, 1.2 * this.m_dTxtWidth, (double) (this.StringHeight * (this.m_iProfitSteps + 1))));
                                }
                                else if (this.m_iProfitSteps < 0)//if loss
                                {
                                    this.m_DrawCont.DrawRectangle(this.m_InLossBrush, this.m_InLossPen, new Rect((double) num19, (double) (this.m_iFocusBid + (this.m_iProfitSteps * this.StringHeight)), 1.2 * this.m_dTxtWidth, (double) (this.StringHeight * (-this.m_iProfitSteps + 1))));
                                }
                            }
                            //else if (this.Portfolio.CurrentPositionAmount < 0)
                            //When opened short (sell) pos
                            else if (UserPos.Amount < 0)
                            {
                                this.m_TxtBlck_Price.Background = Brushes.LightCoral;
                                this.m_TxtBlck_Amount.Background = Brushes.LightCoral;
                                this.m_TxtBlck_Profit.Text = Math.Round((double) ((this.m_dPosPrice - currentPrice) * Math.Pow(10.0, (double) this.m_iPriceDecimals)), this.m_iPriceDecimals).ToString() + " п.";
                                if (currentPrice < this.m_dPosPrice)
                                {
                                    
                                    
                                   this.m_TxtBlck_Profit.Background = this.m_InProfitBrush_Light;
                                }
                                else if (currentPrice > this.m_dPosPrice)
                                {
                                    this.m_TxtBlck_Profit.Background = Brushes.LightCoral;
                                }
                                this.m_iProfitSteps = (int) Math.Round((double) ((this.m_dPosPrice - this.Asks[0].Price) / this.m_dStep), 0);
                                if (this.m_iProfitSteps > 0)
                                {
                                    this.m_DrawCont.DrawRectangle(this.m_InProfitBrush, this.m_InProfitPen, new Rect((double) num19, (double) (this.m_iFocusAsk - (this.m_iProfitSteps * this.StringHeight)), 1.2 * this.m_dTxtWidth, (double) (this.StringHeight * (this.m_iProfitSteps + 1))));
                                }
                                else if (this.m_iProfitSteps < 0)
                                {
                                    this.m_DrawCont.DrawRectangle(this.m_InLossBrush, this.m_InLossPen, new Rect((double) num19, (double) this.m_iFocusAsk, 1.2 * this.m_dTxtWidth, (double) (this.StringHeight * (-this.m_iProfitSteps + 1))));
                                }
                            }
                            if (((this.Bot != null) && (this.Bot.Layer != null)) && ((this.m_iStopLoss != 0) || (this.m_iTakeProfit != 0)))
                            {
                                double num20 = 0.0;
                                if (this.Portfolio.CurrentPositionAmount > 0)
                                {
                                    if (!this.m_bStopTake_ON)
                                    {
                                        this.m_dStopPrice = Math.Round((double) (this.m_dPosPrice - (this.m_iStopLoss * this.Step)), this.Decimals);
                                        this.m_dTakePrice = Math.Round((double) (this.m_dPosPrice + (this.m_iTakeProfit * this.Step)), this.Decimals);
                                        this.m_bStopTake_ON = true;
                                    }
                                    num20 = Math.Round((double) (this.Bids[0].Price - (this.m_iHowFarSendOrder * this.Step)), this.Decimals);
                                    if (((this.Bids[0].Price <= this.m_dStopPrice) && !this.m_bStopLossSended) && (this.m_iStopLoss != 0))
                                    {
                                        this.m_bStopLossSended = true;
                                        this.Bot.Layer.SendOrder(OrderAction.Sell, num20, (double) Math.Abs(currentPositionAmount), OrderFlags.Limit);
                                        this.Bot.WriteStopTakeInfo(this.Asks[0].Price, this.Bids[0].Price, "StopLoss", this.m_dStopPrice, this.m_iStopLoss, this.m_iHowFarSendOrder, num20, Math.Abs(currentPositionAmount), this.m_dPosPrice, this.Portfolio.CurrentPositionAmount);
                                    }
                                    if (((this.Asks[0].Price >= this.m_dTakePrice) && !this.m_bTakeProfitSended) && (this.m_iTakeProfit != 0))
                                    {
                                        this.m_bTakeProfitSended = true;
                                        this.Bot.Layer.SendOrder(OrderAction.Sell, num20, (double) Math.Abs(currentPositionAmount), OrderFlags.Limit);
                                        this.Bot.WriteStopTakeInfo(this.Asks[0].Price, this.Bids[0].Price, "TakeProfit", this.m_dTakePrice, this.m_iTakeProfit, this.m_iHowFarSendOrder, num20, Math.Abs(currentPositionAmount), this.m_dPosPrice, this.Portfolio.CurrentPositionAmount);
                                    }
                                }
                                else if (this.Portfolio.CurrentPositionAmount < 0)
                                {
                                    if (!this.m_bStopTake_ON)
                                    {
                                        this.m_dStopPrice = Math.Round((double) (this.m_dPosPrice + (this.m_iStopLoss * this.Step)), this.Decimals);
                                        this.m_dTakePrice = Math.Round((double) (this.m_dPosPrice - (this.m_iTakeProfit * this.Step)), this.Decimals);
                                        this.m_bStopTake_ON = true;
                                    }
                                    num20 = Math.Round((double) (this.Asks[0].Price + (this.m_iHowFarSendOrder * this.Step)), this.Decimals);
                                    if (((this.Asks[0].Price >= this.m_dStopPrice) && !this.m_bStopLossSended) && (this.m_iStopLoss != 0))
                                    {
                                        this.m_bStopLossSended = true;
                                        this.Bot.Layer.SendOrder(OrderAction.Buy, num20, (double) Math.Abs(currentPositionAmount), OrderFlags.Limit);
                                        this.Bot.WriteStopTakeInfo(this.Asks[0].Price, this.Bids[0].Price, "StopLoss", this.m_dStopPrice, this.m_iStopLoss, this.m_iHowFarSendOrder, num20, Math.Abs(currentPositionAmount), this.m_dPosPrice, this.Portfolio.CurrentPositionAmount);
                                    }
                                    if (((this.Bids[0].Price <= this.m_dTakePrice) && !this.m_bTakeProfitSended) && (this.m_iTakeProfit != 0))
                                    {
                                        this.m_bTakeProfitSended = true;
                                        this.Bot.Layer.SendOrder(OrderAction.Buy, num20, (double) Math.Abs(currentPositionAmount), OrderFlags.Limit);
                                        this.Bot.WriteStopTakeInfo(this.Asks[0].Price, this.Bids[0].Price, "TakeProfit", this.m_dTakePrice, this.m_iTakeProfit, this.m_iHowFarSendOrder, num20, Math.Abs(currentPositionAmount), this.m_dPosPrice, this.Portfolio.CurrentPositionAmount);
                                    }
                                }
                                if ((this.m_iStopLoss != 0) && this.m_Dict_KPriceVCoordin.ContainsKey(this.m_dStopPrice))
                                {
                                    this.m_DrawCont.DrawRectangle(this.m_InLossBrush, this.m_InLossPen, new Rect(0.0, this.m_Dict_KPriceVCoordin[this.m_dStopPrice], base.ActualWidth, (double) this.StringHeight));
                                    //KAA changed 2016-06-01 
                                    this.m_Txt = new FormattedText("SL", this.m_CultureInfo, FlowDirection.LeftToRight, this.m_TypeFace, /*this.m_iFontWeight*/this.FontSize, Brushes.Black);
                                    this.m_DrawCont.DrawText(this.m_Txt, new Point((double) ((int) (base.ActualWidth - (0.7 * this.m_iRightBorder))), this.m_Dict_KPriceVCoordin[this.m_dStopPrice]));
                                }
                                if ((this.m_iTakeProfit != 0) && this.m_Dict_KPriceVCoordin.ContainsKey(this.m_dTakePrice))
                                {
                                    ////KAA changed 2016-06-01 
                                    this.m_DrawCont.DrawRectangle(this.m_InProfitBrush, this.m_InProfitPen, new Rect(0.0, this.m_Dict_KPriceVCoordin[this.m_dTakePrice], base.ActualWidth, (double) this.StringHeight));
                                    this.m_Txt = new FormattedText("TP", this.m_CultureInfo, FlowDirection.LeftToRight, this.m_TypeFace, /*this.m_iFontWeight*/ this.FontSize, Brushes.Black);
                                    this.m_DrawCont.DrawText(this.m_Txt, new Point((double) ((int) (base.ActualWidth - (0.7 * this.m_iRightBorder))), this.m_Dict_KPriceVCoordin[this.m_dTakePrice]));
                                }
                            }
                        }
                        //if no position opened - nothing to show
                        else if (UserPos.Amount == 0)
                        //else if (this.Portfolio.CurrentPositionAmount == 0)
                        {
                            if (this.m_PositionGrid.Visibility == Visibility.Visible)
                            {
                                this.m_PositionGrid.Visibility = Visibility.Collapsed;
                            }
                            this.m_dPosPrice = 0.0;
                            this.m_bStopLossSended = false;
                            this.m_bTakeProfitSended = false;
                            this.m_bStopTake_ON = false;
                            if (this.SettingsObject != null)
                            {
                                this.m_iStopLoss = this.SettingsObject.Trading_Settings.StopLoss_Steps;
                                this.m_iTakeProfit = this.SettingsObject.Trading_Settings.TakeProfit_Steps;
                            }
                        }
                    }
                    this.m_DrawCont.Close();
                    if ((base.ActualWidth > 1.0) && (base.ActualHeight > 1.0))
                    {
                        sw3.Stop();
                      
                        this.m_Bmp = new RenderTargetBitmap((int) base.ActualWidth, (int) base.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
                        sw2.Stop();
                        this.m_Bmp.Render(this.m_DrawVis);
                        this.DOMImage.Source = this.m_Bmp;
                        sw1.Stop();
                    }
                    this.m_OldAsks = this.Asks;
                    this.m_OldBids = this.Bids;
                    _bSoreRepaint = this.m_bRepaint;
                    this.m_bRepaint = false;
                }
            }
            
            sw.Stop();

            long ms = sw.ElapsedMilliseconds;
            CheckOnPaint(ms);
            Log("Onpaint end.  MS=" + ms + " bSoreRepaint=" + _bSoreRepaint);
            _bSoreRepaint = false;
        }


        bool _bSoreRepaint = false;


        private void CheckOnPaint(long ms)
        {

            
           if (_tickerName != null )
           {
               string msg = "DOM.Onpaint " + _tickerName + " ";
               if (_maxRepaintTimeMS>0)
                _perfAnlzr.CheckLim(ms, _maxRepaintTimeMS, msg);
                    
           }
          /*  int parSinceStart = 3;
            int parMaxMsecScan = 15;

            if (ms > parMaxMsecScan)
                if (_tickerName != null && _bNotFirstScan && ((DateTime.Now - _dtSinceFirstScan).Seconds > parSinceStart))
                    Error("Onpaint " + _tickerName + " more than lim ms=" + ms);


            

            if (!_bNotFirstScan)
            {
                _bNotFirstScan = true;
                _dtSinceFirstScan = DateTime.Now;
            }
            */

        }

        private bool _bNotFirstScan = false;
        private DateTime _dtSinceFirstScan = new DateTime(0);


        public void OnPaint_Prices()
        {
            lock (this.m_Dict_KPriceVCoordin)
            {
                double num = (base.ActualWidth - this.m_iRightBorder) - (this.m_dTxtWidth * 1.1);
                this.m_dCheckPrice = this.m_Dict_KPriceVCoordin.Keys.ElementAt<double>(0);
                this.m_dCheckCoord = this.m_Dict_KPriceVCoordin.Values.ElementAt<double>(0);
                this.m_dCheckX = num;
                this.m_DrawCont_Prices = this.m_DrawVis_Prices.RenderOpen();
                foreach (KeyValuePair<double, double> pair in this.m_Dict_KPriceVCoordin)
                {
                    //KAA changed 2016-06-01 
                    this.m_Txt = new FormattedText(pair.Key.ToString("N0" + this.m_iPriceDecimals.ToString()), this.m_CultureInfo, FlowDirection.LeftToRight, this.m_TypeFace, /*this.m_iFontWeight*/this.FontSize, Brushes.Black);
                    this.m_DrawCont_Prices.DrawText(this.m_Txt, new Point((double) ((int) num), pair.Value));
                }
                this.m_DrawCont_Prices.Close();
                if ((base.ActualWidth > 1.0) && (base.ActualHeight > 1.0))
                {
                    //Rendering visual object (whole stock)
                    this.m_Bmp_Prices = new RenderTargetBitmap((int) base.ActualWidth, (int) base.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
                    this.m_Bmp_Prices.Render(this.m_DrawVis_Prices);
                    this.DOMImage_Prices.Source = this.m_Bmp_Prices;
                }
                this.m_bRepaint_Prices = false;
            }
        }

        private void RemoveOrder(OrderStatic OS)
        {
            try
            {
                _tradeOperation.CancellOrdersWithPrice(OS.Price);




            }
            catch (Exception e)
            {
                _alarmer.Error("Error RemoveOrder",e);
            }



         /*   if (this.m_ManageOrderInteractionContext != null)
            {
                if (this.m_CancelOrderInteraction == null)
                {
                    this.m_CancelOrderInteraction = (ManageOrderInteraction) this.m_ManageOrderInteractionContext.InvokeInteraction(GraphicDataInteractionType.ManageOrders);
                }
                if (this.m_CancelOrderInteraction != null)
                {
                    this.m_CancelOrderInteraction.ManageAction = ManageOrdersAction.CancelOrder;
                    Monitor.Enter(this.Portfolio);
                    for (int i = 0; i < this.Orders.Length; i++)
                    {
                        if (((long) OS.OrderStatic_TxtBlck.Tag) == this.Orders[i].OrderID)
                        {
                            this.m_CancelOrderInteraction.OrderID = this.Orders[i].OrderID;
                            this.m_ManagePortfolioSource.SendInteraction(this.m_CancelOrderInteraction);
                        }
                    }
                    Monitor.Exit(this.Portfolio);
                    this.m_bTryingToRemoveOrder = true;
                }
            }
          */
          
        }

        private void SettingsChanged_Button(object sender, RoutedEventArgs e)
        {
            if ((sender != null) && (this.SettingsObject != null))
            {
                Button button = (Button) sender;
                if (button.Tag.ToString() == "Buttn_AutoScroll")
                {
                    this.m_bAutoScroll = button.Content.ToString() != "Да";
                    this.SettingsObject.DOM_Settings.AutoScroll = this.m_bAutoScroll;
                    button.Content = (button.Content.ToString() == "Да") ? "Нет" : "Да";
                }
            }
        }

        private void SettingsChanged_Color(object sender, RoutedPropertyChangedEventArgs<Color> c)
        {
            if ((sender != null) && (this.SettingsObject != null))
            {
                ColorPicker picker = (ColorPicker) sender;
                if (picker.Tag.ToString() == "Buttn_AsksColor")
                {
                    this.m_BackAskBrush = new SolidColorBrush(c.NewValue);
                    this.m_BackAskPen = new Pen(this.m_BackAskBrush, 1.0);
                    this.SettingsObject.DOM_Settings.AsksColor = c.NewValue;
                }
                else if (picker.Tag.ToString() == "Buttn_BidsColor")
                {
                    this.m_BackBidBrush = new SolidColorBrush(c.NewValue);
                    this.m_BackBidPen = new Pen(this.m_BackBidBrush, 1.0);
                    this.SettingsObject.DOM_Settings.BidsColor = c.NewValue;
                }
            }
        }

        private void SettingsChanged_NumericUpDown(object sender, RoutedPropertyChangedEventArgs<object> o)
        {
            if (((sender != null) && (o.NewValue != null)) && (this.SettingsObject != null))
            {
                IntegerUpDown down = (IntegerUpDown) sender;
                int newValue = (int) o.NewValue;
                if (down.Tag.ToString() == "Buttn_VolumesFilledAt")
                {
                    this.m_iMaxAmount = newValue;
                    this.SettingsObject.DOM_Settings.FilledAt = newValue;
                }
                else if (down.Tag.ToString() == "Buttn_RenewSpeed_DOM")
                {
                    this.SettingsObject.DOM_Settings.RenewSpeed = newValue;
                    this.m_Timer.Interval = 0x3e8 / newValue;
                    this.m_Timer.Start();
                }
            }
        }

        public void SettingsSharing()
        {
            this.m_iMaxAmount = this.SettingsObject.DOM_Settings.FilledAt;
            this.m_BackAskBrush = new SolidColorBrush(this.SettingsObject.DOM_Settings.AsksColor);
            this.m_BackBidBrush = new SolidColorBrush(this.SettingsObject.DOM_Settings.BidsColor);
            this.m_bAutoScroll =  this.SettingsObject.DOM_Settings.AutoScroll;
            this.m_Timer.Interval = 0x3e8 / this.SettingsObject.DOM_Settings.RenewSpeed;
            this.m_Timer.Start();
            this.InitializeBrushes();
            this.m_bSettingsShared = true;
        }

        private void SettingsWinInit()
        {
            if (this.m_SettingsWin != null)
            {
                this.m_SettingsWin.Buttn_AsksColor.SelectedColorChanged += new RoutedPropertyChangedEventHandler<Color>(this.SettingsChanged_Color);
                this.m_SettingsWin.Buttn_BidsColor.SelectedColorChanged += new RoutedPropertyChangedEventHandler<Color>(this.SettingsChanged_Color);
                this.m_SettingsWin.Buttn_VolumesFilledAt.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
                this.m_SettingsWin.Buttn_AutoScroll.Click += new RoutedEventHandler(this.SettingsChanged_Button);
                this.m_SettingsWin.Buttn_RenewSpeed_DOM.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
            }
        }

        public void ShowOrderFocus(bool ShowOrNot)
        {

            double mouseX = Mouse.GetPosition(this.DOMImage).X;



            if ((ShowOrNot && (mouseX > 0.0)) && (mouseX < this.DOMImage.ActualWidth))
            {
                this.DOMOrderFocus = new OrderFocus((double) this.StringHeight);
                this.DOMOrderFocus.Width = this.DOMImage.ActualWidth;
                this.DOMOrderFocus.Height = this.StringHeight;
                this.DOMCanvas.Children.Add(this.DOMOrderFocus);
                Canvas.SetLeft(this.DOMOrderFocus, 0.0);
                Canvas.SetTop(this.DOMOrderFocus, (double) this.m_iFocusAsk);
                this.DoFocus();
            }
            else if (!ShowOrNot)
            {
                for (int i = 0; i < this.DOMCanvas.Children.Count; i++)
                {
                    if (this.DOMCanvas.Children[i] is OrderFocus)
                    {
                        this.DOMCanvas.Children.Remove(this.DOMCanvas.Children[i]);
                    }
                }
                this.DOMOrderFocus = null;
            }
        }
        //draw user order rectangles here
        private void ShowOrders(PortfolioOwnedOrder[] Orders)
        {
            for (int i = 0; i < Orders.Length; i++)
            {
                OrderStatic element = null;
                //check if order in not in dictionary m_Dict_KOrderIDVOwnedOrder
                if (!this.m_Dict_KOrderIDVOwnedOrder.ContainsKey(Orders[i].OrderID) && (Orders[i].Price != 0.0))
                {
                    element = new OrderStatic((double) this.StringHeight) {
                        Width = this.DOMImage.ActualWidth,
                        Tag = Orders[i].Action,
                        Price = Orders[i].Price
                    };
                    //record order id
                    element.OrderStatic_TxtBlck.Tag = Orders[i].OrderID;
                    //subscribe event handler to remove
                    element.TryingTo_RemoveOrder += new Action<OrderStatic>(this.RemoveOrder);
                    Panel.SetZIndex(element, 100);
                    if (Orders[i].Action == OrderAction.Sell)
                    {
                        element.OrderStatic_Border.Background = Brushes.Red;
                        element.OrderStatic_TxtBlck.Background = Brushes.LightCoral;
                    }
                    else
                    {
                        element.OrderStatic_Border.Background = Brushes.Green;
                        element.OrderStatic_TxtBlck.Background = Brushes.LimeGreen;
                    }
                    element.OrderStatic_TxtBlck.Text = Math.Abs(Orders[i].Amount).ToString();
                    Canvas.SetLeft(element, 0.0);
                    this.DOMCanvas.Children.Add(element);
                    //if orderstatic is not in  m_Dict_KPriceVOrder(first with price)
                    if (!this.m_Dict_KPriceVOrder.ContainsKey(Math.Round(Orders[i].Price, this.m_iPriceDecimals)))
                    {
                        //bind prices and orderstatics
                        this.m_Dict_KPriceVOrder.Add(Math.Round(Orders[i].Price, this.m_iPriceDecimals), element);
                    }
                    //if orderstatic is not in m_Dict_KPriceVOrder (not firs with price)
                    else if (this.m_Dict_KPriceVOrder.ContainsKey(Math.Round(Orders[i].Price, this.m_iPriceDecimals)))
                    {
                        OrderStatic static2 = this.m_Dict_KPriceVOrder[Math.Round(Orders[i].Price, this.m_iPriceDecimals)];
                        static2.OrderStatic_TxtBlck.Text = (Math.Abs(double.Parse(static2.OrderStatic_TxtBlck.Text)) + Math.Abs(Orders[i].Amount)).ToString();
                        //remove previopusly created element (TODO refact)
                        this.DOMCanvas.Children.Remove(element);
                    }
                    if (!this.m_Dict_KOrderIDVOwnedOrder.ContainsKey(Orders[i].OrderID))
                    {
                        this.m_Dict_KOrderIDVOwnedOrder.Add(Orders[i].OrderID, Orders[i]);
                    }
                    this.m_lOrderStatic.Add(element);
                }
            }
        }

        public void StopOrders_OFF()
        {
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((DOM) target).MouseWheel += new MouseWheelEventHandler(this.OnMouseWheel);
                    ((DOM) target).MouseLeave += new MouseEventHandler(this.OnMouseLeave);
                    ((DOM) target).MouseEnter += new MouseEventHandler(this.OnMouseEnter);
                    ((DOM) target).MouseMove += new MouseEventHandler(this.OnMouseMove);
                    //KAA
                    //((DOM) target).MouseDown += new MouseButtonEventHandler(this.OnMouseDown);
                    ((DOM)target).PreviewMouseDown += new MouseButtonEventHandler(this.OnMouseDown);
                    ((DOM) target).SizeChanged += new SizeChangedEventHandler(this.This_SizeChanged);
                    return;

                case 2:
                    this.DOMImage = (Image) target;
                    return;

                case 3:
                    this.DOMImage_Prices = (Image) target;
                    return;

                case 4:
                    this.DOMImage_AntiSpread = (Image) target;
                    return;

                case 5:
                    this.DOMCanvas = (Canvas) target;
                //    this.DOMCanvas.CacheMode = new BitmapCache();//KAA
                    return;
            }
            this._contentLoaded = true;
        }

        private void This_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!this.BlockPaint)
            {
                this.ChangeOrdersWidth();
                this.Repaint = true;
            }
        }

        public byte ActiveWorkAmount { get; set; }

        public string Test
        {
            get
            {
                string tmp = (string)base.GetValue(TestProperty);
                return (string )base.GetValue(TestProperty);
            }
            set
            {
                base.SetValue(TestProperty, value);
            }


        }



        public Stock_Position[] Asks
        {
            get
            {
                return (Stock_Position[]) base.GetValue(AsksProperty);
            }
            set
            {
                base.SetValue(AsksProperty, value);
            }
        }

        public Stock_Position[] Bids
        {
            get
            {
                return (Stock_Position[]) base.GetValue(BidsProperty);
            }
            set
            {
                base.SetValue(BidsProperty, value);
            }
        }

        public bool BlockPaint { get; set; }

        public IIntelliThink Bot { get; set; }

        public bool CanClick_ChangeStopOrTake { get; set; }

        public string ContractShortName
        {
            get
            {
                return this.m_sContractShortName;
            }
            set
            {
                this.m_sContractShortName = value;
            }
        }

        public int Decimals
        {
            get
            {
                return (int) base.GetValue(DecimalsProperty);
            }
            set
            {
                base.SetValue(DecimalsProperty, value);
            }
        }

        public Dictionary<double, double> Dict_KPriceVCoordin
        {
            get
            {
                return this.m_Dict_KPriceVCoordin;
            }
            set
            {
                this.m_Dict_KPriceVCoordin = value;
            }
        }

        public int DOMScrollDelta
        {
            get
            {
                return this.m_iScrollDelta;
            }
            set
            {
                this.m_iScrollDelta = value;
            }
        }

        public List<double> FiftyLevels { get; set; }

        public int FocusAsk
        {
            get
            {
                return this.m_iFocusAsk;
            }
            set
            {
                this.m_iFocusAsk = ((int) (base.ActualHeight / 2.0)) + ((this.m_iFocusAsk - this.m_iFocusBid) / 2);
                this.m_bNeedToRecount = true;
                this.Repaint = true;
            }
        }

        public int FocusBid
        {
            get
            {
                return this.m_iFocusBid;
            }
            set
            {
                this.m_iFocusBid = value;
            }
        }

        public bool FocusedByClick { get; set; }

        public GraphicDataExternalSource GraphicDataSource
        {
            get
            {
                return (GraphicDataExternalSource) base.GetValue(GraphicDataSourceProperty);
            }
            set
            {
                base.SetValue(GraphicDataSourceProperty, value);
            }
        }

        public double LastClickCoord { get; set; }

        public Grid MC_Grid { get; set; }

        public bool MouseMode { get; set; }

        public bool NewOrderbook
        {
            get
            {
                if (this.Asks == this.m_OldAsks)
                {
                    return (this.Bids != this.m_OldBids);
                }
                return true;
            }
            set
            {
            }
        }

        public PortfolioOwnedOrder[] Orders
        {
            get
            {
                return (PortfolioOwnedOrder[]) base.GetValue(OrdersProperty);
            }
            set
            {
                base.SetValue(OrdersProperty, value);
            }
        }

        public CUserPos UserPos
        {
            get
            {
                return (CUserPos)base.GetValue(UserPosProperty);
            }
            set
            {
                base.SetValue(UserPosProperty, value);
            }

        }
        public System.Timers.Timer PaintTimer
        {
            get
            {
                return this.m_Timer;
            }
        }

        public PortfolioGraphicData Portfolio
        {
            get
            {
                return (PortfolioGraphicData) base.GetValue(PortfolioProperty);
            }
            set
            {
                base.SetValue(PortfolioProperty, value);
            }
        }

        public Grid PositionGrid
        {
            get
            {
                return this.m_PositionGrid;
            }
            set
            {
                this.m_PositionGrid = value;
            }
        }

        public List<TextBlock> PositionTextBlocks
        {
            get
            {
                return this.m_lPositionTxtBlocks;
            }
            set
            {
                this.m_lPositionTxtBlocks = value;
                this.m_TxtBlck_Price = this.m_lPositionTxtBlocks[0];
                this.m_TxtBlck_Amount = this.m_lPositionTxtBlocks[1];
                this.m_TxtBlck_Profit = this.m_lPositionTxtBlocks[2];
            }
        }

        public bool Repaint
        {
            get
            {
                return this.m_bRepaint;
            }
            set
            {
                this.m_bRepaint = value;
                this.OnPaint();
            }
        }

        public bool SendNextOrderAsClose { get; set; }

        public Contract_Settings SettingsObject { get; set; }

        public SettingsWindow SettingsWin
        {
            get
            {
                return this.m_SettingsWin;
            }
            set
            {
                this.m_SettingsWin = value;
                this.SettingsWinInit();
            }
        }

        public string ShortName
        {
            get
            {
                return (string) base.GetValue(ShortNameProperty);
            }
            set
            {
                base.SetValue(ShortNameProperty, value);
            }
        }

        public double Step
        {
            get
            {
                return (double) base.GetValue(StepProperty);
            }
            set
            {
                base.SetValue(StepProperty, value);
            }
        }

        public List<CWorkAmount> ListWorkAmount
        {
            get
            {
                return (List <CWorkAmount>) base.GetValue(ListWorkAmountProperty);

            }

            set
            {
                base.SetValue(ListWorkAmountProperty,value);

            }

        }

        public string CurrAmountNum
        {
            get
            {
                return (string)base.GetValue(CurrAmountNumProperty);
            }
            set
            {
                base.SetValue(CurrAmountNumProperty, value);
               // UpdateControlAmounts();
            }
        }


        public int StopLoss
        {
            set
            {
                this.m_iStopLoss = value;
                this.m_bStopTake_ON = false;
            }
        }

      //  public int StringHeight { get; set; }

        public int TakeProfit
        {
            set
            {
                this.m_iTakeProfit = value;
                this.m_bStopTake_ON = false;
            }
        }
        //KAA
        private void DummyStock()
        {
      /*      int stockDepth = 50;
            Asks = new Stock_Position[stockDepth];
            Bids = new Stock_Position[stockDepth];

            for (int i = 0; i < stockDepth; i++)
            {
                Asks[i] = new Stock_Position();
                Bids[i] = new Stock_Position();

            }
            */
         /* Bids[0].Price = 81950; Bids[0].Amount = 2;
            Bids[1].Price = 81940; Bids[1].Amount = 1;
            Bids[2].Price = 81930; Bids[2].Amount = 30;
            Bids[3].Price = 81920; Bids[3].Amount = 8;
            Bids[4].Price = 81910; Bids[4].Amount = 12;
            Bids[5].Price = 81900; Bids[5].Amount = 10;

            Asks[0].Price = 81970; Asks[0].Amount = 3;
            Asks[1].Price = 81980; Asks[1].Amount = 5;
            Asks[2].Price = 81990; Asks[2].Amount = 13;
            Asks[3].Price = 82000; Asks[3].Amount = 9;
            Asks[4].Price = 82020; Asks[4].Amount = 30;
         */   
        }

        public List<double> TenLevels { get; set; }
    }
}

