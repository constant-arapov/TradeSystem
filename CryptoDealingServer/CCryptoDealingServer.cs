using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.IO;


using Common.Utils;

using TradingLib;
using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Interaction;
using TradingLib.Abstract;
using TradingLib.Enums;
using TradingLib.BotEvents;
using TradingLib.Data;
using TradingLib.Data.DB;
using TradingLib.Common;
using TradingLib.Common.VMCalc;
using TradingLib.Bots;



using ComonentFactory;

using DBCommunicator;
using DBCommunicator.Interfaces;


using BitfinexCommon;
using BitfinexCommon.Enums;
using BitfinexCommon.Messages.Response;


using BitfinexWebSockConnector;
using BitfinexWebSockConnector.Interfaces;

using BitfinexRestConnector.Messages.v1.Response;

using BitfinexRestConnector;

using CryptoDealingServer.Interfaces;
using CryptoDealingServer.Components;

using CryptoDealingServer.Helpers;


namespace CryptoDealingServer
{
    public class CCryptoDealingServer : CBaseDealingServer,
                                        IClientBfxWebSockCon, IClientStockConverterCrypto, IClientDBCommunicator,
                                        IClientDealBox, IClientTimeFrameAnalyzer, IClientStockBox, IClientPosBox,
                                        IClientPositionsBoxCrypto, IDataFormatForCntrlInstr, IClientUserOrderBoxCrypto,
                                        IClientUserDealsPosBoxCrypto, IClientSessionBoxCrypto, IClientMoneyTracker,
                                        IClientTradeHistStorV2, IClientOrdersHistStor

    {


        public override bool IsStockOnline { get; set; }
        public override bool IsSessionOnline { get; set; }
        public override bool IsPossibleToCancelOrders { get; set; }
        public override bool IsSessionActive { get; set; }
        public override bool IsPositionOnline { get; set; }
        public override bool IsOrderControlAvailable { get; set; }
        public override bool IsOnlineVM { get; set; }
        public override bool IsOnlineUserOrderLog { get; set; }
        public override bool IsOnlineUserDeals { get; set; }
        public override bool IsFutInfoOnline { get; set; }
        public override bool IsDealsOnline { get; set; }
        public override bool IsAnalyzerTFOnline { get; set; }

        public override bool IsPossibleNativeCancellOrdByInstr { get; set; }

        private CTradeHistStorV2 _tradeHistStorV2;




        private COrdersHistStor _ordersHistStor;

        private List<int> _pricePrecisionsCrypto =  new List<int>()
            {
                 0,
                 1,
                 2/*,
                 3*/

            };

    public override bool IsReadyStartTrdMgrServ
        {
            get
            {
                return true;
            }

        }



        private CUserOrderBoxCrypto _userOrderBoxCrypto;

        public override IUserOrderBox UserOrderBox
        {
            get
            {
                return _userOrderBoxCrypto;
            }
        }



        private CUserDealsPosBoxCrypto _userDealsPosBoxCrypto;

        public override IUserDealsPosBox UserDealsPosBox
        {
            get
            {
                return _userDealsPosBoxCrypto;
            }
        }

        private CStockBoxCrypto _stockBoxCrypto;
        public override IStockBox StockBox
        {
            get
            {
                return _stockBoxCrypto;
            }
        }

        CPosBoxCrypto _posBoxCrypto;
        public override IPositionBox PositionBox
        {
            get
            {
                return _posBoxCrypto;
            }

        }

        public override CBasePosBox PosBoxBase
        {
            get
            {
                return _posBoxCrypto;
            }

        }






        private CDealboxCrypto _dealBoxCrypto;

        public override IDealBox DealBox
        {
            get
            {
                return _dealBoxCrypto;
            }

        }

        public override DateTime ServerTime
        {
            get
            {

                return DateTime.UtcNow;
            }


        }

        private CComponentFactory _componentFactory;

        private string _bfxAPIKey;
        private string _bfxAPISecret;




        //private List<string> _lstInstruments = new List<string>();
        private List<CCryptoInstrData> _lstInstruments = new List<CCryptoInstrData>();





        private CBitfenixWebSockConnector _bfxWebSockConnector;
        private CBitfenixWebSockConnector _bfxWebSockConnectorPublic;

        private bool _bIsActiveConnectorV1 = true;
        private CBitfinexRestConnectorV1 _bfxRestConnectorV1;
        private CBitfinexRestConnectorV2 _bfxRestConnectorV2;

        //flag to init all instruments (at first start)
        private bool _bfxNeedInitInstruments = false;

        private AutoResetEvent _evServerRunning = new AutoResetEvent(false);

        private List<ResponseTrades> _lstDealsWithNoFee = new List<ResponseTrades>();
        private List<ResponseTrades> _lstNotProcessedDeals = new List<ResponseTrades>();


        /// <summary>
        /// List for remembering deals with unknown BotID 
        /// (which where recieved ealrlier than orderId) was recieved
        /// for future processing
        /// </summary>
        private List<ResponseTrades> _lstDealsNoBotId = new List<ResponseTrades>();

        private CSessionBoxCrypto _sessionBoxCrypto;

        public override ISessionBox SessionBox
        {
            get
            {
                return _sessionBoxCrypto;
            }

        }


        private CTradeHistStor _tradeHistStor = null;

        private CMoneyTracker _moneyTracker;


        /// <summary>
        ///Dependecy fee coefs from turnover
        /// </summary>
        private List<CDBTurnoverFee> _lstTurnOverFeeDep = new List<CDBTurnoverFee>();


        public List<CDBTurnoverFee> LstTurnOverFeeDep
        {
            get
            {
                return _lstTurnOverFeeDep;

            }
            



        }




        public CCryptoDealingServer()
            : base("CryptoDealingServer")
        {
            _componentFactory = new CComponentFactory(this);

            StockExchId = CodesStockExch._04_CryptoBitfinex;

            IsPossibleEmptyInstrCancellOrders = true;
            IsAddTraderOnlineAllowed = true;


            _numOfStepsForMarketOrder = 500;

          

        }

        /*
        public override bool IsStockAvailable(string instrument)
        {
            return true;
        }
        */



        public override void Process()
        {
            //2018-06-27 
            CUtil.IncreaseProcessPriority();

           //2018-06-28 TODO make configurable or instruments num dependent
           ThreadPool.SetMinThreads(130, 130);

            base.Process();
            SetServerTimeAvailable();//just force for now

            _userDealsPosBoxCrypto.CreateDictBotDealId(ListBots);





            //TODO from DB etc
            Instruments.WaitInstrumentsLoaded();
            //_lstInstruments = GenerateListInstruments();
            _lstInstruments = Instruments.GetCryptoInstrDataList();



            //CreateStockConverters();

            CreateSnapshoters();
            _stockBoxCrypto = new CStockBoxCrypto(this, 100, this);

            _dealBoxCrypto = new CDealboxCrypto(this);
            IsDealsOnline = true;
            EvDealsOnline.Set();

            _posBoxCrypto = new CPosBoxCrypto(this);
            EvPosOnline.Set();//TODO normal

            CreateTCPServerAndTradersDispatcher();
            _tradeHistStorV2 = new CTradeHistStorV2(this);
            _ordersHistStor = new COrdersHistStor(this);
            


            //TODO from config
            //  _bfxAPIKey = "62NvrsDVwXDryVsGRU9uVkeDpYNdsnvTHfFnUGVVEsP";
            // _bfxAPISecret = "oNl3hdW0dxGtwN9UDSNNzNk74rzqgequpOcLuwtmNYz";

            _bfxAPIKey = "fVgyf0Rk4hDDDdXAzys7yN0vnGcRPUVYRTLoOxTyDIL";
            _bfxAPISecret = "MDqroztPvZzFIaKKspozdyeAD274OFAZnEZy2nv3eUE";

            CUtil.ThreadStart(ThreadBfxRestV1);
            CUtil.ThreadStart(ThreadBfxRestV2);
            CUtil.ThreadStart(ThreadOneSecondLogics);
            CUtil.ThreadStart(Thread100MsLogics);

            _bfxWebSockConnector = new CBitfenixWebSockConnector(this,
                                                                _lstInstruments,
                                                                true, //isAuth
                                                                _bfxAPIKey,
                                                                _bfxAPISecret);

            _bfxWebSockConnector.Process();


            _bfxWebSockConnectorPublic = new CBitfenixWebSockConnector(this,
                                                              _lstInstruments,
                                                              false, //isAuth
                                                              _bfxAPIKey,
                                                              _bfxAPISecret);



            _bfxWebSockConnectorPublic.Process();

            IsAllBotLoaded = true;
            IsOnlineUserOrderLog = true;
            IsOnlineUserDeals = true;


            StartTradeManagerServer();

            EnableBotLogics();

           
            
            _evServerRunning.WaitOne();

        }


        public override List<int> GetPricePrecisions()
        {
            return _pricePrecisionsCrypto;


        }





        public override decimal GetOrdersBacking(string instrument, decimal price,
                                                    decimal amount)
        {
            return price * amount;
        }




        public void CheckUnProcessedDeals()
        {


        }

        public override decimal GetStepPrice(string instrument)
        {
            //Just for capability with BotPos. Bitfinex cryptostock doesn't
            //use it for calculation. So it is dummy.
            return 0;
        }



        public void UpdatePriceDecimals(string instrument, int newPriceDecimals)
        {
            var res = Instruments.Find(el => el.instrument == instrument);
            if (res != null)
            {
                Log(String.Format("Changed pirce decimals {0} {1} --> {2}", instrument, res.RoundTo, newPriceDecimals));
                res.RoundTo = newPriceDecimals;


                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-us");
                DBCommunicator.UpdateRoundTo(instrument, StockExchId, newPriceDecimals);
            }
        }




        public void UpdateCurrentMinSteps(string instrument, decimal newMinSteps)
        {
            var res = Instruments.Find(el => el.instrument == instrument);
            if (res != null)
            {
                Log(String.Format("Changed min_step  {0} {1} --> {2}", instrument, res.Min_step, newMinSteps));

                res.Min_step = newMinSteps;

                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-us");
                DBCommunicator.UpdateMinStep(instrument, StockExchId, newMinSteps);
            }
        }



        public void TriggerUpdateInstrumentParams(string instrument)
        {

            decimal minStep = GetMinStep(instrument);
            int decimals = GetDecimals(instrument);
            int decimalVolume = GetDecimalVolume(instrument);
            decimal minOrderSize = GetMinOrderSize(instrument);

            _tradersDispatcher.EnqueUpdateInstrumentParams(instrument, minStep, decimals, decimalVolume, minOrderSize);
            //EnqueUpdateInstrumentParams

        }






        public int GetPriceFormat(string instrument)
        {

            /*   var res = Instruments.Find(el => el.instrument == instrument);
               if (res != null)
               {
                   if (res.DecimalVolume == 0)
                       return 0;
                   else
                       return 5 - res.DecimalVolume - 1;

               }


               return 0;*/
            return GetCurrentPriceDecimals(instrument);

        }


        public int GetCurrentPriceDecimals(string instrument)
        {
            var res = Instruments.Find(el => el.instrument == instrument);
            if (res != null)
            {
                return res.RoundTo;
            }
            return 0;
        }

        public int GetVolumeFormat(string instrument)
        {
            var res = Instruments.Find(el => el.instrument == instrument);
            if (res != null)
            {
                return res.DecimalVolume;
            }

            return 0;

        }





        public void ThreadBfxRestV1()
        {
            try
            {

                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-us");
                _bfxRestConnectorV1 = new CBitfinexRestConnectorV1(this, _bfxAPIKey, _bfxAPISecret);

                WaitDataLoadedFromDB();
                Instruments.WaitInstrumentsLoaded();

                while (_bIsActiveConnectorV1)
                {

                    try
                    {
                        ProcessSymbolDetails();
                        //Personal data - perform only on
                        //real server. To protect nonce problem
                        if (GlobalConfig.UseRealServer)
                        {
                            //if (_tradeHistStor != null)
                              //  ProcessMyTrades();
                        }

                        System.Threading.Thread.Sleep(20000);
                        //System.Threading.Thread.Sleep(30 * 60 * 1000); //tempo test 
                    }
                    catch (Exception e)
                    {
                        Error("ThreadBfxRestV1", e);
                    }

                }


            }
            catch (Exception e)
            {
                Error("ThreadBfxRestV1", e);
            }

        }




        public void ThreadBfxRestV2()
        {
            if (!_globalConfig.UseRealServer)
                return;


            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-us");
            _bfxRestConnectorV2 = new CBitfinexRestConnectorV2 (this, _bfxAPIKey, _bfxAPISecret);

            WaitDataLoadedFromDB();
            Instruments.WaitInstrumentsLoaded();
            _ordersHistStor.LoadDataFromDB();
            _tradeHistStorV2.LoadDataFromDB();


            int parSleepTime = 2000;

            while (true)
            {
                try
                {
                    //TODO save each response exec time 
                    ProcessRestV2Trades();
                    Thread.Sleep(parSleepTime);
                    ProcessRestV2Orders();
                    Thread.Sleep(parSleepTime);


                }
                catch (Exception e)
                {
                    Error("ThreadBfxRestV2",e);
                    //2018-11-15
                    Thread.Sleep(parSleepTime);
                }



            }





        }


        public void ThreadOneSecondLogics()
        {

            IsServerTimeAvailable = true;

            while (true)
            {
                try
                {
                    GUIBox.ServerTime = ServerTime;

                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Error("ThreadOneSecondLogics", e);
                    Thread.Sleep(1000);
                }


            }



        }

        public void Thread100MsLogics()
        {

            DBCommunicator.WaitReadyForOperations();
            _moneyTracker = new CMoneyTracker(this);
            _moneyTracker.LoadDataOnStart();

            while (true)
            {
                try
                {

                    _moneyTracker.Process();

                    Thread.Sleep(100);
                }
                catch (Exception e)
                {
                    Error("Thread100MsLogics",e);
                    Thread.Sleep(100);
                }





            }





        }



        





        public void PeriodicActBfxAuth()
        {
            CheckNotPorcessedTrades();
            _userOrderBoxCrypto.CleanUserOrderLog();
        }






        private void CheckNotPorcessedTrades()
        {
            int parOffsetTimeHr = 3;
            int parMaxOldDataSec = 20;

            //strange logics may be remove in the future
            foreach (var el in _lstNotProcessedDeals)
            {
                if (el.Fee == 0)
                {
                    Log(String.Format("CheckNotPorcessedTrades Zero fee DealId={0}", el.Id));
                    DateTime dtDeal = CUtilTime.DateTimeFromUnixTimestampMillis(el.MtsCreate);

                    if ((DateTime.Now.AddHours(parOffsetTimeHr) - dtDeal).TotalSeconds > parMaxOldDataSec)
                    {

                        Error(String.Format("CheckNotPorcessedTrades Zero fee DealId ={0} dt={1}",
                                                el.Id,  //0
                                                dtDeal  //1
                                                ));

                        //2018-06-13 changed
                        //double fee = _tradeHistStor.GetFee(el.Pair, el.Id);
                        double fee = _tradeHistStorV2.GetFee(el.Pair, el.Id);

                        if (fee != 0)
                        {
                            el.Fee = fee;
                            UpdateUserDealsLateUpd(el);
                            Log("Fee was processed. Remove from _lstNotProcessedDeals");
                            _lstNotProcessedDeals.RemoveAll(deal => deal.Id == el.Id);
                            return;
                        }
                        else
                        {
                            Log("CheckNotPorcessedTrades. Fee not found or zero");
                        }


                    }
                }

            }


            List<long> lstIdRemove = new List<long>();

           lock (_lstDealsWithNoFee)
           {
                foreach (var el in _lstDealsWithNoFee)
                {

                    Log(String.Format("Processing with no fee id={0}",
                                                    el.Id));

                    double fee = _tradeHistStorV2.GetFee(el.Pair, el.Id);

                    if (fee != 0) 
                    {

                        el.Fee = fee;
                        
                        Log(String.Format("Getting Fee from rest DealID={0} fee={1}",
                                            el.Id, fee));

                        UpdateUserDealsLateUpd(el);
                        lstIdRemove.Add(el.Id);


                    }
                    else
                    {
                        Log("CheckNotPorcessedTrades. Fee not found or zero");
                    }

                }
                //remove all processed
                foreach (var id in lstIdRemove )
                {
                    Log(String.Format("Remove from _lstDealsWithNoFee DealID={0}", id));                                           
                    _lstDealsWithNoFee.RemoveAll(deal => deal.Id == id);

                }

            }

          




        }


        private void ProcessSymbolDetails()
        {


            Log("GetSymbolDetails start");
            ResponseSymbolDetails[] resp = _bfxRestConnectorV1.GetSymbolDetails();
            Log("GetSymbolDetails end");


            //--- initial fill of DB (on very first start only)
            if (_bfxNeedInitInstruments)
            {
                for (int i = 0; i < resp.Count(); i++)
                {
                    string instrument = resp[i].pair;
                    string instrUpper = instrument.ToUpper();
                    //for now intrested only in Xusd pairs
                    if (instrument.Contains("usd"))
                    {

                        DBCommunicator.TransactAddInstrument(instrUpper, CodesStockExch._04_CryptoBitfinex);
                    }

                }
                return;
            }
            //--- end of initial DB fill

            foreach (var dbIsntr in Instruments)
            {

                for (int i = 0; i < resp.Count(); i++)
                {
                    decimal decMinOrdSize = Convert.ToDecimal(resp[i].minimum_order_size);



                    if (resp[i].pair == dbIsntr.instrument.ToLower() && decMinOrdSize != dbIsntr.minimum_order_size)
                    {
                        dbIsntr.minimum_order_size = decMinOrdSize;

                        int oldDecimalVolume = dbIsntr.DecimalVolume;

                        dbIsntr.DecimalVolume = CUtilConv.GetRoundTo(dbIsntr.minimum_order_size);

                        DBCommunicator.UpdateCryptoInstrumentData(resp[i].pair,
                                                                  CodesStockExch._04_CryptoBitfinex,
                                                                  decMinOrdSize,
                                                                  dbIsntr.DecimalVolume);

                        Log(String.Format("Update minimum_order_size={0} {1}", dbIsntr.minimum_order_size, dbIsntr.instrument));

                        if (oldDecimalVolume != dbIsntr.DecimalVolume)
                        {
                            _bfxWebSockConnector.UpdateDecimalVolume(dbIsntr.instrument, dbIsntr.DecimalVolume);
                            Log(String.Format("Update DecimalVolume {0} {1}", dbIsntr.DecimalVolume, dbIsntr.instrument));

                        }



                    }


                }
            }

            if (_tradeHistStor == null)
                _tradeHistStor = new CTradeHistStor(Instruments.GetInstruments(), DBCommunicator);



        }

        public void ProcessMyTrades()
        {
            //TEMPO for DEBUG, TODO normally from DB
            List<string> _lastInstrumets = new List<string>()
            {
               "BCHUSD",
               "BTCUSD",
               "EDOUSD",
               "EOSUSD",
               "ETHUSD",
               "IOTUSD",
               "LTCUSD",
               "NEOUSD",
               "XRPUSD",
               "ZECUSD"

            };

            foreach (string instrument in _lastInstrumets)
            {
                ResponceMyTrades[] rmt = _bfxRestConnectorV1.GetMyTrades(instrument);
                if (rmt != null)
                    _tradeHistStor.OnRcvdNewHistory(instrument, rmt);

                //_bfxRestConnectorV1.DbgPrintMyTrades(instrument,rmt);
                Thread.Sleep(1500);
            }

        }


        public void ProcessRestV2Trades()
        {
           ResponseTrades[] rt =   _bfxRestConnectorV2.GetTrades();
            if (rt!=null)
               _tradeHistStorV2.Update(rt);

        }


        public void ProcessRestV2Orders()
        {
            ResponseOrders[] ro = _bfxRestConnectorV2.GetOrderHistory();

            if (ro != null)
                _ordersHistStor.Update(ro);



        }

















        //long gid, string symbol, long id, EnmOrderTypes orderType, double price, EnmOrderStatus enmOrderStatus
        public void ProcessOrder(ResponseOrders respOrders, EnmOrderAction ordAction)
        {
            _userOrderBoxCrypto.ProcessOrder(respOrders, ordAction);

            //_userOrderBoxCrypto.ListRawOrdersStruct
        }






        public void UpdateStockBothDir(string instrument, int precision, CSharedStocks stock)
        {
            _stockBoxCrypto.UpdateStockConverterBothDir(instrument, precision, stock);

        }

        public void UpdateStockOneDir(string instrument, Direction dir, int precision, CSharedStocks stock)
        {
            _stockBoxCrypto.UpdateStockConverterOneDir(instrument, dir, precision, stock);

        }





        public void UpdateDeal(string instrument, CRawDeal rd)
        {
            _dealBoxCrypto.Update(instrument, rd);
        }







        //TODO get from dealing server
        private List<string> GenerateListInstruments()
        {

            List<string> lstInstruments = new List<string>();

            lstInstruments.Add("tBTCUSD");
            lstInstruments.Add("tLTCUSD");
            lstInstruments.Add("tETHUSD");
            lstInstruments.Add("tZECUSD");
            lstInstruments.Add("tXMRUSD");
            //_lstInstruments.Add("tDASHUSD");
            //_lstInstruments.Add("tIOTAUSD");
            lstInstruments.Add("tEOSUSD");
            lstInstruments.Add("tSANUSD");
            lstInstruments.Add("tOMGUSD");
            lstInstruments.Add("tBCHUSD");
            lstInstruments.Add("tNEOUSD");
            //  _lstInstruments.Add("tUTPUSD");
            //_lstInstruments.Add("tQTUMUSD");
            lstInstruments.Add("tEDOUSD");
            lstInstruments.Add("tAVTUSD");

            return lstInstruments;


        }




        protected override void StartGateIfNeed()
        {

        }


        public override bool IsPossibleToAddOrder(string instrument)
        {
            //2018-02-23 tempoary always enabled
            //may be will be somthing more complex 
            //in the future
            return true;
        }

        public override bool IsReadyForRecalcBots()
        {
            //changed 2018-03-22
            return true;
        }

        public override bool IsPriceInLimits(string instrument, decimal price)
        {
            //2018-02-23 tempoary always enabled
            //may be will be somthing more complex 
            //in the future
            return true;
        }


        protected override void CreateUserOrderBox()
        {
            _userOrderBoxCrypto = new CUserOrderBoxCrypto(this);

        }


        protected override void CreateUserDealsPosBox()
        {
            //TODO accept - build or not
            _userDealsPosBoxCrypto = new CUserDealsPosBoxCrypto(this, CBaseVMCalc.CreateCryptoVMCalc(), bBuildNonSavedPositionsFromDealsLog: true);
        }

        



        protected override void CreateSessionBox()
        {
            _sessionBoxCrypto = new CSessionBoxCrypto(this);
            _sessionBoxCrypto.Process();
        }


        protected override void CreateExternalComponents()
        {

            _componentFactory.Create(GlobalConfig.DatabaseName, this);
        }



        public override void CancelOrder(long orderId, int botId)
        {
            _bfxWebSockConnector.CancellOrder(orderId);
        }



        public override void CancelAllOrders(int buy_sell, int ext_id, string isin, int botId)
        {
            _bfxWebSockConnector.CancellAllOrders(botId);
            // _bfxWebSockConnector.CancellOrder(8831641920);
        }




        public override void AddOrder(int botId, string instrument, decimal price, EnmOrderDir dir, decimal amount)
        {

            //int decVolume = GetDecimalVolume(instrument);
            //decimal decAmount = CUtilConv.GetDecimalVolume(amount, decVolume);

            //TODO normal
            string instrumentUse = "t" + instrument;

            //temporary disabled
            _bfxWebSockConnector.AddOrder(botId, instrumentUse, dir, amount, price);

        }

        public override void FillDBClassField<T>(Dictionary<string, object> row, T outObj)
        {
            CMySQLConnector.FillClassFields(row, outObj);
        }



        public override int GetGUITradeSystemVolume(string instrument, string inpVolume)
        {
            decimal decVol = Convert.ToDecimal(inpVolume);
            int decVolume = GetDecimalVolume(instrument);
            int intVolume = (int)CUtilConv.GetIntVolume(decVol, decVolume);

            return intVolume;
        }

        //deprecated brunch remove if not need
        public void UpdateDealsPos(string instrument, decimal price, EnmOrderDir dir,
                           int amount, long extId,
                           DateTime moment, long mtsCreate, decimal fee)
        {


            _userDealsPosBoxCrypto.Update(instrument, price, dir, amount, extId, moment, mtsCreate, fee);



        }


        /// <summary>
        /// Calling on "te" (TradeExecute) bifinex events.
        /// Ask UserOrderBoxCrypto to get botId based on OrderId. 
        /// If BotId found -do call Update if not do save to
        /// _lstNotProcessedDeals for using this data later.
        /// 
        /// 
        /// Call from CBitfinexWebSockConnector.ProcessUserTradeExecute
        /// </summary>
        /// <param name="rt"></param>
		public void UpdateUserDeals(ResponseTrades rt)
        {
            Log("[UpdateUserDeals] " + rt.ToString());

            EnmOrderDir dir = rt.ExecAmount > 0 ? EnmOrderDir.Buy : EnmOrderDir.Sell;

            string instrument = rt.Pair.Remove(0, 1);//remove 't'
                                                     //long iAmount = CUtilConv.GetIntVolume(Convert.ToDecimal(rt.ExecAmount), GetDecimalVolume(instrument));
            decimal dcmlAmount = Convert.ToDecimal(rt.ExecAmount);
            DateTime moment = CUtilTime.DateTimeFromUnixTimestampMillis(rt.MtsCreate);

            int botId = _userOrderBoxCrypto.GetBotIdOfOrder(instrument, rt.OrderId);

            if (botId > 0) //botId found
            {
                Log("[UpdateUserDeals] botId=" + botId);
                _userDealsPosBoxCrypto.Update(rt.Id,
                                            rt.OrderId,
                                            instrument,
                                            dir,
                                            dcmlAmount,
                                            botId,
                                            Convert.ToDecimal(rt.ExecPrice),
                                            moment,
                                            rt.MtsCreate,
                                            (decimal)rt.Fee);
               
                //2018-06-13 common situation - remember that no fee
                //to remove it if fee will be recieved and get from rest if not
                if ( (decimal) rt.Fee == 0)
                {
                    lock (_lstDealsWithNoFee)
                        _lstDealsWithNoFee.Add(rt);
                }

            }
            else //botId not found
            {
                //Write error for now (debugging). In the future - remove error msg.
                Error("UpdateUserDeals. Bot id not found");
                //Put to list of deals to process it later (on "tu")
                _lstNotProcessedDeals.Add(rt);

                //add 2018-05-24
                AddDealNoBotId(rt);
                // _lstDealsNoBotId.Add(rt);

            }

        }


        /// <summary>
        /// Call when:
        /// 
        /// Trade update  message triggered. Possible two cases:
        /// 
        /// 1) When we've not recieved trade execute message - do full update
        /// 2) When we already recieved Trade execute message (TE) - just update fee
        /// 
        /// Call from:
        /// 
        /// CBitfinexWeebSockConnector.ProcessUserTradeUpdate
        /// 
        /// 
        /// </summary>
        /// <param name="rt"></param>
        public void UpdateUserDealsLateUpd(ResponseTrades rt)
        {
            Log("[UpdateUserDealsLateUpd] " + rt.ToString());
            EnmOrderDir dir = rt.ExecAmount > 0 ? EnmOrderDir.Buy : EnmOrderDir.Sell; //TODO change to deal dir
            EnmDealDir dealDir = rt.ExecAmount > 0 ? EnmDealDir.Buy : EnmDealDir.Sell; //TODO change to deal dir

            string instrument = rt.Pair.Remove(0, 1);//remove 't'
            decimal dcmlAmount = Convert.ToDecimal(rt.ExecAmount);
            DateTime moment = CUtilTime.DateTimeFromUnixTimestampMillis(rt.MtsCreate);

            int botId = _userOrderBoxCrypto.GetBotIdOfOrder(instrument, rt.OrderId);

            //fee recieved no worry about "no fee" deals
            if (rt.Fee != 0)
            {
                lock (_lstDealsWithNoFee)
                {
                    _lstDealsWithNoFee.RemoveAll(el => el.Id == rt.Id);
                }
            }

            //if deal is not processed by "trade execute message" yet -
            //do full update.

            bool bNotProcessedYet = (_lstNotProcessedDeals.Find(el => el.Id == rt.Id) != null);

            //Deal was not processed yet - do full update
            if (bNotProcessedYet)
            {

                if (botId > 0) //botId found
                {
                    Log("[UpdateUserDealsLateUpd]. First processing deal. botId=" + botId);
                    _userDealsPosBoxCrypto.Update(rt.Id,
                                            rt.OrderId,
                                            instrument,
                                            dir,
                                            dcmlAmount,
                                            botId,
                                            Convert.ToDecimal(rt.ExecPrice),
                                            moment,
                                            rt.MtsCreate,
                                            (decimal)rt.Fee);

                    //deal processed no need update it - do remove
                    _lstNotProcessedDeals.RemoveAll(el => el.Id == rt.Id);
                }
                else //botId not found - now having a REAL problem
                {
                    Error("CAUTION ! UpdateUserDealsLateUpd. BotId not found !");
                    //2018-05-14 remember deals with no botId for future processing 
                    //2018-05-24 refact to funct
                    AddDealNoBotId(rt);
                }

            }
            //if deal has already  processed by "trade execute message" just update fee data
            else
            {
                Log("[UpdateUserDealsLateUpd] Update fee");
                _userDealsPosBoxCrypto.UpdateFee(botId,
                                                 instrument,
                                                 rt.Id,
                                                 Math.Abs(Convert.ToDecimal(rt.Fee)),
                                                 dealDir
                                                 );
                //TODO update fee if order if not found

            }
        }

        private void AddDealNoBotId(ResponseTrades rt)
        {
            if (_lstDealsNoBotId.Find(el => el.Id == rt.Id) == null)
            {
                _lstDealsNoBotId.Add(rt);
                Log(String.Format("Remeber deal with no BotId orderId={0}", rt.OrderId));
            }

        }



        public void UpdatePos(ResponsePositions respPos)
        {
            string instrument = respPos.Symbol.Remove(0, 1);
            _posBoxCrypto.Update(instrument, Convert.ToDecimal(respPos.Amount), respPos.Status);


        }


        public void UpdateUserPosLogLate(CDBUpdateLate dbUpdateFee)
        {
            DBCommunicator.QueueData(dbUpdateFee);
            _tradersDispatcher.EnqueueUpdateUserPosLogLate(dbUpdateFee.BotId,
                    new TradingLib.ProtoTradingStructs.CUserPosLogUpdLate()
                    {
                        Instrument = dbUpdateFee.Instrument,
                        Fee = dbUpdateFee.Fee,
                        Fee_Total = dbUpdateFee.Fee_Total,
                        VMClosed_RUB = dbUpdateFee.VMClosed_RUB,
                        VMClosed_RUB_user = dbUpdateFee.VMClosed_RUB_user

                    }
                        );
        }


        //Added 2018_03_23
        public void EnableBotLogics()
        {
            TriggerRecalcAllBots(EnmBotEventCode.OnUserOrdersOnline, null);

            // Thread.Sleep(10000);

            TriggerRecalcAllBots(EnmBotEventCode.OnUserDealOnline, null);
            TriggerRecalcAllBots(EnmBotEventCode.OnUserOrdersOnline, null);

            TriggerRecalcAllBots(EnmBotEventCode.OnPositionOnline, null);


        }


        /// <summary>
        /// Check for deals previously remembered in _lstDealsNoBotId.
        /// If found order Id than call processing deal (late deal).
        /// 
        /// Call from:
        ///   ProcessOrder (Cancell or delete case)
        ///   2018-06-07 process "add" order as well
        ///   
        /// Added 2018-05-13
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="BotId"></param>
        public void CheckForDealsWithNoBotId(long orderId, int BotId)
        {
            // _userDealsPosBoxCrypto.CheckForNonProcessedDeals(orderId, BotId);
            bool bFound = false;
            
            foreach (var rt in _lstDealsNoBotId)
            {
                if (rt.OrderId == orderId)
                {
                    Log(String.Format("CheckForDealsWithNoBotId. Found deal for OrderId={0}. Call UpdateUserDealsLateUpd",
                                        rt.OrderId));
                    UpdateUserDealsLateUpd(rt);
                    bFound = true;
                }

            }

            if (bFound)
            {
                _lstDealsNoBotId.RemoveAll(el => el.OrderId == orderId);
                Log(String.Format("CheckForDealsWithNoBotId. Remove orderId. OrderId={0}. Call UpdateUserDealsLateUpd", orderId));
            }

                 

            //TODO remove from list

        }


        public void OnClearingProcessed()
        {
            _userOrderBoxCrypto.CleanFullUserOrderLog();

            _moneyTracker.OnClearingProcessed();

        }

        public void UpdateFeeUserDealsLog(CDBUpdateFeeUserDealsLog dbUpdFeeUserDealsLog)
        {

            DBCommunicator.QueueData(dbUpdFeeUserDealsLog);

        }

        public long GetBotIdBfxRest(long id)
        {

            return _ordersHistStor.GetBotIdByOrderId(id);
        }

        public override int GetStockDepth(int precision)
        {
            return precision == 0 ? 25 : 100;

        }

        
        public decimal GetFeeDealingPcnt(int accountId)
        {

            return this.AccountsTrade[accountId].proc_fee_dealing / 100;          
        }
        


        public decimal GetFeePcntLim(int accountId)
        {
            lock (AccountsTrade)
                return this.AccountsTrade[accountId].proc_fee_turnover_limit / 100;
        }

        public decimal GetFeePctMarket(int accountId)
        {
            lock (AccountsTrade)
                return this.AccountsTrade[accountId].proc_fee_turnover_market / 100;

        }



        public override void LoadDataFromDB()
        {
            base.LoadDataFromDB();


            //load fee 
            List <CDBTurnoverFee> lst  = DBCommunicator.LoadTurnoverFeesCoef();
            _lstTurnOverFeeDep = new List<CDBTurnoverFee>(lst);


        }





        public void UpdateFeeTurnoverCoefs()
        {


            List<CDBTurnoverFee> lstDbFeesToUpdate = new List<CDBTurnoverFee>();
            
            //iterate turnovers
            _lstTurnOvers.ForEach(el =>
            {
                CDBTurnoverFee dbTrn =  CalcFeeFromTrunOver(el.turnover);
                //find accountTradeId in accounts
                foreach (var kvp in AccountsTrade)
                    if (kvp.Key == el.account_trade_Id)
                    {
                        bool bChanged = false;
                        if (kvp.Value.proc_fee_turnover_limit != dbTrn.proc_fee_turnover_limit)
                        {
                            Log(String.Format("accountTradeId={0} change proc_fee_turnover_limit {1} => {2}",
                                kvp.Key,
                                kvp.Value.proc_fee_turnover_limit,
                                dbTrn.proc_fee_turnover_limit));

                            kvp.Value.proc_fee_turnover_limit = dbTrn.proc_fee_turnover_limit;

                            bChanged = true;
                        }

                        if (kvp.Value.proc_fee_turnover_market != dbTrn.proc_fee_turnover_market)
                        {

                            Log(String.Format("accountTradeId={0} change proc_fee_turnover_market {1} => {2}",
                               kvp.Key,
                               kvp.Value.proc_fee_turnover_market,
                               dbTrn.proc_fee_turnover_market));

                            kvp.Value.proc_fee_turnover_market = dbTrn.proc_fee_turnover_market;

                            bChanged = true;

                        }

                        



                        if (bChanged)
                        { 
                            kvp.Value.proc_fee_turnover_limit = dbTrn.proc_fee_turnover_limit;
                            kvp.Value.proc_fee_turnover_market = dbTrn.proc_fee_turnover_market;

                            dbTrn.account_money_id = kvp.Key;

                            lstDbFeesToUpdate.Add(dbTrn);

                            break;
                        }
                    }
            }
            );

            if (lstDbFeesToUpdate.Count>0)
                DBCommunicator.UpdateTradersFeeProc(lstDbFeesToUpdate);
            



        }

       


        public CDBTurnoverFee CalcFeeFromTrunOver(decimal inpTurnover)
        {
            CDBTurnoverFee dBTurnoverFee = null;

            int i = _lstTurnOverFeeDep.Count-1;
            do
            {
                dBTurnoverFee = _lstTurnOverFeeDep[i];

            }
            while (i-- >= 0 && inpTurnover <= dBTurnoverFee.turnover);

            return dBTurnoverFee.Copy();

        }

        public override void OnTrdMgrSentReconnect(int channelId)
        {
            //TODO normaly
           if   (channelId == 1)
           {
               _bfxWebSockConnectorPublic.ForceReaconnect();
               
           }

        }






    }
}
