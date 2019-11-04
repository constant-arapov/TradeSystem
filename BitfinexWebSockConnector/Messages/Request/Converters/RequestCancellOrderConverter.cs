using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;


namespace BitfinexWebSockConnector.Messages.Request.Converters
{
    class RequestCancellOrderConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

             if (!(value is RequestCancellOrder))
                throw new ApplicationException("Can't serialize order exception");

             RequestCancellOrder order = (RequestCancellOrder)value;


            writer.WriteStartArray();
            writer.WriteValue(0);
            writer.WriteValue("oc");
            writer.WriteValue((object)null);

            writer.WriteStartObject();

            writer.WritePropertyName("id");
            writer.WriteValue(order.Id);

            writer.WriteEndObject();
            writer.WriteEndArray();


        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(RequestCancellOrder);
        }


    }
}
