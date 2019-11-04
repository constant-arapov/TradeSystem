using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Common.Interfaces;

using BitfinexCommon.Messages.Response;


using BitfinexRestConnector.Messages.v2.Response;
using BitfinexRestConnector.Messages.v2.Request;







namespace BitfinexRestConnector
{
    public class CBitfinexRestConnectorV2 : CBaseBitfinexRestConnector
    {

        
        public CBitfinexRestConnectorV2(IAlarmable client, string key, string secret)
            : base(client, key, secret)
        {



        }

    
        protected override string Nonce
        {
            get
            {


                return ((UInt64)(int)(DateTime.UtcNow - epoch).TotalSeconds * 1000000).ToString();
                
               
            }
        }




        private string SendRequest(string rowBody,string apiPath, string requestMethod)
        {

           

           // string apiPath = "v2/auth/r/orders/hist";


            string stNonce = Nonce;

            string sinaturev2 = String.Format(@"/api/{0}{1}{2}", apiPath, stNonce, rowBody);

            Log(String.Format("=> Nonce={0} path={1}",Nonce, apiPath));
         

            byte[] data = Encoding.UTF8.GetBytes(sinaturev2);
            byte[] hash = hashMaker.ComputeHash(data);
            string signature = GetHexString(hash);



          
            string url = "https://api.bitfinex.com/" + apiPath;
            HttpWebRequest wr = WebRequest.Create(url) as HttpWebRequest;


        

            wr.Headers.Add("bfx-nonce", stNonce);
            wr.Headers.Add("bfx-apikey", Key);
            wr.Headers.Add("bfx-signature", signature);

            wr.ContentType = "application/json";

         
            wr.Method = "POST";

            byte[] bts = Encoding.UTF8.GetBytes(rowBody);

            wr.ContentLength = bts.Length;
            Stream newStream = wr.GetRequestStream();
            newStream.Write(bts, 0, bts.Length);
            newStream.Close();


            string response = null;
            try
            {
             


                HttpWebResponse resp = wr.GetResponse() as HttpWebResponse;
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                response = sr.ReadToEnd();
                sr.Close();
            }
            catch (WebException ex)
            {
                Error("Response Error", ex);
                StreamReader sr = new StreamReader(ex.Response.GetResponseStream());
                response = sr.ReadToEnd();
                sr.Close();
                //throw new BitfinexException(ex, response);
                //TODO process error
            }
            return response;
        }




        public ResponseOrders[] GetOrderHistory()
        {
            RequestOrderHistory req = new RequestOrderHistory();
            string rowBody = JsonConvert.SerializeObject(req);
            string response = SendRequest(rowBody, "v2/auth/r/orders/hist", "POST");

            ResponseOrders[] ro = null;

            Log("<= " + response);

            try
            {
                ro = JsonConvert.DeserializeObject<ResponseOrders[]>(response);
            }
            catch (Exception exc)
            {
                Error("<= " + response, exc);

            }

            return ro;

        }

        public ResponseTrades[]  GetTrades()
        {
            RequestTrades req = new RequestTrades();
            string rowBody = JsonConvert.SerializeObject(req);
            string response = SendRequest(rowBody, "v2/auth/r/trades/hist", "POST");

            ResponseTrades[] rt = null;

            Log("<= "+response);

            try
            {
                rt = JsonConvert.DeserializeObject<ResponseTrades[]>(response);
            }
            catch (Exception exc)
            {
                Error("<= "+response ,exc);
            }

            return rt;



        }

        public void Error(string msg, Exception e=null)
        {
            _alarmer.Error(msg);
            Log(String.Format("Error ! {0} {1}",
                msg,   e.Message));

        }





    }
}
