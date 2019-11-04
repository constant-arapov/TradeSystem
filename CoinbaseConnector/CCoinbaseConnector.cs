using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CoinbaseConnector
{
    public class CCoinbaseConnector
    {

        private List<Tuple<string, string>> _lst = new List<Tuple<string, string>>();

        CDBLayer _dbLayer = new CDBLayer();

        public void Connect()
        {

           

            WebSocket webSocket = new WebSocket("wss://ws-feed.pro.coinbase.com");
            webSocket.OnOpen += WebSocket_OnOpen;
            webSocket.OnMessage += WebSocket_OnMessage;
            webSocket.OnError += WebSocket_OnError;
            webSocket.OnClose += WebSocket_OnClose;


            RequestSubscribe req = new RequestSubscribe();
            var serRecCancByGid = JsonConvert.SerializeObject(req, CCoinbaseJsonSerializer.Settings);

            

            webSocket.Connect();

            System.Threading.Thread.Sleep(3000);
            webSocket.Send(serRecCancByGid);

            Console.ReadLine();
        }


        
        

      


        private void WebSocket_OnClose(object sender, CloseEventArgs e)
        {
            Console.WriteLine("OnOnClose");
        }

        private void WebSocket_OnError(object sender, ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void WebSocket_OnMessage(object sender, MessageEventArgs e)
        {
            //Console.WriteLine("OnMessage");

            string dt =
                String.Format("{0}:{1}:{2}.{3} ",
                DateTime.Now.Hour.ToString("D2"),
                DateTime.Now.Minute.ToString("D2"),
                DateTime.Now.Second.ToString("D2"),
                DateTime.Now.Millisecond.ToString("D3"));



            //string value = "";
            JToken jtypeToken, jBestBidToken, jBestAskToken;
            decimal bestBid =0, bestAsk=0;

            JObject jObj =  JsonConvert.DeserializeObject<JObject>(e.Data, CCoinbaseJsonSerializer.Settings);

            if (jObj.TryGetValue("type", out jtypeToken))
            {
                string stToken = jtypeToken.ToString();
                if (stToken == "snapshot")
                {
                    System.Threading.Thread.Sleep(0);
                }
                else if (stToken == "ticker")
                {
                    if (jObj.TryGetValue("best_bid", out jBestBidToken))                    
                        bestBid = 1000m/Convert.ToDecimal(jBestBidToken);

                    if (jObj.TryGetValue("best_ask", out jBestAskToken))
                        bestAsk = 1000m/Convert.ToDecimal(jBestAskToken);

                    _dbLayer.UpdateBestBidAsk(bestBid, bestAsk);

                }
           

            }


            _lst.Add(new Tuple<string, string>(dt, e.Data));

            if (_lst.Count > 500)
                System.Threading.Thread.Sleep(0);


        }

        private void WebSocket_OnOpen(object sender, EventArgs e)
        {
            Console.WriteLine("OnOpen");
        }

        


    }
}
