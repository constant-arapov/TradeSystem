using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;



namespace TradingLib.ProtoTradingStructs
{

    [ProtoContract]
    public class CUserPosLog 
    {
        [ProtoMember(1)]
        public decimal CloseAmount { get; set; }

        [ProtoMember(2)]
        public decimal PriceOpen { get; set; }

        [ProtoMember(3)]
        public decimal PriceClose { get; set; }

        [ProtoMember(4)]
        public string BuySell { get; set; }

        [ProtoMember(5)]
        public DateTime DtOpen { get; set; }

        [ProtoMember(6)]
        public DateTime DtClose { get; set; }

        [ProtoMember(7)]
        public decimal VMClosed_Points { get; set; }


        [ProtoMember(8)]
        public decimal VMClosed_Steps { get; set; }


        [ProtoMember(9)]
        public decimal VMClosed_RUB_clean { get; set; }


        [ProtoMember(10)]
        public decimal VMClosed_RUB { get; set; }

        [ProtoMember(11)]
        public decimal  VMClosed_RUB_user {get; set; }


        [ProtoMember(12)]
        public decimal Fee { get; set; }


        [ProtoMember(13)]
        public decimal Fee_Stock { get; set; }

        [ProtoMember(14)]
        public decimal Fee_Profit { get; set; }

        [ProtoMember(15)]
        public decimal Fee_Total { get; set; }

        [ProtoMember(16)]
        public string PriceOpenSt { get; set; }

        [ProtoMember(17)]
        public string PriceCloseSt { get; set; }


        [ProtoMember(18)]
        public string CloseAmountSt { get; set; }


    }

    
}
