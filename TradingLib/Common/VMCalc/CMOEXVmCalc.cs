using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common.VMCalc
{
    public class CMOEXVmCalc : CBaseVMCalc
    {
        public CMOEXVmCalc()
            : base()
        {


        }

        public override decimal GetVMClosed_RUB_clean(CBotPos botPos, decimal closedAmount)
        {
            return botPos.VMClosed_Points * botPos.LotSize * closedAmount;

        }


        public override decimal GetVMCurrent_RUB_clean(CBotPos botPos, decimal openedAmount)
        {
            return botPos.VMCurrent_Points * botPos.LotSize * openedAmount;

        }

    }
}
