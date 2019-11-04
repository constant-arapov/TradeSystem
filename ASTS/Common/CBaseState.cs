using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ASTS.Interfaces.Clients;
using ASTS.Connector;

namespace ASTS.Common
{
	public abstract class CBaseState
	{
		protected string _name;
		protected ICleintState  _client;

		public string Name
		{
			get
			{
				return _name;
			}
		}
		

		public CBaseState(ICleintState client)
		{
			_client = client;
			_name = GetType().Name.ToString();
		}

		protected void Log(string message)
		{
			string msgFrormatted = String.Format("[{0}] {1}", _name, message);
			_client.Log(msgFrormatted);			
		}

        protected void Error(string msg, Exception e = null)
        {
            _client .Error(msg, e);
        }

	

        protected void LogState(CBaseState newState)
        {
            string msg = String.Format("+++++++++++++++++++++++  STATE CHANGED  {0} =======> {1}", _name, newState.Name);
            Log(msg);

            _client.LogState(msg);

        }
		

		public virtual void RequestDisconnect()
		{


		}
		 
	}
}
