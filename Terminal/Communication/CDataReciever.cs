using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;


using Common;
using Common.Interfaces;
using Common.Logger;
using Common.Utils;


using Messenger;

using TradingLib;
using TradingLib.Enums;
using TradingLib.ProtoTradingStructs;

using TCPLib.Interfaces;

using Terminal.Interfaces;
using Terminal.Common;
using Terminal.ViewModels;

using Terminal.Communication.Helpers;

namespace Terminal.Communication
{

   
    /// <summary>
    /// Responce: Recieve data from calback (trigered by TCP client)
    /// deserialise data structs.
    /// 
    /// Depend of trade events type coulld:
    /// 1) Call specific method of diffrent part of system.
    /// 2) Put data specific output structs (stocks and deals)
    /// These strucs are used  by market view model.
    /// Data structures are instrument dependent, so it created by response
    /// from other parts of system by specific events.
    /// 
    ///   
    /// Interacts with: CKernelTernminal, TerminalVieModel (clock)
    /// 
    /// 
    /// </summary>
    public class CDataReciever : ITCPClientUser
    {

        private Dictionary<string, CStockClass> _outpStockClass = new Dictionary<string, CStockClass>();
        CLogger _logger;
       // private List<CDealClass> _outpListDealClass = new List<CDealClass>();

        private DateTime _dtLastTimeSynchro = new DateTime(0);

        private CKernelTerminal _kernelTerminal;

        private int _connId { get; set; }

       


        public List<CTIckerData> ListAvailTickers {get;set;}// = new List<CTIckerData>();


        public void Error(string msg, Exception e = null)
        {
            _kernelTerminal.Error(msg, e);

        }
        public Dictionary<string, CStockClass> OutpStockClass
        {
            get
            {
                return _outpStockClass;
            }
        }
        public Dictionary<string, List<CDealClass>> OutpListAggrDeals { get; set; }

        public Dictionary<string, List<CDealClass>> OutpListRawDeals { get; set; }


        private Dictionary<string, CDealsAgregator> _dictDealsAggr = new Dictionary<string, CDealsAgregator>();

        private TerminalViewModel _terminalViewModel;

		private IChildWinDataUpdater _childWinDataUpdater;
		private AuthResponseEventHandler AuthResponse;
        private CMessenger _messenger;

        private DBGDataRcvr _dbg;




        //==========================================================================================================
        //============================== PUBLIC METHODS ============================================================
        //==========================================================================================================



        /// <summary>
        /// Call from CCommunicator
        /// </summary>
        /// <param name="kernelTerminal"></param>
        /// <param name="childWinDataUpdater"></param>
        /// <param name="authResponse"></param>
        /// <param name="connId"></param>
        public CDataReciever(CKernelTerminal kernelTerminal, IChildWinDataUpdater childWinDataUpdater, 
							  AuthResponseEventHandler  authResponse, int connId)
        {
            _kernelTerminal = kernelTerminal;
			_childWinDataUpdater = childWinDataUpdater;
			AuthResponse = authResponse;

            _connId = connId;
            OutpListAggrDeals = new Dictionary<string, List<CDealClass>>();// List<CDealClass>();
            OutpListRawDeals = new Dictionary<string, List<CDealClass>>();
         
            
            //2018-04-23
            _logger = new CLogger("DataReciever");

           _terminalViewModel = _kernelTerminal.ViewModelDispatcher.TerminalViewModel;
           _messenger = new CMessenger();

            _dbg = new DBGDataRcvr(isOn: _kernelTerminal.GlobalConfig.DebugStocks);

        }


        /// <summary>
        /// Callback triggered by tcp client
        /// </summary>
        /// <param name="message">message recieved by TCP client </param>
        public void CallbackReadMessage(byte[] message)
        {
            try
            {


               
                byte byteTradingEvent = 0;
                byte[] arrMsgBody = _messenger.GetBinaryMessageHeaderAndBody(message, ref byteTradingEvent);

                enmTradingEvent tradingEvent = (enmTradingEvent)byteTradingEvent;

                //object msgBody = CUtil.DeSerializeBinary(arrMsgBody);






                if (enmTradingEvent.AuthResponse == tradingEvent)
                    ProcessAuthResponse(arrMsgBody);

                else if (enmTradingEvent.UserOrdersUpdate == tradingEvent)
                    ProcessUserOrdersUpdate(arrMsgBody);

                else if (enmTradingEvent.UserUpdatePositionMonitor == tradingEvent)
                    ProcessUserUpdatePositionMonitor(arrMsgBody);

                else if (enmTradingEvent.UserUpdatePosLog == tradingEvent)
                    ProcessUserUpdatePosLog(arrMsgBody);

                else if (enmTradingEvent.UpdateUserPosLogLate == tradingEvent)
                    ProcessUserUpdPosLogLate(arrMsgBody);

                else if (enmTradingEvent.UserUpdateDealsLog == tradingEvent)
                    ProcessUserUpdateDealsLog(arrMsgBody);

                else if (enmTradingEvent.UserUpdateVM == tradingEvent)
                    ProcessUserUpdateVM(arrMsgBody);

                else if (enmTradingEvent.StockUpadate == tradingEvent)
                    ProcessStockUpadate(arrMsgBody);


                else if (enmTradingEvent.DealsUpdate == tradingEvent)
                    ProcessDealsUpdate(arrMsgBody);

                else if (enmTradingEvent.UserUpdateAvailableTickers == tradingEvent)
                    ProcessUserUpdateAvailableTickers(arrMsgBody);

                else if (enmTradingEvent.SynchroniseTime == tradingEvent)
                    ProcessSynchroniseTime(arrMsgBody);

                else if (enmTradingEvent.UpdateMoneyData == tradingEvent)
                    ProcessUpdateMoneyData(arrMsgBody);


                else if (enmTradingEvent.StopLossTakeProfitAccepted == tradingEvent)
                    ProcessAcceptStoplossTakeProfit(arrMsgBody);

                else if (enmTradingEvent.UpdateInstrumentParams == tradingEvent)
                    ProcessUpdInstrParamsOnilne(arrMsgBody);

                else
                {
                    Error("CDataReciever unknownMessage type");
                }
            }

            catch (Exception e)
            {
                Error("CDataReciever.CallbackReadMessage", e);

            }


        }

        public void CallbackConnectionDisconnect()
        {


        }






        /// <summary>
        /// 
        /// Create structures - list for deals and stocks 
        /// for  instrument
        /// 
        /// Call from - 
        /// 1)CKernetlTerminal.SubscribeTickersFromConfig - on successfull Authorisation
        /// 2)CKernetlTerminal.EditConnectedStock
        /// 3)CKernetlTerminal.EditNonConnectedStock
        /// 
        /// </summary>
        /// <param name="instrument"></param>
        public void AddStructuresForSubscribedTicker(string instrument, decimal stepPrice)
        {
            //if (isin == null)
            //  return;


            lock (OutpListAggrDeals)
                if (!OutpListAggrDeals.ContainsKey(instrument))
                    OutpListAggrDeals[instrument] = new List<CDealClass>();

            lock (OutpStockClass)
                if (!OutpStockClass.ContainsKey(instrument))
                    OutpStockClass[instrument] = new CStockClass();

            //TODO get from GUI
  
            int parPeriod = 30;
       

            if (!_dictDealsAggr.ContainsKey(instrument))
                _dictDealsAggr[instrument] = new CDealsAgregator(parPeriod, stepPrice);



            lock (OutpListRawDeals)
                if (!OutpListRawDeals.ContainsKey(instrument))
                    OutpListRawDeals[instrument] = new List<CDealClass>();


        }


      

        //==========================================================================================================
        //============================== END PUBLIC METHODS ============================================================
        //==========================================================================================================


        private void UpdateAvailTickers(CAvailableTickers avTick)
        {
			//2018-02-25 Not used, need remove &
            if (ListAvailTickers == null ||
                avTick.ListAvailableTickers.Except(ListAvailTickers).Any())
                ListAvailTickers = avTick.ListAvailableTickers;


        }



        private void Log(string msg)
        {
            _logger.Log(msg);

        }



        /*
        private void UpdateTickDict<TData>(CAvailableTickers avTicks, Dictionary <string, TData> dict)
        {
            lock (dict)
            {

                foreach (var v in avTicks.ListAvailableTickers)
                    if (dict.ContainsKey(v.TickerName))
                        dict[v.TickerName] = (TData)Activator.CreateInstance(typeof(TData));

            }
        }
        */
        



    




    



        private bool _bNotFirstTime = false;
        private DateTime _dtFirstTime = new DateTime(0);
        private bool _bWorkingMode = false;
        private int _secSinceStart =5;

        private void CheckDeltaTimeLim(DateTime compTime,  int maxDMsec, string callerName)
        {
            if (!_bNotFirstTime)
            {
                _bNotFirstTime = true;
                _dtFirstTime = DateTime.Now;
                return;
            }
            if (!_bWorkingMode)
                if ((DateTime.Now - _dtFirstTime).TotalSeconds > _secSinceStart)
                    _bWorkingMode = true;
                else
                    return;


               

            //2018-03-28 temporary disabled
            /*
            double deltaMS = (DateTime.Now - compTime).TotalMilliseconds;
            if ( deltaMS > maxDMsec)
                Error("DeltaTime limit was violated deltaMs=" + deltaMS + " maxDMsec=" + maxDMsec + " " +callerName);
             */

        }

        private void ProcessAuthResponse(byte[] arrMsgBody)
        {
            CAuthResponse aresp = CUtilProto.DeserializeProto<CAuthResponse>(arrMsgBody);

            Log("[AuthResponse]");			
			AuthResponse(aresp, _connId);
			_childWinDataUpdater.Update(aresp, _connId);

			

        }

        private void ProcessUserOrdersUpdate(byte[] arrMsgBody)
        {
         CUserOrdersUpdate userOrdersUpdate = CUtilProto.DeserializeProto<CUserOrdersUpdate>(arrMsgBody);
                    Log("[UserOrdersUpdate]");
                    _kernelTerminal.UpdateUserOrders(userOrdersUpdate);


        }


        private void ProcessUserUpdatePositionMonitor(byte[] arrMsgBody)
        {
            CUserPosMonitorUpdate userPosUpdate = CUtilProto.DeserializeProto<CUserPosMonitorUpdate>(arrMsgBody);
            //changed 2018-05-27
            string msg = "[UserUpdatePositionMonitor]";

            if (userPosUpdate.MonitorUserPos == null)
            {
                msg += " MonitorUserPos==null";
            }
            else
            {
                foreach (var poses in userPosUpdate.MonitorUserPos)
                {
                    msg += String.Format("instr={0} amount={1} avPos={2}",
                                            poses.Key, poses.Value.Amount, poses.Value.AvPos);

                }
            }
            Log(msg);
            _kernelTerminal.UpdateUserMonitorPos(userPosUpdate);

        }

		private void ProcessAcceptStoplossTakeProfit(byte[] arrMsgBody)
		{
			CSetOrder stopLossTakeProfit = CUtilProto.DeserializeProto<CSetOrder>(arrMsgBody);

			Log("[ProcessAcceptStoplossTakeProfit]");
			_kernelTerminal.AcceptStopLossTakeProfit(stopLossTakeProfit);

		}


		private void ProcessUpdInstrParamsOnilne(byte[] arrMsgBody)
		{

			CUpdateInstrumentParams updInstrParams = CUtilProto.DeserializeProto<CUpdateInstrumentParams>(arrMsgBody);

			Log("ProcessUpdInstrParamsOnilne]");
			_kernelTerminal.UpdateInstrParamsOnilne(updInstrParams);

		}



        private void ProcessUserUpdatePosLog(byte[] arrMsgBody)
        {
           CUserPosLogUpdate userPosLogUpdate = CUtilProto.DeserializeProto<CUserPosLogUpdate>(arrMsgBody);
           Log("[UserUpdatePosLog]");


			_childWinDataUpdater.Update(userPosLogUpdate, _connId);

        }

        private void ProcessUserUpdPosLogLate(byte[] arrMsgBody)
        {
            CUserPosLogUpdLate userPosUpdLate = CUtilProto.DeserializeProto<CUserPosLogUpdLate>(arrMsgBody);
            Log("[CUserPosLogUpdLate]");

            _childWinDataUpdater.Update(userPosUpdLate, _connId);

        }




        private void ProcessUserUpdateDealsLog(byte[] arrMsgBody)
        {
            CUserDealsLogUpdate userDealsLog = CUtilProto.DeserializeProto<CUserDealsLogUpdate>(arrMsgBody);


           foreach (var kvp in userDealsLog.DictLog)
           {
             
                 //public Dictionary<string, List<CUserDeal>> DictLog{ get; set; }

           }

            Log("[UserUpdatePosLog]");

			_childWinDataUpdater.Update(userDealsLog, _connId);

        }


        private void ProcessUserUpdateVM(byte[] arrMsgBody)
        {
            CUserVMUpdate updateVM = CUtilProto.DeserializeProto<CUserVMUpdate>(arrMsgBody);

            Log("[UserUpdateVM]");

        
			_childWinDataUpdater.Update(updateVM, _connId);
			_childWinDataUpdater.Update(updateVM.ListVM, _connId);



        }

        private double _dtStockUpdate = 0;
        private void ProcessStockUpadate(byte[] arrMsgBody)
        {

            DateTime dtBefUnpack = DateTime.Now;
            CStockClass sc = CUtilProto.DeserializeProto<CStockClass>(arrMsgBody);
            DateTime dtAftUnpack = DateTime.Now;

          //   int sz = sizeof(decimal);
           //  int sz2 = sizeof(long);

            CheckDeltaTimeLim(sc.DtBeforePack, 50, "CDataREciever.ProcessStockUpadate " + sc.Isin);

            _dtStockUpdate = (DateTime.Now - sc.DtBeforePack).TotalMilliseconds;


            /*Log("[StockUpadate]  DtBeforePack=" + sc.DtBeforePack.ToString("hh:mm:ss.fff") + " isin=" +
                  sc.Isin + " bid=" + sc.StockListBids[0][0].Price + " ask=" + sc.StockListAsks[0][0].Price  +
                  " dtBefUnpack=" + dtBefUnpack.ToString("hh:mm:ss.fff") + " dtAftUnpack=" + dtAftUnpack.ToString("hh:mm:ss.fff") +
                   " dt=" + _dtStockUpdate);*/

            Log("[StockUpadate]  DtBeforePack=" + sc.DtBeforePack.ToString("hh:mm:ss.fff") + " isin=" + sc.Isin);



          //  DBGPrintStock(sc);
            //  if (sc.Isin == "IOTUSD")
            //    Thread.Sleep(0);

            if (_outpStockClass.ContainsKey(sc.Isin))
                // sc.Copy(sc.Isin, _outpStockClass[sc.Isin]);
                sc.RebuildStock(sc.Isin, _outpStockClass[sc.Isin]);

            //    _kernelTerminal.ViewModelDispatcher.GetMarketViewModel(sc.Isin).ControlMarket.ControlStockInstance.ForceRepaint();

            _dbg.PrintData(sc, _outpStockClass[sc.Isin]);


        }

        private void DBGPrintStock(CStockClass sc)
        {

            string stBids = "bids ";
            foreach (var kvp in sc.StockListBids)
            {
                int prec = kvp.Key;
                stBids += String.Format("prec={0}", prec);
                foreach (var kvp2 in kvp.Value)
                {
                    stBids += String.Format("({0},{1})", 
                                            kvp2.Price.ToString().Replace(",","."),
                                             kvp2.Volume.ToString().Replace(",", "."));



                }


            }
           string stAsks = "asks ";
            foreach (var kvp in sc.StockListAsks)
            {
                int prec = kvp.Key;
                stAsks += String.Format("prec={0}", prec);
                foreach (var kvp2 in kvp.Value)
                {
                    stAsks += String.Format("({0},{1})",
                                            kvp2.Price.ToString().Replace(",", "."),
                                             kvp2.Volume.ToString().Replace(",", "."));



                }


            }





        }




        private void ProcessDealsUpdate(byte[] arrMsgBody)
        {
            try
            {
                CDealsList lstDeals = CUtilProto.DeserializeProto<CDealsList>(arrMsgBody);
                string isin = lstDeals.Isin;  //lstDeals.DealsList[0].Isin;

                CheckDeltaTimeLim(lstDeals.DtBeforePack, 50, "deals update");

                _dtStockUpdate = (DateTime.Now - lstDeals.DtBeforePack).TotalMilliseconds;

                Log("[Dealsupdate]  DtBeforePack=" + lstDeals.DtBeforePack.ToString("hh:mm:ss.fff") + " isin=" + lstDeals.Isin +
                     " Dt_firtst_deal=" + lstDeals.DealsList[0].DtTm.ToString("hh:mm:ss.fff") +
                     " Dt_last_deal=" + lstDeals.DealsList[lstDeals.DealsList.Count - 1].DtTm.ToString("hh:mm:ss.fff") + " Count=" + lstDeals.DealsList.Count +
                     " dt=" + _dtStockUpdate);





                List<CDealClass> lstAggr = new List<CDealClass>();
                //get summarized deals


                _dictDealsAggr[isin].GenAggrPrice(lstDeals.DealsList, lstAggr);


                lock (OutpListAggrDeals[isin])
                {
                    foreach (CDealClass dealClass in lstAggr)//lstDeals.DealsList)                            
                        OutpListAggrDeals[isin].Add(dealClass);
                }

                lock (OutpListRawDeals[isin])
                {

                    foreach (CDealClass dealClass in lstDeals.DealsList)
                        OutpListRawDeals[isin].Add(dealClass);

                }


            }





            catch (Exception e)
            {
                Error("CDataReciever deals", e);
            }

        }


        private void ProcessUserUpdateAvailableTickers(byte[] arrMsgBody)
        {

            CAvailableTickers avTickers = CUtilProto.DeserializeProto<CAvailableTickers>(arrMsgBody);
            UpdateAvailTickers(avTickers);          
			_childWinDataUpdater.Update(avTickers, _connId);

	
			//Added 2018-02-25
			_kernelTerminal.UpdateInstrParamsOnConnection(avTickers);
        }

        private void ProcessSynchroniseTime(byte[] arrMsgBody)
        {

            Log("[TimeSynchro]");

            bool needSynchro = false;

            lock (_kernelTerminal.TerminalConfig)
                needSynchro = _kernelTerminal.TerminalConfig.NeedTimeSynchro;


            //TODO send moscow time flag or not (or Timezone) on datamessage

            
            CTimeSynchroClass ts = CUtilProto.DeserializeProto<CTimeSynchroClass>(arrMsgBody);

            //removed 2018-03-28 to remove message "Time syncro dtMS=" from error log
            /*
            if (needSynchro)
            {

                int dtMS = (int)(DateTime.Now - ts.DtCurrentTime).TotalMilliseconds;

                int param = 20;
                if (Math.Abs(dtMS) > param)
                {
                    CTimeChanger tch = new CTimeChanger(-dtMS);
                    Error("Time syncro dtMS=" + dtMS);
                }
            }
            */

            DateTime dtMsc = CUtilTime.LocalToMsc(ts.DtCurrentTime);

            try
            {
                _terminalViewModel.StockClock = dtMsc.ToString("HH:mm:ss");
                
            }
            catch (Exception e)
            {
                Thread.Sleep(0);
            }


        }


        private void ProcessUpdateMoneyData(byte[] arrMsgBody)
        {
            CUserMoney um = CUtilProto.DeserializeProto<CUserMoney>(arrMsgBody);          
			_childWinDataUpdater.Update(um, _connId);

        }


        
       
           



    }
}
