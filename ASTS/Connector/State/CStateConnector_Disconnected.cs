using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using ASTS.Interfaces;
using ASTS.Common;
using ASTS.Connector;

namespace ASTS.Connector.State
{
	public class CStateConnector_Disconnected :CBaseStateConnector
	{

	//	private CASTSConnectorSingle _connector;

		public CStateConnector_Disconnected(CASTSConnectorSingle connector): base(connector)
		{

		}
	
		public override void Process()
		{
			try
			{
                _connector.IsConnectedToServer = false;
                Log("Coonnection attempt");
				_connector.Connect();                
				//_connector.SetState(_connector.StateConnected);
                Log("Connection successfull");
                SetState(_connector.StateConnected);

			}
			catch (Exception e)
			{
				//Log Unable connect
				//Thread.Sleep(100);
                string msg = "Connection attempt unsuccessfull";
                Log(msg);
                Error(msg,e);
                //wait for connection reset and protect
                //against "user already in use"
                Thread.Sleep(3000);
								
			}

		}


        public override void RequestDisconnect()
        {
            Log("Already disconnected");
        }


	}
}
