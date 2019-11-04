using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Threading;


using Common.Interfaces;
using Common.Utils;

using TradingLib.Common;


using DBCommunicator;
using DBCommunicator.Interfaces;
using DBCommunicator.Builders;

using TradeManager.Interfaces.Keys;
using TradeManager.Interfaces.Clients;
using TradeManager.Models;
using TradeManager.Commands.Data;

using TradeManager.ViewModels;


namespace TradeManager.DataSource
{
    public class CDBRouter : IAlarmable
    {

     

        private List<ModelDBCon> _lstConfDBCon;
        private List<CDBSource> _lstDBSource = new List<CDBSource>();


     

        private IClientDbRouter  _client;

       
		
    



        public CDBRouter(IClientDbRouter client, List<ModelDBCon> lstConfDBCon)
        {
            _lstConfDBCon = lstConfDBCon;
            _client = client;
           

        }


		private CDBSource GetDBSrource(int serverId)
		{
			return _lstDBSource.Find(el => el.Conf.ServerId == serverId);

		}




		public void CMDAddWithDrawMoney(CCmdDataAddWithdrawMoney cmdData)
		{
			

			object out_result = new object();
			object out_error_message = new object();


			CMySQLProcedureBuilder builder = new CMySQLProcedureBuilder("transact_add_withdraw_money", GetDBSrource(cmdData.ServerId).MySQLConnector);


			var res = builder.Add("inp_operation_id", cmdData.Operation_code)
					 .Add("inp_account_id", cmdData.BotId)
					 .Add("inp_money_change", cmdData.MoneyChanged)
					 .Add("out_result", out_result, ParameterDirection.Output)
					 .Add("out_error_message", out_error_message, ParameterDirection.Output)
					 .Build();

		}



        




        public void CMDSetMaxLossVM(CCmdDataMaxLossVM datMaxLossVM)
        {

            GetDBSrource(datMaxLossVM.ServerId).MySQLConnector.ExecuteProcedure("update_trader_limits",
                                                new Dictionary<string, object>() {
												{"inpStockExchId", datMaxLossVM.TradersLims.StockExchId},
												{"inpBotId",datMaxLossVM.TradersLims.number},
												{"inpNewLim",datMaxLossVM.NewLim}
												}
                                                );
           
        }


        public void CMDAddInstrument(CCmdDataAddInstrument addInstrument)
        {

            object out_result = new object();
            object out_error_message = new object();


            CMySQLProcedureBuilder builder = new CMySQLProcedureBuilder("transact_add_instrument",
                                                                            GetDBSrource(addInstrument.ServerId).MySQLConnector);


            var res = builder.Add("in_intrument", addInstrument.Instrument)
                     .Add("in_stock_exch_id", addInstrument.StockExchId)
                     .Add("out_result", out_result, ParameterDirection.Output)
                     .Add("out_error_message", out_error_message, ParameterDirection.Output)
                     .Build();


        }


        public void CMDDeleteInstrument(CCmdDataDeleteInstrument deleteInstrument)
        {

            GetDBSrource(deleteInstrument.ServerId).MySQLConnector.ExecuteProcedure("transact_delete_instrument",
                                               "in_instrument", deleteInstrument.Instrument,
                                               "in_stock_exch_id", deleteInstrument.StockExchId
                                               );



        }

        public void CMDChangeProcProfit(CCmdDataProcProfit cmdDataProcProfit)
        {

            GetDBSrource(cmdDataProcProfit.ServerId).UpdateProcProfit(cmdDataProcProfit.TradersLims.number,
                                                                                    cmdDataProcProfit.TradersLims.StockExchId,
                                                                                    cmdDataProcProfit.NewLim);


        }

        public void CMDChangeProcFeeDealing (CCmdDataProcDealingFee cmdDataProcProfit)
        {
            GetDBSrource(cmdDataProcProfit.ServerId).UpdateProcFeeDealing(cmdDataProcProfit.TradersLims.number,
                                                                           cmdDataProcProfit.TradersLims.StockExchId,
                                                                           cmdDataProcProfit.NewLim);

            

        }



        public void CMDTrdAddFundsReq(VMTrdAddFundsReq vmTrdAddFundsReq)
        {
            GetDBSrource(vmTrdAddFundsReq.ServerId).UpdTrdAddFundsReq(vmTrdAddFundsReq.id);
        }


        public void CMDTrdWithdrawReq(VMTrdWithdrawReq vmTrdWithDrawReq)
        {
            GetDBSrource(vmTrdWithDrawReq.ServerId).UpdTrdWithDrawReq(vmTrdWithDrawReq.id);
        }




        public void InitConnections()
        {
            foreach (var conf in _lstConfDBCon)
            {
                //TODO get password from external
                CDBSource dbSource = new CDBSource(conf, this);

                dbSource.Connect();

                _lstDBSource.Add(dbSource);

            }

        }

        public void TaskKeepConOpen()
        {
            while (true)
            {
                foreach (var dbSource in _lstDBSource)
                {
                    dbSource.UpdateConnectionState();

                    if (!dbSource.IsDatabaseConnected)
                    {
                        dbSource.Connect();

                    }

                    

                }

                Thread.Sleep(3000);
            }

        }


        public List<ModelTradersLimits> GetTradersLimits()
        {
			return GetDataStockExhIdDep<ModelTradersLimits>("get_traders_limits", "inStockExchId");
         
        }

        public List<ModelInstrument> GetInstruments()
        {
            List<ModelInstrument> lstModelInstr = GetDataStockExhIdDep<ModelInstrument>("get_instruments", "in_stock_exch_id");
            lstModelInstr.ForEach(el =>  el.StockExchId  = el.stock_exch_id); 


            return lstModelInstr;


                   
        }




        public List<ModelAvailableMoney> GetAvailableMoney()
        {

            List<ModelAvailableMoney> _lstMdlAvlMoney = new List<ModelAvailableMoney>();

            foreach (var source in _lstDBSource)
				if (source.IsDatabaseConnected && source.Conf.IsSelected)
				{
					List<ModelAvailableMoney> lstAvMonCurr = source.MySQLConnector.ExecuteSelectObjectProcedureName<ModelAvailableMoney>("get_all_accounts_money_current");
					
					lstAvMonCurr.ForEach(el =>
										 { el.ShortNameDB = source.Conf.ShortNameDB;
										 el.ServerId = source.Conf.ServerId;
										 });
					_lstMdlAvlMoney.AddRange(lstAvMonCurr);

				}

            return _lstMdlAvlMoney;
        }


        public List<ModelTrdAddFundsReq> GetTrdAddFundsReq()
        {

            List<ModelTrdAddFundsReq> _lstTrdAddFundsReq= new List<ModelTrdAddFundsReq>();
            foreach (var source in _lstDBSource)
                if (source.IsDatabaseConnected && source.Conf.IsSelected)
                {


                    _lstTrdAddFundsReq = GetDataStockExhIdDep<ModelTrdAddFundsReq>("get_traders_add_funds_req", "StockExchId");
                   // _lstAddTrdAsetsReq = source.MySQLConnector.ExecuteSelectObjectProcedureName<ModelTradersAddAssetsReq>("get_traders_add_assets_req");

                }

            return _lstTrdAddFundsReq;
        }





		public List<T> GetDataStockExhIdDep<T>(string procedureName, string stockExchIdName) where T : IKey_Server, new() 
		{
			List<T> _lstData = new List<T>();

			foreach (var source in _lstDBSource)
				if (source.IsDatabaseConnected)
					foreach (var stockExchIdValue in source.Conf.LstAvailStockExh)
						if (_client.IsStockExchSelected(stockExchIdValue) && source.Conf.IsSelected )
						{
                          
							List<T> lstAdd = source.GetObject<T>(procedureName, GetParmsStockExch(stockExchIdName, stockExchIdValue));
							lstAdd.ForEach(el => 
                                            {
                                                el.ServerId = source.Conf.ServerId;
                                                el.ShortNameDB = source.Conf.ShortNameDB;
                                            }
                                                );
                            _lstData.AddRange(lstAdd);

						}
																	
			return _lstData;
		}

        public List<ModelTrdWithdrawReq> GetTrdWithdawReq()
        {
            List<ModelTrdWithdrawReq> _lstData = new List<ModelTrdWithdrawReq>();
            foreach (var source in _lstDBSource)
				if (source.IsDatabaseConnected)
					foreach (var stockExchId in source.Conf.LstAvailStockExh)
						if (_client.IsStockExchSelected(stockExchId) && source.Conf.IsSelected )
						{

                            List<ModelTrdWithdrawReq> lstAdd = source.GetWthdrawRequests();

                            lstAdd.ForEach(el =>
                            {
                                el.ServerId = source.Conf.ServerId;
                                el.ShortNameDB = source.Conf.ShortNameDB;
                            }
                                                );


                            _lstData.AddRange(lstAdd);
                            
						}




            return _lstData;

        }
		










		public static Dictionary<string, object>  GetParmsStockExch(string strockExchIdNameInProcedure,int stockExchId)
		{
			return new Dictionary<string, object>() { { strockExchIdNameInProcedure, stockExchId } };
		}




        public void Connect()
        {
            InitConnections();
            CUtil.TaskStart(TaskKeepConOpen);
        }


        public void Process()
        {


        }


        public void Error(string msg, Exception e=null)
        {
            _client.Error(msg, e);          
        }


    }

   




}
