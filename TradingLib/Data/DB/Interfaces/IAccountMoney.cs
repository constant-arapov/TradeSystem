using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Data.DB.Interfaces
{
    public interface IAccountMoney
    {


         int id { get; set; }
         Decimal money_avail { get; set; }
         Decimal money_sess_limit { get; set; }

         int GetId();

    }
}
