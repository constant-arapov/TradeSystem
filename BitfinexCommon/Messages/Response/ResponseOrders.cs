using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

using BitfinexCommon.Enums;


using BitfinexCommon.Messages.Response.Converters;

namespace BitfinexCommon.Messages.Response
{
    [JsonConverter(typeof(ResponseOrdersConverter))]
    public class ResponseOrders
    {
        public long Id { get; set; }
        public long? Gid { get; set; }
        public long Cid { get; set; }
        public string Symbol { get; set; }
        public long? MtsCreate { get; set; }
        public long? MtsUpdate { get; set; }
        public double? Amount { get; set; }
        public double? AmountOrig { get; set; }
        public EnmBfxOrderType Type { get; set; }
        public EnmBfxOrderType TypePrev { get; set; }
        public int? Flags { get; set; }
        public string OrderStatus { get; set; }
        public double? Price { get; set; }
        public double? PriceAvg { get; set; }
        public double? PriceTrailing { get; set; }
        public double? PriceAuxLimit { get; set; }
        public int? Notify { get; set; }
        public int? Hidden { get; set; }
        public int? PlacedId { get; set; }

        public override string ToString()
        {
           return String.Format("Gid={0} Symbol={1} id={2} type={3} price={4} amount={5} amount_orig={6} statusRow={7} statusParsed={8}",
                                                        this.Gid, //0
                                                        this.Symbol, //1
                                                        this.Id,  //2
                                                        this.Type, //3
                                                        this.Price, //4
                                                        this.Amount,   //5
                                                        this.AmountOrig, //6
                                                        this.OrderStatus,//7
                                                        CBfxUtils.GetOrderStatus(OrderStatus));//8

        }
      

    }
}
