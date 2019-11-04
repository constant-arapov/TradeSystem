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
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Timers;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;
    using Xceed.Wpf.Toolkit;

    public class ClusterPanel : UserControl, IComponentConnector
    {
        private bool _contentLoaded;
        public static readonly DependencyProperty ClusterDataSourceProperty = DependencyProperty.Register("ClusterDataSource", typeof(GraphicDataExternalSource), typeof(ClusterPanel));
        internal Canvas ClusterPanel_Canvas;
        internal Grid ClusterPanel_Grid;
        internal Image Image_0;
        internal Image Image_1;
        internal Image Image_2;
        internal Image Image_3;
        internal Image Image_4;
        internal Image Image_5;
        internal Image Image_6;
        internal Image Image_7;
        internal Image Image_8;
        internal Image Image_9;
        private bool m_bAddTimeframe;
        private bool m_bCanResize;
        private bool m_bHistoryLoaded;
        private RenderTargetBitmap m_Bmp;
        private bool m_bRepaintAll;
        private Brush m_Brush = Brushes.WhiteSmoke;
        private bool m_bSaveClusters = true;
        private bool m_bSettingsShared;
        private bool m_bTicksFilled;
        private DateTime m_Buffer;
        private SolidColorBrush m_ClusterBrush = new SolidColorBrush(Colors.WhiteSmoke);
        private TimeSpan m_ClusterTimeInterval = new TimeSpan(0, 1, 0);
        private CultureInfo m_CultureInfo = new CultureInfo("en-US");
        private Dictionary<DateTime, int> m_Dict_KDateVVolume_Buy = new Dictionary<DateTime, int>();
        private Dictionary<DateTime, int> m_Dict_KDateVVolume_Sell = new Dictionary<DateTime, int>();
        //<price,coordinate_Y>
        private Dictionary<double, double> m_Dict_KPriceVCoordin = new Dictionary<double, double>();
        private Dictionary<double, Dictionary<DateTime, int>> m_Dict_KPriceVDictTimeAmount = new Dictionary<double, Dictionary<DateTime, int>>();
        private Dictionary<double, Dictionary<DateTime, int>> m_Dict_KPriceVDictTimeAmount_Buy = new Dictionary<double, Dictionary<DateTime, int>>();
        private Dictionary<double, Dictionary<DateTime, int>> m_Dict_KPriceVDictTimeAmount_Sell = new Dictionary<double, Dictionary<DateTime, int>>();
        private double m_dLastMBV = -1.0;
        private DrawingContext m_DrawCont;
        private DrawingVisual m_DrawVis = new DrawingVisual();
       // private double m_dTxtFontSize = 10.0;
        private double m_dVolumeRectW = 2.0;
        private double m_dXOnClick;
        private FontWeight m_FontWeight = FontWeights.ExtraLight;
        private int m_iClusterFilledAt = 0x3e8;
        private int m_iDecimals;
        private int m_iPercents = 100;
        private int m_iScrollDelta;
        private int m_iTimeFrame = 5;
        private int m_iVolumeRectH = 13;
        //List of all ticks
        private List<Tick_Info> m_lAllTicks = new List<Tick_Info>();
        private List<DateTime> m_lClusterTimes = new List<DateTime>();
        private Brush m_LightCoralBrush = Brushes.LightCoral;
        private Brush m_LightGrayBrush = Brushes.LightGray;
        private Pen m_LightGrayPen = new Pen();
        private Brush m_LightGreenBrush = Brushes.LightGreen;
        //List of cluster images in fact stack structure
        private List<Image> m_lImages = new List<Image>();
        private List<double> m_lLastMBV = new List<double>();
        private Thickness m_Margin = new Thickness(0.0, 0.5, 0.0, 0.0);
        private Color m_MixedCol = Colors.WhiteSmoke;
        private Pen m_Pen = new Pen();
        private string m_sAssignedContractName = string.Empty;
        private string m_sClusterStyle_Color = "Mix";
        private string m_sClusterStyle_Text = "Summ";//KAA "Delta1";
        private SettingsWindow m_SettingsWin;
        private string m_sVertVolumeStyle_Text = "Delta2";
        private string m_sWorkingDirectory = string.Empty;
        private Tick_Info m_Ticks;
        private Timer m_Timer_Ticks;
        private FormattedText m_TxtCurrClustAmount;
        private Brush m_TxtDefaultBrush = Brushes.Black;
        private FontFamily m_TxtDefaultFontFam = new FontFamily("Verdana");
        private FontWeight m_TxtDefaultFontWeight = FontWeights.Normal;
        private Typeface m_TypeFace;


        public int StringHeight
        {
            get { return (int)GetValue(StringHeightProperty); }
            // set { SetValue(StringHeightProperty, value); }
        }



        // Using a DependencyProperty as the backing store for StringHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StringHeightProperty =
            DependencyProperty.Register("StringHeight", typeof(int), typeof(ClusterPanel), new UIPropertyMetadata(1));

        // == SHARED SETTINGS == Step 5.3 create DP in separate control

        public int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            //set { SetValue(FontSizetProperty, value); }
        }



        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(int), typeof(ClusterPanel), new UIPropertyMetadata(1));






        public ClusterPanel()
        {
            this.InitializeComponent();
            if (this.m_TypeFace == null)
            {
                this.m_TypeFace = new Typeface(this.m_TxtDefaultFontFam, FontStyles.Normal, this.m_FontWeight, new FontStretch());
            }
            this.m_LightGrayPen = new Pen(this.m_LightGrayBrush, 1.0);
            //KAA removed 2016-May-31
           // this.StringHeight = 13;
            this.m_lImages.Add(this.Image_0);
            this.m_lImages.Add(this.Image_1);
            this.m_lImages.Add(this.Image_2);
            this.m_lImages.Add(this.Image_3);
            this.m_lImages.Add(this.Image_4);
            this.m_lImages.Add(this.Image_5);
            this.m_lImages.Add(this.Image_6);
            this.m_lImages.Add(this.Image_7);
            this.m_lImages.Add(this.Image_8);
            this.m_lImages.Add(this.Image_9);
            for (int i = 0; i < this.m_lImages.Count; i++)
            {
                RenderOptions.SetBitmapScalingMode(this.m_lImages[i], BitmapScalingMode.NearestNeighbor);
                TextOptions.SetTextRenderingMode(this.m_lImages[i], TextRenderingMode.ClearType);
                TextOptions.SetTextFormattingMode(this.m_lImages[i], TextFormattingMode.Display);
            }
            RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
            this.m_Timer_Ticks = new Timer(90.0);
            this.m_Timer_Ticks.Elapsed += new ElapsedEventHandler(this.m_TicksTimer_Elapsed);
            this.m_Timer_Ticks.Start();
        }

        public void CanRepaintAll()
        {
            this.m_bRepaintAll = true;
            this.m_bTicksFilled = true;
        }

        private void ClusterPanel_Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.ClusterPanel_Grid.Height = this.ClusterPanel_Canvas.ActualHeight;
        }

        private void ClusterPanel_Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.m_bCanResize = true;
            this.m_dXOnClick = e.GetPosition(this.ClusterPanel_Grid).X;

        }

        private void ClusterPanel_Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            this.m_bCanResize = false;
            this.m_dXOnClick = 0.0;
            if (this.SettingsObject != null)
            {
                this.SettingsObject.ClusterPanel_Settings.GridWidth = this.ClusterPanel_Grid.ActualWidth;
            }
            this.m_bTicksFilled = true;
            this.m_bRepaintAll = true;
        }

        private void ClusterPanel_Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.m_bCanResize && ((this.ClusterPanel_Grid.Width + (this.m_dXOnClick - e.GetPosition(this.ClusterPanel_Grid).X)) >= 0.0))
            {
                this.ClusterPanel_Grid.Width += this.m_dXOnClick - e.GetPosition(this.ClusterPanel_Grid).X;
            }
            if (this.SettingsObject != null)
            {
                this.SettingsObject.ClusterPanel_Settings.GridWidth = this.ClusterPanel_Grid.Width;
            }
        }

        private void ClusterPanel_Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.m_bCanResize = false;
            this.m_dXOnClick = 0.0;
            this.m_bTicksFilled = true;
            this.m_bRepaintAll = true;
        }

        private SolidColorBrush CreateClusterBrush(double Price, int Amount, DateTime Time)
        {
            Dictionary<DateTime, int> dictionary;
            double num = 0.0;
            double num2 = 0.0;
            if (this.m_Dict_KPriceVDictTimeAmount_Buy.TryGetValue(Price, out dictionary) && dictionary.ContainsKey(Time))
            {
                num = (double) dictionary[Time];
            }
            if (this.m_Dict_KPriceVDictTimeAmount_Sell.TryGetValue(Price, out dictionary) && dictionary.ContainsKey(Time))
            {
                num2 = (double) dictionary[Time];
            }
            if (num > num2)
            {
                if (this.m_sClusterStyle_Color == "WhiteBalance")
                {
                    num2 = (200.0 * num2) / num;
                    this.m_MixedCol = Color.FromArgb(0x73, Convert.ToByte(num2), 200, Convert.ToByte(num2));
                }
                else if (this.m_sClusterStyle_Color == "BlackBalance")
                {
                    num2 = (200.0 * num2) / num;
                    this.m_MixedCol = Color.FromArgb(100, 0, Convert.ToByte((double) (200.0 - num2)), 0);
                }
                else if (this.m_sClusterStyle_Color == "Mix")
                {
                    num2 = (200.0 * num2) / num;
                    this.m_MixedCol = Color.FromArgb(100, Convert.ToByte(num2), 200, 0);
                }
            }
            else if (num2 >= num)
            {
                if (this.m_sClusterStyle_Color == "WhiteBalance")
                {
                    num = (200.0 * num) / num2;
                    this.m_MixedCol = Color.FromArgb(0x73, 200, Convert.ToByte(num), Convert.ToByte(num));
                }
                else if (this.m_sClusterStyle_Color == "BlackBalance")
                {
                    num = (200.0 * num) / num2;
                    this.m_MixedCol = Color.FromArgb(100, Convert.ToByte((double) (200.0 - num)), 0, 0);
                }
                else if (this.m_sClusterStyle_Color == "Mix")
                {
                    num = (200.0 * num) / num2;
                    this.m_MixedCol = Color.FromArgb(100, 200, Convert.ToByte(num), 0);
                }
            }
            this.m_ClusterBrush = new SolidColorBrush(this.m_MixedCol);
            return this.m_ClusterBrush;
        }

        private TextBlock CreateTextBlock()
        {
            //KAA 2016-06-01
            TextBlock element = new TextBlock {
                FontSize = (double)this.FontSize, //this.m_dTxtFontSize,
                Foreground = this.m_TxtDefaultBrush,
                Margin = this.m_Margin,
                FontFamily = this.m_TxtDefaultFontFam,
                FontWeight = this.m_TxtDefaultFontWeight,
                TextAlignment = TextAlignment.Left
            };
            TextOptions.SetTextFormattingMode(element, TextFormattingMode.Display);
            TextOptions.SetTextRenderingMode(element, TextRenderingMode.Aliased);
            return element;
        }

        /// <summary>
        /// 
        /// </summary>
        ///                             dictionaries <DateTime, int>
        /// <param name="Dictionary">this.m_Dict_KDateVVolume_Buy or 
        ///                             m_Dict_KDateVVolume_Buy sell</param>
        /// <param name="Time">this.m_lClusterTimes[0])</param>
        /// <param name="Amount">this.m_Ticks.Amount</param>
        private void DictionaryManipulation(Dictionary<DateTime, int> Dictionary, DateTime Time, int Amount)
        {
            
            if (Dictionary.ContainsKey(Time))
            {
                Dictionary<DateTime, int> dictionary;
                DateTime time;
                (dictionary = Dictionary)[time = Time] = dictionary[time] + Amount;
            }
            else
            {
                Dictionary.Add(Time, Amount);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Dictionary"> < price <time,amount>
        ///                           dictionaies:
        ///                          m_Dict_KPriceVDictTimeAmount
        ///                          or m_Dict_KPriceVDictTimeAmount_Buy
        ///                          or m_Dict_KPriceVDictTimeAmount_Sell</param>
        /// <param name="Price">this.m_Ticks.Price</param>
        /// <param name="Amount">this.m_Ticks.Amount</param>
        /// <param name="Time">m_lClusterTimes[0]</param>
        private void DictionaryManipulation(Dictionary<double, Dictionary<DateTime, int>> Dictionary, double Price, int Amount, DateTime Time)
        {
            Dictionary<DateTime, int> dictionary;
            //Try get array Dictionary<DateTime, int> by price
            //if already exist cluster with this price
            if (Dictionary.TryGetValue(Price, out dictionary))
            {
                int amount;
                //get current amount and increase it
                if (dictionary.TryGetValue(Time, out amount))
                {
                    Dictionary<DateTime, int> dictionary2;
                    DateTime time;
                    (dictionary2 = dictionary)[time = Time] = dictionary2[time] + Amount;
                }
                else
                {
                    dictionary.Add(Time, Amount);
                }
            }
            //if cluster of this price not exist
            else
            {
               //add it
                dictionary = new Dictionary<DateTime, int>();
                dictionary.Add(Time, Amount);
                Dictionary.Add(Price, dictionary);
            }
        }


        /// <summary>
        /// Initialise or add cluster timers list
        /// Call when no intervals(on staert) and when we need 
        /// to add new one (on time interval eclapses)
        /// </summary>
        public void InitClusterTimes()
        {
            //call when no intervals and when we add new one
            if ((this.m_lClusterTimes.Count == 0) || this.m_bAddTimeframe)
            {
                this.m_bAddTimeframe = false;//signal off interval added
                this.m_lClusterTimes.Clear();
                //remove all textblocks with times
                for (int i = 0; i < this.ClusterPanel_Grid.Children.Count; i++)
                {
                    //in grid child we interested only in Textblocks
                    if (this.ClusterPanel_Grid.Children[i] is TextBlock)
                    {
                        this.ClusterPanel_Grid.Children.Remove(this.ClusterPanel_Grid.Children[i]);
                    }
                }
               
                //find latest en time of time interval less than current time
                DateTime item = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                while (true)
                {
                    this.m_Buffer = item.Add(this.m_ClusterTimeInterval);
                    //if m_buffer later or the same than  DateTime.Now
                    if (DateTime.Compare(this.m_Buffer, DateTime.Now) >= 0)
                    {
                        break;
                    }
                    item = this.m_Buffer;
                }
                //Add to cluster times list
                this.m_lClusterTimes.Add(item);
                //Create textblocks with times
                for (int j = this.ClusterPanel_Grid.ColumnDefinitions.Count - 1; j > 0; j--)
                {
                    if (j == (this.ClusterPanel_Grid.ColumnDefinitions.Count - 1))
                    {
                        this.m_Buffer = this.m_lClusterTimes[0];
                    }
                    else
                    {
                        this.m_Buffer = this.m_Buffer.Subtract(this.m_ClusterTimeInterval);
                        this.m_lClusterTimes.Add(this.m_Buffer);
                    }
                    TextBlock element = this.CreateTextBlock();
                    element.Text = this.m_Buffer.ToString("HH:mm");
                    element.Background = Brushes.LightGray;
                    element.TextAlignment = TextAlignment.Center;
                    this.ClusterPanel_Grid.Children.Add(element);
                    Grid.SetRow(element, 1);
                    Grid.SetColumn(element, j);
                    j--;
                }
                 
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/Visualizer;component/clusterpanel.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void m_TicksTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action method = new Action(this.OnPaint);
            base.Dispatcher.Invoke(DispatcherPriority.Render, method);
        }

        public void OnPaint()
        {
            //KAA 
            this.m_bSettingsShared = true;
            //this.m_bHistoryLoaded = true;

            if ((this.m_bSettingsShared && !this.BlockPaint) && this.m_bHistoryLoaded)
            {
                //get time from top of stack
                DateTime time = this.m_lClusterTimes[0];
                //if current time more than time+time_int
                //shift stack on the top of stack put new image
                if (DateTime.Now > time.Add(this.m_ClusterTimeInterval))
                {
                    for (int i = this.m_lImages.Count - 1; i > 0; i--)
                    {
                        this.m_lImages[i].Source = this.m_lImages[i - 1].Source;
                        if (i == 1)
                        {
                            Image image = new Image();
                            this.m_lImages[0].Source = image.Source;
                        }
                    }
                    this.m_bAddTimeframe = true; //signal interval added
                }
                //add times
                this.InitClusterTimes();
                if ((this.m_lClusterTimes.Count == 10) && this.m_bTicksFilled)
                {
                    double actualWidth = this.ClusterPanel_Grid.ColumnDefinitions[1].ActualWidth;
                    int numOfIntervals = 1;
                    if (this.m_bRepaintAll)
                    {
                        numOfIntervals = 10;
                        this.m_bRepaintAll = false;
                    }
                    for (int jTmInerval = 0; jTmInerval < numOfIntervals; jTmInerval++)
                    {
                        if ((base.ActualWidth - (actualWidth * jTmInerval)) >= 0.0)
                        {
                            this.m_DrawCont = this.m_DrawVis.RenderOpen();
                            Dictionary<DateTime, int> dictionary = null;
                            Dictionary<DateTime, int> dictionary2 = null;
                            Dictionary<DateTime, int> dictionary3 = null;
                            int amount = 0;
                            int num6 = 0;
                            int num7 = 0;
                            string stAmountOfPrice = "";
                            double num8 = -1.0;
                            double y = -1.0;
                            double key = -1.0;
                            //foreach in <price,coord_Y>
                            foreach (KeyValuePair<double, double> pair in this.m_Dict_KPriceVCoordin)
                            {
                                bool bAmountOfPriceWasFound = false;
                                //usualy use only this type of clustering algorithm
                                if (this.m_sClusterStyle_Text == "Summ")
                                {
                                    dictionary = null;

                                    //get dict <time, amount> for specific price
                                    //from L2 dict  <price, <time, amount> 
                                    this.m_Dict_KPriceVDictTimeAmount.TryGetValue(pair.Key, out dictionary);
                                  
                                    if ((dictionary != null) && dictionary.ContainsKey(this.m_lClusterTimes[jTmInerval]))
                                    {
                                        //get amount for timespan from <time,amount>
                                        amount = dictionary[this.m_lClusterTimes[jTmInerval]];
                                        bAmountOfPriceWasFound  = true;
                                        stAmountOfPrice = amount.ToString();
                                    }
                                }
                                else if ((this.m_sClusterStyle_Text == "Delta1") || (this.m_sClusterStyle_Text == "Delta2"))
                                {
                                    dictionary2 = null;
                                    dictionary3 = null;
                                    amount = 0;
                                    num6 = 0;
                                    num7 = 0;
                                    this.m_Dict_KPriceVDictTimeAmount_Buy.TryGetValue(pair.Key, out dictionary2);
                                    this.m_Dict_KPriceVDictTimeAmount_Sell.TryGetValue(pair.Key, out dictionary3);
                                    if ((dictionary2 != null) && dictionary2.ContainsKey(this.m_lClusterTimes[jTmInerval]))
                                    {
                                        num6 = dictionary2[this.m_lClusterTimes[jTmInerval]];
                                        amount += num6;
                                        bAmountOfPriceWasFound  = true;
                                    }
                                    if ((dictionary3 != null) && dictionary3.ContainsKey(this.m_lClusterTimes[jTmInerval]))
                                    {
                                        num7 = dictionary3[this.m_lClusterTimes[jTmInerval]];
                                        amount += num7;
                                        bAmountOfPriceWasFound  = true;
                                    }
                                    if (this.m_sClusterStyle_Text == "Delta1")
                                    {
                                        stAmountOfPrice = (num6 - num7).ToString();
                                    }
                                    else if (this.m_sClusterStyle_Text == "Delta2")
                                    {
                                        stAmountOfPrice = num7.ToString() + " x " + num6.ToString();
                                    }
                                }
                                if (bAmountOfPriceWasFound)
                                {
                                    //if found draw Rectangle with text amount
                                    bAmountOfPriceWasFound  = false;
                                    this.m_TxtCurrClustAmount = new FormattedText(" " + stAmountOfPrice, this.m_CultureInfo, FlowDirection.LeftToRight, this.m_TypeFace, /*this.m_dTxtFontSize*/this.FontSize, Brushes.Black);
                                    this.m_Brush = this.CreateClusterBrush(pair.Key, amount, this.m_lClusterTimes[jTmInerval]);
                                    this.m_Pen = new Pen(this.m_Brush, 1.0);
                                    this.m_dVolumeRectW = (((double) amount) / ((double) this.m_iClusterFilledAt)) * actualWidth;
                                    this.m_DrawCont.DrawRectangle(this.m_Brush, this.m_Pen, new Rect(0.0, pair.Value, this.m_dVolumeRectW, (double) this.StringHeight));
                                    this.m_DrawCont.DrawText(this.m_TxtCurrClustAmount, new Point(0.0, pair.Value));
                                    if (num8 < amount)
                                    {
                                        num8 = amount;
                                        y = pair.Value;
                                        key = pair.Key;
                                    }
                                }
                            }
                            if ((num8 > 0.0) && (actualWidth >= 1.0))
                            {
                                this.m_DrawCont.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Black, 1.0), new Rect(0.0, y, actualWidth - 1.0, (double) this.StringHeight));
                            }
                            if ((key != this.m_dLastMBV) && (jTmInerval == 0))
                            {
                                this.m_dLastMBV = key;
                                this.m_lLastMBV.Add(this.m_dLastMBV);
                            }
                            int totalAmountOfTimeSpanBuy = 0;
                            int totalAmountOfTimeSpanSell = 0;
                            if (jTmInerval < m_lClusterTimes.Count)
                            {
                                m_Dict_KDateVVolume_Buy.TryGetValue(m_lClusterTimes[jTmInerval], out totalAmountOfTimeSpanBuy);
                                m_Dict_KDateVVolume_Sell.TryGetValue(m_lClusterTimes[jTmInerval], out totalAmountOfTimeSpanSell);
                                if (m_sVertVolumeStyle_Text == "Summ")
                                {   //add buy and sell timespan for cluster values 
                                    int totalAmountOfTimeSpan = totalAmountOfTimeSpanBuy + totalAmountOfTimeSpanSell;
                                    //KAA 2016-06-01
                                    m_TxtCurrClustAmount = new FormattedText(" " + totalAmountOfTimeSpan.ToString(), m_CultureInfo, FlowDirection.LeftToRight, m_TypeFace, /*this.m_dTxtFontSize,*/this.FontSize, Brushes.Black);
                                }
                                else if (this.m_sVertVolumeStyle_Text == "Delta1")
                                {
                                    int num15 = totalAmountOfTimeSpanBuy - totalAmountOfTimeSpanSell;
                                    //KAA 2016-06-01
                                    this.m_TxtCurrClustAmount = new FormattedText(" " + num15.ToString(), this.m_CultureInfo, FlowDirection.LeftToRight, this.m_TypeFace, /*this.m_dTxtFontSize,*/this.FontSize, Brushes.Black);
                                }
                                else if (this.m_sVertVolumeStyle_Text == "Delta2")
                                {
                                    //KAA 2016-06-01
                                    this.m_TxtCurrClustAmount = new FormattedText(" " + totalAmountOfTimeSpanSell.ToString() + " x " + totalAmountOfTimeSpanBuy.ToString(), this.m_CultureInfo, FlowDirection.LeftToRight, this.m_TypeFace, /*this.m_dTxtFontSize,*/this.FontSize, Brushes.Black);
                                }
                                else
                                {
                                    return;
                                }
                                if (totalAmountOfTimeSpanBuy > totalAmountOfTimeSpanSell)
                                {
                                    this.m_DrawCont.DrawRectangle(this.m_LightGreenBrush, this.m_LightGrayPen, new Rect(0.0, base.ActualHeight - 27.0, actualWidth, (double) this.StringHeight));
                                }
                                else
                                {
                                    this.m_DrawCont.DrawRectangle(this.m_LightCoralBrush, this.m_LightGrayPen, new Rect(0.0, base.ActualHeight - 27.0, actualWidth, (double) this.StringHeight));
                                }
                                this.m_DrawCont.DrawText(this.m_TxtCurrClustAmount, new Point(0.0, base.ActualHeight - 27.0));
                            }
                            this.m_DrawCont.Close();
                            if (((base.ActualWidth > 1.0) && (base.ActualHeight > 1.0)) && (actualWidth > 1.0))
                            {
                                this.m_Bmp = new RenderTargetBitmap((int) actualWidth, (int) base.ActualHeight, 96.0, 96.0, PixelFormats.Default);
                                this.m_Bmp.Render(this.m_DrawVis);
                                this.m_lImages[jTmInerval].Source = this.m_Bmp;
                            }
                        }
                    }
                    this.m_bTicksFilled = false;
                }
            }
        }

        private List<Tick_Info> ReadClustersHistory()
        {
            List<Tick_Info> list = null;
            if (this.m_sWorkingDirectory != string.Empty)
            {
                string path = this.m_sWorkingDirectory + @"\Data\ComplexTomahawk\TicksHistory\" + this.m_sAssignedContractName.ToString() + "_" + DateTime.Now.DayOfYear.ToString() + ".dat";
                string[] strArray2 = new string[] { this.m_sWorkingDirectory, @"\Data\ComplexTomahawk\TicksHistory\", this.m_sAssignedContractName.ToString(), "_", (DateTime.Now.DayOfYear - 1).ToString(), ".dat" };
                string str2 = string.Concat(strArray2);
                if (File.Exists(str2))
                {
                    File.Delete(str2);
                }
                FileStream serializationStream = null;
                try
                {
                    if (File.Exists(path))
                    {
                        serializationStream = new FileStream(path, FileMode.Open);
                    }
                    else
                    {
                        File.Create(path);
                    }
                }
                catch (IOException)
                {
                }
                if (serializationStream == null)
                {
                    return new List<Tick_Info>();
                }
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    list = (List<Tick_Info>) formatter.Deserialize(serializationStream);
                }
                catch (SerializationException)
                {
                }
                finally
                {
                    serializationStream.Close();
                }
                if (list != null)
                {
                    return list;
                }
            }
            return new List<Tick_Info>();
        }

        private void RebuildClusters()
        {
            if (this.ClusterDataSource != null)
            {
                this.m_Dict_KPriceVDictTimeAmount.Clear();
                this.m_Dict_KPriceVDictTimeAmount_Buy.Clear();
                this.m_Dict_KPriceVDictTimeAmount_Sell.Clear();
                this.m_Dict_KDateVVolume_Buy.Clear();
                this.m_Dict_KDateVVolume_Sell.Clear();
                List<Tick_Info> list = null;
                list = this.ReadClustersHistory();
                if (list != null)
                {
                    if (list != null)
                    {
                        this.m_lAllTicks = list;
                    }
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        for (int j = 0; j < this.m_lClusterTimes.Count; j++)
                        {
                            if (((j != 0) && (list[i].DateTime >= this.m_lClusterTimes[j])) && (list[i].DateTime < this.m_lClusterTimes[j - 1]))
                            {
                                this.DictionaryManipulation(this.m_Dict_KPriceVDictTimeAmount, list[i].Price, list[i].Amount, this.m_lClusterTimes[j]);
                                if (list[i].Direction == TickDirection.Buy)
                                {
                                    this.DictionaryManipulation(this.m_Dict_KPriceVDictTimeAmount_Buy, list[i].Price, list[i].Amount, this.m_lClusterTimes[j]);
                                    this.DictionaryManipulation(this.m_Dict_KDateVVolume_Buy, this.m_lClusterTimes[j], list[i].Amount);
                                }
                                if (list[i].Direction == TickDirection.Sell)
                                {
                                    this.DictionaryManipulation(this.m_Dict_KPriceVDictTimeAmount_Sell, list[i].Price, list[i].Amount, this.m_lClusterTimes[j]);
                                    this.DictionaryManipulation(this.m_Dict_KDateVVolume_Sell, this.m_lClusterTimes[j], list[i].Amount);
                                }
                            }
                            else if ((j == 0) && (list[i].DateTime >= this.m_lClusterTimes[0]))
                            {
                                this.DictionaryManipulation(this.m_Dict_KPriceVDictTimeAmount, list[i].Price, list[i].Amount, this.m_lClusterTimes[0]);
                                if (list[i].Direction == TickDirection.Buy)
                                {
                                    this.DictionaryManipulation(this.m_Dict_KPriceVDictTimeAmount_Buy, list[i].Price, list[i].Amount, this.m_lClusterTimes[0]);
                                    this.DictionaryManipulation(this.m_Dict_KDateVVolume_Buy, this.m_lClusterTimes[0], list[i].Amount);
                                }
                                if (list[i].Direction == TickDirection.Sell)
                                {
                                    this.DictionaryManipulation(this.m_Dict_KPriceVDictTimeAmount_Sell, list[i].Price, list[i].Amount, this.m_lClusterTimes[0]);
                                    this.DictionaryManipulation(this.m_Dict_KDateVVolume_Sell, this.m_lClusterTimes[0], list[i].Amount);
                                }
                            }
                        }
                    }
                    this.m_bRepaintAll = true;
                    this.m_bTicksFilled = true;
                }
            }
        }

        public void SaveClusters_OuterCommand()
        {
            this.SaveClustersHistory(this.m_lAllTicks);
        }

        public void SaveClustersHistory(List<Tick_Info> L)
        {
            if ((L.Count != 0) && this.m_bSaveClusters)
            {
                List<Tick_Info> graph = new List<Tick_Info>();
                Tick_Info item = new Tick_Info();
                Dictionary<double, Dictionary<DateTime, int>> dictionary = new Dictionary<double, Dictionary<DateTime, int>>();
                Dictionary<double, Dictionary<DateTime, int>> dictionary2 = new Dictionary<double, Dictionary<DateTime, int>>();
                for (int i = 0; i < L.Count; i++)
                {
                    DateTime time = L[i].DateTime.Date.AddHours((double) L[i].DateTime.Hour).AddMinutes((double) L[i].DateTime.Minute);
                    if (L[i].Direction == TickDirection.Buy)
                    {
                        this.DictionaryManipulation(dictionary, L[i].Price, L[i].Amount, time);
                    }
                    else if (L[i].Direction == TickDirection.Sell)
                    {
                        this.DictionaryManipulation(dictionary2, L[i].Price, L[i].Amount, time);
                    }
                }
                foreach (KeyValuePair<double, Dictionary<DateTime, int>> pair in dictionary)
                {
                    foreach (KeyValuePair<DateTime, int> pair2 in pair.Value)
                    {
                        item = new Tick_Info {
                            Price = pair.Key,
                            DateTime = pair2.Key,
                            Direction = TickDirection.Buy,
                            Amount = pair2.Value,
                            Point = new Point(0.0, 0.0)
                        };
                        graph.Add(item);
                    }
                }
                foreach (KeyValuePair<double, Dictionary<DateTime, int>> pair3 in dictionary2)
                {
                    foreach (KeyValuePair<DateTime, int> pair4 in pair3.Value)
                    {
                        item = new Tick_Info {
                            Price = pair3.Key,
                            DateTime = pair4.Key,
                            Direction = TickDirection.Sell,
                            Amount = pair4.Value,
                            Point = new Point(0.0, 0.0)
                        };
                        graph.Add(item);
                    }
                }
                if (this.m_sWorkingDirectory != string.Empty)
                {
                    string path = this.m_sWorkingDirectory + @"\Data\ComplexTomahawk\TicksHistory\" + this.m_sAssignedContractName.ToString() + "_" + DateTime.Now.DayOfYear.ToString() + ".dat";
                    FileStream serializationStream = null;
                    try
                    {
                        if (File.Exists(path))
                        {
                            serializationStream = new FileStream(path, FileMode.Create);
                        }
                        else
                        {
                            File.Create(path);
                        }
                    }
                    catch (IOException)
                    {
                    }
                    if (serializationStream != null)
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        try
                        {
                            formatter.Serialize(serializationStream, graph);
                        }
                        catch (SerializationException)
                        {
                        }
                        finally
                        {
                            serializationStream.Close();
                        }
                    }
                }
            }
        }

        private void SettingsChanged_Button(object sender, RoutedEventArgs e)
        {
            if ((sender != null) && (this.SettingsObject != null))
            {
                Button button = (Button) sender;
                if (button.Tag.ToString() == "Buttn_SaveClusters")
                {
                    this.m_bSaveClusters = button.Content.ToString() != "Да";
                    button.Content = (button.Content.ToString() == "Да") ? "Нет" : "Да";
                    this.SettingsObject.ClusterPanel_Settings.SaveClusters = this.m_bSaveClusters;
                }
            }
        }

        private void SettingsChanged_ComboBox(object sender, SelectionChangedEventArgs s)
        {
            if ((sender != null) && (this.SettingsObject != null))
            {
                ComboBox box = (ComboBox) sender;
                int selectedIndex = box.SelectedIndex;
                if (!(box.Tag.ToString() == "Buttn_ClusterStyleText"))
                {
                    if (!(box.Tag.ToString() == "Buttn_VertVolumeStyle"))
                    {
                        if (!(box.Tag.ToString() == "Buttn_ClusterStyleColor"))
                        {
                            if (box.Tag.ToString() == "Buttn_ClustersTF")
                            {
                                this.m_bHistoryLoaded = false;
                                switch (selectedIndex)
                                {
                                    case 0:
                                        this.m_iTimeFrame = 1;
                                        break;

                                    case 1:
                                        this.m_iTimeFrame = 5;
                                        break;

                                    case 2:
                                        this.m_iTimeFrame = 10;
                                        break;

                                    case 3:
                                        this.m_iTimeFrame = 15;
                                        break;

                                    case 4:
                                        this.m_iTimeFrame = 30;
                                        break;

                                    case 5:
                                        this.m_iTimeFrame = 60;
                                        break;

                                    case 6:
                                        this.m_iTimeFrame = 0x5a0;
                                        break;
                                }
                                this.SettingsObject.ClusterPanel_Settings.TimeFrame = this.m_iTimeFrame;
                                this.SaveClustersHistory(this.m_lAllTicks);
                                this.m_ClusterTimeInterval = new TimeSpan(0, this.m_iTimeFrame, 0);
                                this.m_lClusterTimes.Clear();
                                this.InitClusterTimes();
                                this.RebuildClusters();
                                this.m_bHistoryLoaded = true;
                            }
                        }
                        else
                        {
                            switch (selectedIndex)
                            {
                                case 0:
                                    this.m_sClusterStyle_Color = "WhiteBalance";
                                    break;

                                case 1:
                                    this.m_sClusterStyle_Color = "BlackBalance";
                                    break;

                                case 2:
                                    this.m_sClusterStyle_Color = "Mix";
                                    break;
                            }
                            this.SettingsObject.ClusterPanel_Settings.ClusterStyleColor = this.m_sClusterStyle_Color;
                            this.m_bTicksFilled = true;
                            this.m_bRepaintAll = true;
                        }
                    }
                    else
                    {
                        switch (selectedIndex)
                        {
                            case 0:
                                this.m_sVertVolumeStyle_Text = "Summ";
                                break;

                            case 1:
                                this.m_sVertVolumeStyle_Text = "Delta1";
                                break;

                            case 2:
                                this.m_sVertVolumeStyle_Text = "Delta2";
                                break;
                        }

              


                        this.SettingsObject.ClusterPanel_Settings.VolumeStyleText = this.m_sVertVolumeStyle_Text;
                        this.m_bTicksFilled = true;
                        this.m_bRepaintAll = true;
                    }
                }
                else
                {
                    switch (selectedIndex)
                    {
                        case 0:
                            this.m_sClusterStyle_Text = "Summ";
                            break;

                        case 1:
                            this.m_sClusterStyle_Text = "Delta1";
                            break;

                        case 2:
                            this.m_sClusterStyle_Text = "Delta2";
                            break;
                    }
                    this.SettingsObject.ClusterPanel_Settings.ClusterStyleText = this.m_sClusterStyle_Text;
                    this.m_bTicksFilled = true;
                    this.m_bRepaintAll = true;
                }
            }
        }

        private void SettingsChanged_NumericUpDown(object sender, RoutedPropertyChangedEventArgs<object> o)
        {
            if (((sender != null) && (o.NewValue != null)) && (this.SettingsObject != null))
            {
                IntegerUpDown down = (IntegerUpDown) sender;
                int newValue = (int) o.NewValue;
                if (down.Tag.ToString() == "Buttn_ClustersFilledAt")
                {
                    if ((newValue >= 1) && (newValue <= 0x3b9ac9ff))
                    {
                        this.m_iClusterFilledAt = newValue;
                        this.SettingsObject.ClusterPanel_Settings.FilledAt = newValue;
                        this.m_bTicksFilled = true;
                        this.m_bRepaintAll = true;
                    }
                    else
                    {
                        down.Text = "1";
                    }
                }
                else if (down.Tag.ToString() == "Buttn_PercentsForColorGradient")
                {
                    if ((newValue >= 1) && (newValue <= 100))
                    {
                        this.m_iPercents = newValue;
                        this.SettingsObject.ClusterPanel_Settings.PercentsForColorGradient = newValue;
                    }
                    else
                    {
                        down.Text = "1";
                    }
                }
            }
        }
        // Load settings for cluster panel here
        public void SettingsSharing()
        {
            this.m_bHistoryLoaded = false;
            this.m_bSettingsShared = false;
            this.m_iClusterFilledAt = this.SettingsObject.ClusterPanel_Settings.FilledAt;
            this.m_iPercents = this.SettingsObject.ClusterPanel_Settings.PercentsForColorGradient;
            this.m_bSaveClusters = this.SettingsObject.ClusterPanel_Settings.SaveClusters;
            this.m_sClusterStyle_Text = this.SettingsObject.ClusterPanel_Settings.ClusterStyleText;
            this.m_sClusterStyle_Color = this.SettingsObject.ClusterPanel_Settings.ClusterStyleColor;
            this.m_sVertVolumeStyle_Text = this.SettingsObject.ClusterPanel_Settings.VolumeStyleText;
            this.ClusterPanel_Grid.Width = this.SettingsObject.ClusterPanel_Settings.GridWidth;
            this.ClusterPanel_Grid.Height = this.ClusterPanel_Canvas.ActualHeight;
            //time interval of cluster in minutes
            this.m_iTimeFrame = this.SettingsObject.ClusterPanel_Settings.TimeFrame;
            //timespan (time interval) of cluster
            this.m_ClusterTimeInterval = new TimeSpan(0, this.m_iTimeFrame, 0);
            this.m_lClusterTimes.Clear();
            this.InitClusterTimes();
            this.RebuildClusters();
            this.m_bHistoryLoaded = true;
            this.m_bSettingsShared = true;
        }

        private void SettingsWinInit()
        {
            if (this.m_SettingsWin != null)
            {
                this.m_SettingsWin.Buttn_ClustersFilledAt.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
                this.m_SettingsWin.Buttn_PercentsForColorGradient.ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.SettingsChanged_NumericUpDown);
                this.m_SettingsWin.Buttn_ClustersTF.SelectionChanged += new SelectionChangedEventHandler(this.SettingsChanged_ComboBox);
                this.m_SettingsWin.Buttn_SaveClusters.Click += new RoutedEventHandler(this.SettingsChanged_Button);
                this.m_SettingsWin.Buttn_ClusterStyleText.SelectionChanged += new SelectionChangedEventHandler(this.SettingsChanged_ComboBox);
                this.m_SettingsWin.Buttn_ClusterStyleColor.SelectionChanged += new SelectionChangedEventHandler(this.SettingsChanged_ComboBox);
                this.m_SettingsWin.Buttn_VertVolumeStyle.SelectionChanged += new SelectionChangedEventHandler(this.SettingsChanged_ComboBox);
            }
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.ClusterPanel_Canvas = (Canvas) target;
                    this.ClusterPanel_Canvas.SizeChanged += new SizeChangedEventHandler(this.ClusterPanel_Canvas_SizeChanged);
                    this.ClusterPanel_Canvas.MouseLeave += new MouseEventHandler(this.ClusterPanel_Grid_MouseLeave);
                    this.ClusterPanel_Canvas.MouseDown += new MouseButtonEventHandler(this.ClusterPanel_Grid_MouseDown);
                    this.ClusterPanel_Canvas.MouseUp += new MouseButtonEventHandler(this.ClusterPanel_Grid_MouseUp);
                    this.ClusterPanel_Canvas.MouseMove += new MouseEventHandler(this.ClusterPanel_Grid_MouseMove);
                    return;

                case 2:
                    this.ClusterPanel_Grid = (Grid) target;
                    return;

                case 3:
                    this.Image_0 = (Image) target;
                    return;

                case 4:
                    this.Image_1 = (Image) target;
                    return;

                case 5:
                    this.Image_2 = (Image) target;
                    return;

                case 6:
                    this.Image_3 = (Image) target;
                    return;

                case 7:
                    this.Image_4 = (Image) target;
                    return;

                case 8:
                    this.Image_5 = (Image) target;
                    return;

                case 9:
                    this.Image_6 = (Image) target;
                    return;

                case 10:
                    this.Image_7 = (Image) target;
                    return;

                case 11:
                    this.Image_8 = (Image) target;
                    return;

                case 12:
                    this.Image_9 = (Image) target;
                    return;
            }
            this._contentLoaded = true;
        }

        public List<Tick_Info> AllTicks
        {
            get
            {
                return this.m_lAllTicks;
            }
        }

        public string AssignedContractName
        {
            get
            {
                return this.m_sAssignedContractName;
            }
            set
            {
                this.SaveClustersHistory(this.m_lAllTicks);
                this.m_sAssignedContractName = value;
            }
        }

        public bool BlockPaint { get; set; }

        public GraphicDataExternalSource ClusterDataSource
        {
            get
            {
                return (GraphicDataExternalSource) base.GetValue(ClusterDataSourceProperty);
            }
            set
            {
                base.SetValue(ClusterDataSourceProperty, value);
            }
        }

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
            }
        }

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
                this.m_bRepaintAll = true;
            }
        }

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

       
        
        /// <summary>
        /// Ticks == Deals
        /// When recieve new tick, 
        /// update cluster dictionaries
        /// 
        /// </summary>
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
                }
                if (((m_Ticks.Price != 0.0) && (m_Ticks.Amount != 0)) && (m_lClusterTimes.Count >= 1))
                {
                    //Update Total amount of all time, all prices, all directions
                    DictionaryManipulation(m_Dict_KPriceVDictTimeAmount, m_Ticks.Price, m_Ticks.Amount, m_lClusterTimes[0]);
                    if (m_Ticks.Direction == TickDirection.Buy)
                    {   //Update amount of specific price of current timespan
                        DictionaryManipulation(m_Dict_KPriceVDictTimeAmount_Buy, m_Ticks.Price, m_Ticks.Amount, m_lClusterTimes[0]);
                        //Update TOTAL amount of all prices of current timespan
                        DictionaryManipulation(m_Dict_KDateVVolume_Buy, m_lClusterTimes[0], m_Ticks.Amount);
                    }
                    if (m_Ticks.Direction == TickDirection.Sell)
                    {
                        //Update amount of specific price of current timespan
                        DictionaryManipulation(m_Dict_KPriceVDictTimeAmount_Sell, m_Ticks.Price, m_Ticks.Amount, m_lClusterTimes[0]);
                        //Update TOTAL amount of all prices of current timespan
                        DictionaryManipulation(m_Dict_KDateVVolume_Sell, m_lClusterTimes[0], m_Ticks.Amount);
                    }
                }
                m_bTicksFilled = true;

                //Try trap error
               

                //

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
            }
        }
    }
}

