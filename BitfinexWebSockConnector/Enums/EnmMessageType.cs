using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitfinexWebSockConnector.Enums
{
    public enum EnmMessageType
    {
        Info,
        Auth,
        Error,
        Ping,
        Pong,
        Conf,
        Subscribe,
        Subscribed,
        Unsubscribe,
        Unsubscribed
    }
}
