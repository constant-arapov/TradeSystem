using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTS.DealingServer.Session.State
{
    public class CStateSess_NormalTrading : CBaseStateSession
    {

        public CStateSess_NormalTrading(CSessionBoxASTS sessionBox)
            : base(sessionBox)
        {

          
        }

        public override void OnNormalTradingEnd()
        {
            _sessionBox.SetSessionNonActive();
            SetState(_sessionBox.StateSess_NonActive);
        }


        /*
        public override void OnNormalTradingClosed()
        {
            _sessionBox.SetSessionNonActive();
            SetState(_sessionBox.StateSess_NonActive);

        }

        public override void OnClearingStart()
        {
            SetState(_sessionBox.StateSess_Clearing);
            _sessionBox.SetSessionNonActive();

        }
        public override void OnOpenClosePeriod()
        {
            SetState(_sessionBox.StateSess_OpenPeriod);
            _sessionBox.SetSessionNonActive();
        }

        */



    }
}
