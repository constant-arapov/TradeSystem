using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Media;



using TradingLib;
using TradingLib.ProtoTradingStructs;


using Terminal.Common;
using Terminal.TradingStructs;
using Terminal.TradingStructs.Clusters;


namespace Terminal.Controls.Market
{
    public partial class ControlClusters
    {

        // Using a DependencyProperty as the backing store for StringHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StringHeightProperty =
            DependencyProperty.Register("StringHeight", typeof(int), typeof(ControlClusters), new UIPropertyMetadata(1));

        public int StringHeight
        {          
            get { return (int)GetValue(StringHeightProperty); }
            set { SetValue(StringHeightProperty, value); }
        }




        public static DependencyProperty FontSizeProperty =
          DependencyProperty.Register("FontSize", typeof(int), typeof(ControlClusters), new UIPropertyMetadata(1));


        public int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            //   set { SetValue(FontSizeProperty, value); }
        }


         


        public decimal DealsAmountFullBar
        {
            get { return (decimal)GetValue(DealsAmountFullBarProperty); }
            set { SetValue(DealsAmountFullBarProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DealsAmountFullBar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DealsAmountFullBarProperty =
            DependencyProperty.Register("DealsAmountFullBar", typeof(decimal), typeof(ControlClusters), new PropertyMetadata((decimal)1));


        public static readonly DependencyProperty TickerNameProperty = DependencyProperty.Register("TickerName", typeof(string), typeof(ControlClusters));
        public string TickerName
        {
            get
            {
                _tickerName = (string)base.GetValue(TickerNameProperty);
                return _tickerName;
            }
            set
            {
               
                base.SetValue(TickerNameProperty, value);
            }
        }


        public Brush FontColor
        {
            get { return (Brush)GetValue(FontColorProperty); }
            set { SetValue(FontColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FontColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontColorProperty =
            DependencyProperty.Register("FontColor", typeof(Brush), typeof(ControlClusters), new UIPropertyMetadata(Brushes.Black));

        public bool IsInControlDeals
        {
            get { return (bool)GetValue(IsInControlDealsProperty); }
            set { SetValue(IsInControlDealsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMouseInControlDealsArea.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsInControlDealsProperty =
            DependencyProperty.Register("IsInControlDeals", typeof(bool), typeof(ControlClusters));


        /*
        public EnmTF TimeFrame
        {
            get { return (EnmTF)GetValue(TimeFrameProperty); }
            set { SetValue(TimeFrameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Timeframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeFrameProperty =
            DependencyProperty.Register("Timeframe", typeof(EnmTF), typeof(ControlClusters), new UIPropertyMetadata(EnmTF.M5));
        */



		public CClusterPrice ClusterPriceAmount
		{
			get { return (CClusterPrice)GetValue(ClusterPriceAmountProperty); }
			set { SetValue(ClusterPriceAmountProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ClusterPrice.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ClusterPriceAmountProperty =
			DependencyProperty.Register("ClusterPriceAmount", typeof(CClusterPrice), typeof(ControlClusters), new UIPropertyMetadata(new CClusterPrice()));


        private bool _disablePaintClusters;

		public bool DisablePaintClusters
		{
			get { return (bool)GetValue(DisablePaintClustersProperty); }
			set {
                    SetValue(DisablePaintClustersProperty, value);
                     _disablePaintClusters = value; //2018-08-14
                }
		}

		// Using a DependencyProperty as the backing store for DisablePaintClusters.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DisablePaintClustersProperty =
			DependencyProperty.Register("DisablePaintClusters", typeof(bool), typeof(ControlClusters), new UIPropertyMetadata(false));


     


		public bool DisableRecalcClusters
		{
			get { return (bool)GetValue(DisableRecalcClustersProperty); }
			set
            {
                SetValue(DisableRecalcClustersProperty, value);
             
            }
		}

		// Using a DependencyProperty as the backing store for DisableRecalcCluster.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DisableRecalcClustersProperty =
			DependencyProperty.Register("DisableRecalcCluster", typeof(bool), typeof(ControlClusters), new UIPropertyMetadata(false));



		

        public List<DateTime> LstTimes
        {
            get { return (List<DateTime>)GetValue(LstTimesProperty); }
            set { SetValue(LstTimesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LstTimes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LstTimesProperty =
            DependencyProperty.Register("LstTimes", typeof(List<DateTime>), typeof(ControlClusters), new UIPropertyMetadata(new List<DateTime>()));




        public CClusterDate ClusterDate
        {
            get { return (CClusterDate)GetValue(ClusterDateProperty); }
            set { SetValue(ClusterDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ClusterDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClusterDateProperty =
            DependencyProperty.Register("ClusterDate", typeof(CClusterDate), typeof(ControlClusters), new UIPropertyMetadata(new CClusterDate()));




        
        public string TimeFrame
        {
            get { return (string)GetValue(TimeFrameProperty); }
            set { SetValue(TimeFrameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimeFrame.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeFrameProperty =
            DependencyProperty.Register("TimeFrame", typeof(string), typeof(ControlClusters), new UIPropertyMetadata(""));



        
     /*   public string TimeFrameItem
        {
            get { return (string)GetValue(TimeFrameItemProperty); }
            set { SetValue(TimeFrameItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimeFrameItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeFrameItemProperty =
            DependencyProperty.Register("TimeFrameItem", typeof(string), typeof(ControlClusters), new UIPropertyMetadata(""));

       */ 





        public int StockNum
        {
            get { return (int)GetValue(StockNumProperty); }
            set { SetValue(StockNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StockNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StockNumProperty =
            DependencyProperty.Register("StockNum", typeof(int), typeof(ControlClusters), new UIPropertyMetadata(0));





		public int ClustersUpdatePerSec
		{
			get { return (int)GetValue(ClustersUpdatePerSecProperty); }
			set { SetValue(ClustersUpdatePerSecProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ClustersUpdatePerSec.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ClustersUpdatePerSecProperty =
			DependencyProperty.Register("ClustersUpdatePerSec", typeof(int), typeof(ControlClusters), new UIPropertyMetadata(0));



        public int DecimalVolume
        {
            get { return (int)GetValue(DecimalVolumeProperty); }
            set { SetValue(DecimalVolumeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsConnectedToServer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DecimalVolumeProperty =
            DependencyProperty.Register("DecimalVolumeProperty", typeof(int), typeof(ControlClusters), new UIPropertyMetadata(0));



    }
}
