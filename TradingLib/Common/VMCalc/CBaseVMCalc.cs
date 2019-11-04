using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common.VMCalc
{
    public abstract class CBaseVMCalc
    {

        public abstract decimal GetVMCurrent_RUB_clean(CBotPos botPos, decimal openedAmount);
        public abstract decimal GetVMClosed_RUB_clean(CBotPos botPos, decimal closedAmount);
      

        public CBaseVMCalc()
        {

        }
        public void SetBotPos()
        {
           
        }

        public static CBaseVMCalc CreateFORTSVmCalc()
        {
            return new CFORTSVmCalc();
        }

        public static CBaseVMCalc CreateMOEXVMCalc()
        {
            return new CMOEXVmCalc();
        }

        public static CBaseVMCalc CreateCryptoVMCalc()
        {
            return new  CCryptoVMCalc();
        }


    }
}
