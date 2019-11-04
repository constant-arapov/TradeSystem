using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using TradingLib.Interfaces.Clients;



namespace TradingLib.Snapshoters
{
    public class CSnapshoterStockSmart : CSnapshoterStock
    {
        public CSnapshoterStockSmart(IClientSnapshoter client, string nameUpdater, int stockDept, int updateInterval) :
                   base(client, nameUpdater, stockDept, updateInterval)
        {



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
                        !_plaza2Connector.IsSessionActive && IsTimeToUpdateNotOnTradingTime(isin) ||// time to update when session not active
                        !_plaza2Connector.UseRealServer && _plaza2Connector.IsStockOnline            //demo server and stock online
                        )
                    {

                        UpdateOutStock(isin, bDoNotUpdateTraders: false);
                        UpdateDate(isin);
                        UpdateNotTradingTimeDate(isin);

                    }
                }
            }



        }




    }
  
}
