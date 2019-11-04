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
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using System.Timers;
using System.Windows.Threading;
using System.Runtime.CompilerServices;

using Common;
using Common.Interfaces;
using Common.Logger;
using Common.Utils;

using TradingLib;
using TradingLib.Enums;
using TradingLib.ProtoTradingStructs;

using Terminal.Common;
using Terminal.Interfaces;
using Terminal.TradingStructs;

using Terminal.Graphics;
using Terminal.Controls.Market.ChildElements;
using Terminal.Controls.Market.Settings;
using Terminal.Events;
using Terminal.Events.Data;
using Terminal.ViewModels;

namespace Terminal.Controls.Market
{
    /// <summary>
    /// Логика взаимодействия для ControlStock.xaml
    /// </summary>
    public partial class ControlStock : UserControl, IStockNumerable
    {


        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //
        //                                   F I E L D S  A R E A
        //
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++




        private CLogger _logger;
        private string _tickerName;
        private IAlarmable _alarmer;
        CPerfAnlzr _perfAnlzr;

        private CDOUserPos _doUserPos;

        private bool _isInStockArea;

        private ControlFocusBar CntrlFocusBar;
        private bool _autoScroll = true;
        private Brush _errorBrush;
        private Pen _penError;

        private bool _bNeedScrollAllControls = true;
        private bool _needRepaint = true;
        private bool _bSoreRepaint = false;
        private bool _isNeedRepaintPrices = true;
     


        private CultureInfo _cultureInfo = new CultureInfo("en-US");
        private double _oldHighestY;
        private double _oldMinPrice;
        private double _oldDrawnPriceX;
        private double _focusSpeed = 0.25;
        private double _oldLowestAskY;

        private Dictionary<long, OrderData> _dictDrawnOrders = new Dictionary<long, OrderData>();
        private Dictionary<double, double> _dictPriceY = new Dictionary<double, double>();
      

        private List<double> _level1y = new List<double>();
        private List<double> _level2y = new List<double>();


        private Dictionary<double, ControlUserOrders> _dictPriceControlUserOrders = new Dictionary<double, ControlUserOrders>();

        private double _prevLowestAskPrice;
        private double _prevFirstY;
        private double _prevLastY;
        private double _prevFirstPrice;
        private double _prevLastPrice;



        private double _priceWidth;
        private DrawingContext _drwCntxtStockPos;
        private DrawingContext _drwCntxtPrices;
        private DrawingVisual _drwVisStockPos;
        private DrawingVisual _drwVisPrices;
        private double _step = 10;//KAA
        private int _stringHeight;
        private int _fontSize;
        private bool _isConnectedToServer = false;
        private decimal _volumeFullBar;
        private int _deicmals;
        private int _decimalVolume;
        private decimal _bigVolume;
        private double _level1Mult;
        private double _level2Mult;
        private double _stoplossPrice;
        private double _takeProfitPrice;
        private double _stopLossInvertPrice;
        private double _buyStopPrice;
        private double _sellStopPrice;
        private bool _isInControlDeals;

        private bool _isPosActive;
        private decimal _avPos;
        private bool _isBuy;
        private bool _isSell;
        private bool _isProfit;
        private bool _isThreadDrawStock = true;
        private bool _isThreadSetFocus = true;
        private double _currentPrice;
        private double _profitInPrice;
        private double _profitInStep;
       




        private Brush _brushFontColor;
        private Brush _brushAsk;
        private Brush _brushBid;
        private Brush _brushBestBid;
        private Brush _brushBestAsk;

        private Color _colorFont;
        private Color _colorAsk;
        private Color _colorBid;
        private Color _colorBestBid;
        private Color _colorBestAsk;
        private Color _colorVolumeBar;

        private Typeface _typeFacePrices;
        private CultureInfo _cultureInfoPrices;

        private double _textPriceWidth;
        private double _parScaleTextPriceWidth = 1.2;
        private double _scaledTextPriceWidth;

        private double _volumeWidth;


        private Brush _brushGrid;
        private Pen _penGrid;
        private int _lowestAskY;
        private int _highestBidY;





        private int _parScrollMouseStepPxls = 130;
        private Brush _brushLoss;
        private Brush _brushBigVolume;
        private Pen _penBigBolume;
        private Pen _penLoss;
        private Brush _brushProfit; 
        private Brush _brushProfitLight;
        private Pen _penProfit;

        private int _priceDecimals = 0;


        private List<double> _lstUserLines = new List<double>();


        private DateTime _dtLastStockWidthChange = new DateTime();
        private bool _isInStockChangeMonitoring = false;

        private bool _isEnoughTimeSinceStart = false;
        private DateTime _dtFirst = DateTime.MinValue;
        private int _parSecSinceStart = 60;

        private DateTime _dtLastRepaintOnMsMv = new DateTime(0);
        private double _parRepaintMsMvMs = 100;


        private AutoResetEvent _evDrawStock = new AutoResetEvent(true);

        private AutoResetEvent _evDrowCondOrdLevs = new AutoResetEvent(true);


        private CStockPosition[] _asksLocal;
        private CStockPosition[] _bidsLocal;

        private CStockPosition[] _oldAsks;
        private CStockPosition[] _oldBids;

        private int _scrollDelta;



        private Grid _positionGrid;




        private string _shortName = string.Empty;


        private FormattedText _textPrice;
       
        private FontFamily _txtFontFamDefault = new FontFamily("Verdana");
        private Typeface _typeFaceDefault;
        private Brush _brushVolume = new SolidColorBrush(Colors.Orange);
        private Pen _penVolume;

        private double _currMinPrice;
        private double _currHighestY;
        private double _currPriceX;


        private CRendererBackground _rendererStockPositions;
        private CRendererBackground _rendererPrices;
        private CGlyphGenerator _glythGen = new CGlyphGenerator();



        private bool _bIsNeedAutoCentring = false;
        private bool _bIsNeedDrawPriceAfterCentring = false;



        public bool CanClick_ChangeStopOrTake { get; set; }



        public event EventHandler DoScrollAllControls;

        public event EventHandler PriceCoordChanged;
     
       

        private long _maxRepaintTimeMS;



        CLevelDrawer _levelDrawer = new CLevelDrawer();

        private List<string> _dpList = new List<string>();

        private double _prevWidth;
        private AutoResetEvent _evFocus = new AutoResetEvent(true);




       

      
        private Dispatcher _guiDispatcher;


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //
        //                                   P R O P E R T I E S  A R E A
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        //KAA Change 2016_12_19
        public bool Repaint
        {
            get
            {
                return _needRepaint;
            }
            set
            {
                _needRepaint = value;
                // OnPaint();
                //2018-08-09
                ForceRepaint();
            }
        }

        public Grid PositionGrid
        {
            get
            {
                return _positionGrid;
            }
            set
            {
                _positionGrid = value;
            }
        }

        public List<double> FiftyLevels { get; set; }

        public int FocusAsk
        {
            get
            {
                return _lowestAskY;
            }
            set
            {
                _lowestAskY = ((int)(base.ActualHeight / 2.0)) + ((_lowestAskY - _highestBidY) / 2);
                _bNeedScrollAllControls = true;
                _isNeedRepaintPrices = true;
                Repaint = true;
            }
        }

        public byte ActiveWorkAmount { get; set; }



        public List<double> TenLevels { get; set; }
        public bool BIsNeedDrawPriceAfterCentring { get => _bIsNeedDrawPriceAfterCentring; set => _bIsNeedDrawPriceAfterCentring = value; }


        public Dictionary<double, double> DictPriceY
        {
            get
            {
                return _dictPriceY;
            }
            set
            {
                _dictPriceY = value;
            }
        }

        public int ScrollDelta
        {
            get
            {
                return _scrollDelta;
            }
            set
            {
                _scrollDelta = value;
            }
        }


        public bool FocusedByClick { get; set; }

        public Grid GridControlMarket { get; set; }

        public bool MouseMode { get; set; }
        public bool SendNextOrderAsClose { get; set; }

        public bool IsInitializeComplete { get; set; }


        public CDOUserPos DOUSerPos
        {
            get
            {
                return _doUserPos;
            }
        }

        public bool IsInStockArea
        {
            get
            {
                return _isInStockArea;
            }

        }


        /// <summary>
        /// Set ControlFocusBar coordinate based on current mouse cursor 
        /// coordinates
        /// 
        /// Called from 
        /// 1) ShowOrderFocus
        /// 2) OnMouseMove
        /// </summary>
        private void DoFocus()
        {

            try
            {
                _evFocus.Set();
                return;

            }
            catch (Exception e)
            {
                Error("DoFocus", e);
            }


        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //
        //                                   M E T H O D S  A R E A
        //
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public ControlStock()
        {
           
            InitializeComponent();

            _guiDispatcher = Dispatcher.CurrentDispatcher;

            IsInitializeComplete = false;

            (new Thread(ThreadSetFocus)).Start();


            GenDPList();

         
            FiftyLevels = new List<double>();
            TenLevels = new List<double>();

            if (_typeFaceDefault == null)
            {
                _typeFaceDefault = new Typeface(_txtFontFamDefault, FontStyles.Normal, /*FontWeights.Normal*/FontWeights.ExtraLight, new FontStretch());
            }

          

            //2017-03-26 Aliased -> Unspecified (non aliased)
            RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
            //changed 2017-03-19 NearestNeighbour->LowQuality->HiqhQuality as more sharp drawing Glyph in rightest stock
            RenderOptions.SetBitmapScalingMode(ImageStockPositions, BitmapScalingMode.NearestNeighbor);
            TextOptions.SetTextRenderingMode(ImageStockPositions, TextRenderingMode.ClearType);
            TextOptions.SetTextFormattingMode(ImageStockPositions, TextFormattingMode.Display);

            //changed 2017-03-19 as more sharp drawing Glyph in rightest stock
            RenderOptions.SetBitmapScalingMode(ImagePrices, BitmapScalingMode.NearestNeighbor);
            TextOptions.SetTextRenderingMode(ImagePrices, TextRenderingMode.ClearType);
            TextOptions.SetTextFormattingMode(ImagePrices, TextFormattingMode.Display);

           
            _doUserPos = new CDOUserPos(_guiDispatcher);
            //2018-08-09
            CUtil.ThreadStart(ThreadDrawStock);

            Unloaded += ControlStock_Unloaded;
        }

      

        /// <summary>
        /// Shows/hides the bar with selected price ("focus bar")
        /// 
        /// Call when mouse Enter\Exit\Leave or changing mouse mode
        /// </summary>
        /// <param name="ShowOrNot"></param>
        public void ShowOrderFocus(bool ShowOrNot)
        {

            double mouseX = Mouse.GetPosition(ImageStockPositions).X;


            if ((ShowOrNot && (mouseX > 0.0)) && (mouseX < ImageStockPositions.ActualWidth))
            {
                CntrlFocusBar = new ControlFocusBar((double)StringHeight);



                CntrlFocusBar.Width = ImageStockPositions.ActualWidth;
                CntrlFocusBar.Height = StringHeight;
                CanvasCoontrolStock.Children.Add(CntrlFocusBar);
                IsNeedRepaintDeals = true;


                DoFocus();
            }
            else if (!ShowOrNot)
            {
                for (int i = 0; i < CanvasCoontrolStock.Children.Count; i++)
                {
                    if (CanvasCoontrolStock.Children[i] is ControlFocusBar)
                    {
                        CanvasCoontrolStock.Children.Remove(CanvasCoontrolStock.Children[i]);
                    }
                }
                CntrlFocusBar = null;
            }



        }

        
        public void ForceRepaint()
        {
            _needRepaint = true;
            _evDrawStock.Set();

        }


        /// <summary>
        /// Binds to external system components.
        /// Called from view dispatcher
        /// </summary>
        /// <param name="alarmer"></param>
        /// <param name="tickername"></param>
        /// <param name="maxRepaintTimeMS"></param>
        public void BindToSystem(IAlarmable alarmer, string tickername, long maxRepaintTimeMS)
        {

            //_tradeOperation = tradeOperation;
            _alarmer = alarmer;
            _tickerName = tickername;

            _perfAnlzr = new CPerfAnlzr(_alarmer);

            _maxRepaintTimeMS = maxRepaintTimeMS;
        }

        /// <summary>
        /// Changes the width of user order's bars
        /// </summary>
        public void ChangeOrdersWidth()
        {
            _bNeedScrollAllControls = true;
            foreach (KeyValuePair<double, ControlUserOrders> pair in _dictPriceControlUserOrders)
            {
                pair.Value.Width = base.ActualWidth;
            }
            _isNeedRepaintPrices = true;
        }


        /// <summary>
        /// Sets update flag when property for list properties was changed.
        /// For specific properties do forcing repaint.
        /// Not do this for all properties because it is could be situation.
        /// During repaint some DP could be changed (for exaple Level1Y), which calls 
        /// another propertychanged and etc. It could do errors such      
        /// </summary>     
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (_dpList.Contains(e.Property.Name))
                _needRepaint = true;

            if (e.Property.Name == "TakeProfitPrice" ||
                e.Property.Name == "StopLossPrice" ||
                e.Property.Name == "StopLossInvertPrice" ||
                e.Property.Name == "BuyStopPrice" ||
                e.Property.Name == "SellStopPrice" ||
                e.Property.Name == "BuyStopAmount" ||
                e.Property.Name == "SellStopAmount" ||
                e.Property.Name == "IsConnectedToServer" ||
                e.Property.Name == "Level1Mult" ||
                e.Property.Name == "Level2Mult" ||
                e.Property.Name == "VolumeFullBar" ||
                e.Property.Name == "BigVolume"
                )
            {
                ForceRepaint();
                //  _lgDbgPch.Log(e.Property.Name);
            }

            //2018-08-13
            if (e.Property.Name == "StringHeight"
                || e.Property.Name == "FontSize"
                || e.Property.Name == "Step"
                || e.Property.Name == "FontColor")
            {
                _isNeedRepaintPrices = true;
                ForceRepaint();
            }


            if (e.Property.Name == "Level1Mult" ||
                e.Property.Name == "Level2Mult")
                IsNeedRepaintDeals = true;




        }



        private void ControlStock_Unloaded(object sender, RoutedEventArgs e)
        {
            //Stop threads
            _isThreadDrawStock = false;
            _evDrawStock.Set();
            _isThreadSetFocus = false;
            _evFocus.Set();
        }



        private void GenDPList()
        {
            var props = Type.GetType("Terminal.DataBinding.CStockProperties").GetProperties();
            props.ToList().ForEach(property => _dpList.Add(property.Name));
        }

    

        private void Error(string msg, Exception e = null)
        {
            if (_alarmer != null)
                _alarmer.Error(msg, e);

        }

        /// <summary>
        /// In that thread all graphics draw. 
        /// </summary>
        private void ThreadDrawStock()
        {
            //Start to initialize graphics primitves here
            //Note that need to initialize ONLY IN THIS THREAD, not during declaration in class, not in c..tor,
            //because CLR enables access to objects only from thread it was created 
            _drwVisPrices = new DrawingVisual();
            _drwVisStockPos = new DrawingVisual();

            _rendererPrices = new CRendererBackground(this, _drwVisPrices, ImagePrices, _guiDispatcher);
            _rendererStockPositions = new CRendererBackground(this, _drwVisStockPos, ImageStockPositions, _guiDispatcher);

            _errorBrush = new SolidColorBrush(Colors.Black);

            _colorAsk = Colors.White;
            _colorBid = Colors.White;
            _colorBestBid = Colors.White;
            _colorBestAsk = Colors.White;
            _colorVolumeBar = Colors.White;


            _brushAsk = new SolidColorBrush(Colors.White);
            _brushBid = new SolidColorBrush(Colors.White);
            _brushBestAsk = new SolidColorBrush(Colors.White);
            _brushBestBid = new SolidColorBrush(Colors.White);
            _brushFontColor = new SolidColorBrush(Colors.White);
            _brushVolume = new SolidColorBrush(Colors.White);
            _brushGrid = new SolidColorBrush(Colors.Black);
            _brushProfit = new SolidColorBrush(Colors.Green);
            _brushProfitLight = new SolidColorBrush(Color.FromArgb(255, 0x80, 0xff, 0x80));
            _brushLoss = new SolidColorBrush(Colors.Red);
            _brushBigVolume = new SolidColorBrush(Colors.Yellow);


            _brushProfit.Opacity = 0.7;
            _brushLoss.Opacity = 0.5;
            _brushGrid.Opacity = 0.2;

            _penVolume = new Pen(_brushVolume, 1.0);
            _penProfit = new Pen(_brushProfit, 1.0);
            _penLoss = new Pen(_brushLoss, 1.0);
            _penBigBolume = new Pen(_brushBigVolume, 2.0);
            _penError = new Pen(_errorBrush, 2.0);


            _doUserPos.Init(_brushProfit, _brushLoss, _penProfit, _penLoss);


            CLogger lgDr = new CLogger("dbgDrw");


            _penGrid = new Pen(_brushGrid, 0.75);

            _typeFacePrices = new Typeface(_txtFontFamDefault, FontStyles.Normal, FontWeights.ExtraLight, new FontStretch());
            _cultureInfoPrices = new CultureInfo("en-US");
            //end of initalize graphics primitives
           
           
            while (_isThreadDrawStock)
            {
                try
                {

                    _evDrawStock.WaitOne(100000);

                    bool bContinue = false;

                    _guiDispatcher.Invoke(new Action(() =>
                    {
                        try
                        {

                            if (Asks == null || Bids == null
                                || Asks[0].Price == 0 || Bids[0].Price == 0)
                                bContinue = true;
                            else
                                UpdateLocalVars();
                        }
                        catch (Exception e)
                        {
                            Error("ThreadDraw. GUI");
                        }

                    }
                        ));

                    if (bContinue)
                        continue;

                    UpdateBrushes();



                    if (_step != 0.0)
                    {
                        UpdatePriceCoord();

                        //new calculated values
                        UpdateCurrValues();

                        if (IsNeedScrollAllControls())
                        {
                            if (DoScrollAllControls != null)
                                _guiDispatcher.Invoke(new Action(() => DoScrollAllControls(this, null)));

                            _bNeedScrollAllControls = false;
                        }



                        if (IsNeedAutoCentring())
                            DoAutoCentring();


                        if (IsNeedDrawPrices())
                        {
                            _drwCntxtPrices = _drwVisPrices.RenderOpen();
                            DrawPrices();

                            _drwCntxtPrices.Close();
                            _rendererPrices.Render();

                        }

                        _drwCntxtStockPos = _drwVisStockPos.RenderOpen();

                        DrawVolumeBars();

                        DrawLevels();

                        DrawUserLevels();

                        DrawVerticalLines();

                        DrawCondOrderBars();

                        DrawUserPos();

                        UpdateOldValues();

                        _drwCntxtStockPos.Close();
                        _rendererStockPositions.Render();

                        _bSoreRepaint = _needRepaint;
                        _needRepaint = false;

                    }

                }
                catch (Exception e)
                {
                    Error("ControlStock.OnPaint", e);
                }

            }
            
        }

       /// <summary>
       /// 
       /// </summary>
        private void ThreadSetFocus()
        {
            while (_isThreadSetFocus)
            {
                _evFocus.WaitOne();


                double y = 0.0;
                double price = 0;

                double yMouse = 0;


                _guiDispatcher.Invoke(new Action(() =>
                {
                    yMouse = Mouse.GetPosition(ImageStockPositions).Y;

                }));

                lock (DictPriceY)
                {
                    foreach (KeyValuePair<double, double> pair in _dictPriceY)
                    {
                        if ((yMouse >= pair.Value) && (yMouse < (pair.Value + _stringHeight)))
                        {
                            y = pair.Value;
                            price = pair.Key;
                          
                        }
                    }
                }
                
                _guiDispatcher.Invoke(new Action(() =>
                {
                    SelectedY = y;
                    SelectedPrice = price;
                }
                           ));
                 

                if ( _guiDispatcher!=null)
                {

                    _guiDispatcher.Invoke(new Action(() =>
                    {
                        if (CntrlFocusBar != null)
                        {
                            Canvas.SetLeft(CntrlFocusBar, 0.0);
                            Canvas.SetTop(CntrlFocusBar, y);
                        }
                    }
                            ));

               

                }


            }

        }
           

     

        /// <summary>
        /// Moves bid and ask from border of stock more to center.
        /// 
        /// Method IsNeedAutoCentring determines if bid or ask is at 
        /// the edge of window. If so starts this method,
        ///  
        /// This method move bid and ask Y to the specific window 
        /// which is less than in method IsNeedAutoCentring. 
        /// Moving is smothing (like regulation)
        /// </summary>
        private void DoAutoCentring()
        {
            //avg price beetwen bid and ask
            double avgPriceY = (_lowestAskY + _highestBidY) / 2;

            double parWinBorderOffset = 0.3;

            double winBorderTop = parWinBorderOffset * base.ActualHeight;
            double winBorderBot = (1 - parWinBorderOffset) * base.ActualHeight;

            double centerY = 0.5 * base.ActualHeight;

            const int parSleep = 45;
            _bIsNeedAutoCentring = true;

          
                //moving - regulation reduse error beetwen avg price
                //on each iteration
                if (avgPriceY < winBorderTop)
                    _lowestAskY += (int)((centerY - _lowestAskY) * _focusSpeed);

                else if (avgPriceY > winBorderBot)
                    _lowestAskY -= (int)((_highestBidY - centerY) * _focusSpeed);

                //if bid or ask Y is in window stop centring   
                if (((winBorderTop - _lowestAskY) < 1.0) && (_lowestAskY < winBorderBot))
                    _bIsNeedAutoCentring = false;

                else if (((_highestBidY - winBorderBot) < 1.0) && (_highestBidY > winBorderTop))
                    _bIsNeedAutoCentring = false;

                _bNeedScrollAllControls = true;
                _needRepaint = true;
                _bIsNeedDrawPriceAfterCentring = true;
                _isNeedRepaintPrices = true;
                ForceRepaint();
         

        }





      

        private void AddDelUserLevel()
        {


            if (CntrlFocusBar == null)
                return;




            double posY = Canvas.GetTop(CntrlFocusBar);
            double price = -1000;


            bool bFound = false;
            lock (DictPriceY)
            {
                foreach (KeyValuePair<double, double> kvp in DictPriceY)
                    if (kvp.Value == posY)
                    {
                        bFound = true;
                        price = kvp.Key;
                        break;

                    }
            }
            if (!bFound)
                return; //no coord nothing to do


            ExecuteCommand(EventsViewModel.CmdAddDelUserLevel, price);



            if (_lstUserLines.Contains(price))
                _lstUserLines.Remove(price);
            else
                _lstUserLines.Add(price);

            ForceRepaint();


        }



        private void ExecuteCommand(RoutedUICommand cmd, object data)
        {
            cmd.Execute(data, this);

        }



        private string DbgGetNullPriceReport(double posY)
        {
            string st = String.Format("posY={0}+++", posY);
            lock (DictPriceY)
            {
                foreach (KeyValuePair<double, double> pair2 in _dictPriceY)
                {
                    string stEl = String.Format("{0}=>{1} | ", pair2.Key.ToString("N05"), pair2.Value.ToString("N02"));
                    st += stEl;
                }
            }
            return st;
        }

        private double GetSelectedPrice(ref bool isValid)
        {
            isValid = false;
            double posY = Canvas.GetTop(CntrlFocusBar);
            double price = 0.0;

            lock (DictPriceY)
            {
                foreach (KeyValuePair<double, double> pair2 in _dictPriceY)
                    if (pair2.Value == posY)
                    {
                        price = pair2.Key;
                        isValid = true;
                    }
            }

            string message = DbgGetNullPriceReport(posY);
            Log(message);

            if (!isValid)
            {
                //tempo for error catch
                isValid = true;

                Error("ERROR NULL PRICE " + message);

                //2017-12-29 special case. 
                //If price not found try do with less modification
                double tol = 0.5 * StringHeight;

                lock (DictPriceY)
                {
                    foreach (KeyValuePair<double, double> pair2 in _dictPriceY)
                        if (Math.Abs(pair2.Value - posY) < tol)
                        {
                            price = pair2.Key;
                            isValid = true;
                        }
                }
            }

            return price;
        }






        private bool IsOrdersArrFull()
        {
            int i = 0;
            for (i = 0; i < Orders.Length; i++)
                if (Orders[i].Amount == 0)
                    break;


            if (i == Orders.Length - 1)
                return true;


            return false;
        }







        private void AddOrder(MouseButton mouseChangedBtn)
        {

            if (CntrlFocusBar == null)
                return;

            if (IsOrdersArrFull())
                return;


            //add order action
            EnmOrderDir dir = new EnmOrderDir();

            if (mouseChangedBtn == MouseButton.Left)
                dir = EnmOrderDir.Buy;

            else if (mouseChangedBtn == MouseButton.Right)
                dir = EnmOrderDir.Sell;




            int currNum = Convert.ToInt16(CurrAmountNum);
            bool bValidPrice = false;
            decimal price = (decimal)GetSelectedPrice(ref bValidPrice);
            if (!bValidPrice)
                return;

            //int amount =  Convert.ToInt16(ListWorkAmount[currNum].TextAmountValue);
            decimal amount = Convert.ToDecimal(ListWorkAmount[currNum].TextAmountValue);


            //_tradeOperation.AddOrder(_tickerName, amount, dir, (decimal)price);

            ExecuteCommand(EventsViewModel.CmdAddOrder, new CDataAddOrder
            {
                Amount = amount,
                Instrument = _tickerName,
                Dir = dir,
                Price = price
            }
                          );

            Log("Add order _tickerName=" + _tickerName + " Amount=" + amount + " dir=" + dir);

        }




        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {


           

            try
            {


                //focus was set by mouse click
                if (FocusedByClick && (Mouse.GetPosition(ImageStockPositions).X > 0.0))
                {


                    //if left or right mouse button was click
                    if ((e.ChangedButton == MouseButton.Left) || (e.ChangedButton == MouseButton.Right))
                    {
                        //if user clicked not in area with order than exit                     
                        if (Mouse.GetPosition(ImageStockPositions).X > ActualWidth)
                        {   //for order remove there is a special text block so we exit method here
                            return;
                        }


                        //if (_isModeDrawLevel)
                        if (SelectionMode.IsModeDrawLevel)
                            AddDelUserLevel();

                        else if (SelectionMode.IsModeStopLossTakeProfit)
                            SetStopLossTakeProfit();

                        else if (SelectionMode.IsModeStopLossInvert)
                            SetStopLossInvert();

                        else if (SelectionMode.IsModeStopOrder)
                            SetStopOrder();

                        else if (SelectionMode.IsModeRestOrder)
                            SetRestOrder(e.ChangedButton);

                        else
                            AddOrder(e.ChangedButton);



                    }
                }
            }
            catch (Exception exc)
            {
                Error("ControlStock.OnMouseDown", exc);

            }


        }


        private void SetStopLossTakeProfit()
        {
            bool isValidPrice = false;
            double price = GetSelectedPrice(ref isValidPrice);
            if (isValidPrice)
                ExecuteCommand(EventsViewModel.CmdSetStopLossTakeProfit, price);

        }

        private void SetStopLossInvert()
        {
            bool isValidPrice = false;
            double price = GetSelectedPrice(ref isValidPrice);
            if (isValidPrice)
                ExecuteCommand(EventsViewModel.CmdSetStopLossInvert, price);

        }

        private void SetStopOrder()
        {
            int currNum = Convert.ToInt16(CurrAmountNum);

            bool isValidPrice = false;
            decimal price = (decimal)GetSelectedPrice(ref isValidPrice);
            if (!isValidPrice)
                return;

            //changed 2018-03-22
            decimal amount = Convert.ToDecimal(ListWorkAmount[currNum].TextAmountValue.Replace(@".", ","));

            CDataStopOrder dso = new CDataStopOrder
            {
                Amount = amount,
                Price = price
            };


            ExecuteCommand(EventsViewModel.CmdSetStopOrder, dso);

        }



        private void SetRestOrder(MouseButton mouseChangedBtn)
        {

            EnmOrderDir dir = new EnmOrderDir();

            if (mouseChangedBtn == MouseButton.Left)
                dir = EnmOrderDir.Buy;

            else if (mouseChangedBtn == MouseButton.Right)
                dir = EnmOrderDir.Sell;

            bool isValidPrice = false;
            decimal price = (decimal)GetSelectedPrice(ref isValidPrice);
            if (!isValidPrice)
                return;

            CDataRestOrder data = new CDataRestOrder
            {
                Instrument = TickerName,
                Price = price,
                Dir = dir
            };




            ExecuteCommand(EventsViewModel.CmdSendRestOrder, data);

        }



        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            _isInStockArea = true;
            //when mouse on stock not centring
            //(may be user wants to add order)
            _bIsNeedAutoCentring = false;

            if (MouseMode && (CntrlFocusBar == null))
            {
                ShowOrderFocus(true);
            }

        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            _isInStockArea = false;
            ShowOrderFocus(false);
            SelectionMode.ResetAllModes();
            IsNeedRepaintDeals = true;
            // IsNeedRepaintDeals = true;

        }


      

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (MouseMode)
                {
                    if (CntrlFocusBar == null)
                    {
                        ShowOrderFocus(true);
                    }
                    else
                    {
                        DoFocus();
                    }

                    if (SelectionMode.IsModeRestOrder || SelectionMode.IsModeStopLossInvert ||
                        SelectionMode.IsModeStopLossTakeProfit || SelectionMode.IsModeStopOrder ||
                        SelectionMode.IsModeDrawLevel)
                    {
                        if ((DateTime.Now - _dtLastRepaintOnMsMv).TotalMilliseconds> _parRepaintMsMvMs )
                        {
                            IsNeedRepaintDeals = true;
                            _dtLastRepaintOnMsMv = DateTime.Now;
                        }

                    }
                }
            }
            catch (Exception exc)
            {
                Error("OnMouseMove", exc);
            }
        }

     
        /// <summary>
        ///  Move lowest Ask Y coord so having a scroll effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            _parScrollMouseStepPxls = _stringHeight * 10;

            try
            {

                if (e.Delta > 0)
                    _lowestAskY += _parScrollMouseStepPxls;
                else if (e.Delta < 0)
                    _lowestAskY -= _parScrollMouseStepPxls;

                _bNeedScrollAllControls = true;
                _isNeedRepaintPrices = true;
                //ScrollDelta = e.Delta;
                Repaint = true;

               // _lstDt.Add(DateTime.Now);
            }
            catch (Exception exc)
            {
                Error("OnMouseWheel(", exc);

            }
        }
      






        private void DrawUserLevels()
        {

            //List<CLevelEl> _lstEls = UserLevels.GetCopy();

            List<CLevelEl> _lstEls = null;
            _guiDispatcher.Invoke (new Action (()=>
                                    _lstEls = UserLevels.GetCopy()
                                  ));

            if (_lstEls == null)
                return;

            lock (DictPriceY)
            {
                foreach (var kvp in _dictPriceY)
                {
                    double price = kvp.Key;
                    double y = kvp.Value;
                    var res = _lstEls.Find(el => el.Price == price);
                    if (res != null)
                        //DrawUserLevelRectangle(price, y + StringHeight / 2);
                        DrawUserLevelHighlight(y);

                }
            }
          


        }




        private void UpdateLocalVars()
        {

            if (((Step != _step) || (Decimals != _priceDecimals)) || (TickerName != _shortName))
            {
                _step = Step;
                _priceDecimals = Decimals;
                _shortName = TickerName;
                _lowestAskY = 0;
                _prevLowestAskPrice = 0.0;              
            }

            //2018-08-09
            _stringHeight = StringHeight;
            _fontSize = FontSize;
            _isConnectedToServer = IsConnectedToServer;
            _volumeFullBar = VolumeFullBar;
            _decimalVolume = DecimalVolume;
            _deicmals = Decimals;
            _bigVolume = BigVolume;
            _level1Mult = Level1Mult;
            _level2Mult = Level2Mult;

            _stoplossPrice = StopLossPrice;
            _takeProfitPrice = TakeProfitPrice;
            _stopLossInvertPrice = StopLossInvertPrice;
            _buyStopPrice = BuyStopPrice;
            _sellStopPrice = SellStopPrice;

            _isInControlDeals = IsInControlDeals;

          
            CTerminalUtil.UpdateLocalColors(FontColor, ref _colorFont);


            CTerminalUtil.UpdateLocalColors(AskColor, ref _colorAsk);
            CTerminalUtil.UpdateLocalColors(BidColor, ref _colorBid);
            CTerminalUtil.UpdateLocalColors(BestBidColor, ref _colorBestBid);
            CTerminalUtil.UpdateLocalColors(BestAskColor, ref _colorBestAsk);
            CTerminalUtil.UpdateLocalColors(VolumeBarColor, ref _colorVolumeBar);
       

            _asksLocal = (CStockPosition[])Asks.Clone();
            _bidsLocal = (CStockPosition[])Bids.Clone();

            _isPosActive = _doUserPos.IsActive;
            
           
            if (_isPosActive &&  _asksLocal.Length>0 && _bidsLocal.Length>0 && _step!=0)
            {
                _avPos = _doUserPos.AvPos;

                _isBuy = _doUserPos.IsBuy;
                _isSell = _doUserPos.IsSell;


                if (_isBuy)
                {
                    _currentPrice = _bidsLocal[0].Price;
                    _profitInPrice = _currentPrice - Convert.ToDouble(_avPos);                  
                }
                else
                {
                    _currentPrice = _asksLocal[0].Price;
                    _profitInPrice = Convert.ToDouble(_avPos) - _currentPrice;
                }

                _profitInStep = _profitInPrice / _step;

            }
        }



    

    
       


        private void UpdateBrushes()
        {
            CTerminalUtil.UpdateBrush(_colorFont, ref _brushFontColor);
            CTerminalUtil.UpdateBrush(_colorAsk, ref _brushAsk);
            CTerminalUtil.UpdateBrush(_colorBid, ref _brushBid);
            CTerminalUtil.UpdateBrush(_colorBestBid, ref _brushBestBid);
            CTerminalUtil.UpdateBrush(_colorBestAsk, ref _brushBestAsk);
            CTerminalUtil.UpdateBrushAndPen(_colorVolumeBar, ref _brushVolume, ref _penVolume, 2);

        }

     




        private bool IsNeedScrollAllControls()
        {
            lock (DictPriceY)
                if ((_dictPriceY.Count != 0) && _bNeedScrollAllControls)
                    return true;


            return false;
        }


        private void GetL1LineBrushPen(ref Brush brush, ref Pen pen)
        {
            if (LineL1Color != null)            
                brush = LineL1Color;
            
            double thickness = 1.0;
            pen = new Pen(brush, thickness);

        }

        private void GetL2LineBrushPen(ref Brush brush, ref Pen pen)
        {
            if (LineL2Color != null)
                brush = LineL2Color;

            double thickness = 1.0;
            pen = new Pen(brush, thickness);

        }



        private void DrawLevelRectangle(Brush brush, Pen pen, double x, double y, double width, double height)
        {           
            _levelDrawer.DrawLevelRectangle(_drwCntxtStockPos, brush, pen, x, y, width, height);
        }

        private void DrawLevelLine(Pen pen, double x1, double y1, double x2, double y2)
        {			
            _levelDrawer.DrawLevelLine(_drwCntxtStockPos, pen, x1, y1, x2, y2);                       
        }


        private void GetLevelLineParams(double price, ref double xOffset, ref double lineWidth)
        {

            int amount = GetAmountOfPrice(price);

            xOffset = 0;

            if (amount != 0)
                xOffset = 30;

            lineWidth = ActualWidth - xOffset - _scaledTextPriceWidth;
            lineWidth = Math.Max(0, lineWidth);

            Brush brush = _errorBrush;
            Pen pen = _penError;

        }


        private void DrawUserLevelRectangle(double price, double y)
        {
            double xOffset = 0;
            double lineWidth = 0;
            
            GetLevelLineParams(price, ref xOffset, ref lineWidth);
            double lineHeight = 2.0;

            Brush brush = _errorBrush;
            Pen pen = _penError;

            DrawLevelRectangle(brush, pen, xOffset, y, lineWidth, lineHeight);
        }

		private void DrawUserLevelHighlight(double y)
		{
			Brush brush = new SolidColorBrush(Colors.Yellow);
			brush.Opacity = 0.5;
			Pen pen = new Pen(new SolidColorBrush(Colors.Black),1.0);

			_levelDrawer.DrawLevelRectangle(_drwCntxtStockPos, brush, pen, 0, y, ActualWidth, _stringHeight);

		}

        private void DrawLevel1Rectanlge(double price, double y)
        {

            double xOffset = 0;
            double lineWidth = 0;            
            GetLevelLineParams(price, ref xOffset, ref lineWidth);

          
            double lineHeight = 2.0;
            
            Brush brush = _errorBrush;
            Pen pen = _penError;
            GetL1LineBrushPen(ref brush, ref pen);
            DrawLevelRectangle(brush, pen, xOffset, y, lineWidth, lineHeight);

        }

       


        private void DrawLevel2Line(double price, double y)
        {

            double xOffset = 0;
            double lineWidth = 0;
            GetLevelLineParams(price, ref xOffset, ref lineWidth);


            Brush brush = _errorBrush;
            Pen pen = _penError;

            GetL2LineBrushPen(ref brush, ref pen);

            DrawLevelLine(pen, xOffset, y, xOffset+lineWidth, y);

        }


        private void DrawVerticalLines()
        {
            //draw vertical lines
            _drwCntxtStockPos.DrawLine(_penGrid, new Point(ActualWidth - _scaledTextPriceWidth, 0.0), new Point(ActualWidth - _scaledTextPriceWidth, ActualHeight));          
        }

        private bool IsMultipleOf(double price,double mult)
        {
            double level = (mult * _step) * Math.Pow(10.0, (double)_priceDecimals);
            if ((Math.Round((double)(price * Math.Pow(10.0, (double)_priceDecimals)), 2) % level) == 0.0)
                return true;

            return false;
        }


      


        private void DrawLevels()
        {
             
            sw4.Stop();
            //draw levels here
            //2018-06-16 deadlock
            //lock (DictPriceY)
            {
                FiftyLevels.Clear();
                TenLevels.Clear();

                _level1y.Clear();
                _level2y.Clear();


                lock (DictPriceY)
                {
                    foreach (KeyValuePair<double, double> kvp in _dictPriceY)
                    {
                        double price = kvp.Key;
                        double y = kvp.Value;

                        //draw  levels 10
                        double yOffset = 0;


                        // double level1 = Level1Mult; //50;
                        // double level2 = Level2Mult;//10.0;
                        if (IsMultipleOf(price, _level1Mult))
                        {
                            yOffset = 5;
                            //2017-03-27 removed by client request
                            //DrawLevel1Rectanlge(price, y + yOffset);
                            FiftyLevels.Add(y + yOffset);
                            _level1y.Add(y + yOffset);
                        }
                        else if (IsMultipleOf(price, _level2Mult))
                        {
                            yOffset = 7;
                            //2017-03-27 removed by client request
                            //DrawLevel2Line(price, y + yOffset);
                            TenLevels.Add(y + yOffset);
                            _level2y.Add(y + yOffset);
                        }
                        //draw levels 50


                    }
                }
             
                _guiDispatcher.Invoke(new Action(() =>
              {
                  lock (Level1Y)
                  {

                      Level1Y = _level1y;

                  }

                  lock (Level2Y)
                  {
                      Level2Y = _level2y;

                  }
              }
                ));
              
            }

        }

        private void GetBrushPenOneDir(int index,Brush bestBrush, Brush backBrush,  ref Brush brush)
        {


            if (index == 0)
            {
                if (bestBrush != null)
                    brush = bestBrush;
                    //UpdateLocalBrush(bestBrush, ref brush);
            }
            else
            {
                if (backBrush != null)
                    brush = backBrush;
                    //UpdateLocalBrush(backBrush, ref brush);
            }

           

        }


       



        private void GetTotalBarBrushPen(int index, EnmBidAsk enmBidAsk, ref Brush brush, ref Pen pen)
        {

            if (enmBidAsk == EnmBidAsk.Ask)
                GetBrushPenOneDir(index,  _brushBestAsk, _brushAsk, ref brush);
            else if (enmBidAsk == EnmBidAsk.Bid)
                GetBrushPenOneDir(index, _brushBestBid, _brushBid, ref brush);



            double thickness = 1;
            pen = new Pen(brush, thickness);

        }


        private void GetVolumeBarBrush(double volume, ref Brush brush, ref Pen pen)
        {            
                      
            if (volume > Convert.ToDouble(CUtilConv.GetIntVolume(_bigVolume, _decimalVolume)))
            {
                //TODO from config, dependency property
                // brush = Brushes.Yellow;                
                // pen = new Pen(brush, thickness);
                brush = _brushBigVolume;
                pen = _penBigBolume;
            }

        }



        private void DrawOneConditionOrderBar (double price, Brush brush)
        {
            double coordPrice;
            lock (DictPriceY)
            {
                if (_dictPriceY.TryGetValue(price, out coordPrice))
                {

                    Pen pen = new Pen(brush, 1.0);
                    _drwCntxtStockPos.DrawRectangle(brush, pen, new Rect(0.0, coordPrice, ActualWidth, _stringHeight));

                }
            }

        }


        private void DrawCondOrderBars()
        {
            if (!_isConnectedToServer)
                return;

            //TODO DP user selected colors
            DrawOneConditionOrderBar(_takeProfitPrice,  new SolidColorBrush(Color.FromArgb(0xAA, 0x00, 0x00, 0xE0)));
            DrawOneConditionOrderBar(_stoplossPrice, new SolidColorBrush(Color.FromArgb(0x90, 0xF0, 0x00, 0x00)));
            DrawOneConditionOrderBar(_stopLossInvertPrice, new SolidColorBrush(Color.FromArgb(0x50, 0xF0, 0x00, 0x00)));     
            DrawOneConditionOrderBar(_buyStopPrice,  new SolidColorBrush(Color.FromArgb(0x50, 0x00, 0x50, 0x00)));
            DrawOneConditionOrderBar(_sellStopPrice, new SolidColorBrush(Color.FromArgb(0x50, 0x50, 0x00, 0x00)));
        }

        private bool IsStockValueChanged(int index, CStockPosition[] oldStockPos, CStockPosition currStockPos)
        {

            if (oldStockPos == null)
                return true;

            if (index > oldStockPos.Length-1 )
                return true;

            
           
            

                return true;
        }

        private void DrawOneVolumeBar(int index, CStockPosition[] stockPositions, EnmBidAsk enmBidAsk, CStockPosition[] oldStockPos, bool fForceRedraw)
        {

          if ((index >= stockPositions.Length) || (stockPositions[index] == null))
              return;

          double fullBarWidth = ActualWidth - _scaledTextPriceWidth;
          
          double volumeFullBar = Convert.ToDouble(CUtilConv.GetIntVolume(_volumeFullBar, _decimalVolume));
          if (volumeFullBar <= 0) //zero divide protect
              return;


          double barWidth = ((stockPositions[index].Amount) / volumeFullBar) * fullBarWidth;
          barWidth = Math.Min(barWidth, fullBarWidth);

          if (barWidth <= 0.0)
              return;


          CStockPosition stockPos = stockPositions[index];
          

          double coordPrice;

            //Draw here
            //if price is on screen
            lock (DictPriceY)
            {
                if (_dictPriceY.TryGetValue(stockPos.Price, out coordPrice))
                {
                    Brush brushTotalBar = _errorBrush;
                    Pen penTotalBar = _penError;
                    GetTotalBarBrushPen(index, enmBidAsk, ref brushTotalBar, ref penTotalBar);

                    //Draw total bar
                    _drwCntxtStockPos.DrawRectangle(brushTotalBar, penTotalBar, new Rect(0.0, coordPrice, (double)(((int)ActualWidth)), (double)_stringHeight));

                    Brush brushVolume = _brushVolume;
                    Pen penVolume =  _penVolume;
                    GetVolumeBarBrush(stockPos.Amount, ref brushVolume, ref penVolume);
                    //Draw volume bar
                    _drwCntxtStockPos.DrawRectangle(brushVolume, penVolume, new Rect(0.0, coordPrice, barWidth, (double)_stringHeight));
                    ////KAA changed 2016-06-01
                    //print textVolume
                    DrawAmountText(stockPos.Amount, coordPrice);
                   
                }
            }

          _volumeWidth = Math.Max(_volumeWidth, _textPrice.Width);

             

        }

        private void DrawAmountText(int amount, double coordPrice)
        {

                      
            //added 2018-02-21
            string stAmount = CTerminalUtil.GetAmount(amount, _decimalVolume);
             var volumeText = new FormattedText(stAmount, _cultureInfo, FlowDirection.LeftToRight, _typeFaceDefault, _fontSize, _brushFontColor);
            _drwCntxtStockPos.DrawText(volumeText, new Point(5.0, coordPrice));
            
         
        }


       

        private void CopyOldStock(CStockPosition[] curr, ref CStockPosition[] old)
        {
            if (curr != null && curr.Length > 0)
            {
                old = new CStockPosition[curr.Length];
                for (int i = 0; i < curr.Length; i++)
                {
                    old[i] = new CStockPosition();
                    old[i].Amount = curr[i].Amount;
                    old[i].Price = curr[i].Price;
                    old[i].IsEmpty = curr[i].IsEmpty;
                }

            }
        }





        private void DrawVolumeBars()
        {
			//commented 2017-10-21
           // double currMaxPrice = _dictPriceY.Keys.ElementAt<double>(0);
           // double currHighestY = _dictPriceY.Values.ElementAt<double>(0);

            int length = Math.Max(_asksLocal.Length, _bidsLocal.Length);


            bool bForceRedraw = IsPricesChanged();
              


           // sw5.Stop();
        
            for (int k = 0; k < length; k++)
            {
               DrawOneVolumeBar(k, _asksLocal, EnmBidAsk.Ask, _oldAsks, bForceRedraw);
               DrawOneVolumeBar(k, _bidsLocal, EnmBidAsk.Bid, _oldBids, bForceRedraw);
              
            }


            CopyOldStock(_asksLocal, ref _oldAsks);
            CopyOldStock(_bidsLocal, ref _oldBids);




            if (GridControlMarket != null)
            {//2018-08-10 why do we need this ?

                _guiDispatcher.Invoke(new Action(() =>
               {
                   double widthUse = (this._volumeWidth + this._priceWidth) + 10.0;
                   if (GridControlMarket.ColumnDefinitions[4].MinWidth != widthUse)
                        GridControlMarket.ColumnDefinitions[4].MinWidth = widthUse;
               }));
            }


        }

        private int GetAmountOfPrice(double price)
        {

            int amount = GetAmountOfPriceOneDir(Asks, price);
            if (amount != 0)
                return amount;

            amount =  GetAmountOfPriceOneDir(Bids, price);
            
            return amount;
         
   
        }

        private int GetAmountOfPriceOneDir(CStockPosition[] stockPositions, double price)
        {
            foreach (CStockPosition stockPos in stockPositions)
            
                if (stockPos.Price == price)
                     return stockPos.Amount;
                
          
            return 0;
        }

        /// <summary>
        /// Calculate current Y position of current  lowest ask. If price of lowest ask was change
        /// do recalculate new coordinate of lowest ask.
        ///  Using as initial point  Y of lowest ask - fill <price,Y> dictionary        
        /// </summary>
        private void UpdatePriceCoord()
        {
            //initialy use half of height as Y of lowest ask
            if (_lowestAskY == 0)
            {
                _lowestAskY = (int)(base.ActualHeight * 0.5);
            }
            //than if lowest ask price changed than change Y of lowest ask          
            const double pcnt = 10;
            double  tolAbs = _step*pcnt/100;
            if ((_prevLowestAskPrice != 0.0))
                if (Math.Abs(_prevLowestAskPrice - _asksLocal[0].Price) > tolAbs)
                {
                    //calculate how much in pixels point was moved
                    int dltFocusAsk = (int)(Math.Round((double)(((double)(_asksLocal[0].Price - _prevLowestAskPrice)) / _step), 0) * _stringHeight);
                    //and move coordinate
                    _lowestAskY -= dltFocusAsk;
                    if (_lowestAskY < 0)
                        System.Threading.Thread.Sleep(0);

                }
          
             //recalculate bidY as well
            _highestBidY = _lowestAskY + (((int)Math.Round((double)((_asksLocal[0].Price - _bidsLocal[0].Price) / _step), 0)) * _stringHeight);
            _prevLowestAskPrice = _asksLocal[0].Price;
            //KAA changed 2016-06-01                                                                          
            _textPrice = new FormattedText(_asksLocal[0].Price.ToString("N0" + _priceDecimals.ToString()), _cultureInfo, FlowDirection.LeftToRight, _typeFaceDefault,  _fontSize, Brushes.Black);
            _textPriceWidth = _textPrice.Width;
            _scaledTextPriceWidth = _parScaleTextPriceWidth * _textPriceWidth;
            _priceWidth = _textPriceWidth;
           

            bool bNeedUpdate = false;

            lock (DictPriceY)
            {
                //KAA 2017_03_01
                if (_dictPriceY.Count > 0)
                {
                    _prevFirstY = _dictPriceY.First().Value;
                    _prevLastY = _dictPriceY.Last().Value;
                    _prevFirstPrice = _dictPriceY.First().Key;
                    _prevLastPrice = _dictPriceY.First().Value;
                }

                _dictPriceY.Clear();
                //initial point - middle of the screen
                // and lowest ask
                double iCoordPrice = _lowestAskY;
                double price = _asksLocal[0].Price;

                //first find bottom of price and coord
                //
                while (iCoordPrice < base.ActualHeight)
                {
                    iCoordPrice += _stringHeight;
                    price -= _step;
                }
                
                //than fill dictionary <Price,Y> from bottom to  
                //top
                while (iCoordPrice > 0.0)
                {
                    price += _step;
                    iCoordPrice -= _stringHeight;
                    price = Math.Round(price, _priceDecimals);
                    if (_dictPriceY.Count < 1000)
                    {
                        _dictPriceY.Add(price, iCoordPrice);
                    }
                    else
                    {
                        _lowestAskY = 0;
                        _prevLowestAskPrice = 0.0;
                        return;
                    }
                }
          
                //At this point we have filled 
                //dictionary <Price,Y>

                if (_dictPriceY.Count > 0)
                    if (_prevFirstY != _dictPriceY.First().Value ||
                        _prevLastY != _dictPriceY.Last().Value ||
                        _prevFirstPrice != _dictPriceY.First().Key ||
                        _prevLastPrice != _dictPriceY.First().Value)
                            bNeedUpdate = true;


            }
           // sw6.Stop();
         
           
            
           if (bNeedUpdate)
                PriceCoordChanged(this, null);
           
            
                                         
        }


        /// <summary>     
        /// </summary>
        /// <returns>
        /// If autocentring mode switched on and 
        /// mouse not on stock 
        /// in case when ask/bid Y moved
        /// enough return true
        /// </returns>
        private  bool IsNeedAutoCentring()
        {
          //2018-08-10
            //already in centring
          //  if (_bIsNeedAutoCentring)
            //    return false;

            //TODO from settings

            double parPcntFromBorderAutoScroll = 0.1;
            double minAllowedAskY = parPcntFromBorderAutoScroll * base.ActualHeight;
            double maxAllowedBidY = (1- parPcntFromBorderAutoScroll) * base.ActualHeight;

         
            


            if (!_isInStockArea && !_isInControlDeals ) 
                if (_autoScroll)
                    if ((_lowestAskY < minAllowedAskY) || (_highestBidY > maxAllowedBidY ))
                        return true;

            return false;
        }

     



        //2018-08-12
        private void DrawUserPos()
        {
            if (!_isPosActive)
                return;

            int widthUserPos = (int)(ActualWidth - _scaledTextPriceWidth);

            //  _doUserPos.Draw(_drwCntxtStockPos, _stringHeight, widthUserPos, _highestBidY, _lowestAskY, _scaledTextPriceWidth,ActualWidth, /*Bids[0].Price, m_dStep,*/_dictPriceY,  _deicmals);

            double currPrice;

            if (_isBuy)
                currPrice = _bidsLocal[0].Price;
            else
                currPrice = _asksLocal[0].Price;






            double yPos = 0;
            double yBidAsk = 0;

            lock (DictPriceY)
            {
                //position point
                foreach (var kvp in _dictPriceY)
                {
                    if ( Math.Abs(kvp.Key - Convert.ToDouble(_avPos))<0.5 *_step )
                    {
                        yPos = kvp.Value;
                    }
                }

            

            
             
                //bidAskPoint
                foreach (var kvp in _dictPriceY)
                {
                   
                    if (Math.Abs(kvp.Key - Convert.ToDouble(currPrice)) < 0.5 * _step)
                    {
                        yBidAsk = kvp.Value;
                    }
                }


                //pos and bid/ask out of screen window
                if (yBidAsk ==0 && yPos ==0)
                {
                    return;//get out
                }




                if (yBidAsk==0)//not found
                {
                    if (_isBuy)
                    {
                        if (currPrice > Convert.ToDouble(_avPos))
                            yBidAsk= _dictPriceY.Last().Value; //have profit get upper
                        else
                            yBidAsk = _dictPriceY.First().Value; //have loss get bottom
                    }
                    else
                    {
                        if (currPrice < Convert.ToDouble(_avPos))
                            yBidAsk = _dictPriceY.First().Value; //have profit get bottom
                        else
                            yBidAsk = _dictPriceY.Last().Value; //have loss get upper
                    }

                }
          

                if (yPos == 0)//not found
                {
                    if (_isBuy)
                    {
                        if (currPrice > Convert.ToDouble(_avPos))
                            yPos = _dictPriceY.First().Value; //have profit get bottom
                        else
                         yPos = _dictPriceY.Last().Value; //have loss get upper
                }
                    else
                    {
                        if (currPrice < Convert.ToDouble(_avPos))
                         yPos = _dictPriceY.Last().Value; //have profit get upper
                        else
                            yPos = _dictPriceY.First().Value; //have loss get bottom
                }
            }

            }

            Brush brush = Brushes.Black;
            Pen pen = new Pen(Brushes.Black, 1.0);

            if (_profitInPrice>0)
            {
                brush = _brushProfit;
                pen = _penProfit;
            }
            else
            {
                brush = _brushLoss;
                pen = _penProfit;
            }

          
            yPos = Math.Max(yPos, 0);
            yBidAsk = Math.Max(yBidAsk, 0);

            double yRect = Math.Min(yPos, yBidAsk);
            double h = Math.Abs(yPos - yBidAsk);


            _drwCntxtStockPos.DrawRectangle(brush, pen, new Rect(widthUserPos, yRect, _scaledTextPriceWidth, h));

        }
      

        private void CheckOnPaint(long ms)
        {


            if (_tickerName != null)
            {
                string msg = "ControlStock.CheckOnPaint" + _tickerName + " ";
                if (_maxRepaintTimeMS > 0)
                    _perfAnlzr.CheckLim(ms, _maxRepaintTimeMS, msg);

            }
          

        }



        private bool IsPricesChanged()
        {
        
            //something changed 
            if (((_currMinPrice != _oldMinPrice) || (_currHighestY != _oldHighestY)) ||
                ((_oldDrawnPriceX != _currPriceX) || _isNeedRepaintPrices || BIsNeedDrawPriceAfterCentring
                || _oldLowestAskY != _lowestAskY))
                return true; 

            return false;
        }


      


        private void UpdateCurrValues()
        {
            //no coordinates get out
            lock (DictPriceY)
            {
                if (_dictPriceY.Count <= 0)
                    return;

                double parScaleOffs = 1.1;
                _currMinPrice = _dictPriceY.Keys.ElementAt<double>(0);
                _currHighestY = _dictPriceY.Values.ElementAt<double>(0);
                _currPriceX = ActualWidth - (_textPriceWidth * parScaleOffs);
            }

        }

        private void UpdateOldValues()
        {

            _oldMinPrice = _currMinPrice;
            _oldHighestY = _currHighestY;
            _oldDrawnPriceX = _currPriceX;
            _oldLowestAskY = _lowestAskY;
        }


     


        private bool IsNeedDrawPrices()
        {

            if (!_isEnoughTimeSinceStart)
            {
                if (_dtFirst == DateTime.MinValue)
                    _dtFirst = DateTime.Now;

                if ((DateTime.Now - _dtFirst).TotalSeconds > _parSecSinceStart)
                    _isEnoughTimeSinceStart = true; 

               
            }

            //no coordinates get out
            if (_dictPriceY.Count <= 0)
                return false;


            if (!_isConnectedToServer)
                return false;


            //if all same no need to draw
            if (!IsPricesChanged() && _isEnoughTimeSinceStart)
                return false;


            return true;
        }


        private void DrawPrices()
        {
          		                     
            //DO the drawing
            lock (DictPriceY)
            {
              
                
                foreach (KeyValuePair<double, double> pair in _dictPriceY)
                {
                    Brush brush = Brushes.Black;
                    if (_brushFontColor != null)
                        brush = _brushFontColor;

                  
                    FormattedText textPrice = new FormattedText(pair.Key.ToString("N0" + _priceDecimals.ToString()), _cultureInfoPrices, FlowDirection.LeftToRight, _typeFacePrices, _fontSize, _brushFontColor);
                    

                   _drwCntxtPrices.DrawText(textPrice, new Point((double)((int)_currPriceX), pair.Value));

                   
				}

             
               _bIsNeedDrawPriceAfterCentring = false;
               _isNeedRepaintPrices = false;
            }
           
           
        }

       
     
      //

     


        private Brush GetBrushControlFocus()
        {

            if (SelectionMode.IsModeDrawLevel)
                return new SolidColorBrush(/*Color.FromArgb(0xFF, 0xFF, 0x00, 0xFF)*/ Colors.Red);


            return new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0xFF));
              
        }

      

       


       
      

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
		
            EventsViewModel.CmdSizeChanged.Execute(new object(), this);
        
            ChangeOrdersWidth();
            Repaint = true;
            SaveWidthChange();
                              
        }


     


        private void SaveWidthChange()
        {
            //before initialization complete - nothing to do
            //cause multiple change of stocks
            if (!IsInitializeComplete)
                return;

            _dtLastStockWidthChange = DateTime.Now;
            
            if (_prevWidth != ActualWidth)//only if width changed
                if (!_isInStockChangeMonitoring)//if task is not started yet
                    CUtil.TaskStart(TaskSaveWidthChange);


            _prevWidth = ActualWidth;
            
         
        }

        private void TaskSaveWidthChange()
        {
            const int _minChangeSaveIntervalMSec = 500;
            _isInStockChangeMonitoring = true;

            

            while ((DateTime.Now - _dtLastStockWidthChange).TotalMilliseconds <
                                                        _minChangeSaveIntervalMSec)
                Thread.Sleep(100);

            _guiDispatcher.BeginInvoke(new Action
                    (() =>
                        EventsViewModel.CmdStockWidthChanged.Execute(ActualWidth, this))
                    );

            _isInStockChangeMonitoring = false;

        }





     




    }




}
