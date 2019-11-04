using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BitfinexWebSockConnector.Messages.Response;

namespace BitfinexWebSockConnector
{
    static class Err
    {
        public static long ERR_UNK	=         10000;
        public static long ERR_GENERIC	=     10001;
        public static long ERR_CONCURRENCY	= 10008;
        public static long ERR_PARAMS	=     10020;
        public static long ERR_CONF_FAIL =    10050;
        public static long ERR_AUTH_FAIL =	  10100;
        public static long ERR_AUTH_PAYLOAD = 10111;
        public static long ERR_AUTH_SIG =     10112;
        public static long ERR_AUTH_HMAC =    10113;
        public static long ERR_AUTH_NONCE =   10114;
        public static long ERR_UNAUTH_FAIL =  10200;
        public static long ERR_SUB_FAIL =     10300;
        public static long ERR_SUB_MULTI =    10301;
        public static long ERR_UNSUB_FAIL =   10400;
        public static long ERR_READY =        11000;
        public static long EVT_STOP =         20051;
        public static long EVT_RESYNC_START = 20060;
        public static long EVT_RESYNC_STOP	= 20061;
        public static long EVT_INFO =          5000;
        

        private static Dictionary<long, string> _dictMsg = new Dictionary<long, string>() 
        { 
           {ERR_UNK, "Unknown error"},
           {ERR_GENERIC,"Generic error"},
           {ERR_CONCURRENCY,"Concurrency error"},           
           {ERR_PARAMS,	"Request parameters error"},
           {ERR_CONF_FAIL,	"Configuration setup failed"},
           {ERR_AUTH_FAIL,	"Failed authentication"},
           {ERR_AUTH_PAYLOAD,	"Error in authentication request payload"},
           {ERR_AUTH_SIG,	"Error in authentication request signature"},
           {ERR_AUTH_HMAC,	"Error in authentication request encryption"},
           {ERR_AUTH_NONCE,	"Error in authentication request nonce"},
           {ERR_UNAUTH_FAIL, "Error in un-authentication request"},
           {ERR_SUB_FAIL,	"Failed channel subscription"},
           {ERR_SUB_MULTI,	"Failed channel subscription: already subscribed"},
           {ERR_UNSUB_FAIL,	"Failed channel un-subscription: channel not found"},
           {ERR_READY,	"Not ready, try again later"},
           {EVT_STOP,	"Websocket server stopping... please reconnect later"},
           {EVT_RESYNC_START,	"Websocket server resyncing... please reconnect later"},
           {EVT_RESYNC_STOP,	"Websocket server resync complete. please reconnect"},
           {EVT_INFO,	"Info message"}

        
        
        };

        public static string GetErrorMessage(long errCode)
        {
            return _dictMsg.ContainsKey(errCode) ? _dictMsg[errCode] : "Not defined code message";
        }

        public static string GetFullErrorMessage(ResponseError responseError)
        {
            return String.Format("Bitfinex server response error. Code: {0}. Message: \"{1}\",\"{2}\"", 
                                    responseError.Code, GetErrorMessage(responseError.Code),responseError.Msg);
        }

    }
}
