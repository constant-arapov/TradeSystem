using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ASTS.Connector;


namespace ASTS.Connector.State
{
    public class CStateConnector_Off : CBaseStateConnector
    {
        public CStateConnector_Off(CASTSConnectorSingle connector)
            : base(connector)
        {


        }

		public override void Process()
        {
            //few time to Disconnect
            Thread.Sleep(100);
        }



    }
}
