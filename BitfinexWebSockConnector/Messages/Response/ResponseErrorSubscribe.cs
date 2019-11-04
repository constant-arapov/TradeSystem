using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitfinexWebSockConnector.Messages.Response
{
    class ResponseErrorSubscribe : ResponseError
    {
        public string Symbol { get; set; }


    }
}
