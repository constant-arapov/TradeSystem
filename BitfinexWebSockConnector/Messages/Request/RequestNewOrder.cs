using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

using BitfinexCommon.Enums;

using BitfinexWebSockConnector.Messages.Request.Converters;



namespace BitfinexWebSockConnector.Messages.Request
{

	[JsonConverter(typeof(RequestNewOrderConverterter))]
    public class RequestNewOrder 
    {


		public RequestNewOrder()
		{


		}

        public RequestNewOrder(long gid, long cid, string symbol,EnmBfxOrderType type, decimal amount, decimal price)
        {

            Gid = gid;
            Cid = cid;
            Symbol = symbol;
            Type = type;
            Amount = amount;
            Price = price;
        }

         /// <summary>
        /// (optional) Group id for the order
        /// </summary>
        public long? Gid { get; set; }

        /// <summary>
        /// Must be unique in the day (UTC)
        /// </summary>
        public long Cid { get; set; }

        /// <summary>
        /// symbol (tBTCUSD, tETHUSD, ...)
        /// </summary>
        public string Symbol { get; set; }

        public EnmBfxOrderType Type { get; set; }

        /// <summary>
        /// Positive for buy, Negative for sell
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Price (Not required for market orders)
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The trailing price
        /// </summary>
        public double? PriceTrailing { get; set; }

        /// <summary>
        /// Auxiliary Limit price (for STOP LIMIT)
        /// </summary>
        public double? PriceAuxLimit { get; set; }

        /// <summary>
        /// Whether the order is hidden (1) or not (0)
        /// </summary>
        public int Hidden { get; set; }

        /// <summary>
        /// (optional) Whether the order is postonly (1) or not (0)
        /// </summary>
        public int? Postonly { get; set; }


        public int? Close { get; set; }

    }
}
