using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;
using Common.Logger;

using ASTS.Interfaces;
using ASTS.Common;

using ASTS.DealingServer.Session;

namespace ASTS.DealingServer.Session.State
{
	public class CBaseStateSession : CBaseState
	{

	
        protected CSessionBoxASTS _sessionBox;

		public CBaseStateSession(CSessionBoxASTS sessionBox  ) 
            : base(sessionBox)
		{
            _sessionBox = sessionBox;
		}


        protected void SetState(CBaseStateSession newState)
        {
            LogState(newState);
            _sessionBox .SetState(newState);
        }

        public virtual void OnNormalTradingStart()
        {          
        }


        public virtual void OnNormalTradingEnd()
        {


        }

        /*
        public virtual void OnNormalTradingClosed()
        {
        }

        public virtual void OnClearingStart()
        {
        }

        public virtual void OnClearingEnd()
        {
        }

        public virtual void OnOpenPeriodStart()
        { 
        }

        public virtual void OnOpenPeriodEnd()
        {
        }


        public virtual void OnOpenClosePeriod()
        {


        }
        */

	}
}
