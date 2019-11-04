using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BitfinexCommon.Helpers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BitfinexCommon.Messages.Response.Converters
{
    class ResponseTradesConverter : BaseResponseConverter<ResponseTrades>
    {
        



        protected override ResponseTrades JArrayToTradingTicker(JArray array)
        {
            ResponseTrades rt = new ResponseTrades();

            rt.Id = (long) array[0];
            rt.Pair = (string) array[1];
            rt.MtsCreate = (long)array[2];
            rt.OrderId = (long)array[3];
            rt.ExecAmount = (double)array[4];
            rt.ExecPrice = (double)array[5];
            rt.OrderType = (string)array[6];
            rt.OrderPrice = ConvDbl(array[7]);
            rt.Maker =  ConvInt(array[8]);
            //for "tu"
            if (array.Count > 9)
            {
                  rt.Fee = ConvDbl(array[9]);
                  rt.FeeCurrency = (string)array[10];

            }
            return rt;
        }

      




    }
}
