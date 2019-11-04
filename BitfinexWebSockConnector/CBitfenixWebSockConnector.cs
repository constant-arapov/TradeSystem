using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;


using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using Common;
using Common.Utils;
using Common.Interfaces;
using Common.Logger;

using TradingLib;
using TradingLib.Enums;
using TradingLib.Data;

using BitfinexCommon;
using BitfinexCommon.Enums;
using BitfinexCommon.Messages.Response;


using BitfinexWebSockConnector.Interfaces;
using BitfinexWebSockConnector.Enums;
using BitfinexWebSockConnector.Data;
using BitfinexWebSockConnector.Messages;
using BitfinexWebSockConnector.Messages.Request;
using BitfinexWebSockConnector.Messages.Request.Converters;
using BitfinexWebSockConnector.Messages.Response;

using BitfinexWebSockConnector.Helpers;

namespace BitfinexWebSockConnector
{
	public class CBitfenixWebSockConnector : CBaseFunctional, IClientBfxStockStor
	{


        public IClientBfxWebSockCon _client;

        private string _apiKey;
        private string _apiSecret;


        private ILogable _logRaw;


        private long _chanIdPersonalData;

        private Dictionary<long, CBookParams> _dictBookChanidInstr = new Dictionary<long, CBookParams>();

        private Dictionary<long, string> _dictTradesChanidInstr = new Dictionary<long, string>();

        private WebSocket _webSocket;

        private CPerfOrdBook _perf;


        private List<CCryptoInstrData> _lstInstrumentsData = new List<CCryptoInstrData>();

        private Dictionary<string, CBfxStockStor> _dictStockStor = new Dictionary<string, CBfxStockStor>();

        private bool _isAuth;

        private bool _forceReconnect = false;



        public bool IsConnected
        {
            get
            {
                if (_webSocket != null)
                    return _webSocket.IsAlive;
                                  
                return false;
            }

        }




		public CBitfenixWebSockConnector(IClientBfxWebSockCon client,
                                        List<CCryptoInstrData> lstInstruments,
                                        bool isAuth,
										string apiKey,
                                        string apiSecret)
            : base(client, "CBitfenixWebSockConnector" + GetPostFix(isAuth))
		{
            _client = client;
            _lstInstrumentsData = lstInstruments;
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _isAuth = isAuth;
            _logRaw = new CLogger("BfxRaw" + GetPostFix(_isAuth));
            LogRaw("================================ STARTED ======================================================");
           // GenListInstrument();
            CreateSockStor();

            _perf = new CPerfOrdBook(true);

		}

        private static string GetPostFix(bool isAuth)
        {
            return isAuth ? "_auth" : "_public";
        }


        private long GenUniqueId(int botId)
        {
            //After Bitfinex's modification of 2018-01-08
            //uid type was changed to int45 (truncated)
            //so 1000 was changed to 1
            //2018-10-01 changed  CreateAuthNonce => CreateAuthNonceShort
            long uid =  /*1000*/1 * BitfinexAuthentication.CreateAuthNonceShort() + botId;
            return uid;
        }



        public void UpdateCleintStockBothDir(string instrument, int precision, CSharedStocks stock)
        {
            _client.UpdateStockBothDir(instrument, precision, stock);
        }

        public void UpdateClientStockOneDir(string instrument, Direction dir, int precision,
                                        CSharedStocks stock)
        {
            _client.UpdateStockOneDir(instrument, dir, precision, stock);
        }





        public void AddOrder(int botId, string instrument, EnmOrderDir dir, decimal amount, decimal price)
        {
			try
			{

                
				decimal amountUse =  (dir == EnmOrderDir.Buy) ?  amount :   -amount;

                RequestNewOrder newOrd = new RequestNewOrder(gid: botId,
															 cid:GenUniqueId(botId), 
															 symbol:instrument,
															 type:EnmBfxOrderType.Limit, 
															 amount: amountUse,
															 price:  price);

				//var serNewOrd = JsonConvert.SerializeObject(newOrd, Formatting.Indented, new RequestNewOrderConverterter());
				var serNewOrd = JsonConvert.SerializeObject(newOrd, CBitfinexJsonSerializer.Settings);
				//var serNewOrd = CBitfinexJsonSerializer.SerializeObject(newOrd);


				SendMessage(serNewOrd);

				Log(String.Format("[ADD ORDER] ==> botId={0} instrument={1} dir={2} amount={3} price={4}",
                                    botId,
                                    instrument, 
                                    dir,
                                    amount, 
                                    price));




			}
			catch (Exception e)
			{
				Error("CBitfenixWebScokConnector.AddOrder", e);

			}
        }


        public void AddMarketOrder(int botId, string instrument, EnmOrderDir dir, decimal amount, decimal price)
        {


            decimal amountUse = (dir == EnmOrderDir.Buy) ? amount : -amount;

            RequestNewOrder newOrd = new RequestNewOrder(gid: botId,
                                                         cid: GenUniqueId(botId),
                                                         symbol: instrument,
                                                         type: EnmBfxOrderType.Market,
                                                         amount: amountUse,
                                                         price: price);


            newOrd.Close = 1;
       
            var serNewOrd = JsonConvert.SerializeObject(newOrd, CBitfinexJsonSerializer.Settings);
            

            SendMessage(serNewOrd);

            Log(String.Format("[ADD ORDER] ==> {0} dir={1} amount={2} price={3}", instrument, dir, amount, price));


        }



        public void CancellAllOrders(int botId)
        {
           

            CRequestCancellOrderByGid recCancByGid = new CRequestCancellOrderByGid { Gid = botId };
            var serRecCancByGid = JsonConvert.SerializeObject(recCancByGid, CBitfinexJsonSerializer.Settings);

            SendMessage(serRecCancByGid);
            Log(String.Format("[CANCELL ALL ORDERS] ==> gid={0}", botId));

        }


        public void CancellOrder(long orderId)
        {

            RequestCancellOrder co = new RequestCancellOrder { Id = orderId };
            var sco = JsonConvert.SerializeObject(co, CBitfinexJsonSerializer.Settings);

            SendMessage(sco);

            Log(String.Format("[CANCELL ORDER] ==> {0}", orderId));



        }

        

        public void CreateSockStor()
        {

           
            _lstInstrumentsData.ForEach
                (instrData => _dictStockStor[instrData.Instrument] = 
                            new CBfxStockStor(this, instrData.Instrument, instrData.DecimalVolume,200,
                                              GetPricePrecisions()));



        }

        public List<int> GetPricePrecisions()
        {
            return _client.GetPricePrecisions();          
        }

        public int GetStockDepth(int precision)
        {
            return _client.GetStockDepth(precision);
        }




  


        




        private void OnWebSockOpen(object sender, EventArgs e)
        {
            try
            {
                Log("OnWebSockOpen");
                bool bIsValid = false;
                string msg = GetAuthMsg(ref bIsValid);
                if (bIsValid)
                {
                    if (_isAuth)
                        _webSocket.Send(msg);
                    else
                   _lstInstrumentsData.ForEach(instrData =>
                    {

                        foreach (int percision in GetPricePrecisions())                      
                            _webSocket.Send(GetSubscribeBook(instrData.Instrument,
                                                            precisionLevel: percision));
                          
                      
                        _webSocket.Send(GetSubscribeTrade(instrData.Instrument));                      
                    });
                }
                else
                {
                    Error("Auth data was not generateed");
                }

            }
            catch (Exception exc)
            {
                Error("OnWebSockOpen", exc);
            }
        }



        public void ProcessData(string data)
        {
            LogRaw(data);


            if (data[0] == '{') //event message
            {

                MessageBase mb = CBitfinexJsonSerializer.DeserializeObject<MessageBase>(data);


                if (mb.Event == EnmMessageType.Info)
                    ProcessResponseInfo(CBitfinexJsonSerializer.DeserializeObject<ResponseInfo>(data));
                else if (mb.Event == EnmMessageType.Auth)
                    ProcessResponseAuth(CBitfinexJsonSerializer.DeserializeObject<ResponseAuth>(data));
                else if (mb.Event == EnmMessageType.Error)
                    ProcessResponseError(data, CBitfinexJsonSerializer.DeserializeObject<ResponseError>(data));
                else if (mb.Event == EnmMessageType.Subscribed)
                {
                    ResponseSubscribed rs = CBitfinexJsonSerializer.DeserializeObject<ResponseSubscribed>(data);
                    if (rs.Channel == "book")
                    {

                        ResponseSubscribedBook rsb = CBitfinexJsonSerializer.DeserializeObject<ResponseSubscribedBook>(data);

                        //string instrument = rs.Symbol[0] == 't' ? rs.Symbol.Remove(0, 1) : rs.Symbol;
                        CBookParams bp = new CBookParams
                        {
                            Instrument = rsb.Symbol[0] == 't' ? rsb.Symbol.Remove(0, 1) : rsb.Symbol,
                            Precision = rsb.Prec
                        };


                        //string instrument = rs.Symbol;
                        UpdateBookChanInstr(rs.ChanId, bp);
                    }
                    else if (rs.Channel == "trades")
                    {
                        UpdateTradesChanInstr(rs.ChanId, rs.Symbol);
                    }

                }

            }
            else if (data[0] == '[') //channel data
            {

                JArray jArr = CBitfinexJsonSerializer.DeserializeObject<JArray>(data);

                if (jArr.Count() < 2)
                {
                    Error("Invalid message");
                    return;
                }

                long channelId = (int)jArr[0];

                string instrument = "";
                CBookParams bookParam = null;

                if (channelId == _chanIdPersonalData)
                {

                    string evnt = (string)jArr[1];

                    if (evnt == "ws")
                    {
                        ProcessWalletSnapshot(jArr);
                    }
                    else if (evnt == "wu")
                    {
                        ProcessWalletUpdate(jArr);
                    }
                    else if (evnt == "ps")
                    {
                        ProcessPositionsSnapshot((JArray)jArr[2]);
                    }
                    else if (evnt == "pn")
                    {
                        ProcessPositionNew((JArray)jArr[2]);
                    }
                    else if (evnt == "pu")
                    {
                        ProcessPositionUpdate((JArray)jArr[2]);
                    }
                    else if (evnt == "pc")
                    {
                        ProcessPositionClose((JArray)jArr[2]);
                    }
                    else if (evnt == "os")
                    {
                        ProcessOrdersSnapshot((JArray)jArr[2]);
                    }
                    else if (evnt == "on")
                    {
                        ProcessOrderNew((JArray)jArr[2]);
                    }
                    else if (evnt == "ou")
                    {
                        ProcessOrderUpdate((JArray)jArr[2]);
                    }
                    else if (evnt == "oc")
                    {
                        ProcessOrderCancell((JArray)jArr[2]);
                    }
                    else if (evnt == "fos")
                    {
                        ProcesFundingOrderSnapshot(jArr);
                    }
                    else if (evnt == "n")
                    {
                        ProcessNotification((JArray)jArr[2]);
                    }
                    else if (evnt == "te")
                    {
                        ProcessUserTradeExecute((JArray)jArr[2]);
                    }
                    else if (evnt == "tu")
                    {
                        ProcessUserTradeUpdate((JArray)jArr[2]);
                    }
                    else if (evnt == "hb")
                    {
                        if (_name.Contains("auth"))
                            ProcessHeartBeatAuth();

                    }


                }
                else if (GetBookParam(channelId, ref bookParam))
                {
                    if (jArr.Count() == 2)
                    {
                        if (IsHeartBeat(jArr[1]))
                            return;
                        JArray arr = (JArray)jArr[1];
                        if (arr[0].Type == JTokenType.Array)
                        {
                            ProcessOrderBookSnapshot(bookParam, arr);
                        }
                        else
                        {
                            ProcessOrderBookUpdate(bookParam, arr);

                        }
                    }


                }
                else if (GetTradesInst(channelId, ref instrument))
                {

                    if (jArr.Count() == 2)
                    {

                        if (IsHeartBeat(jArr[1]))
                            return;


                        ProcessTradesSnapshot(instrument, (JArray)jArr[1]);

                    }
                    else if (jArr.Count() == 3)
                    {
                        string evnt = (string)jArr[1];
                        //As we don't need trade id - use te, which must be recieved first,
                        //not tu using this docs:
                        // http://blog.bitfinex.com/api/websocket-api-update/
                        if (evnt == "te")
                            ProcessTradeExecute(instrument, (JArray)jArr[2]);

                    }

                }

            }
            else
            {
                Error("Websocket. Unknown message");
            }


        }





        private  void OnWebSockMsg(object sender, MessageEventArgs e)
        {

            try
            {
                ProcessData(e.Data);

             


            }
            catch (Exception excpt)
            {
                Error("Error processing message", excpt);
            }

        }
        
        /// <summary>
        /// Put periodic actions here.
        /// 
        /// Added 2018-05-15
        /// </summary>
        private void ProcessHeartBeatAuth()
        {
            _client.PeriodicActBfxAuth();
        }







        private void OnWebSockError(object sender, ErrorEventArgs e)
        {
            Log("OnWebSockError" + e.Message);
            Error("OnWebSockError ",e.Exception );

        }

        private void OnWebSockClosed(object sender, CloseEventArgs e)
        {

            Error(String.Format("Websocket {0}  closed. Code={1} Reason={2} WasClean={3}",
                 _name, //0
                   e.Code, //1
                   e.Reason, //2
                   e.WasClean //3
                   ));
                  
        }




        public void Process()
        {
            CUtil.ThreadStart(ThreadMain);

          //  CUtil.ThreadStart(ThreadTest);

        }


        private void ThreadTest()
        {
            Thread.Sleep(3000);
            while (true)
            {
                Error("Test.Test.Test.Test.Test.Test.Test.Test.Test.Test.Test.Test");
                Thread.Sleep(1);
            }
        }




        private void ThreadMain()
        {




             _webSocket = new WebSocket("wss://api.bitfinex.com/ws/2");

           // _webSocket = new WebSocket("wss://api.bitfinex.com/ws");


            _webSocket.OnOpen += OnWebSockOpen;

            _webSocket.OnMessage += OnWebSockMsg;

            _webSocket.OnError += OnWebSockError;

            _webSocket.OnClose += OnWebSockClosed;


            bool bIsFirstTimeConn = true;
            //_webSocket.OnClose += 


            // _webSocket.Connect();

            while (true)
            {
                //changed 2018-04-16
                //2018-11-27 added forece reconnect
                if (!_webSocket.IsAlive || _forceReconnect)
                {
                    try
                    {

                        if (_forceReconnect)
                        {
                            Error("Force reconnect started");
                            try
                            {
                                _webSocket.Close();

                            }
                            catch (Exception exc)
                            {
                                Error("WebsocketForceReconnect Close", exc);                                
                            }
                            
                               
                            Thread.Sleep(10000);
                            _forceReconnect = false;
                            Error("Force reconnect ended");
                            //and than try to reconnect below
                        }


                        if (bIsFirstTimeConn)
                        {
                            Log("Try connect to server");
                            bIsFirstTimeConn = false;
                        }
                        else
                        {                           
                            Error("Disconnected. Connection attempt...");
                          
                        }

                        
                           

                        _webSocket.Connect();
                        Thread.Sleep(100);
                    }
                    catch (Exception e)
                    {
                        Thread.Sleep(100);
                        Error("CBetfinexWebSockConnector.ThreadMain",e);
                    }

                }
                else
                    Thread.Sleep(10);


            }


        }

       

        public void SendPing(int cid=0)
        {
            RequestPing rp = new RequestPing { Cid = cid };
            string msg = CBitfinexJsonSerializer.SerializeObject(rp);

            SendMessage(msg);
        }



        private void SendMessage(string msg)
        {
            if (_webSocket.IsAlive)
                _webSocket.Send(msg);

        }


        private bool IsHeartBeat(JToken jtok)
        {
            if (jtok.Type == JTokenType.String)
                if ( (string)jtok == "hb")
                    return true;

            return false;
        }

       

        private bool GetTradesInst(long chanId, ref string instrument)
        {
            return GetSubscibeInstr(chanId, ref instrument, _dictTradesChanidInstr);
        }




        private bool GetSubscibeInstr(long chanId, ref string instrument, Dictionary<long, string> dictChanInstr)
        {


            if (dictChanInstr.ContainsKey(chanId))
            {
                instrument = dictChanInstr[chanId];
                return true;

            }
            return false;
        }

        private bool GetBookParam(long chanId, ref CBookParams bookParam)
        {
            
            if (_dictBookChanidInstr.ContainsKey(chanId))
            {
                bookParam = _dictBookChanidInstr[chanId];
                return true;
            }


            return false;
        }




        private void UpdateBookChanInstr(long chanId, CBookParams newBookParam)
        {
            //UpdateChanInstrDict(_dictBookChanidInstr, chanId, instrument);
            //UpdateBookChanInstr(chanId, newBookParam);
             bool bNeedRemove = false;
            long keyRemove = 0;

            //if channel changed (for example after disconnect)
            //need to remove old channel first
            foreach (var kvp in _dictBookChanidInstr)
            {
                if (kvp.Value.Instrument == newBookParam.Instrument
                    && kvp.Value.Precision == newBookParam.Precision)
                {
                    bNeedRemove = true;
                    keyRemove = kvp.Key;
                }
            }
            if (bNeedRemove)
                _dictBookChanidInstr.Remove(keyRemove);


            _dictBookChanidInstr[chanId] = newBookParam;
        }

    



        private void UpdateTradesChanInstr(long chanId, string instrument)
        {
            UpdateChanInstrDict(_dictTradesChanidInstr, chanId, instrument);
        }






        private void UpdateChanInstrDict( Dictionary<long, string> dictChan, long chanId, string instrument)
        {

            bool bNeedRemove = false;
            long keyRemove = 0;
           
            //if channel changed (for example after disconnect)
            //need to remove old channel first
            foreach (var kvp in dictChan)
            {
                if (kvp.Value == instrument)
                {
                    bNeedRemove = true;
                    keyRemove = kvp.Key;
                }              
            }
            if (bNeedRemove)
                dictChan.Remove(keyRemove);
            
            //add new channel
            dictChan[chanId] = instrument;


        }

        /*
        public void UpdateStock(long chanId, CBookParam newBookParam)
        {

            bool bNeedRemove = false;
            long keyRemove = 0;

            //if channel changed (for example after disconnect)
            //need to remove old channel first
            foreach (var kvp in _dictBookChanidInstr)
            {
                if (kvp.Value.Instrument == newBookParam.Instrument)
                {
                    bNeedRemove = true;
                    keyRemove = kvp.Key;
                }
            }
            if (bNeedRemove)
                _dictBookChanidInstr.Remove(keyRemove);


            _dictBookChanidInstr[chanId] = newBookParam;


        }
        */







        public string GetSubscribeBook(string instrument, int precisionLevel)
        {
           // int precisionLevel = 1;
            RequestSubscribeBook reqBook = new RequestSubscribeBook {
                                                                        Prec = String.Format("P{0}", precisionLevel),
                                                                        Symbol = instrument,
                                                                        Len = precisionLevel==0 ? "25": "100"
                                                                       // Len = "100"
                                                                        };
            string msgBook = CBitfinexJsonSerializer.SerializeObject(reqBook);
            return msgBook;
                         
        }


        public string GetSubscribeTrade(string instrument)
        {
            RequestSubsribeTrades reqTr = new RequestSubsribeTrades { Symbol = instrument };
            string msgTrade = CBitfinexJsonSerializer.SerializeObject(reqTr);
            return msgTrade;
        }
      



        private string  GetAuthMsg(ref bool bIsValid)
        {
            try
            {
                long authNonce = BitfinexAuthentication.CreateAuthNonce();
                string authPayload = BitfinexAuthentication.CreateAuthPayload(authNonce);
                string authSig = BitfinexAuthentication.CreateSignature(authPayload, _apiSecret);

                RequestAuth reqAuth = new RequestAuth
                                        {
                                            ApiKey = _apiKey,
                                            AuthNonce = authNonce,
                                            AuthSig = authSig,
                                            AuthPayload = authPayload
                                            
                                        };

                bIsValid = true;

                return CBitfinexJsonSerializer.SerializeObject(reqAuth);
                     
               

            }
            catch (Exception e)
            {

                Error("Unable gen AuthMsg");
                return "";
            }



        }




        private void ProcessResponseError(string rawMsg, ResponseError responseError)
        {
            string msg = Err.GetFullErrorMessage(responseError);
           
            if (responseError.Code == Err.ERR_SUB_FAIL)
            {
                ResponseErrorSubscribe res = CBitfinexJsonSerializer.DeserializeObject<ResponseErrorSubscribe>(rawMsg);
                msg += " Symbol=" + res.Symbol;

            }
            
            
            Log(msg);
            Error(msg);

        }


		private void ProcessWalletSnapshot(JArray jArr)
		{
			var data = jArr[2];
			ResponseWallet[] wallets= data.ToObject<ResponseWallet[]>();
		    Log("[WALLETS SNAPSHOT] <= ");
            foreach (var wal in wallets)
            {
                Log(wal.ToString());
                //for now - usd and margin only
                if (wal.Currency == "USD" && wal.WalletType == "margin")
                    _client.UpdateWallet(wal.WalletType, wal.Currency,  Convert.ToDecimal(wal.Balance));
            }

		}


        private void ProcessWalletUpdate(JArray jArr)
        {
            var data = jArr[2];
            ResponseWallet wallets = data.ToObject<ResponseWallet>();
            Log("[WALLETS UPDATE] <= " + wallets.ToString()) ;

            if (wallets.Currency == "USD" && wallets.WalletType == "margin")
                _client.UpdateWallet(wallets.WalletType, wallets.Currency, Convert.ToDecimal(wallets.Balance));

        }




        private void ProcessPositionsSnapshot(JArray jArr)
        {

            ResponsePositions[] respPosSnap = jArr.ToObject<ResponsePositions[]>();
            Log("[POSITIONS SNAPSHOT] <= ");
            foreach (ResponsePositions ps in respPosSnap)
            {
                Log(ps.ToString());
                _client.UpdatePos(ps);
            }
        }

        private void ProcessPositionUpdate(JArray jArr)
        {
            ResponsePositions respPos = jArr.ToObject<ResponsePositions>();
            Log(String.Format("[POSITION UPDATE] <== {0} ", respPos.ToString()));
            _client.UpdatePos(respPos);
        }

        private void ProcessPositionClose(JArray jArr)
        {
            ResponsePositions respPos = jArr.ToObject<ResponsePositions>();
            Log(String.Format("[POSITION CLOSE] <== {0}",respPos.ToString()));
            _client.UpdatePos(respPos);
        }


        private void ProcessPositionNew(JArray jArr)
        {
            ResponsePositions respPos = jArr.ToObject<ResponsePositions>();
            Log(String.Format("[POSITION NEW] <== {0}", respPos.ToString()));
            //2018-11-20 added
            _client.UpdatePos(respPos);
        }


        private void ProcessOrderBookSnapshot(CBookParams bookParam, JArray jArrOrderBook)
        {
            _perf.StartOrdeBookSnapshot();
            /*
              _dictStockStor[bookParam.Instrument].UpdateBySnapshot(CBfxUtils.GetPrecInt(bookParam.Precision), 
                                                                        jArrOrderBook);
                                                                        */


            _dictStockStor[bookParam.Instrument].UpdateBySnapshot
                                    (new CBfxStockStorMsgUpdSnap
                                    {
                                        prec = CBfxUtils.GetPrecInt(bookParam.Precision),
                                        jArrOrderBook = jArrOrderBook
                                    });

            _perf.EndOrderBookSnapshot();
        }

        private void ProcessOrderBookUpdate(CBookParams bookParam, JArray jArrOrderBookUpdate)
        {
            _perf.StartOrdeBookUpd();

            decimal price = (decimal)jArrOrderBookUpdate[0];
            long count = (long)jArrOrderBookUpdate[1];
            decimal amount = (decimal)jArrOrderBookUpdate[2];

           /*
             _dictStockStor[bookParam.Instrument].Update(CBfxUtils.GetPrecInt(bookParam.Precision),
                                                              price,count, amount);

           */
             
            _dictStockStor[bookParam.Instrument].Update(
                new CBfxStockStorUpdStock
                {
                    amount = amount,
                    count = count,
                    prec = CBfxUtils.GetPrecInt(bookParam.Precision),
                    price = price
                }
                );
              
            _perf.EndOrderBookUpd();


        }




        private void ProcessTradesSnapshot(string inpInstrWithPrefix, JArray jArrTrades)
        {
           // Thread.Sleep(0);
        }


        private void ProcessUserTradeExecute(JArray jarrTe)
        {
            ResponseTrades rt = jarrTe.ToObject<ResponseTrades>();

            Log(String.Format("[USER TRADE EXECUTE] <== {0}", rt.ToString()));
			_client.UpdateUserDeals(rt);
            
        }


        private void ProcessUserTradeUpdate(JArray jarrTu)
        {

            ResponseTrades rt = jarrTu.ToObject<ResponseTrades>();
            Log(String.Format("[USER TRADE UPDATE] <== {0}", rt.ToString()));
			_client.UpdateUserDealsLateUpd(rt);

        }



        private void ProcessTradeExecute(string inpInstrWithPrefix, JArray jarrTe)
        {

            string instrument = CBfxUtils.RemoveFirstT(inpInstrWithPrefix);


            int decimalVolume = GetDecimalVolume(instrument);

            long lngMilis = (long) jarrTe[1];
            decimal dcmlAmountRaw = (decimal)jarrTe[2];
            decimal dcmlAmount = Math.Abs(dcmlAmountRaw);
            decimal dcmlPrice = (decimal)jarrTe[3];



			long amount = CUtilConv.GetIntVolume(dcmlAmount, decimalVolume);

            //2018-02-22
            //Amount is too small. Not possible to understand how it 
            //could be. Was seen on LTC. For now just ignore these trades
            if (amount == 0)
               return;

           
            
            DateTime dt = CUtilTime.DateTimeFromUnixTimestampMillis(lngMilis);


            CRawDeal rd = new CRawDeal { Amount = amount,
                                         Price = dcmlPrice,
                                         Moment = dt

                                        };
            


            if (dcmlAmountRaw > 0)
                rd.Id_ord_buy = 1;
            else
                rd.Id_ord_sell = 1;

            _client.UpdateDeal(instrument,rd);

          /*  DateTime dtCurr = DateTime.Now;
            double ddt = (dtCurr - dt).TotalSeconds;

            if (ddt != 00)
                Thread.Sleep(0);
           
            */

           

        }


        private int GetDecimalVolume(string instrument)
        {
            return _lstInstrumentsData.Find(el => el.Instrument == instrument).DecimalVolume;
        }


        private void ProcessOrdersSnapshot(JArray jArr)
        {
            ResponseOrders[] respOrders = jArr.ToObject<ResponseOrders[]>();

            Log(String.Format("[ORDER SNAPSHOT] <=="));
            foreach (var order in respOrders)
            {
                if (order.Gid != null)
                {

                    _client.ProcessOrder(order, EnmOrderAction.Added);

                }
                Log(order.ToString());
            }


            
        }


		private void ProcessOrderNew(JArray jArr)
		{
			ResponseOrders respOrders = jArr.ToObject<ResponseOrders>();

            Log(String.Format("[ORDER NEW] <=={0})",respOrders.ToString()));


            _client.ProcessOrder(respOrders, EnmOrderAction.Added);
		}



        private void ProcessOrderUpdate(JArray jArr)
        {
            ResponseOrders respOrders = jArr.ToObject<ResponseOrders>();

            Log(String.Format("[ORDER UPDATE] <=={0})", respOrders.ToString()));

            _client.ProcessOrder(respOrders, EnmOrderAction.Update);
        }




        private void ProcessOrderCancell(JArray jarr)
        {
            ResponseOrders respOrders = jarr.ToObject<ResponseOrders>();

            Log(String.Format("[ORDER CANCELL] <== {0}", respOrders.ToString()));


            _client.ProcessOrder(respOrders, EnmOrderAction.Deleted);

        }

        
        



        private void ProcesFundingOrderSnapshot(JArray jArr)
        {
            Thread.Sleep(0);

        }

        private void ProcessNotification(JArray jArr)
        {
            try
            {
                string type = (string) jArr[1];
                string status = (string)jArr[6];
                string text = (string) jArr[7];


                Log(String.Format("[NOTIFICATION] <== type={0} status={1} text={2}",
                                                        type, status, text));



                if (type == "on-req" && status == "ERROR")
                    OnNewOrderError(text);



            }
            catch (Exception e)
            {
                Error("CBitFenixWebSockConnector.ProcessNotification",e);
            }

        }



        private void OnNewOrderError(string text)
        {
            Error("On new order error. " + text);
        }



        private void ProcessResponseInfo(ResponseInfo infoResponse)
        {
            Log(String.Format("[{0}] <== version {1}",   infoResponse.Event, infoResponse.Version));

        }



        private void ProcessResponseAuth(ResponseAuth respAuth)
        {

            Log(String.Format("[{0}] <== Status: {1} UserId={2} Auth_Id={3}", respAuth.Event, respAuth.Status, respAuth.UserId, respAuth.Auth_Id));

            if (respAuth.Status == "OK")
            {
                _chanIdPersonalData = respAuth.ChanId;

            }
            else
                Error("AuthNotProcessed");


        }

        public void UpdateDecimalVolume(string instrument, int decimalVolume)
        {
            _lstInstrumentsData.ForEach ( instrData => {
                                        if (instrData.Instrument == instrument)
                                                instrData.DecimalVolume = decimalVolume;
                                                    });



            _dictStockStor[instrument].UpdateDecimalVolume(decimalVolume);


        }

        public void ForceReaconnect()
        {
            _forceReconnect = true;


        }




        /*
        public void UpdateStockChange(EnmStockChngCodes code, decimal)
        {

        }
        */


        private void LogRaw(string msg)
        {

            _logRaw.Log(msg);
        }

	}
}
