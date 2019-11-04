using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BitfinexWebSockConnector.Enums;

namespace BitfinexWebSockConnector.Messages.Request
{
    public class RequestPing : MessageBase
    {

        public RequestPing()
        {
            Event = EnmMessageType.Ping;

        }


        public int Cid { get; set; }

    }
}
