using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;


namespace BitfinexRestConnector.Messages.v1.Request
{
    class GenericRequest
    {
        public string request;
        public string nonce;
        public ArrayList options = new ArrayList();

    }
}
