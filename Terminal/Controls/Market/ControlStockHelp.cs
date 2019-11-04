using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Logger;

using Terminal.Common;

using System.Diagnostics;


namespace Terminal.Controls.Market
{
	public partial class ControlStock
	{

		Stopwatch sw = new Stopwatch();

		Stopwatch sw1 = new Stopwatch();
		Stopwatch sw11 = new Stopwatch();


		Stopwatch sw2 = new Stopwatch();
		Stopwatch sw3 = new Stopwatch();
		Stopwatch sw4 = new Stopwatch();
		Stopwatch sw5 = new Stopwatch();
		Stopwatch sw6 = new Stopwatch();
		Stopwatch sw7 = new Stopwatch();





		private void Log(string message)
		{
			if (_logger == null)
			{
				if (TickerName != null && TickerName != Literals.Undefind)
				{
                    //changed 2018_04_23
					_logger = new CLogger("ControlStock_ " + TickerName);
					_logger.Log(message);
				}
			}
			else
				_logger.Log(message);


		}


		private void InitStopWatchs()
		{
			sw.Stop(); sw.Reset(); sw.Start();

			sw1.Stop(); sw1.Reset(); sw1.Start();
			sw11.Stop(); sw11.Reset(); sw11.Start();

			sw2.Stop(); sw2.Reset(); sw2.Start();
			sw3.Stop(); sw3.Reset(); sw3.Start();
			sw4.Stop(); sw4.Reset(); sw4.Start();
			sw5.Stop(); sw5.Reset(); sw5.Start();
			sw6.Stop(); sw6.Reset(); sw6.Start();
			sw7.Stop(); sw7.Reset(); sw7.Start();

		}






	}
}
