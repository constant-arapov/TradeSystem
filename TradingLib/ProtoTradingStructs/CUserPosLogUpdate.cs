using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;


namespace TradingLib.ProtoTradingStructs
{
    [ProtoContract]
    public class CUserPosLogUpdate : IDataLogUpdateCommand<CUserPosLog>
    
    {
       [ProtoMember(1)]
       public Dictionary<string, List<CUserPosLog>> DictLog { get; set; }


       public CUserPosLogUpdate()
       {
           DictLog = new Dictionary<string, List<CUserPosLog>>();

       }



       public void Sort()
       {
           foreach (var kvp in DictLog)
               kvp.Value.Sort(SortComparator);

       }


       public static int SortComparator(CUserPosLog ud1, CUserPosLog ud2)
       {

           DateTime dt1 = ud1.DtOpen;
           DateTime dt2 = ud2.DtOpen;

           if (dt1 == dt2)
               return 0;
           else if (dt1 > dt2)
               return -1;
           else
               return 1;


       }    



    }
}
