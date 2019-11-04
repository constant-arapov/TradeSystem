using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;
using System.ComponentModel;

using Common;
using Common.Logger;

using TradingLib;
using TradingLib.Abstract;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Clients;
using TradingLib.Enums;
using TradingLib.Data;
using TradingLib.ProtoTradingStructs;



namespace Plaza2Connector
{
    public class CStockConverterP2 : CBaseStockConverter, IStockConverter, 
				INotifyPropertyChanged
    {

      
                  
        private ManualResetEventSlim /*AutoResetEvent*/ _evUpdate = new ManualResetEventSlim()/*new AutoResetEvent(false)*/;
       



		public CRawStock[] InpSnapshotRawStocks;
		public Object LckInpRawStocks = new object();
      
   

        public CSharedStocks SharedStocks
        {
            get
            {
                return m_dirStock;


            }



        }
         public CStockConverterP2(int stockDept, string isin, IClientStockConverter client)
			 :base(stockDept,isin,client)
         {
          

             InpSnapshotRawStocks = new CRawStock[stockDept];
            m_dirStock = new CSharedStocks(client);

             for (int i = 0; i < _stockDepth; i++)
                 InpSnapshotRawStocks[i] = new CRawStock();

             (new Thread(ThreadStockConverter)).Start();

         }

      

      

       

    public void TriggerPeriodicUpdate()
    {
        _evUpdate.Set();
    }


    /// <summary>
    /// Main working thread.
	/// Copies data from InpSnapshotRawStocks (input snapshot that periodically updates
	/// by stockstruct) to m_dirStock (model of stock with two 
    /// direction up/down or buy/sell). Sorts each directions.
    /// Than copies this data to StockSnapshoter
    /// Than depend of eclapsed time call UpdateOutStock 
    /// which updates traders stocks
    /// </summary>
    public void ThreadStockConverter()
    {

         
            Stopwatch sw = new Stopwatch();

          


            while (true)
            {
                _evUpdate.Wait();
                _evUpdate.Reset();

               // if (Isin == "USD000000TOD")
                 //   System.Threading.Thread.Sleep(0);


                //Log("____b___________________________________________", sw);


                bool bBidAskChanged = false;

                lock (m_dirStock)
                {
                    try
                    {
                        sw.Reset();
                        sw.Start();
                       
                      //  Log("Start processed m_dirStock", sw);
                        int d = 0;
                        int num_up = 0;
                        int num_down = 0;
						//Copy  all elements from InpSnapshotRawStocks 
						//to m_dirStock.						
                        foreach (CRawStock rsk in InpSnapshotRawStocks)
                        {
                            if (rsk.Dir != 0 && rsk.Price != 0)
                            {
                                Direction dir;
                                if (rsk.Dir == 2)
                                {
                                    dir = Direction.Up;
                                    num_up++;
                                    d = num_up;
                                }
                                else
                                {
                                    dir = Direction.Down;
                                    num_down++;
                                    d = num_down;
                                }
                                //mod 2018-06-22 add prec 0
                                m_dirStock[dir][0][d - 1].Price = rsk.Price;
                                m_dirStock[dir][0][d - 1].Volume = rsk.Volume;

                            }
                        }
						//Log("Copy  data from InpSnapshotRawStocks to m_dirstock finished", sw);
						//Fill al elements that after num_up/num down with zeroes 
						//
                        //2018-06-22 add precision 0
                        for (int ii = num_up; ii < 100; ii++)
                        {
                            if (m_dirStock[Direction.Up][0][ii].Price == 0)
                                break;

                            m_dirStock[Direction.Up][0][ii].Price = 0;
                            m_dirStock[Direction.Up][0][ii].Volume = 0;

                        }
                        for (int ii = num_down; ii < 100; ii++)
                        {
                            if (m_dirStock[Direction.Down][0][ii].Price == 0)
                                break;

                            m_dirStock[Direction.Down][0][ii].Price = 0;
                            m_dirStock[Direction.Down][0][ii].Volume = 0;
                        }

                  //      Log("Fill zero dirstock finished", sw);


                        QuicksortAsc(m_dirStock[Direction.Up][0], 0, num_up - 1);
                        QuicksortDesc(m_dirStock[Direction.Down][0], 0, num_down - 1);

                      //  Log("Sorting dirstock finished", sw);

                        m_dirStock.Bid = m_dirStock[Direction.Down][0][0].Price;
                        m_dirStock.Ask = m_dirStock[Direction.Up][0][0].Price;
                        //2018-07-10
                        m_dirStock.LstStockConf = new List<CStockConf> {
                                new CStockConf
                                {
                                  PrecissionNum = 0,
                                  MinStep = _client.GetMinStep(Instrument),
                                  DecimalsPrice = _client.GetDecimals(Instrument)

                                 }
                        };
                        _bidInternal = m_dirStock.Bid;
                        _askInternal = m_dirStock.Ask;


                        //bid or ask changed
                        if (_bidInternalOld != _bidInternal || _askInternalOld != _askInternal)
                        {
                            //KAA no need forcing bid ask

                            bBidAskChanged = true;
                        //    Log("================= Need update Bid=" + m_dirStock.Bid + " Ask=" + m_dirStock.Ask, sw);

                        }

                      

                        _bidInternalOld = m_dirStock.Bid;
                        _askInternalOld = m_dirStock.Ask;

                      //  Log("m_dirStock was processed", sw);
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
                    
                    bool isTradingServer = _client.GlobalConfig.IsTradingServer;

                  //  Log("Before update inp stocks", sw);
                    if (isTradingServer && _client.SnapshoterStock!=null)
                        _client.SnapshoterStock.UpdateInpStocksBothDir(Instrument, ref m_dirStock, precision:0);
                 //   Log("After update inp stocks", sw);
                    if (bBidAskChanged)
                    {
                    //    Log("Beofre update inp stocks", sw);
                        if (isTradingServer)
                            _client.SnapshoterStock.UpdateOutStock(Instrument,
																			  bDoNotUpdateTraders:true);
                    //    Log("After update inp stocks", sw);
                      //  Log("Before update bidasks", sw);
                        UpdateBidAsk(_bidInternal, _askInternal);
                      //  Log("After update bidasks", sw);
                        _client.UserDealsPosBox.RefreshBotPos(Instrument);
                       
                    }
                    Log("____e___________________________________________", sw);
                    Log("");
                    
                }
                catch (Exception e)
                {
                    Error("CStockStruct.ThreadFunc 2", e);

                }


                //TEMPO FOR DEBUG
                PrintStocks();

            }

          

        }

    
    /// <summary>
    /// Added 2017-11-24 native update stocks - directly
    /// Call from CStockBox.UpdateStockFromNative  
    /// </summary>
    public void UpdateStocksFromNative(string instrument,ref CSharedStocks sourceStock)
    {

        _bidInternal = sourceStock.Bid;
        _askInternal = sourceStock.Ask;

        UpdateBidAsk(_bidInternal, _askInternal);

        _client.SnapshoterStock.UpdateInpStocksBothDir(instrument, ref sourceStock, precision:0);

      //  _client.SnapshoterStock.UpdateOutStock(instrument, bDoNotUpdateTraders:false);
        _client.UserDealsPosBox.RefreshBotPos(Instrument);

    }


    private void PrintStocks()
    {
        Log("____________________________________________________________________________________________________");
        foreach (var kvp in m_dirStock)
        {


            StringBuilder ln = new StringBuilder();
            foreach (var kvp2 in kvp.Value[0])
            {
                ln.Append(String.Format("{0} {1}|", kvp2.Price, kvp2.Volume));

            }
            Log(ln.ToString());
        }

    }



    }
}
