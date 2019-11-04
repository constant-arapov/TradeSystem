using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Data;

using System.Diagnostics;


namespace zTest.Crypto
{
	public class TestUserOrderLog
	{


		CUserOrderLog _userOrderLog = new CUserOrderLog();
		Random _random = new Random();



		public void Test()
		{
			Stopwatch sw1 = new Stopwatch();
			Stopwatch sw2 = new Stopwatch();


			List<string> lstInstruments = new List<string>();

			lstInstruments.Add("tBTCUSD");
			lstInstruments.Add("tLTCUSD");
			lstInstruments.Add("tETHUSD");
			lstInstruments.Add("tZECUSD");
			lstInstruments.Add("tXMRUSD");
			//_lstInstruments.Add("tDASHUSD");
			//_lstInstruments.Add("tIOTAUSD");
			lstInstruments.Add("tEOSUSD");
			lstInstruments.Add("tSANUSD");
			lstInstruments.Add("tOMGUSD");
			lstInstruments.Add("tBCHUSD");
			lstInstruments.Add("tNEOUSD");
			//  _lstInstruments.Add("tUTPUSD");
			//_lstInstruments.Add("tQTUMUSD");
			lstInstruments.Add("tEDOUSD");
			lstInstruments.Add("tAVTUSD");




			

			sw1.Restart();


			for (int i = 1; i < 1000; i++)
			{
				int botId = _random.Next(100, 10000);

				int iInstr = _random.Next(0, lstInstruments.Count - 1);
				
				
				CUserOrder userOrder = new CUserOrder
				{
					Amount = 1234,
					BotId = botId,
					OrderId = _random.Next(10000000, 20000000),
					Instrument = lstInstruments[iInstr]
				};

				_userOrderLog.Update(userOrder);



			}
			
			
			sw1.Stop();

			long ordIdCtrl =  _random.Next(10000000, 20000000);

			CUserOrder userOrderCntrl = new CUserOrder
			{
				Amount = 1234,
				BotId = 5000,
				OrderId = ordIdCtrl,
				Instrument = "tBTCUSD"
			};

			_userOrderLog.Update(userOrderCntrl);



			sw2.Restart();
			int botIdTest = _userOrderLog.GetBotId("tBTCUSD", ordIdCtrl);
			sw2.Stop();

			if (botIdTest != 0)
				System.Threading.Thread.Sleep(0);
		




		}






	}
}
