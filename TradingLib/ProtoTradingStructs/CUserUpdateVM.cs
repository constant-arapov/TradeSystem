using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;


namespace TradingLib.ProtoTradingStructs
{
     [ProtoContract]
    public class CUserVMUpdate
    {

         [ProtoMember(1)]
         public CUserVMStockRecord VMStockRecord {get; set;}


    

         [ProtoMember(2)]
         public List<CUserVmInstrumentRecord> ListVM { get; set; }


         public CUserVMUpdate()
         {
             ListVM = new List<CUserVmInstrumentRecord>();


         }


    }

     

    





}
