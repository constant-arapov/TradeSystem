using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;


namespace TradingLib.ProtoTradingStructs
{
     [ProtoContract]
    public class CSubscribeTicker
    {
         [ProtoMember(1)]

         public List <CCommandSubscribeTickers> ListSubscribeCommands { get; set; }


         public CSubscribeTicker()
         {

             ListSubscribeCommands = new List<CCommandSubscribeTickers>();

         }


    }

     [ProtoContract]
     public class CCommandSubscribeTickers
     {
         [ProtoMember(1)]
         public string Ticker { get; set; }


         [ProtoMember(2)]
         public EnmSubsrcibeActions Action { get; set; }

     }


     public enum EnmSubsrcibeActions
     {
         Subscribe,
         UnSubscribe


     }



}
