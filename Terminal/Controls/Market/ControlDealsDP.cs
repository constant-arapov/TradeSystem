using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;

using System.Windows.Media;


using TradingLib;
using TradingLib.ProtoTradingStructs;

using Terminal.TradingStructs;
using Terminal.ViewModels;

namespace Terminal.Controls.Market
{
    public partial class ControlDeals
    {



        public static readonly DependencyProperty TickerNameProperty = DependencyProperty.Register("TickerName", typeof(string), typeof(ControlDeals));
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

        public int StringHeight
        {
            get { return (int)GetValue(StringHeightProperty); }
            //set { SetValue(StringHeightProperty, value); }
        }



        // Using a DependencyProperty as the backing store for StringHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StringHeightProperty =
            DependencyProperty.Register("StringHeight", typeof(int), typeof(ControlDeals), new UIPropertyMetadata(1));



        public int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            //set { SetValue(FontSizetProperty, value); }
        }



        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(int), typeof(ControlDeals), new UIPropertyMetadata(1));



        public static readonly DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(double), typeof(ControlDeals));


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




        public List<CDeal> Ticks
        {
            get 
            {
               /* var d = List <CDeal> GetValue(TicksProperty);
                if (d != null)
                    System.Threading.Thread.Sleep(0);
                */

                return (List<CDeal>)GetValue(TicksProperty);
            }
            set
            {
                SetValue(TicksProperty, value);
            }

         
        }

        // Using a DependencyProperty as the backing store for Ticks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TicksProperty = DependencyProperty.Register("Ticks", typeof(List<CDeal>), typeof(ControlDeals), new UIPropertyMetadata(new List<CDeal>()));


        public static readonly DependencyProperty OrdersProperty = DependencyProperty.Register("Orders", typeof(OrderData[]), typeof(ControlDeals));
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




        public bool IsNeedRepaintDeals
        {
            get { return (bool)GetValue(IsNeedRepaintDealsProperty); }
            set { SetValue(IsNeedRepaintDealsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsNeedRepaintDeals.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNeedRepaintDealsProperty =
            DependencyProperty.Register("IsNeedRepaintDeals", typeof(bool), typeof(ControlDeals)/*, new UIPropertyMetadata(false)*/);
                                        

        public Brush FontColor
        {
            get { return (Brush)GetValue(FontColorProperty); }
            set { SetValue(FontColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FontColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontColorProperty =
            DependencyProperty.Register("FontColor", typeof(Brush), typeof(ControlDeals), new UIPropertyMetadata(Brushes.Black));


        public Brush LineL1Color
        {
            get { return (Brush)GetValue(LineL1ColorProperty); }
            set { SetValue(LineL1ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LineL1Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineL1ColorProperty =
            DependencyProperty.Register("LineL1Color", typeof(Brush), typeof(ControlDeals), new UIPropertyMetadata(Brushes.Black));




        public Brush LineL2Color
        {
            get { return (Brush)GetValue(LineL2ColorProperty); }
            set { SetValue(LineL2ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LineL2Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineL2ColorProperty =
            DependencyProperty.Register("LineL2Color", typeof(Brush), typeof(ControlDeals), new UIPropertyMetadata(Brushes.Black));



        public bool IsInControlDeals
        {
            get { return (bool)GetValue(IsInControlDealsProperty); }
            set { SetValue(IsInControlDealsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMouseInControlDealsArea.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsInControlDealsProperty =
            DependencyProperty.Register("IsInControlDeals", typeof(bool), typeof(ControlDeals));



        public List<double> Level1Y
        {
            get { return (List<double>)GetValue(Level1YProperty); }
            set { SetValue(Level1YProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Level1Y.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Level1YProperty =
            DependencyProperty.Register("Level1Y", typeof(List<double>), typeof(ControlDeals), new UIPropertyMetadata(new List<double>()));


        public List<Double> Level2Y
        {
            get { return (List<Double>)GetValue(Level2YProperty); }
            set { SetValue(Level2YProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Level2Y.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Level2YProperty =
            DependencyProperty.Register("Level2Y", typeof(List<Double>), typeof(ControlDeals), new UIPropertyMetadata(new List<double>()));


        public int StockNum
        {
            get { return (int)GetValue(StockNumProperty); }
            set { SetValue(StockNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StockNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StockNumProperty =
            DependencyProperty.Register("StockNum", typeof(int), typeof(ControlDeals), new UIPropertyMetadata(0));


        public double StopLossPrice
        {
            get { return (double)GetValue(StopLossPriceProperty); }
            set { SetValue(StopLossPriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StopLossPrice.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StopLossPriceProperty =
            DependencyProperty.Register("StopLossPrice", typeof(double), typeof(ControlDeals), new UIPropertyMetadata(0.0));




        public double TakeProfitPrice
        {
            get { return (double)GetValue(TakeProfitPriceProperty); }
            set { SetValue(TakeProfitPriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TakeProfitPrice.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TakeProfitPriceProperty =
            DependencyProperty.Register("TakeProfitPrice", typeof(double), typeof(ControlDeals), new UIPropertyMetadata(0.0));





        public double StopLossInvertPrice
        {
            get { return (double)GetValue(StopLossInvertPriceProperty); }
            set { SetValue(StopLossInvertPriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StopLossInvertPrice.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StopLossInvertPriceProperty =
            DependencyProperty.Register("StopLossInvertPrice", typeof(double), typeof(ControlDeals), new UIPropertyMetadata(0.0));




        public double BuyStopPrice
        {
            get { return (double)GetValue(BuyStopPriceProperty); }
            set { SetValue(BuyStopPriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BuyStopPrice.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BuyStopPriceProperty =
            DependencyProperty.Register("BuyStopPrice", typeof(double), typeof(ControlDeals), new UIPropertyMetadata(0.0));





        public double SellStopPrice
        {
            get { return (double)GetValue(SellStopPriceProperty); }
            set { SetValue(SellStopPriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SellStopPrice.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SellStopPriceProperty =
            DependencyProperty.Register("SellStopPrice", typeof(double), typeof(ControlDeals), new UIPropertyMetadata(0.0));




        public decimal BuyStopAmount
        {
            get { return (decimal)GetValue(BuyStopAmountProperty); }
            set { SetValue(BuyStopAmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BuyStopAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BuyStopAmountProperty =
            DependencyProperty.Register("BuyStopAmount", typeof(decimal), typeof(ControlDeals), new UIPropertyMetadata(0.0m));




        public decimal SellStopAmount
        {
            get { return (decimal)GetValue(SellStopAmountProperty); }
            set { SetValue(SellStopAmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SellStopAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SellStopAmountProperty =
            DependencyProperty.Register("SellStopAmount", typeof(decimal), typeof(ControlDeals), new UIPropertyMetadata(0.0m));




        public CSelectionMode SelectionMode
        {
            get { return (CSelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionModeProperty =
            DependencyProperty.Register("SelectionMode", typeof(CSelectionMode), typeof(ControlDeals), new UIPropertyMetadata(new CSelectionMode()));



        public double SelectedY
        {
            get { return (double)GetValue(SelectedYProperty); }
            set { SetValue(SelectedYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedYProperty =
            DependencyProperty.Register("SelectedY", typeof(double), typeof(ControlDeals), new UIPropertyMetadata(0.0));



        public double SelectedPrice
        {
            get { return (double)GetValue(SelectedPriceProperty); }
            set { SetValue(SelectedPriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedPrice.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedPriceProperty =
            DependencyProperty.Register("SelectedPrice", typeof(double), typeof(ControlDeals), new UIPropertyMetadata(0.0));





		public CUserLevels UserLevels
		{
			get { return (CUserLevels)GetValue(UserLevelsProperty); }
			set { SetValue(UserLevelsProperty, value); }
		}

		// Using a DependencyProperty as the backing store for UserLevels.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty UserLevelsProperty =
			DependencyProperty.Register("UserLevels", typeof(CUserLevels), typeof(ControlDeals));



        public bool IsConnectedToServer
        {
            get { return (bool)GetValue(IsConnectedToServerProperty); }
            set { SetValue(IsConnectedToServerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsConnectedToServer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsConnectedToServerProperty =
            DependencyProperty.Register("IsConnectedToServer", typeof(bool), typeof(ControlDeals), new UIPropertyMetadata(false));



        public int DecimalVolume
        {
            get { return (int)GetValue(DecimalVolumeProperty); }
            set { SetValue(DecimalVolumeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsConnectedToServer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DecimalVolumeProperty =
            DependencyProperty.Register("DecimalVolumeProperty", typeof(int), typeof(ControlDeals), new UIPropertyMetadata(0));


    }
}
