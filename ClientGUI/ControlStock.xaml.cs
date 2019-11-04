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


using System.Windows.Threading;
using System.Diagnostics;


using GUIComponents;
using TradingLib;
using TradingLib.ProtoTradingStructs;


namespace ClientGUI
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class ControlStock : UserControl
    {
        decimal _minStep;
        int _parCountOfSteps = 10000;

        public CModelMarket ModelMarket { get; set; }
        decimal _maxPrice;

        decimal _highestStockPrice = 0;
        decimal _lowestStockPrice = 0;

        decimal _highestStockPriceOld = 0;
        decimal _lowestStockPriceOld = 0;


        public ControlStock()
        {
            InitializeComponent();
        }

        public void SetParameters(decimal minStep)
        {
            _minStep = minStep;
            _maxPrice = _parCountOfSteps * _minStep;

        }

        Stopwatch sw = new Stopwatch();




        public void CreateStockRecords()
        {
                       
            //TODO
            // Two ways: 
            // 1)Generate first only "realistic values" - near
            // the limits than generate other values in background
            // 2)Generate othre values scrolling the list
            //
            
            for (int i = _parCountOfSteps; i > 0; i--)
            {

                CStockRecord sr = new CStockRecord(i * _minStep);
                ListBoxStock.Items.Add(sr);
               
                 
            }
                   
         //   ListBoxStock.ScrollIntoView(ListBoxStock.Items[10000]);


            const int tick = 100;  //tick= 100ns
            const int _1ms = 10 * tick;

            DispatcherTimer dt = new DispatcherTimer();

            dt.Tick += new EventHandler(dispatcherTimer_Tick);
            dt.Interval = new TimeSpan(100 * _1ms); //   new TimeSpan(0,0,1);
            dt.Start();

        }

        public int GetRownumByPrice(decimal price)
        {
            return _parCountOfSteps - (int)(price / _minStep);

        }



        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            UpdateStockRecords();

        }

        long tmpVol = 0;

        Stopwatch sw1 = new Stopwatch();

        public void UpdateVolumesNewStock(EnmDir dir)        
        {

            decimal price = 0;
            long volume = 0;
            int rowNum = 0;

            int i = 0;


            try
            {

                List<List<CStock>> stockList = null;// ModelMarket.OutpStockClass.StockList;
               
               
               

                for (i = 0; i < stockList[(sbyte)dir].Count; i++)
                {
                    CStock stk = stockList[(sbyte)dir][i];
                    //note if we  retrieve price =0 than all other elements are zeroes      
                    //and nothing to process
                    if (stk.Price == 0)
                    {
                        //Find lowest and highest from curr stock
                        if (i > 0)
                        {
                            if (dir == EnmDir.Up)
                                _highestStockPrice = stockList[(sbyte)dir][i-1].Price;
                            else if (dir == EnmDir.Down)
                                _lowestStockPrice = stockList[(sbyte)dir][i-1].Price;
                        }
                        break;

                    }
                    price = stk.Price;
                    volume = stk.Volume;
                    //Rownum for current items
                    rowNum = GetRownumByPrice(price);

                    ((CStockRecord)ListBoxStock.Items[rowNum]).Volume = volume;
                    if (volume != 0)
                        ((CStockRecord)ListBoxStock.Items[rowNum]).Dir = dir;

                    //for  all prices beetwen neighbours prices in stock model fill zeroes
                    //in stock  fill zeroes
                    //TO DO move to model or ViewModel

                    if (i > 0)
                    {
                        decimal prevPrice = stockList[(sbyte)dir][i - 1].Price;
                        if (price - prevPrice > _minStep)
                        {
                            int iFrom = GetRownumByPrice(price);
                            int iTo = GetRownumByPrice(prevPrice);

                            for (int j = iFrom + 1; j < iTo; j++)
                                ((CStockRecord)ListBoxStock.Items[j]).Volume = 0;


                        }


                    }

                }
            }
            catch (Exception e)
            {

                string st = "";
            }
        }
        private void UpdateSpread()
        {
            List<List<CStock>> stockList = null;// ModelMarket.OutpStockClass.StockList;
            int iFrom = GetRownumByPrice(stockList[(sbyte)EnmDir.Up][0].Price);
            int iTo = GetRownumByPrice(stockList[(sbyte)EnmDir.Down][0].Price);
            if (iTo - iFrom <=1)
                return;

            for (int  i = iFrom + 1; i < iTo; i++ )
                ((CStockRecord)ListBoxStock.Items[i]).Volume = 0;

        }

        private void RemoveStockVolumeInterval(int from, int to)
        {
            for (int i = from; i < to; i++)
            {
                ((CStockRecord)ListBoxStock.Items[i]).Volume = 0;
            }
        }

        private void RemoveVolumeOutStock()
        {
            if (_highestStockPriceOld == 0 || _lowestStockPriceOld == 0)
                return;

            int rowUp;
            int rowDown;
            if (_highestStockPriceOld > _highestStockPrice)
            {
                rowUp = GetRownumByPrice(_highestStockPriceOld);
                rowDown = GetRownumByPrice(_highestStockPrice);

                for (int i = rowUp; i < rowDown; i++)
                    ((CStockRecord)ListBoxStock.Items[i]).Volume = 0;

            }
            if (_lowestStockPriceOld < _lowestStockPrice)
            {

                 rowDown = GetRownumByPrice(_lowestStockPriceOld);
                 rowUp = GetRownumByPrice(_lowestStockPrice);

                 for (int i = rowDown; i > rowUp; i--)
                     ((CStockRecord)ListBoxStock.Items[i]).Volume = 0;

            }
        }

        int rnMeanOld = 0;
        int parMaxDev = 3;
        int offset = 10;
        int moveTo = 0;
        public void UpdateStockRecords()
        {

            if (ModelMarket.OutpStockClass.StockListAsks == null || ModelMarket.OutpStockClass.StockListBids !=null)
                return;




            try
            {
                lock (ModelMarket.OutpStockClass.Locker)
                {


                    UpdateVolumesNewStock(EnmDir.Up);
                    UpdateVolumesNewStock(EnmDir.Down);

                    UpdateSpread();
                    RemoveVolumeOutStock();
                    // sw1.Start();
                    //TODO set all volumes to zero if not first time
                    //List<List<CStock>> stockList = ModelMarket.OutpStockClass.StockList;

                    //int rnUp = GetRownumByPrice(stockList[(sbyte)EnmDir.Up][0].Price);
                    //int rnDown = GetRownumByPrice(stockList[(sbyte)EnmDir.Down][0].Price);
                    //int rnMean = (int)(0.5 * (rnUp + rnDown));





                    if (rnMeanOld == 0)
                    {
                     //   ListBoxStock.ScrollIntoView(ListBoxStock.Items[rnDown + 15]);
                     //   rnMeanOld = rnMean;
                    }
                    else
                    {
                       // if (rnMean - rnMeanOld > parMaxDev)
                        {
                           // ListBoxStock.ScrollIntoView(ListBoxStock.Items[rnUp + offset]);
                        //    rnMeanOld = rnMean;
                        }
                       // else if (rnMean - rnMeanOld < -parMaxDev)
                        {
                          //  ListBoxStock.ScrollIntoView(ListBoxStock.Items[rnDown - offset]);
                         //   rnMeanOld = rnMean;
                        }

                      
                    }

                   

                }
            }
            catch (Exception e)
            {


            }


            decimal twoCyclesHighPrice =   _highestStockPriceOld == 0  ? _highestStockPrice :  Math.Max(_highestStockPrice, _highestStockPriceOld);
            decimal twoCyclesLowPrice = _lowestStockPriceOld == 0 ? _lowestStockPrice : Math.Min(_lowestStockPrice, _lowestStockPriceOld);

            int rowTwoCyclesUp = GetRownumByPrice(twoCyclesHighPrice);
            int rowTwoCyclesDown = GetRownumByPrice(twoCyclesLowPrice);


          //long rowNumHighPriceCur = GetRownumByPrice(_highestStockPrice);
          //long  rowNumLowPriceCur = GetRownumByPrice(_lowestStockPrice);



          //now refresh graphics   

            for (int i = rowTwoCyclesUp; i < rowTwoCyclesDown; i++)
                ((CStockRecord)ListBoxStock.Items[i]).UpdateStockRecordView();
        //foreach (CStockRecord sr in ListBoxStock.Items)
          //sr.UpdateStockRecordView();

        //now clean old values
        
        

        _highestStockPriceOld = _highestStockPrice;
        _lowestStockPriceOld = _lowestStockPrice;
                
          

        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {

                var item = sender as ListBoxItem;
                if (item != null /*&& item.IsSelected*/)
                {

                    CStockRecord sr = (CStockRecord)item.Content;


                    //Do your stuff
                }

            }
            catch (Exception err)
            {


            }

        }


    }


   


  
}
