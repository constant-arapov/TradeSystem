using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitfinexWebSockConnector.Messages.Response
{
    public class ResponseError : MessageBase
    {
        public string Msg { get; set; }
        public long Code { get; set; }


    }
}
