using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using CryptoDealingServer.Components;

namespace zTest.Crypto
{
	public class TestOrderTracker
	{

		COrderStatusTracker _orderStatusTracker = new COrderStatusTracker();

		

		public void Test()
		{

			//long history at one time
			_orderStatusTracker.Update(100,
									   9062207533,
									   "EXECUTED @ 1.6518(1.0): was PARTIALLY FILLED @ 1.65(6.0), PARTIALLY FILLED @ 1.6517(6.0), PARTIALLY FILLED @ 1.6517(6.0)");



			//just executed one time
			_orderStatusTracker.Update(100,
										9062207534,
										"EXECUTED @ 1.6518(1.0)");


			//partialy step by step
			//step 1
			_orderStatusTracker.Update(100,
									   9062207535,
									   "PARTIALLY FILLED @ 1.65(6.0)");


			//step 2
			_orderStatusTracker.Update(100,
									   9062207535,
									   "PARTIALLY FILLED @ 1.6517(6.0): was PARTIALLY FILLED @ 1.65(6.0)");


			//step 3
			_orderStatusTracker.Update(100,
									   9062207535,
									   "PARTIALLY FILLED @ 1.6517(6.0): was PARTIALLY FILLED @ 1.65(6.0), PARTIALLY FILLED @ 1.6517(6.0)");


			//step 4
			_orderStatusTracker.Update(100,
								   9062207535,
								   "EXECUTED @ 1.6518(1.0): was PARTIALLY FILLED @ 1.65(6.0), PARTIALLY FILLED @ 1.6517(6.0), PARTIALLY FILLED @ 1.6517(6.0)");




		}

	


	}
}
