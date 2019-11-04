using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Abstract;
using TradingLib.Interfaces.Clients;

using TradingLib.Enums;

using ASTS.Tables;

namespace ASTS.DealingServer.Stocks
{
	public class CStockConverterASTS : CBaseStockConverter
	{



		public CStockConverterASTS(int stockDepth, string instrument, IClientStockConverter client)
			: base(stockDepth, instrument, client)
		{

			

		}


		public void ProcessConvert(List<CTableRecrd> orderbook )
		{

		


			if (orderbook == null)
			{
				Error("ProcessConvert orderbook is null");
				return ;
            }

            //TODO normal
            for (int i = 0; i < m_dirStock.GetStockDepth(0); i++)
            {
                m_dirStock[Direction.Up][0][i].Price = 0;
                m_dirStock[Direction.Up][0][i].Volume = 0;

                m_dirStock[Direction.Down][0][i].Price = 0;
                m_dirStock[Direction.Down][0][i].Volume = 0;


            }
            


            int num_up = 0;
            int num_down = 0;
			int d = 0 ;
			bool bBidAskChanged = false;


			foreach (CTableRecrd rec in orderbook)
			{
				string instrument = Convert.ToString(rec.values["SECCODE"]);
				Direction dir;
				char buySell = Convert.ToChar(rec.values["BUYSELL"]);
				if (buySell == 'S')
				{
					dir = Direction.Up;
					num_up++;
					d = num_up;
				}
				else
				{
					dir = Direction.Down;
					num_down++;
					d = num_down;
				}

				m_dirStock[dir][d - 1][0].Price = Convert.ToDecimal(rec.values["PRICE"]);
				m_dirStock[dir][d - 1][0].Volume = Convert.ToInt32(rec.values["QUANTITY"]);
			}

            //QuicksortAsc(m_dirStock[Direction.Up], 0, num_up - 1);
            QuicksortDesc(m_dirStock[Direction.Down][0], 0, num_down - 1);

				m_dirStock.Bid = m_dirStock[Direction.Down][0][0].Price;
				m_dirStock.Ask = m_dirStock[Direction.Up][0][0].Price;


				_bidInternal = m_dirStock.Bid;
				_askInternal = m_dirStock.Ask;


				decimal tol = 0.001M;
				//bid or ask changed
				if (_bidInternalOld != _bidInternal || _askInternalOld != _askInternal /*||
					(_bidInternal < tol && _bidInternal > -tol) ||
					(_askInternal < tol && _askInternal > -tol)*/
																					    )
				{
					//KAA no need forcing bid ask

					bBidAskChanged = true;
					Log("================= Need update Bid=" + m_dirStock.Bid + " Ask=" + m_dirStock.Ask);

				}
				_bidInternalOld = m_dirStock.Bid;
				_askInternalOld = m_dirStock.Ask;


              

				_client.SnapshoterStock.UpdateInpStocksBothDir(Instrument, ref m_dirStock, precision:0);

                //bid ask change or short time since start
                if (bBidAskChanged )
				{
					_client.SnapshoterStock.UpdateOutStock(Instrument,
																		  bDoNotUpdateTraders: true);

					UpdateBidAsk(_bidInternal, _askInternal);
					_client.UserDealsPosBox.RefreshBotPos(Instrument);
				}


		

          
			PrintStocks();

	      //_client.SnapshoterStock.UpdateInpStocks(
		}


       
       
        
	
		private void PrintStocks()
		{
			Log("____________________________________________________________________________________________________");
			foreach (var kvp in m_dirStock)
			{
			

				StringBuilder ln = new StringBuilder();
				foreach (var kvp2 in kvp.Value[0])
				{
					ln.Append(String.Format("{0} {1}|", kvp2.Price, kvp2.Volume));
			
				}
				Log(ln.ToString());
			}

		}
	

	}
}
