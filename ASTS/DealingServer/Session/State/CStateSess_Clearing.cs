using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using ASTS.DealingServer.Session;

namespace ASTS.DealingServer.Session.State
{
    public class CStateSess_Clearing : CBaseStateSession
    {
        public CStateSess_Clearing(CSessionBoxASTS sessionBox)
            : base(sessionBox)
        {


        }

      /*  public override void OnClearingEnd()
        {
            SetState(_sessionBox.StateSess_NonActive);
        }
        */
        public override void OnNormalTradingStart()
        {
            _sessionBox.OnSessionActive();
            SetState(_sessionBox.StateSess_NormalTrading);
        }
    }
}
