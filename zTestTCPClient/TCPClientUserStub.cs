using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


using Common;
using Common.Interfaces;

using TCPLib.Interfaces;


using TradingLib;
using Messenger;
using Common.Logger;


namespace zTestTCPClient
{
    class TCPClientUserStub : ITCPClientUser
    {

        CLogger _logger;


        public TCPClientUserStub()
        {


            _logger = new CLogger("TCPClientUserStub");


        }

     
        public void CallbackConnectionDisconnect()
        {

        }





        public void  CallbackReadMessage(byte[] message)
        {
            
            /*
           
           
            */
        }

        public void Log(string msg)
        {

            _logger.Log(msg);
        }

        public void Error(string msg, Exception e = null)
        {
            _logger.Log(msg);
        }

    }
}
