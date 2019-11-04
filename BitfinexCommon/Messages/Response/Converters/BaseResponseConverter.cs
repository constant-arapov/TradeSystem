using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using BitfinexCommon.Helpers;


namespace BitfinexCommon.Messages.Response.Converters
{
    public abstract class BaseResponseConverter <T> : JsonConverter
    {

        protected abstract T JArrayToTradingTicker(JArray array);
        
        

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
       JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            return JArrayToTradingTicker(array);
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        protected double ConvDbl(JToken jtoken)
        {
            return CJConv.ConvDbl(jtoken);
        }

        protected int ConvInt(JToken jtoken)
        {
            return CJConv.ConvInt(jtoken);
        }

        protected long ConvLong(JToken jtoken)
        {
            return CJConv.ConvLong(jtoken);
        }



    }
}
