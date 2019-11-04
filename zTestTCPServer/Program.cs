using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Threading;

using Common;
using Common.Utils;
using Common.Logger;
using Messenger;
using TCPLib;
using TCPLib.Interfaces;

using Common.Interfaces;
using TradingLib;
using TradingLib.Enums;
using TradingLib.ProtoTradingStructs;




namespace zTestTCPServer
{
    class Program
    {
        static void Main(string[] args)
        {


            CTCPServerUser serverUser = new CTCPServerUser();

            CTCPServer tcpServ = new CTCPServer(81, serverUser, "zTest");

            Thread.Sleep(2000);


            while (true)
            {

               /* enmTradingEvent ev = enmTradingEvent.SynchroniseTime;
                byte[] arrHeader = CMessenger.GenBinaryMessageHeader(ev);

                CTimeSynchroClass ts = new CTimeSynchroClass { DtCurrentTime = DateTime.Now };

                //  sw2_6.Stop();
                // byte[] arrBody = CUtil.SerializeBinary(ts).ToArray();


                byte[] arrBody = CUtil.SerializeProto(ts);

                //  byte[] arrBody = (CUtil.SerializeBinaryExt(ob, ref ms, ref formatter)).ToArray();

                //sw2_5.Stop();
                byte[] arrMsg = new byte[arrHeader.Length + arrBody.Length];

                Buffer.BlockCopy(arrHeader, 0, arrMsg, 0, arrHeader.Length);

                
                Buffer.BlockCopy(arrBody, 0, arrMsg, arrHeader.Length, arrBody.Length);


                tcpServ.SendMessage(0, arrMsg);
                */

                Thread.Sleep(1);

            }



        }
    }


    public class CTCPServerUser : ITCPServerUser

    {

         CLogger _log ;
         CLogger _logAlarm ;

    
        public CTCPServerUser()
        {
          

           _log = new CLogger("CTCPServerUser");
           string banner = "===============================================================================";
           Log(banner);
           _logAlarm = new CLogger("CTCPServerUser_alarm", true);
           Error(banner);


        }
        public void CallbackDisconnect(int id)
        {

        }


        public void CallbackNewConnection(int conId)
        {


           // CTCPConnection con = new CTCPConnection(


        }

        bool _bIsSynchro = false;
        long _cnt = 0;

        public void CallbackReadMessage(int id, byte[] message)
        {
            /*
            enmTradingEvent tradingEvent = new enmTradingEvent();
            //not tested 2017-11-10
            CMessenger messengerMock = new CMessenger();


            byte[] arrMsgBody = messengerMock.GetBinaryMessageHeaderAndBody(message, ref tradingEvent);



            CTimeSynchroClass ts = CUtilProto.DeserializeProto<CTimeSynchroClass>(arrMsgBody);

            DateTime dtCurrTime = ts.DtCurrentTime;
        
            
            double parMaxDev = 100;

            int dtMS = (int)(DateTime.Now - ts.DtCurrentTime).TotalMilliseconds;

            bool bIsDev = false;
            string arrow = "";

            if (dtMS > parMaxDev)
            {
                bIsDev = true;
                arrow = "  <===";
            }


            Log("dtCurrTime=" + dtCurrTime.ToString("yyyy.MM.dd HH:mm:ss.fff")+" dtMS="+dtMS+ arrow);

            int param = 20;            



            if (!_bIsSynchro)
                if (_cnt++>3)
                    if (Math.Abs(dtMS) > param)
            {
                _bIsSynchro = true;
                CTimeChanger tch = new CTimeChanger(-dtMS);
                Log("Time syncro dtMS=" + dtMS);
            }
         
                if (bIsDev)
                {
                    Error("Time dev="+dtMS);

                }

             */

        }
        public void Log(string message)
        {
            _log.Log(message);

        }
        public void Error(string description, Exception exception = null)
        {
            //TODO normal alarm
            _logAlarm.Log ("Error." + description);


        }

    }



}
