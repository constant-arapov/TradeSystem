using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common;
using TradingLib.Enums;



namespace TradingLib.Data
{
    public class CRawDeal : CClone
    {
        public CRawDeal()
        {
        }

        public CRawDeal(DEALS.deal d)

        {

            ReplID = d.replID;
            ReplRev = d.replRev;
            ReplAct = d.replAct;
            Isin_id = d.isin_id;
            Id_deal = d.id_deal;
            Amount = d.amount;
            Price = d.price;
            Id_ord_buy = d.id_ord_buy;
            Id_ord_sell = d.id_ord_sell;
            Moment = d.moment;
            Pos = d.pos;

        }


        
        public CRawDeal(AstsCCTrade.ALL_TRADES at)
        {

            ReplID = at.replID;
            ReplRev = at.replRev;
            Amount = at.QUANTITY;
            Price = at.PRICE;
            Moment = at.TRADETIME;

            //hack for simulate direction

            if (at.BUYSELL == "B")
                Id_ord_buy = 1;
            else
                Id_ord_sell = 1;


            int milis = at.MICROSECONDS / 1000;
            Moment = Moment.AddMilliseconds(milis);

        }
        
        public CRawDeal (CTimeFrameInfo tfi)
        {
            Moment = tfi.LastUpdate;
            ReplID = tfi.LastReplId;
            Price = tfi.ClosePrice;
            IsSurrogate = true;

        }

        //for ASTS connector
        public CRawDeal(long amount, decimal price, string buySell, DateTime moment)
        {

            Amount = amount;
            Price = price;

            if (buySell == "B")
                Id_ord_buy = 1;
            else
                Id_ord_sell = 1;

            Moment = moment;

        }




        public EnmDealDir GetDealDir()
        {
            if (Id_ord_buy > Id_ord_sell)
                return EnmDealDir.Buy;
            else if (Id_ord_buy < Id_ord_sell)
                return EnmDealDir.Sell;



            return EnmDealDir.Unknow;
        }
        public long ReplID;
        public long ReplRev;
        public long ReplAct;
        public long Isin_id;
        public long Id_deal;
        public long Amount;
        public decimal Price;
        public long Id_ord_buy;
        public long Id_ord_sell;
        public DateTime Moment;
        public int Pos;
        public bool IsSurrogate= false;
        
    }
}
