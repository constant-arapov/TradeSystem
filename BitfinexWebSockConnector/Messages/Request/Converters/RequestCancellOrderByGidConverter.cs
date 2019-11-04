using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace BitfinexWebSockConnector.Messages.Request.Converters
{
    class RequestCancellOrderByGidConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            if (!(value is CRequestCancellOrderByGid))
                throw new ApplicationException("Can't serialize RequestCancellOrderByGid)");

            CRequestCancellOrderByGid order = (CRequestCancellOrderByGid)value;


            writer.WriteStartArray();
            writer.WriteValue(0);
            writer.WriteValue("oc_multi");
            writer.WriteValue((object)null);

            writer.WriteStartObject();

            writer.WritePropertyName("gid");
            
            writer.WriteStartArray();
            writer.WriteStartArray();

            writer.WriteValue(order.Gid);
     //       writer.WriteValue(long.MaxValue);//commented 2018-11-19
            writer.WriteEndArray();
            writer.WriteEndArray();

            writer.WriteEndObject();
            writer.WriteEndArray();


        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(RequestCancellOrderByGidConverter);
        }


    }
}
