using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitfinexWebSockConnector.Messages.Response
{
    public class ResponseSubscribed : MessageBase
    {
        public string Channel { get; set; }
        public long ChanId {get;set;}
        public string Symbol { get; set; }
         




    }
}
