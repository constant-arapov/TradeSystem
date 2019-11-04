using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ASTS.DealingServer.Session;

namespace ASTS.DealingServer.Session.State
{
    public class CStateSess_OpenPeriod : CBaseStateSession
    {
        public CStateSess_OpenPeriod(CSessionBoxASTS sessionBox)
            : base(sessionBox)
        {
         

        }
        /*
        public override void OnOpenPeriodEnd()
        {
            SetState(_sessionBox.StateSess_NonActive);
      
        }
        */

    }
}
