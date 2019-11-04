using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Interfaces.Clients;
using TradingLib.Data.DB;


namespace CryptoDealingServer.Interfaces
{
    public interface IClientPositionsBoxCrypto :  IClientPosBox
    {

        CListInstruments Instruments { get; set; }
		int StockExchId { get; set; }
    }
}
