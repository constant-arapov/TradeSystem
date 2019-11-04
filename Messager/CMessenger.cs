using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.IO;
using System.Xml;

using Common;
using Common.Interfaces;
using Common.Utils;


//using TradingLib;
//using TradingLib.Interfaces;
//using TradingLib.Interfaces.Components;
//using TradingLib.Enums;


namespace Messenger 
{
    public class CMessenger : IMessenger
    {
        private static long _cyclicCount=0;
       /* public static string GenMessageHeader(enmTradingEvent enmEvnt)
        {
            UpdateCyclicCount();
          
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("header");
            xml.AppendChild(root);

            root.SetAttribute("type", enmEvnt.ToString());
            root.SetAttribute("time", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff"));
            root.SetAttribute("cyclic_count", _cyclicCount.ToString());

            string s = xml.OuterXml;

            return s;
        }
        */
        private static void UpdateCyclicCount()
        {
            if (_cyclicCount > Int64.MaxValue)
                _cyclicCount = 1;
            else
                _cyclicCount++;


        }
                                        //2017-11-30 changed enmTradingEvent to byte
        public /*static*/ byte[] GenBinaryMessageHeader(/*enmTradingEvent*/byte enmEvnt)
        {

            UpdateCyclicCount();
         
            byte[] arrType = new byte[] { (byte)enmEvnt };

            byte[] arrCyclicCount = BitConverter.GetBytes(_cyclicCount);
           


            byte[] arr  = new byte[arrType.Length + arrCyclicCount.Length];

       
            Buffer.BlockCopy(arrCyclicCount, 0, arr, 0, arrCyclicCount.Length);
            Buffer.BlockCopy(arrType, 0, arr, arrCyclicCount.Length, arrType.Length);

           
            return arr;
        }
                                                                        //2017-11-30 changed enmTradingEvent to byte
        public /*static*/ byte[] GetBinaryMessageHeaderAndBody(byte[] message, ref /*enmTradingEvent*/byte ev)
        {
          
            byte[] arrCyclicCount = new byte[sizeof(long)];
            byte[] arrType = new byte[1];


            Buffer.BlockCopy(message, 0, arrCyclicCount, 0, arrCyclicCount.Length);
           ev = /*(enmTradingEvent)*/  message[arrCyclicCount.Length ];
           int msgBodyPos = arrCyclicCount.Length + 1;
           int msgBodyLen = message.Length - msgBodyPos;

           byte[] msgBody = new byte[msgBodyLen];
               

           Buffer.BlockCopy(message, msgBodyPos, msgBody, 0, msgBodyLen);
        
           long cyclicCount = BitConverter.ToInt64(arrCyclicCount, 0);
           return msgBody;
            
        }
        /*
        public static enmTradingEvent GetMessageHeaderInfo(ref string messageBody, string message, ref string stTime)
        {

           int num =  message.IndexOf("/>");
           string header = message.Substring(0, num+2);
           messageBody = message.Substring(num+2, message.Length - num-2);
           
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(header);
            XmlNode headerNode = xml.SelectSingleNode("header");
            string stType = Convert.ToString(headerNode.Attributes["type"].Value);
            stTime = Convert.ToString(headerNode.Attributes["time"].Value);

            return CUtil.GetEnmByString<enmTradingEvent>(stType);
        }
        */
   
    }
}
