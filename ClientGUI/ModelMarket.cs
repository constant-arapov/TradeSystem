using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common;
using Common.Interfaces;

using TradingLib;
using TradingLib.ProtoTradingStructs;


using Messenger;


namespace ClientGUI
{
    public class CModelMarket : ITCPClientUser
    {

        private CStockClass _outpStockClass = new CStockClass();
        public CStockClass OutpStockClass
        {
            get
            {
                return _outpStockClass;
            }
        }

        public void Error(string msh, Exception e = null)
        {

        }


        public CModelMarket()
        {
          //  OutpStockClass = new CStockClass();

        }

        bool bNeedSynchroTime = true;

        public void CallbackReadMessage(byte[] message)
        {

            string messageBody = "";
            string stTime = "";

            //enmTradingEvent te = CMessenger.GetMessageHeaderInfo(ref messageBody, message, ref stTime);
            if (bNeedSynchroTime)
            {
                DateTime tm =    Convert.ToDateTime(stTime);

            }


            //if (te == enmTradingEvent.StockUpadate)
            {
                CStockClass sc = (CStockClass)CUtil.DeSerializeFromXMLString(messageBody, typeof(CStockClass));               
                //tempo do normal
                if (sc.Isin == "RTS-3.16")
                    sc.Copy(sc.Isin,  _outpStockClass);
            }

           
            //  CStockClass sc = (CStockClass) CUtil.DeSerializeFromXMLString(message, typeof(CStockClass));
            //CStockClass sc = 

        }



    }
}
