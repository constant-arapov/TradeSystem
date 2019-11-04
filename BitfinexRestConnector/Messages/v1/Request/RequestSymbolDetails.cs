using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitfinexRestConnector.Messages.v1.Request
{
    class RequestSymbolDetails : GenericRequest
    {
        public RequestSymbolDetails(string nonce)
        {
            this.nonce = nonce;
            this.request = "/v1/symbols_details";

        }


    }
}
