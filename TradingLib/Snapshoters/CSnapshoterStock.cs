using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Common;
using Common.Interfaces;

using TradingLib;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Bots;
using TradingLib.BotEvents;
using TradingLib.TradersDispatcher;

using TradingLib.Enums;

namespace TradingLib.Snapshoters
{
    public class CSnapshoterStock :  CBaseSnapshoter, ISnapshoterStock
    {

      
        protected Dictionary<string, CSharedStocks> _inpStocks = new Dictionary<string, CSharedStocks>();
        protected Dictionary<string, CSharedStocks> _outpStocks = new Dictionary<string, CSharedStocks>();

        Dictionary<string, DateTime> _dtLastUpdateNotTradingTime = new Dictionary<string, DateTime>();

        private int _parIntervalNotTradingTime = 1000;
        
        public Dictionary<string, CSharedStocks> OutputStocks
        {
            get
            {
                return _outpStocks;
            }
        }


        private IClientSnapshoter _client;

     
        public  CSnapshoterStock(IClientSnapshoter client, string nameUpdater,  int stockDept, int updateInterval) :
                    base(client, nameUpdater, updateInterval)
        {
            _client = client;
           
            foreach (KeyValuePair<string, long> kvp in client.Instruments.DictInstrument_IsinId)
            {
                string isin = kvp.Key;


                _inpStocks[isin] = new CSharedStocks(_client);
                _outpStocks[isin] = new CSharedStocks(_client);
                _dtLastUpdateNotTradingTime[isin] = DateTime.Now;
                     
            }

            (new Thread(ThreadFunc)).Start();
                 
        }


        public void UpdateNotTradingTimeDate(string isin)
        {
            _dtLastUpdateNotTradingTime[isin] = DateTime.Now;
        }



        public bool IsTimeToUpdateNotOnTradingTime(string isin)
        {
            if ((DateTime.Now - _dtLastUpdateNotTradingTime[isin]).TotalMilliseconds > _parIntervalNotTradingTime)
                return true;

            return false;
        }



        /// <summary>
        /// Specific algorithm for periodically update traders stock
        /// </summary>
        /// <param name="isin"></param>
        protected override void UpdateAlgorithm(string isin)
        {
           
            if (IsTimeToUpdate(isin))
            {

                CSharedStocks inpStk = _inpStocks[isin];


                if ((_plaza2Connector.IsSessionOnline && _plaza2Connector.UseRealServer) ||
                    !_plaza2Connector.UseRealServer)
                    
                {

                    if (inpStk.IsStocksDifferent(_outpStocks[isin]) ||   //stock changed from last update
                        !_plaza2Connector.IsSessionActive &&  IsTimeToUpdateNotOnTradingTime(isin) ||// time to update when session not active
                        !_plaza2Connector.UseRealServer  && _plaza2Connector.IsStockOnline            //demo server and stock online
                        )
                    {

                        UpdateOutStock(isin, bDoNotUpdateTraders:false);
                        UpdateDate(isin);
                        UpdateNotTradingTimeDate(isin);

                    }
                }
            }



        }


       
		/// <summary>
		/// Tell bot that it need to take data from OutputStocks and 
		/// put it to their internal structures
		/// </summary>		
        private void TriggerBotsStockUpdate(string isin)
        {
            try
            {
			
                foreach (CBotBase bb in _plaza2Connector.ListBots)
                    if (bb.SettingsBot.ListIsins.Contains(isin))
                    {
                        BotEventStock bevs = null;
                        bb.Recalc(isin, EnmBotEventCode.OnStockUpdate, (object)bevs);

                    }
            
            }
            catch (Exception e)           
            {
                Error("Error in TriggerStockUpdate",e);

            }          
        }



        private void TriggerTradersStocksUpdate(string isin)
        {

            try
            {
                _plaza2Connector.UpdateTradersStocks(isin);

            }

            catch (Exception e)
            {

                Error("Error in TriggerTradersStocksUpdate");
            }



        }
        /// <summary>
		/// Copy data from sourceStock to _inpStocks
		/// 
		/// Call from:
        /// StockConverter.ThreadStockConverter (old)
        /// P2StockConverterP2.UpdateStocksFromNative
        /// </summary>      
        public void  UpdateInpStocksBothDir(string isin, ref CSharedStocks sourceStock,
                                    int precision)
        {

            if (!_inpStocks[isin].CopyStocksBothDir( ref sourceStock, precision))
                Error("Unable copy stock both dir");
          
        }

        public void UpdateInpStocksOneDir(string isin, ref CSharedStocks sourceStock, Direction dir,
                                   int precision)
        {

            if (!_inpStocks[isin].CopyStocksOneDir(ref sourceStock,dir, precision))
                Error("Unable copy stock one");

        }





        /// <summary>
        /// 1)Copy to _outpStocks from _inpStocks
        /// 2) Trigger update.		 
        /// 
        /// 2.1 Update Bot's stocks - in every case call (see "call from")
        /// 2.2 Update trader's stock only periodically
        /// 
        /// 
        /// Call from 
        /// - StockDispatcher.UpdateAlgorithm - periodically update
        /// - StockProcessor.ThreadStockConverter - update if bid/ask changed
        /// </summary>		
        public void UpdateOutStock(string isin, bool bDoNotUpdateTraders)
        {
            CSharedStocks ss = _inpStocks[isin];

            if (!_outpStocks[isin].CopyStocksFull(ref ss))
                Error("Unable update outstock");
            else //was copied successfull
            {
                //trigger all bots for update specific stock - 2018-01-11 moved
                //TriggerBotsStockUpdate(isin);
                if (!bDoNotUpdateTraders)
                    if (_plaza2Connector.GlobalConfig.IsTradingServer)
                    {
                        //2018-01-11 moved here for optimisation reasons
                        TriggerBotsStockUpdate(isin);
                        TriggerTradersStocksUpdate(isin); //trigger stock dispacher update trader's stocks
                    }
            }
            
        }

     


    }

   
}
