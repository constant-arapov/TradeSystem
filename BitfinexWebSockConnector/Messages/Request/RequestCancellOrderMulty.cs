using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Newtonsoft.Json;

using BitfinexWebSockConnector.Messages.Request.Converters;

namespace BitfinexWebSockConnector.Messages.Request
{
    [JsonConverter(typeof(RequestCancellOrderMultyConverter))]
    class RequestCancellOrderMulty
    {
        public long Id { get; set; }
        public long Cid { get; set; }
        public string Cid_date { get; set; }
        public int Gid { get; set; }
        public int Id_Max { get; set; }

    }
}
