using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

using Common;



namespace TradingLib.ProtoTradingStructs
{
    [ProtoContract]
    public class COrder : CClone
    {

      
        
        [ProtoMember(1)]
        public sbyte Action { get; set; }

        [ProtoMember(2)]
        public sbyte Dir { get; set; }

        [ProtoMember(3)]
        public string Isin { get; set; }
        
        [ProtoMember(4)]
        public decimal Price { get; set; }
        
        [ProtoMember(5)]
        public decimal Amount { get; set; }

        [ProtoMember(6)]
        public DateTime Moment { get; set; }

        [ProtoMember(7)]
        public int UserId {get;set;}


        [ProtoMember(8)]
        public int OrderId { get; set; }



        public COrder()
        {


        }

        



    }
}
