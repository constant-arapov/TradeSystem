using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

using BitfinexWebSockConnector.Messages.Response.Converters;



namespace BitfinexWebSockConnector.Messages.Response
{
   
    public class ResponseSubscribedBook : ResponseSubscribed
    {

        public string Prec { get; set; }



    }
}
