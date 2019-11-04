using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ASTS.Interfaces;
using ASTS.Common;
using ASTS.Connector;



namespace ASTS.Connector.State
{
	public abstract class CBaseStateConnector  : CBaseState
	{
		protected CASTSConnectorSingle _connector;
       


		public CBaseStateConnector(CASTSConnectorSingle connector) : base (connector)
		{
			_connector = connector;
         

		}
	
      

       protected void SetState(CBaseStateConnector newState) 
		{
            LogState(newState);			
		    _connector .SetState(newState);
		}



        public abstract void Process();
       

	}
}
