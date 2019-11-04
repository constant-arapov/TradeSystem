using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

using BitfinexRestConnector.Messages.v2.Response.Converters;

namespace BitfinexRestConnector.Messages.v2.Response
{
    [JsonConverter(typeof(ConvertResponseOrderHistory))]
    public class ResponseOrdersHistory
    {
        public UInt64 id { get; set; }

        public int gid { get; set; }

        public UInt64 cid { get; set; }

        public string symbol { get; set; }

        public UInt64 mts_create { get; set; }


        public UInt64 mts_update { get; set; }


        public decimal amount { get; set; }


        //public decimal amount { get; set; }



    }
}
