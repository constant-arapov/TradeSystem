using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;




using Common;
using Common.Interfaces;
using Common.Collections;
using Common.Logger;
using Common.Utils;

using TCPLib;
using TCPLib.Interfaces;


using Messenger;

using TradingLib;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Clients;
using TradingLib.Enums;
using TradingLib.Common;
using TradingLib.Data;
using TradingLib.ProtoTradingStructs;
using TradingLib.Bots;
using TradingLib.BotEvents;


using TradingLib.TradersDispatcher.Helpers;

//using DBCommunicator;


namespace TradingLib.TradersDispatcher
{
   public class CTradersDispatcher :  /*IAlarmable,*/  CBaseFunctional,   ITCPServerUser /*, ILogable*/
    {
        /*CPlaza2Connector*/ IClientTradersDispatcher _client;
        CTCPServer _tcpServer;
        CPerfAnlzr _perfAnlzr;

        /*CDBCommunicator*/IDBCommunicator _dbCommunicator;

       // CLogger _logger;


        Dictionary<int, CTrader> _dictConnIdTrader = new Dictionary<int, CTrader>();
        Dictionary<int, int> _dictBotIdConnId = new Dictionary<int, int>();

        Dictionary<int, CBotTrader> _dictKBotIdVBotTrader = new Dictionary<int, CBotTrader>();


        int _perfSendDataToClient;



        /*CMessenger*/
        IMessenger _messenger;

        System.Timers.Timer _synchroTimeTimer;

        private Dictionary<string, CStockClass> _currentStocksList =
            new Dictionary<string, CStockClass>();


        private Dictionary<string, CParallelUpdater> _dictStockParallelUpdaters = new Dictionary<string, CParallelUpdater>();
        private Dictionary<string, CParallelUpdater> _dictDealsParallelUpdaters = new Dictionary<string, CParallelUpdater>();



        private CBlockingQueue<CTradingData> _bqTraderData = new CBlockingQueue<CTradingData>();

        private Dictionary<string, CLogger> _dictLogger = new Dictionary<string, CLogger>();


        private CTradersAccountsStorage _tradesAccountsStorage ;


        private DBGTrdDispStockUpd _dbgStockUpd;

        private Dictionary<string, Dictionary<Direction, Dictionary<int,  DateTime>>> _dictDtLastStockSnapSent = 
            new Dictionary<string, Dictionary<Direction, Dictionary<int, DateTime>>>();


        private int _snapshotIntervalMs = 1000;


        public CTradersAccountsStorage TradesAccountsStorage
        {
            get
            {
                return _tradesAccountsStorage;
            }

        }



        private CDBGTrdDispatcher _dbg;

        private int _stockExchId;

        //CStocksProtoPacker

        public CTradersDispatcher(/*CPlaza2Connector*/IClientTradersDispatcher client /*, CTCPServer tcpServer*/)
            : base ( (IAlarmable) client)
        {
            _client = client;
            _stockExchId = _client.StockExchId;
            _dbCommunicator = _client.DBCommunicator;
            _messenger = _client.Messenger;

            //_messenger = new CMessenger();


            _perfSendDataToClient = _client.GlobalConfig.PerfSendDataToClient;
            _dbg = new CDBGTrdDispatcher(this);


            if (_client.GlobalConfig.StockSnapshotInvtervalMs == 0)
                throw new ApplicationException("Stock snapshot interval is not set.");


            _snapshotIntervalMs = _client.GlobalConfig.StockSnapshotInvtervalMs;


         //   _logger = new CLogger("TradersDicpatcher");
            Log("Starting traders dispatcher");

            (new Thread(ThreadFunc)).Start();

           // _timerTimeSynchro.Elapsed += new ElapsedEventHandler(OnTimeSynchro_Elapsed);
            //_timerTimeSynchro.Interval = 5 * 1000;

            

            foreach (var kvp in _client.Instruments.DictInstrument_IsinId)
            {
                //_currentStocks[kvp.Key] = new Dictionary<Direction, List<CStock>>();
                _currentStocksList[kvp.Key] = new CStockClass(kvp.Key, _client.GetPricePrecisions());

                _dictStockParallelUpdaters[kvp.Key] = new CParallelUpdater(kvp.Key, SendUpdateTradersStock);
                _dictDealsParallelUpdaters[kvp.Key] = new CParallelUpdater(kvp.Key, SendUpdateTradersDeals);

                _dictDtLastStockSnapSent[kvp.Key] = new Dictionary<Direction, Dictionary<int, DateTime>>();

                foreach(Direction dir in Enum.GetValues(typeof(Direction)))
                {
                    _dictDtLastStockSnapSent[kvp.Key][dir] = new Dictionary<int, DateTime>();
                    foreach(int perc in _client.GetPricePrecisions() )                   
                        _dictDtLastStockSnapSent[kvp.Key][dir][perc] = DateTime.MinValue;
                    

                }


                //_dictDtLastStockSnapSent[kvp.Key] = DateTime.MinValue;


                //_currentStocksList[kvp.Key].StockList.Add(new List<CStock>()); //DOWN
                //_currentStocksList[kvp.Key].Add(new List<CStock>()); //UP
              
            }
            
            
            //TO DO get from config
            double interv = /*60 * */1000;
            _synchroTimeTimer = new System.Timers.Timer(interv);
            _synchroTimeTimer.Elapsed += new ElapsedEventHandler(SynchroTimeTimer_Elapsed);

            _synchroTimeTimer.Start();

            _perfAnlzr = new CPerfAnlzr((IAlarmable) _client);
            
          
            foreach (KeyValuePair <string,long> kvp in client.Instruments.DictInstrument_IsinId)
            {
                string isin = kvp.Key;
                _dictLogger[isin] = new CLogger("PerfTradersDisp_" + isin,
                                                true, "PerfTradersDisp", true);

            }



            _dbgStockUpd = new DBGTrdDispStockUpd(isOn: _client.GlobalConfig.DebugStocks,
                                                         dictInstr : client.Instruments.DictInstrument_IsinId);

        }

       /*
        public void Log(string msg)
        {
            if (_logger != null)
                _logger.Log(msg);



        }
        */


     






        public bool IsNeedSenUserPosLog(int botID)
        {
            if (_client.UserDealsPosBox == null ||
             _client.UserDealsPosBox.DicBotPosLog == null ||
               !_client.UserDealsPosBox.DicBotPosLog.ContainsKey(botID)
               )
                return false;

            return true;
        }

        public bool IsNeedSendUserDealsLog(int botID)       
        {
        
            if (_client.UserDealsPosBox == null ||
                _client.UserDealsPosBox.DictUserDealsLog == null ||
                !_client.UserDealsPosBox.DictUserDealsLog.ContainsKey(botID)                               
                )
            return false;

            return true;

        }
       

        private void LogIsin(string isin,string msg)
        {
            if (_dictLogger.ContainsKey(isin))
                _dictLogger[isin].Log(msg);


        }



        void SynchroTimeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {

            CTimeSynchroClass ts = new CTimeSynchroClass { DtCurrentTime = DateTime.Now };

            SendDataToClients(ts, enmTradingEvent.SynchroniseTime, "");
           
        }


		public void CallbackDisconnect(int conId)
		{
            try
            {
                if (_dictConnIdTrader.ContainsKey(conId)
                    && _dictConnIdTrader[conId].BotId != 0)
                {

                    int botId = _dictConnIdTrader[conId].BotId;
                    if (_dictKBotIdVBotTrader.ContainsKey(botId))
                        _dictKBotIdVBotTrader[botId].Recalc("", EnmBotEventCode.OnTraderDisconnected, null);

                    _client.DeleteClientInfo(conId);
                    Log(String.Format("Disconnect connection id={0}", conId));
                }
            }
            catch (Exception e)
            {
                Error("TradersDispatcher.CallbackDisconnect", e);
            }
		}

        /*public void Error(string msg, Exception e=null)
        {
            if (_plaza2Connector != null && _plaza2Connector.Alarmer != null)
                _plaza2Connector.Alarmer.Error(msg, e);

        }
         */


        public bool IsConnected(int botId)
        {

            if (_dictBotIdConnId.ContainsKey(botId))
                return true;

            return false;
        }

        public void SetTCPServer(CTCPServer tcpServer)
        {
            _tcpServer = tcpServer;

        }

        public void UpdateTradersStocks(string isin)
        {

            _dictStockParallelUpdaters[isin].Update();

        }

        


        public void UpdateTradersDeals(string isin)
        {
            _dictDealsParallelUpdaters[isin].Update();

        }
        public void EnqueueUpdateStockCommand(string isin)
        {
            _bqTraderData.Add(new CTradingData 
                                { Data = (object)isin, 
                                  Event = enmTradingEvent.StockUpadate 
                                }
                                  );
            
        }

        public void EnqueueSendAuthResponse(bool isSuccess, string errMsg, int connId)
        {

            _bqTraderData.Add(new CTradingData
                                {
                                    Data = (object) new CAuthResponse { IsSuccess = isSuccess, ErrorMessage = errMsg},
                                    Event = enmTradingEvent.AuthResponse,
                                    ConnId = connId
                                }
                                );

        }


        public void EnqueueAcceptStopLossTakeProfit(int botID, string instrument, EnmOrderTypes stoplossTakeProfit, decimal price, decimal amount=0)
        {

            if (IsConnected(botID))
            {

                int connId = _dictBotIdConnId[botID];

                _bqTraderData.Add(new CTradingData
                {
                    Data = (object)new CSetOrder { Instrument = instrument, OrderType = stoplossTakeProfit, Price = price, Amount = amount },
                    Event = enmTradingEvent.StopLossTakeProfitAccepted,
                    ConnId = connId


                }
                );
            }


        }

		public void EnqueUpdateInstrumentParams(string Instrument, decimal min_step, 
												int decimals, int decimalVolume, decimal minimum_order_size)
		{
			_bqTraderData.Add(new CTradingData
			{
				Data = new CUpdateInstrumentParams
				{
					Instrument = Instrument,
					Min_step = min_step,
					Decimals = decimals,
					DecimalVolume = decimalVolume,
					MinimumOrderSize = minimum_order_size
				},
				Event = enmTradingEvent.UpdateInstrumentParams

			}
			);

		}

        public void EnqueueSendAvailableTickers(/*int connId*/int botId)
        {


            int connId = _dictBotIdConnId[botId];


            CAvailableTickers availTicks = new CAvailableTickers();




            //changed 2018-06-22
            foreach (var el in _client.Instruments)
            //foreach (var kvp in _client.DictInstruments)
            {
                // Commented 2017-11-30
                //

                // if (_client.IsExistBotIdInstrument(botId,kvp.Key))
                {
                    availTicks.ListAvailableTickers.Add(new CTIckerData
                    {
                        TickerName = el.instrument,
                        Decimals = el.RoundTo,
                        Step = el.Min_step,
                        DecimalVolume = el.DecimalVolume,
                        minimum_order_size = el.minimum_order_size

                    });
                }
            }



            _bqTraderData.Add(new CTradingData
            {

                Data = (object)availTicks,
                Event = enmTradingEvent.UserUpdateAvailableTickers,
                ConnId = connId


            });





        }




        public void EnqueueUpdatUserPositionsMonitor(CBotBase bb, int traderId)
        {

             int connId = -1;
            lock (_dictBotIdConnId)
            {
                if (!_dictBotIdConnId.ContainsKey(traderId))
                    return;

                connId = _dictBotIdConnId[traderId];
            }
            if (connId != -1)
            {



                CUserPosMonitorUpdate userPosUpdate = new CUserPosMonitorUpdate();
                userPosUpdate.MonitorUserPos = new Dictionary<string, CUserPos>();
                lock (bb.MonitorPositionsAll)
                {                  
                    foreach (var v in bb.MonitorPositionsAll)
                    {
                        string isin = v.Key;
                        userPosUpdate.MonitorUserPos[isin] = new CUserPos
                        {                           
                            Amount = v.Value.Amount,
                            AvPos = v.Value.AvPos
                        };

                                                        
                    }
                }

        _bqTraderData.Add(new CTradingData
           {
               Data = (object) userPosUpdate,
               Event = enmTradingEvent.UserUpdatePositionMonitor,
               ConnId = connId


           }
           );
                _dbg.DBGUpdUSerPosMon(traderId, userPosUpdate);
              
            }
          
           
        }

        public void EnqueueUpdateVm(int botID)
        {
            if (!IsNeedSenUserPosLog(botID))
                return;

            var res = _dictBotIdConnId.FirstOrDefault(a => a.Key == botID);                         
            if (res.Key == 0) return; //not found

            int connId = res.Value;


            CUserVMUpdate updateVM = new CUserVMUpdate();
            CBotTrader bot = _dictKBotIdVBotTrader[botID];
            lock (bot.DictVMBotSessionTotalClosed)
            {
                foreach (var kvp in bot.DictVMBotSessionTotalClosed)
                    updateVM.ListVM.Add(new CUserVmInstrumentRecord { Isin = kvp.Key, VM = kvp.Value });
                
                updateVM.VMStockRecord = new CUserVMStockRecord
                {
                    StockName = CodesStockExch.GetStockName(_stockExchId),
                    TotalVM = bot.VMAllInstrClosed
                };
            }

            _bqTraderData.Add(new CTradingData
           {
               ConnId = connId,
               Data = (object)updateVM,
               Event = enmTradingEvent.UserUpdateVM
           }
           );



        }
        public void EnqueueUpdateDealsLog(int botID)
        {
    
            if(!IsNeedSendUserDealsLog(botID))
                return;

            var res = _dictBotIdConnId.FirstOrDefault(a => a.Key == botID);
                         
            if (res.Key == 0) return; //not found

            int connId = res.Value;


            CUserDealsLogUpdate udl = new CUserDealsLogUpdate();


            lock (_client.UserDealsPosBox.DictUserDealsLog)
            {
              if (_client.UserDealsPosBox.DictUserDealsLog.ContainsKey(botID))
              {
               var userDealsLog =    _client.UserDealsPosBox.DictUserDealsLog[botID];
                foreach (var kvp in  userDealsLog)
                {
                    udl.DictLog[kvp.Key] = new List<CUserDeal>();
                    foreach (var v in kvp.Value)
                    {
                        CUserDeal userDeal = v;
                        string instrument = kvp.Key;
                        //userDeal.Decimals = _client.Instruments.GetDecimals(instrument);
                        int decimals = _client.Instruments.GetDecimals(instrument);
                        userDeal.PriceSt = v.Price.ToString("N0" + decimals);
                        //userDeal.PriceSt = _client.Instruments.
                        udl.DictLog[kvp.Key].Add(userDeal);


                    }
                }
              }
            }

            _bqTraderData.Add(new CTradingData
            {
                ConnId = connId,
                Data = (object)udl,
                Event = enmTradingEvent.UserUpdateDealsLog
            }
           );



            //CUserD


        }




        public void UpdateMoneyData()
        {

           
            foreach (var kvp in _client.AccountsMoney)
                EnqueueUpdateMoneyData(kvp.Key);

             

            


        }



        public void EnqueueUpdateMoneyData(int botId)
        {



            var res = _dictBotIdConnId.FirstOrDefault(a => a.Key == botId);

            if (res.Key == 0) return; //not found

            int connId = res.Value;


            CUserMoney userMon = new CUserMoney();
            CUtil.CopyObjProperties(_client.AccountsMoney[botId], userMon.AccountMoney);
           
 

            
             CAccountTrade accTr = new CAccountTrade();
             CUtil.CopyObjProperties(_client.AccountsTrade[botId], accTr);

			//injection !
			//TODO normal in the future
			 accTr.money_sess_limit = _client.GetSessionLimit(botId);

             userMon.ListAccountTrade.Add(accTr);                                

         


            _bqTraderData.Add(new CTradingData
            {
                ConnId = connId,
                Data = (object)userMon,
                Event = enmTradingEvent.UpdateMoneyData
            }
            );
        }


        public void EnqueueUpdateUserPosLog(int botID)
        {
            if (!IsNeedSenUserPosLog(botID))
                return;
           


            var res = _dictBotIdConnId.FirstOrDefault(a => a.Key == botID);

            if (res.Key == 0) return; //not found

            int connId = res.Value;


         
           // CUserPosMonitorUpdate upos = new CUserPosMonitorUpdate();

            CUserPosLogUpdate poslog = new CUserPosLogUpdate();
            poslog.DictLog = new Dictionary<string, List<CUserPosLog>>();

            //for trader terminals send all deals
            lock (_client.UserDealsPosBox.DicBotPosLog)
            {
                var dictBotPosLog = _client.UserDealsPosBox.DicBotPosLog[botID];

                foreach (var kvp in dictBotPosLog)
                {
                    poslog.DictLog[kvp.Key] = new List<CUserPosLog>();
                    string instrument = kvp.Key;
                    //userDeal.Decimals = _client.Instruments.GetDecimals(instrument);
                    int decimals = _client.Instruments.GetDecimals(instrument);
                    int decimalVolume = _client.Instruments.GetDecimalVolume(instrument);

                    foreach (var v in kvp.Value)
                        poslog.DictLog[kvp.Key].Add
                            (new CUserPosLog
                            {
                                PriceOpen = v.PriceOpen,
                                PriceClose = v.PriceClose,
                                BuySell= v.BuySell,
                                DtOpen = v.DtOpen,
                                DtClose = v.DtClose,
                                CloseAmount = v.CloseAmount,
                                VMClosed_Points =  v.VMClosed_Points,
                                VMClosed_RUB = v.VMClosed_RUB,
                                Fee = v.Fee,
                                PriceOpenSt = v.PriceOpen.ToString("N0" + decimals),//TODO put to client side
                                PriceCloseSt = v.PriceClose.ToString("N0" + decimals),
                                CloseAmountSt = v.CloseAmount.ToString("N0"+ decimalVolume)
                            });
                }

            }

         
            

            _bqTraderData.Add (new CTradingData
            {
                ConnId = connId,
                Data = (object)  poslog,
                Event = enmTradingEvent.UserUpdatePosLog
            }
            );
        }





        public void EnqueueUpdateUserPosLogLate(int botId, CUserPosLogUpdLate userPosLogUpdLate)
        {
            int connId = -1;

            if (!_dictBotIdConnId.TryGetValue(botId, out connId))
                return;


            _bqTraderData.Add(new CTradingData
            {
                ConnId = connId,
                Data = (object)userPosLogUpdLate,
                Event = enmTradingEvent.UpdateUserPosLogLate
            });


        }


      

      

        Stopwatch sw_1 = new Stopwatch();
        Stopwatch sw_2 = new Stopwatch();


       /// <summary>
       /// Sends stock to traders
	   /// 
	   /// Note: packing end sending in parallel thread, not using blocking queue -
	   /// triggering from parallel updater
       /// 
       /// Call from:
       ///  CParallelUpdater
       /// </summary>
       /// <param name="isin"></param>
        public void SendUpdateTradersStock(string isin)
        {
            sw_1.Reset();
            sw_1.Start();


            sw_2.Reset();
            sw_2.Start();

            CopyStock(isin);
        

            sw_2.Stop();


            _currentStocksList[isin].DtBeforePack = DateTime.Now;
           
            

            SendDataToClients(_currentStocksList[isin], enmTradingEvent.StockUpadate, isin);


            CleanCurrentStockQueue(isin);
            sw_1.Stop();

           /* if (sw_1.ElapsedMilliseconds > 20)
            {
                System.Threading.Thread.Sleep(1);
            }
            */
        }


        private void CleanCurrentStockQueue(string instr)
        {

            foreach (var kvp in  _currentStocksList[instr].QueueCMDStockChng)
            {
                foreach(var kvp2 in kvp.Value)
                if (kvp2.Value.Count != 0)
                    kvp2.Value.Clear();

            }


        }




        




        const int cnstBroadCast = -1;
       // connId == -1 for all else for specific connId

        public void SendDataToClients(object ob, enmTradingEvent ev, string isin, int connId = cnstBroadCast)
        {
            try
            {


                Stopwatch sw2_1 = new Stopwatch();
                Stopwatch sw2_2 = new Stopwatch();
                Stopwatch sw2_3 = new Stopwatch();
                //Stopwatch sw2_4 = new Stopwatch();
                //Stopwatch sw2_5 = new Stopwatch();
                //Stopwatch sw2_6 = new Stopwatch();




                sw2_1.Reset(); sw2_1.Start();
                sw2_2.Reset(); sw2_2.Start();
                sw2_3.Reset(); sw2_3.Start();
              //  sw2_4.Reset(); sw2_4.Start();
              //  sw2_5.Reset(); sw2_5.Start();
              //  sw2_6.Reset(); sw2_6.Start();

                byte[] arrHeader = /*CMessenger*/_messenger.GenBinaryMessageHeader((byte) ev);
              //  sw2_6.Stop();
               // byte[] arrBody = CUtil.SerializeBinary(ob).ToArray();

               
                byte[] arrBody = CUtilProto.SerializeProto(ob);

              //  byte[] arrBody = (CUtil.SerializeBinaryExt(ob, ref ms, ref formatter)).ToArray();

                //sw2_5.Stop();
                byte[] arrMsg = new byte[arrHeader.Length + arrBody.Length];

                //sw2_4.Stop();
                Buffer.BlockCopy(arrHeader, 0, arrMsg, 0, arrHeader.Length);

                sw2_3.Stop();
                Buffer.BlockCopy(arrBody, 0, arrMsg, arrHeader.Length, arrBody.Length);

                sw2_2.Stop();
                


                //TO DO send olny subscribers that are subscribed
       
                if (connId == cnstBroadCast)
                {

                    foreach (KeyValuePair<int, CTrader> kvp in _dictConnIdTrader)
                    if (kvp.Value.IsLoggedOn)
                        if (ev == enmTradingEvent.SynchroniseTime || kvp.Value.SubscribedIsins.Contains(isin))
                            _tcpServer.SendMessage(kvp.Key, arrMsg);

                    sw2_1.Stop();

                    LogIsin(isin, ev + " sw2_1.Milliseconds= " + sw2_1.ElapsedMilliseconds + " s2_1.Ticks=" + sw2_1.ElapsedTicks);

                    _perfAnlzr.CheckLim(sw2_1.ElapsedMilliseconds, _perfSendDataToClient, "CTradersDispatcher.SendDataToClients sw2_1");
                    _perfAnlzr.CheckLim(sw2_2.ElapsedMilliseconds, _perfSendDataToClient, "CTradersDispatcher.SendDataToClients sw2_2");
                    _perfAnlzr.CheckLim(sw2_3.ElapsedMilliseconds, _perfSendDataToClient, "CTradersDispatcher.SendDataToClients sw2_3");
                }
                else
                {

                    _tcpServer.SendMessage(connId, arrMsg);

                }
             
             
            }
            catch (Exception e)
            {
                Error("CTradersDispatcher. Serialization", e);

            }

        }


       /*
        private  bool  _bIsFirstScan = false;
        private  bool _bIsWorkingMode = false;
        private DateTime _dtTimeFirstScan = new DateTime(0);

        private void CheckLim(double val, double maxVal, string msg)
        {
            if (!_bIsFirstScan)
            {
                _bIsFirstScan = true;
                _dtTimeFirstScan = DateTime.Now;
            }
            else
            {

            }

            if (val>maxVal)
            {
                string outMsg = "CTradeDispatcher ElapsedMs more than lim "+msg+"val ="+val+"maxVal="+maxVal+" ";
                Error(outMsg);
            }

        }
        */

         /// <summary>
		/// Note: packing end sending in parallel thread, not using blocking queue -
		/// triggering from parallel updater
         /// </summary>
         /// <param name="isin"></param>
        public void SendUpdateTradersDeals(string isin)
        {
            List<CRawDeal> lstDeals = _client.DealBox.DealsStruct[isin].ListDeals;
           // List<CDealClass> lstDealsOut = new List<CDealClass>();

            CDealsList lstDealsOut = new CDealsList (isin);

            //lock (lstDeals)
            lock (_client.DealBox.DealsStruct[isin].ListDeals)
            {
                while (lstDeals.Count!=0)
                {
                    lstDealsOut.DealsList .Add(new CDealClass
                                        {
                                            Amount = lstDeals[0].Amount,
                                            Price = lstDeals[0].Price,
                                            DirDeal = lstDeals[0].GetDealDir(),
                                            DtTm = lstDeals[0].Moment,                                            
                                           // Isin = isin ,
                                           // Id = lstDeals[0].ReplID
                                        });
                    lstDeals.RemoveAt(0);
                }
            }
            if (lstDealsOut.DealsList.Count != 0)
            {
                lstDealsOut.DtBeforePack = DateTime.Now;

              
                SendDataToClients(lstDealsOut, enmTradingEvent.DealsUpdate,isin);

           
            }

        }
       /// <summary>
       /// Generates user orders for traders
       /// 
       /// Call from processRawOrdLogStruct
       /// </summary>    
        public void EnqueueUserOrdersUpdate(int traderId, CBotBase bb)
        {


            int connId = -1;
            lock (_dictBotIdConnId)
            {
                //update 2017-02-07
                if (!_dictBotIdConnId.ContainsKey(traderId))
                    return;

                connId = _dictBotIdConnId[traderId];
            }
            if (connId != -1)
            {
                CUserOrdersUpdate ob = new CUserOrdersUpdate();
                ob.MonitorOrders = new Dictionary<string,Dictionary<long,COrder>>();

                lock (bb.MonitorOrdersAll)
                {

                    foreach (var v in bb.MonitorOrdersAll)
                    {
                        string isin = v.Key;
                        ob.MonitorOrders[isin] = new Dictionary<long, COrder>();
                        foreach (var t in v.Value)
                        {
                            long orderId=  t.Key;
                            COrder ord = t.Value;
                            ob.MonitorOrders[isin][orderId] = (COrder) ord.Copy();

                        }
                    }
                }

                _bqTraderData.Add(new CTradingData
                {
                    ConnId = connId,
                    Data = (object)ob,
                    Event = enmTradingEvent.UserOrdersUpdate
                }
                 );
                //SendDataToClients(ob, enmTradingEvent.UserOrdersUpdate, "", connId);
                _dbg.DBGUserordersUpdate(traderId, ob);
            }
        }
      
        Stopwatch sw1 = new Stopwatch();
        Stopwatch sw2 = new Stopwatch();
        Stopwatch sw3 = new Stopwatch();
        Stopwatch sw4 = new Stopwatch();

        private void ThreadFunc()
        {
            while (true)
            {
                try
                {
                    CTradingData tc = _bqTraderData.GetElementBlocking();
                    
                    sw1.Reset();sw1.Start();
                    sw2.Reset();sw2.Start();
                    sw3.Reset();sw3.Start();

                    bool isReady = _client.IsStockOnline && _client.IsDealsOnline;
                    //2018-02-20 change to 1
                    _bqTraderData.CheckLimit(10, isReady , 1, (IAlarmable)_client, "CTradersDispatcher");

                    sw2.Stop();

					if (enmTradingEvent.AuthResponse == tc.Event)
						SendUpdateAuthResponse(tc.ConnId, (CAuthResponse)tc.Data);

					else if (enmTradingEvent.UserUpdatePositionMonitor == tc.Event)
						SendUpdateUserPosMonitor(tc.ConnId, (CUserPosMonitorUpdate)tc.Data);

					else if (enmTradingEvent.UserOrdersUpdate == tc.Event)
						SendUpdateUserOrders(tc.ConnId, (CUserOrdersUpdate)tc.Data);

					else if (enmTradingEvent.UserUpdatePosLog == tc.Event)
						SendUpdateUserPosLog(tc.ConnId, (CUserPosLogUpdate)tc.Data);

                    else if (enmTradingEvent.UpdateUserPosLogLate == tc.Event)
                        SendUpdateUserPosLogLate(tc.ConnId, (CUserPosLogUpdLate)tc.Data);

                    else if (enmTradingEvent.UserUpdateDealsLog == tc.Event)
						SendUpdateUserDealsLog(tc.ConnId, (CUserDealsLogUpdate)tc.Data);

					else if (enmTradingEvent.UserUpdateVM == tc.Event)
						SendUpdateTotalVM(tc.ConnId, (CUserVMUpdate)tc.Data);

					else if (enmTradingEvent.UserUpdateAvailableTickers == tc.Event)
						SendUpadateAvailableTickers(tc.ConnId, (CAvailableTickers)tc.Data);

					else if (enmTradingEvent.UpdateMoneyData == tc.Event)
						SendUpadateAccountMoney(tc.ConnId, (CUserMoney)tc.Data);

					else if (enmTradingEvent.StopLossTakeProfitAccepted == tc.Event)
						SetStopLossTakeProfitAccepted(tc.ConnId, (CSetOrder)tc.Data);

					else if (enmTradingEvent.UpdateInstrumentParams == tc.Event)
						SendUpdateInstrumentsParams(tc.ConnId, (CUpdateInstrumentParams)tc.Data);

                    sw1.Stop();

                    //2017-11-02 removed
                   /* if (sw1.ElapsedMilliseconds > 10)
                    {
                        System.Threading.Thread.Sleep(1);
                    }
                    */
                  

                }
                catch (Exception e)
                {
                    Error("CTradersDispatcher.ThreadFunc",e);
                }
            }

        }

        private void SendUpdateAuthResponse(int connId,CAuthResponse authResponse)
        {

            SendDataToClients(authResponse, enmTradingEvent.AuthResponse, "", connId);            
        }


        private void SendUpdateUserPosMonitor(int connId, CUserPosMonitorUpdate userPosUpdate)
        {
            SendDataToClients(userPosUpdate, enmTradingEvent.UserUpdatePositionMonitor, "", connId);
        }

        private void SendUpdateUserPosLog(int connId, CUserPosLogUpdate userPosLogUpdate)
        {
            SendDataToClients(userPosLogUpdate, enmTradingEvent.UserUpdatePosLog, "", connId);
        }

        private void SendUpdateUserPosLogLate(int connId, CUserPosLogUpdLate userPosLogUpdLate)
        {
            SendDataToClients(userPosLogUpdLate, enmTradingEvent.UpdateUserPosLogLate, "", connId);
        }




        private void SendUpdateUserDealsLog(int connId, CUserDealsLogUpdate userDealsLog)
        {
            SendDataToClients(userDealsLog , enmTradingEvent.UserUpdateDealsLog, "", connId);

        }

        private void SendUpdateTotalVM(int connId, CUserVMUpdate userUpdateVM)
        {

            SendDataToClients(userUpdateVM, enmTradingEvent.UserUpdateVM, "", connId);

        }

        


        private void SendUpadateAvailableTickers(int connId, CAvailableTickers availTickers)
        {
            SendDataToClients(availTickers, enmTradingEvent.UserUpdateAvailableTickers, "",connId);
         
        }

        private void SendUpadateAccountMoney(int connId,  CUserMoney accMoney)
        {
            SendDataToClients(accMoney, enmTradingEvent.UpdateMoneyData, "", connId);

        }

        private void SetStopLossTakeProfitAccepted(int connId, CSetOrder stopLossTakeProfit)
        {
            SendDataToClients(stopLossTakeProfit, enmTradingEvent.StopLossTakeProfitAccepted, "", connId);

        }

	   //this is mostly for Bitfinex
		private void SendUpdateInstrumentsParams(int connId, CUpdateInstrumentParams updateInstrumentParams)
		{
			SendDataToClients(updateInstrumentParams, enmTradingEvent.UpdateInstrumentParams,updateInstrumentParams.Instrument);//broadcast
		}









        private void SendUpdateUserOrders(int connId, CUserOrdersUpdate userOrdersUpdate)
        {
            SendDataToClients(userOrdersUpdate, enmTradingEvent.UserOrdersUpdate, "", connId);

        }

        private bool IsQueueLarge(string instr, Direction dir, int prec, CStock[] sourceList)
        {

            double parMaxSizePcnt = 30; //30
            double parMaxAddRemove = 20; //10

            double cntAddRemove = 0;

            int sizeStock = sourceList.Length;

            

            double cntQueue = _client.SnapshoterStock.OutputStocks[instr].QueueCmdStockExch[dir][prec].Count;
           

            var res = _client.SnapshoterStock.OutputStocks[instr].QueueCmdStockExch[dir][prec].FindAll(el => el.Code == EnmStockChngCodes._02_Add ||
                                                                                           el.Code == EnmStockChngCodes._03_Remove );
            if (res != null)            
                cntAddRemove = res.Count;

            if (cntQueue / sizeStock * 100 > parMaxSizePcnt || cntAddRemove / sizeStock > parMaxAddRemove)
                return true;

           



            return false;

        }


        /// <summary>
        /// Copy from Snapshoter's Stock (or cmd queue)
        /// to current stock (local copy whichs
        ///  is send to clients).
        /// Current stock is sending to traders in
        /// "SendUpdateTradersStock" metthod.
        /// 
        /// Call from SendUpdateTradersStock
        /// </summary>
        /// <param name="instr"></param>
        public void CopyStock(string instr)
        {                               
            lock (_client.SnapshoterStock.OutputStocks[instr].Lck)
            {

                try
                {
                    

                    var sharedStock = _client.SnapshoterStock.OutputStocks[instr];
                    bool bSnapshotCopied = false;
                    

                    //per one dir
                    foreach (var kvp in sharedStock)
                    {

                        foreach (int prec in _client.GetPricePrecisions())
                        {
                            try
                            {
                                Direction dir = kvp.Key;
                                var lstStock = kvp.Value[prec];

                                List<CStock> currentList = null;
                                if (dir == Direction.Down)
                                    currentList = _currentStocksList[instr].StockListBids[prec];
                                else if (dir == Direction.Up)
                                    currentList = _currentStocksList[instr].StockListAsks[prec];

                                int cntQueue = _currentStocksList[instr].QueueCMDStockChng[dir][prec].Count;

                              
                                //If enough time left, or to much "stock change commands" in the queue
                                //Do Update using snapshot.
                                if ( (DateTime.Now - _dictDtLastStockSnapSent[instr][dir][prec]).TotalMilliseconds >
                                     _snapshotIntervalMs || IsQueueLarge(instr, dir, prec, lstStock)
                                       )                                     
                                {
                                    //Copy from shared Snapshoter's Stock to current stock 
                                    //(local copy whichs is send to clients)
                                    CopyOneDirStock(instr, currentList, lstStock);
                                    _dictDtLastStockSnapSent[instr][dir][prec] = DateTime.Now;
                                }
                                else   //Do Copy commands from one queue to another
                                {   
                                    //before copy do clear all data in output queue                               
                                    currentList.Clear();
                                    foreach (var cmd in _client.SnapshoterStock.OutputStocks[instr].QueueCmdStockExch[dir][prec])
                                        _currentStocksList[instr].QueueCMDStockChng[dir][prec].Add(cmd);
                                }
                                _currentStocksList[instr].LstStockConf = 
                                    new List<CStockConf>(sharedStock.LstStockConf);


                                //_currentStocksList[instr].QueueCMDStockChng[prec].Clear();

                                

                                _dbgStockUpd.PrintQueue(instr, prec, dir, _currentStocksList[instr].QueueCMDStockChng[dir][prec]);
                                //after copy clear all data in input queue
                                _client.SnapshoterStock.OutputStocks[instr].QueueCmdStockExch[dir][prec].Clear();

                               
                                   


                            }
                            catch (Exception exc)
                            {
                                Thread.Sleep(0);
                            }
                                                                                                               

                        }
                     
                       
                    }

                 

                    _dbgStockUpd.PrintStock(instr, _currentStocksList[instr]);
                }

                catch (Exception e)
                {

                    Error("CopyStock", e);

                }
               

            }//end lock
        
        }



        private void CopyOneDirStock(string isin, List<CStock> outputCurrentStock, CStock[] sourceStock)
        {


            outputCurrentStock.Clear();

            foreach (CStock stk in sourceStock)
                outputCurrentStock.Add(new CStock(stk.Price, stk.Volume));

        }


        public void CallbackNewConnection(int conId)
        {
            //TODO auth etc...
            _dictConnIdTrader[conId] = new CTrader();


            
        }


        private bool IsPassedAuth(CAuthRequest req)
        {
            
                       
           return     _dbCommunicator.LoginRequest(req.User, req.Password);
           
        }





        private void ProcessAuthRequest(int conId,byte[] arrMsgBody)
        {
            //TODO auth here
            int botId = -1;
            CAuthRequest areq = CUtilProto.DeserializeProto<CAuthRequest>(arrMsgBody);
            if (!IsPassedAuth(areq))
            {
                EnqueueSendAuthResponse(false, "Пользователь и пароль не найдены", conId);
                Log(String.Format("User {0} auth failed",areq.User));
            }

            else
            {
                botId = Convert.ToInt16(areq.User);

                _dictConnIdTrader[conId].BotId = botId;
                _dictBotIdConnId[botId] = conId;
                EnqueueSendAuthResponse(true, "", conId);

                _dictConnIdTrader[conId].IsLoggedOn = true;

                foreach (CBotBase bb in _client.ListBots)
                    if (bb.BotId == botId)
                    {
                        _dictKBotIdVBotTrader[botId] = (CBotTrader)bb;
                        bb.Recalc("", EnmBotEventCode.OnTraderConnected, null);
                        break;
                    }

                Log(String.Format("Auth success user={0} conId={1}",areq.User,conId));

            }
           // Log("AuthRequest botId=" + botId);


        }
      


        private void ProcessAddOrder(int conId, byte[] arrMsgBody)
        {
            CAddOrder sc = CUtilProto.DeserializeProto<CAddOrder>(arrMsgBody);
           
            _dictKBotIdVBotTrader[_dictConnIdTrader[conId].BotId].AddOrderByTrader(sc.Isin, sc.Price, sc.Dir, sc.Amount);
            Log("Add order bot_id=" + _dictConnIdTrader[conId].BotId + " isin=" +
                    sc.Isin + " dir=" + sc.Dir + " Price=" + sc.Price + " Amount=" + sc.Amount);

        }



        private void ProcessCancellOrderById(int conId, byte[] arrMsgBody)
        {
            CCancellOrderById co = CUtilProto.DeserializeProto<CCancellOrderById>(arrMsgBody);
            //_plaza2Connector.CancelOrder( co.Id, _dictConnIdTrader[conId].BotId);

            _dictKBotIdVBotTrader[_dictConnIdTrader[conId].BotId].CancelOrder(co.Id);

            _dbg.DBGCancellOrderById(_dictConnIdTrader[conId].BotId,co);
            
        }


        private void ProcessCancellAllOrders(int conId, byte[] arrMsgBody)
        {

            CCancellAllOrders coa = CUtilProto.DeserializeProto<CCancellAllOrders>(arrMsgBody);

            _dictKBotIdVBotTrader[_dictConnIdTrader[conId].BotId].CancellAllBotOrders();
            _dbg.DBGCancellAllOrders(_dictConnIdTrader[conId].BotId);
        }

        private void ProcessCancellOrdersByIsin(int conId, byte[] arrMsgBody)
        {

            CCancellOrderByIsin coa = CUtilProto.DeserializeProto<CCancellOrderByIsin>(arrMsgBody);
            _dictKBotIdVBotTrader[_dictConnIdTrader[conId].BotId].CancellOrdersWithInstrumenByTrader(coa.Isin);

            _dbg.DBGCancellAllOrdersByIsin(_dictConnIdTrader[conId].BotId, coa.Isin);
        }

        private void ProcessCloseAllPositionsByIsin (int conId,byte[] arrMsgBody)
        {

                CCloseAllPositionsByIsin cap = CUtilProto.DeserializeProto<CCloseAllPositionsByIsin>(arrMsgBody);
                _dictKBotIdVBotTrader[_dictConnIdTrader[conId].BotId].ClosePositionOfInstrumentByTrader  (cap.Isin);

                 _dbg.DBGCloseAllPositionByIsin(_dictConnIdTrader[conId].BotId, cap.Isin);
        }

        private void ProcessCloseAllPositions(int conId, byte[] arrMsgBody)
        {
            CCloseAllPositions cap = CUtilProto.DeserializeProto<CCloseAllPositions>(arrMsgBody);
            GetBotTrader(conId).CloseAllPositions();
            _dbg.DBGCloseAllPositions(GetBotTrader(conId).BotId);
        }




        private void ProcessUserSubscribeTicker(int conId, byte[] arrMsgBody)
        {

            CSubscribeTicker subscrTicks = CUtilProto.DeserializeProto<CSubscribeTicker>(arrMsgBody);
            _dictConnIdTrader[conId].SubscribeIsin(subscrTicks);


			//2017-01-15 experimental logics temporary disabled

			/*
            CBotTrader bot = _dictKBotIdVBotTrader[_dictConnIdTrader[conId].BotId];

            foreach (var cmd in subscrTicks.ListSubscribeCommands)
            {
                if (cmd.Action == EnmSubsrcibeActions.Subscribe)
                    bot.OnSubscribeInstrument(cmd.Ticker);
                else if (cmd.Action == EnmSubsrcibeActions.UnSubscribe)
                    bot.OnUnSubscribeInstrument(cmd.Ticker);

                 

            }
            */



        }

        private void ProcessTypeOrder(int conId, byte[] arrMsgBody)
        {
            CSetOrder sltp = CUtilProto.DeserializeProto<CSetOrder>(arrMsgBody);
            _dictKBotIdVBotTrader[_dictConnIdTrader[conId].BotId].SetTypeOrderByTrader(sltp.Instrument,  sltp.OrderType, sltp.Price, sltp.Amount);
        }

		private void ProcessSendOrderThrow(int conId, byte[] arrMsgBody)
		{
			CSendOrderThrow sot = CUtilProto.DeserializeProto<CSendOrderThrow>(arrMsgBody);
			_dictKBotIdVBotTrader[_dictConnIdTrader[conId].BotId].SendOrderThrowByTrader(sot.Instrument, sot.OrderDir, sot.Amount, sot.ThrowSteps);
		}

		private void ProcessInvertPosition(int conId, byte[] arrMsgBody)
		{
			CInvertUserPos cup = CUtilProto.DeserializeProto<CInvertUserPos>(arrMsgBody);
			_dictKBotIdVBotTrader[_dictConnIdTrader[conId].BotId].InvertPositionByTrader(cup.Instrument);
		}

        private void PorcessOrderRest(int conId, byte[] arrMsgBody)
        {
            CDataRestOrder ro = CUtilProto.DeserializeProto<CDataRestOrder>(arrMsgBody);

            _dictKBotIdVBotTrader[_dictConnIdTrader[conId].BotId].AddRestOrder(ro.Instrument, ro.Price, ro.Dir);
            
        }

        private void ProcessClientInfo(int conId, byte[] arrMsgBody)
        {
            CClientInfo ci = CUtilProto.DeserializeProto<CClientInfo>(arrMsgBody);


            ci.ConId = conId;
            ci.BotId = GetBotId(conId);
            ci.DtConnection = DateTime.Now;
            ci.Ip = _tcpServer.GetClientIp(conId);

            Log(String.Format("Client info. ConId={0} ip={1} botId={2} version={3} instance={4}",
                                conId,  //0
                                 _tcpServer.GetClientIp(conId),
                                GetBotId(conId),//1
                                ci.Version,//2
                                ci.Instance//3
                                
                                ));

            _client.AddClientInfo(ci);
            //_dictKBotIdVBotTrader[_dictConnIdTrader[conId].BotId].AddRestOrder(ro.Instrument, ro.Price, ro.Dir);

        }

        private int GetBotId(int conId)
        {
            return _dictConnIdTrader[conId].BotId;
        }

        private CBotTrader GetBotTrader(int conId)
        {
            return _dictKBotIdVBotTrader[GetBotId(conId)];
        }



        public void CallbackReadMessage(int conId,byte[] message)
        {
            byte byteTradingEvent = 0;
          

            byte[] arrMsgBody = _messenger.GetBinaryMessageHeaderAndBody(message, ref  byteTradingEvent);
            enmTradingEvent tradingEvent = (enmTradingEvent) byteTradingEvent;


            Log("Read message from client");

            if (enmTradingEvent.AuthRequest == tradingEvent)
                ProcessAuthRequest(conId, arrMsgBody);

            //TODO command as bot not directly
            else if (enmTradingEvent.AddOrder == tradingEvent)
                ProcessAddOrder(conId, arrMsgBody);

            else if (enmTradingEvent.CancellOrderById == tradingEvent)
                ProcessCancellOrderById(conId, arrMsgBody);


            else if (enmTradingEvent.CancellAllOrders == tradingEvent)
                ProcessCancellAllOrders(conId, arrMsgBody);


            else if (enmTradingEvent.CancellOrdersByIsin == tradingEvent)
                ProcessCancellOrdersByIsin(conId, arrMsgBody);

            else if (enmTradingEvent.CloseAllPositions == tradingEvent)
                ProcessCloseAllPositions(conId, arrMsgBody);

            else if (enmTradingEvent.CloseAllPositionsByIsin == tradingEvent)
                ProcessCloseAllPositionsByIsin(conId, arrMsgBody);


            else if (enmTradingEvent.UserSubscribeTicker == tradingEvent)
                ProcessUserSubscribeTicker(conId, arrMsgBody);

            else if (enmTradingEvent.SetStoplossTakeProfit == tradingEvent)
                ProcessTypeOrder(conId, arrMsgBody);

            else if (enmTradingEvent.SendOrderThrow == tradingEvent)
                ProcessSendOrderThrow(conId, arrMsgBody);


            else if (enmTradingEvent.InvertPosition == tradingEvent)
                ProcessInvertPosition(conId, arrMsgBody);

            else if (enmTradingEvent.SendOrderRest == tradingEvent)
                PorcessOrderRest(conId, arrMsgBody);

            else if (enmTradingEvent.ClientInfo == tradingEvent)
                ProcessClientInfo(conId, arrMsgBody);


            else
            {
                throw new ApplicationException("Unknown message from client");


            }

          }

    }
}
