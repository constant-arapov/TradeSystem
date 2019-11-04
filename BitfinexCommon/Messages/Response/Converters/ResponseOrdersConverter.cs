using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using BitfinexCommon;
using BitfinexCommon.Enums;

using BitfinexCommon.Helpers;


namespace BitfinexCommon.Messages.Response.Converters
{

    public class ResponseOrdersConverter : BaseResponseConverter<ResponseOrders>
    {
       
        protected override ResponseOrders JArrayToTradingTicker(JArray array)
        {
            ResponseOrders ro = new ResponseOrders();
            
                ro.Id = ConvLong(array[0]);
                ro.Gid = (long?)array[1];
                ro.Cid =  ConvLong((long)array[2]);
                ro.Symbol = (string)array[3];
                ro.MtsCreate = (long?)array[4];
                ro.MtsUpdate = (long?)array[5];
                ro.Amount =  ConvDbl(array[6]);
                ro.AmountOrig =   (double?)array[7];
                ro.Type = CBfxUtils.GetOrderType((string)array[8]);
                ro.TypePrev = CBfxUtils.GetOrderType((string)array[9]);
                // 10
                // 11
                ro.Flags = (int?)array[12];
                ro.OrderStatus = (string)array[13];
                // 14
                // 15
                ro.Price = (double?)array[16];
                ro.PriceAvg = (double?)array[17];
                ro.PriceTrailing = (double?)array[18];
                ro.PriceAuxLimit = (double?)array[19];
                // 20
                // 21
                // 22
                ro.Notify = (int?)array[23];
                ro.Hidden = (int?)array[24];
				ro.PlacedId = (int?)array[25];
            
			return ro;


        }

      

     

     
    } 
}
