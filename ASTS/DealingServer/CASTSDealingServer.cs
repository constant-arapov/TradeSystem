using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


using Common;
using Common.Utils;
using Common.Interfaces;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Interaction;
using TradingLib.Abstract;
using TradingLib.Enums;
using TradingLib.Common;
using TradingLib.BotEvents;

using DBCommunicator;
using DBCommunicator.Interfaces;

using ComonentFactory;

using ASTS.Interfaces;
using ASTS.Interfaces.Clients;
using ASTS.Interfaces.Interactions;
using ASTS.Connector;
using ASTS.DealingServer.Session;
using ASTS.DealingServer.Stocks;



namespace ASTS.DealingServer
{
    public class CASTSDealingServer : CBaseDealingServer,
                                         IClientDBCommunicator,
                                         IClientSessionBox,
                                         IClientUserOrdeBoxASTS,                                         
                                         IClientPositionsBoxASTS,
                                         IDealingServerForASTSAllTrades,
                                         IDealingServerForASTSConnector,
                                         IDealingServerForTableSysTime
    {




        public override bool IsPossibleToCancelOrders { get; set; }


        public override bool IsAnalyzerTFOnline { get; set; }
        public override bool IsDealsOnline { get; set; }
        public override bool IsFutInfoOnline { get; set; }

        public override bool IsOnlineUserDeals { get; set; }
        public override bool IsOnlineUserOrderLog { get; set; }
        public override bool IsOnlineVM { get; set; }
        public override bool IsOrderControlAvailable { get; set; }
        
   
        public override bool IsPositionOnline { get; set; }


        public override bool IsSessionActive { get; set; }
    
        public override bool IsSessionOnline { get; set; }


        public override bool IsStockOnline { get; set; }

        public override bool IsPossibleNativeCancellOrdByInstr { get; set; }


        public Action<bool, string> PasswordChangeReply;
       

        private CComponentFactory _componentFactory;





        private CUserDealsPosBoxASTS _userDealsPosBox;
        public override IUserDealsPosBox UserDealsPosBox
        {
            get
            {
                return _userDealsPosBox;
            }
        }



        public CUserDealsPosBoxASTS UserDealsPosBoxInp
        {
            get
            {

                return _userDealsPosBox;
            }


        }




        private CUserOrderBoxASTS _userOrderBox;
        public override IUserOrderBox UserOrderBox 
        {
            get
            {
                return _userOrderBox;
            }        
        }




        private CPosistionsBoxASTS _positionsBoxASTS;

        public override IPositionBox PositionBox
        {
            get
            {
                return _positionsBoxASTS;

            }

        }    
        public CPosistionsBoxASTS PosistionsBoxInp 
        {
            get
            {
                return _positionsBoxASTS;
            }
        
        }

        public CUserOrderBoxASTS UserOrderBoxInp
        {

            get
            {

                return _userOrderBox;
            }


        }


        private string _newPassword;
        public string NewPassword
        {
            get
            {
                return _newPassword;
            }

        }

        public CDealBoxASTS DealBoxInp
        {
            get
            {
                return _dealBox;
            }

            
        }


        private CDealBoxASTS _dealBox;
        public override IDealBox DealBox
        {
            get
            {
                return _dealBox;
            }
        }

        private CSessionBoxASTS _sessionBox;
        public override ISessionBox SessionBox
        {
            get
            {
                return _sessionBox;
            }
         }

        public CSessionBoxASTS SessionBoxInp
        {
            get
            {
                return _sessionBox;
            }


        }


        public override bool IsServerTimeAvailable
        {
            get
            {
                if (GUIBox == null)
                    return false;

                else
                    return GUIBox.IsServerTimeAvailable;


            }

        }

        public bool IsConnectedToServer
        {
            get
            {
                if (_stockConnector == null)
                    return true;
                else
                    return _stockConnector.IsConnectedToServer;
            }
        }

        public override CBasePosBox PosBoxBase
        {
            get
            {
                return _positionsBoxASTS;
            }
        }


       /* public string StockPassword
        {


        }
        */
        /*protected CGlobalConfig _globalConfig;

        public override CGlobalConfig GlobalConfig
        {
            get
            {
                return _globalConfig;
            }
        }
        */



       


        public  override bool IsReadyStartTrdMgrServ
        {
           get
           {
             return true;
           }   
        }


        IStockConnector _stockConnector;

       

		private CStockBoxASTS _stockBox;
		public override IStockBox StockBox
		{
			get
			{
				return _stockBox;
			}

		}

		public CStockBoxASTS StockBoxInp
		{
			get
			{
				return _stockBox;
			}

		}

        private DateTime _serverTime;
        public override DateTime ServerTime
        {
            //TODO normal
            get
            {
                return DateTime.Now.AddHours(-2);
            }
            set
            {
                _serverTime = value;
            }

        }

        public string Account
        {
            get
            {

                return _stockConnector.Account;
            }
        }


        public string Password
        {
            get
            {

                return _stockConnector.Password;

            }



        }
        

        public CASTSDealingServer(/*IAlarmable alarmer*/): base(/*alarmer*/"ASTSDealingServer")
        {



			
            _componentFactory = new CComponentFactory(_logger);

            //StockExchId = CodesStockExch._02_MoexSPOT;
         //   StockExchId = GlobalConfig.StockExchId;



            Sounder = new CSounder();

            // force for ASTS. It's work. I Don't know why
            GUIBox.IsOnlineUserOrderLogDelayed = true;
            //for ASTS no need wait
            //IsOnlineUserDeals = true;
           
        }

        public override decimal GetOrdersBacking(string instrument, decimal price, 
                                                    decimal amount)
        {
            return price * amount;

        }




        public override bool IsPossibleToAddOrder(string instrument)
        {
            if (IsSessionActive || _sessionBox.IsTradingEnabledForInstrument(instrument)
                )
                return true;


            return false;

        }



        public override void Process()
        {

            try
            {

                base.Process();
                //tempo check
               
                Instruments.WaitInstrumentsLoaded();
                _sessionBox.OnInstrumentLoaded();

               

				CreateSnapshoters();

                _dealBox = new CDealBoxASTS(this);
				_stockBox = new CStockBoxASTS(this,100);

              

                _stockConnector = new CASTSConnectorSingle(this);

                 _positionsBoxASTS = new CPosistionsBoxASTS(this);

                 EvPosOnline.Set();


                CreateTCPServerAndTradersDispatcher();

                WaitTradeDisableByTimeLoaded();
                UpdateBotsDisableTradingByTime();
                
                WaitDataLoadedFromDB();
                SendSynchronizeDataToBots();
                StartTradeManagerServer();

            }
            catch (Exception e)
            {
                Error("Error", e);

            }




        }


        public override void ChangePassword(string currPassword,string newPassword)
        {
            _newPassword = newPassword;
            _stockConnector.ChangePassword(currPassword,newPassword);           
            
        }

        public string  LoadStockExchPassword(string login)
        {
                      
            return  DBCommunicator.LoadStockExchPassword(StockExchId, login);            
                       
        }


        public void SaveNewPassword(string login)
        {
            DBCommunicator.SaveNewPassword(StockExchId, login, _newPassword);

            PasswordChangeReply(true, "");

        }





        private void  SendSynchronizeDataToBots()
        {

            //Not possoble determine when user deals online on ASTS. 
            //So send after all user deals loaded from DB
            //TriggerRecalcAllBots(EnmBotEventCode.OnUserDealOnline, null);
            //TriggerRecalcAllBots(EnmBotEventCode.OnUserOrdersOnline, null);
           // TriggerRecalcAllBots(EnmBotEventCode.OnPositionOnline, null);

           



        }




        public void WaitConnectorDisconnected()
        {

            if (!IsConnectedToServer)
                return;

            _stockConnector.WaitConnectionClosed();
            

        }


        /// <summary>
        ///Disconnect connector
        ///
        /// Call from GUI
        /// </summary>
        public void Disconnect()
        {
            _stockConnector.DisconnectFromServer();

        }



        public override bool IsReadyRefreshBotPos()
        {

            if (IsStockOnline && IsOnlineUserDeals)                               
                return true;

            return false;

        }


        public override void FillDBClassField<T>(Dictionary<string, object> row, T outObj)
        {
            CMySQLConnector.FillClassFields(row, outObj);
        }
        public override void AddOrder(int botId, string isin, decimal price, EnmOrderDir dir, decimal amount)
        {
            _stockConnector.AddOrder(botId, isin, price, dir, amount);
        }

        public override void CancelOrder(long orderId, int botId)
        {
            _stockConnector.CancelOrder(orderId, botId);
        }

        public override void CancelAllOrders(int buy_sell, int ext_id, string isin, int botId)
        {
            _stockConnector.CancelAllOrders(buy_sell, ext_id, isin, botId);           
        }

        protected override void CreateExternalComponents()
        {
            _componentFactory.Create("atfs",this);
        }

        protected override void CreateSessionBox()
        {
            _sessionBox = new CSessionBoxASTS(this);
        }

        protected override void CreateUserDealsPosBox()
        {
            _userDealsPosBox = new CUserDealsPosBoxASTS(this);
        }

        protected override void CreateUserOrderBox()
        {
			_userOrderBox = new CUserOrderBoxASTS(this);
        }

       public override bool IsReadyForRecalcBots()
        {
            if (IsOnlineUserOrderLog && IsOnlineUserDeals && IsStockOnline)
                return true;

            return false;
        }





        protected override void StartGateIfNeed()
        {
            
			string ASTSHome = Environment.GetEnvironmentVariable(@"ASTS_BRIDGE");
			if (ASTSHome == null)
			{
				Error("ASTS BRIDGE not found");


			}
			else
			{	//Process not started
				if (CUtil.GetProcess("ASTSBridge") == null)
				{
					Process p2RouterProcess = new Process();


					p2RouterProcess.StartInfo.WorkingDirectory = ASTSHome;

					p2RouterProcess.StartInfo.FileName = ASTSHome + @"\ASTSBridge.exe";


					//if not set hands application
					p2RouterProcess.StartInfo.UseShellExecute = false;

					p2RouterProcess.Start();
				}

			}



        }


        public override bool IsPriceInLimits(string instrument, decimal price)
        {
            //TODO real check
            return true;
        }

        public override decimal GetStepPrice(string intrument)
        {
            //for ASTS always 1, multiple to 1 is always 
            //the same value for calculation
            return 1;  

        }

        public override decimal GetMinStep(string  instrument)
        {
            return Instruments.GetMinStep(instrument); ;
        }

        public override long GetLotSize(string instrument)
        {

            return Instruments.GetLotSize(instrument);
        }



    }
}
