using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;


namespace CoinbaseConnector
{


    public class RequestSubscribeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            writer.WriteStartObject();
            writer.WritePropertyName("type");
            writer.WriteValue("subscribe");
            
            writer.WritePropertyName("product_ids");
            writer.WriteStartArray();
            writer.WriteValue("BTC-USD");
           
            writer.WriteEndArray();


           

            writer.WritePropertyName("channels");
           
            writer.WriteStartArray();
            writer.WriteValue("level2");
            writer.WriteValue("ticker");
            writer.WriteEndArray();
            writer.WriteEndObject();
        


            /*
            writer.WriteStartArray();
            writer.WriteValue(0);
            writer.WriteValue("oc_multi");
            writer.WriteValue((object)null);

            writer.WriteStartObject();

            writer.WritePropertyName("gid");

            writer.WriteStartArray();
            writer.WriteStartArray();

           
            //       writer.WriteValue(long.MaxValue);//commented 2018-11-19
            writer.WriteEndArray();
            writer.WriteEndArray();

            writer.WriteEndObject();
            writer.WriteEndArray();
            */



        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(RequestSubscribeConverter);
        }

    }
}
