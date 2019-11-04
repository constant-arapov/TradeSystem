using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;
using System.ComponentModel;

using Common;
using TradingLib;

namespace Plaza2Connector
{
    public class CStockConverter : INotifyPropertyChanged
    {

        public CRawStock[] InpRawStocks;
        public Object LckInpRawStocks = new object();

        private int _stockDepth;

        CLogger m_logger;
        public string Isin { get; set; }

        public ManualResetEventSlim /*AutoResetEvent*/ EvUpdate = new ManualResetEventSlim()/*new AutoResetEvent(false)*/;
        CSharedStocks m_dirStock;

        private decimal _bidInternal = 0;
        private decimal _askInternal = 0;


        private decimal _bidInternalOld = 0;
        private decimal _askInternalOld = 0;


        private decimal _bidOut = 0;
        private decimal _askOut = 0;

        private object _lckBidOut = new object();
        private object _lckAskOut = new object();



        public event PropertyChangedEventHandler PropertyChanged;


         CPlaza2Connector m_plaza2Connector;


        private decimal _bid;
        public decimal Bid
        {

            get
            {
                return _bid;

            }
            set
            {
                _bid = value;
                RaisePropertyChanged("Bid");
            }

        }


        private decimal _ask;
        public decimal Ask
        {

            get
            {
                return _ask;

            }
            set
            {
                _ask = value;
                RaisePropertyChanged("Ask");
            }

        }

         public CSharedStocks SharedStocks
        {
            get
            {
                return m_dirStock;


            }



        }
        /*
         public decimal GetBestPrice(Direction dir)
         {
             decimal price = 0;

             lock (m_dirStock.Lck)
             {
                 try
                 {
                     //this.MutexStocks.WaitOne();

                     if (this.Stocks[dir].Count > 0)
                     {

                         price = this.Stocks[dir][0].Price;
                         if (price == 0)
                             Error("price==0");


                     }
                 }
                 catch (Exception e)
                 {
                     string err = "GetBestPrice error ";
                     m_logger.Log(err + e.StackTrace + " " + e.Message);
                     Error(err, e);

                 }
                 finally
                 {

                     //  this.MutexStocks.ReleaseMutex();
                 }

             }
             return price;

         }
        */


        public void Error(string description, Exception exception = null)
        {
            m_plaza2Connector.Alarmer.Error(description, exception);

        }


        protected void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }


        public CStockConverter(int stockDept,string isin, CPlaza2Connector plaza2Connector)
        {
            _stockDepth = stockDept;
            Isin = isin;
            m_plaza2Connector = plaza2Connector;

            InpRawStocks = new CRawStock[stockDept];
            m_dirStock = new CSharedStocks(stockDept);

            for (int i = 0; i < _stockDepth; i++)
                InpRawStocks[i] = new CRawStock();
            
              (new Thread(ThreadStockConverter)).Start();

        }

        public void Log(string msg, Stopwatch sw=null)
        {
            string outMsg = msg;
            if (sw != null)
                outMsg = " [" + sw.ElapsedTicks.ToString() + "] " + msg;

            m_logger.Log(outMsg);
        }


        int PartitionAsc(CStock[] array, int start, int end)
        {
            int marker = start;
            for (int i = start; i <= end; i++)
            {
                if (array[i].Price <= array[end].Price)
                {
                    CStock temp = array[marker]; // swap
                    array[marker] = array[i];
                    array[i] = temp;
                    marker += 1;
                }
            }
            return marker - 1;
        }
        //from inet
        void QuicksortAsc(CStock[] array, int start, int end)
        {
            if (start >= end)
            {
                return;
            }
            int pivot = PartitionAsc(array, start, end);
            QuicksortAsc(array, start, pivot - 1);
            QuicksortAsc(array, pivot + 1, end);
        }
        //==============================================================================
        int PartitionDesc(CStock[] array, int start, int end)
        {
            int marker = start;
            for (int i = start; i <= end; i++)
            {
                if (array[i].Price >= array[end].Price)
                {
                    CStock temp = array[marker]; // swap
                    array[marker] = array[i];
                    array[i] = temp;
                    marker += 1;
                }
            }
            return marker - 1;
        }
        //from inet
        void QuicksortDesc(CStock[] array, int start, int end)
        {
            if (start >= end)
            {
                return;
            }
            int pivot = PartitionDesc(array, start, end);
            QuicksortDesc(array, start, pivot - 1);
            QuicksortDesc(array, pivot + 1, end);
        }


        public void UpdateBidAndAskGUI(decimal bid, decimal ask)
        {

            Ask = ask;
            Bid = bid;
        }

        public void UpdateBidAsk(decimal bid, decimal ask)
        {

            try
            {

                lock (_lckBidOut)
                {
                    _bidOut = bid;

                }
                lock (_lckAskOut)
                {
                    _askOut = ask;
                }

                //TODO separate GUI and recalc logics
                //TODO for calculation use another bid and ask
                //if need directly use:
                //UpdateBidAndAskGUI(decimal bid, decimal ask)

                m_plaza2Connector.GUIBox.ExecuteWindowsStockUpdate(new Action(() => UpdateBidAndAskGUI(bid, ask)));

                Log("======== DEBUGING ======   Bid=" + Bid + " Ask=" + Ask + " =============================================");

            }
            catch (Exception e)
            {

                Error("UpdateBidAsk", e);
            }

        }

       public decimal GetBid()
        {
            lock (_lckBidOut)
            {
                return _bidOut;
            }

        }

        public decimal GetAsk()
        {
            lock (_lckAskOut)
            {
                return _askOut;

            }

        }
    public void ThreadStockConverter()
    {

            m_logger = new CLogger("StockConverter_" + Isin,false,"",true);
            Stopwatch sw = new Stopwatch();
            while (true)
            {
                EvUpdate.Wait();
                EvUpdate.Reset();

                Log("____b___________________________________________", sw);


                bool bNeedForceStockUpdate = false;

                lock (m_dirStock)
                {
                    try
                    {
                        sw.Reset();
                        sw.Start();

                        Log("Start processed m_dirStock", sw);
                        int d = 0;
                        int num_up = 0;
                        int num_down = 0;
                        foreach (CRawStock rsk in InpRawStocks)
                        {
                            if (rsk.Dir != 0 && rsk.Price != 0)
                            {
                                Direction DrSt;
                                if (rsk.Dir == 2)
                                {
                                    DrSt = Direction.Up;
                                    num_up++;
                                    d = num_up;
                                }
                                else
                                {
                                    DrSt = Direction.Down;
                                    num_down++;
                                    d = num_down;
                                }

                                m_dirStock[DrSt][d - 1].Price = rsk.Price;
                                m_dirStock[DrSt][d - 1].Volume = rsk.Volume;

                            }
                        }


                        Log("Fill data dirstock finished", sw);

                        for (int ii = num_up; ii < 100; ii++)
                        {
                            if (m_dirStock[Direction.Up][ii].Price == 0)
                                break;

                            m_dirStock[Direction.Up][ii].Price = 0;
                            m_dirStock[Direction.Up][ii].Volume = 0;

                        }
                        for (int ii = num_down; ii < 100; ii++)
                        {
                            if (m_dirStock[Direction.Down][ii].Price == 0)
                                break;

                            m_dirStock[Direction.Down][ii].Price = 0;
                            m_dirStock[Direction.Down][ii].Volume = 0;
                        }

                        Log("Fill zero dirstock finished", sw);


                        QuicksortAsc(m_dirStock[Direction.Up], 0, num_up - 1);
                        QuicksortDesc(m_dirStock[Direction.Down], 0, num_down - 1);

                        Log("Sorting dirstock finished", sw);

                        m_dirStock.Bid = m_dirStock[Direction.Down][0].Price;
                        m_dirStock.Ask = m_dirStock[Direction.Up][0].Price;

                        _bidInternal = m_dirStock.Bid;
                        _askInternal = m_dirStock.Ask;


                        // Log("PrintStocksStarted", sw);
                        //PrintStocks();

                        //Log("PrintStocksFinished",sw);

                        if (_bidInternalOld != _bidInternal || _askInternalOld != _askInternal)
                        {
                            bNeedForceStockUpdate = true;
                            Log("================= Need update Bid=" + m_dirStock.Bid + " Ask=" + m_dirStock.Ask, sw);

                        }




                        _bidInternalOld = m_dirStock.Bid;
                        _askInternalOld = m_dirStock.Ask;

                        Log("m_dirStock was processed", sw);
                        sw.Stop();
                    }
                    catch (Exception e)
                    {
                        string st = e.Message;
                        Error("CStockStruct.ThreadFunc", e);

                    }
                   
                        
                    

                }
               

                try
                {

                    Log("Before update inp stocks", sw);
                    m_plaza2Connector.StockDispatcher.UpdateInpStocks(Isin, m_dirStock);
                    Log("After update inp stocks", sw);
                    if (bNeedForceStockUpdate)
                    {
                        Log("Beofre update inp stocks", sw);
                        m_plaza2Connector.StockDispatcher.UpdateOutStock(Isin);
                        Log("After update inp stocks", sw);
                        Log("Before update bidasks", sw);
                        UpdateBidAsk(_bidInternal, _askInternal);
                        Log("After update bidasks", sw);
                        m_plaza2Connector.UserDealsPosBox.RefreshBotPos(Isin);
                    }
                    Log("____e___________________________________________", sw);
                    Log("");

                }
                catch (Exception e)
                {
                    Error("CStockStruct.ThreadFunc 2", e);

                }





            }


        }


    }
}
