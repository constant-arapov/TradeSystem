using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;


using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics;

using Common;
using Common.Interfaces;
using Common.Collections;
using Common.Logger;
using Common.Utils;

using TCPLib.Interfaces;


namespace TCPLib
{
    public class CTCPConnection : IDisposable
    {
        TcpClient _tcpClient;
        CLogger _loggerRead;
        CLogger _loggerWrite;

        string _logSubdir;


    
        NetworkStream _tcpStream;

        const byte _startChar = 0xFF;
        const int _Mb = 1024 * 1024;


        const int _maxMessageSize = 10 * _Mb;


        Thread _bThreadRead, _bThreadWrite;

       //bool _bSEQOnDisconnectStarted = false;
        bool _bSEQOnDisconnectFinished = false;

        public bool IsThreadReadFinished { get; set; }
        public bool IsThreadWriteFinished { get; set; }

        Mutex _mxDisconnect = new Mutex();

    

        public int Id { get; set; }

        private ITCPConnectionUser _tcpConnectionUser;

        BlockingCollection<string> _queueWrite = new BlockingCollection<string>();

       // BlockingCollection<byte[]> _queueWriteBinary = new BlockingCollection<byte[]>();
        CBlockingQueue<byte[]> _queueWriteBinary = new CBlockingQueue<byte[]>();



        bool _bEnableLogRead = true;
        bool _bEnableLogWrite = true;

       



        public CTCPConnection(TcpClient tcpClient, int connId, ITCPConnectionUser tcpConnectionUser)
                                
                                
        {
            _tcpClient = tcpClient;
            _tcpClient.NoDelay = true;
            _tcpClient.ReceiveBufferSize = 65536;

            _tcpStream = _tcpClient.GetStream();
            _logSubdir = tcpConnectionUser.LogSubDir;   //logSubdir;

        
            Id = connId;

            _tcpConnectionUser = tcpConnectionUser;


            IsForceClose = false;

            ClientIp = GetClientIp();
            ClientPort = GetClienPort();


            _bThreadRead = new Thread(ThreadRead);
            _bThreadRead.Start();


            _bThreadWrite = new Thread(ThreadWrite);
            _bThreadWrite.Start();

          //  Error("CTCPConnection test");
          
        }

        public void TerminateWriter()
        {

            _queueWriteBinary.IsTerminated = true;
        }




        bool IsForceClose { get; set; }

        public void Dicsonnect()
        {


            
             IsForceClose = true;
             
            _tcpClient.GetStream().Close();

            _bThreadRead.Abort();
            _bThreadWrite.Abort();
            while (_bThreadRead.IsAlive || _bThreadWrite.IsAlive)
                Thread.Sleep(1);
         
            _tcpClient.Close();

        }


        public void SendMessage(string message)
        {

            _queueWrite.Add(message);
        }

        public void SendMessage(byte[] message)
        {

            _queueWriteBinary.Add(message);

        }
        public string ClientIp
        { 
            get;set;
            /*
            get
            {
                if (_tcpClient == null || _tcpClient.Client == null ||
                    (IPEndPoint)_tcpClient.Client.RemoteEndPoint == null
                    )
                    return "";

                return ((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Address.ToString();
                                                            
            }
            */
        }


        public string GetClientIp()
        {

             if(_tcpClient == null || _tcpClient.Client == null ||
                    (IPEndPoint)_tcpClient.Client.RemoteEndPoint == null
                    )
                    return "unknown";

                return ((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Address.ToString();


        }

        public int GetClienPort()
        {


            if (_tcpClient == null || _tcpClient.Client == null ||
              (IPEndPoint)_tcpClient.Client.RemoteEndPoint == null)
                return -1;

            return ((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Port;



        }

        public int ClientPort
        {
            get;
            set;

            /*get
            {
                if (_tcpClient == null || _tcpClient.Client == null  ||
                   (IPEndPoint)_tcpClient.Client.RemoteEndPoint == null)
                    return -1;

                return ((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Port;
            }
            */
        }
        public bool IsConnected
        {

           get
            {
                if (_tcpClient!=null && _tcpClient.Connected)
                    return true;

                return false;
            }


        }


        public void Dispose()
        {
            if (_tcpStream != null) _tcpStream.Close();
            _tcpStream = null;
            if (_tcpClient != null) _tcpClient.Close();
            _tcpClient = null;
             _loggerRead.Dispose();
             _loggerWrite.Dispose();
             _loggerRead = null;
             _loggerWrite = null;
            _bThreadRead = null;
            _bThreadWrite = null;

            _mxDisconnect.Dispose();
            _mxDisconnect = null;

        }
 
        private void Error(string msg, Exception exception = null)
        {
            _tcpConnectionUser.Error(msg, exception);

        }

        Stopwatch sw2_1 = new Stopwatch();

        private byte[] ReadMessage()
        {
           // byte[] arrBytes = new byte[_tcpClient.ReceiveBufferSize];
            byte[] arrFirstByte = new byte[1];
            byte[] arrMessageSize = new byte[sizeof(int)];

            //int res = _tcpClient.GetStream().Read(arrFirstByte, 0, 1);

           
            ReadDataFromSocket(1, arrFirstByte);

            if (arrFirstByte[0] != _startChar)
                ThrowSecurityException("Invalid first char of message ! CH=" + arrFirstByte[0]);
     
            
           
            sw2_1.Reset();
            sw2_1.Start();
           

                    
            ReadDataFromSocket(sizeof(int), arrMessageSize);
            int messageSize = BitConverter.ToInt32(arrMessageSize, 0);
            if (!IsValidMessageSize(messageSize))            
                ThrowSecurityException("Invalid message size ! Sz="+arrMessageSize);
              
            
            

            byte[] buffMessage = new byte[messageSize];
            ReadDataFromSocket(messageSize, buffMessage);

            sw2_1.Stop();


            _loggerRead.Log("Read sw2_1.Milliseconds=" + sw2_1.ElapsedMilliseconds + " sw2_1.Ticks=" + sw2_1.ElapsedTicks);
         

            return buffMessage;

        }

        private void ThrowSecurityException(string msg)
        {

            msg  += " ip=" + ClientIp + " port=" + ClientPort;

            Error(msg);
            throw (new ExceptionSecurityThreat(msg)
                   {
                       ClientIp = ClientIp,
                       ClientPort = ClientPort.ToString()
                       
                   }

                   );



        }



        private bool IsValidMessageSize(int sizeMsg)
        {
            if (sizeMsg >= 0 && sizeMsg <= _maxMessageSize)
                return true;

            return false;

        }
        private void ReadDataFromSocket(int  sizeOfData, byte[] buffData)
        {
             int totalByteRead = 0;
        
            int cntRead = 0;
            int maxCntTrial = 1000;

            //todo get one stream class
            NetworkStream networkStream = _tcpClient.GetStream();

            do
            {
           
            
                int dbgLastTotalByteRead = totalByteRead;
                int byteToRead =  sizeOfData - totalByteRead;



                int byteWasRead = networkStream.Read(buffData, totalByteRead,
                                            byteToRead);
              

                totalByteRead += byteWasRead;
                cntRead++;

                if (totalByteRead >  sizeOfData)                
                    throw new ExceptionReadMessage("ReadMessage. totalByteRead > messageSize");
           
                if (cntRead > maxCntTrial)
                    throw new ExceptionReadMessage("ReadMessage. cntRead > maxCntTrial");
                
            }
            while (totalByteRead !=  sizeOfData);

           


        }
     
        public void ThreadRead ()
        {

           // string stDate = CUtil.GetTimeString(DateTime.Now);

            //TODDO set switchmode to true in production
            _loggerRead = new CLogger(GetTimeConnIpPort() + "_Reader", true, _logSubdir,true);
          
            _loggerRead.Log("=========================  Begin read ===========================================");

            while (IsConnected)
            {
                try
                {
                     
                      byte [] msg =  ReadMessage();
                   
                      CallbackReadMessage(msg);
                    
                    
                }
                
                catch (ExceptionSecurityThreat e)
                {

                    Error(e.Message, e);
                    _tcpClient.Close();
                    OnDisconnect();
                    _tcpConnectionUser.OnSecurityThreat(ClientIp);

                }
                catch (ExceptionReadMessage e)
                {
                    if (!IsForceClose)
                    {
                        Error(e.Message, e);                        

                        _tcpClient.Close();
                        OnDisconnect();
                      
                    }
                }
                catch (Exception e)
                {
                    if (!IsForceClose)
                    {
                        //2018-01-11
						if (e.InnerException is SocketException)
						{
							SocketException socExc = (SocketException) e.InnerException;
							if (socExc.ErrorCode == CodeStockException.WSAECONNRESET)
								_tcpConnectionUser.Log(this.ClientIp+" Connection closed by peer");
						}
						else
							Error(e.Message, e);

                        if (!IsConnected)
                            OnDisconnect();
                    }
                }


            }
            IsThreadReadFinished = true;
        }

        /// <summary>
        /// Call from:
        ///  Call on disconnect when on ThreadRead or ThreadWrite
        ///  exception catched. As exceptions on both threads
        ///  could be catched simultaneously  - only first 
        ///  caller executes (executes client event) and blocks
        ///  execution of the second caller.The second caller
        ///  waits the first caller ends end exits.
        ///  
        /// 1) ThreadRead (on catch exception)
        /// 2) ThreadWrite (on catch exception)
        /// </summary>
        public void OnDisconnect()
        {

            //note on of thread read or write do rise this event
            _mxDisconnect.WaitOne();


            if (_bSEQOnDisconnectFinished)
            {
                _mxDisconnect.ReleaseMutex();
                return;
            }


            _tcpConnectionUser.CallbackConnectionDisconnect((object)Id);
           // _callbackDisconnect((object) Id);

            _bSEQOnDisconnectFinished = true;

            _mxDisconnect.ReleaseMutex();
        }





        Stopwatch sw1_1 = new Stopwatch();

        public void ThreadWrite()
        {

            try
            {
                //TODO remove flush mode in production
                _loggerWrite = new CLogger(GetTimeConnIpPort() + "_Writer", true, _logSubdir, true);


                while (true)
                // foreach (byte[] message in _queueWriteBinary.GetConsumingEnumerable())
                {
                    byte[] message = _queueWriteBinary.GetElementBlocking();
                    //2018-05-20 process exit if queue is empty
                    if (message == null)
                    {
                        _loggerWrite.Log("Exit with empty queue");
                        OnDisconnect();
                        IsThreadWriteFinished = true;
                       
                        return;
                    }


                    //was 3 changed to 10 2017-10-25
                    _queueWriteBinary.CheckLimit(10, true, 3, _tcpConnectionUser, "TCPConnection._queueWriteBinary");


                    if (!IsConnected)
                    {
                        OnDisconnect();
                        _loggerWrite.Log("Exit on non empty queue");
                        break;
                    }
                    try
                    {
                        sw1_1.Reset();
                        sw1_1.Start();
                        WriteMessage(message);
                        sw1_1.Stop();
                        _loggerWrite.Log("Write sw1_1.Milliseconds=" + sw1_1.ElapsedMilliseconds + " sw1_1.Ticks=" + sw1_1.ElapsedTicks);
                    }
                    catch (Exception e)
                    {

                        if (!IsConnected)
                        {
                            OnDisconnect();
                            break;
                        }
                    }


                }
                IsThreadWriteFinished = true;
            }

            catch (Exception e)
            {
                Error("ThreadWrite", e);

            }


        }


      









        int i = 0;


        public void CallbackReadMessage(byte[] message)
        {
            if (_tcpConnectionUser != null) 
                _tcpConnectionUser.CallbackReadMessage(Id,message);
           
        }

        private void WriteMessage (string message)
        {

            NetworkStream tcpStream = _tcpClient.GetStream();
            byte[] _arrStartCh = new byte[] { _startChar };
            tcpStream.Write(_arrStartCh, 0, 1);
            string messageTot = i++.ToString() + "__" + message +"<";
            int sz = messageTot.Length;
            byte[] szArr = BitConverter.GetBytes(sz);           
            tcpStream.Write(szArr, 0, sizeof(int));
            //byte[] message = Encoding.UTF8.GetBytes(i++.ToString());                                
            byte[] arrMessage = Encoding.UTF8.GetBytes(messageTot);
            tcpStream.Write(arrMessage, 0, arrMessage.Length);
            tcpStream.Flush();
            if ( _bEnableLogWrite)
                _loggerWrite.Log(messageTot);

        }


        

        private void WriteMessage(byte [] message)
        {

            NetworkStream tcpStream = _tcpClient.GetStream();
            byte[] _arrStartCh = new byte[] { _startChar };
            tcpStream.Write(_arrStartCh, 0, 1); 
           
            int sz = message.Length;
            byte[] szArr = BitConverter.GetBytes(sz);
            tcpStream.Write(szArr, 0, szArr.Length);
            tcpStream.Write(message, 0, sz);
           
            tcpStream.Flush();
        




        }
        private string GetTimeConnIpPort()
        {

            return CUtilTime.GetTimeString(DateTime.Now) + "_" + GetConnIpPort();


        }

        private string GetConnIpPort()
        {


            return "IP=" + this.ClientIp + " Port=" + this.ClientPort;
        }


        ~CTCPConnection()
        {

            Console.WriteLine("CTCPConnection Id="+Id+" finilized");

        }



        private string _dummyMessage =
       "TEST1 TEST2 TEST3 TEST4 TEST5 TEST6 TEST7 TEST8 TEST9 TEST10 TEST11 TEST12 TEST13 TEST14"
   + "TEST15 TEST16 TEST17 TEST18 TEST19 TEST20 TEST21 TEST22 TEST23 TEST24 TEST25 TEST26 TEST27 TEST28"
   + "TEST29 TEST30 TEST31 TEST32 TEST33 TEST34 TEST35 TEST36 TEST37 TEST38 TEST39 TEST40 TEST41 TEST42<";





    }
}
