using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;


using Common;
using Common.Interfaces;
using Common.Collections;
using Common.Utils;

using TradingLib.Interfaces.Interaction;

using ASTS.Interfaces.Interactions;


namespace ASTS.Connector
{
	public class CASTSConnectorDouble : CBaseASTSConnector
	{

		private CASTSConnection _astsConectionData;



        public override string Password
        {
            get
            {
                throw new ApplicationException("Password not defined in code yet");
                return "";
            }

        }

        public override bool IsConnectedToServer { get; set; }
      




		public CASTSConnectorDouble(IDealingServerForASTSConnector dealingServer)
			: base(dealingServer)
		{

			_queueTransactions = new CBlockingQueue<Action>();

			CUtil.ThreadStart(ProcessMainThread);
			CUtil.ThreadStart(ProcessDataThread);

			

		}

		public void ProcessMainThread()
		{


			_astsConnectionMain = new CASTSConnection(_confASTSConnector, _dealingServer, false,"ConnectionMain");

			_astsConnectionMain.Connect();
			MainConnectionLoop();
			_astsConnectionMain.Disconnect();
		}


		public void ProcessDataThread()
		{


            _astsConectionData = new CASTSConnection(_confASTSConnector, _dealingServer, true, "ConnectionData");

			_astsConectionData.Connect();
			DataConnectionLoop();
			_astsConectionData.Disconnect();
		}

      



		private void MainConnectionLoop()
		{
			try
			{
				DateTime dtLastTransact = DateTime.Now;
				double parSleepMs = 100;

				while (true)
				{
					//blocking get
					Action act =  _queueTransactions.Get();
					double dt = parSleepMs- (DateTime.Now - dtLastTransact).TotalMilliseconds;
					if (dt > 0)
						Thread.Sleep((int)dt);

					act.Invoke();
					Log("Transaction processed. dt="+dt);
					dtLastTransact = DateTime.Now;
				}
			}
			catch (Exception e)
			{
				Error("CASTSConnectorDouble.MainConnectionLoop", e);

			}

		}

		private void DataConnectionLoop()
		{
			try
			{
				DateTime dtStart;
				int parSleep = 100;
				int parMaxLength = 30000 * 1024; //30 kB
				//for (int i = 0; i < 100; i++)
				while (true)
				{
					dtStart = DateTime.Now;







					int len = _astsConectionData.RequestData();


					int dt = parSleep - (DateTime.Now - dtStart).Milliseconds;

					Log("RequestData. len=" + len);
					if (dt > 0)
						if (len <= parMaxLength)
							Thread.Sleep(dt);

				}
			}
			catch (Exception e)
			{
				Error("CASTSConnectorDouble.DataConnectionLoop", e);
			}

		}



		private StringDictionary GetConnectionMainParams()
		{


			StringDictionary parameters = new StringDictionary();
			//TODO from config
			parameters.Add("Interface", "IFCBroker_27");

			parameters.Add("Server", "EQ_TEST");
			parameters.Add("Host", "127.0.0.1:15005");
			parameters.Add("Feedback", "constant_arapov@bk.ru");
			parameters.Add("Boards", "TQBR");

			parameters.Add("UserId", "MU9013100003");
			parameters.Add("Password", "8213");

			return parameters;

		}

		private StringDictionary GetConnectionDataParams()
		{


			StringDictionary parameters = new StringDictionary();
			//TODO from config
			parameters.Add("Interface", "IFCBroker_27");

			parameters.Add("Server", "EQ_TEST");
			parameters.Add("Host", "127.0.0.1:15005");
			parameters.Add("Feedback", "constant_arapov@bk.ru");
			parameters.Add("Boards", "TQBR");

			parameters.Add("UserId", "MU9013100004");
			parameters.Add("Password", "8213");

			return parameters;



		}





	}
}
