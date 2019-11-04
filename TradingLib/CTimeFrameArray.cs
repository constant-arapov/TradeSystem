using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;


using TradingLib.Data;

namespace TradingLib
{
   public  class CTimeFrameArray : IXMLSerializable
    {
      // public List<CTimeFrameInfo> ListTimeFrameInfo = new List<CTimeFrameInfo>();

       public CListTimeFrameInfo ListTimeFrameInfo = new CListTimeFrameInfo();



       //public System.Threading.Mutex mx = new System.Threading.Mutex();



       public DateTime DtLastWrite = new DateTime(0);


       public CTimeFrameArray() {}

       public string FileName { get; set; }
       public void SelfInit() { }
       public bool NeedSelfInit { get; set; }

    }

   public class CListTimeFrameInfo : List<CTimeFrameInfo>, ICloneable
   {

       public CTimeFrameInfo  GetTFIByDate(DateTime dt)
       {
           foreach (CTimeFrameInfo tfi in this)
           {
               if (tfi.Dt == dt)
                   return tfi;

           }
           return new CTimeFrameInfo();// not found
       }

       public object Clone()
       {


           return MemberwiseClone();

       }


   }



}
