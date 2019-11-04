using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Interfaces.Clients;
using TradingLib.Data;

using TradingLib.Abstract;

namespace ASTS.DealingServer
{
    public class CDealBoxASTS : CBaseDealBox
    {
        public CDealBoxASTS(IClientDealBox client) : base(client)
        {

            _client.EvDealsOnline.Set();
        }

        public void Update(string instrument,CRawDeal rd)
        {

            m_dealsStructList[instrument].Update(rd);

        }




    }
}
