using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Interfaces.Clients;
using TradingLib.Data;
using TradingLib.Data.DB;


namespace CryptoDealingServer.Interfaces
{
    public interface IClientUserDealsPosBoxCrypto : IClientUserDealsPosBox
    {
        void UpdateUserPosLogLate(CDBUpdateLate dbUpdateFee);
        void UpdateFeeUserDealsLog(CDBUpdateFeeUserDealsLog dbUpdFeeUserDealsLog);
        decimal GetFeeDealingPcnt(int accountId);

        decimal GetFeePcntLim(int accountId);

        decimal GetFeePctMarket(int accountId);
       


    }
}
