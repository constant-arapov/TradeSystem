using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;


using Common.Interfaces;
using Common.Utils;

using ASTS.Interfaces;
using ASTS.Interfaces.Interactions;
using ASTS.DealingServer;

namespace ASTS.Tables
{
    public class CTableTrades : CBaseTable
    {
        private CUserDealsPosBoxASTS _userDealsPosBoxASTS;


		IDealingServerForTrades _dealingServer;
		//TODO move to base class
		private object _lck = new object();
		private DateTime _dtLastData;
		private int _parMsSinceLastUpdate = 300;
		private bool _bRecordRecieved = false;
		

        public CTableTrades(string name, IDealingServerForTrades dealingServer)
            : base(name, dealingServer)
        {

            _userDealsPosBoxASTS = dealingServer.UserDealsPosBoxInp;
			_dealingServer = dealingServer;
			CUtil.TaskStart(TaskWaitDataRecievedOnConnection);

        }

        protected override void ProcessRecord()
        {
			base.ProcessRecord();

			while (!_dealingServer.IsDealsPosLogLoadedFromDB)
				Thread.Sleep(100);


            string instrument =  Convert.ToString( _currentRecord.values ["SECCODE"]);
            decimal price = Convert.ToDecimal(_currentRecord.values["PRICE"]);
            char buySell = Convert.ToChar(_currentRecord.values["BUYSELL"]);
            int amount = Convert.ToInt32(_currentRecord.values["QUANTITY"]);
            long extId =  Convert.ToInt64(_currentRecord.values["EXTREF"]);
            long tradeNo = Convert.ToInt64(_currentRecord.values["TRADENO"]);
            string tradeDate = Convert.ToString(_currentRecord.values["TRADEDATE"]);
            string tradeTime = Convert.ToString(_currentRecord.values["TRADETIME"]);
            long microsecs = Convert.ToInt32(_currentRecord.values["MICROSECONDS"]);
            decimal fee = Convert.ToDecimal(_currentRecord.values["COMMISSION"]);

            _userDealsPosBoxASTS.Update(instrument, price, buySell,
                                         amount, extId,tradeNo,
                                         tradeDate, tradeTime, microsecs,
                                          fee);


			lock (_lck)
			{
				_dtLastData = DateTime.Now;

			}

			_bRecordRecieved = true;
        }
		//Added 2017-10-21 
		private void TaskWaitDataRecievedOnConnection()
		{



			while (true)
			{
				lock (_lck)
				{
					if (_bRecordRecieved)
						if ((DateTime.Now - _dtLastData).TotalMilliseconds > _parMsSinceLastUpdate)
							break;
				}

				Thread.Sleep(50);				
			}			
			_dealingServer.IsOnlineUserDeals = true;
		}



    }
}
