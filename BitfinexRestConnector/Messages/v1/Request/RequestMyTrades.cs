using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitfinexRestConnector.Messages.v1.Request
{
    class RequestMyTrades : GenericRequest
    {

        public string symbol { get; set; }

        public RequestMyTrades(string nonce, string symbolRquested)
        {
            this.nonce = nonce;
            request = "/v1/mytrades";
            symbol = symbolRquested;
        }


    }
}
