using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

using Common;
using Common.Logger;
using Common.Interfaces;
using Common.Utils;

using Messenger;


using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Abstract;
using TradingLib.GUI;
using TradingLib.Data;
using TradingLib.Enums;
using TradingLib.Data.DB;
using TradingLib.ProtoTradingStructs;
using TradingLib.ProtoTradingStructs.TradeManager;
using TradingLib.Bots;
using TradingLib.BotEvents;
using TradingLib.TradersDispatcher;
using TradingLib.Snapshoters;
using TradingLib.TradeManagerServer;



using TCPLib;


namespace TradingLib.Abstract
{
    public abstract class CBaseDealingServer : IDealingServer, IAlarmable, IGUIDispatcherable, IBotOerations,
                                                    IClientGUIBox, IClientGUIBot, IClearingProcessorClient, IClientTradersDispatcher,
                                                    IClientSessionBox, IClientGUICandleBox, IClientUserDealsPosBox,
                                                    IClientSession, IClientInstruments, IClientUserDealsCollection, IClientDealBox,
                                                    IClientStockBox, IClientPosBox, IClientSnapshoter, IClientWindowManualTrading,
                                                    IClientControlBotGUI, IClientInstrumentGrid, IClientTradeManagerServer, ILogable,
                                                    IClientComponentFactory


    {


        public abstract IDealBox DealBox { get; }
        public abstract bool IsDealsOnline { get; set; }
        public abstract bool IsStockOnline { get; set; }
        public abstract bool IsFutInfoOnline { get; set; }
        public abstract bool IsOnlineUserDeals { get; set; }
        public abstract bool IsOnlineUserOrderLog { get; set; }
        public abstract bool IsAnalyzerTFOnline { get; set; }
        public abstract bool IsOnlineVM { get; set; }
        public abstract bool IsOrderControlAvailable { get; set; }
        public abstract bool IsPositionOnline { get; set; }
        public abstract bool IsSessionActive { get; set; }
        public abstract IUserOrderBox UserOrderBox { get; }
        public abstract bool IsSessionOnline { get; set; }
        public abstract bool IsPossibleToCancelOrders { get; set; }

        public abstract bool IsReadyStartTrdMgrServ { get; }

        public abstract bool IsPossibleNativeCancellOrdByInstr { get; set; }


        public abstract void FillDBClassField<T>(Dictionary<string, object> row, T outObj);
        public abstract IStockBox StockBox { get; }

        public abstract CBasePosBox PosBoxBase { get; }


        public bool IsTimeToInitCandles { get; set; }

        public bool IsAutomaticClearingProcessed { get; set; }

        public bool UseRealServer { get; set; }

        public bool IsAllBotLoaded { get; set; }


        public virtual bool IsDatabaseConnected { get; set; }
        public virtual bool IsDatabaseReadyForOperations { get; set; }


        public abstract IPositionBox PositionBox { get; }




        public bool IsTimeSynchronized { get; set; }


        public IReportDispatcher ReportDispatcher { get; set; }

        public CSounder Sounder { set; get; }


        public int SessionCurrent { get; set; }

        public decimal USDRate { set; get; }

        public bool IsDealsPosLogLoadedFromDB { get; set; }

        public Dictionary<string, decimal> DictStepPrice = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> DictMinStep = new Dictionary<string, decimal>();

        public virtual bool IsServerTimeAvailable { get; set; }

        public bool IsPossibleEmptyInstrCancellOrders { get; set; }

        private int _portListening;

        protected bool _simulateMode = false;


        private bool _useTradeManagerServer = true;

        protected CTradeManagerServer _trdManagerServer;

        private int _portTradeManager;

        private bool _isBotInitiallyCreatedOnStart = false;

        protected virtual bool IsAddTraderOnlineAllowed { get; set; }


        /// <summary>
        /// Current turnovers of traders
        /// </summary>
        protected List<CDBTurnOver> _lstTurnOvers = new List<CDBTurnOver>();

        public List<CDBTurnOver> LstTradersTurnOver
        {
            get
            {
                return _lstTurnOvers;

            }



        }


            

     



        public int PortTradeManager
        {
            get
            {
                return _portTradeManager;
            }

        }



        public bool IsSimulateMode
        {
            get
            {
                return _simulateMode;
            }
        }


        public bool IsAllInstrAllMarketsAvailable
        {
            get
            {

                if (Instruments == null)
                    return false;

                return Instruments.IsAllinstrumentsOfAllMarketsAvail();


            }
        }

        public bool IsLoadedMoneyData { get; set; }

      //  public bool IsDataLoadedFromDB { get; set; }


        public int StockExchId { get; set; }

        public CGlobalConfig GlobalConfig
        {
            get
            {
                return _globalConfig;
            }
        }


        public decimal BrokerFeeCoef
        {
            get
            {
                return GlobalConfig.BrokerFeeCoef / 100;


            }


        }

        public decimal InternalFeeCoef
        {
            get
            {
                return GlobalConfig.InternalFeeCoef / 100;


            }

        }



        public Dispatcher GUIDispatcher
        {
            get
            {
                if (GUIBox != null)
                    return GUIBox.GUIDispatcher;

                return null;
            }
            set { }
        }

		public CTradersDispatcher TradersDispatcher
		{
			get
			{
				return _tradersDispatcher;
			}

		}


		public CBotFactory BotFactory
		{
			get
			{
				return m_botFactory;

			}
		}

        private List<Dictionary<string, object>> _lstBotsConfig = new List<Dictionary<string, object>>();

        public List<Dictionary<string, object>> LstBotsConfig
        {
            get
            {
                return _lstBotsConfig;
            }
        }

        private List<Dictionary<string, object>> _lstBotsInstrumentsConfig = new List<Dictionary<string, object>>();

        public List<Dictionary<string, object>> LstBotsInstrumentsConfig
        {
            get
            {
                return _lstBotsInstrumentsConfig;
            }
        }



		public IMessenger Messenger { get; set; }

        public ManualResetEvent EvStockOnline { get; set; }
        public ManualResetEvent EvDealsOnline { get; set; }
        public ManualResetEvent EvPosOnline { get; set; }
        private ManualResetEvent _evTradeDisabledByTimeLoaded;
        private ManualResetEvent _evDataLoadedFromDB;
        private ManualResetEvent _evBotConfigLoaded = new ManualResetEvent(false);


        public CGlobalConfig _globalConfig;
        public CListInstruments Instruments { get; set; }

        public bool AnalzyeTimeFrames { get; set; }

        private bool _isGlobalConfigAvail = false;
        public bool IsGlobalConfigAvail
        {
            get
            {
                return _isGlobalConfigAvail;

            }

        }

        public bool NeedHistoricalDeals { get; set; } 


        public Dictionary<int, CDBAccountMoney> AccountsMoney { get; set; }
        public Dictionary<int, CDBAccountTrade> AccountsTrade { get; set; }
        public Dictionary<string, CFutLims> DictFutLims { get; set; }
        public Dictionary<long, CBotBase> DictBots { get; set; }

        public List<CBotBase> ListBots { get; set; }

        public Dictionary<string, decimal> DictVM { get; set; }


        protected CLogger _logger;

        public CLogger Logger { get { return _logger; } }

        public Dictionary<string, CInstrument> DictInstruments { get; set; }

		public ISnapshoterStock SnapshoterStock { get; set; }
		public CSnapshoterDeals SnapshoterDeals { get; set; }
     

        public CAlarmer/*IAlarmable*/ Alarmer { get; set; }

        public  CGUIBox GUIBox { set; get; }

        public /*CDBCommunicator*/IDBCommunicator DBCommunicator { get; set; }
        public CClearingProcessor ClearingProcessor { get; set; }


		
        public virtual ISessionBox SessionBox { get; set; }

        public abstract IUserDealsPosBox UserDealsPosBox { get;  }
	


        protected string _name;
		protected CTradersDispatcher _tradersDispatcher;
        protected DateTime _dtTradeDisabled;



		public virtual DateTime ServerTime { get; set; }
		private CTCPServer _tcpServer;


		protected CBotFactory m_botFactory;

        private int m_parLocalOffset = -2;

        protected ManualResetEvent _evServerTimeAvailable = new ManualResetEvent(false);
        private bool _bServerTimeAvailable = false;

        private CTradeManagerServer _tradeManagerServer;

        //default value childs need overwrite it
        //
        protected int _numOfStepsForMarketOrder = 100;

        protected CResourceAnalyzer _resourceAnalyzer;



        public int  NumOfStepsForMarketOrder
        { get
            {
                return _numOfStepsForMarketOrder;

            }


        }

        private decimal _moneyCurrStockExch = 0;

        public decimal MoneyCurrentStockExch
        {
            get
            {
                return _moneyCurrStockExch;
            }
        }


        private DateTime _dtLastWaletdUpdate = new DateTime();
        private DateTime _dtLastWaletUpdate;

        public DateTime DtLastWalletUpdate
        {
            get
            {

                return _dtLastWaletUpdate;

            }



        }

        protected List<int> _pricePrecisions;





        public CBaseDealingServer(string name)
        {
            _name = name;
        

            _logger = new CLogger(_name);
            
            CreateSunchronizationObjects();         

            Alarmer = new CAlarmer(this);

            ReadConfig();
            _resourceAnalyzer = new CResourceAnalyzer();

            AccountsMoney = new Dictionary<int, CDBAccountMoney>();
            AccountsTrade = new Dictionary<int, CDBAccountTrade>();
            DictInstruments = new Dictionary<string, CInstrument>();
            ListBots = new List<CBotBase>();
            DictVM = new Dictionary<string, decimal>();
            DictFutLims = new Dictionary<string, CFutLims>();


            if (GlobalConfig.ProcessPriorityRealTime)
                CUtil.IncreaseProcessPriority();
         


            NeedHistoricalDeals = GlobalConfig.NeedHistoricalDeals;
            GUIBox = new CGUIBox(this);


            DictBots = new Dictionary<long, CBotBase>();
			   

        }
        /// <summary>
        /// Call from 
        /// 1) CBaseDealingServer.UpdateTrdMgrTotalPos (on start)
        /// 2) CBasePosBox.UpdateTrdMgrTotalPos (on update pos)
        /// 
        /// </summary>
        public void UpdateTotalInstrumentPosition()
        {
            if (_tradeManagerServer != null)
                _tradeManagerServer.UpdateTotalInstrumentPosition();
            
        }


        /// <summary>
        /// Call from:
        /// 1) CBotBase.UpdateBotStatusTrdMgr (on Enable, Disable bot etc)
        /// 2) CBotBase.UpdateVmOpenCloseTrdMgr (on VM changed)
        /// </summary>
        /// <param name="botStatus"></param>
        public void UpdateBotStatusTrdMgr(CBotStatus botStatus)
        {           
            if (_tradeManagerServer != null)
                _tradeManagerServer.UpdateBotStatus(botStatus);
        }



        /// <summary>
        /// Call from CBotBase
        /// </summary>
        /// <param name="botId"></param>
        /// <param name="botPos"></param>
        public void UpdateBotPosTrdMgr(int botId, CBotPos botPos)
        {
            if (_tradeManagerServer != null)
                _tradeManagerServer.UpdateBotPos(botId, botPos);
              
        }



        protected abstract void CreateSessionBox();
        protected abstract void CreateUserDealsPosBox();
      //  protected abstract void CreateClearingProcessor();

        protected abstract void CreateExternalComponents();

        protected abstract void StartGateIfNeed();
        //protected abstract void CreateBots();
		protected abstract void CreateUserOrderBox();

		public abstract void AddOrder(int botId, string isin, decimal price, EnmOrderDir dir, decimal amount);
		public abstract void CancelOrder(long orderId, int botId);	
		public abstract void CancelAllOrders(int buy_sell, int ext_id, string isin, int botId);

        public abstract bool IsPriceInLimits(string instrument, decimal price);

        public abstract bool IsPossibleToAddOrder(string instrument);


        public abstract bool IsReadyForRecalcBots();

        public abstract decimal GetOrdersBacking(string instrument, decimal price,
                                                    decimal amount);


        
          


        public virtual void Process()
        {
            Log(String.Format("Process {0}..............................................................", _name));

            CreatePricePrecisions();

            CUtil.ThreadStart(ThreadOnceAHourLogics);
            CUtil.ThreadStart(ThreadOnceASecondLogics);

            Log("Create session box");
            CreateSessionBox();

            //changed 2017_07_01 it was AFTER TaskCheckUnsavedSessionsAndClearing before

            //Here we create DBCommunicator, ReportDispatcher and Messenger             
            CreateExternalComponents();



            CUtil.TaskStart(TaskLoadBotsConfig);
            CUtil.TaskStart (SessionBox.TaskCheckUnsavedSessionsAndClearing);
            

            Log("Create UserDealsPosBox");    
            CreateUserDealsPosBox();
            Log("Create ClearingProcessor");
            CreateClearingProcessor();

            CUtil.TaskStart(TaskLoadDataFromDB);
            CUtil.TaskStart(LoadDataInstruments);
            Log("StartGateIfNeed");
            StartGateIfNeed();
            Log("CreateBots");
            CreateBots();
            CUtil.TaskStart(TaskCheckInstrumentsAndBotsConsistent); 
 
			Log("Create  UserOrderBox ");
			CreateUserOrderBox();
        }


        public virtual List<int> GetPricePrecisions()
        {
            return _pricePrecisions;
        }



        protected virtual void CreatePricePrecisions()
        {
            _pricePrecisions = new List<int>()
            {
                0
            };

        }



        
        
        //always true by default
        public virtual bool IsActualSessionNumber(int sessNum)
        {
            return true;

        }



		protected void StartTradeManagerServer()
		{
			if (_useTradeManagerServer)
				CUtil.ThreadStart(ThreadOnStartTradeMgrServ);
					
		}


        public void UpdateStepPrice(string instrument, decimal newStepPrice)
        {
            DBCommunicator.QueueData(
                new CDBUpdateStepPrice {  Instrument =instrument,
                                           NewStepPrice = newStepPrice,
                                           StockExchId = StockExchId
                                       }
                
                
                );
        }





        private void ThreadOnStartTradeMgrServ()
		{


            //while (!IsPositionOnline)
            while (!IsReadyStartTrdMgrServ)
                Thread.Sleep(100);


			_tradeManagerServer = new CTradeManagerServer(this);
            // Before start 
            UpdateTotalInstrumentPosition();
            TriggerRecalcAllBots(EnmBotEventCode.OnForceUpdTrdMgr,null);
			_tradeManagerServer.Process();
		/*	while (true)
			{
				try
				{
					


				}
				catch (Exception e)
				{
					Error("CBaseDealingServer.ThreadTradeManagerServ", e);
				}
			}
			*/
		}




        private void ThreadOnceAHourLogics()
        {
            WaitDataLoadedFromDB();


            while (true)
            {
                //todo set 7
                if ((DateTime.Now - DBCommunicator.DtLastExcute).TotalHours > 1)
                    DBCommunicator.DummySelect();


                Thread.Sleep(1 * 60 * 60 * 1000);
            }


        }


        private DateTime _dtLstTmChkBotsConfig = new DateTime(0);
        private int _parFreqChkBotConfSec = 10;//tempo do set to 10

        private void ThreadOnceASecondLogics()
        {

            //WaitBotConfigLoaded();

            while (true)
            {
                try
                {
                    if (IsAllBotLoaded)
                    {
                        if ((DateTime.Now - _dtLstTmChkBotsConfig).TotalSeconds > _parFreqChkBotConfSec)
                        {
                            _dtLstTmChkBotsConfig = DateTime.Now;
                            CheckForNewAddedTraders();
                            CheckForMoneyChanged();
                            CheckForAccountTradesChanged();
                        }
                }

                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Error("ThreadOnceASecondLogics", e);
                    Thread.Sleep(1000);
                }
            }

        }

        private void CheckForNewAddedTraders()
        {

            if (!IsAddTraderOnlineAllowed)
                return;

             //Wait till initial load on start completes
             if (!_isBotInitiallyCreatedOnStart)
                return;


           //Load data from db
           List<Dictionary<string, object>> lstBotConfDB = DBCommunicator.LoadBotsConfig(StockExchId);



            List<Dictionary<string, object>> lstOnlineCopy = new List <Dictionary<string, object>>();

            //get list of bots that are actually using
            lock (LstBotsConfig)            
                foreach (var botConf in _lstBotsConfig)                
                    lstOnlineCopy.Add(new Dictionary<string, object>(botConf));
                
                       
            Dictionary<string, object> confToAdd = null;
          

            //compare online data with db
            foreach (var conf in lstBotConfDB)
            {
                bool bNotFound = true;
                int j = 0;
                foreach (var confOnl in lstOnlineCopy)
                //if (!lstOnlineCopy.Contains(Convert.ToInt32(conf["number"])))
                {
                  
                    if (Convert.ToInt32(conf["number"]) == Convert.ToInt32(confOnl["number"]))
                    //confToAdd = conf;
                    {
                        
                    
                        //compare MaxLossVMClosedTotal" change if need
                        if (Convert.ToDecimal(conf["MaxLossVMClosedTotal"]) 
                            != Convert.ToDecimal(confOnl["MaxLossVMClosedTotal"]))
                        {
                            lock(LstBotsConfig)                       
                                LstBotsConfig[j] = conf;

                            Log(String.Format("Changed BotId={0} MaxLossVMClosedTotal={1}",
                                            Convert.ToInt32(confOnl["number"]), Convert.ToDecimal(conf["MaxLossVMClosedTotal"])));
                            
                            //added 2018-11-24
                            DictBots[Convert.ToInt32(conf["number"])].UpdateMaxLossVMClosedTotal(Convert.ToDecimal(conf["MaxLossVMClosedTotal"]));
                        }
                        bNotFound = false;
                        break;

                    }
                    j++;
                }
                if (bNotFound)
                    confToAdd = conf;

                j++;
            }

            //if new el was found
            if (confToAdd != null)
            {
                int botId = Convert.ToInt32(confToAdd["number"]);

                Log(String.Format("CheckForNewAddedTraders.New trader found botId={0}", botId));

                //Add botConf to onlne value
                List<CBotBase> listBots = ListBots;
                lock (LstBotsConfig)
                    _lstBotsConfig.Add(confToAdd);


             
                
                //load instrument-based conf from db
                List<Dictionary<string, object>> lstBotInstrConfigDB = DBCommunicator.LoadBotInstrumentConfig(StockExchId);


                //compare instrument-base comp from db with online values
                var diff =   lstBotInstrConfigDB.FindAll(el => Convert.ToInt32(el["BotId"]) == botId);

                //Do add new elements to online values
                lock (LstBotsInstrumentsConfig)
                    foreach (var el in diff)
                        LstBotsInstrumentsConfig.Add(el);


                Dictionary<int, CDBAccountMoney> accountsMoneyDB = new Dictionary<int, CDBAccountMoney>();

                //update accounts money 
                DBCommunicator.LoadMoneyDataGeneric<CDBAccountMoney>(DBCommunicator.LoadAccountsMoney(), accountsMoneyDB);

                lock (AccountsMoney)
                    AccountsMoney[botId] =accountsMoneyDB[botId];







                //update accounts trade
                Dictionary<int, CDBAccountTrade> accountsTradeDB = new Dictionary<int, CDBAccountTrade>();

                DBCommunicator.LoadMoneyDataGeneric<CDBAccountTrade>(DBCommunicator.LoadAccountsTrade(StockExchId), accountsTradeDB);

                lock (AccountsTrade)
                    AccountsTrade[botId] = accountsTradeDB[botId];



                lock (UserDealsPosBox.AcconuntsFeeProc)            
                    UserDealsPosBox.AcconuntsFeeProc[botId] = accountsTradeDB[botId].proc_profit;




                //Now do create new bot
                Log("CheckForNewAddedTraders. Creating bot starting");
                BotFactory.CreateOneBotFromConfig(confToAdd, lstBotInstrConfigDB, botId, ref listBots, true);
                Log("CheckForNewAddedTraders. Creating bot finished");

                //Set m_positionOnlineRecieve flag (one of condition to recalc)
                DictBots[botId].ForcePosOnlineRecieved();

               
                UserDealsPosBox.OnAddNewBot(botId);

                TriggerRecalculateBot(botId, "", EnmBotEventCode.OnForceUpdTrdMgr, null);

                Log("CheckForNewAddedTraders. Finished");
            }

         




        }




        private void CheckForMoneyChanged()
        {
            Dictionary<int, CDBAccountMoney> accountsMoneyDB = new Dictionary<int, CDBAccountMoney>();
            
            
            DBCommunicator.LoadMoneyDataGeneric<CDBAccountMoney>(DBCommunicator.LoadAccountsMoney(), accountsMoneyDB);

            int botIdFound = -1;
            decimal moneyNew = 0;
            decimal moneyOld = 0;
            lock (AccountsMoney)
            {
                foreach (var kvp in accountsMoneyDB)
                {
                    if (AccountsMoney[kvp.Key].money_avail != kvp.Value.money_avail)
                    {
                      botIdFound = kvp.Key;
                      moneyOld = AccountsMoney[botIdFound].money_avail;
                      moneyNew = kvp.Value.money_avail;
                      AccountsMoney[botIdFound].money_avail = moneyNew;
                      break;
                    }

                }

            }

            if (botIdFound > 0)
            {
                Log(String.Format("Update money botId={0} {1} => {2}",
                    botIdFound, moneyOld, moneyNew));

                Dictionary<int, CDBAccountTrade> accountsTradeDB = new Dictionary<int, CDBAccountTrade>();

                DBCommunicator.LoadMoneyDataGeneric<CDBAccountTrade>(DBCommunicator.LoadAccountsTrade(StockExchId), accountsTradeDB);


                lock (AccountsTrade)
                {
                    AccountsTrade[botIdFound] = accountsTradeDB[botIdFound];
                }


            }



        }

        private void CheckForAccountTradesChanged()
        {
            Dictionary<int, CDBAccountTrade> accountsTradeDB = new Dictionary<int, CDBAccountTrade>();

            DBCommunicator.LoadMoneyDataGeneric<CDBAccountTrade>(DBCommunicator.LoadAccountsTrade(StockExchId), accountsTradeDB);

            decimal moneyAvailOld = -1;
            decimal moneyAvailChanged = -1;
            decimal leverageOld = -1;
            decimal leverageChanged = -1;
            decimal procProfitOld = -1;
            decimal procProfitChanged = -1;
            decimal procFeeDealingOld = -1;
            decimal procFeeDealingChanged = -1;

            int botId = -1;


            lock (AccountsTrade)
            {
                foreach (var kvp in accountsTradeDB)
                {
                    bool changed = false;
                    if (AccountsTrade[kvp.Key].money_avail != kvp.Value.money_avail)
                    {
                        changed = true;
                        botId = kvp.Key;
                        moneyAvailOld = AccountsTrade[kvp.Key].money_avail;
                        moneyAvailChanged = kvp.Value.money_avail;
                    }

                    if (AccountsTrade[kvp.Key].leverage != kvp.Value.leverage)
                    {
                        changed = true;
                        botId = kvp.Key;
                        leverageOld = AccountsTrade[kvp.Key].leverage;
                        leverageChanged = kvp.Value.leverage;                        
                       

                    }

                    if (AccountsTrade[kvp.Key].proc_profit != kvp.Value.proc_profit)
                    {
                        changed = true;
                        botId = kvp.Key;
                        procProfitOld = AccountsTrade[kvp.Key].proc_profit;
                        procProfitChanged = kvp.Value.proc_profit;
                        
                    }

                    if (AccountsTrade[kvp.Key].proc_fee_dealing != kvp.Value.proc_fee_dealing)
                    {
                        changed = true;
                        botId = kvp.Key;
                        procFeeDealingOld = AccountsTrade[kvp.Key].proc_fee_dealing;
                        procFeeDealingChanged = kvp.Value.proc_profit;

                    }



                    if (changed)
                    {
                        AccountsTrade[kvp.Key] = kvp.Value;
                        break;
                    }
                }           
            }

            if (moneyAvailChanged > 0)            
                Log(String.Format("moneyAvailChanged botId={0} {1}=>{2}",
                                    botId, moneyAvailOld, moneyAvailChanged));
            

            if (leverageChanged > 0)            
                Log(String.Format("leverageChanged botId={0} {1}=>{2}",
                                    botId,  leverageOld, leverageChanged));

            

            if (procProfitChanged >0)
                Log(String.Format("procProfitChanged  botId={0} {1}=>{2}",
                                    botId, procProfitOld, procProfitChanged));


            if (procFeeDealingChanged > 0)
                Log(String.Format("procFeeDealingChanged  botId={0} {1}=>{2}",
                                    botId, procFeeDealingOld, procFeeDealingChanged));





        }

        





        public decimal GetSessionLimit(int BotId)
		{
			lock (LstBotsConfig)// _lstBotsConfig) //2018-08-27
			{
				decimal res = 0;
				foreach (var kvp in _lstBotsConfig)
				{
					object y = kvp["number"];
					if (y != null)
						Thread.Sleep(0);


					if ((int)kvp["number"] == BotId)
						res =  (decimal)kvp["MaxLossVMClosedTotal"];
				}
				return res;

			}
			//listbo


		}

        public void WaitBotConfigLoaded()
        {
            _evBotConfigLoaded.WaitOne();
        }


        public void SetServerTimeAvailable()
        {
            if (!_bServerTimeAvailable)
            {
                _bServerTimeAvailable = true;
                _evServerTimeAvailable.Set();
            }


        }


        public void WaitServerTimeAvailable()
        {
            _evServerTimeAvailable.WaitOne();

        }




        public List<string> GetInsruments()
        {

            return Instruments.GetInstruments();

        }



		public virtual bool IsStockAvailable(string instrument)
		{
			if (StockBox == null || !StockBox.IsStockAvailable(instrument))
				return false;

		
			return true;
		}


        public void UpdateBotsDisableTradingByTime()
        {
            Dictionary<string, long> dict = Instruments.GetDisableTradingCodes();

            foreach (var kvp in dict)
                foreach (CBotBase bb in ListBots)
                {

                    CDisableTradeData dtl = new CDisableTradeData { TradeDisableCode = kvp.Value, DtDisable = _dtTradeDisabled };
                    bb.Recalc(kvp.Key, EnmBotEventCode.OnTimeDisableTradeLoaded, dtl);
                   
                }


            



        }

        public virtual void ChangePassword(string currPassword, string newPassword)
        {


        }


        /// <summary>
        /// Call instruments loading from database when ready. 
        /// Calls from CBaseDealingServer.Process()
        /// </summary>
        public void LoadDataInstruments()
        {
            DBCommunicator.WaitDatabaseConnected();
            DBCommunicator.WaitReadyForOperations();


            Instruments.LoadDataFromDB();


        }



        /// <summary>
        /// Waits till DB is available than loads 
        /// data from database
        /// 
        /// Call from this.Process()
        /// </summary>
        private void TaskLoadDataFromDB()
        {

            try
            {
                Log("TaskLoadDataFromDB entry");

                DBCommunicator.WaitDatabaseConnected();
                DBCommunicator.WaitReadyForOperations();


                LoadDataFromDB();

                Log("TaskLoadDataFromDB exit");

            }
            catch (Exception e)
            {
                Error("TaskLoadDataFromDB", e);

            }



        }

		protected void CreateSnapshoters()
		{
			SnapshoterStock = new CSnapshoterStock(this, "StockSnapshoter",Convert.ToInt32(GlobalConfig.StockDepth),100);
			SnapshoterDeals = new CSnapshoterDeals(this, "DealsSnapshoter", 100);


		}

        private void TaskLoadBotsConfig()
        {

            DBCommunicator.WaitReadyForOperations();

            lock (LstBotsConfig)
            {
                _lstBotsConfig = DBCommunicator.LoadBotsConfig(StockExchId);
            }
            _lstBotsInstrumentsConfig = DBCommunicator.LoadBotInstrumentConfig(StockExchId);

            _evBotConfigLoaded.Set();

        }

        /// <summary>
        /// Call from LoadDataFromDB
        /// </summary>
        private void LoadMoneyData()
        {

            //TODO lock-minimized
            lock (AccountsMoney)
                DBCommunicator.LoadMoneyDataGeneric<CDBAccountMoney>(DBCommunicator.LoadAccountsMoney(), AccountsMoney);

            lock (AccountsTrade)
                DBCommunicator.LoadMoneyDataGeneric<CDBAccountTrade>(DBCommunicator.LoadAccountsTrade(StockExchId), AccountsTrade);

            WaitBotConfigLoaded();
           
            //if proc profit exists (for traders) just copy it
            foreach (var kvp in AccountsTrade)
                UserDealsPosBox.AcconuntsFeeProc[kvp.Key] = kvp.Value.proc_profit;


            //if not exists do fill with zeroes
            lock (LstBotsConfig)
            {
                foreach (var el in _lstBotsConfig)
                {
                    int id = Convert.ToInt32(el["number"]);
                    if (!UserDealsPosBox.AcconuntsFeeProc.ContainsKey(id))
                        UserDealsPosBox.AcconuntsFeeProc[id] = 0;
                }
            }

            IsLoadedMoneyData = true;

           

        }




        public decimal GetAccountTradeMoney(int botId)
        {
           
            lock (AccountsTrade)
            {
                
                if (AccountsTrade.ContainsKey(botId))
                    return AccountsTrade[botId].money_avail;
                else
                {
                    Error("GetAccountTradeMoney. No botId=" + botId);
                    return 0m;
                }
            }

            
        }




        public void CreateTCPServerAndTradersDispatcher()
		{
			if (GlobalConfig.IsTradingServer)
			{  

				_tradersDispatcher = new CTradersDispatcher(this);
				_tcpServer = new CTCPServer(_portListening , _tradersDispatcher, "TradersDispatcher");

				_tradersDispatcher.SetTCPServer(_tcpServer);

			}
		}




        /// <summary> 
        /// Load money data (accounts)
        /// Load trade data (BotPosLog and UserDealsLog)
		/// 
        /// Call when  1)On start
        ///            2)After automatic clearing
        ///            
        /// Call from 1)CClearingProcessor.ProcessAutomaticClearing
        ///           2)CBaseDealingServer.TaskLoadDataFromDB (call on start)
        /// </summary>                          
        public virtual void LoadDataFromDB()
        {


            Log("LoadDataFromDB entry");
            LoadMoneyData();
            //KAA added 2018-03-27. Instruments are primary for trade data.
            //This fix error when build
            Instruments.WaitInstrumentsLoaded();
            //end
            UserDealsPosBox.LoadTradeData();
            LoadTimeTradeDisabled();
            //IsDataLoadedFromDB = true;
            _evDataLoadedFromDB.Set();
            _evTradeDisabledByTimeLoaded.Set();


            Log("LoadDataFromDB exit");
        }


        public void WaitTradeDisableByTimeLoaded()
        {

            _evTradeDisabledByTimeLoaded.WaitOne();

        }



        private void LoadTimeTradeDisabled()
        {
            _dtTradeDisabled = DateTime.Now.Date.Add(DBCommunicator.LoadTimeTradeDisable(StockExchId));
                       
        }





        private void CreateSunchronizationObjects()
        {
            EvStockOnline = new ManualResetEvent(false);
            EvDealsOnline = new ManualResetEvent(false);
            EvPosOnline = new ManualResetEvent(false);
            _evTradeDisabledByTimeLoaded = new ManualResetEvent(false);
            _evDataLoadedFromDB = new ManualResetEvent(false);
        }

        public void WaitDataLoadedFromDB()
        {
            _evDataLoadedFromDB.WaitOne();

        }




		protected  void CreateClearingProcessor()
		{
			ClearingProcessor = new CClearingProcessor(this);
		}

		public void UpdateMoneyData()
		{

			if (TradersDispatcher != null)
				TradersDispatcher.UpdateMoneyData();
		}


        public void Error(string description, Exception exception = null)
        {
            Log("ERRROR ! "+description);
            Alarmer.Error(description, exception);
        }

        protected void ReadConfig()
        {

            string pathFile = CUtil.GetConfigDir() + @"\globalSettings.xml"; //System.AppDomain.CurrentDomain.BaseDirectory + @"config\globalSettings.xml";
            _globalConfig = new CGlobalConfig(pathFile);
            CSerializator.Read<CGlobalConfig>(ref _globalConfig);

            _isGlobalConfigAvail = true;

            //UseRealServer = GlobalConfig.UseRealServer;

            //SubscribedIsins = GlobalConfig.ListIsins;
            AnalzyeTimeFrames = GlobalConfig.AnalzyeTimeFrames;
            StockExchId = GlobalConfig.StockExchId;
            _portListening = GlobalConfig.PortListening;
            _portTradeManager = GlobalConfig.PortTradeManager;
        }

         public void Log(string message)
         {
             _logger.Log(message);
         }





		 public void UnloadBot(long id)
		 {

			 m_botFactory.UnloadBot(id);

		 }

		 public void LoadBot(long id)
		 {

			 m_botFactory.LoadBot(id);

		 }


		 public void EnableBot(long id)
		 {
			 m_botFactory.EnableBot(id);
		 }



		 public void DisableBot(long id)
		 {
			 m_botFactory.DisableBot(id);
		 }


		 public CBotBase GetBotById(long id)
		 {
			 if (!DictBots.ContainsKey(id))
				 return null;
			 else
				 return DictBots[id];

		 }


		 public string GetDataPath()
		 {
			 return CUtil.GetDataDir();
		 }

		 public IGUIBot CreateGUIBot(IDealingServer dealingServer, long botId)
		 {
			 return new CGUIBot((IClientGUIBot) this, botId);

		 }



		 protected  void CreateBots()
		 {
             
			 m_botFactory = new CBotFactory(this);
			 m_botFactory.CreateBots();

            _isBotInitiallyCreatedOnStart = true;


         }


         public DateTime ServerTimeLocal()
         {
             return DateTime.Now.AddHours(m_parLocalOffset);

         }

         public void OnSessionActivate()
         {
             Log("On session activate");
         }


         public void OnSessionDeactivate()
         {
             Log("On session nonactive");
         }

         public void OnEnableCancellOrders()
         {
             Log("On enable cancell orders");

         }
         public void OnDisableCancellOrders()
         {
             Log("On disable cancell orders");
         }

         public void OnIntradeyClearingBegin()
         {
             Log("On intraday clearing begin");
         }

         public void OnDaySessionExpired()
         {
             Log("On moning session expired");
             //(new Task(ProcessAutomaticClearing)).Start();
         }


         public void OnIntradayClearingEnd()
         {
             Log("On intraday clearing end");
         }

         public void OnEveningClearingBegin()
         {
             Log("On evening clearing begin");

         }

         public void OnEveningClearingEnd()
         {
             Log("On main clearing end");
         }
         public void OnNightStarted()
         {
             Log("On night started");
         }

         public void OnNightEnded()
         {
             Log("On night ended");

         }

         public void SendReports()
         {
             ReportDispatcher.GenReports();
         }

         public void UpdateTradersPosLog(int extId)
         {

             if (TradersDispatcher != null)
                 TradersDispatcher.EnqueueUpdateUserPosLog(extId);


         }


         public void UpdateGUIDealCollection(CRawUserDeal rd)
         {

             //string isinDeal = Instruments.GetInstrumentByIsinId(rd.Instrument);
             string instrument = rd.Instrument;

             long botId = 0;
             if (rd.Ext_id_buy != 0)
                 botId = rd.Ext_id_buy;
             else if (rd.Ext_id_sell != 0)
                 botId = rd.Ext_id_sell;
             else
                 return;

             //no bot with id - return (i.e. supervisor bot case)
             if (!DictBots.ContainsKey(botId))
                 return;


             CBotBase bot = DictBots[botId];
             foreach (var isin in bot.SettingsBot.ListIsins)
                 if (isin == instrument
                     && IsSessionOnline
                     && rd.Moment > SessionBox.CurrentSession.SessionBegin)
                 {


                     /* bot.GUIBot.UserDealsCollection.mx.WaitOne();
                      bot.GUIBot.UserDealsCollection.Add(rd, isinDeal);
                      bot.GUIBot.UserDealsCollection.mx.ReleaseMutex();
                      */
                     //2017-04-26
                     bot.GUIBot.UpdateDeal(rd, instrument);

                 }



         }


         public void UpdateDBUserDealsLog(CDBUserDeal userDeal)
         {
             DBCommunicator.QueueData(userDeal);
         }

        
         public void UpdateDBPosLog(long accountId, int stockExchId, string Instrument,
                                    CBotPos botPos)
         {


             // Action<object> act = new Action<object>
             DBCommunicator.QueueData(
                 (
                 new CDBUserPosLog
                 {
                     PriceOpen = botPos.PriceOpen,
                     PriceClose = botPos.PriceClose,
                     BuySell = botPos.BuySell,
                     DtOpen = botPos.DtOpen,
                     DtClose = botPos.DtClose,
                     CloseAmount = botPos.CloseAmount,
                     VMClosed_Points = botPos.VMClosed_Points,
                     VMClosed_Steps = botPos.VMClosed_Steps,
                     VMClosed_RUB = botPos.VMClosed_RUB,
                     VMClosed_RUB_clean = botPos.VMClosed_RUB_clean,
                     VMClosed_RUB_user = botPos.VMClosed_RUB_user,
                     Fee = botPos.Fee,
                     Fee_Stock = botPos.Fee_Stock,
                     Fee_Total = botPos.Fee_Total,
                     Fee_Profit = botPos.Fee_Profit,
                     account_trade_Id = (int)accountId,
                     Instrument = Instrument,
                     stock_exch_id = stockExchId,
                     ReplId = botPos.ReplIdClosed





                 })


                 );





         }
        

         public void TriggerRecalcAllBotsWithInstrument(string instrument, EnmBotEventCode evnt, object data)
         {
             foreach (CBotBase bb in this.ListBots)
                 foreach (string isn in bb.SettingsBot.ListIsins)
                     if (instrument == isn)
                         bb.Recalc(instrument, evnt, data);

         }


         public void TriggerRecalcAllBots(EnmBotEventCode evnt, object data)
         {
             foreach (CBotBase bb in this.ListBots)
                 bb.Recalc("", evnt, data);


         }




         public void TriggerRecalculateBot(int botId, string isin, EnmBotEventCode code, object data)
         {

             try
             {
                 foreach (CBotBase bot in ListBots)
                 {
                     if (bot.BotId == botId)
                         bot.Recalc(isin, code, data);

                 }

             }

             catch (Exception e)
             {
                 Error("Plaza2Connector.RecalculateBot", e);
             }
         }

         public virtual bool IsReadyRefreshBotPos()
         {

             if (IsStockOnline && IsDealsOnline &&
                                IsOnlineUserDeals)
                 return true;

             return false;

         }

         public void GUIBotUpdatePosLog(CBotPos BotPos, int extId)
         {
             foreach (CBotBase bot in ListBots)
                 if (bot.BotId == extId)
                     bot.GUIBot.UpdatePosLog(BotPos);

         }


         public void GUIBotUpdateMonitorPos(CBotPos bp, string isin, int botId)
         {
             foreach (CBotBase bb in ListBots)
             {
                 if (bb.BotId == botId && bb.GUIBot != null)
                     bb.GUIBot.UpdateMonitorPos(isin, bp);
             }

         }


         public string GetTicker(long id)
         {
             //return DictIsin_id[id];
             return Instruments.GetInstrumentByIsinId(id);

         }

         public decimal GetMinOrderSize(string instrument)
         {
            return Instruments.GetMinOrderSize(instrument);

        } 

         public virtual decimal GetStepPrice(string instrument)
         {
             // lock (DictStepPrice)

             //modification 2017-08-11
             //Undo modification 2017-11-27. Step price could change from sessio to session.

             return DictStepPrice[instrument];
             //return Instruments.GetStepPrice(instrument);

          
         }

         public virtual decimal GetMinStep(string instrument)
         {
             //return DictMinStep[instrument];

             //modification 2017-08-11
             return Instruments.GetMinStep(instrument);

         }
        
		public virtual int GetDecimals(string instrument)
		{
			return Instruments.GetDecimals(instrument);
		}


		public virtual int GetDecimalVolume(string instrument)
		{
			return Instruments.GetDecimalVolume(instrument);
		}






         public decimal GetBid(string instrument)
         {

             if (StockBox != null)
                 return StockBox.GetBid(instrument);


             return 0;

         }

         public decimal GetAsk(string instrument)
         {

             if (StockBox != null)
                 return StockBox.GetAsk(instrument);


             return 0;

         }

         public virtual long GetLotSize(string instrument)
         {
             //lotsize is 1 by default
             return 1;
         }


         public virtual decimal GetMaxPrice(string instrument)
         {
             //do not check in fact
             return 10000000000;
         }

         public virtual decimal GetMinPrice(string instrument)
         {
             //do not check in fact
             return -10000000000;
         }


         public bool IsInstrumentExist(string instrument)
         {
            return  Instruments.IsContainsInstrument(instrument);

           
         }




		 public void UpdateTradersDeals(string isin)
		 {
			 try
			 {
				 if (GlobalConfig.IsTradingServer && _tradersDispatcher != null && IsDealsOnline)
				 {
					 //  m_tradersDispatcher.EnqueueUpdateDealsCommand(isin);
					 _tradersDispatcher.UpdateTradersDeals(isin);

				 }
			 }
			 catch (Exception e)
			 {
				 Error("UpdateTradersStocks", e);

			 }


		 }


		 public void UpdateTradersStocks(string isin)
		 {

			 try
			 {

				 if (GlobalConfig.IsTradingServer && _tradersDispatcher != null && IsStockOnline)
				 {
					 //    m_tradersDispatcher.EnqueueUpdateStockCommand(isin);
					 _tradersDispatcher.UpdateTradersStocks(isin);
				 }
			 }
			 catch (Exception e)
			 {
				 Error("UpdateTradersStocks", e);

			 }

		 }

        public void AddClientInfo(CClientInfo ci)
        {
            ci.StockExchId = StockExchId;

            if (_tradeManagerServer!=null) //_tradeManagerServer was not started yet
                _tradeManagerServer.AddClientsInfo(ci);
        }

        public void DeleteClientInfo(int conId)
        {
            _tradeManagerServer.DeleteClientInfo(conId);

        }





        //2017-11-09
        private void TaskCheckInstrumentsAndBotsConsistent()
        {
            Instruments.WaitInstrumentsLoaded();
            WaitBotConfigLoaded();
            List<string> listInstr =  Instruments.GetInstruments();
            foreach (var botInstrConf in _lstBotsInstrumentsConfig)
            {
                 if (!listInstr.Contains ( botInstrConf["Instrument"]))
                     Error(String.Format("Database is inconsistent BotId={0} instrument {1} not exists in instruments ", 
                                          botInstrConf["BotId"],      botInstrConf["Instrument"]));                   
            }


        }

        public bool IsExistBotIdInstrument(int botId, string instrument)
        {
            foreach (var v in _lstBotsInstrumentsConfig)
            {
                if (v["Instrument"].ToString() == instrument && Convert.ToInt16(v["BotId"]) == botId)
                    return true;
            }
           


            return false;
        }


		public void WaitInstrumentLoaded()
		{
			Instruments.WaitInstrumentsLoaded();
		}


		/// <summary>
		/// Default converter for FORTS,ASTS
		/// </summary>
		/// <param name="inpVolume"></param>
		/// <returns></returns>
		public virtual int GetGUITradeSystemVolume(string instrument,string inpVolume)
		{
			return Convert.ToInt32(inpVolume);
		}


        public void UpdateWallet(string walletType, string currency, decimal balance)
        {

            _dtLastWaletUpdate = DateTime.UtcNow;
            _moneyCurrStockExch = balance;
            DBCommunicator.UpdateWalletLog(_dtLastWaletUpdate, walletType, currency, balance);

        }

        public bool IsAllUserPosClosed()
        {
            if (UserDealsPosBox == null)
                return false;


            return UserDealsPosBox.IsAllPosClosed();
        }

        public bool IsAllStockExchClosed()
        {
            if (PosBoxBase == null)
                return false;

            return PosBoxBase.IsAllPosClosed();

        }


        public decimal GetSessionProfit()
        {
            if (UserDealsPosBox == null)
                return 0;


            return UserDealsPosBox.GetSessionProfit();


        }

        public virtual int GetStockDepth(int precision)
        {
            return Convert.ToInt32(GlobalConfig.StockDepth);

        }
		
        public void UpdDBVMOpenedClosedTot(int accountId, decimal vmAllInstrOpenedAndClosed)
        {
            CDBUpdVMOpenedClosedTot data = new CDBUpdVMOpenedClosedTot
            {
                AccountId = accountId,
                StockExchId = StockExchId,
                VMAllInstrOpenedAndClosed = vmAllInstrOpenedAndClosed

            };



            DBCommunicator.QueueData(data);
        }


        public void UpdDBPosInstr(int botId,  string instrument, decimal amount, decimal avPos)
        {
            DBCommunicator.QueueData(new CDBUpdPosInstr
            {
                account_id = botId,
                instrument = instrument,
                amount = amount,
                AvPos = avPos,
                stock_exch_id = StockExchId,
                Dt_upd = DateTime.UtcNow
            });

        }


        /// <summary>
        /// Calc turnovers in db and 
        /// update fields in db. 
        /// </summary>
        public void UpdateTurnOver()
        {
            DateTime dt = DateTime.Now.AddDays(-30);
            //calc turnovers from db
            _lstTurnOvers = DBCommunicator.GetTradersTurnover(dt);
            //update tunovers field in db
            if (_lstTurnOvers.Count>0)
                DBCommunicator.UpdateTradersTurnover(_lstTurnOvers);

        }

        public virtual void OnTrdMgrSentReconnect(int channelId)
        {
            //nothing to do by default
        }



      


    }
}
