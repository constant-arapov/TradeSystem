using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;
using TradingLib.Enums;


namespace TradingLib.ProtoTradingStructs
{


    [ProtoContract]
    public class CUserDeal 
    {
        public CUserDeal()
        {

        }


        [ProtoMember(1)]
        public long ReplId { get; set; }
                
        [ProtoMember(2)]
        public decimal Amount { get; set; }

        [ProtoMember(3)]
        public decimal Price { get; set; }


        [ProtoMember(4)]
        public EnmDealDir BuySell { get; set; }

        [ProtoMember(5)]
        public DateTime Moment { get; set; }

        [ProtoMember(6)]
        public decimal Fee { get; set; }


        [ProtoMember(7)]
        //public decimal Decimals { get; set; }
        public string PriceSt { get; set; }

        //added 2018-06-13
        public long DealId { get; set; }

        public decimal FeeDealing { get; set; }
        //2018-08-05
        public decimal Fee_Stock { get; set; }

    }
}
