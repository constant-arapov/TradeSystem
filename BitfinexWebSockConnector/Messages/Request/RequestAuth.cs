using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BitfinexWebSockConnector.Enums;
using BitfinexWebSockConnector.Messages;


namespace BitfinexWebSockConnector.Messages.Request
{
	public class RequestAuth : MessageBase
	{


        public RequestAuth()
        {

            Event = EnmMessageType.Auth;
        }



		public string ApiKey { get; set; }

		public string AuthSig { get; set; }

		public long AuthNonce { get; set; }

        public string AuthPayload { get; set; }


	}
}
