using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitfinexRestConnector.Messages.v1.Request
{
    class RequestActiveOrders : GenericRequest
    {
        public RequestActiveOrders(string nonce)
        {
            this.nonce = nonce;
            this.request = "/v1/orders";
        }
    }
}
