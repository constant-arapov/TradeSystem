using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Net;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using Common.Interfaces;

using BitfinexRestConnector.Messages.v1.Request;
using BitfinexRestConnector.Messages.v1.Response;


namespace BitfinexRestConnector
{
    public class CBitfinexRestConnectorV1 : CBaseBitfinexRestConnector
    {
        private static string MSgNoResp = "";


        public CBitfinexRestConnectorV1(IAlarmable client, string key, string secret) : base (client, key,secret)
        {

        }


        /*     public BalancesResponse GetBalances()
      {
          BalancesRequest req = new BalancesRequest(Nonce);
          string response = SendRequest(req,"GET");
          BalancesResponse resp = BalancesResponse.FromJSON(response);

          return resp;
      }
      public CancelOrderResponse CancelOrder(int order_id)
      {
          CancelOrderRequest req = new CancelOrderRequest(Nonce, order_id);
          string response = SendRequest(req,"POST");
          CancelOrderResponse resp = CancelOrderResponse.FromJSON(response);
          return resp;
      }
      public CancelAllOrdersResponse CancelAllOrders()
      {
          CancelAllOrdersRequest req = new CancelAllOrdersRequest(Nonce);
          string response = SendRequest(req,"GET");
          return new CancelAllOrdersResponse(response);
      }
      public OrderStatusResponse GetOrderStatus(int order_id)
      {
          OrderStatusRequest req = new OrderStatusRequest(Nonce, order_id);
          string response = SendRequest(req, "POST");
          return OrderStatusResponse.FromJSON(response);
      }*/
        public ResponseActiveOrders GetActiveOrders()
        {
            RequestActiveOrders req = new RequestActiveOrders(Nonce);
            string response = SendRequest(req, "POST");
            return ResponseActiveOrders.FromJSON(response);
        }
        /*public ActivePositionsResponse GetActivePositions()
        {
            ActivePositionsRequest req = new ActivePositionsRequest(Nonce);
            string response = SendRequest(req, "POST");
            return ActivePositionsResponse.FromJSON(response);
        }

        public NewOrderResponse ExecuteBuyOrderBTC(decimal amount, decimal price, OrderExchange exchange, OrderType type)
        {
            return ExecuteOrder(OrderSymbol.BTCUSD, amount, price, exchange, OrderSide.Buy, type);
        }
        public NewOrderResponse ExecuteSellOrderBTC(decimal amount, decimal price, OrderExchange exchange, OrderType type)
        {
            return ExecuteOrder(OrderSymbol.BTCUSD, amount, price, exchange, OrderSide.Sell, type);
        }
        public NewOrderResponse ExecuteOrder(OrderSymbol symbol, decimal amount, decimal price, OrderExchange exchange, OrderSide side, OrderType type)
        {
            NewOrderRequest req = new NewOrderRequest(Nonce, symbol, amount, price, exchange, side, type);
            string response = SendRequest(req,"POST");
            NewOrderResponse resp = NewOrderResponse.FromJSON(response);
            return resp;
        }
        */

        public ResponseSymbolDetails[] GetSymbolDetails()
        {
            RequestSymbolDetails sd = new RequestSymbolDetails(Nonce);
            string resp = SendRequest(sd, "GET");

           
            ResponseSymbolDetails[] respSymbDet = JsonConvert.DeserializeObject<ResponseSymbolDetails[]>(resp);

            return respSymbDet;


        }

        public ResponceMyTrades[] GetMyTrades(string instrument)
        {
            ResponceMyTrades[] respMyTrades = null;
            RequestMyTrades mt = new RequestMyTrades(Nonce, instrument);
            string resp = SendRequest(mt, "GET");
            if (resp.Contains("price"))//simple validation
                respMyTrades = JsonConvert.DeserializeObject<ResponceMyTrades[]>(resp);
            return respMyTrades;

        }




        private string SendRequest(GenericRequest request, string httpMethod)
        {
            int parMaxAttemptTrials = 100;
            int parSleepTime = 10000;

            string response = null;
            int i;
            for (i = 0; i < parMaxAttemptTrials; i++)
            {
                Log("===> " + request);

                string json = JsonConvert.SerializeObject(request);
                string json64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
                byte[] data = Encoding.UTF8.GetBytes(json64);
                byte[] hash = hashMaker.ComputeHash(data);
                string signature = GetHexString(hash);

                HttpWebRequest wr = WebRequest.Create("https://api.bitfinex.com" + request.request) as HttpWebRequest;
                wr.Headers.Add("X-BFX-APIKEY", Key);
                wr.Headers.Add("X-BFX-PAYLOAD", json64);
                wr.Headers.Add("X-BFX-SIGNATURE", signature);
                wr.Method = httpMethod;

                StreamReader sr =null;
                try
                {
                    HttpWebResponse resp = wr.GetResponse() as HttpWebResponse;
                    sr = new StreamReader(resp.GetResponseStream());
                    response = sr.ReadToEnd();
                    sr.Close();
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        sr = new StreamReader(ex.Response.GetResponseStream());
                        if (sr!=null)
                            response = sr.ReadToEnd();
                    }
                    //Error("WebException",ex);
                    Log("WebException" + ex.Message);

                    //throw new BitfinexException(ex, response);
                    //TODO error
                }
                finally
                {
                    if (sr != null)
                        sr.Close();
                }

                response = (response == null || response=="") ? MSgNoResp : response;
                Log("<=== " + response);

                if (!response.Contains ("ERR_RATE_LIMIT"))
                    break;
                else 
                   Thread.Sleep(parSleepTime);
               
            }

            if (i == parMaxAttemptTrials)
                throw new ApplicationException("CBitfinexRestConnectorV1. Unable to get response");

            if (response == MSgNoResp)
                throw new ApplicationException("CBitfinexRestConnectorV1. No response");


            return response;
        }

        public void DbgPrintMyTrades(string instrument,ResponceMyTrades[] rmt)
        {
            Log(String.Format("BEGIN LAST TRADES FOR {0}================================",instrument));
            for (int i = 0; i < rmt.Length; i++)           
                Log(rmt[i].ToString());
            Log(String.Format("END LAST TRADES FOR {0}=================================", instrument));
        }



    }
}
