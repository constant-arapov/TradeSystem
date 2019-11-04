using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Interfaces;
using Common.Collections;

using TradingLib.Interfaces.Clients;

using System.Threading;

using TradingLib.Interfaces;
using TradingLib.Data;


namespace TradingLib.GUI
{
    public class CUserDealsCollection : Dictionary<string, CObservableIdCollection</*CGUIUserDealViewModel*/IIDable<long>, long>>  
    {
        public delegate void DealsUpdateUpdateDelegate(string isin, long botId, CObservableIdCollection<IIDable<long>, long> userDealsCol);
        public event DealsUpdateUpdateDelegate DealsUpdateEvent;

        private long m_botId { get; set; }

        public Mutex mx = new Mutex ();



        private /*CPlaza2Connector*/IClientUserDealsCollection m_plaza2Connector;



        public CUserDealsCollection(long botId, /*CPlaza2Connector*/IClientUserDealsCollection plaza2Connector)
        {

            m_botId = botId;
            m_plaza2Connector = plaza2Connector;
            
        }
        public void Add(CRawUserDeal rd, string isin)
        {
			mx.WaitOne();

            if (!this.ContainsKey(isin))
                this[isin] = new CObservableIdThradeSafeCollection<IIDable<long>, long>();


            decimal parHeight = 70;
            string botName = GetBotName(m_botId);
            this[isin].UpdateWithId(new CGUIUserDealViewModel (rd,  parHeight, botName));
            
           
            NotifyCollectionChanged(isin, m_botId);
			mx.ReleaseMutex();

        }
        private string GetBotName(long botId)
        {
            string botName = "unknown";
            if (m_plaza2Connector.DictBots.ContainsKey(botId))
              botName = m_plaza2Connector.DictBots[botId].Name;
            
            return botName;

        }



        public void NotifyCollectionChanged(string isin, long botId)
        {

           


            if (DealsUpdateEvent != null)
                DealsUpdateEvent(isin,botId, this[isin]);

        }

    }
}
