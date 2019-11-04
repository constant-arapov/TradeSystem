using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using Newtonsoft.Json.Linq;

using Common.Utils;
using Common.Logger;
using Common.Collections;

using TradingLib;
using TradingLib.Enums;
using TradingLib.Data;

using BitfinexCommon;

using BitfinexWebSockConnector.Interfaces;
using BitfinexWebSockConnector.Data;
using BitfinexWebSockConnector.Helpers;

namespace BitfinexWebSockConnector 
{
    class CBfxStockStor
    {


        private Dictionary<int, Dictionary<Direction, List<CBfxStockRec>>> _store;

        private CLogger _logger;
        private string _instrument;


        private Dictionary<Direction, List<CBfxStockRec>> _copy = new Dictionary<Direction, List<CBfxStockRec>>();
        private Stopwatch _sw = new Stopwatch();


        private CSharedStocks _outStock;

        private int _decimalVolume;

        private IClientBfxStockStor _client;
		private int _stockDept;

        private Dictionary<int, CLogger> _dictLogger = new Dictionary<int, CLogger>();

        private CBlockingQueue<CBfxStorStorMsg> _queue = new CBlockingQueue<CBfxStorStorMsg>();

        private CPerfStockStor _perf;

        public CBfxStockStor(IClientBfxStockStor client, string instrument, int decimalVolume,
                              int stockDept, List<int> lstPrecissions)
        {
            _client = client;

            _store = new Dictionary<int, Dictionary<Direction, List<CBfxStockRec>>>();
            _stockDept = stockDept;

            foreach (var prec in lstPrecissions)
            {
                _store[prec] = new Dictionary<Direction, List<CBfxStockRec>>();
                _store[prec][Direction.Up] = new List<CBfxStockRec>();
                _store[prec][Direction.Down] = new List<CBfxStockRec>();
                _dictLogger[prec] = new CLogger( String.Format("{0}_p{1}",instrument,prec), 
                                                    flushMode: true, subDir: "StockStor");



            }
            _instrument = instrument;
            _logger = new CLogger(instrument, flushMode: false, subDir: "StockStor");
            
            _outStock = new CSharedStocks(_client);
            _decimalVolume = decimalVolume;
            

            CUtil.ThreadStart(ThreadMain);


            _perf = new CPerfStockStor(true, instrument,lstPrecissions);
        }

        

        public void ThreadMain()
        {

            while (true)
            {
               CBfxStorStorMsg msg = _queue.GetElementBlocking();
               if (msg.Event == EnmStockMsgUpd.UpdateStock)
                {
                    CBfxStockStorUpdStock updStock = (CBfxStockStorUpdStock)msg.Data;
                    Update(updStock.prec, updStock.price, updStock.count, updStock.amount);
                }
               else if (msg.Event == EnmStockMsgUpd.UpdateSnapshot)
                {
                    CBfxStockStorMsgUpdSnap updSnap = (CBfxStockStorMsgUpdSnap)msg.Data;
                    UpdateBySnapshot(updSnap.prec, updSnap.jArrOrderBook);


                }
               

            }
            

        }

        



        public void UpdateBySnapshot(int prec, JArray jArrOrderBook)
        {
            Log(prec,"===> SNAPSHOT");


            _store[prec][Direction.Up].Clear();
            _store[prec][Direction.Down].Clear();

            foreach (JToken el in jArrOrderBook)
            {
                decimal price = (decimal)el[0];
                long count = (long)el[1];
                decimal amount = (decimal)el[2];


                Direction dir = amount > 0 ? Direction.Down : Direction.Up;

                _store[prec][dir].Add(new CBfxStockRec { Price = price, Amount = Math.Abs(amount) });

                //2018-06-26 truncate - overflow protection
              //  if (_store[prec][dir].Count > _stockDept)
                  //  break;
            }


            UpdateClientStockBothDir(prec, 
                    new CCmdStockChange
                    {
                       
                        Code = EnmStockChngCodes._01_UpdBySnapshot,
                        Price = 0,
                        Volume = 0
                    }
                                          );


            //2018-06-27 perfomance
           // PrintStock(prec);


        }


       


        private void UpdateClientStockBothDir(int precision, CCmdStockChange cmdSockChange)
        {
         

            lock (_outStock)
            {
                
                    foreach (Direction dir in Enum.GetValues(typeof(Direction)))
                    {
                        for (int j = 0; j < _store[precision][dir].Count; j++)
                        {
                            try
                            {
                                _outStock[dir][precision][j].Price = _store[precision][dir][j].Price;
                                _outStock[dir][precision][j].Volume = CUtilConv.GetIntVolume(_store[precision][dir][j].Amount, _decimalVolume);                              
                            }
                            catch (Exception exc)
                            {
                                _client.Error("CbfxStockStor.UpdateClientStockBothDir", exc);

                            }
                        }
                    }

                    _outStock.InpCmdStockChange[Direction.Up][precision] = cmdSockChange;
                    _outStock.InpCmdStockChange[Direction.Down][precision] = cmdSockChange;
            }

            _outStock.UpdateBidAsk();
          
            _client.UpdateCleintStockBothDir(_instrument, precision, _outStock);
         
        }

     






        private void UpdateClientStockOneDir(Direction dir,
                                                int precision, CCmdStockChange cmdSockChange)
        {

            lock(_outStock)
            {

            
                    for (int j = 0; j < _store[precision][dir].Count; j++)
                    {
                        try
                        {
                            _outStock[dir][precision][j].Price = _store[precision][dir][j].Price;
                            _outStock[dir][precision][j].Volume = CUtilConv.GetIntVolume(_store[precision][dir][j].Amount, _decimalVolume);
                        }
                        catch (Exception exc)
                        {
                            _client.Error("CbfxStockStor.UpdateClientStockOneDir", exc);

                        }
                    }

                   for (int i= _store[precision][dir].Count; 
                            i< _outStock.Count - _store[precision][dir].Count;i++)
                   {
                        _outStock[dir][precision][i].Price = 0;
                        _outStock[dir][precision][i].Volume = 0;
                    }



                _outStock.InpCmdStockChange[dir][precision] = cmdSockChange;

                _outStock.UpdateBidAsk();

                _client.UpdateClientStockOneDir(_instrument, dir, precision, _outStock);


            }


        }


        





        public void Update(CBfxStockStorUpdStock data)
        {
            CBfxStorStorMsg msg = new CBfxStorStorMsg { Event = EnmStockMsgUpd.UpdateStock, Data = data };
            _queue.Add(msg);
        }


        public void UpdateBySnapshot(CBfxStockStorMsgUpdSnap data)
        {
            CBfxStorStorMsg msg = new CBfxStorStorMsg { Event = EnmStockMsgUpd.UpdateSnapshot, Data = data };
            _queue.Add(msg);
        }





        public void Update(int prec, decimal price, long count, decimal amount)
        {
            _perf.UpdStart();
            Direction dir = Direction.Up;

            CCmdStockChange cmd =  new CCmdStockChange
            {                            
                Price = price                
            };

            if (count > 0)
            {

              //  if (count == 1)
                 //   System.Threading.Thread.Sleep(0);

                UpdateOrInsert(prec,price, amount, ref cmd, ref dir);
              
            }
            else if (count == 0)
            {
                     dir = (amount == 1) ? Direction.Down : Direction.Up;
                    _store[prec][dir].RemoveAll(el => el.Price == price);
                    Log(prec,String.Format("{0} price={1} dir={2}", "==> remove", price, dir));
                //2018-06-27 perfomance
                 PrintStock(prec);

                cmd.Code = EnmStockChngCodes._03_Remove;

                
                                  

            }

            UpdateClientStockOneDir(dir, prec, cmd);

            _perf.UpdEnd(prec);
        }

        public void UpdateDecimalVolume(int decimalVolume)
        {
            Log(String.Format("Update decimal volume {0} => {1}", _decimalVolume, decimalVolume));
            _decimalVolume = decimalVolume;
        }






        public void UpdateOrInsert(int prec, decimal price, decimal amount, 
                                        ref CCmdStockChange cmd, ref Direction dir)
        {   
         
             dir = (amount > 0) ? Direction.Down : Direction.Up;


            decimal amountAbs= Math.Abs(amount);
            cmd.Volume = CUtilConv.GetIntVolume(amountAbs, _decimalVolume);

            for (int i = 0; i < _store[prec][dir].Count; i++)
            {
                if (_store[prec][dir][i].Price == price)
                {
                    _store[prec][dir][i].Amount = amountAbs;
                    Log(prec,String.Format("{0} price={1} amount={2}", "==> update", price, amountAbs));
                    //2018-06-27 perfomance
                    // PrintStock(prec);

                    cmd.Code = EnmStockChngCodes._04_Update;

                    
                    return;
                }

            }



            //cmd.Code = dir


            Log(prec,String.Format("{0} price={1} amount={2}", "==> add", price, amountAbs));
            _store[prec][dir].Add(new CBfxStockRec { Price = price, Amount = Math.Abs(amountAbs) });
            Sort(prec,dir);

            cmd.Code = EnmStockChngCodes._02_Add;
            


            //2018-06-27 perfomance
            //PrintStock(prec);


            /*  CBfxStockRec rec =  this[dir].Find(el => el.Price == price);
              if (rec == null)
              {
                  Log(String.Format("{0} price={1} amount={2}", "==> add", price, amount));
                  this[dir].Add(new CBfxStockRec { Price = price, Amount = Math.Abs(amount) });
                  Sort(dir);
              }
              else
              {
                   Log(String.Format("{0} price={1} amount={2}", "==> update", price, amount));

                  rec.Amount = Math.Abs(amount);
              }
              */
        }

        








        public void Sort(int prec, Direction dir)
        {

            int mult = (dir == Direction.Up) ? 1 : -1;


           
           _store[prec][dir].Sort (delegate ( CBfxStockRec x, CBfxStockRec y)
           {
                        return mult * x.Price.CompareTo(y.Price);
           }
                    );



        }

        private void Log(int prec, string msg)
        {
            //2018-01-04 disabled as using a lot of of disk space
         
          //  _dictLogger[prec].Log(msg);
        }

        //remove this in the future
        private void Log(string msg)
        {
            //_logger.Log(msg);
        }
        


        private void PrintStock(int prec)
        {            
            Log(prec,"========================================================= BEGIN ============================================================================================");
            string bids= "", asks ="";
            //this[Direction.Up].ForEach( el => bids += el.Price +" " +el.Amount +"|");

            string stOldPrice = "";
            _store[prec][Direction.Up].ForEach(el => 
                {
                    bids += el.Price + " " + el.Amount + "|";
                    if (el.Price.ToString() == stOldPrice)
                        System.Threading.Thread.Sleep(0);

                    stOldPrice = el.Price.ToString();			
                }
                );

            Log(prec,""); Log(prec,"");
            //this[Direction.Down].ForEach(el => asks += el.Price + " " + el.Amount + "|");
            _store[prec][Direction.Down].ForEach(el =>
            {
                asks += el.Price + " " + el.Amount + "|";
                //if (el.Price.ToString() == stOldPrice)
                  //  System.Threading.Thread.Sleep(0);

                stOldPrice = el.Price.ToString();

            }
              );
            Log(prec,bids);
            Log(prec,asks);
            Log(prec,"======================================================= END ==============================================================================================");
            Log(prec,"");


        }





    }
}
