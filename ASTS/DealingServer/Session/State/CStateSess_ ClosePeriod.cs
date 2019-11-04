using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ASTS.DealingServer.Session;

namespace ASTS.DealingServer.Session.State
{
    class CStateSess_ClosePeriod : CBaseStateSession
    {
       public  CStateSess_ClosePeriod (CSessionBoxASTS sessionBox)
                : base (sessionBox)
       {

       }

       public override void OnNormalTradingStart()
       {

           SetState(_sessionBox.StateSess_NormalTrading);
       }
    }
}
