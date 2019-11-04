using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;


namespace BitfinexWebSockConnector
{
    public static class CBitfinexJsonSerializer
    {
        public static JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None,
            Converters = new List<JsonConverter>() { new StringEnumConverter() { CamelCaseText = true} },
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };



        public static T DeserializeObject<T>(string msg)
        {
            return JsonConvert.DeserializeObject<T>(msg, CBitfinexJsonSerializer.Settings);
        }

        public static string SerializeObject(object msg)
        {
            return JsonConvert.SerializeObject(msg, CBitfinexJsonSerializer.Settings);

        }


       


     
    }
}
