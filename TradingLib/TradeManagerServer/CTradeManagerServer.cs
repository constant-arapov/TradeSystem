using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Common;
using Common.Interfaces;
using Common.Utils;

using Common.Collections;

using TCPLib.Interfaces;


using TradingLib.Enums;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;

using TradingLib.TradersDispatcher;


using TradingLib.ProtoTradingStructs;
using TradingLib.ProtoTradingStructs.TradeManager;

using TradingLib.Bots;


using Messenger;

using TCPLib;


namespace TradingLib.TradeManagerServer
{
	public class CTradeManagerServer : CBaseFunctional, ITCPServerUser
	{

		IClientTradeManagerServer _dealingServer;
        CTCPServer _tcpServer;

        Dictionary<int, CTradeManagerLink> _dictConIdTradeMgrLink = new Dictionary<int, CTradeManagerLink>();

        IMessenger _messenger;

        IDBCommunicator _dbCommunicator;

        private CBlockingQueue<CTradingData> _bqTraderData = new CBlockingQueue<CTradingData>();

		//================= LIST FOR DATA STORAGE ========================
        private CListPositionInstrTotal _posInstrTotal = new CListPositionInstrTotal();
        private CListBotStatus _lstBotStatus;
        private CListBotPosTrdMgr _lsBotPosTrdMgr;
        private CListClientInfo _lstClientInfo;
		//private CListTraderInfoSummary _lstTrdInfoSummary;
		// END LISTS FOR DATA STORAGE



        const int cnstBroadCast = -1;


		public CTradeManagerServer(IClientTradeManagerServer dealingServer)
			: base(dealingServer)
		{
			_dealingServer = dealingServer;
            _dbCommunicator = dealingServer.DBCommunicator;
            _messenger = dealingServer.Messenger;

            _lsBotPosTrdMgr = new CListBotPosTrdMgr(_dealingServer.StockExchId);
            _lstBotStatus = new CListBotStatus(_dealingServer.StockExchId);
            _lstClientInfo = new CListClientInfo(_dealingServer.StockExchId);

            CUtil.ThreadStart(ThreadProcessSendQueue);

            CUtil.ThreadStart(ThreadEnqueueStateData);
            //_dbCommunicator = _client.DBCommunicator;
		}

        public void Process()
        {
            //int port = 6000;
            _tcpServer = new CTCPServer(_dealingServer.PortTradeManager
                                        , this, "_TradeManagerServer");


        }






		public void CallbackNewConnection(int conId)
		{
            _dictConIdTradeMgrLink[conId] = new CTradeManagerLink(); 
            //TODO enque send data on new connection 


		}



		public void CallbackDisconnect(int conId)
		{

            if (_dictConIdTradeMgrLink.ContainsKey(conId))
                _dictConIdTradeMgrLink.Remove(conId);
		}





		public void CallbackReadMessage(int conId, byte[] message)
		{
            byte byteTradingEvent = 0;

            byte[] arrMsgBody = _messenger.GetBinaryMessageHeaderAndBody(message, ref  byteTradingEvent);
            enmTradingEvent tradingEvent = (enmTradingEvent)byteTradingEvent;


            Log("Read message from client");

            if (enmTradingEvent.AuthRequest == tradingEvent)
                ProcessAuthRequest(conId, arrMsgBody);
            else if (enmTradingEvent.EnableBot == tradingEvent)
                ProcessEnableBot(conId, arrMsgBody);
            else if (enmTradingEvent.DisableBot == tradingEvent)
                ProcessDisableBot(conId, arrMsgBody);
            else if (enmTradingEvent.CloseBotPosTrdMgr == tradingEvent)
                ProcessClosePos(conId, arrMsgBody);
            else if (enmTradingEvent.SendReconnect == tradingEvent)
                ProcessSendReconnect(conId, arrMsgBody);
            

		}


        private void ProcessClosePos(int conId, byte[] arrMsgBody)
        {

            CCloseBotPosTrdMgr closePos = CUtilProto.DeserializeProto<CCloseBotPosTrdMgr>(arrMsgBody);
            CBotBase bot = _dealingServer.GetBotById(closePos.BotId);
            bot.ClosePositionOfInstrument(closePos.Instrument);
        }

        
        private void ProcessSendReconnect(int conId, byte[] arrMsgBody)        
        {
            CSendReconnect sendRecon = CUtilProto.DeserializeProto<CSendReconnect>(arrMsgBody);
            Log("Process sending reconnect");
            _dealingServer.OnTrdMgrSentReconnect(sendRecon.ConnectionId);
           

        }





		private void ProcessEnableBot(int conId, byte[] arrMsgBody)
		{
			CEnableBot disableBot = CUtilProto.DeserializeProto<CEnableBot>(arrMsgBody);
			Log("Enable bot Id=" + disableBot.BotId);
			_dealingServer.EnableBot(disableBot.BotId);

		}


		private void ProcessDisableBot(int conId, byte[] arrMsgBody)
		{
			CDisableBot disableBot = CUtilProto.DeserializeProto<CDisableBot>(arrMsgBody);
			Log("Disable bot Id="+disableBot.BotId);
			_dealingServer.DisableBot(disableBot.BotId);

		}



        private void ProcessAuthRequest(int conId, byte[] arrMsgBody)
        {
            //TODO auth here
            int botId = -1;
            CAuthRequest areq = CUtilProto.DeserializeProto<CAuthRequest>(arrMsgBody);
            if (!IsPassedAuth(areq))
            {
				//response: auth is not success
                EnqueueSendAuthResponse(false, "Пользователь и пароль не найдены", conId);
            }

            else
            {
       

                _dictConIdTradeMgrLink[conId].ConnId = conId;
				//response: successfull auth.
                EnqueueSendAuthResponse(true, "", conId);

               _dictConIdTradeMgrLink[conId].IsLoggedOn = true;
                
             
                 
            }
            Log("AuthRequest botId=" + botId);
           

        }
   
        private bool IsPassedAuth(CAuthRequest req)
        {


            //return _dbCommunicator.LoginRequest(req.User, req.Password);
            return _dbCommunicator.LoginRequestTradeManager(req.User, req.Password);


        }



        private void ThreadProcessSendQueue()
        {
            while (true)
            {
                try
                {
                    CTradingData tc = _bqTraderData.GetElementBlocking();


                    if (enmTradingEvent.AuthResponse == tc.Event)
                        SendUpdateAuthResponse(tc.ConnId, (CAuthResponse)tc.Data);
                    else if (enmTradingEvent.PositionInstrTotal == tc.Event ||
                             enmTradingEvent.BotStatus == tc.Event ||
                             enmTradingEvent.BotPosTrdMgr == tc.Event ||
                             enmTradingEvent.ClientInfo == tc.Event)							
								SendDataToClients(tc.Data, tc.Event);

                }
                catch (Exception e)
                {
                    Error("CTradeManagerServer. ThreadFunc", e);

                }

            }
        }


        /// <summary>
        /// Periodically send data tot TradeManager clients
        /// </summary>
        private void ThreadEnqueueStateData()
        {
            while (true)
            {
                try
                {


                    lock (_posInstrTotal)
                        EnqueueUpdateTotalInstrumentPosition(_posInstrTotal);

                   
                    Enqueue( _lstBotStatus.GetCopy(), enmTradingEvent.BotStatus);
                    Enqueue(_lsBotPosTrdMgr.GetCopy(), enmTradingEvent.BotPosTrdMgr);
                    Enqueue(_lstClientInfo.GetCopy(), enmTradingEvent.ClientInfo);
                }
                catch (Exception e)
                {
                    Error("CTradeManager",e);
                }

                Thread.Sleep(1000);
            }


        }


        public void UpdateTotalInstrumentPosition()
        {

            lock (_posInstrTotal)
            {
                var lstPos =  _dealingServer.PosBoxBase.ListPos.GetCopy();
                _posInstrTotal.Lst = new List<CPositionInstrTotal>(lstPos.Lst);
                _posInstrTotal.StockExchId = lstPos.StockExchId;
            }
        }

        public void UpdateBotStatus(CBotStatus botStatus)
        {
            botStatus.StockExchId = _dealingServer.StockExchId;            
            _lstBotStatus.Update(botStatus);
                               
        }



		/// <summary>
		///Call from CBasedealingServer.UpdateBotPosTrdMgr
		/// </summary>
		/// <param name="botId"></param>
		/// <param name="botPos"></param>
        public void UpdateBotPos(int botId, CBotPos botPos)
        {
            _lsBotPosTrdMgr.Update(botId, botPos);
            
        }

        public void AddClientsInfo(CClientInfo ci)
        {
             
            _lstClientInfo.Add(ci);
        }

        public void DeleteClientInfo(int conId)
        {
            _lstClientInfo.Delete(conId);
        }







        public void EnqueueSendAuthResponse(bool isSuccess, string errMsg, int connId)
        {

            _bqTraderData.Add(new CTradingData
            {
                Data = (object)new CAuthResponse { IsSuccess = isSuccess, ErrorMessage = errMsg },
                Event = enmTradingEvent.AuthResponse,
                ConnId = connId
            }
                                );

        }


       




        public void EnqueueUpdateTotalInstrumentPosition(CListPositionInstrTotal  lstPosInstrTotal)
        {
            Enqueue(lstPosInstrTotal, enmTradingEvent.PositionInstrTotal);

        }

        private void Enqueue(object data, enmTradingEvent evnt, int connId = cnstBroadCast)
        {

            _bqTraderData.Add(new CTradingData { Data = data, Event = evnt, ConnId = connId });


        }


        private void SendUpdateAuthResponse(int connId, CAuthResponse authResponse)
        {

            SendDataToClients(authResponse, enmTradingEvent.AuthResponse,  connId);
        }


        public void SendDataToClients(object ob, enmTradingEvent ev, int connId = cnstBroadCast)
        {
            byte[] arrHeader = /*CMessenger*/_messenger.GenBinaryMessageHeader((byte)ev);

            byte[] arrBody = CUtilProto.SerializeProto(ob);

            byte[] arrMsg = new byte[arrHeader.Length + arrBody.Length];

     
            Buffer.BlockCopy(arrHeader, 0, arrMsg, 0, arrHeader.Length);

  
            Buffer.BlockCopy(arrBody, 0, arrMsg, arrHeader.Length, arrBody.Length);
            if (connId == cnstBroadCast)
            {
				foreach (KeyValuePair<int, CTradeManagerLink> kvp in _dictConIdTradeMgrLink)
				{
					if (kvp.Value.IsLoggedOn)
						_tcpServer.SendMessage(kvp.Key, arrMsg);

				}

            }
            else
                _tcpServer.SendMessage(connId, arrMsg);
        }

	}
}
