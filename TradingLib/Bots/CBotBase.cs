using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

using Common;
using Common.Interfaces;
using Common.Logger;


using System.Threading;


using TradingLib.Interfaces;
using TradingLib.BotEvents;

using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Data;
using TradingLib.Enums;
using TradingLib.ProtoTradingStructs;
using TradingLib.ProtoTradingStructs.TradeManager;
using TradingLib.GUI;



namespace TradingLib.Bots
{
     [Serializable]
    public class CBotBase  :CBaseProppertyChanged, IAlarmable, ILogable, IDisposable, IClientBotRiskManager
    {


        System.Collections.Concurrent.BlockingCollection<CBotEventStruct> EventsQueue {get;set;}// = new System.Collections.Concurrent.BlockingCollection<CBotEventStruct>();

        private /*CAlarmer*/IAlarmable m_alarmer;
        protected object m_currState;

        public Dictionary<string, CBotMarketData> MonitorMarketDataAll = new Dictionary<string, CBotMarketData>();
        public Dictionary<string, CBotPos> MonitorPositionsAll { get; set; }// = new Dictionary<string, CBotPos>();

    
         /// <summary>
         ///instrument - id -  COrder>
         ///Note: need Lock !
         /// </summary>
        public Dictionary<string, Dictionary<long, COrder>> MonitorOrdersAll = new Dictionary<string, Dictionary<long, COrder>>();


		protected CMonitorOrdersAllTotal _monitorOrdersAllTotal = new CMonitorOrdersAllTotal();


		//public Dictionary<string, Dictionary<decimal, int>> MonitorOrdersPrice



		private Dictionary<string, bool> _dictIsClosingPos = new Dictionary<string, bool>();



        public Dictionary<string, string> SettingsStrategy {set;get;}


        public bool IsExternal { get; set; }

        public AppDomain AppDomainBot { get; set; }
        private Mutex mxRecalc = new Mutex();

        private System.Threading.Thread m_mainThread;

        public bool IsSupervisor { get; set; }

       // protected bool IsOneInstrumentBot { get; set; }

        public bool NeedTFAnalyzer { get; set; }

        protected decimal  m_dictVMBotSessionTotalAllInstruments;

        //protected object m_currState;

        private string _name;


        private CBotRiskManager _riskManager;

        private CBotStatus _botStatus;
        private CTraderInfoSummary _traderInfoSummary;

       

        public string Name 
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                RaisePropertyChanged("Name");

            }
            
        }
    
        public int BotId   {set; get;}

        protected /*CPlaza2Connector*/IDealingServer _dealingServer;

        //TODO get from parameters, TODO increase
        private int m_parPeriodicActionsMS = 1000;


   
        protected CLogger m_logger;
        protected CLogger m_loggerStates;
        protected CLogger m_loggerMonitor;

        public CLogger Logger { get { return m_logger; } }
        public CSettingsBot SettingsBot { get; set; }



        protected int par_MaxOrderSentCount = 1;
        protected int m_currOrderSentCountTotal;


        public  /*CGUIBot*/ IGUIBot GUIBot { get; set; }


        protected Dictionary<string, int> m_dictSentOrderCount = new Dictionary<string, int>();

        List<CRawOrdersLogStruct> m_listLocalRawOrdersStructs = new List<CRawOrdersLogStruct>();
        List<CRawPosition> m_listLocalRawPositions = new List<CRawPosition>();
 

        List<CRawUserDeal> m_listLocalRawUserDeals = new List<CRawUserDeal>();
        List<CRawUserDeal> m_listLocalBotUserDeals = new List<CRawUserDeal>();



        protected Dictionary<string, Mutex> mxCurrentStocks = new Dictionary<string, Mutex>();

        protected Dictionary<string, object> lckCurrentStocks = new Dictionary<string, object>();


        protected Dictionary<string, Dictionary<Direction, List<CStock>>> m_currentStocks =
             new Dictionary<string, Dictionary<Direction, List<CStock>>>();

        
     
        protected  Dictionary<string,CTradeTimer> m_timers = new Dictionary<string,CTradeTimer>();


        protected  bool m_disableBot = false;


        protected Dictionary<string, decimal> _bid = new Dictionary<string, decimal>();
        protected Dictionary<string, decimal> _ask = new Dictionary<string, decimal>();


       // public bool 



        public bool DisabledBot
        {
            get
            {
                return m_disableBot;
            }
            set
            {

                m_disableBot = value;
               
                RaisePropertyChanged("DisabledBot");
            }
        }



        protected Dictionary<string, CRawPosition> m_dictPosAllBotsSummary = new Dictionary<string, CRawPosition>();

        protected Dictionary <string, decimal> m_dictVMOpenedAndClosed = new  Dictionary <string, decimal> ();
        public Dictionary<string, decimal> DictVMOpenedAndClosed
        {
            get
            {
                return m_dictVMOpenedAndClosed;
            }

        }



        public  Dictionary<string, decimal> DictVMBotSessionTotalClosed {get;set;}//  = new Dictionary<string, decimal>();
        public decimal VMAllInstrClosed { get; set; }
        public decimal VMAllInstrOpenedAndClosed { get; set; }
        public decimal VMAllInstrOpened { get; set; }
        private decimal _oldVmAllInstrOpenedAndClosed = 0;
        private decimal _prevDbInstrTotOpenedClosed = 0;


        protected System.Threading.Mutex mxDictPosLocal = new System.Threading.Mutex();

               

        private bool SelfTerminated = false;


        private bool m_userOrdersOnlineRecieved = false;
        private bool IsUserDealsOnlineRecieved = false;
        private bool m_positionOnlineRecieved = false;


        public DateTime ServerTime
        {
            get
            {
                return _dealingServer.ServerTime;
            }
        }
		public string BotSubDir
		{
			get
			{
				return String.Format(@"Bots\{0}", Name);
			}
		}


       
        public CBotBase()
        {



        }


        public CBotBase(int botId, string name, CSettingsBot settingsBot, Dictionary<string, string> settingsStrategy,
                                                                                    IDealingServer plaza2Connector)
                                                                                   
        {
            Init(botId, name, settingsBot, settingsStrategy, plaza2Connector);
            this.IsSupervisor = true;



        }
         /*
        public void Dispose()
        {


        
            //   GC.SuppressFinalize(this);      


        }
         */






        public virtual void Init(int botId, string name, CSettingsBot settingsBot, Dictionary<string, string> settingsStrategy,
                                                                                   /*CPlaza2Connector*/IDealingServer dealingServer)
        {
            Name = name;
            SettingsBot = settingsBot;
            SettingsStrategy = settingsStrategy;
            IsExternal = SettingsBot.IsExternal;
            NeedTFAnalyzer = settingsBot.NeedTFAnalyzer;
            BotId = botId;
            _dealingServer = dealingServer;


            _botStatus = new CBotStatus() { BotId = botId };
            _traderInfoSummary = new CTraderInfoSummary {StockExchId  = _dealingServer.StockExchId,  BotId = botId };


           
            MonitorPositionsAll = new Dictionary<string, CBotPos>();

			GUIBot = /*new CGUIBot(plaza2Connector, BotId);*/dealingServer.CreateGUIBot(dealingServer, BotId);
            m_alarmer = dealingServer.Alarmer;
            CreateMarketData();

            m_logger = new CLogger(Name, flushMode:true, subDir: BotSubDir);
			m_loggerStates = new CLogger(Name + "_states", flushMode: true, subDir: BotSubDir);
            m_loggerMonitor = new CLogger(Name + "_monitor", flushMode: true, subDir: BotSubDir);


            string stStartMessage = "STARTING BOT " + Name;
            CBotHelper.PrintBanner(m_logger, stStartMessage);
            CBotHelper.PrintBanner(m_loggerStates, stStartMessage);
            CBotHelper.PrintBanner(m_loggerMonitor, stStartMessage);



            DictVMBotSessionTotalClosed = new Dictionary<string, decimal>();
            LoadParameters();




            EventsQueue = new System.Collections.Concurrent.BlockingCollection<CBotEventStruct>();


            CreateTimers();

            /*
            foreach (KeyValuePair<string, Dictionary<Direction, List<CStock>>> kvp in m_currentStocks)
            {
                string isin = kvp.Key;
                m_currentStocks[isin][Direction.Up] = new List<CStock>();
                m_currentStocks[isin][Direction.Down] = new List<CStock>();
            }*/


			settingsBot.ListIsins.ForEach((instr) => _dictIsClosingPos[instr] = false);

            if (!SettingsBot.Enabled)
                //DisableBot();
                DisabledBot = true; // on start do it with no alarming


            _riskManager = new CBotRiskManager(_dealingServer, this);
            //_dealingServer.UpdateBotStatusTrdMgr(_botStatus);
            //UpdateTrdMgr();
        }


		



        public void Dispose()
        {


            DisableBot("On dispose");

            GUIBot.Dispose();
            GUIBot = null;
            

            //TO DO in the future - something more clever
            //transactionable etc

          //  m_mainThread.Abort();
            while (m_mainThread.IsAlive)
                System.Threading.Thread.Sleep(10);

            //tempo remove
          //  Thread.Sleep(5000);
         //   GC.Collect();
           // GC.WaitForPendingFinalizers();


        }
        ~CBotBase()
        {
            Console.WriteLine("CBotBase was destryed");

        }


         /*
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
         */

        public void Error(string description, Exception excp=null)
             
        {
          _dealingServer.Alarmer.Error(description, excp);

        }



        /// <summary>
        /// Calculate VM closed, total (oened+closed)
        /// Calculate per instrument and  for all instruments
        /// 
        ///Call from: 
        ///1) OnUserDealOnlineData
        ///2) UpdateMonitorPosisionsAll
        ///3) OnStockUpdateData (TODO optimize it)
        ///4) From event OnForceUpdTotalVM
        /// </summary>
        private void UpdateTotalVm()
        {
            //added 2017-11-07
            if (!_dealingServer.IsOnlineUserDeals || !_dealingServer.IsStockOnline)
                return;

            decimal sumAllClosed = 0;
            decimal sumAllOpened =0;
            decimal sumAllOpenedAndClose = 0;

           

            foreach (string isin in SettingsBot.ListIsins)
            {
                decimal sumClosed = 0;
                decimal sumOpened = 0;
                bool bFoundClosed = false;

                bool bFoundOpened = false;

               

                //changed 2017-11-08
                decimal sumOpenedAndClose = 
                    _dealingServer.UserDealsPosBox.CalcBotVMOpenedAndClosed(BotId, isin, ref sumClosed,ref bFoundClosed, ref sumOpened);

                

                //2018-01-22 update opened and close no metter found in closed or not
                sumAllOpenedAndClose += sumOpenedAndClose;
                sumAllOpened += sumOpened;

                //if no vm on instrument do  not record it
                if (bFoundClosed)
                {
                    m_dictVMOpenedAndClosed[isin] = sumOpenedAndClose;
                    
                    sumAllClosed += sumClosed;

                    lock (DictVMBotSessionTotalClosed)
                    {
                        DictVMBotSessionTotalClosed[isin] = sumClosed;
                       
                    }
                }
               
               

               
            }

            VMAllInstrClosed = sumAllClosed;
            VMAllInstrOpenedAndClosed = sumAllOpenedAndClose;
            VMAllInstrOpened = sumAllOpened;

            UpdateVmOpenCloseTrdMgr();

            UpdateDBVmOpenClose();

            //tempo debug
            //   if (BotId == 101)
            //   Thread.Sleep(0);




            //VMBotAllInstrOpenedAndClosed = 

        }

        //2018_08_27
        public void ForcePosOnlineRecieved()
        {
            m_positionOnlineRecieved = true;
        }

        public void ResetIsClosingPos(string instrument)
        {
            _dictIsClosingPos[instrument] = false;

        }


        /// <summary>
        /// Call from:
        /// 
        /// 1) UpdateTotalVm()
        /// </summary>
        private void UpdateVmOpenCloseTrdMgr()
        {

        
            //TO DO inherit
            if (SettingsBot.StrategyCode == enmStrategysCode.StrategyTrader)
            {
                if (VMAllInstrOpenedAndClosed != _oldVmAllInstrOpenedAndClosed)
                {
                    //_traderInfoSummary.VMAllInstrOpenedAndClosed = VMAllInstrOpenedAndClosed;
                    _botStatus.VMAllInstrOpenedAndClosed = VMAllInstrOpenedAndClosed;
                    _dealingServer.UpdateBotStatusTrdMgr(_botStatus);
                }

                _oldVmAllInstrOpenedAndClosed = VMAllInstrOpenedAndClosed;
            }

        }


        public void UpdateMaxLossVMClosedTotal(decimal maxLossVMClosedTotalnew)
        {

            Log(String.Format("Changed MaxLossVMClosedTotal {0}=>{1}",
                            SettingsBot.MaxLossVMClosedTotal, maxLossVMClosedTotalnew));

            SettingsBot.MaxLossVMClosedTotal = maxLossVMClosedTotalnew;

        }




        private bool _updDBVmFirstTime = true;

        private void UpdateDBVmOpenClose()
        {
            if (SettingsBot.StrategyCode == enmStrategysCode.StrategyTrader)
                if (VMAllInstrOpenedAndClosed != _prevDbInstrTotOpenedClosed || _updDBVmFirstTime)
                {
                    _dealingServer.UpdDBVMOpenedClosedTot(BotId, VMAllInstrOpenedAndClosed);
                    _updDBVmFirstTime = false;
                }
            _prevDbInstrTotOpenedClosed = VMAllInstrOpenedAndClosed;
        }

        private void UpdateDBPosInstr(decimal amount, decimal avPos)
        {
            if (SettingsBot.StrategyCode == enmStrategysCode.StrategyTrader)
            {



            }


        }



        protected void AddTimer(string name, double expTimeMS = 1000)
        {
            m_timers[name] = new CTradeTimer(name, this, EnmBotEventCode.OnTimer,  expTimeMS);
        }


        protected void AddTimerSpecific(string name, EnmBotEventCode code, double expTimeMS)
        {
            m_timers[name] = new CTradeTimer(name, this, code, expTimeMS);
        }

        protected void StartTimer(string name)
        {
            m_timers[name].Set();
        }

        protected void StopTimer(string name)
        {
            m_timers[name].Reset();
        }

       /* public bool TimerExpired(string name)
        {
            return m_timers[name].IsExpired;

        }
         */

        public void Log(string message)
        {
            m_logger.Log(message);

        }

        private void LogMonitor(string message)
        {
            m_loggerMonitor.Log(message);
        }

        private bool IsPossibleToCancelOrders()
        {
     
            if (!_dealingServer.SessionBox.IsPossibleCancellOrders())
            {
                Log("Not Possible to cancel orders");
                
                return false;
            }
             

            return true;
        }


        private bool IsSentOrderCountExceedsLim(string isin)
        {
            if (m_dictSentOrderCount.ContainsKey(isin))
            {
                if (!SettingsBot.DictBotIsinLimits.ContainsKey(isin))
                {
                    Error(String.Format("No order counts limit bot={0} instr={1}", BotId, isin));                    
                    return true;
                }
                else //ContainsKey
                    if (m_dictSentOrderCount[isin] >= SettingsBot.DictBotIsinLimits[isin].MaxSendOrderRuntime)
                    {
                        //2018-04-25
                        //remove this condition as we trying achieve 24x7 mode
                        /*
                        string err = "Unable to Add order.  Exceded max order count. BotName=" + Name + 
                            " OrderSentCount=" + m_dictSentOrderCount[isin] +
                            " par_MaxOrderSentCount=" + SettingsBot.DictBotIsinLimits[isin].MaxSendOrderRuntime +
                            " bot=" + this.Name;
                        Log(err);
                        Error(err);
                        return true;*/
                    }

            }

            return false;
        }

        private decimal GetCostOfAllOfers()
        {
            decimal sum = 0m;
            lock(MonitorOrdersAll)
            {
                foreach (var kvp in MonitorOrdersAll)
                {
                    foreach(var kvp2 in kvp.Value)
                    {
                        decimal price = kvp2.Value.Price;
                        decimal amount = kvp2.Value.Amount;

                        //2018-05-22 changed
                        decimal money = _dealingServer.GetOrdersBacking("",price, amount);//price * amount;

                        sum += money;

                    }

                }
                return sum;
            }

            
        }

        private decimal GetCostBalanceOfAllOfers()
        {
            decimal sum = 0m;
            lock (MonitorOrdersAll)
            {
                foreach (var kvp in MonitorOrdersAll)
                {
                    foreach (var kvp2 in kvp.Value)
                    {
                        decimal price = kvp2.Value.Price;
                        decimal amount = kvp2.Value.Amount;
                        int mult = kvp2.Value.Dir == (sbyte)EnmOrderDir.Sell ? -1 : 1;
                        
                        decimal money = mult * _dealingServer.GetOrdersBacking("", price, amount);//price * amount;

                        sum += money;

                    }

                }
                return sum;
            }


        }




        /// <summary>
        ///Calc total cost of opened position of all instruments.        
        /// </summary>
        /// <returns></returns>
        private decimal GetCostOfAllPositions()
        {
            decimal sum = 0m;
            lock (MonitorPositionsAll)
            {
                decimal curCost = 0;
                foreach (var kvp in MonitorPositionsAll)                
                    curCost += Math.Abs(kvp.Value.AvPos * kvp.Value.Amount);


                sum += curCost;

            }

            return sum;


        }

        private decimal GetCostBalanceOfAllPositions()
        {
            decimal sum = 0m;
            lock (MonitorPositionsAll)
            {
                decimal curCost = 0;
                foreach (var kvp in MonitorPositionsAll)
                {                    
                    curCost += kvp.Value.AvPos * kvp.Value.Amount;

                }
                sum += curCost;

            }

            return sum;


        }



        protected bool IsEnoughMoneyToAddOrder(decimal price, decimal amount,  EnmOrderDir dirOrderReq)
        {
            decimal availMoney = _dealingServer.GetAccountTradeMoney(BotId);

            //  decimal costOfReqOrder = price * amount;
            decimal costOfReqOrder = _dealingServer.GetOrdersBacking("", price, amount);


            decimal sumExistOrd = GetCostOfAllOfers();
        
            

            decimal costOfAllPos = GetCostOfAllPositions();

            //changed 2018-12-20
            //2018-12-23 temporary diabled costOfAllPos
            //2018-12-28 bring bag and added extra logics
            decimal costTotal = costOfReqOrder + sumExistOrd + costOfAllPos;
            
            //2018-12-28
            decimal availableMoneyAbs = 2 * availMoney;

            if (costTotal > availableMoneyAbs)
            {
                Error(String.Format("Not Enough money to Add Order. Absolute.  CostTotal={0} availableMoneyAbs ={1}" +
                                     "costOfReqOrder={2} sumExistOrd={3} costOfAllPos={4}", 
                                   costTotal,      //0
                                   availableMoneyAbs,//1
                                   costOfReqOrder, //2
                                   sumExistOrd,    //3
                                    costOfAllPos   //4
                                   ));
                return false;
            }


            //EDITTAG 2018-12-28
            int mult = (dirOrderReq == EnmOrderDir.Sell) ? -1 : 1;

            decimal costBalanceOfReqOrder = mult * _dealingServer.GetOrdersBacking("", price, amount);

            decimal sumExistOrdBalance =  GetCostBalanceOfAllOfers();
            decimal balanceOfAllPos = GetCostBalanceOfAllPositions();

           



            decimal balance = sumExistOrdBalance + balanceOfAllPos + costBalanceOfReqOrder;

            if (Math.Abs(balance) > availMoney)
            {
                Error(String.Format("Not Enough money to Add Order. Balance. CostTotal={0} availMoney={1}" +
                                     "costBalanceOfReqOrder={2} sumExistOrdBalance={3} balanceOfAllPos={4}",
                                   balance,      //0
                                   availMoney,     //1
                                   costBalanceOfReqOrder, //2
                                   sumExistOrdBalance,    //3
                                   balanceOfAllPos   //4
                                   ));
                return false;
            }




            //END 2018-12-28

            return true;
        }

       
        



        /// <summary>
        /// Check for ability to add limit order. If dealing server not allow
        /// do not send order,
        /// If money lmit exceeds do not send order.
        /// 
        /// </summary>     
        protected bool IsPossibleToAddOrder(string instrument, decimal price)
        {
            //Added again 2017-11-30 protect against send order for
            //view-only instrument
            if (!_dealingServer.IsExistBotIdInstrument(BotId, instrument))
            {
                Log("Unable to Add order. Not exist BotId-instrument config");
                return false;

            }
            


            if (!_dealingServer.IsPossibleToAddOrder(instrument))
            {
                Log("Unable to Add order. Session is not active.");
                return false;
            }
             if (IsSentOrderCountExceedsLim(instrument))
                return false;
            


           if (IsVMExceedsLimit())
            {
                string err = "Unable to Add order.  VM exceed limits";

                Log(err);
                Error(err);

                return false;
            }


             if (!_dealingServer.IsPriceInLimits(instrument, price))
                 return false;



            return true;
        }

        private bool IsVMExceedsLimit()
        {
            return _riskManager.IsVMExceedsLimit();
        }


         /// <summary>
         /// Check for ability to limit order. If no ablility
         /// </summary>
        private bool IsPossibleToForceAddOrder(string isin, decimal price)
        {
            //changed 2017-07-18 was IsSessionActive only


            

            if (!_dealingServer.IsPossibleToAddOrder(isin))
            {
                Log("Unable to Add order. Session is not active.");
                return false;
            }

            if (IsSentOrderCountExceedsLim(isin))
                return false;


            if (!_dealingServer.IsPriceInLimits(isin, price))
                return false;


            return true;
        }


        public  bool AddOrder(string isin, decimal price, /*OrderDirection*/EnmOrderDir dir, decimal amount)
        {

            return AddOrder(isin, price, dir, amount, BotId);

        }

         /// <summary>
         /// Add limit order with checking of money limits
         /// 
         /// Call from:
        ///  - AddOrderNearSpread
        ///  - AddMarketOrder
         /// </summary>       
        protected virtual bool  AddOrder(string instrument, decimal price,  /*OrderDirection*/EnmOrderDir dir, decimal amount, int botId)

        {
            //TO DO order controller

            if (IsPossibleToAddOrder(instrument, price))
            {

                _dealingServer.AddOrder(botId, instrument, price, dir, Math.Abs(amount));
                LogAddOrder("AddOrder",  instrument, price,dir,amount);
                m_currOrderSentCountTotal++;
                if (!m_dictSentOrderCount.ContainsKey(instrument)) 
                    m_dictSentOrderCount[instrument] = 0;

                m_dictSentOrderCount[instrument]++;
                m_timers["WaitAddOrderReply"].Set();
                return true;
            }

            
            DisableBot("Order command was not added");
           // Error("Unable to add order");
         
            return false;

        }

        /// <summary>
        /// Adapter for ForceAddOrder
        /// Call from GUI
        /// </summary>        
        public void ForceAddOrder(string instrument, decimal price, /*OrderDirection*/EnmOrderDir dir, decimal amount)
        {
            ForceAddOrder(instrument, price, dir, amount, BotId);

        }

        /// <summary>
        /// Add limit order without checking of money limits
        /// 
        /// Call from:
        /// 
        /// 1) ForceAddOrder (adapter)
        /// 2) ForceAddMarketOrder
        /// 
        /// </summary>      
        protected bool ForceAddOrder(string instrument, decimal price, /*OrderDirection*/EnmOrderDir dir, decimal amount, int botId, int supervisorID = 0)
        {
          if (IsPossibleToForceAddOrder(instrument, price))
          {
              _dealingServer.AddOrder(botId, instrument, price, dir, Math.Abs(amount)/*, supervisorID*/);
              LogAddOrder("ForceAddOrder", instrument, price, dir, amount);

              Log("ForceAddOrder command was executed sucessfully.");
              m_currOrderSentCountTotal++;
              if (!m_dictSentOrderCount.ContainsKey(instrument)) m_dictSentOrderCount[instrument] = 0;
              m_dictSentOrderCount[instrument]++;
              m_timers["WaitAddOrderReply"].Set();
              return true;


          }

          Log("Order command was not added");
          return false;
      }

        protected void LogAddOrder(string command, string instrument, decimal price, EnmOrderDir dir, decimal amount)
        {
            string msg = String.Format("{0} instrument={1} price={2} dir={3} amount={4}",
                                              command, instrument, price, dir, amount);
            //Log("AddOrder command was executed sucessfully.");
            Log(msg);
            LogMonitor(msg);


        }


        protected bool AddOrderNearSpread(string isin, /*OrderDirection*/EnmOrderDir  dir, decimal amount, int stepsOffset)
        {
            return AddOrderNearSpread(isin, dir, amount, stepsOffset, this.BotId);
        }



        protected bool AddOrderNearSpread(string isin, /*OrderDirection*/EnmOrderDir dir, decimal amount, int stepsOffset, int botId)
        {
            try
            {
                if (!_dealingServer.IsStockOnline)
                {

                    Error("Not possible to send order as stock is not online");
                    return false;

                }



                decimal step = _dealingServer.DictInstruments[isin].Min_step;

                decimal offset = stepsOffset * step;




                //TO DO check
                decimal price = _dealingServer.StockBox.GetBestPice(isin, dir);
                //changed 2018_03_23
                decimal maxPrice = _dealingServer.GetMaxPrice(isin); //_dealingServer.DictFutLims[isin].Max;
                decimal minPrice = _dealingServer.GetMinPrice(isin); //_dealingServer.DictFutLims[isin].Min;


                if (price > maxPrice || price < minPrice)
                {

                    Error("Price is out of limits. minPrice=" + minPrice + " maxPrice=" + maxPrice);
                    return false;

                }


                if (dir == /*OrderDirection*/ EnmOrderDir.Buy)
                {
                    price -= offset;
                    price = Math.Min(price, maxPrice);

                }
                else if (dir == /*OrderDirection*/EnmOrderDir .Sell)
                {
                    price += offset;
                    price = Math.Max(price, minPrice);
                }
                AddOrder(isin, price, dir, amount, botId);
            }
            catch (Exception e)
            {
                Error("AddOrderNearSpread",e);
                return false;
            }


            return true;  


        }



         /// <summary>
         /// Call from:
         /// 1) CBotTrader.TriggerStopOrder
         /// 2) CBotBase.InvertPosition
         /// 3) CBotTrader.SendStopLossInvert
         /// 4) CBotTrader.TriggerStopOrder
         /// 5) Other bots (CBotHighLowContra etc..)
         /// </summary>
         /// <param name="isin"></param>
         /// <param name="dir"></param>
         /// <param name="amount"></param>
         /// <returns></returns>
        public bool AddMarketOrder(string isin, /*OrderDirection*/EnmOrderDir dir, decimal amount)
        {
            return AddMarketOrder(isin, dir, amount, BotId);

        }





        private bool IsPossibleToAddMarketOrder(string instrument,decimal price, decimal minPrice,decimal maxPrice)
        {
            if (!_dealingServer.IsStockOnline)
            {
                Error("Not possible to send order as stock is not online");
                return false;
            }

           


            if (price > maxPrice || price < minPrice)
            {
                Error("BestPrice is out of limits. minPrice=" + minPrice + " maxPrice=" + maxPrice);
                return false;
            }
            


            if (price <= 0)
            {
                Error("Wrong adding market order. Wrong price." + "Price=" + price);
                //KAA 2017-03-07
             //   DisableBot();
                return false;

            }



            return true;
        }





        private decimal GetPriceForMarketOrder(decimal price, decimal minPrice, decimal maxPrice, string instrument, /*OrderDirection*/EnmOrderDir dir)
        {
            //2018-04-05 now dealing server must supply this value, default is 100
            int m_parNumOfSteps = _dealingServer.NumOfStepsForMarketOrder;
            decimal step = _dealingServer.DictInstruments[instrument].Min_step;
            decimal offset = m_parNumOfSteps * step;
            decimal priceOut = price;



            if (dir == /*OrderDirection*/EnmOrderDir.Buy)
            {
                priceOut += offset;
				priceOut = Math.Min(priceOut, maxPrice);

            }
            else if (dir == /*OrderDirection*/EnmOrderDir.Sell)
            {
                priceOut -= offset;
				priceOut = Math.Max(priceOut, minPrice);
            }

            return priceOut;
        }


        public decimal GetMinOrderSize(string instrument)
        {
            return  _dealingServer.GetMinOrderSize(instrument);           
        }


		protected virtual decimal GetPriceWithOffset(string instrument, EnmOrderDir dir, int throwSteps)
		{
			decimal step = _dealingServer.DictInstruments[instrument].Min_step;

            /*OrderDirection*/
            EnmOrderDir crossDir = (dir == EnmOrderDir.Buy) ? /*OrderDirection*/EnmOrderDir.Sell : /*OrderDirection*/EnmOrderDir.Buy;

			decimal priceOut =   _dealingServer.StockBox.GetBestPice(instrument, crossDir);
            //changed 2018-03-22
            decimal maxPrice = _dealingServer.GetMaxPrice(instrument); // _dealingServer.DictFutLims[instrument].Max;
			decimal minPrice =  _dealingServer.GetMinPrice(instrument);//_dealingServer.DictFutLims[instrument].Min;

			decimal offsetPrice = throwSteps * step;

			if (dir == EnmOrderDir.Buy)
			{
				priceOut += offsetPrice;
				priceOut = Math.Min(priceOut, maxPrice);

			}
			else if (dir == EnmOrderDir.Sell)
			{
				priceOut -= offsetPrice;
				priceOut = Math.Max(priceOut, minPrice);
			}



			return priceOut;
		}


		public void SendOrderThrow(string instrument, EnmOrderDir dir, decimal amount, decimal priceWithOffset)
		{			

			//OrderDirection orderDir = (dir == EnmOrderDir.Buy) ? OrderDirection.Buy : OrderDirection.Sell;


            //changed 2018-03-22
			decimal maxPrice = _dealingServer.GetMaxPrice(instrument); //_dealingServer.DictFutLims[instrument].Max;
            decimal minPrice = _dealingServer.GetMinPrice(instrument);//_dealingServer.DictFutLims[instrument].Min;

			if (!IsPossibleToAddMarketOrder(instrument, priceWithOffset, minPrice, maxPrice))
			{
				Error("Not possible to send OrderThrow");
				return;

			}





			AddOrder(instrument, priceWithOffset, /*orderDir*/dir, amount);


		}


        /// <summary>
        /// Send market order WITH checking money limits (AddMarketOrder)
        /// </summary>  
        protected bool AddMarketOrder(string instrument, EnmOrderDir dir ,decimal amount, int botId)
        {
            Log("AddMarketOrder");
            //OrderDirection crossDir = (dir == OrderDirection.Buy) ? OrderDirection.Sell : OrderDirection.Buy;
            EnmOrderDir crossDir = (dir ==  EnmOrderDir.Buy) ?  EnmOrderDir.Sell : EnmOrderDir.Buy;

            //TO DO check
            decimal price = _dealingServer.StockBox.GetBestPice(instrument, crossDir);
            //changed 2018-03-22
            decimal maxPrice =  _dealingServer.GetMaxPrice(instrument); //_dealingServer.DictFutLims[instrument].Max;
            decimal minPrice = _dealingServer.GetMinPrice(instrument);  //_dealingServer.DictFutLims[instrument].Min;



            if (!IsPossibleToAddMarketOrder(instrument, price, minPrice, maxPrice))
            {
                Log("Not possible to add market order");
                return false;
            }

            decimal priceOrder = GetPriceForMarketOrder(price, minPrice, maxPrice, instrument, dir);

          
             AddOrder(instrument, priceOrder, dir, amount, botId);
             LogAddOrder("AddMarketOrder",instrument,price,dir,amount);
           
           return true;  

        }

        /// <summary>
        /// Call sending market order WITHOUT checking money limits (ForceAddOrder)
        /// 
        /// Call from:
        /// 1) ClosePositionByMarket
        /// 2) GUI manual trading window
        /// </summary>      
        public bool ForceAddMarketOrder(string instrument, /*OrderDirection*/EnmOrderDir dir, decimal amount, int botId, int supervisorID=0)
        {

            Log("Force add market order");
        
            EnmOrderDir crossDir = (dir == EnmOrderDir.Buy) ? EnmOrderDir.Sell : EnmOrderDir.Buy;

            //TO DO check
            decimal price = _dealingServer.StockBox.GetBestPice(instrument, crossDir);

            decimal maxPrice = _dealingServer.GetMaxPrice(instrument);   
            decimal minPrice = _dealingServer.GetMinPrice(instrument); 


            if (!IsPossibleToAddMarketOrder(instrument, price, minPrice, maxPrice))
                return false;

            decimal priceOrder = GetPriceForMarketOrder(price, minPrice, maxPrice, instrument, dir);

            ForceAddOrder(instrument, priceOrder, dir, amount, botId,supervisorID);
            LogAddOrder("ForceAddMarketOrder", instrument, price,dir, amount);

            return true;  


        }
         

       /*
         //TODO remove Deprecated used by supervisor
        protected bool ClosePositionByMarketDeprecated(string isin,CRawPosition rp)
        {

            EnmOrderDir dir = rp.Pos > 0 ? EnmOrderDir.Sell : EnmOrderDir.Buy;


            return AddMarketOrder(isin, dir, Math.Abs(rp.Pos));

        }
        */
		 /*
        protected bool  ClosePositionByMarket(string isin, OrderDirection dir, int amount)
        {
            return  ClosePositionByMarket(isin, dir, amount, BotId);
        }
		 */
         /*
        protected bool ClosePositionByMarketPartial(string isin, EnmOrderDir dir, int amount)
        {

            EnmOrderDir dirNeg = dir == EnmOrderDir.Buy ? EnmOrderDir.Sell : EnmOrderDir.Buy;
            return AddMarketOrder(isin, dirNeg, amount, BotId);

        }
       */



         /// <summary>
         /// Close postion by market of selected instrument. 
         /// 
         /// Note here we force to send market order with no limit check.
         /// Because we need to have a possibility to close position even if
         /// limits are violated
         /// 
         /// Call from:
         /// - CloseAllBotPositions
         /// - ClosePositionOfInstrument
         /// </summary>      
        protected bool ClosePositionByMarket(string instrument,  decimal amount)
        {
            Log("ClosePositionByMarket");
            return ForceAddMarketOrder(instrument, GetOrderDir(amount), amount, BotId);
            //return AddMarketOrder(instrument, GetOrderDir(amount), Math.Abs(amount), BotId, moneylimitCheck: false);
        }


        private EnmOrderDir GetOrderDir(decimal amount)
        {
            return amount > 0 ? EnmOrderDir.Sell : EnmOrderDir.Buy;

        }



        /// <summary>
        /// Call from TradersDispatcher (terminal)
        /// </summary>
        /// <param name="orderId"></param>
        public void  CancelOrder(long orderId)
        {

            if (IsPossibleToCancelOrders())
            {
                string msg = "CancelOrder id=" + orderId;
                Log(msg);
                LogMonitor(msg);
                _dealingServer.CancelOrder(orderId,BotId);
                m_timers["WaitCancelOrderReply"].Set();
            }

        }


        protected bool IsNoOpenedOrders()
        {
            lock (MonitorOrdersAll)
            {
                foreach (var v in MonitorOrdersAll)
                {
                    if (v.Value.Count > 0)
                        return false;
                }
            }
            return true;
        }

        protected bool IsNoOpenedPos()
        {
            foreach (var v in MonitorPositionsAll)
            {
                if (v.Value.Amount != 0)
                    return false;
            }
            return true;
        }



        /// <summary>
        /// Cancell all orders of bots
        /// 
        /// Call from:
        /// GUICOmponents\WindowManualTrading   ButtonCommonCommands_Click
        /// SelfTerimnate
        /// BotTrader.RecalcBotLogics
        /// ...and other bots
        /// </summary>
        public  void CancellAllBotOrders()
        {

            if (IsNoOpenedOrders())
            {
                Log("No Orders. No need to cancell");
                return;
            }
            
            

           

            if (IsPossibleToCancelOrders())
            {
                string msg = "CancellAllBotOrders";
                Log(msg);
                LogMonitor(msg);

                //If dealing server supports "cancel order" transaction with epmty
                // instrument (for all instrument, like in plaza2), use dealing server command.
                // Else enumerate all instrument and send order  transaction for each instrument
                if (_dealingServer.IsPossibleEmptyInstrCancellOrders)
                    CancellAllOrdersNatively();
                else
                    CancellAllOrdersEnumInstr();

                m_timers["WaitCancelOrderReply"].Set();
            }

        }



        /// <summary>
        /// Dealing server supports "cancel order" transaction with epmty
        ///  instrument (for all instrument, like in plaza2), use dealing server command.
        ///  
        /// 
        /// </summary>
        private void CancellAllOrdersNatively()
        {
            _dealingServer.CancelAllOrders( (int)EnmCancelOrderDir.BuOrSell, BotId, "", BotId);
        }
   

       
        /// <summary>
        ///Routine enumerates all bot's instruments and send cancell order for each
        ///instrument step by step.
        ///
        /// Call from:
        /// CancellAllBotOrders
        /// 
        /// 
        /// </summary>
        private void CancellAllOrdersEnumInstr()
        {
            lock (MonitorOrdersAll)
            {
                foreach (var kvp in MonitorOrdersAll)
                {
                    string instrument = kvp.Key;
                    CancellAllOrdersByInstrument(instrument);

                }
            }

        }

   
        public bool IsPossibleToCancellOrderWithInstrument(string instrument)
        {

            if (MonitorOrdersAll == null)
                return false;

            lock (MonitorOrdersAll)
            {
                if (!MonitorOrdersAll.ContainsKey(instrument))
                    return false;

                if (MonitorOrdersAll[instrument].Count == 0)
                    return false;
            }

            return true;

        }


        /// <summary>
        /// Using for dealing servers that doesn't allow native method for 
        /// delete instruments. If only one instrument order exist do
        /// send cancell all orders else enumerate Monitor orders and send 
        /// cancell by id.
        /// </summary>
        /// <param name="instrument"></param>
        private void EnumOrdersForCancellByInstrument(string instrument)
        {

            lock (MonitorOrdersAll)
            {
                int cnt = 0;
                foreach (var kvp in MonitorOrdersAll)
                {
                    string instr = kvp.Key;
                    if (kvp.Value.Count > 0)
                        cnt++;

                    if (cnt > 1)
                        break;//more than one instruments orders

                }
                //only one instrument orders
                if (cnt == 1)
                    _dealingServer.CancelAllOrders(/*3*/ (int)EnmCancelOrderDir.BuOrSell, this.BotId, instrument, BotId); //send cancell all orders
                else 
                {
                    //enumerate all orders of given instrument and cancell all
                    //orders by id
                    foreach (var kvp in MonitorOrdersAll)
                        if (kvp.Key == instrument)
                            foreach (var kvp2 in kvp.Value)
                                _dealingServer.CancelOrder(kvp2.Key, BotId);
                }
            }

        }



        /// <summary>
        /// Call from:
        /// 1) CancellAllOrdersEnumInstr
        /// 2) CancellOrdersWithInstrumenByTrader
        /// </summary>
        /// <param name="instrument"></param>
        public void CancellAllOrdersByInstrument(string instrument)
        {
            //Possible cancell orders (trades session is active)
            if (IsPossibleToCancelOrders())
            {

                //KAA changed 2017-06-07
                //check for possibility order cancell (orders of instruments exists
                //in MonitorOrdersAll)
                if (IsPossibleToCancellOrderWithInstrument(instrument))
                {

                    if (_dealingServer.IsPossibleNativeCancellOrdByInstr)
                        _dealingServer.CancelAllOrders(/*3*/ (int)EnmCancelOrderDir.BuOrSell, this.BotId, instrument, BotId);
                    else
                        EnumOrdersForCancellByInstrument(instrument);


                    string msg = "CancellAllBotInstrumentOrders " + instrument;
                    Log(msg);
                    LogMonitor(msg);

                    //2017_02_09 added condition cause from GUI could send cancell order when 
                    //no orders at all. But better to send comand ro Stock exch 
                    //TODO check thrade safety
                    /*if (_dealingServer.UserOrderBox.DictUsersOpenedOrders != null)
                        if (_dealingServer.UserOrderBox.DictUsersOpenedOrders.ContainsKey(this.BotId) &&
                            _dealingServer.UserOrderBox.DictUsersOpenedOrders.Count > 0)
                                m_timers["WaitCancelOrderReply"].Set();
                     * */

                    //2017-07-20 use MonitorOrdersAll insted of DictUsersOpenedOrders
                    bool bFound = false;
                    lock (MonitorOrdersAll)
                    {
                        foreach (var kvp in MonitorOrdersAll)
                        {
                            foreach (var kvp2 in kvp.Value)
                            {
                                COrder order = kvp2.Value;
                                if (order.Isin == instrument)
                                {
                                    bFound = true;
                                    break;
                                }

                            }

                        }
                    }
                       
                    if (bFound)
                         m_timers["WaitCancelOrderReply"].Set();
               
                }
            }

        }



        protected void CancellAllBotOrdersBuyOrSell(int buy_sell)
        {
            if (IsPossibleToCancelOrders())
            {
                //not tested
                Log("CancellAllBotOrdersBuyOrSell");
                _dealingServer.CancelAllOrders(buy_sell, this.BotId, "",  BotId);
                m_timers["WaitCancelOrderReply"].Set();
            }
        }

      /*
        protected void CancellAllOrders()
        {
            if (IsPossibleToCancelOrders())
            {
                Log("CancellAllOrders");
                _dealingServer.CancelAllOrders(3, 0, "", BotId);
                m_timers["WaitCancelOrderReply"].Set();
            }
        }
         */
         //not tested not using
        protected void CancelAllOrdersBuyOrSell(int buy_sell)
        {
            if (IsPossibleToCancelOrders())
            {
                //not tested
                Log("CancelAllOrdersBuyOrSell");
                _dealingServer.CancelAllOrders(buy_sell, this.BotId, "", BotId);
                m_timers["WaitCancelOrderReply"].Set();
            }
        }
		/// <summary>
		/// Cancell all orders with instrumen, dir and price.		
		/// </summary>		
		public void CancellOrdersByPriceAndDir(string instrument, EnmOrderDir dir, decimal price)
		{
            
			

            lock (MonitorOrdersAll)
            {
                foreach (var kvp in MonitorOrdersAll)
                {
                    string instrCurr = kvp.Key;
                    if (instrCurr != instrument)
                        continue;

                    foreach (var kvp2 in kvp.Value)
                    {
                        long id = kvp2.Key;
                        COrder ord = kvp2.Value;
                        if (ord.Dir == (sbyte)dir && ord.Price == price)
                            _dealingServer.CancelOrder(id, BotId);




                    }

                }
            }

            string msg = "CancellOrdersByPriceAndDir";
            Log(msg);
            LogMonitor(msg);

		}
		public void InvertPosition(string instrument)
		{
			
			Log("InvertPosition");

			decimal amount = MonitorPositionsAll[instrument].Amount;

			if (amount ==0)
			{
				Log("No poision. Exit.");
				return;
			}

		     /*OrderDirection*/EnmOrderDir dirCross = (amount > 0) ? /*OrderDirection*/EnmOrderDir.Sell : /*OrderDirection*/EnmOrderDir.Buy;

			AddMarketOrder(instrument, dirCross, 2 * amount);

		}


        public void MainThread()
        {

            try
            {
         

                foreach (CBotEventStruct value in EventsQueue.GetConsumingEnumerable())
                {                

                    ProcessRecalc(value);
                                     
                }
            }
            catch (Exception e)
            {
                Log("Error ! Message=" + e.Message + " stacktrace="+e.StackTrace);
                Error("CBotBase MainThread", e);

            }



        }


      


        private void PrintOpenedOrders()
        {
          /*  foreach (KeyValuePair<long,COrder> order in m_dictOrder)
            {
                m_logger.Write(order.Key + "" + " orderId="+order.Value.OrderId + " isin="+order.Value.Isin+ " dir=" +  order.Value.Dir+  " price="+ order.Value.Price+ 
                                " amount="+ order.Value.Amount+ " amount_rest="+order.Value.Amount_rest+ " action="+m_plaza2Connector.GetActionString(order.Value.Action)   );

            }
            */


        }

        private void DBGPrinrUserOrder(CRawOrdersLogStruct rol)
        {
           
            Log(String.Format
            ("[OnOrderAccepted] Instrument={0}, IsinId={1} Di={2} Amount={3} Deal_Price={4} Amount_rest={5} Id_ord={6}", 
                                rol.Instrument, //0
                                rol.Isin_Id,    //1
                                rol.Dir,        //2
                                rol.Amount,     //3                            
                                rol.Deal_Price, //4
                                rol.Amount_rest,//5
                                rol.Id_ord      //6
                                ));

        }



		 protected virtual void OnUserOrderUpdateLogics(string instrument)
		 {

			 Thread.Sleep(0);

		 }



         /// <summary>
        /// Updates all user orders 
         /// </summary>
         /// <param name="isin"></param>
         /// <param name="rols"></param>
        protected virtual void UpdateMonitorOrdersAll(string isin,CRawOrdersLogStruct rols)
        {

            lock (MonitorOrdersAll)
            {
                MonitorOrdersAll[isin][rols.Id_ord] = new COrder
                {
                    Isin = isin,
                    Price = rols.Price,
                    Action = rols.Action,
                    Amount = rols.Amount,
                    Moment = rols.Moment,
                    Dir = rols.Dir
                };
            }                                                 

        }

         /// <summary>
         /// Processes order log structure change for native plaza 2 orders_log structure.
         /// Copies data to  MonitorOrdersAll, which is used for send traders and bots. 
         /// 
         /// Call from:
         /// 1) CBotBase.InitialListOrder.
         /// 2) RecalcBotStructs, events:
        ///                            - OnOrderAccepted
        ///                            - OnOrderCancel
        ///                            - OnOrderDeal
         /// </summary>
         /// <param name="rols"></param>
        protected virtual void ProcessRawOrdLogStruct(CRawOrdersLogStruct rols)
        {

            try
            {
                //KAA changed 2017_01_14 as we use clearing order/pos calc now				
                if (rols.Ext_id == BotId /*&& rols.Moment > m_plaza2Connector.SessionBox.CurrentSession.SessionBegin*/)
                {
					bool bNeedUpdMonitOrdersTot = false;
                    
                    //KAA changed 2017-06-07
                    //string instrument =   "unknown";
                    string instrument = rols.Instrument;

                    //for plaza 2 use isin_id
                    if (instrument == "" || instrument == null)
                    {
                        //if (m_plaza2Connector.DictIsin_id.ContainsKey(rols.Isin_Id))
                        if (_dealingServer.Instruments.IsContainsIsinId(rols.Isin_Id))
                            instrument = _dealingServer.Instruments.DictIsinId_Instrument[rols.Isin_Id];

                        else
                            Log("Error ! Unknown isin");
                    }


                    lock (MonitorOrdersAll)
                    {
                        if (!MonitorOrdersAll.ContainsKey(instrument))
                            MonitorOrdersAll[instrument] = new Dictionary<long, COrder>();







                        //order with Id is not exists
                        if (!MonitorOrdersAll[instrument].ContainsKey(rols.Id_ord))
                        {
                            // 2016-01-14 KAA
                            // To protect from situation when server repeat
                            // to send order that was already dealed before and
                            // we have empty moitor because it was removed as dealed
                            // (or deleted) previously.
                            //TODO: test
                            if (rols.Action == (sbyte)EnmOrderAction.Added)
                            {
                                //MonitorOrdersAll[isin][rols.Id_ord] = new COrder(isin, rols);
								//2017-10-23 if clearing processed - could be possible order will not remove
                            
                             
						     if (_dealingServer.IsActualSessionNumber(rols.Sess_id))
								    {
    									UpdateMonitorOrdersAll(instrument, rols);

	    								GUIBot.UpdateOrders(instrument, rols);
		    							bNeedUpdMonitOrdersTot = true;
			    					}
                            }
                        }

                        else //order with Id is already exists
                        {
                            if (rols.Action == (sbyte)EnmOrderAction.Deleted)
                            {
                                //full remove
                                if (rols.Amount_rest == 0)
                                {
                                    MonitorOrdersAll[instrument].Remove(rols.Id_ord);
                                    GUIBot.RemoveOrders(instrument, rols);
                                    bNeedUpdMonitOrdersTot = true;
                                }
                                else//partial removed
                                {
                                    //MonitorOrdersAll[isin][rols.Id_ord] = new COrder(isin, rols);
                                    UpdateMonitorOrdersAll(instrument, rols);
                                    GUIBot.UpdateOrders(instrument, rols);
                                    bNeedUpdMonitOrdersTot = true;
                                }

                            }
                            else if (rols.Action == (sbyte)EnmOrderAction.Deal)
                            {

                                if (rols.Amount_rest == 0)
                                {
                                    MonitorOrdersAll[instrument].Remove(rols.Id_ord);
                                    GUIBot.RemoveOrders(instrument, rols);
                                    bNeedUpdMonitOrdersTot = true;
                                }
                                else
                                {



                                    //TO DO check for partial
                                    MonitorOrdersAll[instrument][rols.Id_ord].Amount -= rols.Amount;
                                    GUIBot.UpdateOrders(instrument, rols);
                                    bNeedUpdMonitOrdersTot = true;


                                }

                            }

                            else if (rols.Action == (sbyte)EnmOrderAction.Added)
                            {

                                //MonitorOrdersAll[isin][rols.Id_ord] = new COrder(isin, rols);
                                UpdateMonitorOrdersAll(instrument, rols);
                                bNeedUpdMonitOrdersTot = true;
                                //do nothing

                            }
                            else
                            {
                                Log("Error ! Unknown order type");

                            }


                        }
                    } //end lock(MonitorOrdersAll)

                    //note MonitorOrdersAll locking inside UpdateFully
					if (bNeedUpdMonitOrdersTot)
						_monitorOrdersAllTotal.UpdateFully(instrument, MonitorOrdersAll);

                }
			


            }
            catch (Exception e)
            {

                Error("ProcessRawOrdLogStruct bot=" + Name, e);

            }

        }

      


      

        protected virtual void OnPositionsOnlineData()
        {

          //  _dealingServer.PositionBox.mxListRowsPositions.WaitOne();

            m_listLocalRawPositions = new List<CRawPosition>();
         //   m_listLocalRawPositions.AddRange(_dealingServer.PositionBox.ListRawPos);
        //    _dealingServer.PositionBox.mxListRowsPositions.ReleaseMutex();



            UpdatePositionsAll();


            m_positionOnlineRecieved = true;
            


        }
     
         /*
        private void InitialUserDeals()
        {
            m_plaza2Connector.UserDealsPosBox.mxListRawUserDeal.WaitOne();
            m_listLocalRawUserDeals.AddRange(m_plaza2Connector.UserDealsPosBox.ListRawUserDeal);
            m_plaza2Connector.UserDealsPosBox.mxListRawUserDeal.ReleaseMutex();

            //experimental 2016-10-05


         //   DateTime dtSessBegin = m_plaza2Connector.SessionBox.CurrentRawSession.Begin;

            foreach (CRawUserDeal rd in m_listLocalRawUserDeals)
            {


                
                if (  (rd.Ext_id_buy == this.BotId || rd.Ext_id_sell == this.BotId)  ) 
                {
                    m_listLocalBotUserDeals.Add(rd);

                   // CalculateBotPos(rd);
                    //TO DO dictionary

                }


            }
         
          
        }
          */

        /// <summary>
        /// Creates local copy of  UserOrderBox's list orders.
        /// Call ProcessRawOrdLogStruct to recalculate orders
        /// in this local Copy;
        /// 
        /// Call from: 
        ///           OnOnlineUserOrdersData
        ///           
        /// Call when: 
        ///          All order data is online
        /// </summary>
        private void InitialListOrder()
        {


			//TODO encapsulate

            try
            {
                _dealingServer.UserOrderBox.mxListRawOrders.WaitOne();
                m_listLocalRawOrdersStructs = new List<CRawOrdersLogStruct>();
                m_listLocalRawOrdersStructs.AddRange(_dealingServer.UserOrderBox.ListRawOrdersStruct);


            }
            catch (Exception e)
            {

                Error("InitialListOrder", e);

            }
            finally
            {
                _dealingServer.UserOrderBox.mxListRawOrders.ReleaseMutex();
            }
      

            try
            {

                const int TRIALS_COUNT = 30;
                int j = 0;
                for (j = 0; j < TRIALS_COUNT; j++)
                {
                    if (_dealingServer.IsAllInstrAllMarketsAvailable)
                        break;

                    System.Threading.Thread.Sleep(100);
                }
                if (j == TRIALS_COUNT)
                {
                    Log("Dict isin is unavailable - timeout.");
                    return;
                    //TO DO disable bot
                }


                foreach (CRawOrdersLogStruct rols in m_listLocalRawOrdersStructs)
                {

                    ProcessRawOrdLogStruct(rols);

                  
                   
                }
            }
            catch (Exception e)
            {
                string err = "Error in InitialListOrder message";
                Log(err + e.Message + " stack" + e.StackTrace);
                Error(err+" CBotBase.InitialListOrder",e );

            }

        }

         //TODO    
         //
         //Make stock uniform format

        private void CopyStock(string isin)
        {

           

                lock (_dealingServer.SnapshoterStock.OutputStocks[isin].Lck)
                {
                    lock (lckCurrentStocks)
                    {
                       try
                       {
                           m_currentStocks[isin][Direction.Up].Clear();
                           m_currentStocks[isin][Direction.Down].Clear();

                        for (int i = 0; i < _dealingServer.SnapshoterStock.OutputStocks[isin].GetStockDepth(0);
                                i++)
                        {

                            m_currentStocks[isin][Direction.Up].Add(new CStock(_dealingServer.SnapshoterStock.OutputStocks[isin][Direction.Up][0][i].Price,
                                                                     _dealingServer.SnapshoterStock.OutputStocks[isin][Direction.Up][0][i].Volume));

                            m_currentStocks[isin][Direction.Down].Add(new CStock(_dealingServer.SnapshoterStock.OutputStocks[isin][Direction.Down][0][i].Price,
                                                                       _dealingServer.SnapshoterStock.OutputStocks[isin][Direction.Down][0][i].Volume));



                        }
                       }     
                        catch (Exception e)
                        {   
                            Error("CopyStock",e);
                        }
                    }
                }

              


            
        }

     

       

        /// <summary>
        /// 
        /// Call from:
        /// - CreateOneBotFromConfig
        /// 
        /// </summary>
        public void SynchronizeOnBotReload()
        {

            if (_dealingServer.IsOnlineUserDeals)            
                OnUserDealOnlineData();

            if (_dealingServer.IsOnlineUserOrderLog)
                OnOnlineUserOrdersData();

            if (_dealingServer.IsPositionOnline)
                OnPositionsOnlineData();
            


        }

 

        /// <summary>
        ///Call from:
        ///
        /// - RecalcBotStructs
        /// - SynchronizeOnBotReload
        /// </summary>      
        protected virtual void OnUserDealOnlineData()
        {
            //InitialUserDeals();
            //IsUserDealsOnlineRecieved = true;
            UpdateTotalVm();
            IsUserDealsOnlineRecieved = true;
            

        }
       



        protected virtual void OnOnlineUserOrdersData()
        {
            InitialListOrder();
            PrintOpenedOrders();
            m_userOrdersOnlineRecieved = true;


        }

        private void UpdateBidAsk()
        {

            lock (lckCurrentStocks)
            {
                try
                {
                    foreach (var kvp in m_currentStocks)
                    {

                        string instrument = kvp.Key;
                        List<CStock> stockDown= kvp.Value[Direction.Down];
                        List<CStock> stockUp = kvp.Value[Direction.Up];
                        if (stockDown.Count>0)
                            _bid[instrument] = kvp.Value[Direction.Down][0].Price;

                        if(stockUp.Count>0)
                        _ask[instrument] = kvp.Value[Direction.Up][0].Price;

                        /*
                        m_currentStocks[isin][Direction.Up].Add(new CStock(m_plaza2Connector.StockDispatcher.OutputStocks[isin][Direction.Up][i].Price,
                                                                  m_plaza2Connector.StockDispatcher.OutputStocks[isin][Direction.Up][i].Volume));

                        */


                    }

                }
                catch (Exception e)
                {

                    Error("UpdateBidAsk",e);
                }



            }




        }


        protected virtual void OnStockUpdateData(string instrument)
        {
           

            CopyStock(instrument);
            
            UpdateBidAsk();
          
            //added 2017-11-07
            //TODO make update if bid ask changed
            //
         
            UpdateTotalVm();
            
        }

       


        protected virtual void OnUserDealData(string instrument, CBotEventStruct botEvent)
        {
            CBotPos bp = (CBotPos)botEvent.Data;
            UpdateMonitorPosisionsAll(instrument, bp);

        }

		//2017-11-13
		int parBotQueueSz = 100;
		int parLastSignalledMsec = 1000;

		private DateTime _dtLastSignaled = new DateTime();

		private void CheckBotQueueSize(int count)
		{
			if (count > parBotQueueSz &&
				(DateTime.Now - _dtLastSignaled).TotalMilliseconds > parLastSignalledMsec)
			{
				Error(String.Format("Bot id={0} queue count={1} more than {2}",
							BotId, count,parBotQueueSz));

				_dtLastSignaled = DateTime.Now;
			}

		}

        protected virtual void  RecalcBotStructs(CBotEventStruct botEvent)
        {
            try
            {
                string errMsg = "";
                string isin = botEvent.Isin;
                EnmBotEventCode evCode = botEvent.EventCode;

                Log("Bot update struct " + " isin=" + isin + " processing event " + evCode.ToString());
                Log("Count in queue=" + EventsQueue.Count);
				CheckBotQueueSize(EventsQueue.Count);
               
                if (evCode == EnmBotEventCode.OnStockUpdate)
                    OnStockUpdateData(isin);
                else if (evCode == EnmBotEventCode.OnUserOrdersOnline)
                    OnOnlineUserOrdersData();
                else if (evCode == EnmBotEventCode.OnUserDealOnline)
                    OnUserDealOnlineData();
                else if (evCode == EnmBotEventCode.OnPositionOnline)
                    OnPositionsOnlineData();

                else if (botEvent.EventCode == EnmBotEventCode.OnUserDealOnline)
                    OnDealsOnlineData();
               

                else if (evCode == EnmBotEventCode.OnOrderAdded)
                {
                    //to do timer




                }
                else if (evCode == EnmBotEventCode.OnOrderAccepted)
                {
                    m_timers["WaitAddOrderReply"].Reset();
                    CRawOrdersLogStruct rol = (CRawOrdersLogStruct)botEvent.Data;
                    ProcessRawOrdLogStruct(rol);
                    PrintOpenedOrders();
                    DBGPrinrUserOrder(rol);
                }

                else if (evCode == EnmBotEventCode.OnOrderCancel)
                {

                    m_timers["WaitCancelOrderReply"].Reset();
                    CRawOrdersLogStruct rol = (CRawOrdersLogStruct)botEvent.Data;
                    ProcessRawOrdLogStruct(rol);
                }

                else if (evCode == EnmBotEventCode.OnOrderDeal)
                {
                    //2017-02-09 commented
                    //2017-06-15 uncommented again               
                    if (m_timers["WaitAddOrderReply"].IsStarted)
                        m_timers["WaitAddOrderReply"].Reset();
                    CRawOrdersLogStruct rol = (CRawOrdersLogStruct)botEvent.Data;
                    ProcessRawOrdLogStruct(rol);


                }

                else if (evCode == EnmBotEventCode.OnErrorAddOrder)
                {
                    Error("Error on add order");
                    //  Customer  asked to remove this condition 2017-10-18
                    //   DisableBot("Error adding order");
                }

                else if (evCode == EnmBotEventCode.OnErrorCancelOrder)
                {
                    //2017-12-12 remove condition
                   // DisableBot("Error cancel order");

                }

                else if (evCode == EnmBotEventCode.OnUserDeal)
                    OnUserDealData(isin, botEvent);


                else if (evCode == EnmBotEventCode.OnDeals)
                {

                    BotEventDeal bd = (BotEventDeal)botEvent.Data;
                    //string isin = bd.Isin;
                    //MonitorMarketDataAll[isin].

                }

                else if (evCode == EnmBotEventCode.OnPostionUpdate)
                {

                    UpdatePositionsAll();



                }

                else if (botEvent.EventCode == EnmBotEventCode.OnTimer)
                {
                    string tmName = (string)botEvent.Data;
                    StartTimer(tmName);


                }

                else if (botEvent.EventCode == EnmBotEventCode.OnTFUpdate)
                {


                    BotEventTF BTF = (BotEventTF)botEvent.Data;

                    Log("TFUpdate   TFUpdate=" + BTF.TFUpdate + " HighPrice=" + BTF.TFI.HighPrice + " LowPrice=" + BTF.TFI.LowPrice);

                    if (BTF.TFUpdate == EnmTF.D1)
                    {
                        string _isin = BTF.TFI.Isin;
                        MonitorMarketDataAll[_isin].HighDayPrice = BTF.TFI.HighPrice;
                        MonitorMarketDataAll[_isin].LowDayPrice = BTF.TFI.LowPrice; ;
                    }
                }
                else if (botEvent.EventCode == EnmBotEventCode.OnForceUpdTrdMgr)
                {
                    UpdateBotStatusTrdMgr();
                    UpdateBotPosAllTrdMgr();
                    UpdateTraderInfoSumTrdMgr();
                }
                //2018-04-25
                else if (botEvent.EventCode == EnmBotEventCode.OnForceUpdTotalVM)
                {
                    UpdateTotalVm();
                }
                //2018-10-31
                else if (botEvent.EventCode == EnmBotEventCode.OnSessionEnd)
                    OnSessionEnd();



            }
            catch (Exception e)
            {
                Error("RecalcBotStructs",e);

            }
        }

       
         /// <summary>
         /// Call from OnUserDealData
         /// </summary>
         /// <param name="isin"></param>
         /// <param name="bp"></param>
        protected virtual void UpdateMonitorPosisionsAll(string isin, CBotPos bp)
        {
            if ((_dealingServer.IsOnlineUserDeals) &&
                   (!MonitorPositionsAll.ContainsKey(isin) || MonitorPositionsAll[isin].Amount == 0))
                PlaySoundOpenPos(isin);

            lock (MonitorPositionsAll)
            {
                MonitorPositionsAll[isin] = bp;

            }

			//KAA 2017-03-06
			//protect against multiple close request
			if (bp.Amount == 0)
				_dictIsClosingPos[isin] = false;


			

           
            UpdateTotalVm();


            GUIBot.UpdateMonitorPos(isin, bp);
            _dealingServer.UpdateBotPosTrdMgr(BotId,bp);
            UpdateDBPosInstr(bp.Amount, bp.AvPos);

        }

        //TO DO move to supervisor
        private void UpdatePositionsAll()
        {
            //_dealingServer.PositionBox.mxListRowsPositions.WaitOne();

            foreach (KeyValuePair<string, CRawPosition> vp in _dealingServer.PositionBox.DictPos)
                m_dictPosAllBotsSummary[vp.Key] = vp.Value;
                          
          //  _dealingServer.PositionBox.mxListRowsPositions.ReleaseMutex();


        }


        public void EnableBot()
        {
            if (m_disableBot)
            {
                string stMessage = "ENABLE BOT. " + Name;
                CBotHelper.PrintBanner(m_logger, stMessage);
                CBotHelper.PrintBanner(m_loggerStates, stMessage);

                DisabledBot = false;
                _botStatus.IsDisabled = false;
    
                UpdateBotStatusTrdMgr();
            }

        }





        public void DisableBot(string reason=null)
        {
            if (!m_disableBot)
            {
                string stMessage = "DISABLE BOT. " + Name +" Id=" + BotId  ;

                if (reason != null)
                    stMessage += " reason: " + reason;

                CBotHelper.PrintBanner(m_logger,stMessage);
                CBotHelper.PrintBanner(m_loggerStates,stMessage);
                          
                DisabledBot = true;
                _botStatus.IsDisabled = true;
              

                Error(stMessage);
                UpdateBotStatusTrdMgr();
            }

        }

        private void PlaySoundOpenPos(string isin)
        {
            //2018-03-06 - deprecated

            //_dealingServer.Sounder.PlaySound
              //  (new CSoundMessage() { code = SoundCode.BotPositionOpen, param1 = Name, param2 = isin });
        }

        //protected virtual void OnPositionOnline(){}

        protected virtual void OnDealsOnlineData() { }

        protected virtual void OnPositionUpdateLogics() { }

        //protected virtual void OnUserDealsOnlineLogics() { }

        protected virtual void OnStockUpdateLogics(string instrument) { }

        protected virtual void OnUserDealsLogics(string instrument) { }

        protected virtual void OnSessionUpdate() { }

        public bool _readyForRecalcBotLogics = false;
        public bool IsReadyForRecalcBotLogics
        {

            get
            {
                return _readyForRecalcBotLogics;

            }
            set
            {
                _readyForRecalcBotLogics = value;

                if (GUIBot != null && GUIBot.IsReadyForRecalcBotLogics != _readyForRecalcBotLogics)
                    GUIBot.IsReadyForRecalcBotLogics = _readyForRecalcBotLogics;
                RaisePropertyChanged("IsReadyForRecalcBotLogics");
            }



        }

        protected virtual bool IsReadyForRecalcLogics()
        {

            //2017-11-13

            if (_dealingServer.IsSimulateMode)
                return true;



            if (!_dealingServer.IsReadyForRecalcBots() ||
                 !IsUserDealsOnlineRecieved || !m_userOrdersOnlineRecieved || 
                 !m_positionOnlineRecieved             
                || (NeedTFAnalyzer && ! _dealingServer.IsAnalyzerTFOnline)

                )
            {
                if (IsReadyForRecalcBotLogics)
                    IsReadyForRecalcBotLogics = false;
                Log("Not ready, end recalc");
                return false;
            }



            if (!IsReadyForRecalcBotLogics)
                IsReadyForRecalcBotLogics = true;

            return true;
        }






        protected virtual void RecalcBotLogics(CBotEventStruct botEvent)
        {

            if (SelfTerminated)
                return;

        

            RiskManagerChecks();


            string instrument = botEvent.Isin;




            if (botEvent.EventCode == EnmBotEventCode.OnPostionUpdate)
                OnPositionUpdateLogics();


            else if (botEvent.EventCode == EnmBotEventCode.OnSessionUpdate)
                OnSessionUpdate();

            else if (botEvent.EventCode == EnmBotEventCode.OnStockUpdate)
                OnStockUpdateLogics(instrument);

            else if (botEvent.EventCode == EnmBotEventCode.OnUserDeal)
                OnUserDealsLogics(instrument);

            else if (botEvent.EventCode == EnmBotEventCode.OnOrderAccepted)
                OnUserOrderUpdateLogics(instrument);

            else if (botEvent.EventCode == EnmBotEventCode.OnOrderDeal)
                OnUserOrderUpdateLogics(instrument);

            else if (botEvent.EventCode == EnmBotEventCode.OnOrderCancel)
                OnUserOrderUpdateLogics(instrument);

            else if (botEvent.EventCode == EnmBotEventCode.OnCrossOrderReply)
                OnCrossOrderReply();

           

           // TMPDEBUG();
        }

        private void OnSessionEnd()
        {          
            if (m_disableBot)
            {
                decimal availMoney = _dealingServer.GetAccountTradeMoney(BotId);
                if (availMoney > 0)
                {
                    Log("New Session.Enable bot.");
                    EnableBot();
                    //2018-12-01
                    SelfTerminated = false;
                    Log("New Session.Reset self termination");
                }

            }



        }


        int i = 0;

        private void TMPDEBUG()
        {
           //if(BotId==100)
           // if (++i < 2)
             //   ForceAddMarketOrder ("MOEX", EnmOrderDir.Buy,1,100);
           

        }

        private void OnCrossOrderReply()
        {


            Log("OnCrossOrderReply");
            LogMonitor("OnCrossOrderReply");

            List<string> foundInProcess = new List<string>();


            foreach (var kvp in _dictIsClosingPos)
                if (kvp.Value)
                    foundInProcess.Add(kvp.Key);

            //TODO thing what is to do with it
            if (foundInProcess.Count > 1)
                Error("Multiple inprocess during cross fix");

            foreach (var instr in foundInProcess)
                _dictIsClosingPos[instr] = false;

        }


        private void RiskManagerChecks()
        {
            if (_riskManager != null)
                _riskManager.Check();


            //CheckOpenedPositions();
            //CheckMaxLossVm();
            //CheckBotPosAreEqualStockExchPos();
          
        }


        protected void ProcessRecalc(CBotEventStruct botEvent)
        {


            try
            {
                mxRecalc.WaitOne();

                Log("===================== Recalculation BEGIN event = " + botEvent.EventCode + "  =========================================================================================================================================");

                RecalcBotStructs(botEvent);

                if (IsReadyForRecalcLogics() && !DisabledBot)
                    RecalcBotLogics(botEvent);
                


                Log("===================== Recalculation END event = " + botEvent.EventCode + "  =========================================================================================================================================");
            }
            catch (Exception e)
            {                
                Error("Error ProcessRecalc "+ this.Name, e);
            }
            finally 
            {
                mxRecalc.ReleaseMutex();
            }

        }



        protected virtual void LoadParameters()
        {

           


        }


        public void CreateMarketData()
        {

            MonitorMarketDataAll = new Dictionary<string,CBotMarketData>();

            foreach (string isins in SettingsBot.ListIsins)            
                MonitorMarketDataAll[isins] = new CBotMarketData();
            
        }

        public virtual void Start()
        {
            try 
            {

                 foreach (string isin in SettingsBot.ListIsins )
                 {
                     m_currentStocks[isin] = new Dictionary<Direction, List<CStock>>();
                     mxCurrentStocks[isin] = new Mutex();
                     lckCurrentStocks[isin] = new object();

                     m_currentStocks[isin][Direction.Up] = new List<CStock>();
                     m_currentStocks[isin][Direction.Down] = new List<CStock>();

                 }


                 m_mainThread = new System.Threading.Thread(MainThread);
                 m_mainThread.Start();
            }

            catch (Exception e)
            {
                Log("error !" + e.Message);
                Error("CBotBase.start ", e);

            }
        }


         /// <summary>
         /// Close bot positions of all instruments
         /// 
         /// Call from:
         /// - SelfTerminate
         /// - WindowManualTrading.ButtonCommonCommands_Click
         /// </summary>
        public void CloseAllPositions()
        {        
                lock (MonitorPositionsAll)
                {
                    foreach (KeyValuePair<string, CBotPos> kv in MonitorPositionsAll)
                    {
                        string isin = kv.Key;
                        CBotPos bp = kv.Value;
                        if (bp.Amount != 0)
                        {
                            //protect against multiple sending AddOrder commands
                            if (!_dictIsClosingPos[isin])
                            {
                                _dictIsClosingPos[isin] = true;
                                ClosePositionByMarket(isin, bp.Amount);
                                Log("CloseAllPositions. Closing position isin=" + isin + " amount=" + bp.Amount);
                            }
                            else
                            {
                                Log("CloseAllPositions. Unable to close position. Previous closing is in process.");
                            }
                        }

                    }
                }
            
        }




     
        /// <summary>
        /// Close positions of selected instrument
        ///
        /// Finds open positions of instrument. If found call closing method.
        /// Also checks if this position is not already in closing process
        /// 
        /// Call from:
        /// - ClosePositionOfInstrumentByTrader
        /// - CBotTrader.CloseByTakeProfit
        /// - CBotTrader.CloseByStopLoss
        /// </summary>
        /// <param name="instrument"></param>
        public virtual void ClosePositionOfInstrument(string instrument)
        {

            lock (MonitorPositionsAll)
            {              
                foreach (KeyValuePair<string, CBotPos> kv in MonitorPositionsAll)
                {
                    string currInstr = kv.Key;
                    if (currInstr == instrument)
                    {
                        CBotPos bp = kv.Value;
						if (bp.Amount != 0 )
							
                        {
							//KAA 2017-03-06
							//protect against multiple close request
                            if (!_dictIsClosingPos[currInstr])
                            {
                                _dictIsClosingPos[currInstr] = true;
                                ClosePositionByMarket(currInstr, bp.Amount);
                                Log("Closing position isin=" + currInstr + " amount=" + bp.Amount);
                            }
                            else
                            {
                                //need to fix strategy
                                Error("Trial of position close while previous close position is in progress");

                            }
                        }
                    }
                }
            }

        }



        public virtual void SelfTerminate(string reason=null)
        {

            Log("Start self terminating");

            CloseAllPositions();
            CancellAllBotOrders();

            SelfTerminated = true;

            if (reason != null)
            {
                string msg = "Reason: "+ reason;
                Log(msg);
                Error(msg);
            }
         
 
            //TO DO close all positions and special func close all oreders
            CBotHelper.PrintBanner(m_logger,  "Bot was self terminated"); 
       

            DisableBot(reason);


        }

        //2018-06-13
        protected virtual bool NeedProcessStockUpdate(string instrument)
        {
            lock (MonitorPositionsAll)
            {
                if (MonitorPositionsAll.ContainsKey(instrument))
                {
                    if (MonitorPositionsAll[instrument].Amount != 0)
                        return true;
                }

            }

            





            return false;
        }




        private long _parMaxPossibleQueueSize = 1000;
        private bool _isThrottleMode = false;

        public void Recalc(string isin, EnmBotEventCode evCode, object data)
        {
            //2018-06-13 
            //Experimental logics protect against queue overflow. 
            //if in throttle mode do ignore "stock update", event  
            //which is most  frequence event
            if (EventsQueue.Count> _parMaxPossibleQueueSize)
            {
                if (!_isThrottleMode)
                {
                    _isThrottleMode = true;
                    Error(String.Format("Entering throttle mode botId={0} Count={1}",
                         BotId, EventsQueue.Count));
                }

                if (evCode == EnmBotEventCode.OnStockUpdate)
                    return;

            }

            if (_isThrottleMode)
            {
                Error(String.Format("Exiting throttle mode botId={0} Count={1}",
                          BotId, EventsQueue.Count));
                _isThrottleMode = false;
            }

            //2018-06-13
            if (evCode == EnmBotEventCode.OnStockUpdate)
            {
                if (!NeedProcessStockUpdate(isin))
                {
                    // Log("Don't need update stock "+isin);
                    return;
                }
                Log("Need update stock " + isin);

            }
           

            EventsQueue.Add(new CBotEventStruct(isin, evCode, data));
        }
          

        protected virtual void CreateTimers()
        {

            //TODO get from config
            //chage 2017.02.09
            //2017-11-29 code inspection found that OnErrorAddOrder doesn't disabled bot (customer request).
            //So TODO: in the future if we'll do not need this DO remove this timer. 
            m_timers.Add("WaitAddOrderReply",new CTradeTimer("WaitAddOrderReply",this, EnmBotEventCode.OnErrorAddOrder, 10000));//was 3000
            m_timers.Add("WaitCancelOrderReply", new CTradeTimer("WaitCancelOrderReply", this, EnmBotEventCode.OnErrorCancelOrder, 10000)); //was 3000


            AddTimer("PeriodicalActions", m_parPeriodicActionsMS); StartTimer("PeriodicalActions");

        }

         /// <summary>
         /// Call from:
         ///           
         /// 1) EnableBot()
         /// 2) DisableBot();
         /// 3) On event OnForceUpdTrdMgr - force on start
         /// </summary>
        private void UpdateBotStatusTrdMgr()
        {
            if (SettingsBot.StrategyCode == enmStrategysCode.StrategyTrader) 
                _dealingServer.UpdateBotStatusTrdMgr(_botStatus);
        }


        /// <summary>
        /// Call from:
        /// 
        /// 1) OnForceUpdTrdMg
        /// </summary>
        public void UpdateBotPosAllTrdMgr()
        {
            if (SettingsBot.StrategyCode == enmStrategysCode.StrategyTrader)
            {
                lock (MonitorPositionsAll)
                {
                    foreach (var kvp in MonitorPositionsAll)
                    {
                        _dealingServer.UpdateBotPosTrdMgr(BotId, kvp.Value);
                    }

                }

            }

        }


        public void UpdateTraderInfoSumTrdMgr()
        {
            if (SettingsBot.StrategyCode == enmStrategysCode.StrategyTrader)
            {

                Thread.Sleep(0);
            }
        }

        
        protected void SetState(object NewState)
        {
           
            string message;

            if (m_currState == null)
                message = "=========== Set state [ " + NewState+" ] =====================";
   
            else
                message = "=========== Changed state from [" + m_currState.ToString() + "] to ===>" +
                "[" + NewState.ToString() + "]==============================";

            m_currState = NewState;
            Logger.Log(message);
            m_loggerStates.Log(message);
            GUIBot.UpdateBotState(m_currState.ToString());
        }


        protected bool IsState(object compState)
        {

            return EqualityComparer<object>.Default.Equals(m_currState, compState);


        }





        
    }

   
 
}
