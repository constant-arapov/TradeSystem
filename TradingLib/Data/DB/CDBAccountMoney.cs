using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Data.DB.Interfaces;

namespace TradingLib.Data.DB
{
    public class CDBAccountMoney : IAccountMoney
    {

        public int id { get; set; }
        public int user_id { get; set; }
        public Decimal money_avail { get; set; }
        public Decimal money_sess_limit { get; set; }

      


        public int GetId()
        {
            return id;
        }


    }
}
