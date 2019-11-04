using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using BitfinexWebSockConnector.Messages;

namespace BitfinexWebSockConnector.Messages.Response
{
    public class ResponseInfo : MessageBase
    {
        public string Version { get; set; }
       
        public string Code { get; set; }
        public string Msg { get; set; }
        
    }
}
