using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitfinexRestConnector.Messages.v2.Request
{
    public class RequestOrderHistory
    {
        public RequestOrderHistory()
        {



            DateTime epoch = new DateTime(1970, 1, 1);
            Int64 now = (Int64)(DateTime.UtcNow - epoch).TotalSeconds * 1000;
            Int64 depth = (Int64)60 * 24 * 60 * 60 * 1000;

            //end = now - (Int64)1 * 5 * 60 * 60 * 1000;
            end = now;
            start = end - depth;
            limit = 25;

        }

       


        public long start;
        public long end;
        public long limit;


    }
}
