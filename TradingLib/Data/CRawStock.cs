using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;



namespace TradingLib.Data
{
    public class CRawStock
    {
        public  CRawStock() { }
        
        

        public CRawStock(sbyte dir, int isin_id, decimal price, long replID, long replRev, long volume, DateTime moment, long replAct)
        {

            Dir = dir;
            Isin_id = isin_id;
            Price = price;
            ReplID = replID;
            ReplRev = replRev;
            Volume = volume;
            Moment = moment;
            ReplAct = replAct;

        }



     



        public CRawStock(AGGR.orders_aggr oa, int tmp)
        {

         
            Dir = oa.dir;
           
            Isin_id = oa.isin_id;
          
            Price = oa.price;
          
            ReplID = oa.replID;
          
            ReplRev = oa.replRev;
          
            Volume = oa.volume;
          
            Moment = oa.moment;
          
            ReplAct = oa.replAct;

          

        }


        public void Update (AGGR.orders_aggr oa)
        {

            Dir = oa.dir;
            Isin_id = oa.isin_id;
            Price = oa.price;
            ReplID = oa.replID;
            ReplRev = oa.replRev;
            Volume = oa.volume;
            Moment = oa.moment;
            ReplAct = oa.replAct;

        }

        public void Update(CRawStock rs)
        {
            Dir = rs.Dir;
            Isin_id = rs.Isin_id;
            Moment = rs.Moment;
            Price = rs.Price;
            ReplAct = rs.ReplAct;
            ReplID = rs.ReplID;
            ReplRev = rs.ReplRev;
            Volume = rs.Volume;

        }
        public sbyte Dir { get; set; }
        public int Isin_id { get; set; }
        public decimal Price { get; set; }
        public long ReplID { get; set; }
        public long ReplRev { get; set; }
        public long Volume { get; set; }
        public DateTime Moment { get; set; }
        public long ReplAct { get; set; }

    }
}
