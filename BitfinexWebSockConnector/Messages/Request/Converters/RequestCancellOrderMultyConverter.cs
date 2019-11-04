using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;


namespace BitfinexWebSockConnector.Messages.Request.Converters
{
    class RequestCancellOrderMultyConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            if (!(value is RequestCancellOrderMulty))
                throw new ApplicationException("Can't serialize order multy exception");

            RequestCancellOrderMulty order = (RequestCancellOrderMulty)value;


            writer.WriteStartArray();
            writer.WriteValue(0);
            writer.WriteValue("oc_multi");
            writer.WriteValue((object)null);

            writer.WriteStartObject();

            writer.WritePropertyName("id");
            //writer.WriteRawValue("{id: "+order.Id+"}");
            writer.WriteValue(order.Id);

            writer.WritePropertyName("gid");
            writer.WriteValue(order.Gid);
           // writer.WriteStartArray();

            //writer.WriteRaw("GID,");
            //writer.WriteStartArray();
           // writer.WriteValue(order.Gid);
            //writer.WriteEndArray();


            //writer.WritePropertyName("cid_max");
            //writer.WriteValue(9828490168);




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
