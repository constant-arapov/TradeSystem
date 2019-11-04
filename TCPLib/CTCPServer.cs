using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Common;
using Common.Interfaces;
using Common.Logger;

using TCPLib.Interfaces;


namespace TCPLib
{
    public class CTCPServer : ITCPConnectionUser, ILogable
    {
        private TcpListener _listener; // Объект, принимающий TCP-клиентов

        private Dictionary<int,CTCPConnection> _clientConnectionsPool = new Dictionary<int,CTCPConnection>();

        private CLogger _log;


        private int _connCounter = 0;
        private int _port;
        private string _name;
    

       // Dictionary<int, TcpClient> _dictTCPClients = new Dictionary<int, TcpClient>();

        private ITCPServerUser _tcpServerUser;


                             
        private List<string> _lstBlackIp = new List<string>();

        




        public string LogSubDir
        {
            get
            {
                return "TCPServer";
            }

        }

        public string TestMessage
        {
            get
            {           
                return CTCPHelper.TestServerMessage;
            }

        }

		public void Log(string message)
		{
			_log.Log(message);
		}
      

        public CTCPServer(int port, ITCPServerUser tcpServerUser, string name)
        {
            _port = port;
            _tcpServerUser = tcpServerUser;
            _name = name;

            (new Thread(MainThread)).Start();


        }

        public string GetClientIp(int conId)
        {

            string ip = "not found";

            CTCPConnection con = null;
            lock (_clientConnectionsPool) 
                if (_clientConnectionsPool.TryGetValue(conId,out con))                
                    if (con !=null)                    
                        ip = con.ClientIp;
                    

                


            return ip;

        }




        public void MainThread()
        {
            _log = new CLogger("TCPServer_" +_name);

            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.Start();

            while (true)
            {
                // Accept new clients
                try
                {
                    TcpClient tcpClient = _listener.AcceptTcpClient();
                    if (IsInBlackList(tcpClient))
                    {
                        tcpClient.Close();

                        continue;
                    }

                    // _dictTCPClients[_connCounter] = tcpClient;
                    AddClient(_connCounter, tcpClient);
                    _connCounter++;
                }
                catch (Exception e)
                {
                    Error("CTCPServer.MainThread",e);
                }
            }




        }


        private bool IsInBlackList(TcpClient tcpClient)
        {
            string ip = ((System.Net.IPEndPoint) tcpClient.Client.RemoteEndPoint).Address.ToString();
            lock (_lstBlackIp)
            {
                if (_lstBlackIp.Contains(ip))
                {
                    Error("Host "+ip+" is from black list. Close it");
                    return true;
                }

            }

            return false;

        }

        public void OnSecurityThreat(string ip)
        {
            lock (_lstBlackIp)
            {
                if (!_lstBlackIp.Contains(ip))
                    _lstBlackIp.Add(ip);

            }
        }





    
        public void Error(string description, Exception exception = null)
        {
            //TODO normal alarm
          
            Log("Error."+description);
            if (_tcpServerUser != null)
               _tcpServerUser.Error(description, exception);


        }


        public void AddClient(int connId, TcpClient tcpCleint) //2018-04-23
        {
            //2018-04-23 changed
            lock (_clientConnectionsPool)
             _clientConnectionsPool[connId] = new CTCPConnection(/*_dictTCPClients[_connCounter]*/ tcpCleint, connId, this);


            string ip = "unknown";
            int port = -1;
            if (tcpCleint.Client.RemoteEndPoint != null)
            {
                if (tcpCleint.Client.RemoteEndPoint is IPEndPoint)
                {
                    IPEndPoint endPoint = (IPEndPoint)tcpCleint.Client.RemoteEndPoint;
                    ip = endPoint.Address.ToString();
                    port = endPoint.Port;
                }
            }
            Log(String.Format("Added connection conId={0} address={1} port={2}",  
                              connId,
                              ip,
                              port));

            _tcpServerUser.CallbackNewConnection(_connCounter);
        }

    

        public void TaskRemoveConnection(int id)
        {

            //for (int i = 0; i < _clientConnectionsPool.Count; i++)

            Log(String.Format("Try to remove connection id={0}", id));

            if (_clientConnectionsPool.ContainsKey(id))
            {
                CTCPConnection con = _clientConnectionsPool[id];
                //2018-05-20 terminate when queue is empty
                con.TerminateWriter();

                if (con.Id == id)
                {
                    while (!con.IsThreadReadFinished || !con.IsThreadWriteFinished)
                        Thread.Sleep(50);

                   // _dictTCPClients[id] = null;


                    con.Dispose();
                    //2018-04-23
                    lock (_clientConnectionsPool)
                    {
                        //2018-05-19 added check
                        if (_clientConnectionsPool.ContainsKey(id))
                        {
                            _clientConnectionsPool[id] = null;
                            con = null;
                            _clientConnectionsPool.Remove(id);
                        }
                        else
                        {
                            Error(
                                String.Format("TaskRemoveConnection. No connections id={0} in _clientConnectionsPool", 
                                                id));
                        }
                    }
                    //TEMPO remove
                    // GC.Collect();
                    // Thread.Sleep(60000);
                    Log(String.Format("Connection removed id={0}", id));
                    return;
                }

            }           
        }

        public void SendMessage (int ConId, byte[] message)
        {
            try 
            {
               // if (_clientConnectionsPool.Count > 0 )
               //2018-04-23
               lock (_clientConnectionsPool)
                    if (_clientConnectionsPool.ContainsKey(ConId))
                        _clientConnectionsPool[ConId].SendMessage(message);


            }
            catch (Exception e)
            {
                Error("Error sending message",e);

            }
        }
        /*
        public void SendMessage(int ConId, byte[] message)
        {
            try
            {
                if (_clientConnectionsPool.Count > 0)
                    _clientConnectionsPool[ConId].SendMessage(message);


            }
            catch (Exception e)
            {
                Error("Error sending message", e);

            }
        }
        */



        public void CallbackConnectionDisconnect(object arg)
        {
            int id = (int)arg;
			_tcpServerUser.CallbackDisconnect(id);
            (new Task(new Action(() => TaskRemoveConnection(id)))).Start();
        }

        public void CallbackReadMessage(int id,byte[] message)
        {
            if (_tcpServerUser != null)
                _tcpServerUser.CallbackReadMessage(id, message);
        }
        ~CTCPServer()
        {
            // Если "слушатель" был создан
            if (_listener != null)
            {
                // Остановим его
                _listener.Stop();
            }
        }




    }
}
