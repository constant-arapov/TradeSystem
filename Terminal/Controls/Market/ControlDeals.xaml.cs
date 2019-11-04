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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Markup;
using System.Windows.Threading;


using Common;
using Common.Interfaces;
using Common.Utils;
using TradingLib;
using GUIComponents;


using Terminal.Common;
using Terminal.Conf;
using Terminal.TradingStructs;
using Terminal.Graphics;
using Terminal.Controls.Market.Settings;
using Terminal.Controls.Market.ChildElements;
using Terminal.Interfaces;

using Terminal.ViewModels;


namespace Terminal.Controls.Market
{
    /// <summary>
    /// Логика взаимодействия для ControlDeals.xaml
    /// </summary>
    public partial class ControlDeals : UserControl, IAlarmable
    {

        private IAlarmable _alarmer;

        private bool _bShowDealsLine = true;

        private Brush _brushBuy = new SolidColorBrush(Colors.LightGreen);
        private Brush _brushSell = new SolidColorBrush(Colors.LightCoral);

        private Brush _brushBuyBg;
        private Brush _brushSellBg;

        private Brush _brushBuyOrSell = Brushes.WhiteSmoke;
        private Brush _brushBuyOrSellBg;


        private CultureInfo _cultureInfo = new CultureInfo("en-US");
        private CultureInfo _cultureInfoBg;



        private Dictionary<double, double> _dictPriceY = new Dictionary<double, double>();
        private double _radiusMin = 4.5;

        private double _currRadius;
        private DrawingContext _drwCntxtLevels;
        private DrawingContext _drwCntxtDeals;
        private DrawingVisual _drwVisLevels = new DrawingVisual();
        private DrawingVisual _drwVisDeals = new DrawingVisual();

        private DrawingVisual _drwVisDealsBg;
        private DrawingVisual _drwVisLevelsbg;

        private DrawingContext _drwCntxtDealsBg;
        private DrawingContext _drwCntxtLevelsBg;



        private Pen _ellipsePen;
        private Pen _ellipsePenBg;



        private int _focusAsk;
        private int _focusBid;
        private int _scrollDelta;
        //KAA 2016-12-22
        private int _showTicksFrom = 0;

        private List<double> _level1y = new List<double>();
        private List<double> _level2y = new List<double>();




        private Pen _penLines = new Pen(Brushes.Black, 1.0);

        private Pen _penLinesBg;




        private Timer _timerDeals;
        private FontFamily _fontFamilyTextDefault = new FontFamily("Verdana");


        private FormattedText _formattedTextTicketAmount;
        private Typeface _typeFaceDeals;
        private Typeface _typeFaceDealsBg;


        private Brush _errorBrush = new SolidColorBrush(Colors.Black);
        private Pen _errorPen;


        private Brush _errorBrushBg = new SolidColorBrush(Colors.Black);
        private Pen _errorPenBg;


        private bool _bNeedToReadd = false;

        private CSelectionDrawer _selectionDrawer;

        private CSelectionDrawerBg _selectionDrawerBg;


        private List<string> _dpList = new List<string>();


        private CMapAmountRadius _mapAmountRadius;



        private CLevelDrawer _levelDrawer = new CLevelDrawer();
        private CRenderer _rendererDeals;
        private CRenderer _rendereLevels;


        private CRendererBackground _rendererDealsBg;
        private CRendererBackground _rendererLevelsBg;


        OrderData[] _locOrders;


        private Dictionary<long, OrderData> _dictDrawnOrders = new Dictionary<long, OrderData>();

        private Dictionary<double, ControlUserOrders> _dictPriceControlUserOrders = new Dictionary<double, ControlUserOrders>();

        private CDeal[] _dealsArrayBg = null;

        private Dispatcher _guiDispatcher;

        private Dictionary<double, double> _dictPriceYBg;


        public bool BlockPaint { get; set; }


        private int _stringHeightBg;

        public int StringHeightBg
        {
            get
            {
                return _stringHeightBg;
            }

        }



        private double _stepBg;

        private Color _colorFontBg;
        private Brush _fontBrushBg;

        private int _decimalVolumeBg;

        private int _decimalsBg;

        private int _fontSizeBg;

        public int FontSizeBg
        {
            get
            {
                return _fontSizeBg;

            }

        }



        private int _stockNum;


        private System.Threading.AutoResetEvent _evDraw;

        private OrderData[] _ordersUsersBg;


        private double _actualWidth;

        private Color _colorLevel1LineBg;
        private Color _colorLevel2LineBg;

        private Brush _brushLevel1LineBg;
        private Brush _brushLevel2LineBg;


        private decimal _buyStopAmount;
        private decimal _sellStopAmount;

        private FontFamily _fontFamilyTextDefaultBg;

        private double _stopLossPriceBg;


        private List<CLevelEl> _lstLevelEls;


        public double StopLossPriceBg
        {
            get
            {
                return _stopLossPriceBg;
            }

        }




        private double _takeProfitPriceBg;
        public double TakeProfitPriceBg
        {
            get
            {
                return _takeProfitPriceBg;
            }


        }


        private double _stopLossInvertPriceBg;
        public double StopLossInvertPriceBg
        {
            get
            {
                return _stopLossInvertPriceBg;

            }

        }


        private double _buyStopPriceBg;
        public double BuyStopPriceBg
        {
            get
            {
                return _buyStopPriceBg;
            }
        }


        private double _sellStopPriceBg;
        public double SellStopPriceBg
        {
            get
            {
                return _sellStopPriceBg;
            }

        }



        public double SelectedYBg { get; set; }


        public CSelectionMode SelectionModeBg { get; set; }

        public List<CLevelEl> LstUserLevelsBg { get; set; }
        //changed 2017-06-07
        // public int Decimals { get; set; }


        public static readonly DependencyProperty DecimalsProperty = DependencyProperty.Register("Decimals", typeof(int), typeof(ControlDeals));
        public int Decimals
        {
            get
            {
                return (int)base.GetValue(DecimalsProperty);
            }
            set
            {
                base.SetValue(DecimalsProperty, value);
            }
        }



        public Dictionary<double, double> DictPriceY { get; set; }
        

        public List<double> LstLevel1 { get; set; }

        public int FocusAsk
        {
            get
            {
                return _focusAsk;
            }
            set
            {
                _focusAsk = value;
            }
        }

        public int FocusBid
        {
            get
            {
                return _focusBid;
            }
            set
            {
                _focusBid = value;
            }
        }







        public bool MouseMode { get; set; }

        public int ScrollDelta
        {
            get
            {
                return _scrollDelta;
            }
            set
            {
                _scrollDelta = value;

                IsNeedRepaintDeals = true;
            }
        }

        public bool SendNextOrderAsClose { get; set; }






        public List<double> LstLevel2 { get; set; }


        public bool TicksFilled
        {
            set
            {

                IsNeedRepaintDeals = value;
            }
        }

        public Color ColorLevel1LineBg { get => _colorLevel1LineBg; set => _colorLevel1LineBg = value; }


        public double SelectedPriceBg {get;set;}

        







        /*
        public int OrdersCount
        {


        }
        */



        private Brush _brushBuyStop;
        private Brush _brushSellStop;
		private Brush _brushStopLoss;
		private Brush _brushTakeProfit;
		private Brush _brushStopLossInvert;


        



        private Brush _brushBuyStopBg;
        private Brush _brushSellStopBg;
        private Brush _brushStopLossBg;
        private Brush _brushTakeProfitBg;
        private Brush _brushStopLossInvertBg;



        CPerfAnlzr _perfAnlzr;




        DateTime _dbgCreated;

        public ControlDeals()
        {

            _guiDispatcher = Dispatcher.CurrentDispatcher;

            _dbgCreated = DateTime.Now;
            InitializeComponent();

            _locOrders = new OrderData[Params.MaxUserOrdersPerInstr];


            _errorPen = new Pen(_errorBrush, 1);

            _rendererDeals = new CRenderer(this, _drwVisDeals, ImageDeals);
            _rendereLevels = new CRenderer(this, _drwVisLevels, ImageLevels);

         
            _mapAmountRadius = new CMapAmountRadius(_showTicksFrom, _radiusMin);
         

           
            _ellipsePen = new Pen(Brushes.Gray, 1.0);
         
            if (_typeFaceDeals == null)
            {
                _typeFaceDeals = new Typeface(_fontFamilyTextDefault, FontStyles.Normal, FontWeights.SemiBold, new FontStretch());
            }
      
      
          
            _timerDeals = new Timer(100.0);//KAA was 45
            //_timerDeals.Elapsed += new ElapsedEventHandler(TimerDeals_Elapsed);
            //_timerDeals.Start();
           // CUtil.TaskStart(TaskPaint);


         
        
            //_timerLevels = new Timer(60.0);
			//_timerLevels.Elapsed += new ElapsedEventHandler(TimerLevels_Elapsed);
            //_timerLevels.Start();
            RenderOptions.SetEdgeMode(ImageDeals, EdgeMode.Aliased);
			//2017-03-23 changed was NearestNeighbour - because on rightest stock - bad sharpness
            RenderOptions.SetBitmapScalingMode(ImageDeals, BitmapScalingMode.LowQuality);
            TextOptions.SetTextRenderingMode(ImageDeals, TextRenderingMode.ClearType);
            TextOptions.SetTextFormattingMode(ImageDeals, TextFormattingMode.Display);



            _selectionDrawer = new CSelectionDrawer(this);
            InitBrushes();

			_perfAnlzr = new CPerfAnlzr(_alarmer);

           // CUtil.TaskStart(TaskStarterLevelsCondOrders);
           


            CUtil.ThreadStart(ThreadDrawDeals);

        }

        public void ForceRepaint()
        {
            if (_evDraw !=null)
                _evDraw.Set();
        }





        private void InitBrushes()
        {
            //TODO from settings DP etc
            _brushBuyStop = new SolidColorBrush(Color.FromArgb(0x50,0x00,0xFF,00));
            _brushSellStop = new SolidColorBrush(Color.FromArgb(0x50, 0xFF, 0x00, 00));
			_brushStopLoss = new SolidColorBrush(Color.FromArgb(0x50, 0xF0, 0x00, 0x00));
			_brushTakeProfit = new SolidColorBrush(Color.FromArgb(0x50, 0x00, 0x00, 0xE0));
			_brushStopLossInvert = new SolidColorBrush(Color.FromArgb(0x50, 0xF0, 0x00, 0x00));



        }

     



        private void DrawLevel1RectanlgeBg(double y)
        {
            Brush brush = _errorBrush;
            Pen pen = _errorPen;
            GetL1LevelBrushPenBg(ref brush, ref pen);
            double lineHeight = 2.0;

            _levelDrawer.DrawLevelRectangle(_drwCntxtLevelsBg, brush, pen, 0, y, ActualWidth, lineHeight);

            //   DrawLevelRectangle(brush, pen, 0, y, ActualWidth, lineHeight);


        }






        private void DrawLevel2LineBg(double y)
        {
            Brush brush = _errorBrushBg;
            Pen pen = _errorPenBg;
            GetL2LevelBrushPenBg(ref brush, ref pen);

            _levelDrawer.DrawLevelLine(_drwCntxtLevelsBg, pen, 0, y, ActualWidth, y);

        }



        private void GetL1LevelBrushPenBg(ref Brush brush, ref Pen pen)
        {
            if (_brushLevel1LineBg != null)
                brush = _brushLevel1LineBg;

            double thickness = 1.0;
            pen = new Pen(brush, thickness);
        }


        

        private void GetL2LevelBrushPenBg(ref Brush brush, ref Pen pen)
        {
            if (_brushLevel2LineBg != null)
                brush = _brushLevel2LineBg;

            double thickness = 1.0;
            pen = new Pen(brush, thickness);
        }


        private void GenDPList()
        {
            var props = Type.GetType("Terminal.DataBinding.CDealsProperties").GetProperties();
            props.ToList().ForEach(property => _dpList.Add(property.Name));
        }


        private DateTime _dtConToServer;
        private DateTime _dtStart = DateTime.MinValue;
        private int _secSinceStart = 3;

        private bool _isEnoughTimeSinceStart = false;


        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
			string propertyName = e.Property.Name;


            if (_dpList.Contains(e.Property.Name))
                IsNeedRepaintDeals = true;

            if (propertyName == "IsConnectedToServer")
            {
                _dtConToServer = DateTime.Now;

               // ForceRepaint();

            }
            /*
            if (!_isEnoughTimeSinceStart)
            {
                if (_dtStart == DateTime.MinValue)
                    _dtStart = DateTime.Now;
                if ((DateTime.Now - _dtStart).TotalSeconds > _secSinceStart)
                {
                    _isEnoughTimeSinceStart = true;
                    ForceRepaint();
                }
            }*/
           


            if (propertyName == "StopLossPrice" ||
                propertyName == "TakeProfitPrice" ||
                propertyName == "BuyStopPrice" ||
                propertyName == "SellStopPrice" ||
                propertyName == "StopLossInvertPrice" ||
                propertyName == "LineL1Color" ||
                propertyName == "LineL2Color" ||
                propertyName == "IsConnectedToServer" ||
                propertyName == "IsNeedRepaintDeals" ||
                propertyName == "Level1Y")
                    ForceRepaint();
					//OnPaintLevelsCondOrders();
           /*
            if (propertyName == "IsNeedRepaintDeals")
            {
               ForceRepaint();
            }
            */




        }








        private double GetRadius(int amount)
        {
            return _mapAmountRadius.GetRadius(amount);
        }



         public void BindToSystem( IAlarmable alarmer /*,ITradeOperations tradeOperations*/)
         {
             _alarmer = alarmer;
           
         }

         public void Error(string msg, Exception e=null)
         {
            if (_alarmer !=null)
             _alarmer.Error(msg,e);


         }


      
        


        /// <summary>
        /// Draw ribbon with deals circles and lines.
        /// Iterate last N deals, getting radius of circles (depend on amount)
        /// and draw circles with deal's amount and lines beetwen them
        /// Drawing on whole current  width of ControlDeals area
        /// 
        /// </summary>
        private void DrawDealsRibbonBg()
        {

            //KAA 2016_11_03
            double parOffset = 2;// 4;//for 97 dpi
            //create vars before iteration
            double y = 0.0;
            double x = 0.0;
            double yText = 0.0;
            double priceHighest = 0.0;
            double yLowest = 0.0;
            Point point = new Point(0.0, 0.0);
          


          
            if (_dealsArrayBg == null || _dealsArrayBg.Length == 0)
                return;

            //get pice and y for top size of screen first
            if (_dictPriceYBg.Count > 0)
            {
                KeyValuePair<double, double> pair = _dictPriceYBg.ElementAt<KeyValuePair<double, double>>(0);
                priceHighest = pair.Key;
                yLowest = pair.Value;
            }
            //initial get controll width
            double actualWidth = base.ActualWidth - parOffset;

            CDeal currDeal;
            int i;

            /*  Iterate from last (newest) down to the first (latest).
             *  During each iteration do decrease actualWidth on radius, so it 
             *  becomes less. When it is less than 0 do break the iteration.
             *  Also draeing line
             */
            for (i = _dealsArrayBg.Length - 1; i > -1 && actualWidth >= 0; i--)
            {
                currDeal = _dealsArrayBg[i];
                //Is it is possible ?
                //TODO check and remove
                if (currDeal == null)
                    break;

                //protection
                if (currDeal.Amount <= 0)
                    continue;

                _currRadius = GetRadius(currDeal.Amount);
                actualWidth -= _currRadius;

                if (_bShowDealsLine)
                {  //dtaw lines beetwen cirlces 
                    y = (yLowest + (_stringHeightBg * Math.Round((double)((priceHighest - currDeal.Price) / _stepBg), 0))) + (_stringHeightBg / 2);
                    if (i != (_dealsArrayBg.Length - 1))
                        _drwCntxtDealsBg.DrawLine(_penLinesBg, point, new Point(actualWidth, y));

                }

                point = new Point(actualWidth, y);
            }



            /* When actualWidth becomes less than zero that means that deals ribbon's width
             * is more than control's width and we could redraw ribbon from current index
             * to last (latest) e.g from left to right
            */
            i++; //fiting iterator

            for (int j = i; j < _dealsArrayBg.Length; j++)
            {
                currDeal = _dealsArrayBg[j];


                if (j != i) //not do on the first iteration for fitting
                    actualWidth += _currRadius;

                _currRadius = GetRadius(currDeal.Amount);


                // int tmp = 0;
                // if (TickerName == "BTCUSD")
                //  tmp = 1;

                //if price exist on screen draw deal circle and text amount
                if (_dictPriceYBg.ContainsKey(currDeal.Price))
                {

                    _formattedTextTicketAmount = GetAmountDealTextBg(currDeal.Amount);



                    //calculate position of circle's center
                    y = _dictPriceYBg[currDeal.Price] + (_stringHeightBg / 2);
                    x = actualWidth - (_formattedTextTicketAmount.Width / 2.0);
                    yText = y - (_formattedTextTicketAmount.Height / 2.0);
                    //draw circles with deals

                    DrawDealCircleBg (new Point(actualWidth, y), _currRadius, currDeal.Direction);


                    if (currDeal.Amount > _showTicksFrom)
                       _drwCntxtDealsBg.DrawText(_formattedTextTicketAmount, new Point(x, yText));

                }
            }



        }
     

        

        private FormattedText GetAmountDealTextBg(int amount)
        {

            Brush brush = Brushes.Black;
            if (_fontBrushBg != null)
                brush = _fontBrushBg; ;


            //2018-02-21
            string stAmount = CTerminalUtil.GetAmount(amount, _decimalVolumeBg );


            return new FormattedText(stAmount, _cultureInfo, FlowDirection.LeftToRight, _typeFaceDeals,
                                       _fontSizeBg, brush);


        }




        private void DrawDealCircleBg(Point center, double radius, EnmDealDirection dir)
        {
            if (dir == EnmDealDirection.Buy)
                _brushBuyOrSellBg = _brushBuyBg;
            else
                _brushBuyOrSellBg = _brushSellBg;

            //_drwCntxtDeals.DrawEllipse(_brushBuyOrSell, _ellipsePen, center, radius, radius);

            _drwCntxtDealsBg.DrawEllipse(_brushBuyOrSellBg, _ellipsePenBg, center, radius, radius);



        }


        private void DrawSelectionBg()
        {
            _selectionDrawerBg.Draw(_drwCntxtLevelsBg);
        }




        private void DrawUserLevelHighlightBg(double y, DateTime dt)
        {
            if (_actualWidth <= 0)
                return;


            Brush brush = new SolidColorBrush(Colors.Yellow);
            brush.Opacity = 0.5;
            Pen pen = new Pen(new SolidColorBrush(Colors.Black), 1.0);

            _drwCntxtLevelsBg.DrawRectangle(brush, pen, new Rect(0, y, _actualWidth, _stringHeightBg));
            string text = dt.Day.ToString("D2") + "." + dt.Month.ToString("D2");
            FontFamily ff = new FontFamily("Arial");
            Typeface typeface = new Typeface(ff, FontStyles.Normal, FontWeights.Normal, new FontStretch());
            int sz = Math.Max(_stringHeightBg - 1, 1);

            FormattedText formattedText = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, sz, new SolidColorBrush(Colors.Black));
            double xOffset = 30;
            double yOffset = 0;
            _drwCntxtLevelsBg.DrawText(formattedText, new Point(xOffset, y - yOffset));




        }















        

        private void DrawUserLevelsBg()
        {

            if (_lstLevelEls == null)
                return;

          

            try
            {
               // lock (_dictPriceYBg)
                {
                    foreach (var kvp in _dictPriceYBg)
                    {
                        double price = kvp.Key;
                        double y = kvp.Value;
                        var res = _lstLevelEls.Find(el => el.Price == price);
                        if (res != null)
                            //DrawUserLevelRectangle(price, y + StringHeight / 2);
                            DrawUserLevelHighlightBg(y, res.Dt);




                    }
                }
            }
            catch (Exception e)
            {
                Error("DrawUserLevels", e);
            }


            //DrawUserLevelHighlight();

        }

        



        public void ThreadDrawDeals()
        {

            
            _drwVisDealsBg = new DrawingVisual();
            _drwVisLevelsbg = new DrawingVisual();

            _rendererDealsBg = new CRendererBackground (this, _drwVisDealsBg, ImageDeals, _guiDispatcher);
            _rendererLevelsBg = new CRendererBackground(this, _drwVisLevelsbg, ImageLevels, _guiDispatcher);

             _colorFontBg = Colors.White;

            _fontBrushBg = new SolidColorBrush(_colorFontBg);

            _brushBuyBg = new SolidColorBrush(Colors.LightGreen);
            _brushSellBg = new SolidColorBrush(Colors.LightCoral);
            _ellipsePenBg = new Pen(Brushes.Gray, 1.0);

            _penLinesBg = new Pen(Brushes.Black, 1.0);

            _errorBrushBg = new SolidColorBrush(Colors.Red);

            _errorPenBg = new Pen(Brushes.Black, 1.0);


             _evDraw = new System.Threading.AutoResetEvent(true);

            _ordersUsersBg =  new OrderData[Params.MaxUserOrdersPerInstr];


            _colorLevel1LineBg = Colors.White;
            _colorLevel2LineBg = Colors.White;

            _brushLevel1LineBg = new SolidColorBrush(_colorLevel1LineBg);
            _brushLevel2LineBg = new SolidColorBrush(_colorLevel2LineBg);

            _brushBuyStopBg = new SolidColorBrush(Color.FromArgb(0x50, 0x00, 0xFF, 00));
            _brushSellStopBg = new SolidColorBrush(Color.FromArgb(0x50, 0xFF, 0x00, 00));
            _brushStopLossBg = new SolidColorBrush(Color.FromArgb(0x50, 0xF0, 0x00, 0x00));
            _brushTakeProfitBg = new SolidColorBrush(Color.FromArgb(0x50, 0x00, 0x00, 0xE0));
            _brushStopLossInvertBg = new SolidColorBrush(Color.FromArgb(0x50, 0xF0, 0x00, 0x00));

            _selectionDrawerBg = new CSelectionDrawerBg(this);

            _cultureInfoBg = new CultureInfo("en-US");

            _fontFamilyTextDefaultBg = new FontFamily("Verdana");

            while (true)
            {
                try
                {
                    _evDraw.WaitOne(10000);

                    _guiDispatcher.Invoke(
                        new Action(() =>
                       {
                           try
                           {

                               IsNeedRepaintDeals = false;

                               CTerminalUtil.UpdateLocalColors(FontColor, ref _colorFontBg);
                               CTerminalUtil.UpdateLocalColors(LineL1Color, ref _colorLevel1LineBg);
                               CTerminalUtil.UpdateLocalColors(LineL2Color, ref _colorLevel2LineBg);

                               _dealsArrayBg = Ticks.ToArray();

                               lock (DictPriceY)
                                    _dictPriceYBg = new Dictionary<double, double>(DictPriceY);

                               _stringHeightBg = StringHeight;

                               _stepBg = Step;

                               _decimalVolumeBg = DecimalVolume;

                               _decimalsBg = Decimals;

                               _fontSizeBg = FontSize;

                               if (Orders!=null)
                                  Orders.CopyTo(_ordersUsersBg, 0);

                               

                               _actualWidth = ActualWidth;

                               _stockNum = StockNum;

                               _level1y = Level1Y.ToArray().ToList();
                               _level2y = Level2Y.ToArray().ToList();


                               _buyStopAmount = BuyStopAmount;
                               _sellStopAmount = SellStopAmount;
                               _stopLossPriceBg = StopLossPrice;
                               _takeProfitPriceBg = TakeProfitPrice;
                               _stopLossInvertPriceBg = StopLossInvertPrice;
                               _buyStopPriceBg = BuyStopPrice;
                               _sellStopPriceBg = SellStopPrice;



                               SelectedYBg = SelectedY;
                               SelectedPriceBg = SelectedPrice;


                               if (UserLevels!=null)
                                LstUserLevelsBg = UserLevels.GetCopy();

                               SelectionModeBg = new CSelectionMode()
                               {
                                   IsModeDrawLevel = SelectionMode.IsModeDrawLevel,
                                   IsModeRestOrder = SelectionMode.IsModeRestOrder,
                                   IsModeStopLossInvert = SelectionMode.IsModeStopLossInvert,
                                   IsModeStopLossTakeProfit = SelectionMode.IsModeStopLossTakeProfit,
                                   IsModeStopOrder = SelectionMode.IsModeStopOrder
                               };

                               //if (SelectionModeBg.IsModeStopOrder)
                                 //  sSystem.Threading.Thread.Sleep(0);


                               if (UserLevels!=null)
                                    _lstLevelEls = UserLevels.GetCopy();

                               DrawUserOrders();

                             

                           }
                           catch (Exception e)
                           {
                               Error("ThreadDrawDeals.GUI", e);
                           }
                          
                       }
                        ));


                    CTerminalUtil.UpdateBrush(_colorFontBg, ref _fontBrushBg);
                    CTerminalUtil.UpdateBrush(_colorLevel1LineBg, ref _brushLevel1LineBg);
                    CTerminalUtil.UpdateBrush(_colorLevel2LineBg, ref _brushLevel2LineBg);


                    _drwCntxtLevelsBg = _drwVisLevelsbg.RenderOpen();

                    DrawLevelsBg();
                    DrawCondOrdersBg();
                    DrawSelectionBg();
                    DrawUserLevelsBg();

                    _drwCntxtLevelsBg.Close();
                    _rendererLevelsBg.Render();

                    _drwCntxtDealsBg = _drwVisDealsBg.RenderOpen();

                   
                    DrawDealsRibbonBg();


                    _drwCntxtDealsBg.Close();
                    _rendererDealsBg.Render();
                   

                    //System.Threading.Thread.Sleep(100);

                }
                catch (Exception exc)
                {

                    Error("ThreadDrawDeals",exc);
                    System.Threading.Thread.Sleep(100);

                }




            }






        }




        private double GetNormPrice(double rawPrice)
        {
            return Math.Round(rawPrice, Decimals);
        }

        private double GetNormPriceBg(double rawPrice)
        {
            return Math.Round(rawPrice, _decimalsBg);
        }



        private ControlUserOrders CreateControlUserOrders(OrderData order)
        {
            ControlUserOrders element = new ControlUserOrders((double)StringHeight, StockNum)
            {
                Width = ActualWidth,
                Tag = order.Action,
                Price = order.Price
            };
            //record order id
            element.OrderStatic_TxtBlck.Tag = order;
           
            //TODO remove magic number
            Panel.SetZIndex(element, 100);
            if (order.Action == EnmOrderAction.Sell)
            {
                element.OrderStatic_Border.Background = Brushes.Red;
                element.OrderStatic_TxtBlck.Background = Brushes.LightCoral;
            }
            else
            {
                element.OrderStatic_Border.Background = Brushes.Green;
                element.OrderStatic_TxtBlck.Background = Brushes.LimeGreen;
            }
            element.OrderStatic_TxtBlck.Text = Math.Abs(order.Amount).ToString();
            Canvas.SetLeft(element, CanvasControlDeals.ActualWidth - element.Width);
            CanvasControlDeals.Children.Add(element);

            return element;

        }

        private ControlUserOrders CreateControlUserOrdersBg(OrderData order)
        {
            ControlUserOrders element = new ControlUserOrders((double)_stringHeightBg, _stockNum)
            {
                Width = _actualWidth,
                Tag = order.Action,
                Price = order.Price
            };
            //record order id
            element.OrderStatic_TxtBlck.Tag = order;

            //TODO remove magic number
            Panel.SetZIndex(element, 100);
            if (order.Action == EnmOrderAction.Sell)
            {
                element.OrderStatic_Border.Background = Brushes.Red;
                element.OrderStatic_TxtBlck.Background = Brushes.LightCoral;
            }
            else
            {
                element.OrderStatic_Border.Background = Brushes.Green;
                element.OrderStatic_TxtBlck.Background = Brushes.LimeGreen;
            }
            element.OrderStatic_TxtBlck.Text = Math.Abs(order.Amount).ToString();
            Canvas.SetLeft(element, CanvasControlDeals.ActualWidth - element.Width);
            CanvasControlDeals.Children.Add(element);

            return element;

        }




        /// <summary>
        /// Draw user order rectangles.
        /// Compares Orders with _dictDrawnOrders
        /// if different adds order to 
        /// _dictDrawnOrders and draw ControlUserOrders  
        /// or edit amount.
        /// 
        /// </summary>
        private void RedrawControlOrders()
        {
            for (int i = 0; i < _locOrders.Length; i++)
            {
                double price = GetNormPrice(_locOrders[i].Price);

                if (price == 0.0)
                    continue;

                //Check if order is not in _dictDrawnOrders
                if (!_dictDrawnOrders.ContainsKey(_locOrders[i].OrderID))
                {

                    //if ControlUserOrder IS NOT IN  dictPriceControlUserOrders(first with price)
                    if (!_dictPriceControlUserOrders.ContainsKey(price))
                    {
                        //draw new
                        ControlUserOrders controlUserOrders = CreateControlUserOrders(_locOrders[i]);
                        //bind prices and ControlUserOrder
                        _dictPriceControlUserOrders.Add(price, controlUserOrders);
                    }
                    //if orderstatic IS ALREADY IN dictPriceControlUserOrders (not first with price)
                    else
                    {
                        //just update
                        ControlUserOrders controlUserOrders = _dictPriceControlUserOrders[price];
                        controlUserOrders.OrderStatic_TxtBlck.Text = (Math.Abs(decimal.Parse(controlUserOrders.OrderStatic_TxtBlck.Text)) + Math.Abs(_locOrders[i].Amount)).ToString();
                   
                    }



                    _dictDrawnOrders.Add(_locOrders[i].OrderID, _locOrders[i]);
                    

                }
            }
        }


        /// <summary>
        /// Draw user order rectangles.
        /// Compares Orders with _dictDrawnOrders
        /// if different adds order to 
        /// _dictDrawnOrders and draw ControlUserOrders  
        /// or edit amount.
        /// 
        /// </summary>
        private void RedrawControlOrdersBg()
        {
            for (int i = 0; i < _ordersUsersBg.Length; i++)
            {
                double price = GetNormPriceBg(_ordersUsersBg[i].Price);

                if (price == 0.0)
                    continue;

                //Check if order is not in _dictDrawnOrders
                if (!_dictDrawnOrders.ContainsKey(_ordersUsersBg[i].OrderID))
                {

                    //if ControlUserOrder IS NOT IN  dictPriceControlUserOrders(first with price)
                    if (!_dictPriceControlUserOrders.ContainsKey(price))
                    {
                        //draw new
                        ControlUserOrders controlUserOrders = CreateControlUserOrdersBg(_ordersUsersBg[i]);
                        //bind prices and ControlUserOrder
                        _dictPriceControlUserOrders.Add(price, controlUserOrders);
                    }
                    //if orderstatic IS ALREADY IN dictPriceControlUserOrders (not first with price)
                    else
                    {
                        //just update
                        ControlUserOrders controlUserOrders = _dictPriceControlUserOrders[price];
                        controlUserOrders.OrderStatic_TxtBlck.Text = (Math.Abs(decimal.Parse(controlUserOrders.OrderStatic_TxtBlck.Text)) + Math.Abs(_locOrders[i].Amount)).ToString();

                    }



                    _dictDrawnOrders.Add(_locOrders[i].OrderID, _locOrders[i]);


                }
            }
        }














        /// <summary>
        /// Set top position of ControlUserOrders depend on screen window
        /// </summary>
        private void SetControlOrdersTop()
        {

            foreach (KeyValuePair<double, ControlUserOrders> pair2 in _dictPriceControlUserOrders)
            {
                ControlUserOrders userOrders = pair2.Value;
                double price = pair2.Key;

                //if order price is IN current screen area draw it on specific position, 
                //else draw on top for sell, draw on bottom for buy

                double y = 0;
                lock (DictPriceY)
                {
                    if (DictPriceY.ContainsKey(price))
                        y = DictPriceY[price];

                }
                                
                if (y!=0)
                    Canvas.SetTop(userOrders, y);                
                else if (((EnmOrderAction)userOrders.Tag) == EnmOrderAction.Sell)
                {
                    Canvas.SetTop(userOrders, 0.0);
                }
                else if (((EnmOrderAction)userOrders.Tag) == EnmOrderAction.Buy)
                {
                  
                    Canvas.SetTop(userOrders, ActualHeight - userOrders.Height);
                }
            }

        }



        public void OnPriceCoordChanged(object sender, EventArgs e)
        {
            UpdateLevelsAndCondOrders();

        }


        






        private void DrawUserOrders()
        {
            if (Orders == null)
                return;

            lock (Orders)
            {
                Orders.CopyTo(_locOrders,0);

            }


           //Draw user order rectangles here
            RedrawControlOrders();

            //Two cases:
            //1) Equal order's count but different amount of one orders.
            //TODO check is this case possible (never seen before)
            if (_locOrders.Length == _dictDrawnOrders.Count)
                for (int i = 0; i < _locOrders.Length; i++)
                    if (_dictDrawnOrders.ContainsKey(_locOrders[i].OrderID) //orderWith specific Id 
                        && (_dictDrawnOrders[_locOrders[i].OrderID].Amount != _locOrders[i].Amount))//has different amont                        
                        _bNeedToReadd = true;



            //2) Not equal order's count
            if ((_locOrders.Length != _dictDrawnOrders.Count) || _bNeedToReadd)
            {
                //DO remove all ControlUserOrders
                //than redraw it again

                //Note: Although we remove all child ControlUserOrders bellow
                //this cycle must be to protect against "hanging" orders
                foreach (KeyValuePair<double, ControlUserOrders> pair in _dictPriceControlUserOrders)
                {
                    ControlUserOrders userOrders = pair.Value;
                    if (CanvasControlDeals.Children.Contains(userOrders))
                        CanvasControlDeals.Children.Remove(userOrders);

                }

                //Remove ALL ControlUserOrders
                for (int j = 0; j < CanvasControlDeals.Children.Count; j++)
                    if (CanvasControlDeals.Children[j] is ControlUserOrders)
                        CanvasControlDeals.Children.Remove(CanvasControlDeals.Children[j]);

                //clear all dictionaries
                _dictPriceControlUserOrders.Clear();
                _dictDrawnOrders.Clear();
                //redraw (all ControlUserOrders was previously removed)
                RedrawControlOrders();
                _bNeedToReadd = false;
            }
            SetControlOrdersTop();


        }
        



        

        private void DrawConditionOrderBarBg(double price, Brush brushBar, string title)
        {
            if (price == 0)
                return;

            double coordPrice;

            if (_dictPriceYBg.TryGetValue(price, out coordPrice))
            {

                Pen pen = new Pen(brushBar, 1.0);

                Brush brushFont = Brushes.Black;
                if (_fontBrushBg != null)
                    brushFont = _fontBrushBg;



                _drwCntxtLevelsBg.DrawRectangle(brushBar, pen, new Rect(0.0, coordPrice, _actualWidth, (double)_stringHeightBg));
                //string text = title+" "+amount.ToString();
                Typeface typeFace = new Typeface(_fontFamilyTextDefaultBg, FontStyles.Normal, FontWeights.Normal, new FontStretch());
                var amountText = new FormattedText(title, _cultureInfoBg, FlowDirection.LeftToRight, typeFace, _fontSizeBg, brushFont);
                _drwCntxtLevelsBg.DrawText(amountText, new Point(10, coordPrice));

            }


        }






        private void DrawCondOrdersBg()
        {
            if (_buyStopAmount != 0)
                DrawConditionOrderBarBg(_buyStopPriceBg, _brushBuyStopBg, "BuyStop:" + _buyStopAmount.ToString());

            if (_sellStopAmount != 0)
                DrawConditionOrderBarBg(_sellStopPriceBg, _brushSellStopBg, "SellStop:" + _sellStopAmount.ToString());


            if (_stopLossPriceBg != 0)
                DrawConditionOrderBarBg(_stopLossPriceBg, _brushStopLossBg, "StopLoss");

            if (_takeProfitPriceBg != 0)
                DrawConditionOrderBarBg(_takeProfitPriceBg, _brushTakeProfitBg, "TakeProfit");

            if (_stopLossInvertPriceBg != 0)
                DrawConditionOrderBarBg(_stopLossInvertPriceBg, _brushStopLossInvertBg, "StopLossInvert");



        }


        

        private void DrawLevelsBg()
        {

            foreach (var y in _level1y)
                DrawLevel1RectanlgeBg(y);

            foreach (var y in _level2y)
                DrawLevel2LineBg(y);


        }






        private System.Threading.AutoResetEvent _evUpdLevelsCondOrders = new System.Threading.AutoResetEvent(false);
        private int _parLevelsRept = 1000000000;//100




        public void UpdateLevelsAndCondOrders()
        {
            _evUpdLevelsCondOrders.Set();

        }


       



        public void ChangeOrdersWidth()
        {

            foreach (KeyValuePair<double, ControlUserOrders> pair in _dictPriceControlUserOrders)
            {
                pair.Value.Width = ActualWidth;
            }



        }

    
        private void ControlDeals_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!BlockPaint)
            {
           
                IsNeedRepaintDeals = true;
                ChangeOrdersWidth();
                UpdateLevelsAndCondOrders();
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            IsInControlDeals = false;
          //  SelectionMode.ResetAllModes();
         
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {

        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (_dictDrawnOrders.Count >0)
                IsInControlDeals = true;
        }

     



    }
}
