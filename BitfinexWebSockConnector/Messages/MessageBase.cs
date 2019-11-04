using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BitfinexWebSockConnector.Enums;

namespace BitfinexWebSockConnector.Messages
{
    public class MessageBase 
    {
        public virtual EnmMessageType Event { get; set; }
    }
}
