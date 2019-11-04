using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common.VMCalc
{
    public class CFORTSVmCalc : CBaseVMCalc
    {
        public override decimal GetVMClosed_RUB_clean(CBotPos botPos, decimal closedAmount)
        {
            return botPos.VMClosed_Steps * botPos.StepPrice * closedAmount;

        }

        public override decimal GetVMCurrent_RUB_clean(CBotPos botPos, decimal openedAmount)
        {
            return botPos.VMCurrent_Steps * botPos.StepPrice * openedAmount;
        }


        public CFORTSVmCalc()
            : base()
        {

        }






    }
}
