using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BitfinexWebSockConnector.Enums;


namespace BitfinexWebSockConnector.Messages.Request
{
    class RequestSubsribeTrades : MessageBase
    {
        public RequestSubsribeTrades()
        {
            Event = EnmMessageType.Subscribe;
            Channel = "trades";            

        }

        public string Channel { get; set; }

        public string Symbol { get; set; }

    }
}
