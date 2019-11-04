using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Net;
using System.Net.Sockets;
using System.Threading;

using System.Threading.Tasks;


using Common;
using Common.Interfaces;
using Common.Logger;

using TCPLib.Interfaces;


namespace TCPLib
{
    public class CTCPClient : ITCPConnectionUser, ILogable, IAlarmable
    {

        TcpClient _tcpClient;

        IPAddress _ipServer;
        int _portServer;
        CTCPConnection _tcpConnection;

        CLogger _log;

        ITCPClientUser _tcpClientUser;
        IAlarmable _alarmer;

        public string LogSubDir
        {
            get
            {
                return "TCPClient";
            }

        }

        public string TestMessage
        {
            get
            {
                return CTCPHelper.TestClientMessage;                 
            }

        }

        public bool IsConnected
        {

            get
            {
                if (_tcpClient == null || !_tcpClient.Connected)
                    return false;

                    return true;
            }


        }




        public CTCPClient(string ipServer, int portServer, ITCPClientUser tcpClientUser =null)
        {
             _ipServer = IPAddress.Parse(ipServer);
            _portServer = portServer;
           
            _tcpClient = new TcpClient();
            _log = new CLogger("TCPClient");

            _tcpClientUser = tcpClientUser;
            
                
        }


        public void Log(string message)
        {
            _log.Log(message);

        }



        public void Error(string description, Exception e = null)
        {
            if (_tcpClientUser != null)
            {
                _tcpClientUser.Error(description, e);
                Log("Error." + description);
            }

        }


        public void OnSecurityThreat(string ip)
        {



        }
        
        public void TaskRemoveConnection()
        {

            while (!_tcpConnection.IsThreadReadFinished || !_tcpConnection.IsThreadWriteFinished)
            {
                //2018-08-29 terminate write queue to prevent looping
                _tcpConnection.TerminateWriter();
                Thread.Sleep(50);
            }

            _tcpConnection.Dispose();
            _tcpConnection = null;
       
        }

        public void CallbackConnectionDisconnect(object arg)
        {
            int id = (int)arg;
            _tcpClientUser.CallbackConnectionDisconnect();//2018-08-29
            (new Task(new Action(() => TaskRemoveConnection()))).Start();
        }


        public void CallbackReadMessage(int id,byte[] message)
        {
            if (_tcpClientUser != null)
                _tcpClientUser.CallbackReadMessage(message);

        }

        public void Disconnect()
        {
           // _tcpClient.GetStream().Close();
           // _tcpClient.Close();
            //_tcpConnection.Dispose();
            //(new Task(new Action(() => TaskRemoveConnection()))).Start();
            _tcpConnection.Dicsonnect();
        }

       
        //2018-05-27 changed to 7 was 5
        private int _parWaitTimeoutSec = 7;


        public bool ConnectToServer()
        {
            try
            {
            
                //2018-03-29 change to assync, was before:
              //  _tcpClient.Connect(_ipServer, _portServer);
                //_tcpConnection = new CTCPConnection(_tcpClient, 0, this);

                var result = _tcpClient.BeginConnect(_ipServer, _portServer, new AsyncCallback(ConnectCallback), _tcpClient);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(_parWaitTimeoutSec));
              
              



                if (success)
                {
                    if (_tcpClient.Connected) //changed 2018-07-17
                    {
                        _tcpConnection = new CTCPConnection(_tcpClient, 0, this);
                        Log(String.Format("Succesfully connected ip={0} port={1}", _ipServer, _portServer));
                        return true;
                    }
                    else
                    {   
                        //connection timeout
                        return false;
                    }
                }
                else
                {
                    Error("CTCPClient.ConnectToServer connect server timeout");
                    return false;
                }
            }
            catch (Exception e)
            {
                Error("CTCPClient.ConnectToServer ",e);
                //if (e != null)
                  //  Thread.Sleep(0);

				



                return false;
            }


        }

        private void ConnectCallback(IAsyncResult ar)
        {
            //Nothing to do here this must be as BeginConnect must be not null

        }


        public void WriteMessage(byte[] message)
        {
            if (_tcpConnection !=null)
                _tcpConnection.SendMessage(message);

        }


        private string _dummyMessage =
       "TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST"
   + "TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST"
   + "TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST<";




    }
}
