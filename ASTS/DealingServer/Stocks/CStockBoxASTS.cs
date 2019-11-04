using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib;
using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Clients;
using TradingLib.Abstract;
using TradingLib.Enums;
using TradingLib.Data;


using MOEX.ASTS.Client;

using ASTS.Tables;


namespace ASTS.DealingServer.Stocks
{
	public class CStockBoxASTS : CBaseStockBox,  IStockBox 
	{

		//private Dictionary<string, CStockConverterASTS> m_stockStructDict = new Dictionary<string, CStockConverterASTS>();

		private Dictionary<string, CStockConverterASTS> _disctStockConverter = new Dictionary<string, CStockConverterASTS>();




		public CStockBoxASTS(IClientStockBox client, int stockDepth)
			: base(client, stockDepth)
		{


			CreateStocksStructDict(stockDepth);
		}

		protected  void CreateStocksStructDict(int stockDepth)
		{
		
			foreach (var dbInstr in _client.Instruments)
			{
				_disctStockConverter[dbInstr.instrument] = new CStockConverterASTS(stockDepth, dbInstr.instrument, _client);

			}

            
			if (!_client.IsStockOnline)
			{
				_client.IsStockOnline = true;
				_client.EvStockOnline.Set();

			}


		}


		public decimal GetBestPice(string instrument, EnmOrderDir ordDir) 
		{
            decimal val = 0;

            try
            {

                {
                    if (EnmOrderDir.Buy == ordDir)
                        val = _disctStockConverter[instrument].GetBid();
                    else
                        val = _disctStockConverter[instrument].GetAsk();




                }
            }
            catch (Exception e)
            {
                Error("GetBestPice", e);

            }

            return val;
		}


		public decimal GetBid(string instrument) 
		{
			//return 0;
            return _disctStockConverter[instrument].GetBid();
		}


		public decimal GetAsk(string instrument) 
		{
			//return 0;
            return _disctStockConverter[instrument].GetAsk();
		}

		public void ReInitAllStocks() 
		{
		
		}
        //make stub for while
		public bool IsStockAvailable(string instrument)
		{
            return true;
		}

		public override CBaseStockConverter GetStockConverter(string instrument)
	    {

			return _disctStockConverter[instrument];
			//return new CStockConverterP2(100, "", null);

	    }

		public void Update(List<CTableRecrd> orderBook)
		{


			


			if (orderBook==null || orderBook.Count==0)
			{
					//Error("wrong orderbook");
                    return;
			}


            try
            {
                string instrument = Convert.ToString(orderBook[0].values["SECCODE"]);
                if (!_disctStockConverter.ContainsKey(instrument))                
                   // Error("_disctStockConverter doesn't contain " + instrument);
                    return;
                

                _disctStockConverter[instrument].ProcessConvert(orderBook);

                //tell GUI - we online
              /* if (!_client.IsStockOnline)
                {
                    _client.IsStockOnline = true;
                    _client.EvStockOnline.Set();

                }
				*/

            }
            catch (Exception e)
            {
                Error("CStockBoxASTS.Update",e);
            }


	  }

	}



	
}
