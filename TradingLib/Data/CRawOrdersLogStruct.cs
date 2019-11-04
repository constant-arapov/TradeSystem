using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Data
{
   public  class CRawOrdersLogStruct
    {

       public CRawOrdersLogStruct()
       {


       }


       public CRawOrdersLogStruct(FUTTRADE.orders_log ol)
       {

           ReplRev = ol.replRev;
           ReplId = ol.replID;
           Moment = ol.moment;
           Isin_Id = ol.isin_id;
           Id_ord = ol.id_ord;
           Sess_id = ol.sess_id;
           Price = ol.price;
           Dir = ol.dir;
           Action = ol.action;       
           Amount = ol.amount;
           Amount_rest = ol.amount_rest;
           Id_deal = ol.id_deal;
           Deal_Price = ol.deal_price;
           Ext_id = ol.ext_id;



        }

       

      

       
        public long ReplRev;
        public long ReplId;
        public DateTime Moment;
        public long Isin_Id;
        public long Id_ord;
        public int Sess_id;
        public decimal Price;
        public sbyte Dir;
        public sbyte Action;
        public string ActionString;
        public decimal Amount;
        public int Amount_rest;
        public long Id_deal;
        public decimal Deal_Price;
        public int Ext_id;

        public string Instrument;


    }





}
