using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;


namespace BitfinexRestConnector.Messages.v1.Response
{
    public class ResponseActiveOrders
    {
        public List<ResponseOrderStatus> orders;

        public static ResponseActiveOrders FromJSON(string response)
        {
            List<ResponseOrderStatus> orders = JsonConvert.DeserializeObject<List<ResponseOrderStatus>>(response);
            return new ResponseActiveOrders(orders);
        }
        private ResponseActiveOrders(List<ResponseOrderStatus> orders)
        {
            this.orders = orders;
        }
    }
}
