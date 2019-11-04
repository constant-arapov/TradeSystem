using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.ProtoTradingStructs
{

    public class CAggrDealStruct
    {

        public CDealClass DealClass { get; set; }
        public int NumDeals { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }

        public DateTime DtBegin { get; set; }
        public DateTime DtEnd { get; set; }

        public decimal Sum { get; set; }
        //public long AmountTotal { get; set; }

        public CAggrDealStruct()
        {
            DealClass = new CDealClass();
            NumDeals = 0;
            MaxPrice = 0;

            DtBegin = new DateTime(0);
            DtEnd = new DateTime(0);
            Sum = 0;

        }


    }
}
