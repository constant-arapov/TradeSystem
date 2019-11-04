using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Enums;

namespace ASTS.Common
{

    public delegate void DAddOrder(int botId, string instrument, decimal price, EnmOrderDir dir, decimal amount);
    public delegate void DCancelOrder(long orderId, int botId);
    public delegate void DCancelAllOrders(int buy_sell, int ext_id, string instrument, int botId);
    public delegate void DChangePassword(string currPAssword,string newPassword);
    public delegate byte[] DGetSnapshot();

}
