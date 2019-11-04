using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using ASTS.Interfaces;

namespace ASTS.DealingServer.Session.State
{
    public class CStateSess_NonActive : CBaseStateSession
    {
        public CStateSess_NonActive(CSessionBoxASTS sessionBoxASTS)
            : base(sessionBoxASTS)
        {


        }

        public override void  OnNormalTradingStart()
       {
            _sessionBox.OnSessionActive();            
            SetState(_sessionBox.StateSess_NormalTrading);
            

       }
        /*
        public override void OnOpenPeriodStart()
        {
            _sessionBox.OnSessionActive();
            SetState(_sessionBox.StateSess_OpenPeriod);
        }

        public override  void OnClearingStart()
        {
            _sessionBox.SetSessionNonActive();
            SetState(_sessionBox.StateSess_Clearing);


        }
        */
    }
}
