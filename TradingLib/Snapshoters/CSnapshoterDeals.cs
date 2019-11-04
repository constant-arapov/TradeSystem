using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.TradersDispatcher;


namespace TradingLib.Snapshoters
{
    public class CSnapshoterDeals : CBaseSnapshoter
    {


		public CSnapshoterDeals(IClientSnapshoter plaza2Connector, string nameUpdater, int updateInterval) 
            :base ((IClientSnapshoter)plaza2Connector, nameUpdater, updateInterval)
        {

            if (_plaza2Connector.GlobalConfig.IsTradingServer)
                        (new Thread(ThreadFunc)).Start();

        }

   

        protected override void UpdateAlgorithm(string isin)
        {

            if (IsTimeToUpdate(isin))
            {                    
                    if (_plaza2Connector.GlobalConfig.IsTradingServer)
                        _plaza2Connector.UpdateTradersDeals(isin);
                    
                    UpdateDate(isin);
                
            }



        }




    }
}
