using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;




namespace TradingLib.Data
{
    public class CInstrument  
    {

        public CInstrument()
        {



        }



        public CInstrument(FUTINFO.fut_instruments fbr)
        {

            Isin_id = fbr.isin_id;
            Min_step = fbr.min_step;
            Step_price = fbr.step_price;
            D_exp = fbr.d_exp;
            RoundTo = fbr.roundto;


        }

        public  long Isin_id;
        public decimal Min_step;
        public  decimal Step_price;
        public string PriceFormat;
        public int RoundTo;

        public long LotSize;
        public int IsShortable;
        public decimal minimum_order_size;
        public int DecimalVolume;

        DateTime D_exp;
    }
}
