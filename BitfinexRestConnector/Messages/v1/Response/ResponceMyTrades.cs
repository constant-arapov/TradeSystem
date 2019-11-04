using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitfinexRestConnector.Messages.v1.Response
{
    public class ResponceMyTrades
    {
        public decimal price { get; set; }
        public decimal amount { get; set; }
        public string exchange { get; set; }
        public string type { get; set; }
        public string fee_currency { get; set; }
        public decimal fee_amount { get; set; }
        public Int64 tid { get; set; }
        public Int64 order_id { get; set; }

        public override string ToString()
        {
            string st = String.Format("tid={0} order_id={1} amount={2} price={3} fee_amount={4}",
                                       tid, //0   
                                       order_id,//1
                                       amount,//2
                                       price, //3
                                       fee_amount, //4
                                       fee_currency //5
                                       );

            return st;

        }


    }
}
