using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


using Common.Interfaces;
using Common;
using Common.Logger;


using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;

namespace TradingLib.Snapshoters
{
    /// <summary>
    /// Base class for DealsDispatcher and StockDispatcher.
    /// Periodically check for date of last update and if
    /// it is expires call specific method UpdateAlgorithm in
    /// derived class. Derived class call update date 
    /// to save last date to update.
    /// </summary>


    public class CBaseSnapshoter :  IAlarmable, ILogable
    {

        protected /*CPlaza2Connector*/IClientSnapshoter _plaza2Connector;
        protected CLogger _logger;

        protected Dictionary<string, DateTime> _dictLastUpdate = new Dictionary<string, DateTime>();
        protected int _paramUpdInterval = 50;
        protected int _paramSleep = 1;//100;
        protected string _nameUpdater = "unknown";



        public CBaseSnapshoter(/*CPlaza2Connector*/IClientSnapshoter plaza2Connector, string nameUpdater, int paranUpdateInterval)
        {

            _nameUpdater = nameUpdater;
            _plaza2Connector = plaza2Connector;
            _logger = new CLogger(_nameUpdater);
            _paramUpdInterval = paranUpdateInterval;

            foreach (KeyValuePair<string, long> kvp in plaza2Connector.Instruments.DictInstrument_IsinId)            
                _dictLastUpdate[kvp.Key] = new DateTime(0);
            

          
            
        }
        protected virtual void UpdateAlgorithm(string isin)
        {


        }

        
        


        /// <summary>
        /// Periodically requests last time of instrument updates
        /// and call UpdateAlgorithm method. This method determine
        /// need of updating and if need update it.
        /// </summary>
        //TO DO try use sleep again to decrease cpu load
        protected void ThreadFunc()
        {

            while (true)
            {

                try
                {

                    for (int i = 0; i < _dictLastUpdate.Count; i++)
                    {
                        string isin = _dictLastUpdate.Keys.ElementAt(i);
                        UpdateAlgorithm(isin);
                    }
                   // if (!_plaza2Connector.GlobalConfig.IsTradingServer)
                        Thread.Sleep(_paramSleep);


                }
                catch (Exception e)
                {
                    Error(_nameUpdater+".ThreadFunc", e);

                }
            }
        }

        protected bool IsTimeToUpdate(string isin)
        {

            double dt = (DateTime.Now - _dictLastUpdate[isin]).TotalMilliseconds;


            if (dt > _paramUpdInterval)
                return true;
            else
                return false;


        }


        protected void UpdateDate(string isin)
        {
            _dictLastUpdate[isin] = DateTime.Now;

        }
      /*  protected virtual void DoUpdate(string isin)
        {


        }
        */

        public void Error(string msg, Exception e = null)
        {

            _plaza2Connector.Error(msg, e);

        }

        public void Log(string msg)
        {
            _logger.Log(msg);

        }


    }
}
