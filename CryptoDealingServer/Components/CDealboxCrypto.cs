using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using TradingLib.Interfaces.Clients;
using TradingLib.Data;

using TradingLib.Abstract;

namespace CryptoDealingServer.Components
{
    class CDealboxCrypto : CBaseDealBox
    {
        public CDealboxCrypto(IClientDealBox client)
            : base (client)

        {

        }

        public void Update(string instrument, CRawDeal rd)
        {

            DealsStruct[instrument].Update(rd);
        }

    }
}
