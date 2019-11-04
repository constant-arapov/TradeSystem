using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Text.RegularExpressions;


using Common.Logger;

using BitfinexCommon;
using BitfinexCommon.Enums;

using TradingLib.Enums;


namespace CryptoDealingServer.Components
{
	public class COrderStatusTracker : Dictionary<int,Dictionary<long, CLstHistOrdStatus>>
	{

		private CLogger _logger;


		public COrderStatusTracker()
		{
			_logger = new CLogger("COrderStatusTracker");
		}


		public void Update(int botId, long orderId, string newStatus)
		{
			CLstHistOrdStatus newLstHistOrdStatus = GetLstHistOrdStatus(newStatus);

			if (!this.ContainsKey(botId))
				this[botId] = new Dictionary<long, CLstHistOrdStatus>();

			//no historical data  for order - just create and get out
			if (!this[botId].ContainsKey(orderId))
			{
				this[botId][orderId] = newLstHistOrdStatus;
				
			}
			//hist data for order exists - need update
			else
			{
				//copy elements that not exist from generated ListHistOrdStatus 
				//to existing ListHistOrderStatus
				int countExist = this[botId][orderId].Count;
				for (int i = countExist; i < newLstHistOrdStatus.Count; i++)				
					this[botId][orderId].Add(newLstHistOrdStatus[i]);
				

			}


			PrintOrderHistry(botId, orderId, newStatus);

		}





		public CLstHistOrdStatus GetLstHistOrdStatus(string newStatus)
		{
			CLstHistOrdStatus lstHistOrdStatus = new CLstHistOrdStatus();

			string[] stArr = newStatus.Split(':');

			if (stArr == null || stArr.Count() == 0)
				throw new ApplicationException("COrderStatusTracker.Update");

			//first do process "was" - history deals (if exists)
			if (stArr.Count() == 2)
			{
				string hist = stArr[1].Remove(0, 4);//remove " was "

				string[] histLst = hist.Split(',');

				foreach (var el in histLst)
				{
					COrderStateRecord order = ParseRecord(el);
					lstHistOrdStatus.Add(order);
				}
			}

			//then process "last" - current deal - put it to the end of history list
			lstHistOrdStatus.Add(ParseRecord(stArr[0]));

			return lstHistOrdStatus;
		}




		private COrderStateRecord ParseRecord(string stOneRecord)
		{
			

			string pat = @"\s*([\w\s]*)\s@\s([0-9\.]*)\(([0-9\.]*)\)";



			Regex reg = new Regex(pat);
			Match m = reg.Match(stOneRecord);

			if (m.Groups.Count != 4)
				throw new ApplicationException("COrderStatusTracker.ParseRecord");

			string status = m.Groups[1].ToString();
			decimal price = Convert.ToDecimal(m.Groups[2].ToString().Replace('.',','));
			decimal amount = Convert.ToDecimal(m.Groups[3].ToString().Replace('.', ','));
			 


			COrderStateRecord ordStatusRec = new COrderStateRecord();
			ordStatusRec.Price = price;
			ordStatusRec.Amount = amount;
			ordStatusRec.OrderStatus = CBfxUtils.GetOrderStatus(status);
			ordStatusRec.Dir = amount > 0 ? EnmDealDir.Buy : EnmDealDir.Sell; 
			return ordStatusRec;
		}



		private void PrintOrderHistry(int botId, long orderId,string newStatus)
		{
			Log("=========================================================================================");
			Log("botId = "+botId);
			Log("orderId="+orderId);
			Log(newStatus);
			foreach(var el in this[botId][orderId])			
				Log(string.Format("price={0} amount={1} dir={2} status={3}", el.Price, el.Amount, el.Dir, el.OrderStatus));

			Log("==========================================================================================");
		}

		private void Log(string msg)
		{
			_logger.Log(msg);
		}





	}

	




	public class CLstHistOrdStatus : List<COrderStateRecord>
	{



	}


	public class COrderStateRecord
	{

		public decimal Price { get; set; }
		public decimal Amount { get; set; }
		public EnmDealDir Dir { get; set; }
		public  EnmBfxOrderStatus OrderStatus { get; set; }


	}




}
