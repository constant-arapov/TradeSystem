using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Interfaces;
using Common.Logger;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Interaction;
using TradingLib.Abstract;
using TradingLib.Enums;
using TradingLib.Data;

using System.Diagnostics;
using TradingLib;

namespace Plaza2Connector
{
	public class CStockBox : CBaseStockBox, IStockBox, IStockBoxForP2Connector, IAlarmable, ILogable
    {


		protected Dictionary<string, CStockStruct> m_stocksStructDict = new Dictionary<string, CStockStruct>();

		public Dictionary<string, CStockStruct> StocksStructDict
		{
			get
			{

				return m_stocksStructDict;
			}


		}
      


		public  CStockBox(IClientStockBox client, int stockDepth) : base(client,stockDepth)
        {
			CreateStocksStructDict(stockDepth);                             
        }


		public override CBaseStockConverter GetStockConverter(string instrument)
		{
			return m_stocksStructDict[instrument].StockConverter;

		}

        



        public decimal GetBid(string isin)
        {
            //return GetBestPice(isin,OrderDirection.Buy);
            return m_stocksStructDict[isin].StockConverter.GetBid();
        }


        public decimal GetAsk(string isin)
        {
            //return GetBestPice(isin, OrderDirection.Sell);
            return m_stocksStructDict[isin].StockConverter.GetAsk();
            

        }

        public void ReInitAllStocks()
        {

            foreach (KeyValuePair<string, CStockStruct> kvp in StocksStructDict)
                kvp.Value.NeedReInitStock = true;



        }
     
        

		public bool IsStockAvailable(string instrument)
		{

			if (m_stocksStructDict == null || !m_stocksStructDict.ContainsKey(instrument))
				return false;

			return true;
		}



        protected  void CreateStocksStructDict(int stockDepth)
        {

			//KAA 2017-08-05
           /* foreach (KeyValuePair<string, long> pair in _client.Instruments.DictInstrument_IsinId)
                m_stocksStructDict.Add(pair.Key, 
                                        new CStockStruct(pair.Key, _client.ListBots, (IClientStockStruct) _client, stockNum));
            
           */

			foreach (var dbInstr in _client.Instruments)
			{
				m_stocksStructDict[dbInstr.instrument] = new CStockStruct(dbInstr.instrument, _client.ListBots, _client, stockDepth);

			}


        }


        public decimal GetBestPice(string isin, EnmOrderDir ordDir)
        {
             decimal val =0;
           
             try
             {
             
                 {
                     if ( EnmOrderDir.Buy == ordDir)
                         val = m_stocksStructDict[isin].StockConverter.GetBid();
                     else
                         val = m_stocksStructDict[isin].StockConverter.GetAsk();

                  


                 }
             }
             catch (Exception e)
             {
                 Error("GetBestPice", e);

             }

            return val;


        }

        
        //SPOT and currency update
        public void UpdateStock(string instrument, CRawStock rs)
        {

            m_stocksStructDict[instrument].Add(rs);

        }
        
        //Added 2017-24-11 for updating stock from native connector
        /// <summary>
        /// Call from
        /// CPlaza2Connector
        /// </summary>
        /// <param name="instrument"></param>
        /// <param name="sourceStock"></param>
        public void UpdateStockFromNative(string instrument, ref CSharedStocks sourceStock)
        {
            if (m_stocksStructDict.ContainsKey(instrument))
                m_stocksStructDict[instrument].StockConverterP2.UpdateStocksFromNative(instrument, ref  sourceStock);

        }


        //FORTS update
        public void UpdateStock(CRawStock rs)
        {


            try
            {

                string isin = _client.Instruments.GetInstrumentByIsinId(rs.Isin_id);                              
                m_stocksStructDict[isin].Add(rs);

                                                        
            }
            catch (Exception e)
            {
                string err = "Unable to update stockbox. Message=";
                Log(err+e.Message +" stacktrace=" + e.StackTrace );
                Error(err, e);
            }

               

        }


		//FORTS update
		public void SimUpdateStock(CRawStock rs)
		{


			try
			{

				string isin = _client.Instruments.GetInstrumentByIsinId(rs.Isin_id);

				//TODO normal
				//if (m_stocksStructDict.ContainsKey(isin))
					m_stocksStructDict[isin].Add(rs);


			}
			catch (Exception e)
			{
				string err = " SimUpdateStock";
				Log(err + e.Message + " stacktrace=" + e.StackTrace);
				Error(err, e);
			}



		}



    }
}
