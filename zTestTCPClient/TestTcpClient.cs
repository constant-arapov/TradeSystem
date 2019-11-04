using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;


using TradingLib;
using Messenger;
using Common;
using Common.Utils;

using TCPLib;
using ProtoBuf;

using System.Diagnostics;

using TradingLib.Enums;
using Common.Logger;

namespace zTestTCPClient
{
    public class TestTcpClient
    {


        TCPClientUserStub _tcpClientStub = new TCPClientUserStub();
        CTCPClient _tcpClient;

        CLogger _log;
        public TestTcpClient()
        {

            _log = new CLogger("TestTcpClient");

            _log.Log("============================================================================================================");
            _tcpClient = new CTCPClient("83.146.113.81", 81, _tcpClientStub);


            while (!_tcpClient.ConnectToServer())
                Thread.Sleep(1000);



            (new Thread(ThreadWrite)).Start();

            while (true)
                Thread.Sleep(100);


        }


        Stopwatch sw = new Stopwatch();
        public  void ThreadWrite()
        {
            /*
            int parSleep = 10000;
            while (true)
            {
                enmTradingEvent ev = enmTradingEvent.SynchroniseTime;
                byte[] arrHeader = CMessenger.GenBinaryMessageHeader(ev);
                sw.Reset();
                sw.Start();
                CTestMessageClass ts = new CTestMessageClass { DtCurrentTime = DateTime.Now };

                //  sw2_6.Stop();
                // byte[] arrBody = CUtil.SerializeBinary(ts).ToArray();


                byte[] arrBody = CUtil.SerializeProto(ts);

                //  byte[] arrBody = (CUtil.SerializeBinaryExt(ob, ref ms, ref formatter)).ToArray();

                //sw2_5.Stop();
                byte[] arrMsg = new byte[arrHeader.Length + arrBody.Length];

                Buffer.BlockCopy(arrHeader, 0, arrMsg, 0, arrHeader.Length);


                Buffer.BlockCopy(arrBody, 0, arrMsg, arrHeader.Length, arrBody.Length);


                _tcpClient.WriteMessage(arrMsg);

                _log.Log("ms="+ sw.ElapsedMilliseconds+" tick="+ sw.ElapsedTicks);
                sw.Stop();
                 
                //_tcpClient.WriteMessage)
                Thread.Sleep(parSleep);
            }*/
        }



    }


   





}
