using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Utils;


namespace TradingLib.Common.VMCalc
{
    class CCryptoVMCalc : CBaseVMCalc
    {
        public override decimal GetVMClosed_RUB_clean(CBotPos botPos, decimal closedAmount)
        {
            return botPos.VMClosed_Points * closedAmount;

        }


        public override decimal GetVMCurrent_RUB_clean(CBotPos botPos, decimal openedAmount)
        {
            return botPos.VMCurrent_Points * openedAmount;

        }



    }
}
