using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plaza2Connector
{
    public class CPart
    {

        public CPart(PART.part p)
        {
            Repld = p.replID;
            ReplRev = p.replRev;
            Money_old = p.money_old;
            Money_amount = p.money_amount;
            Money_free = p.money_free;
            Balance_money = p.balance_money;

        }

        public long Repld;
        public long ReplRev;
        public decimal Money_old;
        public decimal Money_amount;
        public decimal Money_free;
        public decimal Money_blocked;
        public decimal Balance_money;

    }
}
