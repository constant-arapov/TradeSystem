using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;





namespace P2ConnectorNativeImp
{
	public class CStubDealingServer : IPlaza2ConnectorNativeClient
	{

		public void Error(string description, Exception exception = null)
		{
			throw new ApplicationException(description);
		}

		public CStubDealingServer()
		{

			
		}

		public void Process()
		{
			try
			{
				CP2ConnectorNative p = new CP2ConnectorNative(this);

			}
			catch (Exception e)
			{

			}

		}



	}
}
