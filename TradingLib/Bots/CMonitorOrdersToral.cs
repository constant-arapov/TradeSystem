using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.ProtoTradingStructs;
using TradingLib.Enums;

namespace TradingLib.Bots
{
	
	//note: thread unsafe
    /// <summary>
    /// instrument - dir - price - amount
    /// </summary>
	public class CMonitorOrdersAllTotal : Dictionary<string,Dictionary<EnmOrderDir,Dictionary<decimal,decimal>>>
	{
		public void UpdateFully(string instrument,  Dictionary<string, Dictionary<long, COrder>>monitorOrdersAll)
		{

			if (!this.ContainsKey(instrument))
				this[instrument] = new Dictionary<EnmOrderDir, Dictionary<decimal, decimal>>();
			else
				this[instrument].Clear();

			lock (monitorOrdersAll)
			{
				foreach (var kvp in monitorOrdersAll)
				{
					string instr = kvp.Key;
					foreach (var kvp2 in kvp.Value)
					{
						COrder order = kvp2.Value;
						Update(instrument, order);

					}


				}
				
			}
			
		}

		
		
		public void Update(string instrument, COrder order)
		{
			

			if (!this[instrument].ContainsKey((EnmOrderDir)order.Dir))
				this[instrument] = new Dictionary<EnmOrderDir, Dictionary<decimal, decimal>>()
					{
						{ (EnmOrderDir)order.Dir, new Dictionary<decimal, decimal>()}
					};

			if (!this[instrument][(EnmOrderDir)order.Dir].ContainsKey(order.Price))
				this[instrument][(EnmOrderDir)order.Dir][order.Price] = 0;




			this[instrument][(EnmOrderDir)order.Dir][order.Price] += order.Amount;


		

			


		}



		
	}
}
