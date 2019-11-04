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
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Timers;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Threading;



using Common;
using Common.Utils;



using TradingLib;

using Terminal.Interfaces;
using Terminal.Common;
using Terminal.TradingStructs;
using Terminal.TradingStructs.Clusters;
using Terminal.Controls.Market.Settings;
using Terminal.Graphics;
using Terminal.Events;



namespace Terminal.Controls.Market
{
    /// <summary>
    /// Логика взаимодействия для ControlClusters.xaml
    /// </summary>
    public partial class ControlClusters : UserControl, IStockNumerable
    {

        //internal Canvas ClusterPanel_Canvas;

        CGlyphGenerator _glyphGen = new CGlyphGenerator();

                   
        private Brush _brushCluster = Brushes.WhiteSmoke;
        private Pen _penCluster = new Pen();
        private Pen _penClusterTotalBg;

        private Brush _brushClusterTotal = Brushes.WhiteSmoke;
        private Brush _brushClusterTotalBg;

        private string _tickerName;

        private CultureInfo _cultureInfoDefault = new CultureInfo("en-US");
        private CultureInfo _culterInfoBg;
     
        //<price,coordinate_Y>
        private Dictionary<double, double> dictPriceY = new Dictionary<double, double>();               
       
        private DrawingContext _drwCntxt;
        private DrawingVisual _drwVis = new DrawingVisual();
        // private double m_dTxtFontSize = 10.0;
        private double _volumeRectWidth = 2.0;
   
        private FontWeight _fontWeightDefault = FontWeights.ExtraLight;

        private System.Threading.AutoResetEvent _evPaint = new System.Threading.AutoResetEvent(false);  
       
    
        private int _scrollDelta;
 
       
 
        private Pen _penClusterTotal = new Pen();
     
        //List of cluster images in fact stack structure
        private List<Image> _lstImageSegments = new List<Image>();
        private List<Image> _lstImageSegmentsBg = new List<Image>();

        private Color _colorClusterCurr = Colors.WhiteSmoke;
        
        private FormattedText _FrmTxtCurrClustAmount;
        private Brush _brushTxtDefault = Brushes.Black;
        private FontFamily _fontFamilyDefault = new FontFamily("Verdana");
        private FontFamily _fontFamilyDefaultBg;

        private Typeface _typeFaceDefault;
        private Typeface _typeFaceDefaultBg;

        private CRenderer _renderer;


        private List<string> _dpList = new  List<string> ();

        private CAlarmer _alarmer;


        private int _tSleep=1000;

        private bool _isZoomeMode = false;
        private Color _colorFontBg;
        private Brush _brushFontBg;


        /// <summary>
        /// List of X coord of lines (which are separate images)
        /// </summary>
        private List<double> _lstXLines;

        public bool BlockPaint { get; set; }



        public int Decimals { get; set; }


        public DelFreeSpaceForControlCluster GetAvailableSpaceForScrollViewer;

        public int _fontSizeScaled = 1;


        public bool IsInitializeComplete { get; set; }

        private double _actualHeight = 0;

        public Dictionary<double, double> DictPriceY;


        public Grid GridControlMarket { get; set; }
        private double _widthClusterVisible;
        private double _widthOneCluster;
        private int _colsClustVisble;

        private decimal _dealsAmountFullBar;
        private int _decimalVolume;
        private int _stirngHeight;

        private List<CCluster> _lstPrevDrawClusterDate;
        private List<List<CDrawCluster>> _lstPrevDrawClusterPrice;


        private List<CCluster> _lstDrawClusterDate;
        private List<List<CDrawCluster>> _lstDrawClusterPrice;


        private double _prevWidth = 0;
        private DateTime _dtLastWidthChange = new DateTime();
        private bool _isInWidthChangeMonitoring = false;

        private Dispatcher _guiDispatcher;

        private Point _pntPrev = new Point(0, 0);

        public int ScrollDelta
        {
            get
            {
                return _scrollDelta;
            }
            set
            {
                _scrollDelta = value;
                ForceRepaint();
               
            }
        }


        private DrawingVisual _drwVisClustersBg;
        private DrawingContext _drwCntxtClustersBg;
        private CRendererBackground _rendererClusterBg;
        private CGlyphGenerator _glyphGenBg;


        private bool _isNeedFullRedraw = false;


        private List<DateTime> _lstTimes;

        private AutoResetEvent _evDrawClusters;

        private CClusterPrice _clusterPriceAmount;
        private CClusterDate _clusterDate;




        public ControlClusters()
        {
            InitializeComponent();

            IsInitializeComplete = false;


            GenDPList();

            _renderer = new CRenderer(this, _drwVis);
            _alarmer = CKernelTerminal.GetKernelTerminalInstance().Alarmer;

            if (_typeFaceDefault == null)
            {
                _typeFaceDefault = new Typeface(_fontFamilyDefault, FontStyles.Normal, _fontWeightDefault, new FontStretch());
            }
            _penClusterTotal = new Pen(Brushes.LightGray, 1.0);
            //KAA removed 2016-May-31
           // StringHeight = 13;
            _lstImageSegments.Add(Image_0);
            _lstImageSegments.Add(Image_1);
            _lstImageSegments.Add(Image_2);
            _lstImageSegments.Add(Image_3);
            _lstImageSegments.Add(Image_4);
            _lstImageSegments.Add(Image_5);
            _lstImageSegments.Add(Image_6);
            _lstImageSegments.Add(Image_7);
            _lstImageSegments.Add(Image_8);
            _lstImageSegments.Add(Image_9);

            ScrollViewerClusters.ScrollToRightEnd();

            for (int i = 0; i < _lstImageSegments.Count; i++)
            {
                RenderOptions.SetBitmapScalingMode(_lstImageSegments[i], BitmapScalingMode.NearestNeighbor);
                TextOptions.SetTextRenderingMode(_lstImageSegments[i], TextRenderingMode.ClearType);
                TextOptions.SetTextFormattingMode(_lstImageSegments[i], TextFormattingMode.Display);
            }
            RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);



          //  CUtil.TaskStart(TaskTriggerPaint);
            CUtil.ThreadStart(ThreadDrawClusters);


            SizeChanged += ControlClusters_SizeChanged;

            GridCanvasControlClusters.MouseEnter += GridCanvasControlClusters_MouseEnter;

            _guiDispatcher = Dispatcher.CurrentDispatcher;


       
        }

      

        private void GridCanvasControlClusters_MouseEnter(object sender, MouseEventArgs e)
        {
            _isZoomeMode = false;
        }


        private void ControlClusters_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RebuildLinesCoord();
            SaveWidthChange();
        }


        private void RebuildLinesCoord()
        {
            try
            {
                //
                //
                //
                int cnt = VisualTreeHelper.GetChildrenCount(GridCanvasControlClusters);

                _lstXLines = new List<double>();

                for (int i = 0; i < cnt; i++)
                {

                    DependencyObject depObj = VisualTreeHelper.GetChild(GridCanvasControlClusters, i);
                    List<Point> lst = new List<Point>();
                    if (depObj is Line)
                    {
                        Line ln = (Line)depObj;

                        Point pnt1 = ln.TransformToAncestor(CanvasControlClusters).Transform(new Point(0, 0));
                        _lstXLines.Add(pnt1.X);
                    }



                }
            }
            catch (Exception err)
            {
                Error("Error in RebuildLinesCoord", err);
            }



        }





        //part of experimerntal logics 
        //TOOD appy for other classes if success
        private void GenDPList()
        {
            var props = Type.GetType("Terminal.DataBinding.CClusterProperties").GetProperties();
            props.ToList().ForEach(property => _dpList.Add(property.Name));
          

        }


        private void Error(string msg, Exception e = null)
        {
            if (_alarmer != null)
                _alarmer.Error(msg, e);


        }

        protected override void  OnPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            string propertyName = args.Property.Name;


            if (propertyName == "ClustersUpdatePerSec")
            {
                _tSleep = GetParSleepBeetwenClusterkUpd(); 
           
                  
            }
            
         //  if (propertyName == "TimeFrame")
           //     ForceRepaint();

            if (propertyName == "StringHeight" ||
                propertyName == "FontSize" ||
                propertyName == "ActualHeight" ||
                propertyName == "ActualWidth" ||
                propertyName == "DealsAmountFullBar")
            {
                ForceRepaint();
                _isNeedFullRedraw = true;
            }


        }

        private int GetParSleepBeetwenClusterkUpd()
        {

            double par = 200; //default value

            double maxValue = 1000; //1 per sec
            double minValue = 200; //5 per sec





            double updPerSec = (double)ClustersUpdatePerSec;
                if (updPerSec != 0)
                {

                    par = 1 / updPerSec * 1000;


                }

           

            par = Math.Min(maxValue, par);
            par = Math.Max(minValue, par);


            return (int)par;

        }



        public void OnPriceCoordChanged(object sender, EventArgs e)
        {

            ForcePaint();

        }


      


        private void CanvasControlClustersSizeChanged(object sender, SizeChangedEventArgs e)
        {
          //  System.Threading.Thread.Sleep(0);
            try
            {
                GridCanvasControlClusters.Height = CanvasControlClusters.ActualHeight;
                ScrollViewerClusters.Height = CanvasControlClusters.ActualHeight;

                if (GetAvailableSpaceForScrollViewer != null)
                {
                    bool validFreeSpace = false;
                    double freeWidth = GetAvailableSpaceForScrollViewer(ref validFreeSpace);

                    if (validFreeSpace)
                        ScrollViewerClusters.Width = freeWidth;// -2;

                }
            }
            catch (Exception exc)
            {
                Error("CanvasControlClustersSizeChanged(",exc);
            }

          
        }


        private void SaveWidthChange()
        {
            if (!IsInitializeComplete)
                return;

            _dtLastWidthChange = DateTime.Now;

            if (_prevWidth != ActualWidth) 
                if (!_isInWidthChangeMonitoring)
                    CUtil.TaskStart(TaskSaveWidthChange);



            _prevWidth = ActualWidth;

        }

        private void TaskSaveWidthChange()
        {
            const int _minChangeSaveIntervalMSec = 500;
            _isInWidthChangeMonitoring = true;



            while ((DateTime.Now - _dtLastWidthChange).TotalMilliseconds <
                                                        _minChangeSaveIntervalMSec)
                Thread.Sleep(100);
            
            _guiDispatcher.BeginInvoke(new Action
                    (() =>
                        EventsViewModel.CmdClusterWidthChanged.Execute(ActualWidth, this))
                    );
                   
            _isInWidthChangeMonitoring = false;




        }


        private void GridCanvasControlClusters_MouseDown(object sender, MouseButtonEventArgs e)
        {
       

        }

        private void GridCanvasControlClusters_MouseLeave(object sender, MouseEventArgs e)
        {

           
           
           
        }



      
        /// <summary>
        /// If mouse is near "Line" object do change mouse cursor 
        /// to "ScrollWE" (two arrows - west and east) to show user 
        /// another mode of scrolling. 
        /// There two mode of determine area - less accuracity 
        /// for the first line (nearest to control ControlDeals),
        /// and more accuracity for the others.
        /// 
        /// Added 2018-04-30
        /// Mod 2018-05-01
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridCanvasControlClusters_MouseMove(object sender, MouseEventArgs e)
        {
            Point pnt = e.GetPosition(CanvasControlClusters);
           if (_isZoomeMode)
                if (_pntPrev.X != 0 && _pntPrev.Y!=0)
            {
                double dist = Math.Sqrt(Math.Pow(pnt.X - _pntPrev.X, 2) + Math.Pow(pnt.Y - _pntPrev.Y, 2));

                    if (dist > 10)
                    {
                        _isZoomeMode = false;

                        //Mouse.SetCursor(Cursors.Arrow);
                        GridCanvasControlClusters.Cursor = Cursors.Arrow;
                        
                        return;
                    }
            }

            _pntPrev = pnt;

            double dltFirst = 5;
            double dltOther = 2;

            for (int i=0; i<_lstXLines.Count;i++ )
            {
                double dltCurr = Math.Abs(_lstXLines[i] - pnt.X);                
                if ((i==0 && dltCurr < dltFirst  ) ||
                     (dltCurr  < dltOther ))
                {
                    // Mouse.SetCursor(Cursors.ScrollWE);
                    GridCanvasControlClusters.Cursor = Cursors.ScrollWE;
                     _isZoomeMode = true;
                    return;
                }
            }

            if (!_isZoomeMode)
            {
                //Mouse.SetCursor(Cursors.Arrow);
                GridCanvasControlClusters.Cursor = Cursors.Arrow;

               _isZoomeMode = false;
            }
        }




        private void GridCanvasControlClusters_MouseUp(object sender, MouseButtonEventArgs e)
        {
           
           
                       
        }

		private void GenClusterBrushPen(CCluster clusterData, DateTime Time)
		{
			double buyAmount = clusterData.AmountBuy;
			double sellAmount = clusterData.AmountSell;

			double balance = 0;



			if (clusterData.TotalDir == EnmDealDirection.Sell)
			{
				if (sellAmount !=0)
					balance = (200.0 * buyAmount) / sellAmount;

				_colorClusterCurr = Color.FromArgb(0x73, 200, Convert.ToByte(balance), Convert.ToByte(balance));
			}
			else
			{
				if (buyAmount !=0)
					balance = (200.0 * sellAmount) / buyAmount;
				_colorClusterCurr = Color.FromArgb(0x73,  Convert.ToByte(balance),200, Convert.ToByte(balance));

			}

			
			_brushCluster = new SolidColorBrush(_colorClusterCurr);
			_penCluster = new Pen(_brushCluster, 1.0);
		}




		
        private TextBlock CreateTimeSpanLabel(DateTime dt)
        {
            //KAA 2016-12-19
            TextBlock element = new TextBlock
            {
                FontSize =  _fontSizeScaled,   //FontSize, 
                Foreground = _brushTxtDefault,
                Margin = new Thickness(0.0, 0.5, 0.0, 0.0),
                FontFamily = _fontFamilyDefault,
                FontWeight = FontWeights.Normal,
                TextAlignment = TextAlignment.Center,
                Height  = StringHeight,
                Background = Brushes.LightGray,
               
            };
            TextOptions.SetTextFormattingMode(element, TextFormattingMode.Display);
            TextOptions.SetTextRenderingMode(element, TextRenderingMode.Aliased);

           
            element.Text = dt.ToString("HH:mm");


            return element;
        }






        /// <summary>
        /// Call from OnPaint
        /// 
        /// TODO optimize !!!
        /// </summary>
        private void UpdateTimesAxe()
        {
            //not created yet
            if (LstTimes.Count == 0)
                return;



            //remove all textblocks with times
            for (int i = 0; i < GridCanvasControlClusters.Children.Count; i++)            
                //in grid child we interested only in Textblocks
                if (GridCanvasControlClusters.Children[i] is TextBlock)                
                    GridCanvasControlClusters.Children.Remove(GridCanvasControlClusters.Children[i]);
                
            

            DateTime dt = new DateTime(0);

            //Create new textblock elements
            for (int j = GridCanvasControlClusters.ColumnDefinitions.Count - 1; j > 0; j--)
            {

                //if (TickerName == "undefined")
                  //  Thread.Sleep(0);


                int ind = LstTimes.Count - j / 2 - 1;

                dt = LstTimes[ind];

              

                TextBlock element = CreateTimeSpanLabel(dt);

      
                GridCanvasControlClusters.Children.Add(element);
                //ClusterPanel_Grid.ColumnDefinitions[j].Width = 60;
                Grid.SetRow(element, 1);
                Grid.SetColumn(element, j);
                //note: double decrese as we have other elemnts as well
                j--; 
            }



        }





        

        private void ForcePaint()
        {

            _evPaint.Set();
        }

     


        private FormattedText GetFormattedTextBg(string text)
        {
            Brush brush = Brushes.Black;
            if (_brushFontBg != null)
                brush = _brushFontBg;

            // return new FormattedText(text, _cultureInfoDefault, FlowDirection.LeftToRight, _typeFaceDefault, FontSize, brush);
            //2018-05-04
            return new FormattedText(text, _culterInfoBg, FlowDirection.LeftToRight, _typeFaceDefaultBg, _fontSizeScaled, brush);
        }




        //Depend on comarison buy sell getting brush
        private void GenClusterTotalBrushBg(int amountBuy, int amountSell)
        {
            if (amountBuy > amountSell)
                _brushClusterTotalBg = Brushes.LightGreen;
            else
                _brushClusterTotalBg = Brushes.LightCoral;

        }


        

        private void DrawClusterDateBg(CCluster cluster, double actualCanvasWidth)
        {
         
            //double actualWidthClusterGrid = GridCanvasControlClusters.ColumnDefinitions[1].ActualWidth;

            GenClusterTotalBrushBg(cluster.AmountBuy, cluster.AmountSell);

            double offsetBottom = 12;//27.0;
            double y = _actualHeight - _stirngHeight - offsetBottom;

            //Draw rectangle with summary of period on bottom of screen
            //DrawRectangle(_brushClusterTotal, _penClusterTotal, new Rect(0.0, y, actualCanvasWidth, (double)_stirngHeight));
            _drwCntxtClustersBg.DrawRectangle(_brushClusterTotalBg, _penClusterTotalBg,
                                                new Rect(0.0, y, actualCanvasWidth, (double)_stirngHeight));
            //2018-07-10
            string stAmountTotal = CTerminalUtil.GetAmount(cluster.AmountTotal, _decimalVolume);

            _FrmTxtCurrClustAmount = GetFormattedTextBg(" " + stAmountTotal);

            _drwCntxtClustersBg.DrawText(_FrmTxtCurrClustAmount, new Point(0.0, y));
            
           

            
        }




        private void DrawClusterPriceBarBg(/*double price, int amount*/CCluster clusterData, double y, DateTime dtInterval, double actualWidthClusterGrid)
        {

            GenClusterBrushPen(clusterData, dtInterval);

            //calculate width of cluster bar
            long dealsAmountFullBar = CUtilConv.GetIntVolume(Math.Max(_dealsAmountFullBar, 1), _decimalVolume);//zero devide protect
            _volumeRectWidth = (((double)clusterData.AmountTotal) / ((double)dealsAmountFullBar)) * actualWidthClusterGrid;
            //draw cluster bar with proportional width
            //DrawRectangle(_brushCluster, _penCluster, new Rect(0.0, y, _volumeRectWidth, (double)_stirngHeight));
            _drwCntxtClustersBg.DrawRectangle(_brushCluster, _penCluster, new Rect(0.0, y, _volumeRectWidth, (double)_stirngHeight));


            //   FormattedText(text, _cultureInfoDefault, FlowDirection.LeftToRight, _typeFaceDefault, _fontSizeScaled, brush);

            //2018-02-21
            string stAmount = CTerminalUtil.GetAmount(clusterData.AmountTotal, _decimalVolume);



            //print cluster amount
            //DrawText(_FrmTxtCurrClustAmount, new Point(0.0, y));
            //KAA 2017-03-01 changed to Glyph
            //2018-05-01 changed to fontsizeScaled
            GlyphRun gr = _glyphGenBg.GetGlyph(0, y + _fontSizeScaled,
                                             _fontSizeScaled,
                                            " " + /*clusterData.AmountTotal.ToString()*/stAmount);
            _drwCntxtClustersBg.DrawGlyphRun(_brushFontBg, gr);


        }
        
       

        private List<int> GetNeedDrawImagesInd()                                        
        {
                       
            List<int> _lstRes = new List<int>();

            try
            {

                //No cluster list formed yet do  - need full redraw
                if (_lstPrevDrawClusterDate == null ||
                    _lstPrevDrawClusterPrice == null || _lstPrevDrawClusterDate.Count == 0 ||
                    _lstDrawClusterDate.Count == 0)
                {
                    for (int i = 0; i < _lstTimes.Count; i++)
                        _lstRes.Add(i);

                    return _lstRes;

                }


              
                if (_lstTimes.Count - _lstPrevDrawClusterDate.Count > 0)
                {
                    for (int i = _lstPrevDrawClusterDate.Count; i < _lstTimes.Count; i++)
                        if (!_lstRes.Contains(i))
                            _lstRes.Add(i);

                }
                



                //check summary clusters if changed - need redraw
                for (int i = 0; i < _lstTimes.Count; i++)
                {

                    //already in list
                    if (_lstRes.Contains(i))
                        continue;


                    if (i > _lstPrevDrawClusterDate.Count)
                        break; //we remember it previously

                    if (_lstPrevDrawClusterDate[i] == null &&
                        _lstDrawClusterDate[i] == null)
                        continue;




                    if ((_lstPrevDrawClusterDate[i] == null && _lstDrawClusterDate[i] != null) ||
                        (_lstPrevDrawClusterDate[i] != null && _lstDrawClusterDate[i] == null))
                        _lstRes.Add(i);
                    else if (_lstPrevDrawClusterDate[i].AmountTotal != _lstDrawClusterDate[i].AmountTotal)
                        _lstRes.Add(i);

                }

                if (_lstDrawClusterPrice!=null && _lstPrevDrawClusterPrice!=null &&
                    _lstPrevDrawClusterPrice.Count == _lstDrawClusterPrice.Count)
                {

                    for (int i = 0; i < _lstTimes.Count; i++)
                    {
                        //already in list
                        if (_lstRes.Contains(i))
                            continue;

                            int cnt1 = _lstDrawClusterPrice[i].Count;
                            int cnt2 = _lstPrevDrawClusterPrice[i].Count;

                            if (cnt1!=cnt2)
                            {
                                _lstRes.Add(i);
                                continue;
                            }


                       
                            for (int j = 0; j < _lstPrevDrawClusterPrice[i].Count; j++)
                            {
                                if (_lstPrevDrawClusterPrice[i][j].Cluster.AmountTotal !=
                                    _lstDrawClusterPrice[i][j].Cluster.AmountTotal ||
                                    _lstDrawClusterPrice[i][j].Y != _lstPrevDrawClusterPrice[i][j].Y ||
                                    _lstDrawClusterPrice[i][j].Width != _lstPrevDrawClusterPrice[i][j].Width)
                                {
                                    _lstRes.Add(i);
                                    break;
                                }
                            }
                        
                      


                    }
                }
                _lstRes.Sort();

            }
            catch (Exception e)
            {
                Error("GetNeedDrawImagesInd",e);
            }
                    

            return _lstRes;
        }




    


        private void CopyDataPrevIteration()
        {
            try
            {
                //Copy data of prev iteration
                if (_lstDrawClusterDate != null && _lstDrawClusterPrice != null)
                {
                    // _lstPrevDrawClusterDate = new List<CCluster>(_lstDrawClusterDate);
                    _lstPrevDrawClusterDate = new List<CCluster>();

                    _lstPrevDrawClusterPrice = new List<List<CDrawCluster>>();

                    foreach (var el in _lstDrawClusterDate)
                    {
                        if (el == null)
                            _lstPrevDrawClusterDate.Add(el);
                        else
                        {
                            _lstPrevDrawClusterDate.Add(el.Copy());
                        }
                    }

                    for (int i = 0; i < _lstDrawClusterPrice.Count; i++)
                    {
                        _lstPrevDrawClusterPrice.Add(new List<CDrawCluster>());
                        for (int j = 0; j < _lstDrawClusterPrice[i].Count; j++)
                        {         
                            
                            CDrawCluster drwClust = _lstDrawClusterPrice[i][j].Copy();
                            _lstPrevDrawClusterPrice[i].Add(drwClust);
                        }
                    }


                }

            }
            catch (Exception e)
            {
                Error("ContolStock.CopyDataPrevIteration",e);
            }


        }


       


        private void DrawClustersBg()
        {
            double actualWidthClusterGrid=0;


            _lstTimes = new List<DateTime>();

            Dictionary<double, double> dictPriceY = null;


            CopyDataPrevIteration();


            bool bNoNeedDraw = false;




            _guiDispatcher.Invoke(new Action
                (() =>
                    {

                        try
                        {
                            if (GridControlMarket==null || GridControlMarket.ColumnDefinitions.Count==0||
                                GridCanvasControlClusters==null || GridCanvasControlClusters.ColumnDefinitions.Count<2)
                            {
                                bNoNeedDraw = true;
                                return;
                            }

                            _widthClusterVisible = GridControlMarket.ColumnDefinitions[0].ActualWidth;
                            _widthOneCluster = GridCanvasControlClusters.ColumnDefinitions[1].ActualWidth;


                            if (_widthClusterVisible<=0 || _widthClusterVisible<=0)
                            {
                                bNoNeedDraw = true;
                                return;
                            }


                            _lstDrawClusterPrice = new List<List<CDrawCluster>>();
                            _lstDrawClusterDate = new List<CCluster>();
                          

                            lock (DictPriceY)
                                dictPriceY = new Dictionary<double, double>(DictPriceY);

                            if (dictPriceY.Count == 0)
                            {
                                bNoNeedDraw = true;
                                return;
                            }

                            actualWidthClusterGrid = GridCanvasControlClusters.ColumnDefinitions[1].ActualWidth;

                            _colsClustVisble = (int)(_widthClusterVisible / _widthOneCluster) + 1;

                            lock (LstTimes)
                                _lstTimes = new List<DateTime>(LstTimes);

                            _lstTimes.RemoveRange(_colsClustVisble, _lstTimes.Count- _colsClustVisble);

                            _dealsAmountFullBar = DealsAmountFullBar;
                            _decimalVolume = DecimalVolume;

                            _stirngHeight = StringHeight;

                           

                          

                           


                            _clusterPriceAmount = ClusterPriceAmount;
                            _clusterDate = ClusterDate;


                            CCluster clusterDateSrc = /*ClusterDate*/ ClusterDate.GetValue(_lstTimes[0]);



                        }
                        catch (Exception exc)
                        {
                            Error("DrawClustersBackground.GUIDisp", exc);
                        }
                    }
                ));


          

            //double actualWidthClusterGrid = GridCanvasControlClusters.ColumnDefinitions[1].ActualWidth;
            try
            {
                if (bNoNeedDraw)
                    return;



                for (int i = 0; i < _lstTimes.Count; i++)
                {

                    DateTime dt = _lstTimes[i];

                    _lstDrawClusterPrice.Add(new List<CDrawCluster>());

                    CCluster clusterDateSrc = /*ClusterDate*/ _clusterDate .GetValue(dt);

                    CCluster clusterDate = null;
                    if (clusterDateSrc != null)
                        clusterDate = clusterDateSrc.Copy();

                    _lstDrawClusterDate.Add(clusterDate);

                    //mod 2018-06-28 to make possible fractial prices

                    //foreach (var kvp in DictPriceY)                   
                    for (int j = 0; j < dictPriceY.Count - 1; j++)
                    {
                        var kvp = dictPriceY.ElementAt(j);
                        double price = kvp.Key;

                        if (price < 0)
                            continue;

                        double y = kvp.Value;
                        var kvpNext = dictPriceY.ElementAt(j + 1);
                        double priceNext = kvpNext.Key;
                        double dlt = priceNext - price;

                        double priceFrom = price - dlt / 2;
                        double priceTo = price + dlt / 2;

                        // CCluster clusterPrice = ClusterPriceAmount.GetClusterData(price, dt);
                        CCluster clusterPrice = _clusterPriceAmount.GetAmountInPriceArea(priceFrom, priceTo, dt);



                        if (clusterPrice != null)
                        {

                            if (clusterPrice.AmountTotal != 0)
                                // DrawClusterPriceBar(clusterPrice, y, dt, actualWidthClusterGrid);
                                _lstDrawClusterPrice[i].Add(new CDrawCluster()
                                {
                                    Cluster = clusterPrice.Copy(),
                                    Y = y,
                                    Dt = dt,
                                    Width = actualWidthClusterGrid


                                }
                                         );


                        }

                    }



                }





                List<int> lstRedrawSegments = GetNeedDrawImagesInd( );



                int _cntUse = Math.Min(_lstDrawClusterPrice.Count, _colsClustVisble);


                for (int i = 0; i < _cntUse; i++)
                {

                    if (!lstRedrawSegments.Contains(i)&& !_isNeedFullRedraw)
                        continue;

                    _drwCntxtClustersBg = _drwVisClustersBg.RenderOpen();

                   

                    foreach (var el in _lstDrawClusterPrice[i])
                        DrawClusterPriceBarBg(el.Cluster, el.Y, el.Dt, el.Width);

                    if (_lstDrawClusterDate[i] != null)
                        DrawClusterDateBg(_lstDrawClusterDate[i], actualWidthClusterGrid);


                    //  CCluster clusterDate = ClusterDate.GetValue(dt);

                    //  if (clusterDate != null)
                    //  DrawClusterDate(clusterDate);
                    _drwCntxtClustersBg.Close();
                    _rendererClusterBg.Render(_lstImageSegmentsBg[i]);

                    _isNeedFullRedraw = false;

                }
            }
            catch (Exception e)
            {
                Error("DrawClusterBg",e);
            }
            

        }






       
       
        public void ForceRepaint()
        {
            if (_evDrawClusters!=null)
                _evDrawClusters.Set();
            
        }

      
        private void WaitImagesInit()
        {
            while (Image_0 == null || Image_1 == null || Image_2 == null ||
                    Image_3 == null || Image_4 == null || Image_5 == null ||
                    Image_6 == null || Image_7 == null || Image_8 == null ||
                    Image_9 == null)
                        System.Threading.Thread.Sleep(200);
            
        }      

        public void ThreadDrawClusters()
        {

            try
            {
                _drwVisClustersBg = new DrawingVisual();
              

               _glyphGenBg = new CGlyphGenerator();
                _rendererClusterBg = new CRendererBackground(this, _drwVisClustersBg, _guiDispatcher);
                _culterInfoBg = new CultureInfo("en-US");
                _fontFamilyDefaultBg = new FontFamily("Verdana");
                _typeFaceDefaultBg = new Typeface(_fontFamilyDefaultBg, FontStyles.Normal, _fontWeightDefault, new FontStretch());

                _evDrawClusters = new AutoResetEvent(true);

              

            }
            catch (Exception e)
            {

                Error("ThreadDrawClusters.Create", e);

            }

           



            _brushFontBg = new SolidColorBrush(Colors.White);

            WaitImagesInit();//2018-06-09

            _lstImageSegmentsBg.Add(Image_0);
            _lstImageSegmentsBg.Add(Image_1);
            _lstImageSegmentsBg.Add(Image_2);
            _lstImageSegmentsBg.Add(Image_3);
            _lstImageSegmentsBg.Add(Image_4);
            _lstImageSegmentsBg.Add(Image_5);
            _lstImageSegmentsBg.Add(Image_6);
            _lstImageSegmentsBg.Add(Image_7);
            _lstImageSegmentsBg.Add(Image_8);
            _lstImageSegmentsBg.Add(Image_9);
            
            //This is specieal pause which sets time to allow other contols
            //load. Without this pause prices will draw a very long time - about 20 seconds
            System.Threading.Thread.Sleep(15000);

            while (true)
            {
                try
                {
                   
                   
                   _evDrawClusters.WaitOne(500);
                    _isNeedFullRedraw = true;
                   

                    if (_disablePaintClusters)
                    {
                        Thread.Sleep(100);
                        continue;
                    }


                    _guiDispatcher.Invoke(new Action(() =>
                    {
                        DisableRecalcClusters = true;


                        _actualHeight = ActualHeight;
                        _isNeedFullRedraw = true;

                        CTerminalUtil.UpdateLocalColors(FontColor, ref _colorFontBg);

                       

                        UpdateTimesAxe();

                    }
                    ));


                    UpdateBrushes();

                    DrawClustersBg();



                    _guiDispatcher.Invoke(new Action(() => DisableRecalcClusters = false));




                    //Thread.Sleep(500);
                }
                catch (Exception e)
                {
                    Error("ThreadDrawClusters(", e);
                    Thread.Sleep(100);

                }
                finally
                {
                    _guiDispatcher.Invoke(new Action(() =>
                    {
                        DisableRecalcClusters = false;
                    }));
                }




            }


        }

        private void UpdateBrushes()
        {

            CTerminalUtil.UpdateBrush(_colorFontBg, ref _brushFontBg);

        }






        // Load settings for cluster panel here
        private void ActionUpdateSettings()
        {
       
          
           
           

            
        }

        public void UpdateSettings()
        {
            Action action5 = new Action(ActionUpdateSettings);
            base.Dispatcher.Invoke(DispatcherPriority.Input, action5);


        }


        public void InitFontSizeScaled()
        {
            _fontSizeScaled = FontSize;


        }

        private int _iCntr = 0;

        private void ScrollViewerClusters_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

            
            

            if (_isZoomeMode)
            {
                double deltaZoom = e.Delta > 0 ? 20 : -20;
                double newWidth = Math.Max(GridCanvasControlClusters.Width + deltaZoom, GridCanvasControlClusters.MinWidth);
                newWidth = Math.Min(newWidth, GridCanvasControlClusters.MaxWidth);

                GridCanvasControlClusters.Width = newWidth;

               
                RebuildLinesCoord();
                _iCntr++;
                if (_iCntr % 2 == 0)
                {
                    int deltaScaleFont = e.Delta > 0 ? 1 : -1;
                    _fontSizeScaled = Math.Max(_fontSizeScaled + deltaScaleFont, 1);
                    _fontSizeScaled = Math.Min(_fontSizeScaled, FontSize);
                }
                  //Mouse.SetCursor(Cursors.ScrollWE);
            }            
            else
            {
                double increment = e.Delta > 0 ? 20 : -20;

                double newValue;
                newValue = Math.Min(ScrollViewerClusters.HorizontalOffset + increment, ScrollViewerClusters.ScrollableWidth);
                newValue = Math.Max(ScrollViewerClusters.HorizontalOffset + increment, 0);

                ScrollViewerClusters.ScrollToHorizontalOffset(newValue);
            }


        }
        
       
        
       
    }
}
