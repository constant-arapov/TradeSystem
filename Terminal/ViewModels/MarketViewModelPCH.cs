using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using TradingLib;
using TradingLib.ProtoTradingStructs;

using Terminal.Common;
using Terminal.DataBinding;
using Terminal.TradingStructs;

namespace Terminal.ViewModels
{
    public partial class MarketViewModel
    {


        //Not possible use PropChangedAuto
        private string _currAmountNum = "0";

        public string CurrAmountNum
        {
            get
            {
                return _currAmountNum;

            }

            set
            {
                _currAmountNum = value;
                RaisePropertyChanged("CurrAmountNum");

            }



        }

        private string _stringHrigh = "1";



        [Magic]
        public string StringHeight
        {
            get
            {
                return _stringHrigh;
            }

            set
            {
                _stringHrigh = value;
            }

        }



        private string _fontSize = "1";

        [Magic]
        public string FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                _fontSize = value;
            }
        }


        //TODO remove ticker name
        private string _tickerName;
        [Magic]
        public string TickerName
        {
            get
            {
                return _tickerName;
            }
            set
            {
                _tickerName = value;

            }

        }


        [Magic]
        public int Decimals { get; set; }


        [Magic]
        public double Step { get; set; }


        [Magic]
        public List<CWorkAmount> ListWorkAmount { get; set; }

        //List<CDeal> _ticks = new List<CDeal>();

       // [Magic]



        private List <CDeal> _ticks = new List<CDeal>();

        [Magic]
        public List <CDeal>  Ticks
        {
            get
            {
                lock (_ticks)
                {
                    return _ticks;
                }
            }
           
        }



        [Magic]
        public bool IsNeedRepaintDeals { get; set; }



      

        private OrderData[] _orders ;//= new OrderData[10];//KAA temp
        [Magic]
        public OrderData[] Orders
        {
            get
            {
                return _orders;
            }

        }

        //TODO property changed
        public CStockPosition[] Asks
        {
            get
            {
                return this.m_Asks;
            }
        }

        public CStockPosition[] Bids
        {
            get
            {
                return this.m_Bids;
            }
        }


        private decimal _volumeFullBar = 1;

        [Magic]
        public decimal VolumeFullBar
        {
            get
            {
                return _volumeFullBar;
            }

            set
            {
                if (value>0)
                    if (value <_parMaxVolumeBar)
                        _volumeFullBar = value;
            }

        }

        //private long _bigVolume;

        [Magic]
        public decimal BigVolume { get; set; }


        private decimal _dealsAmountFullBar = 1;


        [Magic]
        public decimal DealsAmountFullBar
        {

            get
            {
                return _dealsAmountFullBar;
            }
            set
            {
                //changed 2018
                //zero devide protection
                _dealsAmountFullBar = Math.Max(value, 0.000000001m);
            }



        }

        [Magic]
        public bool IsInControlDeals { get; set; }

        [Magic]
        public int UserPosAmount { get; set; }

        private List<double> _level1Y = new List<double>();

        [Magic]
        public List<double> Level1Y 
        {
            get
            {
                return _level1Y ;
            }
            set
            {
                _level1Y = value;
            }
            
        }

        private List<double> _level2Y = new List<double>();


        [Magic]
        public List<double> Level2Y
        {
            get
            {
                return _level2Y;
            }
            set
            {
                _level2Y = value;
            }

        }

        [Magic]
        public double ActualHeight { get; set; }


        [Magic]
        public int StockNum { get; set; }


      
        //public EnmTF TimeFrame { get; set; }
        [Magic]
        public string TimeFrame { get; set; }


        [Magic]
        public bool IsConnectedToServer { get; set; }


		[Magic]
		public double StopLossPrice { get; set; }

		[Magic]
		public double TakeProfitPrice { get; set; }


        [Magic]
        public double StopLossInvertPrice { get; set; }


        [Magic]
        public double BuyStopPrice { get; set; }
        
        [Magic]                       
        public double SellStopPrice { get; set; }


        [Magic]
        public decimal BuyStopAmount { get; set; }


        [Magic]
        public decimal SellStopAmount { get; set; }

        [Magic]
        public CSelectionMode SelectionMode { get; set; }


		[Magic]
		public double SelectedY { get; set; }

        [Magic]
        public double SelectedPrice { get; set; }

		[Magic]
		public bool IsModeKeyboardTrading { get; set; }

		[Magic]
		public int ThrowSteps { get; set; }

        [Magic]
        public double Level1Mult { get; set; }

        [Magic]
        public double Level2Mult { get; set; }
        
        [Magic]
        public int DecimalVolume { get; set; }

		/*[Magic]
		public decimal   { get; set; }*/

        //[Magic]
        //public string TimeFrameItem { get; set; }


    }
}
