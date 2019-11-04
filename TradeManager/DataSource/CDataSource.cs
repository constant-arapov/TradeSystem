using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.ComponentModel;

using System.Windows.Input;

using MySql.Data.MySqlClient;


using Common;
using Common.Interfaces;
using Common.Utils;

using TCPLib;


using DBCommunicator;
using DBCommunicator.Interfaces;

using TradingLib.Enums;
using TradingLib.Common;
using TradingLib.ProtoTradingStructs;
using TradingLib.ProtoTradingStructs.TradeManager;
using TradingLib.Interfaces.Keys;

using TradeManager.Interfaces;
using TradeManager.Interfaces.Clients;
using TradeManager.TCPCommu;
using TradeManager.Models;
using TradeManager.ViewModels;
using TradeManager.DataSyncher;
using TradeManager.Commands.Data;



using System.Reflection;


namespace TradeManager.DataSource
{
	public class CDataSource : CBaseDataSource, IClientDatabaseConnector, IClientCommuTradeManager, 
                                                ICommandToDataSource,    IAlarmable, IClientDbRouter
	{

        //=========VIEW MODELS=====================
		public bool IsDatabaseConnected { get; set; }
		public bool IsDatabaseReadyForOperations { get; set; }


        public VMAccount VMAccount {get;set;}
		private  CDBRouter _dbRouter;

		public ObservableCollection<VMAvailableMoney> CollVMAvailMoney = new ObservableCollection<VMAvailableMoney>();
		public ObservableCollection<VMTradersLimits> CollVMTradersLimits = new ObservableCollection<VMTradersLimits>();
        public ObservableCollection<VMServer> CollVMServers { get; set; }// = new ObservableCollection<VMServer>();
		public ObservableCollection<VMDBCon> CollVMDBCon { get; set; }
        public ObservableCollection<VMPosInstrTotal> CollVMPosInstrTotal = new ObservableCollection<VMPosInstrTotal>();
        public ObservableCollection<VMInstrument> CollVMInstruments = new ObservableCollection<VMInstrument>();
        public ObservableCollection<VMStockExch> CollVMStockExchId { get; set; }
        public ObservableCollection<VMTrdAddFundsReq> CollVMTrdAddFundsReq =
                                                                                new ObservableCollection<VMTrdAddFundsReq>();

        public ObservableCollection<VMTrdWithdrawReq> CollVMTrdWithdrawReq =
                                                                              new ObservableCollection<VMTrdWithdrawReq>();

        //public ObservableCollection


        public ICollectionView CollViewVMBotStatus
        {
            get
            {
                return _dataSynchBotStatus.CollViewVM;
            }

        }

        public ICollectionView CollViewVMBotPosTrdMgr
        {
            get
            {
                return _dataSynchBotPosTrdMgr.CollViewVM;
            }
        }

        public ICollectionView CollViewPosInstrTotal
        {

            get
            {
                return _dataSynchPosInstrTotal.CollViewVM;                    
            }

        }

        public ICollectionView CollectionViewClientInfo
        {
            get
            {
                return _dataSyncherClientInfo.CollViewVM;
            }


        }








        public VMTotals VMTotalsInstance;



        //=======END VIEW MODELS====================

        //=========MODELS=====================
		private System.Windows.Threading.Dispatcher _dispatcher;
		private List<ModelAvailableMoney> _lstAvailMoney;
		private List<ModelTradersLimits> _lstTradersLimits;
        private List<ModelInstrument> _lstModelInstruments;

        private List<ModelTrdAddFundsReq> _lstModelTrdAddFundsReq;
        private List<ModelTrdWithdrawReq> _lstModelTrdWithdrawReq;

        private List<ModelServer> _lstModelServer = new List<ModelServer>();
	
		private List<ModelDBCon> _lstModelDBCon;
        private List<CPositionInstrTotal> _lstModelPosInstrTotal = new List<CPositionInstrTotal>();
        private List<CBotStatus> _lstBotStatus = new List<CBotStatus>();
        private List<CBotPosTrdMgr> _lstBotPosTrdMgr = new List<CBotPosTrdMgr>();

		


		private ModelTotals _modelTotals = new ModelTotals();




        //=======END MODELS========================
		

        private string _dbHost;
        private long _dbPort;


#pragma warning disable CS0169 // Поле "CDataSource._MySqlConnector" никогда не используется.
        //2018-08-06 removed as not used
	//	private CMySQLConnector _MySqlConnector;
#pragma warning restore CS0169 // Поле "CDataSource._MySqlConnector" никогда не используется.
		private CCommuTradeManager _commuTradeManager;

        private List<int> _lstStockExh;

      


        //TODO load from config
        //TODO using ViewModel, observable collection etc
      /*  private Dictionary<int, bool> _stockExchStates = new Dictionary<int, bool>()
        {
            {1, false},
            {4, true}

        };
        */

        private List<ModelStockExchState> _stockExchStates;


        


        private IClientDataSource _client;

		



		public CDataSource(IClientDataSource client, 
                            System.Windows.Threading.Dispatcher guiDisp,
                           string dbHost, long  dbPort, 
                            List<int> lstStockExchId, 
                            List<ModelStockExchState> stockExchStates,
							List<ModelDBCon> lstConfDBCon ) :
			base(client, guiDisp)
		{

            _client = client;
            _dbHost = dbHost;
            _dbPort = dbPort;
            _dispatcher = guiDisp;
            _lstStockExh = lstStockExchId;
			_lstModelDBCon = lstConfDBCon;

			//_dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
			VMTotalsInstance = new VMTotals(_modelTotals);

            CollVMServers = new ObservableCollection<VMServer>();
			CollVMDBCon = new ObservableCollection<VMDBCon>();

            VMAccount = new VMAccount();
            _stockExchStates = stockExchStates;

            CollVMStockExchId = new ObservableCollection<VMStockExch>();
            _lstStockExh.ForEach (elStockExchId => CollVMStockExchId.Add (
                                            new VMStockExch 
                                            {
                                                StockExchId = elStockExchId, 
                                                StockExchName = _client.GetStockExchName(elStockExchId)
                                            }
                                            ));


                                                                          
		}


        public override bool IsStockExchSelected(int stockExhId)
        {
            var res = _stockExchStates.Find(el => el.CodeStockExchId == stockExhId);
            if (res == null)
                throw new ApplicationException("CDataStource.IsStockExchSelected stockExchId not found");

            
            return res.IsSelected;
        }

      
		
        public void UpdateFilterStockExhId(int stockExchId, bool isChecked)
	    {

            var res = _stockExchStates.Find(el => el.CodeStockExchId == stockExchId);  
                if (res == null)
                    throw new ApplicationException("CDataStource.UpdateFilterStockExhId stockExchId not found");

                res.IsSelected = isChecked;

            _dataSynchBotStatus.UpdateFilterStockExchId();
            _dataSynchPosInstrTotal.UpdateFilterStockExchId();
            _client.SaveConfig();
            
        }


		public void ConnectToDatabase()
		{

			_dispatcher.Invoke(new Action(
							   () =>
								 ModelVMSynchro(_lstModelDBCon, CollVMDBCon, VMDBCon.Create )));
										  		
			_dbRouter = new CDBRouter(this, _lstModelDBCon);
			_dbRouter.Connect();
		

			CUtil.TaskStart(TaskUpdateDBData);
		}


        /// <summary>
        /// From TCP
        /// </summary>
        /// <param name="aresp"></param>
        /// <param name="conId"></param>
		public void UpdateDealingServersAuthStat(CAuthResponse aresp, int conId)
		{

            lock (_lstModelServer)
            {
                foreach (var kvp in _lstModelServer)                
                    if (kvp.ConId == conId)                    
                        kvp.IsAvailable = aresp.IsSuccess;
                                         
            }

            _dispatcher.Invoke(new Action(() =>
            {              
                ModelVMSynchro(_lstModelServer, CollVMServers, VMServer.Create);
           
            }
                    ));
		}


        public void UpdateAccount(object sender, ExecutedRoutedEventArgs e)
        {

            CAuthRequest ar = (CAuthRequest)e.Parameter;

            VMAccount.User = ar.User;
            VMAccount.Password = ar.Password;
           
        }



        public void UpdatePositionInstrTotal(CListPositionInstrTotal listPosInstrTotal)
        {

           _dataSynchPosInstrTotal.Update(listPosInstrTotal.StockExchId, listPosInstrTotal.Lst);
                     
        }


        public void UpdateBotPosTrdMgr(CListBotPosTrdMgr listBotPosTrdInstrument)
        {

            UpdateTraderNames(listBotPosTrdInstrument.Lst);
            _dataSynchBotPosTrdMgr.Update(listBotPosTrdInstrument.StockExchId, listBotPosTrdInstrument.Lst);
							      
        }

        public void UpdateClientInfo(CListClientInfo listClietInfo)
        {
            //_dataSynchBotStatus.Update(listBotStatus.StockExchId, listBotStatus.Lst);
            UpdateTraderNames(listClietInfo.Lst);

            _dataSyncherClientInfo.Update(listClietInfo.StockExchId, listClietInfo.Lst);

        }


        public void UpdateListBotStatus(CListBotStatus listBotStatus)
        {
            UpdateTraderNames(listBotStatus.Lst);



                listBotStatus.Lst.ForEach
                    (el =>
                    {
                        try
                        {
                            if (_lstTradersLimits != null)
                            {
                                var recLim = _lstTradersLimits.Find(fnd => fnd.number == el.BotId);
                                if (recLim != null)
                                    if (recLim.MaxLossVMClosedTotal != el.Limit)
                                    {
                                        el.Limit = recLim.MaxLossVMClosedTotal;
                                    }
                            }
                        }
                        catch (Exception e)
                        {
                            Error("UpdateListBotStatus", e);
                        }
                    }

                    );

          
       


			_dataSynchBotStatus.Update(listBotStatus.StockExchId, listBotStatus.Lst);

              
           
		
        }


        protected void UpdateTraderNames<T>(List<T> listDest) where T : CBaseTrdMgr_StockExch_BotId, IKey_TraderName
        {
            
                if (_lstTradersLimits == null)
                    return;






                listDest.ForEach
                    (el =>
                    {
                        try
                        {
                            var recLim = _lstTradersLimits.Find(fnd => fnd.number == el.BotId && fnd.StockExchId == el.StockExchId);
                            if (recLim != null)
                                if (recLim.name != el.TraderName)
                                {
                                    el.TraderName = recLim.name;
                                }
                        }
                        catch (Exception e)
                        {
                            Error("UpdateTraderNames", e);
                        }


                    }

                    );

           
          

        }

        private void ValidateConfigsConsistent()
        {
           if (_commuTradeManager.ServersConf.Servers.Count != _lstStockExh.Count)
            {
                string msg = "Ошибка конфигурации. Несоответствие в servers.xml и TradeManagerConfig.xml";
                msg += " (Servers.Count != _lstStockExh.Count)";
                _client.FatalError(msg);


            }


        }



		public void ConnectToDealingServers()
		{
			//CUtil.TaskStart(TaskUpdateVMFromDealingServer);
			_commuTradeManager = new CCommuTradeManager(this);


            InitLstModelDealingServer();
            ValidateConfigsConsistent();


            _commuTradeManager.ConnectToDealingServers();
	
		

		}



        private void InitLstModelDealingServer()
        {

            for (int i = 0; i < _commuTradeManager.ServersConf.Servers.Count; i++)
            {
                var servConf = _commuTradeManager.ServersConf.Servers[i];

                _lstModelServer.Add(new ModelServer
                {
                    ConId = i,
                    Ip = servConf.IP,
                    Port = servConf.Port,
                    Name = servConf.Name
                }

                                    );

            }
        }

		

		public void TaskUpdateDBData()
		{
			//while (!IsDatabaseConnected || IsDatabaseReadyForOperations)
				//Thread.Sleep(100);

			while (App.IsRunning)
			{
				try
				{
					_lstAvailMoney =   _dbRouter.GetAvailableMoney();
					_lstTradersLimits = _dbRouter.GetTradersLimits();
                    _lstModelInstruments = _dbRouter.GetInstruments();

                    _lstModelTrdAddFundsReq = _dbRouter.GetTrdAddFundsReq();
                    _lstModelTrdWithdrawReq  =  _dbRouter.GetTrdWithdawReq();


                    CalculateTotals();

					if (App.IsRunning)
					_dispatcher.Invoke(new Action(() =>
						{
                            try
                            {
                                ModelVMSynchro(_lstAvailMoney, CollVMAvailMoney, VMAvailableMoney.Create);
                                ModelVMSynchro(_lstTradersLimits, CollVMTradersLimits, VMTradersLimits.Create);
                                ModelVMSynchro(_lstModelInstruments, CollVMInstruments, VMInstrument.Create);
                                ModelVMSynchro(_lstModelTrdAddFundsReq, CollVMTrdAddFundsReq, VMTrdAddFundsReq.Create);
                                ModelVMSynchro(_lstModelTrdWithdrawReq, CollVMTrdWithdrawReq, VMTrdWithdrawReq.Create);

                                UpdateTotals();
                            }
                            catch (Exception exc) 
                            {
                                Error("CDataSource.TaskUpdateDBData GUI",exc);

                            }
						}
					));



					Thread.Sleep(200);
				}
				catch (Exception e)
				{
					Error("CDataSource.TaskUpdateDBData",e);
				}


			}

		}



	




		private void UpdateTotals()
		{
			//TODO normal, with reflection
		//	if (VMTotalsInstance.TotalAvailableMoney != _modelTotals.TotalAvailableMoney)
				VMTotalsInstance.TotalAvailableMoney = _modelTotals.TotalAvailableMoney;
                //VMTotalsInstance.ManualUpdate();
		}

		private void GenServerModels()
		{
			//_commuTradeManager.ServersConf.
		}



		private void CalculateTotals()
		{
            decimal sum = 0;
			foreach (ModelAvailableMoney m in _lstAvailMoney)
                sum += m.money_avail;

            _modelTotals.TotalAvailableMoney = sum;
		}





		private void ModelVMSynchro<TModel, TVM>(List<TModel> lstModels,
													ObservableCollection<TVM> collectionVM,
													 Func<TModel, TVM> createVMElement)
		{
			//number of lines changed that means
			//delete or insert occured
			if (lstModels.Count != collectionVM.Count)
			{

				collectionVM.Clear();
				lstModels.ForEach(modelElement =>
				{
					collectionVM.Add(createVMElement.Invoke(modelElement));
					
				}
										);
			}
			else
			{
				//note: at this point assume amount of elements in lstModels is 
				//equal of collectionVM

				for (int i = 0; i < lstModels.Count; i++)
				{
					List<PropertyInfo> sourceProperties = lstModels[i].GetType().GetProperties().ToList();
					List<PropertyInfo> destProperties = collectionVM[i].GetType().GetProperties().ToList();

					if (sourceProperties.Count != destProperties.Count)
						throw new ApplicationException("ModelVMSyncro. sourceProperties.Count != destProperties.Count");

					sourceProperties.ForEach
					(srcProp =>
						{
							bool bfound = false;
							destProperties.ForEach
							(dstProp =>
							{
								if (srcProp.Name == dstProp.Name)
								{
									bfound = true;
									object dstEl= dstProp.GetValue(collectionVM[i],null);
									object srcEl = srcProp.GetValue(lstModels[i], null);

									if (!CUtilReflex.IsEqualValues(srcEl, dstEl))
										dstProp.SetValue(collectionVM[i], srcEl, null);
								}

							}
							);

							if (!bfound)
								Error("ModelVMSyncro. " + srcProp.Name + "not found");



						}
					);
				}
			}
		}


		public void  DisableTrader(object sender, ExecutedRoutedEventArgs e)
		{
			VMBotStatus botStatus = (VMBotStatus) e.Parameter;

			int conid = GetConnId(botStatus.StockExchId);
            
			_commuTradeManager.SendDataToServer(conid, new CDisableBot { BotId = botStatus.BotId },
													enmTradingEvent.DisableBot);
			
		}


        public void SendReconnect(object sender, ExecutedRoutedEventArgs e)
        {

            CSendReconnect sendRec = (CSendReconnect)e.Parameter;
            int conId = GetConnId(sendRec.StockExchId);


            _commuTradeManager.SendDataToServer(conId, sendRec,
                                                    enmTradingEvent.SendReconnect); ;

        }



        public void EnableTrader(object sender, ExecutedRoutedEventArgs e)
		{
			VMBotStatus botStatus = (VMBotStatus)e.Parameter;

			int conid = GetConnId(botStatus.StockExchId);

			_commuTradeManager.SendDataToServer(conid, new CEnableBot { BotId = botStatus.BotId },
													enmTradingEvent.EnableBot);

		}

        public void AddInstrument(object sender, ExecutedRoutedEventArgs e)
        {
            _dbRouter.CMDAddInstrument((CCmdDataAddInstrument)e.Parameter);  
        }



        public void DeleteInstrument(object sender, ExecutedRoutedEventArgs e)
        {

            _dbRouter.CMDDeleteInstrument((CCmdDataDeleteInstrument) e.Parameter);
          
        }

        public void ChangeProcProfit(object sender, ExecutedRoutedEventArgs e)
        {
            _dbRouter.CMDChangeProcProfit((CCmdDataProcProfit)e.Parameter);

        }

        public void ChangeProfitDealingFee(object sender, ExecutedRoutedEventArgs e)
        {
            _dbRouter.CMDChangeProcFeeDealing((CCmdDataProcDealingFee)e.Parameter);
            
        }

        public void CompleteTrdAddFundsReq (object sender, ExecutedRoutedEventArgs e)
        {
            _dbRouter.CMDTrdAddFundsReq((VMTrdAddFundsReq)e.Parameter);
        }


        public void CompleteTrdWithdrawReq(object sender, ExecutedRoutedEventArgs e)
        {

            _dbRouter.CMDTrdWithdrawReq ( (VMTrdWithdrawReq) e.Parameter);
        }

        private int GetConnId(int stockExchId)
		{

            int iFound = -1;
            for(int i=0; i<_lstStockExh.Count; i++)
            {
                if (_lstStockExh[i] == stockExchId)
                    iFound = i;
            }


            //if (stockExchId == CodesStockExch._01_MoexFORTS)
            //return 0;

            return iFound;
        }


		public void SetMaxLossVM(object sender, ExecutedRoutedEventArgs e)
		{

            _dbRouter.CMDSetMaxLossVM((CCmdDataMaxLossVM)e.Parameter);
		

		}



		public void AddWithDrawMoney(object sender, ExecutedRoutedEventArgs e)
		{
			
			_dbRouter.CMDAddWithDrawMoney((CCmdDataAddWithdrawMoney)e.Parameter);
			
		}


        public void CloseTraderPos(object sender, ExecutedRoutedEventArgs e)
        {

            CCloseBotPosTrdMgr data = (CCloseBotPosTrdMgr)  e.Parameter;

            int conId = GetConnId(data.StockExchId);

            _commuTradeManager.SendDataToServer(conId, data, enmTradingEvent.CloseBotPosTrdMgr);

            //Thread.Sleep(0);
        }

        public void OnConnectionDisconnect(int conId)
        {

          //  _lstModelServer = new List<ModelServer>();

         //   _lstModelServer.Add(new ModelServer { ConId = 0, Ip = "", IsAvailable = false, Name = "1234567", Port = 11 });


           
            if (_lstModelServer.Count >= conId - 1)
            {
                //  _lstModelServer[conId].IsAvailable = false;
                // _lstModelServer[conId].Name = "123";


                ModelServer oldValue = _lstModelServer[conId];

                _lstModelServer[conId] = new ModelServer
                {
                    IsAvailable = false,
                    ConId = oldValue.ConId,
                    Ip = oldValue.Ip,
                    Name = oldValue.Name,
                    Port = oldValue.Port
                };


             //   CollVMServers[conId].IsAvailable = false;
            }
            else
                Error("OnConnectionDisconnect connection not found");
              

            _dispatcher.Invoke(new Action(() =>
            {
                ModelVMSynchro(_lstModelServer, CollVMServers, VMServer.Create);

            }
            ));


        }




    }
}
