using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Enums;

namespace TradingLib.Data
{
	public class CUserOrderLog : Dictionary<int, Dictionary<string,List<CUserOrder>>>
	{

		public void Update(CUserOrder userOrder)
		{
			if (!this.ContainsKey(userOrder.BotId))			
				this[userOrder.BotId] = new Dictionary<string, List<CUserOrder>>();

			if (!this[userOrder.BotId].ContainsKey(userOrder.Instrument))
				this[userOrder.BotId][userOrder.Instrument] = new List<CUserOrder>();

			CUserOrder resUserOrder = this[userOrder.BotId][userOrder.Instrument].Find(el => el.OrderId == userOrder.OrderId);

			//not found do add
			if (resUserOrder == null)
				this[userOrder.BotId][userOrder.Instrument].Add(userOrder);
			else
			{
				
				resUserOrder.Status = userOrder.Status;
				resUserOrder.OrderActionLast = userOrder.OrderActionLast;
				//TODO update other fields
			}


		}


		public int GetBotId(string instrument,long orderId)
		{

			//CUserOrder resUserOrder = this[userOrder.BotId][userOrder.Instrument].Find(el => el.OrderId == userOrder.OrderId);

			foreach (var kvp in this)
			{
				if (!kvp.Value.ContainsKey(instrument))
					continue;

				foreach (var lst in kvp.Value.Values)
				{
					var res = lst.Find(el => el.OrderId == orderId);
					if (res != null)
						return kvp.Key; //botId

				}


			}

			//throw new ApplicationException("CUserOrderLog.botId not found");
			return -1;//not found

		}


		public void Delete(int botId, string instrument, long orderId)
		{
			if (!this.ContainsKey(botId))
				return;

			if (!this[botId].ContainsKey(instrument))
				return;


			int ind = this[botId][instrument].FindIndex(el => el.OrderId == orderId);
			this[botId][instrument].RemoveAt(ind);

		}



        public void Cleaning()
        {
            int parMaxSize = 100;

            int ind = -1;
            bool bFound = false;
            int botId = -1;
            string instrument = "";

            foreach (var kvp in this)
            {
                botId = kvp.Key;
                foreach (var kvp2 in kvp.Value)
                {
                    instrument = kvp2.Key;
                    
                    if (kvp2.Value.Count > parMaxSize)
                    {
                        ind = this[botId][instrument].FindIndex(ord => ord.OrderActionLast == EnmOrderAction.Deleted);
                        //2018-11-13
                        if (ind > 0)
                        {
                            bFound = true;
                            break;
                        }
                    }
                                                                
                }

                if (bFound)
                    break;

            }


            if (bFound)            
                this[botId][instrument].RemoveAt(ind);

            

        }

        public void CleaningFull()
        {
            foreach (var kvp in this)
            {
                kvp.Value.Clear();

            }
            


        }









	}
}
