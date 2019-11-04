using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using BitfinexWebSockConnector.Messages.Response;

namespace BitfinexWebSockConnector.Messages.Response.Converters
{
    public class ResponseWalletConverter :  JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ResponseWallet);
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
         JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            return JArrayToTradingTicker(array);
        }

        private ResponseWallet JArrayToTradingTicker(JArray array)
        {
            return new ResponseWallet
            {
                WalletType = (string)array[0],
                Currency = (string)array[1],
                Balance = (double)array[2],
                UnsettledInterest = (double)array[3],
                BalanceAvailable = (double?)array[4]
            };
        }


    }


}
