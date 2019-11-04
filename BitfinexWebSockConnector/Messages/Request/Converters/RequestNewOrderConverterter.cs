using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Newtonsoft.Json;

using BitfinexCommon;

using BitfinexCommon.Messages.Response.Converters;

namespace BitfinexWebSockConnector.Messages.Request.Converters
{
    class RequestNewOrderConverterter : JsonConverter
    {

	

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is RequestNewOrder))
                throw new ApplicationException("Can't serialize order");


            RequestNewOrder order =  (RequestNewOrder) value;

            writer.WriteStartArray();
            writer.WriteValue(0);
            writer.WriteValue("on");
            writer.WriteValue((object)null);

            writer.WriteStartObject();

            if (order.Gid.HasValue)
            {
                writer.WritePropertyName("gid");
                writer.WriteValue(order.Gid.Value);
            }

            writer.WritePropertyName("cid");
            writer.WriteValue(order.Cid);

            writer.WritePropertyName("type");
            //TODO remove from OrderConverter !
            writer.WriteValue(CBfxUtils.GetOrderTypeString(order.Type));
           
            writer.WritePropertyName("symbol");
            writer.WriteValue(order.Symbol);

            writer.WritePropertyName("amount");
            writer.WriteValue(order.Amount.ToString(CultureInfo.InvariantCulture));

          
            writer.WritePropertyName("price");
             writer.WriteValue(order.Price.ToString(CultureInfo.InvariantCulture));
       

            if (order.PriceTrailing.HasValue)
            {
                writer.WritePropertyName("price_trailing");
                writer.WriteValue(order.PriceTrailing.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (order.PriceAuxLimit.HasValue)
            {
                writer.WritePropertyName("price_aux_limit");
                writer.WriteValue(order.PriceAuxLimit.Value.ToString(CultureInfo.InvariantCulture));
            }

            writer.WritePropertyName("hidden");
            writer.WriteValue(order.Hidden);

            if (order.Postonly.HasValue)
            {
                writer.WritePropertyName("postonly");
                writer.WriteValue(order.Postonly.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (order.Close.HasValue)
            {
                writer.WritePropertyName("Close");
                writer.WriteValue(order.Close.Value);


            }


            writer.WriteEndObject();
            writer.WriteEndArray();
			
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }


        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(RequestNewOrder);
		
        }

    }
}
