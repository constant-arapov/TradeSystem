using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Utils;
using Common.Interfaces;



using TCPLib;
using TCPLib.Interfaces;

using Messenger;

using TradingLib.Interfaces.Components;
using TradingLib.Enums;
using TradingLib.Data;
using TradingLib.ProtoTradingStructs;



namespace TradingLib.Abstract
{
    public abstract class CBaseCommunicator :  CBaseFunctional
    {


        protected abstract void AddDataReciever(int connId);
        protected abstract ITCPClientUser GetTCPClientUser(int connId);
        protected abstract void RemoveDataReciever(int connId);
        protected abstract void OnUnsuccessfullConnectTrial(UserConReq conReq, int connId);
        protected abstract void OnAuthoriseSuccess(CAuthResponse aresp, int conId);




        public bool IsEnabledUserConnectionToServer { get; set; }

        protected CServersConf _serverConfig;
        public CServersConf ServersConf
        {
            get
            {
                return _serverConfig;
            }
            set
            {
                _serverConfig = value;
            }

        }


        /// <summary>
        /// Dictionary ConId-TCPClients
        /// </summary>
        protected Dictionary<int, CTCPClient> _dictTCPClients = new Dictionary<int, CTCPClient>();

        protected  IMessenger _messenger = new CMessenger();

    
       

        public CBaseCommunicator(IAlarmable alarmer)
            :base (alarmer)
        {
            IsEnabledUserConnectionToServer = true;
            ReadServerConf();

        }

        public virtual void OnUserTryConnectToServer(UserConReq authReq)
        {
            IsEnabledUserConnectionToServer = false;
            TryUserConnectToServer(authReq);
            

        }

        public bool IsConnected(int conId)
        {
            if (_dictTCPClients == null || _dictTCPClients.Count == 0 ||
                !_dictTCPClients.ContainsKey(conId) || !_dictTCPClients[conId].IsConnected)
                return false;

            return true;

        }




       

       

        private void ReadServerConf()
        {
			try
			{
				string pathToConfig = CUtil.GetConfigDir() + @"\servers.xml";
				_serverConfig = new CServersConf(pathToConfig, false);

				CSerializator.Read<CServersConf>(ref _serverConfig);

				_serverConfig.Servers.ForEach(a => a.IsConnected = false);
			}
			catch (Exception e)
			{
				Error("CBaseCommunicator",e);
			}
        

        }

        protected bool GetTCPClient(int conId, out CTCPClient tcpClient)
        {


            return _dictTCPClients.TryGetValue(conId, out tcpClient);


        }



        protected void AddTCPCleintAndDataReciever(int connId)
        {


            AddDataReciever(connId);

            if (_dictTCPClients.ContainsKey(connId))
                throw new Exception("TCP client is already exist");


            CTCPClient tcpClient = new CTCPClient(_serverConfig.Servers[connId].IP, (int)_serverConfig.Servers[connId].Port, /*dataReciever*/ GetTCPClientUser(connId));
            _dictTCPClients[connId] = tcpClient;


        }



        protected void RemoveTCPClientAndReciever(int connId)
        {
            RemoveDataReciever(connId);
            _dictTCPClients.Remove(connId);
        }



        /// <summary>
        /// Creates message of specificated format (header, body) and writes message to tcpClient.
        /// 
        /// 
        /// 
        /// Call from:
        /// 1) Internal class methods
        /// 2) From MarketViewModel
        /// </summary>
        /// <param name="conId"></param>
        /// <param name="ob"></param>
        /// <param name="ev"></param>
        public void SendDataToServer(int conId, object ob, enmTradingEvent ev)
        {

            CTCPClient tcpClient = null;
            if (GetTCPClient(conId, out tcpClient))
            {

                byte[] arrHeader = _messenger.GenBinaryMessageHeader((byte)ev);
                byte[] arrBody = CUtilProto.SerializeProto(ob);

                byte[] arrMsg = new byte[arrHeader.Length + arrBody.Length];

                Buffer.BlockCopy(arrHeader, 0, arrMsg, 0, arrHeader.Length);
                Buffer.BlockCopy(arrBody, 0, arrMsg, arrHeader.Length, arrBody.Length);

                tcpClient.WriteMessage(arrMsg);

            }

        }


        public void TryUserConnectToServer(UserConReq conReq)
        {
            //changed 2018-03-29
            try
            {


                int connId = conReq.ConnNum;

                if (!_dictTCPClients.ContainsKey(connId))
                {
                    AddTCPCleintAndDataReciever(connId);

                    bool bSuccess = TryConnectToServer(connId);
                    //Connection attempt was not sucesss
                    if (!bSuccess)
                    {
                        RemoveTCPClientAndReciever(connId);
                        OnUnsuccessfullConnectTrial(conReq, connId);

                        IsEnabledUserConnectionToServer = true;

                        return; //if server unavailable - exit
                    }
                }

                //if available send auth request to server
                SendAuthRequest(connId, conReq.AuthRequest);
            }
            catch (Exception e)
            {
                Error("CBaseCommunicator.TryUserConnectToServer", e);
            }



        }



        public void SendAuthRequest(int conId, CAuthRequest ar)
        {

            //CAuthRequest ar = new CAuthRequest { User = _terminalConfig.User, Password = _terminalConfig.Password };
            enmTradingEvent ev = enmTradingEvent.AuthRequest;
            SendDataToServer(conId, ar, ev);



        }


        private bool TryConnectToServer(int connId)
        {
            if (_dictTCPClients[connId].IsConnected)
                return true;
            else
                return _dictTCPClients[connId].ConnectToServer();

        }

        
        /// <summary>
        /// When received message is AuthResponce, DataReciever
        /// call this method. If authorisation sucessfull set servers 
        /// as "connected" and call class client's actions when
        /// authorisation is success.
        /// </summary>
        /// <param name="aresp"></param>
        /// <param name="conId"></param>
        protected void OnAuthResponse(CAuthResponse aresp, int conId)
        {

            if (aresp.IsSuccess)
            {
                _serverConfig.Servers[conId].IsConnected = true;                
                OnAuthoriseSuccess(aresp, conId);

            }
            IsEnabledUserConnectionToServer = true;

        }



        public bool IsConnectedSomething()
        {
            if (_dictTCPClients == null || _dictTCPClients.Count == 0)
                return false;

            foreach (var kvp in _dictTCPClients)
                if (kvp.Value.IsConnected)
                    return true;





            return false;
        }

    }
}
