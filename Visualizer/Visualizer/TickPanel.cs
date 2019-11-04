namespace Visualizer
{
    using IntelliTradeComplex2.Modules;
    using Model;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Timers;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    using Xceed.Wpf.Toolkit;
    using IntelliTradeComplex2.Trade;

    using GUIComponents;
    


    public class TickPanel : UserControl, IComponentConnector
    {
        private bool _contentLoaded;
        public static readonly DependencyProperty LeadersDataSourcesProperty = DependencyProperty.Register("LeadersDataSources", typeof(List<GraphicDataExternalSource>), typeof(TickPanel));
        public static readonly DependencyProperty LeadersProperty = DependencyProperty.Register("Leaders", typeof(List<Leader>), typeof(TickPanel));
        public static readonly DependencyProperty LeadersTickSourcesProperty = DependencyProperty.Register("LeadersTickSources", typeof(List<TickGraphicData>), typeof(TickPanel));
        private RenderTargetBitmap m_Bmp;
        private RenderTargetBitmap m_Bmp_Ticks;
        private bool m_bPriceCoordinFilled;
        private bool m_bSettingsShared = true; //KAA
        private bool m_bShowDots = true;
        private bool m_bShowLines;
        private bool m_bTicksFilled;
        private Brush m_BuyBrush = new SolidColorBrush(Colors.LightGreen);
        private Brush m_BuyOrSellBrush = Brushes.WhiteSmoke;
        private CultureInfo m_CultureInfo = new CultureInfo("en-US");
        private Dictionary<double, double> m_Dict_KPriceVCoordin = new Dictionary<double, double>();
        private double m_dR0 = 4.5;
        private double m_dR1 = 7.0;
        private double m_dR2 = 10.0;
        private double m_dR3 = 14.0;
        private double m_dR4 = 18.0;
        private double m_dR5 = 21.0;
        private double m_dR6 = 25.0;
        private double m_dRadius;
        private DrawingContext m_DrawCont;
        private DrawingContext m_DrawCont_Ticks;
        private DrawingVisual m_DrawVis = new DrawingVisual();
        private DrawingVisual m_DrawVis_Ticks = new DrawingVisual();
       // private double m_dTxtFontSize = 10.0;
        private Pen m_EllipsePen;
        private int m_iFilterTicksFrom;
        private int m_iFocusAsk;
        private int m_iFocusBid;
        private int m_iScrollDelta;
        private int m_iShowTicksFrom;
        private int m_iTicksWeight = 1;
        private int m_iVolumeRectH = 13;
        private List<Tick_Info> m_lAllTicks = new List<Tick_Info>();
        private Brush m_LeadersBrush = Brushes.Blue;
        private Brush m_LevelBrush = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0));
        private Pen m_LevelPen;
        private List<string> m_lForFile = new List<string>();
        private Brush m_LinesBrush = Brushes.Black;
        private Pen m_LinesPen = new Pen(Brushes.Black, 1.0);
        private List<Leader> m_lLeaders = new List<Leader>();
        private List<Brush> m_lLeadersBrushes = new List<Brush>();
        private List<Pen> m_lLeadersPens = new List<Pen>();
        private List<TickGraphicData> m_lLeadersTickSources = new List<TickGraphicData>();
        private Path m_Path_Leaders;
        private PathFigure m_PFigure_Leaders;
        private PathGeometry m_PGeometry_Leaders;
        private Brush m_SellBrush = new SolidColorBrush(Colors.LightCoral);
        private SettingsWindow m_SettingsWin;
        private string m_sTicksStyle = "Dots";
        private Tick_Info m_Ticks;
        private Timer m_Timer_Leaders;
        private Timer m_Timer_Levels;
        private Timer m_Timer_Ticks;
        private FontFamily m_TxtFontFam = new FontFamily("Verdana");
        private FormattedText m_TxtPercent;
        private FormattedText m_TxtSign;
        private FormattedText m_TxtTickAmo;
        private Typeface m_TypeFace;
        internal Canvas TickPanel_Canvas;
        internal Image TickPanel_Image;
        internal Image TickPanel_LevelsImage;
        internal Image TickPanel_TicksImage;
        public static readonly DependencyProperty TicksProperty = DependencyProperty.Register("Ticks", typeof(Tick_Info[]), typeof(TickPanel));



        public static readonly DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(double), typeof(TickPanel));



        // == SHARED SETTINGS == Step 5.2 create DP in separate control
        
        public int StringHeight
        {
            get { return (int)GetValue(StringHeightProperty); }
            //set { SetValue(StringHeightProperty, value); }
        }



        // Using a DependencyProperty as the backing store for StringHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StringHeightProperty =
            DependencyProperty.Register("StringHeight", typeof(int), typeof(TickPanel), new UIPropertyMetadata(1));



        public int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            //set { SetValue(FontSizetProperty, value); }
        }



        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(int), typeof(TickPanel), new UIPropertyMetadata(1));







        public ControlWorkAmount WorkAmountGrid { get; set; }

        public TickPanel()
        {
            this.InitializeComponent();
            this.InitializeLeadersBrushes();
            this.m_LevelPen = new Pen(this.m_LevelBrush, 0.75);
            this.m_EllipsePen = new Pen(Brushes.Gray, 1.0);
            //this.StringHeight = 13;
            if (this.m_TypeFace == null)
            {
                this.m_TypeFace = new Typeface(this.m_TxtFontFam, FontStyles.Normal, FontWeights.SemiBold, new FontStretch());
            }
            //KAA 2016-06-01
            this.m_TxtSign = new FormattedText("!", this.m_CultureInfo, FlowDirection.LeftToRight, this.m_TypeFace,
                                                    /*this.m_dTxtFontSize*/this.FontSize  * 1.25, Brushes.Black);
            this.m_Timer_Ticks = new Timer(100.0);//KAA was 45
            this.m_Timer_Ticks.Elapsed += new ElapsedEventHandler(this.m_TicksTimer_Elapsed);
            this.m_Timer_Ticks.Start();
            this.m_Timer_Leaders = new Timer(250.0);
            this.m_Timer_Leaders.Elapsed += new ElapsedEventHandler(this.m_LeadersTimer_Elapsed);
            this.m_Timer_Leaders.Start();
            this.m_Timer_Levels = new Timer(60.0);
            this.m_Timer_Levels.Elapsed += new ElapsedEventHandler(this.m_LevelsTimer_Elapsed);
            this.m_Timer_Levels.Start();
            RenderOptions.SetEdgeMode(this.TickPanel_TicksImage, EdgeMode.Aliased);
            RenderOptions.SetBitmapScalingMode(this.TickPanel_TicksImage, BitmapScalingMode.NearestNeighbor);
            TextOptions.SetTextRenderingMode(this.TickPanel_TicksImage, TextRenderingMode.ClearType);
            TextOptions.SetTextFormattingMode(this.TickPanel_TicksImage, TextFormattingMode.Display);
           


            WorkAmountGrid = new ControlWorkAmount();
          
             Canvas.SetBottom(WorkAmountGrid, 20);
             Canvas.SetLeft(WorkAmountGrid, 1);
          //   Canvas.SetZIndex(WorkAmountGrid, Int32.MaxValue);


             TickPanel_Canvas.Children.Add(WorkAmountGrid);
          
        }

    



        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/Visualizer;component/tickpanel.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void InitializeLeadersBrushes()
        {
            Brush blue = Brushes.Blue;
            this.m_lLeadersBrushes.Add(blue);
            Brush green = Brushes.Green;
            this.m_lLeadersBrushes.Add(green);
            Brush red = Brushes.Red;
            this.m_lLeadersBrushes.Add(red);
            Brush black = Brushes.Black;
            this.m_lLeadersBrushes.Add(black);
            Brush lightCoral = Brushes.LightCoral;
            this.m_lLeadersBrushes.Add(lightCoral);
            Brush gray = Brushes.Gray;
            this.m_lLeadersBrushes.Add(gray);
            Brush orange = Brushes.Orange;
            this.m_lLeadersBrushes.Add(orange);
            Brush brown = Brushes.Brown;
            this.m_lLeadersBrushes.Add(brown);
            Pen item = new Pen(blue, 2.0);
            this.m_lLeadersPens.Add(item);
            Pen pen2 = new Pen(green, 2.0);
            this.m_lLeadersPens.Add(pen2);
            Pen pen3 = new Pen(red, 2.0);
            this.m_lLeadersPens.Add(pen3);
            Pen pen4 = new Pen(black, 2.0);
            this.m_lLeadersPens.Add(pen4);
            Pen pen5 = new Pen(lightCoral, 2.0);
            this.m_lLeadersPens.Add(pen5);
            Pen pen6 = new Pen(gray, 2.0);
            this.m_lLeadersPens.Add(pen6);
            Pen pen7 = new Pen(orange, 2.0);
            this.m_lLeadersPens.Add(pen7);
            Pen pen8 = new Pen(brown, 2.0);
            this.m_lLeadersPens.Add(pen8);
        }

        private void m_LeadersTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action method = new Action(this.OnPaint_Leaders);
            base.Dispatcher.Invoke(DispatcherPriority.Background, method);
        }

        private void m_LevelsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action method = new Action(this.OnPaint_Levels);
            base.Dispatcher.Invoke(DispatcherPriority.Background, method);
        }

        private void m_TicksTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action method = new Action(this.OnPaint);
            base.Dispatcher.Invoke(DispatcherPriority.Render, method);
        }

        public void OnPaint()
        {

            //KAA 2015_01_19
         //   m_bTicksFilled = true;

            if ((this.m_bSettingsShared && !this.BlockPaint) && this.m_bTicksFilled)
            {
                this.m_DrawCont_Ticks = this.m_DrawVis_Ticks.RenderOpen();
                double actualWidth = base.ActualWidth;
                double y = 0.0;
                double x = 0.0;
                double num4 = 0.0;
                double priceHighest = 0.0;
                double yLowest = 0.0;
                bool flag = true;
                Point point = new Point(0.0, 0.0);
                Tick_Info[] infoArray = null;
                lock (this.m_lAllTicks)
                {
                    infoArray = this.m_lAllTicks.ToArray();
                }
                if (infoArray != null)
                {
                    if (this.m_Dict_KPriceVCoordin.Count > 0)
                    {
                        KeyValuePair<double, double> pair = this.m_Dict_KPriceVCoordin.ElementAt<KeyValuePair<double, double>>(0);
                        priceHighest = pair.Key;
                        yLowest = pair.Value;
                    }
                    for (int i = infoArray.Length - 1; i > -1; i--)
                    {
                        Tick_Info info = infoArray[i];
                        if (info == null)
                        {
                            break;
                        }
                        if (info.Amount < 10)
                        {
                            this.m_dRadius = this.m_dR1;
                        }
                        if (info.Amount >= 10)
                        {
                            this.m_dRadius = this.m_dR2;
                        }
                        if (info.Amount >= 100)
                        {
                            this.m_dRadius = this.m_dR3;
                        }
                        if (info.Amount >= 1000)
                        {
                            this.m_dRadius = this.m_dR4;
                        }
                        if (info.Amount >= 10000)
                        {
                            this.m_dRadius = this.m_dR5;
                        }
                        if (info.Amount >= 100000)
                        {
                            this.m_dRadius = this.m_dR6;
                        }
                        if (info.Amount > 0)
                        {
                            if (info.Amount < this.m_iShowTicksFrom)
                            {
                                this.m_dRadius = this.m_dR0;
                            }
                            actualWidth -= this.m_dRadius;
                            if (this.m_bShowLines)
                            {
                                y = (yLowest + (this.StringHeight * Math.Round((double) ((priceHighest - info.Price) / this.Step), 0))) + (this.StringHeight / 2);
                                if (i != (infoArray.Length - 1))
                                {
                                    this.m_DrawCont_Ticks.DrawLine(this.m_LinesPen, point, new Point(actualWidth, y));
                                }
                            }
                            point = new Point(actualWidth, y);
                        }
                        if ((actualWidth < 0.0) || (i == 0))
                        {
                            if (this.m_bShowDots)
                            {
                                for (int j = i; j < infoArray.Length; j++)
                                {
                                    info = infoArray[j];
                                    if (info.Direction == TickDirection.Buy)
                                    {
                                        this.m_BuyOrSellBrush = this.m_BuyBrush;
                                    }
                                    else
                                    {
                                        this.m_BuyOrSellBrush = this.m_SellBrush;
                                    }
                                    if (j != i)
                                    {
                                        actualWidth += this.m_dRadius;
                                    }
                                    if (info.Amount < 10)
                                    {
                                        this.m_dRadius = this.m_dR1;
                                    }
                                    if (info.Amount >= 10)
                                    {
                                        this.m_dRadius = this.m_dR2;
                                    }
                                    if (info.Amount >= 100)
                                    {
                                        this.m_dRadius = this.m_dR3;
                                    }
                                    if (info.Amount >= 0x3e8)
                                    {
                                        this.m_dRadius = this.m_dR4;
                                    }
                                    if (info.Amount >= 0x2710)
                                    {
                                        this.m_dRadius = this.m_dR5;
                                    }
                                    if (info.Amount >= 0x186a0)
                                    {
                                        this.m_dRadius = this.m_dR6;
                                    }
                                    //KAA 2016-06-01
                                    this.m_TxtTickAmo = new FormattedText(info.Amount.ToString(), this.m_CultureInfo, FlowDirection.LeftToRight, this.m_TypeFace, 
                                                                            /*this.m_dTxtFontSize*/ this.FontSize, Brushes.Black);
                                    if (info.Amount < this.m_iShowTicksFrom)
                                    {
                                        flag = false;
                                        this.m_dRadius = this.m_dR0;
                                    }
                                    else
                                    {
                                        flag = true;
                                    }
                                    if (this.m_Dict_KPriceVCoordin.ContainsKey(info.Price))
                                    {
                                        y = this.m_Dict_KPriceVCoordin[info.Price] + (this.StringHeight / 2);
                                        x = actualWidth - (this.m_TxtTickAmo.Width / 2.0);
                                        num4 = y - (this.m_TxtTickAmo.Height / 2.0);
                                        this.m_DrawCont_Ticks.DrawEllipse(this.m_BuyOrSellBrush, this.m_EllipsePen, new Point(actualWidth, y), this.m_dRadius, this.m_dRadius);
                                        if (flag)
                                        {
                                            this.m_DrawCont_Ticks.DrawText(this.m_TxtTickAmo, new Point(x, num4));
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                    if ((this.SendNextOrderAsClose && this.MouseMode) && (this.m_TxtSign != null))
                    {
                        this.m_DrawCont_Ticks.DrawEllipse(Brushes.Yellow, new Pen(Brushes.Black, 1.0), new Point(base.ActualWidth / 2.0, base.ActualHeight - 40.0), 10.0, 10.0);
                        this.m_DrawCont_Ticks.DrawText(this.m_TxtSign, new Point((base.ActualWidth / 2.0) - (this.m_TxtSign.Width / 2.0), (base.ActualHeight - 40.0) - (this.m_TxtSign.Height / 2.0)));
                    }
                    this.m_DrawCont_Ticks.Close();
                    if ((base.ActualWidth > 1.0) && (base.ActualHeight > 1.0))
                    {
                        this.m_Bmp_Ticks = new RenderTargetBitmap((int) base.ActualWidth, (int) base.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
                        this.m_Bmp_Ticks.Render(this.m_DrawVis_Ticks);
                        this.TickPanel_TicksImage.Source = this.m_Bmp_Ticks;
                    }
                    this.m_bTicksFilled = false;
                }
            }
        }

        private void OnPaint_Leaders()
        {
            if ((this.Leaders != null) && !this.BlockPaint)
            {
                this.m_DrawCont = this.m_DrawVis.RenderOpen();
                double openPrice = 0.0;
                double num2 = 0.0;
                double num3 = 0.0;
                double num4 = 0.0;
                double num5 = 2.0;
                double num6 = 10.0;
                double x = 0.0;
                double num8 = 0.0;
                int paintFocus = 0;
                int num1 = ((int) base.ActualHeight) / 2;
                lock (this.Leaders)
                {
                    if (((this.Leaders != null) && (this.LeadersDataSources != null)) && (this.Leaders.Count == this.LeadersDataSources.Count))
                    {
                        for (int i = 0; i < this.Leaders.Count; i++)
                        {
                            if (this.Leaders[i].Name == this.LeadersDataSources[i].Name)
                            {
                                if (((TickDataSourceContext) this.LeadersDataSources[i].SourceContext).OpenPriceUpdated)
                                {
                                    lock (this.Leaders)
                                    {
                                        foreach (Leader leader in this.Leaders)
                                        {
                                            if (leader.Name == this.Leaders[i].Name)
                                            {
                                                leader.OpenPrice = ((TickDataSourceContext) this.LeadersDataSources[i].SourceContext).OpenPrice;
                                                leader.OpenPriceUpdated = true;
                                            }
                                        }
                                    }
                                }
                                if (this.Leaders[i].OpenPriceUpdated && (this.Leaders[i].TickList.Count > 1))
                                {
                                    if (this.Leaders[i].PaintFocus < 1.0)
                                    {
                                        this.Leaders[i].PaintFocus = base.ActualHeight * 0.75;
                                    }
                                    openPrice = this.Leaders[i].OpenPrice;
                                    num2 = this.Leaders[i].TickList[this.Leaders[i].TickList.Count - 1];
                                    num3 = num2 - openPrice;
                                    num4 = Math.Round((double) ((num3 / num2) * 100.0), 2);
                                    num8 = num4;
                                    //KAA 2016-06-01
                                    this.m_TxtPercent = new FormattedText((num4 + "%").ToString(), this.m_CultureInfo, FlowDirection.LeftToRight, this.m_TypeFace, 
                                                                            /*this.m_dTxtFontSize*/this.FontSize, Brushes.White);
                                    double width = this.m_TxtPercent.Width;
                                    num2 = this.Leaders[i].TickList[this.Leaders[i].TickList.Count - 2];
                                    num3 = num2 - openPrice;
                                    num4 = Math.Round((double) ((num3 / num2) * 100.0), 2);
                                    if ((this.Leaders[i].TickList[this.Leaders[i].TickList.Count - 1] > this.Leaders[i].TickList[this.Leaders[i].TickList.Count - 2]) && (this.Leaders[i].TickList[this.Leaders[i].TickList.Count - 1] != this.Leaders[i].LastPrice))
                                    {
                                        Leader local1 = this.Leaders[i];
                                        local1.PaintFocus -= (Math.Abs((double) (num8 - num4)) / 0.01) * 10.0;
                                    }
                                    else if ((this.Leaders[i].TickList[this.Leaders[i].TickList.Count - 1] < this.Leaders[i].TickList[this.Leaders[i].TickList.Count - 2]) && (this.Leaders[i].TickList[this.Leaders[i].TickList.Count - 1] != this.Leaders[i].LastPrice))
                                    {
                                        Leader local2 = this.Leaders[i];
                                        local2.PaintFocus += (Math.Abs((double) (num8 - num4)) / 0.01) * 10.0;
                                    }
                                    this.Leaders[i].LastPrice = this.Leaders[i].TickList[this.Leaders[i].TickList.Count - 1];
                                    if (this.Leaders[i].PaintFocus < (base.ActualHeight * 0.2))
                                    {
                                        this.Leaders[i].PaintFocus = base.ActualHeight * 0.5;
                                    }
                                    else if (this.Leaders[i].PaintFocus > (base.ActualHeight * 0.8))
                                    {
                                        this.Leaders[i].PaintFocus = base.ActualHeight * 0.5;
                                    }
                                    x = base.ActualWidth - width;
                                    paintFocus = (int) this.Leaders[i].PaintFocus;
                                    this.m_DrawCont.DrawRectangle(this.m_lLeadersBrushes[i], this.m_lLeadersPens[i], new Rect(x, (double) paintFocus, width, (double) this.StringHeight));
                                    this.m_DrawCont.DrawText(this.m_TxtPercent, new Point(x, (double) paintFocus));
                                    Path path = new Path {
                                        Stroke = this.m_LeadersBrush
                                    };
                                    this.m_Path_Leaders = path;
                                    this.m_PGeometry_Leaders = new PathGeometry();
                                    PathFigure figure = new PathFigure {
                                        StartPoint = new Point(x, (double) paintFocus),
                                        IsClosed = false,
                                        IsFilled = false
                                    };
                                    this.m_PFigure_Leaders = figure;
                                    for (int j = this.Leaders[i].TickList.Count - 1; j > -1; j--)
                                    {
                                        num2 = this.Leaders[i].TickList[j];
                                        num3 = num2 - openPrice;
                                        num4 = Math.Round((double) ((num3 / num2) * 100.0), 2);
                                        x -= num5;
                                        paintFocus = (int) (this.Leaders[i].PaintFocus + (((num8 - num4) / 0.01) * num6));
                                        LineSegment segment = new LineSegment {
                                            Point = new Point(x, (double) paintFocus)
                                        };
                                        this.m_PFigure_Leaders.Segments.Add(segment);
                                        if (i > (this.m_lLeadersBrushes.Count - 1))
                                        {
                                            this.InitializeLeadersBrushes();
                                        }
                                    }
                                    this.m_PGeometry_Leaders.Figures.Add(this.m_PFigure_Leaders);
                                    this.m_Path_Leaders.Data = this.m_PGeometry_Leaders;
                                    if (i > (this.m_lLeadersBrushes.Count - 1))
                                    {
                                        this.InitializeLeadersBrushes();
                                    }
                                    else
                                    {
                                        this.m_DrawCont.DrawGeometry(this.m_lLeadersBrushes[i], this.m_lLeadersPens[i], this.m_PGeometry_Leaders);
                                    }
                                }
                            }
                        }
                    }
                }
                this.m_DrawCont.Close();
                if ((((int) base.ActualWidth) > 1) && (((int) base.ActualHeight) > 1))
                {
                    this.m_Bmp = new RenderTargetBitmap((int) base.ActualWidth, (int) base.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
                    this.m_Bmp.Render(this.m_DrawVis);
                    this.TickPanel_Image.Source = this.m_Bmp;
                }
            }
        }

        private void OnPaint_Levels()
        {
            if (!this.BlockPaint)
            {
                this.m_DrawCont = this.m_DrawVis.RenderOpen();
                if (this.TenLevels != null)
                {
                    lock (this.TenLevels)
                    {
                        foreach (double num in this.TenLevels)
                        {
                            this.m_DrawCont.DrawLine(this.m_LevelPen, new Point(0.0, num), new Point(base.ActualWidth, num));
                        }
                    }
                }
                if (this.FiftyLevels != null)
                {
                    lock (this.FiftyLevels)
                    {
                        foreach (double num2 in this.FiftyLevels)
                        {
                            this.m_DrawCont.DrawRectangle(this.m_LevelBrush, this.m_LevelPen, new Rect(0.0, num2, base.ActualWidth, 3.0));
                        }
                    }
                }
                this.m_DrawCont.Close();
                if ((base.ActualWidth > 1.0) && (base.ActualHeight > 1.0))
                {
                    this.m_Bmp = new RenderTargetBitmap((int) base.ActualWidth, (int) base.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
                    this.m_Bmp.Render(this.m_DrawVis);
                  //  this.TickPanel_LevelsImage.Source = this.m_Bmp;
                }
            }
        }

        private void SettingsChanged_ComboBox(object sender, SelectionChangedEventArgs e)
        {
            if ((sender != null) && (this.SettingsObject != null))
            {
                ComboBox box = (ComboBox) sender;
                int selectedIndex = box.SelectedIndex;
                if (box.Tag.ToString() == "Buttn_TicksStyle")
                {
                    switch (selectedIndex)
                    {
                        case 0:
                            this.m_sTicksStyle = "Dots";
                            this.m_bShowDots = true;
                            this.m_bShowLines = false;
                            break;

                        case 1:
                            this.m_sTicksStyle = "Lines";
                            this.m_bShowDots = false;
                            this.m_bShowLines = true;
                            break;

                        case 2:
                            this.m_sTicksStyle = "DotsLines";
                            this.m_bShowDots = true;
                            this.m_bShowLines = true;
                            break;
                    }
                    this.SettingsObject.TickPanel_Settings.TicksStyle = this.m_sTicksStyle;
                    this.m_bTicksFilled = true;
                }
            }
        }

        private void SettingsChanged_NumericUpDown(object sender, RoutedPropertyChangedEventArgs<object> o)
        {
            if (((sender != null) && (o.NewValue != null)) && (this.SettingsObject != null))
            {
                IntegerUpDown down = (IntegerUpDown) sender;
                int newValue = (int) o.NewValue;
                if (down.Tag.ToString() == "Buttn_ShowTicksFrom")
                {
                    this.m_iShowTicksFrom = newValue;
                    this.SettingsObject.TickPanel_Settings.ShowTicksFrom = newValue;
                }
                else if (down.Tag.ToString() == "Buttn_TicksWeight")
                {
                    this.m_iTicksWeight = newValue;
                    this.SettingsObject.TickPanel_Settings.TicksWeight = newValue;
                    this.m_LinesPen = new Pen(Brushes.Black, (double) this.m_iTicksWeight);
                    this.m_bTicksFilled = true;
                }
                else if (down.Tag.ToString() == "Buttn_RenewSpeed_Ticks")
                {
                    this.SettingsObject.TickPanel_Settings.RenewSpeed = newValue;
                    this.m_Timer_Ticks.Interval = 0x3e8 / newValue;
                    this.m_Timer_Ticks.Start();
                }
            }
        }

        public void SettingsSharing()
        {
            if (this.SettingsObject != null)
            {
                this.m_iFilterTicksFrom = this.SettingsObject.TickPanel_Settings.FilterTicksFrom;
                this.m_LinesBrush = new SolidColorBrush(this.SettingsObject.TickPanel_Settings.TicksColor);
                this.m_iShowTicksFrom = this.SettingsObject.TickPanel_Settings.ShowTicksFrom;
                this.m_iTicksWeight = this.SettingsObject.TickPanel_Settings.TicksWeight;
                this.m_LinesPen = new Pen(Brushes.Black, (double) this.m_iTicksWeight);
                this.m_sTicksStyle = this.SettingsObject.TickPanel_Settings.TicksStyle;
                if (this.m_sTicksStyle == "Dots")
                {
                    this.m_bShowDots = true;
                    this.m_bShowLines = false;
                }
                else if (this.m_sTicksStyle == "Lines")
                {
                    this.m_bShowDots = false;
                    this.m_bShowLines = true;
                }
                else if (this.m_sTicksStyle == "DotsLines")
                {
                    this.m_bShowDots = true;
                    this.m_bShowLines = true;
                }
                this.m_Timer_Ticks.Interval = 0x3e8 / this.SettingsObject.TickPanel_Settings.RenewSpeed;
                this.m_Timer_Ticks.Start();
                this.m_bSettingsShared = true;
            }
        }

        private void SettingsWinInit()
        {
            if (this.m_SettingsWin != null)
            {
                this.m_SettingsWin.Buttn_ShowTicksFrom.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
                this.m_SettingsWin.Buttn_TicksWeight.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
                this.m_SettingsWin.Buttn_TicksStyle.SelectionChanged += new SelectionChangedEventHandler(this.SettingsChanged_ComboBox);
                this.m_SettingsWin.Buttn_RenewSpeed_Ticks.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((TickPanel) target).MouseDown += new MouseButtonEventHandler(this.TickPanel_MouseDown);
                    return;

                case 2:
                    this.TickPanel_TicksImage = (Image) target;
                    return;

                case 3:
                    this.TickPanel_Canvas = (Canvas) target;
                    this.TickPanel_Canvas.SizeChanged += new SizeChangedEventHandler(this.TickPanel_SizeChanged);
                    this.TickPanel_Canvas.MouseDown += new MouseButtonEventHandler(this.TickPanel_MouseDown);
                    return;

                case 4:
                    this.TickPanel_Image = (Image) target;
                    return;

                case 5:
                   // this.TickPanel_LevelsImage = (Image) target;

           //_workAmountGrid = new ControlWorkAmount();
           



                    return;
            }
            this._contentLoaded = true;
        }

        private void TickPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
        //    System.Threading.Thread.Sleep(0);

        }

        private void TickPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!this.BlockPaint)
            {
                this.m_bTicksFilled = true;
            }
        }

        public bool BlockPaint { get; set; }

        public int Decimals { get; set; }

        public Dictionary<double, double> Dict_KPriceVCoordin
        {
            get
            {
                return this.m_Dict_KPriceVCoordin;
            }
            set
            {
                this.m_Dict_KPriceVCoordin = value;
                this.m_bPriceCoordinFilled = true;
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
                this.m_iFocusAsk = value;
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

        public List<Leader> Leaders
        {
            get
            {
                return (List<Leader>) base.GetValue(LeadersProperty);
            }
            set
            {
                base.SetValue(LeadersProperty, value);
            }
        }

        public List<GraphicDataExternalSource> LeadersDataSources
        {
            get
            {
                return (List<GraphicDataExternalSource>) base.GetValue(LeadersDataSourcesProperty);
            }
            set
            {
                base.SetValue(LeadersDataSourcesProperty, value);
            }
        }

        public List<TickGraphicData> LeadersTickSources
        {
            get
            {
                return (List<TickGraphicData>) base.GetValue(LeadersTickSourcesProperty);
            }
            set
            {
                base.SetValue(LeadersDataSourcesProperty, value);
            }
        }

        public bool MouseMode { get; set; }

        public int ScrollDelta
        {
            get
            {
                return this.m_iScrollDelta;
            }
            set
            {
                this.m_iScrollDelta = value;
                this.m_bTicksFilled = true;
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

        public double Step
        {
            get
            {

                return (double)base.GetValue(StepProperty);
            }

            set
            {

                base.SetValue(StepProperty, value);
            }
        }
        //public int StringHeight { get; set; }

        public List<double> TenLevels { get; set; }

        public Tick_Info Ticks
        {
            get
            {
                return this.m_Ticks;
            }
            set
            {
                this.m_Ticks = value;
                lock (this.m_lAllTicks)
                {
                    this.m_lAllTicks.Add(this.m_Ticks);
                    if (this.m_lAllTicks.Count > 1000)
                    {
                        this.m_lAllTicks.RemoveAt(0);
                    }
                }
                this.m_bTicksFilled = true;
            }
        }

        public bool TicksFilled
        {
            set
            {
                this.m_bTicksFilled = value;
            }
        }
    }
}

