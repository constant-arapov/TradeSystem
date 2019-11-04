using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

using BitfinexWebSockConnector.Messages.Request.Converters;

namespace BitfinexWebSockConnector.Messages.Request
{
    [JsonConverter(typeof(RequestCancellOrderConverter))]
    class RequestCancellOrder
    {
        public long Id { get; set; }


    }
}
