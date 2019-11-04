using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Windows;
using System.Windows.Media;




using TradingLib;
using TradingLib.ProtoTradingStructs;

using Terminal.Common;
using Terminal.ViewModels;
using Terminal.TradingStructs;


namespace Terminal.Controls.Market
{
    public partial class ControlStock
    {
        public static readonly DependencyProperty CurrAmountNumProperty = DependencyProperty.Register("CurrAmountNum",
                                                                           typeof(string), typeof(ControlStock),
                                                                           new PropertyMetadata(""));


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



        // Using a DependencyProperty as the backing store for StringHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StringHeightProperty =
            DependencyProperty.Register("StringHeight", typeof(int), typeof(ControlStock), new UIPropertyMetadata(1));

        public int StringHeight
        {
            get { return (int)GetValue(StringHeightProperty); }
            //set { SetValue(StringHeightProperty, value); }
        }



        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(int), typeof(ControlStock), new UIPropertyMetadata(1));


        //It's hide base class property of FontSize 
        //TODO fix it !
        public int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            //set { SetValue(FontSizetProperty, value); }
        }



        public static readonly DependencyProperty AsksProperty = DependencyProperty.Register("Asks", typeof(CStockPosition[]), typeof(ControlStock));
        public CStockPosition[] Asks
        {
            get
            {
                return (CStockPosition[])base.GetValue(AsksProperty);
            }
            set
            {
                base.SetValue(AsksProperty, value);
            }
        }

        public static readonly DependencyProperty BidsProperty = DependencyProperty.Register("Bids", typeof(CStockPosition[]), typeof(ControlStock));
        public CStockPosition[] Bids
        {
            get
            {
                return (CStockPosition[])base.GetValue(BidsProperty);
            }
            set
            {
                base.SetValue(BidsProperty, value);
            }
        }




        public static readonly DependencyProperty DecimalsProperty = DependencyProperty.Register("Decimals", typeof(int), typeof(ControlStock));
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




        public static readonly DependencyProperty OrdersProperty = DependencyProperty.Register("Orders", typeof(OrderData[]), typeof(ControlStock));
        public OrderData[] Orders
        {
            get
            {
                return (OrderData[])base.GetValue(OrdersProperty);
            }
            set
            {
                base.SetValue(OrdersProperty, value);
            }
        }



        public static readonly DependencyProperty UserPosProperty = DependencyProperty.Register("UserPos", typeof(ViewModelUserPos), typeof(ControlStock));
        public ViewModelUserPos UserPos
        {
            get
            {
                return (ViewModelUserPos)base.GetValue(UserPosProperty);
            }
            set
            {
                base.SetValue(UserPosProperty, value);
            }

        }


        public static readonly DependencyProperty ListWorkAmountProperty = DependencyProperty.Register("ListWorkAmount",
                                                                               typeof(List<CWorkAmount>), typeof(ControlStock));
        public List<CWorkAmount> ListWorkAmount
        {
            get
            {
                return (List<CWorkAmount>)base.GetValue(ListWorkAmountProperty);

            }

            set
            {
                base.SetValue(ListWorkAmountProperty, value);

            }

        }

        public static readonly DependencyProperty TickerNameProperty = DependencyProperty.Register("TickerName", typeof(string), typeof(ControlStock));
        public string TickerName
        {
            get
            {
                return (string)base.GetValue(TickerNameProperty);
            }
            set
            {
                base.SetValue(TickerNameProperty, value);
            }
        }


        public static readonly DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(double), typeof(ControlStock));
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


        
        public decimal VolumeFullBar
        {
            get 
            {
                //long val = (long)GetValue(VolumeFullBarProperty);

                return (decimal)GetValue(VolumeFullBarProperty); 
            }
            set 
            {
                SetValue(VolumeFullBarProperty, value); 
            }
        }

        // Using a DependencyProperty as the backing store for VolumeFullBar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VolumeFullBarProperty =
            DependencyProperty.Register("VolumeFullBar", typeof(decimal), typeof(ControlStock));


        public decimal BigVolume
        {
            get { return (decimal)GetValue(BigVolumeProperty); }
            set { SetValue(BigVolumeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BigVolume.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BigVolumeProperty =
            DependencyProperty.Register("BigVolume", typeof(decimal), typeof(ControlDeals));





        public Brush FontColor
        {
            get { return (Brush)GetValue(FontColorProperty); }
            set { SetValue(FontColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FontColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontColorProperty =
            DependencyProperty.Register("FontColor", typeof(Brush), typeof(ControlStock), new UIPropertyMetadata(Brushes.Black));




        public Brush BidColor
        {
            get { return (Brush)GetValue(BidColorProperty); }
            set { SetValue(BidColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BidColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BidColorProperty =
            DependencyProperty.Register("BidColor", typeof(Brush), typeof(ControlStock), new UIPropertyMetadata(Brushes.Black));




        public Brush AskColor
        {
            get { return (Brush)GetValue(AskColorProperty); }
            set { SetValue(AskColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AskColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AskColorProperty =
            DependencyProperty.Register("AskColor", typeof(Brush), typeof(ControlStock), new UIPropertyMetadata(Brushes.Black));




        public Brush BestBidColor
        {
            get { return (Brush)GetValue(BestBidColorProperty); }
            set { SetValue(BestBidColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BestBidColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BestBidColorProperty =
            DependencyProperty.Register("BestBidColor", typeof(Brush), typeof(ControlStock), new UIPropertyMetadata(Brushes.Black));




        public Brush BestAskColor
        {
            get { return (Brush)GetValue(BestAskColorProperty); }
            set { SetValue(BestAskColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BestAskColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BestAskColorProperty =
            DependencyProperty.Register("BestAskColor", typeof(Brush), typeof(ControlStock), new UIPropertyMetadata(Brushes.Black));






        public Brush VolumeBarColor
        {
            get { return (Brush)GetValue(VolumeBarColorProperty); }
            set { SetValue(VolumeBarColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VolumeBarColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VolumeBarColorProperty =
            DependencyProperty.Register("VolumeBarColor", typeof(Brush), typeof(ControlStock), new UIPropertyMetadata(Brushes.Black));





        public Brush BigVolumeColor
        {
            get { return (Brush)GetValue(BigVolumeColorProperty); }
            set { SetValue(BigVolumeColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BigVolumeColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BigVolumeColorProperty =
            DependencyProperty.Register("BigVolumeColor", typeof(Brush), typeof(ControlStock), new UIPropertyMetadata(Brushes.Black));




        public Brush LineL1Color
        {
            get { return (Brush)GetValue(LineL1ColorProperty); }
            set { SetValue(LineL1ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LineL1Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineL1ColorProperty =
            DependencyProperty.Register("LineL1Color", typeof(Brush), typeof(ControlStock), new UIPropertyMetadata(Brushes.Black));


        public Brush LineL2Color
        {
            get { return (Brush)GetValue(LineL2ColorProperty); }
            set { SetValue(LineL2ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LineL2Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineL2ColorProperty =
            DependencyProperty.Register("LineL2Color", typeof(Brush), typeof(ControlStock), new UIPropertyMetadata(Brushes.Black));





        public bool IsInControlDeals
        {
            get { return (bool)GetValue(IsInControlDealsProperty); }
            set { SetValue(IsInControlDealsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMouseInControlDealsArea.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsInControlDealsProperty =
            DependencyProperty.Register("IsInControlDeals", typeof(bool), typeof(ControlStock));





        public List<double> Level1Y
        {
            get { return (List<double>)GetValue(Level1YProperty); }
            set { SetValue(Level1YProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Level1Y.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Level1YProperty =
            DependencyProperty.Register("Level1Y", typeof(List<double>), typeof(ControlStock), new UIPropertyMetadata(new List<double>() ));





        public List<Double> Level2Y
        {
            get { return (List<Double>)GetValue(Level2YProperty); }
            set { SetValue(Level2YProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Level2Y.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Level2YProperty =
            DependencyProperty.Register("Level2Y", typeof(List<Double>), typeof(ControlStock), new UIPropertyMetadata(new List<double>()));


        public int StockNum
        {
            get { return (int)GetValue(StockNumProperty); }
            set { SetValue(StockNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StockNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StockNumProperty =
            DependencyProperty.Register("StockNum", typeof(int), typeof(ControlStock), new UIPropertyMetadata(0));





		public double StopLossPrice
		{
			get { return (double)GetValue(StopLossPriceProperty); }
			set { SetValue(StopLossPriceProperty, value); }
		}

		// Using a DependencyProperty as the backing store for StopLossPrice.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StopLossPriceProperty =
			DependencyProperty.Register("StopLossPrice", typeof(double), typeof(ControlStock), new UIPropertyMetadata(0.0));




		public double TakeProfitPrice
		{
			get { return (double)GetValue(TakeProfitPriceProperty); }
			set { SetValue(TakeProfitPriceProperty, value); }
		}

		// Using a DependencyProperty as the backing store for TakeProfitPrice.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TakeProfitPriceProperty =
			DependencyProperty.Register("TakeProfitPrice", typeof(double), typeof(ControlStock), new UIPropertyMetadata(0.0));





        public double StopLossInvertPrice
        {
            get { return (double)GetValue(StopLossInvertPriceProperty); }
            set { SetValue(StopLossInvertPriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StopLossInvertPrice.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StopLossInvertPriceProperty =
            DependencyProperty.Register("StopLossInvertPrice", typeof(double), typeof(ControlStock), new UIPropertyMetadata(0.0));




        public double  BuyStopPrice
        {
            get { return (double )GetValue(BuyStopPriceProperty); }
            set { SetValue(BuyStopPriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BuyStopPrice.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BuyStopPriceProperty =
            DependencyProperty.Register("BuyStopPrice", typeof(double ), typeof(ControlStock), new UIPropertyMetadata(0.0));





        public double SellStopPrice
        {
            get { return (double)GetValue(SellStopPriceProperty); }
            set { SetValue(SellStopPriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SellStopPrice.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SellStopPriceProperty =
            DependencyProperty.Register("SellStopPrice", typeof(double), typeof(ControlStock), new UIPropertyMetadata(0.0));




        public decimal BuyStopAmount
        {
            get { return (decimal)GetValue(BuyStopAmountProperty); }
            set { SetValue(BuyStopAmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BuyStopAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BuyStopAmountProperty =
            DependencyProperty.Register("BuyStopAmount", typeof(decimal), typeof(ControlStock), new UIPropertyMetadata(0.0m));




        public decimal SellStopAmount
        {
            get { return (decimal)GetValue(SellStopAmountProperty); }
            set { SetValue(SellStopAmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SellStopAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SellStopAmountProperty =
            DependencyProperty.Register("SellStopAmount", typeof(decimal), typeof(ControlStock), new UIPropertyMetadata(0.0m));




        public CSelectionMode SelectionMode
        {
            get { return (CSelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionModeProperty =
            DependencyProperty.Register("SelectionMode", typeof(CSelectionMode), typeof(ControlStock), new UIPropertyMetadata(new CSelectionMode()));




        
		public double SelectedPrice
		{
			get { return (double)GetValue(SelectedPriceProperty); }
			set { SetValue(SelectedPriceProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SelectedPrice.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectedPriceProperty =
			DependencyProperty.Register("SelectedPrice", typeof(double), typeof(ControlStock), new UIPropertyMetadata(0.0));

		


                    
        public double SelectedY
        {
            get { return (double)GetValue(SelectedYProperty); }
            set { SetValue(SelectedYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedYProperty =
            DependencyProperty.Register("SelectedY", typeof(double), typeof(ControlStock), new UIPropertyMetadata(0.0));



        public bool IsNeedRepaintDeals
        {
            get { return (bool)GetValue(IsNeedRepaintDealsProperty); }
            set { SetValue(IsNeedRepaintDealsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsNeedRepaintDeals.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNeedRepaintDealsProperty =
            DependencyProperty.Register("IsNeedRepaintDeals", typeof(bool), typeof(ControlStock)/*, new UIPropertyMetadata(false)*/);


		public CUserLevels UserLevels
		{
			get { return (CUserLevels)GetValue(UserLevelsProperty); }
			set { SetValue(UserLevelsProperty, value); }
		}

		// Using a DependencyProperty as the backing store for UserLevels.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty UserLevelsProperty =
			DependencyProperty.Register("UserLevels", typeof(CUserLevels), typeof(ControlStock));




		public int StockUpdatePerSec
		{
			get { return (int)GetValue(StockUpdatePerSecProperty); }
			set { SetValue(StockUpdatePerSecProperty, value); }
		}

		// Using a DependencyProperty as the backing store for StockUpdatePerSec.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StockUpdatePerSecProperty =
			DependencyProperty.Register("StockUpdatePerSec", typeof(int), typeof(ControlStock), new UIPropertyMetadata(0));




		public int ThrowSteps
		{
			get { return (int)GetValue(ThrowStepsProperty); }
			set { SetValue(ThrowStepsProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ThrowSteps.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ThrowStepsProperty =
			DependencyProperty.Register("ThrowSteps", typeof(int), typeof(ControlStock), new UIPropertyMetadata(0));




        public double Level1Mult
        {
            get { return (double)GetValue(Level1MultProperty); }
            set { SetValue(Level1MultProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Level1Mult.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Level1MultProperty =
            DependencyProperty.Register("Level1Mult", typeof(double), typeof(ControlStock), new UIPropertyMetadata(0.0));




        public double Level2Mult
        {
            get { return (double)GetValue(Level2MultProperty); }
            set { SetValue(Level2MultProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Level2Mult.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Level2MultProperty =
            DependencyProperty.Register("Level2Mult", typeof(double), typeof(ControlStock), new UIPropertyMetadata(0.0));




        public bool IsConnectedToServer
        {
            get { return (bool)GetValue(IsConnectedToServerProperty); }
            set { SetValue(IsConnectedToServerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsConnectedToServer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsConnectedToServerProperty =
            DependencyProperty.Register("IsConnectedToServer", typeof(bool), typeof(ControlStock), new UIPropertyMetadata(false));



        public int DecimalVolume
        {
            get { return (int)GetValue(DecimalVolumeProperty); }
            set { SetValue(DecimalVolumeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsConnectedToServer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DecimalVolumeProperty =
            DependencyProperty.Register("DecimalVolumeProperty", typeof(int), typeof(ControlStock), new UIPropertyMetadata(0));



    }
}
