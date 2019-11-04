using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Logger;

using TradingLib.Data.DB;
using TradingLib.Interfaces.Components;

using BitfinexRestConnector.Messages.v1.Response;



namespace CryptoDealingServer.Helpers
{
    class CTradeHistStor : Dictionary<string, List<ResponceMyTrades> > 
    {
        
        private CLogger _logger;

        private int _parMaxHistBuffSz = 1000;

        private IDBCommunicator _dbCommunicator;
        

        public CTradeHistStor(List<string> initInstrLists, IDBCommunicator dbCommunicator)
        {
            foreach (var instr in initInstrLists)            
                this[instr] = new List<ResponceMyTrades>();


            _logger = new CLogger("CTradeHistStor");

           

            _dbCommunicator = dbCommunicator;


        }


        public double GetFee(string instrument, long tid)
        {


            if (!this.ContainsKey(instrument))
                return 0;

            lock(this[instrument])
            {
                var trade = this[instrument].Find(el => el.tid == tid);
                if (trade != null)
                    return Convert.ToDouble(trade.fee_amount);
                else
                    return 0;

            }


          

        }


        public void OnRcvdNewHistory(string instrument, ResponceMyTrades[] arrRespMyTrades)
        {
            if (!this.ContainsKey(instrument))
                this[instrument] = new List<ResponceMyTrades>();


           

          


            for (int i= arrRespMyTrades.Length-1; i >= 0; i--)                          
                Update(instrument, arrRespMyTrades[i]);


            RemoveOldEls(instrument);



            var linesInstr = GetLstString(instrument);

            Log(String.Format("==== Begin processing ===== {0} ===============================", instrument));
            foreach (var el in linesInstr)
                Log(el);

            
            Log(String.Format("=== End processing ===== {0} ===============================", instrument));


        }


        private void Update(string instrument,ResponceMyTrades responceMyTrades)
        {
          
            lock (this[instrument])
            {
                //not exists
                if (this[instrument].Find(el => el.tid == responceMyTrades.tid) == null)
                {                    
                    this[instrument].Add(responceMyTrades);
                    _dbCommunicator.QueueData(new CDBInsertBfxTrades
                    {
                        TradeId = responceMyTrades.tid,
                        OrderId = responceMyTrades.order_id,
                        Amount = responceMyTrades.amount,
                        Price = responceMyTrades.price,
                        Fee = responceMyTrades.fee_amount

                    }
                                             );
                }
                               
            }
             
        }


        private void RemoveOldEls(string instrument)
        {
           

            lock(this)
            {
                int sz = this[instrument].Count;
                while (sz > _parMaxHistBuffSz)
                {
                    this[instrument].RemoveAt(0);
                    sz = this[instrument].Count;
                }



            }

        }




        private List<string> GetLstString(string instrument)
        {
            List<string> lst = new List<string>();

            lock (this[instrument])
            {
                foreach(var el in this[instrument])                
                    lst.Add(el.ToString());               
            }
            return lst;
        }


        private void Log(string msg)
        {
            _logger.Log(msg);
        }


      




    }
}
