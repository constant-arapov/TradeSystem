using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using TradingLib.Enums;

namespace TradingLib.Data
{
    public class CRawUserDeal
    {



        //for testing
        public CRawUserDeal()
        {



        }


        public CRawUserDeal(USER_DEAL.user_deal deal, string instrument)
        {
         
            ReplId = deal.replID;
            ReplRev =  deal.replRev;


            if (deal.status_buy == 1)
                Dir = (sbyte) OrderDirection.Buy;
            else if (deal.status_sell == 1)
                Dir = (sbyte) OrderDirection.Sell;


            Price= deal.price;
            
            Amount = deal.amount;
            Ext_id_buy = deal.ext_id_buy;
            Ext_id_sell = deal.ext_id_sell;
            Moment = deal.moment;
            //Isin_Id = deal.isin_id;
            Instrument = instrument;


            Id_ord_buy = deal.id_ord_buy;
            Id_ord_sell = deal.id_ord_sell;

            Session_id = deal.sess_id;
            Id_Deal = deal.id_deal;

            Fee_buy = deal.fee_buy;
            Fee_sell = deal.fee_sell;

           


        }

        public long ReplId;
        public long ReplRev;
        public decimal Price;
        public decimal Amount;
        public sbyte Dir;
        public DateTime Moment;
        public long Id_Deal;
        //public long Isin_Id;
        public long Id_ord_buy;
        public long Id_ord_sell;

        public long Ext_id_buy;


        public long Ext_id_sell;

        public int Session_id;

        public decimal Fee_buy;
        public decimal Fee_sell;

        public decimal Fee_Dealing; //2018-07-16

        public decimal FeeStock;

        public string Instrument;


      
    }
}
