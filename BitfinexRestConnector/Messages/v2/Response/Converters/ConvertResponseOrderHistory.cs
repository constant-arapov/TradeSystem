using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using BitfinexRestConnector.Messages.v2.Response;

namespace BitfinexRestConnector.Messages.v2.Response.Converters
{
    public class ConvertResponseOrderHistory : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ResponseOrdersHistory);
        }



        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
           JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            return JArrayToTradingTicker(array);
        }



        private ResponseOrdersHistory JArrayToTradingTicker(JArray array)
        {
            ResponseOrdersHistory oh = new ResponseOrdersHistory();

            oh.id = (UInt64)array[0];
            oh.gid = (int)array[1];
            oh.cid = (UInt64)array[2];
            oh.symbol = (string)array[3];

            oh.mts_create = (UInt64)array[4];
            oh.mts_update = (UInt64)array[5];

          
            return oh;


        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public bool CanWrite = false;


    }
}
