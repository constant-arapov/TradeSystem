using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace BitfinexWebSockConnector.Data
{
    class CBfxStockStorMsgUpdSnap
    {
        public int prec;

        public JArray jArrOrderBook;
    }
}
