using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;


using BitfinexWebSockConnector.Messages.Response.Converters;

namespace BitfinexWebSockConnector.Messages.Response
{
    [JsonConverter(typeof(ResponseWalletConverter))]
    public class ResponseWallet
    {
        public string WalletType { get; set; }
        public string Currency { get; set; }
        public double Balance { get; set; }
        public double UnsettledInterest { get; set; }
        public double? BalanceAvailable { get; set; }


        public override string ToString()
        {

            double balAvailable = BalanceAvailable == null ? 0 : (double) BalanceAvailable;

            return String.Format(@"WalletType={0} Currency={1} Balance={2} UnsettledInterest={3} BalanceAvailable={4}",
                                 WalletType,
                                 Currency,
                                 Balance,
                                 UnsettledInterest,
                                 balAvailable);

        }


    }
}
