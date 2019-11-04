using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

using TradingLib;



namespace TradingLib.ProtoTradingStructs
{
     [ProtoContract]
    public class CUserDealsLogUpdate  : IDataLogUpdateCommand <CUserDeal>
    {
         [ProtoMember(1)]
         public Dictionary<string, List<CUserDeal>> DictLog{ get; set; }

         


         public CUserDealsLogUpdate()
         {

             DictLog = new Dictionary<string, List<CUserDeal>>();

         }

         public void Sort()
         {
             foreach (var kvp in DictLog)
                kvp.Value.Sort(SortComparator);
             
         }


         public static int SortComparator(CUserDeal ud1, CUserDeal ud2)
         {
            
                 DateTime dt1 = ud1.Moment;
                 DateTime dt2 = ud2.Moment;

             if (dt1==dt2)
                 return 0;
             else if (dt1 > dt2)
                 return -1;
             else 
                 return 1;


         }    
    }





}
