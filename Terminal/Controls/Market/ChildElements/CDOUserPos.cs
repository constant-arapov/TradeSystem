using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;


using System.Windows.Media;



namespace Terminal.Controls.Market.ChildElements
{

    /// <summary>
    /// Draws rectangle between current prices and the price of opened position
    /// </summary>
    public class CDOUserPos : DependencyObject
    {




        public int Amount
        {
            get { return (int)GetValue(AmountProperty); }
            set { SetValue(AmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Amount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AmountProperty =
            DependencyProperty.Register("Amount", typeof(int), typeof(CDOUserPos), new UIPropertyMetadata(0));




        public decimal AvPos
        {
            get { return (decimal)GetValue(AvPosProperty); }
            set { SetValue(AvPosProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AvPos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AvPosProperty =
            DependencyProperty.Register("AvPos", typeof(decimal), typeof(CDOUserPos), new UIPropertyMetadata((decimal)0));






        public bool IsProfit
        {
            get { return (bool)GetValue(IsProfitProperty); }
            set { SetValue(IsProfitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsProfit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsProfitProperty =
            DependencyProperty.Register("IsProfit", typeof(bool), typeof(CDOUserPos), new UIPropertyMetadata(false));





        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(CDOUserPos), new UIPropertyMetadata(false));

        





        public bool IsBuy
        {
            get { return (bool)GetValue(IsBuyProperty); }
            set { SetValue(IsBuyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBuy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBuyProperty =
            DependencyProperty.Register("IsBuy", typeof(bool), typeof(CDOUserPos), new UIPropertyMetadata(false));




        public bool IsSell
        {
            get { return (bool)GetValue(IsSellProperty); }
            set { SetValue(IsSellProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSell.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSellProperty =
            DependencyProperty.Register("IsSell", typeof(bool), typeof(CDOUserPos), new UIPropertyMetadata(false));




        public double ProfitInPrice
        {
            get { return (double)GetValue(ProfitInPriceProperty); }
            set { SetValue(ProfitInPriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProfitInPrice.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProfitInPriceProperty =
            DependencyProperty.Register("ProfitInPrice", typeof(double), typeof(CDOUserPos), new UIPropertyMetadata((double)0));

        


        public double ProfitInPoints
        {
            get { return (double)GetValue(ProfitInPointsProperty); }
            set { SetValue(ProfitInPointsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProfitInPoints.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProfitInPointsProperty =
            DependencyProperty.Register("ProfitInPoints", typeof(double), typeof(CDOUserPos), new UIPropertyMetadata((double)0));



        public int ProfitInSteps
        {
            get { return (int)GetValue(ProfitInStepsProperty); }
            set { SetValue(ProfitInStepsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProfitInSteps.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProfitInStepsProperty =
            DependencyProperty.Register("ProfitInSteps", typeof(int), typeof(CDOUserPos), new UIPropertyMetadata(0));

        


        public string ProfitInPointsString
        {
            get { return (string)GetValue(ProfitInPointsStringProperty); }
            set { SetValue(ProfitInPointsStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProfitInPointsString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProfitInPointsStringProperty =
            DependencyProperty.Register("ProfitInPointsString", typeof(string), typeof(CDOUserPos), new UIPropertyMetadata(""));




        public string AvPosString
        {
            get { return (string)GetValue(AvPosStringProperty); }
            set { SetValue(AvPosStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AvPosString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AvPosStringProperty =
            DependencyProperty.Register("AvPosString", typeof(string), typeof(CDOUserPos), new UIPropertyMetadata(""));




        public string AmountString
        {
            get { return (string)GetValue(AmountStringProperty); }
            set { SetValue(AmountStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AmountString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AmountStringProperty =
            DependencyProperty.Register("AmountString", typeof(string), typeof(CDOUserPos), new UIPropertyMetadata(""));




        private Brush _brushProfit;
        private Brush _brushLoss;
        private Brush _brushPos;

        private Pen _penProfit;
        private Pen _penLoss;
        private Pen _penPos;


        private System.Windows.Threading.Dispatcher _guiDispatcher;

        public CDOUserPos(System.Windows.Threading.Dispatcher disp)
        {
         

            _brushPos = Brushes.Violet;
            _penPos = new Pen(_brushPos, 1);
            _guiDispatcher = disp;


        }


        //call from ThreadDraw
        public void Init(Brush brushProfit, Brush brushLoss, Pen penProfit,Pen penLoss)
        {
            _brushProfit = brushProfit;
            _brushLoss = brushLoss;

            _penProfit = penProfit;
            _penLoss = penLoss;

        }


        //NEED REFACTORING ! remove DP vars
        public void Draw(DrawingContext drwCntxt, double stringHeight, double x, double highestBidY, double lowestAskY, double widthTextPrice, double widthTotal, 
                         Dictionary<double,double> dictPriceY, int decimals)
        {
          

         
            int profitInSteps = 0;
            bool isBuy = false;
            bool isSell = false;
            bool isProfit = false;
            bool isActive = false;
            decimal avPos = 0;

            _guiDispatcher.Invoke(new Action(() =>
           {
               profitInSteps = ProfitInSteps;
               isBuy = IsBuy;
               isSell = IsSell;
               isProfit = IsProfit;
               isActive = IsActive;
               avPos = AvPos;
           }));

            if (!isActive)
                return;


            try
            {
                

                double price = (double)Math.Round(avPos, decimals);

				//2017_03_20 removed as client request
                ///*double price = (double)AvPos;
                //if (dictPriceY.ContainsKey(price))
                  //  drwCntxt.DrawRectangle(_brushPos, _penPos, new Rect(0.0, dictPriceY[price], widthTotal, stringHeight));
             


                double height = 0;
                double y = 0;


                height = stringHeight * (Math.Abs(profitInSteps) + 1);

                double heightVector = (profitInSteps) * stringHeight;


                if (isBuy)
                {
                    if (isProfit)
                        y = highestBidY;
                    else
                        y = highestBidY + heightVector;
                }
                else if (isSell)
                {
                    if (isProfit)
                        y = lowestAskY - heightVector;
                    else
                        y = lowestAskY;

                }

                Brush brush = Brushes.Black;
                Pen pen = new Pen(Brushes.Black, 1.0);

                if (isProfit)
                {
                    brush = _brushProfit;
                    pen = _penProfit;
                }
                else
                {
                    brush = _brushLoss;
                    pen = _penProfit;
                }





               drwCntxt.DrawRectangle(brush, pen, new Rect(x, y, widthTextPrice, height));
            }
            catch (Exception e)
            {

                CKernelTerminal.ErrorStatic("CDOUserPos.Draw", e);
            }



        }

    
        


        
    }
}
