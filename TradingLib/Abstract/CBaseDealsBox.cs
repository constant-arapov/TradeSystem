using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Interfaces.Clients;

using TradingLib.Interfaces.Components;
using TradingLib.Data;
using TradingLib.Data.DB;

namespace TradingLib.Abstract
{
    public abstract class CBaseDealBox : IDealBox
    {

        protected IClientDealBox _client;


        protected Dictionary<string, CDealsStruct> m_dealsStructList = new Dictionary<string, CDealsStruct>();

        public Dictionary<string, CDealsStruct> DealsStruct
        {
            get
            {
                return m_dealsStructList;
            }

        }



        public CBaseDealBox(IClientDealBox client)
        {

            _client = client;
            CreateDealsStructDict();
        }




        public void UpdateAllDealStructLastData()
        {
            foreach (var v in m_dealsStructList)
            {

                v.Value.UpdateAllLastDealData();

            }


        }


        public void Error(string description, Exception exception = null)
        {

            _client.Error(description, exception);
        }


        public void CreateDealsStructDict()
        {

            //TO DO change to string
            try
            {

                //   foreach (KeyValuePair<string, long> pair in m_plaza2Connector.Instruments.DictInstrument_IsinId)
                //     m_dealsStructList.Add(pair.Key, new CDealsStruct(/*pair.Value,*/ pair.Key, m_plaza2Connector.ListBots, m_plaza2Connector));


                //2017-05-05

                foreach (CDBInstrument dbInsr in _client.Instruments)
                    m_dealsStructList.Add(dbInsr.instrument,
                        new CDealsStruct(/*pair.Value,*/ /*pair.Key*/dbInsr.instrument, _client.ListBots, _client));



            }
            catch (Exception e)
            {
                Error("CDealBox.CreateDealsStructDict", e);


            }


        }


    }
}
