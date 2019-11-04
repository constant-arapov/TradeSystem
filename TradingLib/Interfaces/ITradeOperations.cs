using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.Enums;

namespace TradingLib.Interfaces
{
    public interface ITradeOperations
    {
        void AddOrder(int botId, string isin, decimal price, EnmOrderDir dir, decimal amount);
        //void CancelOrder(long orderId);
        void CancelOrder(long orderId, int BotId);
        void CancelAllOrders(int buy_sell, int ext_id, string isin, int botId);

        void ChangePassword(string currPassword,string newPassword);
    }
}
