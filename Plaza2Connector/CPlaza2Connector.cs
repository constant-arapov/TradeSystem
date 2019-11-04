using System;
using System.Collections.Generic;

using System.Text;

using ru.micexrts.cgate;
using System.Runtime.InteropServices;
using ru.micexrts.cgate.message;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Threading;

using Common;
using Common.Interfaces;
using Common.Collections;
using Common.Logger;
using Common.Utils;


using TradingLib;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Interaction;
using TradingLib.Abstract;
using TradingLib.Enums;
using TradingLib.BotEvents;
using TradingLib.Data;
using TradingLib.Data.DB;
using TradingLib.Data.DB.Interfaces;
using TradingLib.ProtoTradingStructs;
using TradingLib.Bots;
using TradingLib.Common;
using TradingLib.Snapshoters;
using TradingLib.GUI;
using TradingLib.TradersDispatcher;


using DBCommunicator;
using DBCommunicator.Interfaces;
//using DBCommunicator.DBData;

using TCPLib;
using ReportDispatcher;

using Messenger;
using ComonentFactory;

using P2ConnectorNativeImp;
using P2ConnectorNativeImp.Interfaces;

using Plaza2Connector.Interfaces;
using Plaza2Connector.Simulator;


//using Plaza2Connector.Interfaces;

//using Plaza2Connector.ClearingProcessor;




namespace Plaza2Connector
{
 
    public  class CPlaza2Connector :   CBaseDealingServer,  IAlarmable, ILogable, IDealingServer,   IGUIDispatcherable,IClientUserDealsPosBox, IClientDBCommunicator,
                                        IClearingProcessorClient, IClientInstruments,IClientGUICandleBox, IClientGUIBox, IClientGUIBot,
                                        IClientStockConverter, IClientStockStruct,  IClientTimeFrameAnalyzer, 
                                        IClientTradersDispatcher, IP2ConnectorNativeClient, IClientSessionBoxP2
    {

     


        //public CMySQLConnector MySQLConnector;
      


        public  int SupervisorBotId = 10;


        private long m_lastDealRevId =0;

        public long LastDealRevId
        {
            get 
            {
                return m_lastDealRevId;
            }
            set
            {
                m_lastDealRevId = value;
            }

        }

        private long m_lastDealLifeNum = 0;

        public long LastDealLifeNum
        {
            get
            {
                return m_lastDealLifeNum;
            }
            set
            {
                m_lastDealLifeNum = value;
            }

        }

           private long m_lastASTSCurrDealLifeNum = 0;

           public long LastASTSCurrDealLifeNum 
           {
               get
               {
                   return m_lastASTSCurrDealLifeNum;
               }
               set
               {
                   m_lastASTSCurrDealLifeNum = value;
               }

           }




        private long m_lastASTSCurrDealRevId = 0;

        public long LastASTSCurrDealRevId
        {
            get
            {
                return m_lastASTSCurrDealRevId;
            }
            set
            {
                m_lastASTSCurrDealRevId = value;
            }



        }


        public long  m_lastStockRevId =0;//{ set; get; }

        public long LastStockRevId
        {
            get
            {
                return m_lastStockRevId;
            }
            set
            {
                m_lastStockRevId = value;

            }


        }


        public override CBasePosBox PosBoxBase
        {
            get
            {
                return _positionBox;
            }
        }


        
        public long LastUserOrderLogRevId { set; get; }
        public long LastUserDealRevId { set; get; }



         System.Threading.Mutex m_mutexRawStock = new System.Threading.Mutex();

         private List<CRawStock> m_rawStocks = new List<CRawStock>();

         


         public CLogServer LogServer { get; set; }

         //public List <string> SubscribedIsins = new  List <string> ();

         
       
         

         System.Threading.Mutex m_mutexInpBufferStock = new System.Threading.Mutex();
         private List<CRawStock> m_inpBufferStocks = new List<CRawStock>();

  
         private List<CRawStock> m_outpBufferStocks = new List<CRawStock>();



		 


         System.Threading.Mutex m_mutexStock = new System.Threading.Mutex();
		/*
         private Dictionary<Direction, List<CStock>> m_stock = new  Dictionary<Direction, List<CStock>>() { {Direction.Up, new List<CStock>()}, {Direction.Down, new List<CStock>()  }};
		*/

		 private CStockBox _stockBox;


		
         public override IStockBox  StockBox
		 {
			 get
			 {
				 return _stockBox;
			 }
		 
		 }

		public IStockBoxForP2Connector StockBoxInp
		{
			get
			{
				return _stockBox;
			}

		}

        public CStockBox StockBoxP2
        {
            get
            {

                return _stockBox;
            }


        }


         
        public override bool IsReadyStartTrdMgrServ
        {
            get
            {
                return IsPositionOnline;
            }

        }
       

     




		 private CUserOrderBox _userOrderBox;
         public  override IUserOrderBox UserOrderBox
		 {
			 get
			 {
				 return _userOrderBox;
			 }
		 }

		 public IUserOrdersBoxForP2Connector UserOrderBoxInp
		 {
			 get
			 {
				 return _userOrderBox;
			 }

		 }

		 private CDealBox _dealsBox;
		 public override IDealBox DealBox
		 {
			 get
			 {
				 return _dealsBox;
			 }

		 }
		 public IDealBoxForP2Connector DealBoxInp
		 {
			 get
			 {
				 return _dealsBox;
			 }
		 }



		 private CUserDealsPosBoxP2 _userDealsPosBox;
         //public /*CUserDealsPosBox*/  IUserDealsPosBox /*CBaseUserDealsPosBox*/ UserDealsPosBox{ set; get; }

		 public override IUserDealsPosBox UserDealsPosBox
		 {
			 get
			 {
				 return _userDealsPosBox;
			 }

		 }
		

		 public IUserdealsPosBoxForP2Connector UserDealsPosBoxInp
		 {
			 get
			 {
				 return _userDealsPosBox;
			 }

		 }

         private CPositionsBox _positionBox;

         public override IPositionBox PositionBox
         {
             get { return _positionBox; }
         }

         public CPositionsBox PositionBoxInp
         {
             get
             {
                 return _positionBox;

             }


         }
        


         public CPart Part;

		 public CSessionBox _sessionBox;
         public override ISessionBox SessionBox
		 {
			 get
			 {
				 return _sessionBox;
			 }
		  }
		 public ISessionBoxForP2Connector SessionBoxInp
		 {
			 get
			 {
				 return _sessionBox;
			 }

		 }

       
       

      


        



         public string Client_code  {set; get;}
         public string Broker_code {set; get;}


        //TO DO refact move to one dictionary




         private bool _readyForCreatePublishers = false;


         public long MemoryUsssage { get; set; }


         



         public bool IsLoadedSessionData { get; set; }


        



        
         public bool IsReadyForCreatePublishers         
          {
             get
             {
                 return _readyForCreatePublishers;
             }
             set
             {
                 _readyForCreatePublishers = value;
                 UpdateFORTSOnlineStatus();
                 if (GUIBox != null)
                     GUIBox.IsReadyForCreatePublishers = value;

             }

         }




         private bool _readyForSendOrder = false;
         public bool IsReadyForSendOrder 
          {
             get
             {
                 return _readyForSendOrder;
             }
             set
             {
                 _readyForSendOrder = value;
                 UpdateFORTSOnlineStatus();
                 if (GUIBox != null)
                     GUIBox.IsReadyForSendOrder = value;

             }

         }




         public bool _onlineUserOrderLog = false;
         public override bool IsOnlineUserOrderLog
         {
             get
             {
                 return _onlineUserOrderLog;
             }
             set
             {
                 _onlineUserOrderLog = value;
                 UpdateFORTSOnlineStatus();
                 if (GUIBox != null)
                     GUIBox.IsOnlineUserOrderLog = value;

             }

         }


         private bool _orderControlAvailable = false;      
         public override bool IsOrderControlAvailable 
         {
             get
             {
                 return _orderControlAvailable;
             }
             set
             {
                 _orderControlAvailable = value;
                 UpdateFORTSOnlineStatus();
                 if (GUIBox != null)
                     GUIBox.IsOrderControlAvailable = value;

             }

         }



         private bool _onlineUserDeals = false;       
         public override bool IsOnlineUserDeals
         {
             get
             {
                 return _onlineUserDeals;
             }
             set
             {
                 _onlineUserDeals = value;
                 UpdateFORTSOnlineStatus();
                 if (GUIBox != null)
                   GUIBox.IsOnlineUserDeals = value;

             }

         }


        private bool _sessionOnline = false;   
        public override bool IsSessionOnline
        {
            get
            {
                return _sessionOnline;
            }
            set
            {
                if (_sessionOnline == false &&
                    value == true)
                    (new Task(SessionBox.TaskCheckUnsavedSessionsAndClearing)).Start();

              
                _sessionOnline = value;
                UpdateFORTSOnlineStatus();
                if (GUIBox != null)
                    GUIBox.IsSessionOnline = _sessionOnline; //value;

               

            }

        }

       


        

        private bool _positionOnline  =false;        
        public override bool IsPositionOnline
        {
            get
            {
                return _positionOnline;
            }
            set
            {
                _positionOnline = value;
                UpdateFORTSOnlineStatus();
                if (GUIBox != null)
                    GUIBox.IsPositionOnline = value;

            }

        }



        private bool _stockOnline = false;
     
        public override bool IsStockOnline
        {
            get
            {
                return _stockOnline;
            }
            set
            {
                _stockOnline = value;
                UpdateFORTSOnlineStatus();
                if (GUIBox != null)
                    GUIBox.IsStockOnline = value;

            }

        }
        




         private bool _dealOnline = false;
         public override bool IsDealsOnline
         {
             get
             {
                 return _dealOnline;
             }
             set
             {
                 _dealOnline = value;
                 UpdateFORTSOnlineStatus();
                 if (GUIBox != null)
                     GUIBox.IsDealsOnline = value;
                 
             }

         }

        



         



        //TO DO add to GUI
         private bool _futInfoOnline = false;
         public override  bool IsFutInfoOnline
         {
             get
             {
                 return _futInfoOnline;
             }
             set
             {
                 _futInfoOnline = value;
                /* UpdateFORTSOnlineStatus();
                 if (GUIBox != null)
                     GUIBox.IsDealsOnline = value;
                 */
             }

         }




        


         private bool _sessionActive = false;     
         public override bool IsSessionActive 
         {
             get
             {
                 return _sessionActive;
             }
             set
             {
                 _sessionActive = value;
                 UpdateFORTSOnlineStatus();
                  if (GUIBox != null)
                      GUIBox.IsSessionActive = value;

             }

         }


         private bool _possibleToCancelOrders = false;     
         public override bool IsPossibleToCancelOrders
         {
             get
             {
                 return _possibleToCancelOrders;
             }
             set
             {
                 _possibleToCancelOrders = value;
                 UpdateFORTSOnlineStatus();
                  if (GUIBox != null)
                      GUIBox.IsPossibleToCancelOrders = value;

             }

         }


         private bool _sessionInClearing = false;
         public bool IsSessionInClearing   
         {
             get
             {
                 return _sessionInClearing;
             }
             set
             {
                 _sessionInClearing = value;
                 UpdateFORTSOnlineStatus();    
                  if (GUIBox != null)
                      GUIBox.IsSessionInClearing = value;

             }

         }




         private bool _serverTimeAvailable = false;
         public override bool IsServerTimeAvailable
         {
             get
             {
                 return _serverTimeAvailable;
             }
             set
             {
                 _serverTimeAvailable = value;
                 UpdateFORTSOnlineStatus();
                  if (GUIBox != null)
                      GUIBox.IsServerTimeAvailable = value;

             }

         }




         private bool _onlineVM = false;
         public override bool IsOnlineVM
         {
             get
             {
                 return _onlineVM;
             }
             set
             {
                 _onlineVM = value;
                 UpdateFORTSOnlineStatus();
                  if (GUIBox != null)
                      GUIBox.IsOnlineVM = value;

             }

         }

         private bool _FORTSOnline = false;
         public bool IsFORTSOnline
         {
             get
             {
                 return _FORTSOnline;
             }
             set
             {
                 _FORTSOnline = value;

                 if (GUIBox != null)
                     GUIBox.IsFORTSOnline = value;

             }

         }

        private bool _analyzerTFOnline = false;
        public override bool IsAnalyzerTFOnline
        {
            get
            {
                return _analyzerTFOnline;
            }
            set
            {
                _analyzerTFOnline = value;
                if (GUIBox != null)
                    GUIBox.IsAnalyzerTFOnline = value;

            }

        }

        public override bool IsPossibleNativeCancellOrdByInstr { get; set; }


        private bool _isDatabaseConnected;

        public bool IsDatabaseConnected
        {
            get
            {
                return _isDatabaseConnected;
            }
            set
            {
                _isDatabaseConnected = value;
                if (GUIBox != null)
                    GUIBox.IsDataBaseConnected = value;

            }


        }



        private bool _isDatabaseReadyForOperations;

        public bool IsDatabaseReadyForOperations
        {
            get
            {
                return _isDatabaseReadyForOperations;
            }
            set
            {
                _isDatabaseReadyForOperations = value;
            }

        }






		private bool _useNativeConnector = true;



		private CP2ConnectorNative _p2ConnectorNative;



       





        

        public DateTime DtSessionCurrentBegin;

		private CComponentFactory _componentFactory;

         List<CSettingsListener> m_heartBeatListenerSettings = new List<CSettingsListener>();
          


         List<CSettingsListener> m_infoListenersSettings = new List<CSettingsListener>();
         List<CSettingsListener> m_marketListenersSettings = new List<CSettingsListener>();
         List<CSettingsListener> m_stocksListenersSettings = new List<CSettingsListener>();

         List<CSettingsListener> m_userDataListenersSettings = new List<CSettingsListener>();



         List<CSettingsListener> m_orderControlReplyListenerSetting = new List<CSettingsListener>();
         List<CSettingsPublisher> m_orderControlPublisherSettings = new List<CSettingsPublisher>();


        //ASTS
         List<CSettingsListener> m_ASTSSpotListenerSettings = new List<CSettingsListener>();
         List<CSettingsListener> m_ASTSCurrListenerSettings = new List<CSettingsListener>();



         Dictionary<int, List<CSettingsListener>> m_dictOrderControlReplyListenerSetting = new Dictionary<int, List<CSettingsListener>>();
         Dictionary<int, List<CSettingsPublisher>> m_dictOrderControlPublisherSettings = new Dictionary<int, List<CSettingsPublisher>>();




         
      



         public static System.IO.StreamWriter outpFile;

		

         //private CRiskManager RiskManager;





         CConnection m_connHeartbeat;
         CConnection m_connInfoData;
         CConnection m_connMarketData;
         CConnection m_connStocksData;
         CConnection m_connOrderControl;
         CConnection m_connUserData;
         CConnection m_connASTSSpot;
         CConnection m_connASTSCurr;





         Dictionary<int, CConnection> m_dictConnOrderControl = new Dictionary<int, CConnection>();

		 private CP2Simulator _simulator;
        


         public override decimal GetMaxPrice(string instrument)
         {
             return DictFutLims[instrument].Max;
         }


         public override decimal GetMinPrice(string instrument)
         {
             return DictFutLims[instrument].Min;
         }




         public override bool IsPriceInLimits(string isin, decimal price)
         {
             //TODO Think what is to do on this situation ? Example TOD/TOM on FORTS
             if (!DictFutLims.ContainsKey(isin))
             {
                 Error("Try to send order which is not in DictFutLims. Not Possible to check limits");
                 return true;

             }


             if (price < DictFutLims[isin].Min || price > DictFutLims[isin].Max)
             {

                 Error("Price is grater than limits. min=" + DictFutLims[isin].Min + " max=" + DictFutLims[isin].Max);
                 return false;
             }

             return true;
         }
    


         private void UpdateFORTSOnlineStatus()
         {
             IsFORTSOnline = IsOnlineUserDeals && IsOnlineUserOrderLog && IsOnlineVM && IsOrderControlAvailable && IsPositionOnline && IsReadyForSendOrder && 
                 IsServerTimeAvailable &&IsSessionActive && IsStockOnline && IsDealsOnline;

         }






         public override bool IsPossibleToAddOrder(string instrument)
         {
             

             if (IsSessionActive)
                 return true;


             return false;

         }

     

         public void UpdateClientCode(string fullCode)
         {
            //tempo - till session table will avail
             Broker_code = fullCode.Substring(0, 4);
             Client_code = fullCode.Substring(4, 3);
             if (Broker_code!="" && Client_code!="")
             {
                 IsReadyForCreatePublishers = true;

             }
             
          


            
         }



         public override bool IsActualSessionNumber(int sessNum)
         {
             return SessionBox.CurrentSession.SessionNumber == sessNum;

         }




         public string GetActionString(int action)
         { 
             if (action == 0)
                 return "[Удалена]";
             else if (action == 1)
                 return "[Добавлена]";
             else if (action == 2)
                 return "[Cделка]";

             return action.ToString(); ;
         }


		


		 //TODO remove supervisorID
         public override void AddOrder(int botId, string isin, decimal price, EnmOrderDir dir, decimal dcmlAmount)
         {
             int iAmount = Convert.ToInt32(dcmlAmount);
             CCommandStucture cs = new CCommandStucture();

             cs.CommandName = "FutAddOrder";

             cs.Command = (object)new CCommandAddOrder(1, Broker_code, isin, Client_code, OrderTypes.Part, dir, iAmount, price.ToString(), botId);            
             //m_connOrderControl.CommandQueue.Add(cs);
             int connId = 0;
             /*if (supervisorID == 0)*/
                 connId = botId;
             /*else
                 connId = supervisorID;
			 */
             m_dictConnOrderControl[connId].CommandQueue.Add(cs);

             Log("Add new order dir="+dir+" amount=" + dcmlAmount + " price="+price +" botId="+botId);

         }

         



         public override void CancelOrder(long orderId, int botId)
         {
             CCommandStucture cs = new CCommandStucture();
             cs.CommandName = "FutDelOrder";
             cs.Command = new CCommandDelOrder(orderId);
            // m_connOrderControl.CommandQueue.Add(cs);
             m_dictConnOrderControl[botId].CommandQueue.Add(cs);


         }

         public override void CancelAllOrders(int buy_sell, int ext_id, string isin, int botId)
         {
             CCommandStucture cs = new CCommandStucture();
             cs.CommandName = "FutDelUserOrders";
             cs.Command = new CCommandDelUserOrders(buy_sell, ext_id, isin);
            // m_connOrderControl.CommandQueue.Add(cs);
             //m_dictConnOrderControl[botId].CommandQueue.Add(cs);
             m_dictConnOrderControl[botId].CommandQueue.Add(cs);
             Log("Cancel all orders. ext_id="+ext_id+" isin="+isin);



         }


         public void OnCrossOrderReply(int botId)
         {

             Log("Cross order found botId=botId");
             TriggerRecalculateBot(botId, "", EnmBotEventCode.OnCrossOrderReply, null);

         }


     
        
        
         //public  Dictionary<string, long> DictInstr_IsinId {get; set; }
         //public Dictionary<long, string> DictIsin_id { get; set; }



       
             
       


         public bool IsAllContolConnectionReady()
         {
            
              foreach (var v in m_dictConnOrderControl)
                  if (!v.Value.IsOrderControlReady) 
                      return false;

             return true;
         }

         public override bool IsReadyForRecalcBots()
         {
               if (IsOnlineUserDeals &&  IsOnlineUserOrderLog  && IsStockOnline 
               && IsOrderControlAvailable && IsPositionOnline && IsSessionOnline  &&
                 IsFutInfoOnline && IsDealsOnline && IsTimeSynchronized)
                   return true;

               return false;

         }



		 public long GetIsinIdByInstrument(string instrument)
		 {
			 return Instruments.GetIsinIdByInstrument(instrument);
		 }

		

         public CPlaza2Connector(): base("Plaza2Connector")
         {


			 _componentFactory = new CComponentFactory(_logger);


             //note. not possible to create in base class
             //Messenger = new CMessenger();


             StockExchId = CodesStockExch._01_MoexFORTS;
		
             UseRealServer = CUtil.GetEnvVariableBool("USE_REAL_PLAZA2_SERVER");
            

          

             Sounder = new CSounder();

            

             LogServer = new CLogServer();

           //  Instruments = new CListInstruments();

            //TO DO read last revisions from file   


             ReadDataFromFiles();

             (new System.Threading.Thread(ThreadDumpLastDataToFiles)).Start();

             (new Task(TaskRoundtripTime)).Start();


             //CreateGUIEvents();



             //StartGateIfNeed();
             OpenCgate();
           
            // (new System.Threading.Thread(ThreadResourcesAnalyzer)).Start();


            // string tmp = CUtil.GetTemp();

             IsPossibleEmptyInstrCancellOrders = true;
             IsPossibleNativeCancellOrdByInstr = true;

         }

        public override decimal GetOrdersBacking(string instrument, decimal price,
                                                    decimal amount)
        {
            //Tempo. Do use "GO" in the future
            decimal parStockLeverage = 0.1m; //10%
            return price * amount * parStockLeverage;
        }







        private bool IsNetworkAvail = false;
         private bool IsRoundtripDetermined = false;
         private void TaskRoundtripTime()
         {
            const int NUM_PACKETS = 5;

            double rt = 0;
            int i = 0;
            double sumRt =0;

          //  CUtil.GetRoundTrip("91.208.232.1", 500);

            for ( i=0; i < NUM_PACKETS; i++)
            {
                 rt = (double)  CUtil.GetRoundTrip("91.208.232.1", 500);
                 if (rt < 0)
                 {
                     IsNetworkAvail = false;
                     IsRoundtripDetermined = true;
                     return;
                 }

                 sumRt += rt;
            }

       
            double  meanRt = (sumRt / i);
            IsRoundtripDetermined = true;
            return;
         }

         private void ReadOneDataFromFile(string fileName, ref long lastRevId)
         {

             string path = CUtil.GetDataDir();
             string lastRevIdPath = path +"\\"+ fileName;


             try
             {                
                 if (System.IO.File.Exists(lastRevIdPath))
                     lastRevId = Convert.ToInt64(System.IO.File.ReadAllText(lastRevIdPath));
             }
             catch (Exception eExt)
             {

                 try
                 {
                     if (lastRevId == 0)
                     {

                         string backupPath = CUtil.GenBackupFileName(lastRevIdPath);
                         if (System.IO.File.Exists(backupPath))
                             lastRevId = Convert.ToInt64(System.IO.File.ReadAllText(backupPath));
                         string st = eExt.Message;
                     }
                 }
                 catch (Exception e)
                 {
                     Error("ReadOneDataFromFile", e);

                 }
             }

         }

      

       




         private void ReadDataFromFiles()
         {
             ReadOneDataFromFile("last_revid_deals.txt", ref m_lastDealRevId);
             ReadOneDataFromFile("last_revid_stock.txt", ref m_lastStockRevId);
             ReadOneDataFromFile("last_ASTSCurr_revid_deals.txt", ref m_lastASTSCurrDealRevId);
             
             ReadOneDataFromFile("last_deal_lifenum.txt", ref m_lastDealLifeNum);
             ReadOneDataFromFile("last_ASTSCurr_lifenum.txt", ref m_lastASTSCurrDealLifeNum);

         }

         private void WriteLastDataToFiles(string fileName, long val)
         {
             string dataDir = CUtil.GetDataDir();
             string fullPathToFile = dataDir + "\\" + fileName;


             CUtil.WriteTextFile(fileName, fullPathToFile, val.ToString());



         }

         private int m_parPeriodBeetwenDump = 500;
         private void  ThreadDumpLastDataToFiles ()
         {
           
             while (true)
            {
                try
                {
                    WriteLastDataToFiles("last_revid_deals.txt", m_lastDealRevId);
                    WriteLastDataToFiles("last_revid_stock.txt", m_lastStockRevId);
                    WriteLastDataToFiles("last_ASTSCurr_revid_deals.txt", m_lastASTSCurrDealRevId);
                    WriteLastDataToFiles("last_deal_lifenum.txt", m_lastDealLifeNum);
                    WriteLastDataToFiles("last_ASTSCurr_lifenum.txt", m_lastASTSCurrDealLifeNum);
               
                }
                catch (Exception e)
                {

                    //System.Diagnostics.Debug.Assert(false,"ThreadDumpLastRevisions.Error Dump "+e.Message);
                    Error("ThreadDumpLastRevisions",e);

                }

                System.Threading.Thread.Sleep(m_parPeriodBeetwenDump);

            }

         }

       




         public void TaskWriteBackups(string inpPath, string info)
         {
             bool bOK = false;
             int maxCount = 100;
             int   i = 0;
             while (!bOK)
             {
                 try
                 {
                     //    System.Threading.Thread.Sleep(400);
                     string path = CUtil.GenBackupFileName(inpPath);
                     System.IO.File.WriteAllText(path, info);
                 }
                 catch (System.IO.IOException )
                 {


                 }
                 catch (Exception e)
                 {
                     Error("TaskWriteBackups", e);


                 }
                 finally
                 {

                     bOK = true;
                 }

                 System.Threading.Thread.Sleep(2);

             }
             if (i++ > maxCount)
             {
                 Error("TaskWriteBackups. Max count exceed.");
                 return;

             }
         }

      

         


         protected override void StartGateIfNeed()
         {
			 Log("Start StartGateIfNeed");
             //kill process with another digit capacity
             if (GlobalConfig.Is64x)
             {
                 if (CUtil.GetProcess("P2MQRouter") != null)
                     CUtil.GetProcess("P2MQRouter").Kill();
             }
             else
             {
                 if (CUtil.GetProcess(@"P2MQRO~1") != null)
                     CUtil.GetProcess(@"P2MQRO~1").Kill();
                 else
                     if (CUtil.GetProcess("P2MQRouter64") != null)
                         CUtil.GetProcess("P2MQRouter64").Kill();
             }
             //start if not in process list
             if ((GlobalConfig.Is64x && CUtil.GetProcess(@"P2MQRO~1") == null && CUtil.GetProcess("P2MQRouter64") == null) ||
                    (!GlobalConfig.Is64x && CUtil.GetProcess("P2MQRouter") == null)
                   )
             {
                 string cgateHome = Environment.GetEnvironmentVariable(@"CGATE_HOME");
                 if (cgateHome == null)
                 {
                     System.Windows.Forms.MessageBox.Show("Cgate is not installed. Exiting.");
                     System.Diagnostics.Process.GetCurrentProcess().Kill();
                 }
                 else
                 {


                     Process p2RouterProcess = new Process();

                     string workDir = cgateHome+@"\bin";
                     p2RouterProcess.StartInfo.WorkingDirectory = workDir;
                     if (GlobalConfig.Is64x)
                         p2RouterProcess.StartInfo.FileName = workDir + @"\P2MQRouter64.exe";
                     else
                        p2RouterProcess.StartInfo.FileName = workDir + @"\P2MQRouter.exe";
                     p2RouterProcess.StartInfo.Arguments = string.Format("/ini:\"{0}\\CLIENT_router.ini\"",cgateHome);
                 
					 //if not set hands application
					 p2RouterProcess.StartInfo.UseShellExecute = false;

                     p2RouterProcess.Start();

                 }


             }

			 Log("End StartGateIfNeed");
         }



		 private void CreateNativeConnector()
		 {
			 _p2ConnectorNative = new CP2ConnectorNative(this);
			_p2ConnectorNative.Process();


		 }

      

        //TO DO normal

        /*
         private void CreateDictBots()
         {

             DictBots = new Dictionary<long, CBotBase>();
             foreach (CBotBase bot in ListBots)
                 DictBots[bot.BotId] = bot;
         }
        */


     

        

         private void OpenCgate()
         {
             try
             {
                 string appKey;

                 if (UseRealServer)
                     appKey = GlobalConfig.ApplicationKey;
                 else
                     appKey = "11111111";


                 string stOpen = String.Format(@"ini={0}\netrepl.ini;key={1}", CUtil.GetConfigDir(), appKey);
                 CGate.Open(stOpen);
                 //CGate.Open("ini=netrepl.ini;key=" + appKey);
                 CGate.LogInfo("test .Net log.");
             }
             catch (Exception e)
             {

                 Error("Unable open cgate.", e);
             }


         }


        /*
         public bool IsOrderFromPrevSession(CRawUserDeal rd)
         {
             try
             {
                 long OrderId = 0;

                 if (rd.Ext_id_buy != 0)
                     OrderId = rd.Id_ord_buy;
                 else if (rd.Ext_id_sell != 0)
                     OrderId = rd.Id_ord_sell;


                 string isin = Instruments.GetInstrumentByIsinId(rd.Isin_Id); //DictIsin_id[rd.Isin_Id];

                 UserOrderBox.mxListRawOrders.WaitOne();

                 foreach (CRawOrdersLogStruct rols in UserOrderBox.ListRawOrdersStruct)
                     if (rols.Id_ord == OrderId && rols.Moment < SessionBox.CurrentSession.SessionBegin)
                     {
                         UserOrderBox.mxListRawOrders.ReleaseMutex();
                         return true;

                     }


                 UserOrderBox.mxListRawOrders.ReleaseMutex();
             }
             catch (Exception e)
             {
                 Error("IsOrderFromPrevSession", e);
             }


             return false; ;
         }
        */



        /// <summary>
        /// Call from native
        /// </summary>
        /// <param name="isinId"></param>
        /// <param name="sourceStock"></param>
		 public void UpdateInpStocks(long isinId, ref CSharedStocks sourceStock)
		 {

			 string instrument = Instruments.GetInstrumentByIsinId(isinId);
			 
			 //TODO normal
			 while (SnapshoterStock == null)
				 Thread.Sleep(100);

             if (instrument != "")
                 //	 SnapshoterStock.UpdateInpStocks(instrument, ref sourceStock);

                 StockBoxP2.UpdateStockFromNative(instrument, ref sourceStock);

             EvStockOnline.Set();
             //var s = StockBoxInp.S

            // if (stockConv != null)
              //   Thread.Sleep(0);



		 }
 




         public override void FillDBClassField<T>(Dictionary<string, object> row, T outObj)
         {
             CMySQLConnector.FillClassFields(row, outObj);
         }




         


         protected override void CreateSessionBox()
         {
             Log("Create SessionBox");
             _sessionBox = new CSessionBox(this);
         }

         protected override void CreateUserDealsPosBox()
         {            
             _userDealsPosBox = new CUserDealsPosBoxP2(this);
         }


		 protected override void CreateExternalComponents()
		 {
			 _componentFactory.Create("atfs",this);
		 }

        

       
		 protected override void CreateUserOrderBox()
		 {
			
			 _userOrderBox = new CUserOrderBox(this);
		 }


		
		




         public override void Process()
         {
             try
             {

                 base.Process();


                 if (_useNativeConnector)
                     CUtil.ThreadStart(CreateNativeConnector);

				


                 string defaultIp = "127.0.0.1";
                 string defaultPort = "4001";
                 string defaultProtocol = "p2tcp";// "p2lrpcq";// "p2tcp";
                 string appName = GlobalConfig.AppName;


				 if (_simulateMode)
				 {
					 _simulator = new CP2Simulator(this);

				 }

				

               
                 //ASTS experimental
                  //m_S          MCXSPOT_MDCOMMON_REPL


                 //--- Common information about contracts: isin, etc

                 m_heartBeatListenerSettings.Add(new CSettingsListener("ListenerHeartBeat", "p2repl://FORTS_FUTTRADE_REPL;tables=heartbeat"));


                 //TO DO seporate session to it own connection
                 m_infoListenersSettings.Add(new CSettingsListener("ListenerFutInfo", "p2repl://FORTS_FUTINFO_REPL;tables=fut_instruments,investr,fut_sess_contents"));
                 m_infoListenersSettings.Add(new CSettingsListener("ListenerSession", "p2repl://FORTS_FUTINFO_REPL;tables=session"));

                 m_infoListenersSettings.Add(new CSettingsListener("ListenerInfo", "p2repl://FORTS_INFO_REPL"));
                 m_infoListenersSettings.Add(new CSettingsListener("ListenerPart", "p2repl://FORTS_PART_REPL"));

                 m_infoListenersSettings.Add(new CSettingsListener("ListenerUSD", "p2repl://FORTS_FUTINFO_REPL;tables=usd_online"));



                 //----all user's data
                 m_userDataListenersSettings.Add(new CSettingsListener("ListenerVM", "p2repl://FORTS_VM_REPL;tables=fut_vm"));
                 m_userDataListenersSettings.Add(new CSettingsListener("ListenerUserOrderLog", "p2repl://FORTS_FUTTRADE_REPL;tables=orders_log"));
                 m_userDataListenersSettings.Add(new CSettingsListener("ListenerUserDeals", "p2repl://FORTS_FUTTRADE_REPL;tables=user_deal"));
                 m_userDataListenersSettings.Add(new CSettingsListener("ListenerPosition", "p2repl://FORTS_POS_REPL;tables=position"));
                 //-------


                 //------market data, big volumes of data
                 m_marketListenersSettings.Add(new CSettingsListener("ListenerDeals", "p2repl://FORTS_DEALS_REPL;tables=deal"));
                 //   m_marketListenersSettings.Add(new CSettingsListener("ListenerOrderLog", "p2repl://FORTS_ORDLOG_REPL",SubscribedIsins  ));

                 //---- stock data-------
				 //2017-11-18 if using native connector we don't need this
				 if (!_useNativeConnector)
				 {
                    //2018-04-18 changed but not tested
                    //string stock = "p2repl://FORTS_FUTAGGR" + GlobalConfig.StockDepth + "_REPL";
                    string stock = "p2repl://FORTS_FUTAGGR50_REPL";
                    m_stocksListenersSettings.Add(new CSettingsListener("ListenerStock", stock));
					 //-------
				 }



                 //ASTS. Why we receieve all tables not only that I need
                 m_ASTSSpotListenerSettings.Add(new CSettingsListener(ConstP2.ListenerASTSSpotSECURITIES, "p2repl://MCXSPOT_INFO_REPL;tables=SECURITIES"));
                 m_ASTSSpotListenerSettings.Add(new CSettingsListener(ConstP2.ListenerASTSSpotOrerbook, "p2repl://MCXSPOT_AGGR_REPL;tables=ORDERBOOK", needDatalogging:false));
                m_ASTSSpotListenerSettings.Add(new CSettingsListener(ConstP2.ListenerASTSSpotALL_TRADES, "p2repl://MCXSPOT_MDTRADE_REPL;tables=ALL_TRADES",  needDatalogging:false));

                 m_ASTSCurrListenerSettings.Add(new CSettingsListener(ConstP2.ListenerASTSCurrSECURITIES, "p2repl://MCXCC_INFO_REPL;tables=SECURITIES"));
                 m_ASTSCurrListenerSettings.Add(new CSettingsListener(ConstP2.ListenerASTSCurrOrerbook,   "p2repl://MCXCC_AGGR_REPL;tables=ORDERBOOK", needDatalogging:true));
                 m_ASTSCurrListenerSettings.Add(new CSettingsListener(ConstP2.ListenerASTSCurrALL_TRADES, "p2repl://MCXCC_MDTRADE_REPL;tables=ALL_TRADES",  needDatalogging:true)); 
                                                                                                                     

                 //end ASTS


                 //------for control commands-----
                 foreach (CBotBase bb in ListBots)
                 {
                     m_dictOrderControlReplyListenerSetting[bb.BotId] = new List<CSettingsListener>();
                     m_dictOrderControlReplyListenerSetting[bb.BotId].Add(new CSettingsListener("ListenerOrderControlReply_" + bb.BotId, "p2mqreply://;ref=srvlink_" + bb.BotId));
                     m_dictOrderControlPublisherSettings[bb.BotId] = new List<CSettingsPublisher>();
                     m_dictOrderControlPublisherSettings[bb.BotId].Add(new CSettingsPublisher("PublisherOrderControl_" + bb.BotId,
                                                                                               "p2mq://FORTS_SRV;category=FORTS_MSG;name=srvlink_" + bb.BotId + ";timeout=5000;scheme=|FILE|forts_messages.ini|message", bb.BotId));

                     //   m_orderControlReplyListenerSetting.Add(new CSettingsListener("ListenerOrderControlReply", "p2mqreply://;ref=srvlink", SubscribedIsins ));            
                     // m_orderControlPublisherSettings.Add(new CSettingsPublisher("PublisherOrderControl", "p2mq://FORTS_SRV;category=FORTS_MSG;name=srvlink;timeout=5000;scheme=|FILE|forts_messages.ini|message",1));

                 }
                 //-------------------



                 Instruments.WaitInstrumentsLoaded();

				 Log("Create connections");


                 if (GlobalConfig.SubscribeCurrency)
                 {
                     m_connASTSCurr = new CConnection("ConnectionASTSCurr", defaultProtocol, defaultIp, defaultPort, appName, m_ASTSCurrListenerSettings, new List<CSettingsPublisher>(), this, 2);


                 }
                 if (GlobalConfig.SubscribeSpot)
                 {
                     m_connASTSSpot = new CConnection("ConnectionASTSSpot", defaultProtocol, defaultIp, defaultPort, appName, m_ASTSSpotListenerSettings, new List<CSettingsPublisher>(), this, 2);
                   
                 }



                 //end subscribe currency
                 m_connHeartbeat = new CConnection("ConnectionHeartBeat", defaultProtocol, defaultIp, defaultPort, appName, m_heartBeatListenerSettings, new List<CSettingsPublisher>(), this, 2);

                

                 int par_periodicSleep = 50;

                 while (!IsServerTimeAvailable || !IsLoadedMoneyData)                 
                     Thread.Sleep(par_periodicSleep);

                 


                  

                 m_connInfoData = new CConnection("ConnectionInfoData", defaultProtocol, defaultIp, defaultPort, appName, m_infoListenersSettings, new List<CSettingsPublisher>(), this, 2);
               //  Thread.Sleep(1000000000);



                



                 //TO DO normal event
                 while (!Instruments.IsMarketInstrumentsAvailable(StockExchId)
                     || !this.IsFutInfoOnline)             
                 {
                    Thread.Sleep(par_periodicSleep);
                 }

                 Log("DictIsinIsAvailable");

                 while (!IsAutomaticClearingProcessed)
                     Thread.Sleep(par_periodicSleep);

                 Log("AutomaticClearingProcessed");


        


                 //TODO stock dept - from config
                 //KAA increase stock depth
                 /*StockBox*/_stockBox = new CStockBox(this,100);

				
				 


				 CreateSnapshoters();
                                  
                 _dealsBox = new CDealBox(this);
              
                 _positionBox = new CPositionsBox(this);

                 while (!this.IsDealsPosLogLoadedFromDB)
                     System.Threading.Thread.Sleep(par_periodicSleep);
         
                
                 m_connUserData = new CConnection("ConnectionUserData", defaultProtocol, defaultIp, defaultPort, appName, m_userDataListenersSettings, new List<CSettingsPublisher>(), this, 1);
                 
                 //TO DO if session no available
                while (!this.IsOnlineUserDeals || !this.IsPositionOnline)
                    System.Threading.Thread.Sleep(par_periodicSleep);

               

                 while (GUIBox == null ||
                     (AnalzyeTimeFrames && GUIBox.GUICandleBox == null)
                     || (AnalzyeTimeFrames && !GUIBox.GUICandleBox.IsAllLastDataLoaded))
                     System.Threading.Thread.Sleep(10);




                 m_connMarketData = new CConnection("ConnectionMarketData", defaultProtocol, defaultIp, defaultPort, appName, m_marketListenersSettings, new List<CSettingsPublisher>(), this, 1);//sleep time was 0

                 while (!this.IsDealsOnline)
                     System.Threading.Thread.Sleep(par_periodicSleep);

				 if (!_useNativeConnector)
				 {
					 m_connStocksData = new CConnection("ConnectionStocksData", defaultProtocol, defaultIp, defaultPort, appName, m_stocksListenersSettings, new List<CSettingsPublisher>(), this, 0); //sleep time was 0
				 }
				
                
                 

             //TO DO normal event
             //TO DO other conditions
             while (!this.IsReadyForCreatePublishers)
             {

                 Thread.Sleep(par_periodicSleep);

             }

             Log("IsReadyForCreatePublishers");

             

          foreach (CBotBase bb in ListBots)
          {
              m_dictConnOrderControl[bb.BotId] = new CConnection("ConnectionOrderControl_" + bb.BotId, defaultProtocol, defaultIp, defaultPort, appName,
                                                         m_dictOrderControlReplyListenerSetting[bb.BotId], m_dictOrderControlPublisherSettings[bb.BotId], this, 1);

          }




          CreateTCPServerAndTradersDispatcher();

            
                
          // m_connOrderControl = new CConnection("ConnectionOrderControl", defaultProtocol, defaultIp, defaultPort, appName, m_orderControlReplyListenerSetting, m_orderControlPublisherSettings, this, 1);

         // System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.Batch;

      //    while (!this.IsSessionActive)

          //    while (!this.IsSessionOnline)
          {
              System.Threading.Thread.Sleep(par_periodicSleep);

          }

          Log("SessionIsActive");


          WaitTradeDisableByTimeLoaded();
          UpdateBotsDisableTradingByTime();

		  StartTradeManagerServer();



          bool was = false;
          while (true)
          {

              if (IsReadyForSendOrder)
              {
                  if (!was)
                  {

                      Log("IsReadyForSendOrder");


                      was = true;
                  }
              }
              Thread.Sleep(1000);
          }
          
             }

             catch (Exception e)
             {


                 Error("CPlaza2Connector.Process", e);


             }

             }


      

    }
}
