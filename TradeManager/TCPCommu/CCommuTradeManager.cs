using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using Common;
using Common.Interfaces;
#pragma warning disable CS0105 // Директива using для "System.Threading" ранее встречалась в этом пространстве имен
using System.Threading;
#pragma warning restore CS0105 // Директива using для "System.Threading" ранее встречалась в этом пространстве имен

using TCPLib;
using TCPLib.Interfaces;

using TradingLib;
using TradingLib.Abstract;
using TradingLib.Data;
using TradingLib.ProtoTradingStructs;





using TradeManager.Interfaces.Clients;


namespace TradeManager.TCPCommu
{
	public class CCommuTradeManager : CBaseCommunicator, IClientDataRecieverTrdMgr
    {

		private IClientCommuTradeManager _client;

        


        public IClientCommuTradeManager DataUser
        {
            get
            {
                return _client;
            }

        }


#pragma warning disable CS0169 // Поле "CCommuTradeManager._tcpClient" никогда не используется.
       // private CTCPClient _tcpClient;
#pragma warning restore CS0169 // Поле "CCommuTradeManager._tcpClient" никогда не используется.

        /// <summary>
        /// Dictionary ConId-DataRecier 
        /// </summary>
        private Dictionary<int, CDataRecieverTrdMgr> _dictDataReceiever = new Dictionary<int, CDataRecieverTrdMgr>();

        CPasswordSaver _passwordSaver;


		public CCommuTradeManager(IClientCommuTradeManager client) 
            : base(client)
        {
			_client = client;
            _passwordSaver = new CPasswordSaver(this);
		
        }


        



		

		public void ConnectToDealingServers()
		{

            LoadAccountData();

			while (App.IsRunning)
			{
				try
				{
					int i=0;
					foreach (var servConf in _serverConfig.Servers)
					{

                        CAuthRequest authReq = GetAuthRequest();
						//TODO from password saver !!!
						if (!servConf.IsConnected &&   IsEnabledUserConnectionToServer 
                            && authReq!=null)
							OnUserTryConnectToServer(new UserConReq 
											{
												/*AuthRequest = new CAuthRequest
														{ User = "constant",
														   Password = "ivanov"},
													*/
	                                            AuthRequest = authReq,
											 
											ConnNum = i,
											 ServerName = servConf.Name
											}
											);

						i++;
					}
					Thread.Sleep(2000);

                    IsEnabledUserConnectionToServer = true;


                }
				catch (Exception e)
				{
					Error("CCommuTradeManager.ConnectToDealingServers", e);
				}
				

			}
			
		
        }






        private void LoadAccountData()
        {
            try
            {
                //note: the same password for all connections
                CAuthRequest ar = _passwordSaver.GetUserAuthReq(0);
                if (ar != null)
                {
                    _client.VMAccount.User = ar.User;
                    _client.VMAccount.Password = ar.Password;
                }
            }
            catch (Exception e)
            {
                Error("CommuTradeManager.ListAccountData", e);
            }

        }



        public override void OnUserTryConnectToServer(UserConReq authReq)
        {
            base.OnUserTryConnectToServer(authReq);
            _passwordSaver.OnConnectionTrial(authReq);

        }



        private CAuthRequest GetAuthRequest()
        {


            CAuthRequest ar = null;

            if (_client.VMAccount.User != "" && _client.VMAccount.Password != null)
            {
                ar = new CAuthRequest()
                {
                    User = _client.VMAccount.User,
                    Password = _client.VMAccount.Password

                };
            }
         
            return ar;

        }






		protected override void AddDataReciever(int connId)
		{

            if (_dictDataReceiever.ContainsKey(connId))
                throw new Exception("DataReceiever is already exist");

			CDataRecieverTrdMgr dataReciever = new CDataRecieverTrdMgr(this, connId); ;
            _dictDataReceiever[connId] = dataReciever;
		}

		protected override ITCPClientUser GetTCPClientUser(int connId)
		{
			//throw new NotImplementedException();
            return _dictDataReceiever[connId];

		}

		protected override void OnAuthoriseSuccess(CAuthResponse aresp, int conId)
		{
			_client.UpdateDealingServersAuthStat(aresp, conId);
            _passwordSaver.OnConnectedSuccess(0);
            
			
		}


		protected override void OnUnsuccessfullConnectTrial(UserConReq conReq, int connId)
		{
			//throw new NotImplementedException();
		}


		protected override void RemoveDataReciever(int connId)
		{
			_dictDataReceiever.Remove(connId);

		}

		public void AuthResponse(CAuthResponse aresp, int connId)
		{
			if (aresp.IsSuccess)
			{
				_serverConfig.Servers[connId].IsConnected = true;
				IsEnabledUserConnectionToServer = true;
				OnAuthoriseSuccess(aresp, connId);

			}

		}

        public void OnDisconnect(int conId)
        {

            _client.OnConnectionDisconnect(conId);
            /*
            if (_serverConfig != null)
                if (_serverConfig.Servers != null)
                    if (_serverConfig.Servers.Count >= conId - 1)
                    {
                        _serverConfig.Servers[conId].IsConnected = false;
                    }
                    */

        }

       


        /*  public void SendAuthRequest(int conId, CAuthRequest ar)
          {

              //CAuthRequest ar = new CAuthRequest { User = _terminalConfig.User, Password = _terminalConfig.Password };
           //   enmTradingEvent ev = enmTradingEvent.AuthRequest;
            //  SendDataToServer(conId, ar, ev);



          }
           */






    }
}
