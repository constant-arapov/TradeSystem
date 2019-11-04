using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.BotEvents
{
    public class BotEventDeal
    {

        public BotEventDeal(long isin_id,string isin, long amount, decimal price)
        {

            Isin_id = isin_id;
            Isin = isin;
            Amount = amount;
            Price = price;



        }


        public long Isin_id;
        public string Isin;
        public long Amount;
        public decimal Price;


    }
}
