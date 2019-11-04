using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using TCPLib;
using TradingLib;
using TradingLib.Enums;
using Messenger;

using System.Windows.Input;


using Common;
using Common.Interfaces;
using Common.Utils;

using TCPLib.Interfaces;


using TradingLib.Interfaces.Components;
using TradingLib.Abstract;
using TradingLib.ProtoTradingStructs;
using TradingLib.Data;

using Messenger;


using Terminal.Conf;
using Terminal.TradingStructs;

namespace Terminal.Communication
{
  /// <summary>
  /// Response: communication with trading server.
  /// Mainly for serve communication
  /// MarketViewModel => StockExchangeServer
  /// Packs messages and writes to TcpClient
  /// Manages all TcpClients and  DataRecievers
  /// 
  /// There are twoconcurrent methods of send messages:
  /// 1) Call public methods of this class, like Sendxxxx
  /// which packs structure.  
  /// 2) Pack structure by client, than call SendDataToServer
  /// 
  /// By user request try to connect with server.
  /// If connection ok try authorise.
  /// 
  /// Interacts: CTCPClient, CDataReciever, CKernelTerminal,
  ///            MarketViewModel
  /// 
  /// </summary>
    public class CCommunicator : CBaseCommunicator
    {
        CKernelTerminal _kernelTerminal;
        
        /// <summary>
        /// Dictionary ConId-DataRecier 
        /// </summary>
        private Dictionary<int, CDataReciever> _dictDataReceiever = new Dictionary<int,CDataReciever>();


       


       

       

       // private CAlarmer _alarmer;
     


    

        //==========================================================================================================
        //============================== PUBLIC METHODS ============================================================
        //==========================================================================================================


        public CCommunicator(CKernelTerminal kernelTerminal)
            : base (kernelTerminal.Alarmer)
        {
            _kernelTerminal = kernelTerminal;
           // _alarmer = kernelTerminal.Alarmer;

         
        }


      

        protected override void OnUnsuccessfullConnectTrial(UserConReq conReq, int connId)
        {
            _kernelTerminal.ViewModelDispatcher.
                    Update(new CAuthResponse()
                    {
                        IsSuccess = false,
                        ErrorMessage = "Сервер недоступен" + conReq.ServerName
                    }, connId);


        }


    

   






		

        protected override void OnAuthoriseSuccess(CAuthResponse aresp, int conId)
        {
            _kernelTerminal.ViewModelDispatcher.Update(ServersConf.Servers, conId);
            _kernelTerminal.OnConnectionSuccess(conId, aresp);
            _kernelTerminal.ViewModelDispatcher.OnAuthoriseSuccessServer(conId);
        }



        //???????????
        public void Disconnect(int connId)
        {
          if (_dictTCPClients.ContainsKey(connId))
            if (_dictTCPClients[connId].IsConnected)
            {


                _dictTCPClients[connId].Disconnect();
                _dictTCPClients[connId] = null;
                _dictTCPClients.Remove(connId);

                _dictDataReceiever[connId] = null;
                _dictDataReceiever.Remove(connId);

                _serverConfig.Servers[connId].IsConnected = false;
            }
            //_lstTCPClients[connId].
        }


    


      

        protected override void RemoveDataReciever(int connId)
        {
            _dictDataReceiever.Remove(connId);
        }





        public void TryConnectToServerTillSuccess(int connId)
        {
            while (!_dictTCPClients[connId].ConnectToServer())
                Thread.Sleep(500);

            

        }


     





        public CDataReciever GetDataReciever(int conId)
        {
            if (_dictDataReceiever ==null || !_dictDataReceiever.ContainsKey(conId))
                return null;

            
           
            return _dictDataReceiever[conId];
        }

      


        public void SendSubscribeTickerList(int conId, CSubscribeTicker st)
        {

            enmTradingEvent ev = enmTradingEvent.UserSubscribeTicker;
            //CSubscribeTicker st = new CSubscribeTicker { SubscribeCommand = new CCommandSubscribeTickers { Ticker = ticker, Action = EnmSubsrcibeActions.Subscribe  } };


            SendDataToServer(conId,st, ev);

        }

        public void SendOrderType(int conId, string instrument, EnmOrderTypes stopLossTakeProfit, decimal price, decimal amount=0)
        {
            enmTradingEvent ev = enmTradingEvent.SetStoplossTakeProfit;
            CSetOrder ssp = new CSetOrder 
            {
              Instrument =instrument,
              Price = price, 
              OrderType =stopLossTakeProfit,
			  Amount = amount
             };
            SendDataToServer(conId, ssp, ev);

        }

		public void SendOrderThrow(int conId, string instrument, decimal amount, EnmOrderDir dir, int throwSteps)
		{

			enmTradingEvent ev = enmTradingEvent.SendOrderThrow;

			CSendOrderThrow sot = new CSendOrderThrow
			{
				Instrument = instrument,
				OrderDir = dir,
				Amount = amount,
				ThrowSteps = throwSteps
			};

			SendDataToServer(conId, sot, ev);

		}


        public void SendOrderRest(int conId, CDataRestOrder dataRestOrder)
        {

            enmTradingEvent ev = enmTradingEvent.SendOrderRest;


            SendDataToServer(conId, dataRestOrder, ev);



        }


		public void InvertPosition(int conId, string instrument)
		{
			enmTradingEvent ev = enmTradingEvent.InvertPosition;

			CInvertUserPos invPos = new CInvertUserPos()
			{
				Instrument = instrument

			};

			SendDataToServer(conId, invPos, ev);

		}



    



      




 




        public void SendUnsubscribeOneTicker(int conID, string ticker)
        {
            SendSubscribeOrUnsubscribeOneTicker(conID, ticker, EnmSubsrcibeActions.UnSubscribe);

        }


        public void SendSubscribeOneTicker(int conID, string ticker)
        {
            SendSubscribeOrUnsubscribeOneTicker(conID, ticker, EnmSubsrcibeActions.Subscribe);
        }



        

      




        //==========================================================================================================
        //============================== END PUBLIC METHODS ============================================================
        //==========================================================================================================



        private void SendSubscribeOrUnsubscribeOneTicker(int conId, string ticker, EnmSubsrcibeActions act)
        {
            enmTradingEvent ev = enmTradingEvent.UserSubscribeTicker;
            CSubscribeTicker st = new CSubscribeTicker 
                 {  ListSubscribeCommands = new List<CCommandSubscribeTickers>
                    {
                     (new CCommandSubscribeTickers 
                            { Ticker = ticker, 
                              Action = act  
                            })
                    }
                 };

            SendDataToServer(conId, st, ev);

        }

      

        protected override ITCPClientUser GetTCPClientUser(int connId)
        {
            return _dictDataReceiever[connId];
        }


        protected override void AddDataReciever(int connId)
        {
              if (_dictDataReceiever.ContainsKey(connId))
                throw new Exception("DataReceiever is already exist");

              CDataReciever dataReciever = new CDataReciever(_kernelTerminal, _kernelTerminal.ViewModelDispatcher, OnAuthResponse, connId);
              _dictDataReceiever[connId] = dataReciever;
        }




    }
}
