using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common;
using Common.Interfaces;

using Common.Utils;



using TradingLib.Enums;

using TradingLib.ProtoTradingStructs;
using TradingLib.ProtoTradingStructs.TradeManager;


using TCPLib;
using TCPLib.Interfaces;

using Messenger;

using TradeManager.Interfaces.Clients;



namespace TradeManager.TCPCommu
{
    public class CDataRecieverTrdMgr :  CBaseFunctional,  ITCPClientUser
    {

        //private  _messenger;       
        private IMessenger _messenger;
		private IClientDataRecieverTrdMgr _client;

		private int _connId;


		public CDataRecieverTrdMgr(IClientDataRecieverTrdMgr client, int connId)
			:base (client, String.Format(@"CDataRecieverTrdMgr_connId={0}__",connId))
        {
			_client = client;
			_connId = connId;

            _messenger = new CMessenger();			
			
		
        }

        public void CallbackReadMessage(byte[] message)
        {

			try
			{
				byte byteTradingEvent = 0;
				byte[] arrMsgBody = _messenger.GetBinaryMessageHeaderAndBody(message, ref byteTradingEvent);

				enmTradingEvent tradingEvent = (enmTradingEvent)byteTradingEvent;

                if (enmTradingEvent.AuthResponse == tradingEvent)
                    ProcessAuthResponse(arrMsgBody);
                else if (enmTradingEvent.PositionInstrTotal == tradingEvent)
                    ProcessPositionInstrTotal(arrMsgBody);
                else if (enmTradingEvent.BotStatus == tradingEvent)
                    ProcessBotStatus(arrMsgBody);
                else if (enmTradingEvent.BotPosTrdMgr == tradingEvent)
                    ProcessBotPosTrdMgr(arrMsgBody);
                else if (enmTradingEvent.ClientInfo == tradingEvent)
                    ProcessClientInfo(arrMsgBody);


                else
                    Error("Unknown message");
			}
			catch (Exception e)
			{
				Error("CDataRecieverTrdMgr.CallbackReadMessage", e);
			}


        }


        public void CallbackConnectionDisconnect()
        {
            Log("Disconnected");
            _client.OnDisconnect(_connId);
        }




        private void ProcessAuthResponse(byte[] arrMsgBody)
        {
            CAuthResponse aresp = CUtilProto.DeserializeProto<CAuthResponse>(arrMsgBody);           			
			Log("[AuthResponse]");
			_client.AuthResponse(aresp, _connId);

	
        }

        private void ProcessPositionInstrTotal(byte[] arrMsgBody)
        {
            CListPositionInstrTotal posInstr = CUtilProto.DeserializeProto <CListPositionInstrTotal>(arrMsgBody);
            Log("[CPositionInstrumentTotal]");

            _client.DataUser.UpdatePositionInstrTotal(posInstr);


        }

        private void ProcessBotStatus(byte[] arrMsgBody)
        {
            CListBotStatus listBotStatus = CUtilProto.DeserializeProto<CListBotStatus>(arrMsgBody);
            Log("[CListBotStatus]");

            _client.DataUser.UpdateListBotStatus(listBotStatus);

        }

        private void ProcessBotPosTrdMgr(byte[] arrMsgBody)
        {
            CListBotPosTrdMgr listBotPosTrdMgr = CUtilProto.DeserializeProto<CListBotPosTrdMgr>(arrMsgBody);
            Log("[CListBotPosTrdMgr]");

            _client.DataUser.UpdateBotPosTrdMgr(listBotPosTrdMgr);

        }

        private void ProcessClientInfo(byte[] arrMsgBody)
        {
            CListClientInfo listClientInfo = CUtilProto.DeserializeProto<CListClientInfo>(arrMsgBody);
            Log("[CListClietInfo]");

            _client.DataUser.UpdateClientInfo(listClientInfo);


        }







    }
}
