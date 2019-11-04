using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

//using BitfinexRestConnector.Messages.v1.Response.Converters;

namespace BitfinexRestConnector.Messages.v1.Response
{
    //[JsonConverter(typeof(ConverterResponseSymbolDetails))]
    public class ResponseSymbolDetails
    {
        public string pair { get; set; }
        public int price_precision { get; set; }
        public string initial_margin { get; set; }
        public string minimum_margin { get; set; }
        public string maximum_order_size { get; set; }
        public string minimum_order_size { get; set; }
        public string expiration { get; set; }
        public bool margin { get; set; }

    }
}
