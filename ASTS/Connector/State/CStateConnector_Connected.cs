using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MOEX.ASTS.Client;

using ASTS.Common;

using ASTS.Connector;


namespace ASTS.Connector.State
{
	public class CStateConnector_Connected : CBaseStateConnector
	{

       


		public CStateConnector_Connected(CASTSConnectorSingle connector)
			: base(connector)
		{

		}

		public override void Process()
		{
			try
			{
                _connector.IsConnectedToServer = true;
                //Untill disconnection is not requested
                //process data in loop
				_connector.MainLoop();

                //if requested try disconnect
                if (_connector.RequestDisconnect)
                {
                    TryDisconnect();
                  
                }
             
                
			}
			catch (Exception e)
			{

                if (e.GetType() == typeof(ClientException))
                {

                    int code = ((ClientException)e).Code;

                    if (code == ErrorCodes._002_RequestConnecrionDiscard ||
                        code == ErrorCodes._019_TradeSystemIsUnavailable)
                    {
                        //_connector.SetState(_connector.StateDisconnected);
                        Error("Server disconnected. Error: "+ e.Message);
                        SetState(_connector.StateDisconnected);

                    }

                }
                else //unknown exception throw it 
                {
                    throw e;

                }


			
			}

		}
        public void TryDisconnect()
        {
            try
            {
                Log("Try disconect");
                //tell connector disconnect
                _connector.Disconnect();
            }
            catch (Exception e)
            {
               Error("Unable disconnect",e);
            }
          

            Log("Disconnected");
       
            SetState(_connector.StateOff);

        }



        public  override void  RequestDisconnect()
        {
           
            //this use for Connector's MainLoop in antother process  exit

            if (!_connector.RequestDisconnect)
                _connector.RequestDisconnect = true;
        }


	}
}
