using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BitfinexWebSockConnector.Enums;

namespace BitfinexWebSockConnector.Messages.Request
{
    public class RequestSubscribeBook : MessageBase
    {
        public RequestSubscribeBook()
        {
            Event = EnmMessageType.Subscribe;

            Channel = "book";

            //Prec = "P1";

            Freq = "F0";

            //Len = "100";
        }


        public string Channel { get; set; }

        public string Symbol { get; set; }

        public string Prec { get; set; }

        public string Freq { get; set; }

        public string Len { get; set; }




    }
}
