using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Data;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Interaction;
using TradingLib.Abstract;

using Common.Interfaces;



namespace Plaza2Connector
{
    public class CDealBox : CBaseDealBox, IDealBox, IDealBoxForP2Connector, IAlarmable
    {

        


     

       


        public CDealBox(IClientDealBox client) : base(client)
        {
            



        }

        



        public void UpdateDeals (DEALS.deal dealInp)
        {

            try
            {

                int isin_id = dealInp.isin_id;
                string isin = _client.Instruments.GetInstrumentByIsinId(isin_id);
                
                m_dealsStructList[isin].Update(dealInp);
            }

            catch (Exception e)
            {
                string st = e.Message;
                Error("UpdateDeals",e);
            }


        }

        //for FX
        public void UpdateDeals(string instrument, AstsCCTrade.ALL_TRADES at)
        {
            m_dealsStructList[instrument].Update(at);


        }


		public void SimUpdateDeals(CRawDeal dealInp)
		{
			try
			{
				int isin_id = (int)dealInp.Isin_id;
				string isin = _client.Instruments.GetInstrumentByIsinId(isin_id);
				
				//TODO normal
			//	if (m_dealsStructList.ContainsKey(isin))
					m_dealsStructList[isin].Update(dealInp);
			}
			catch (Exception e)
			{
				Error("CDealsBox.SimUpdateDeals", e);

			}

		}



      



    }
}
