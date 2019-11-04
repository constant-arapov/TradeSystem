using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Diagnostics;

using Common.Logger;

using TradingLib.Interfaces.Clients;
using TradingLib.Data;
using TradingLib.ProtoTradingStructs;


namespace TradingLib.Abstract
{
	public abstract class CBaseStockConverter : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;



		protected CSharedStocks m_dirStock;

		private decimal _bid;
		public decimal Bid
		{

			get
			{
				return _bid;

			}
			set
			{
				_bid = value;
				RaisePropertyChanged("Bid");
			}

		}


		private decimal _ask;
		public decimal Ask
		{

			get
			{
				return _ask;

			}
			set
			{
				_ask = value;
				RaisePropertyChanged("Ask");
			}

		}

		public decimal GetBid()
		{
			lock (_lckBidOut)
			{
				return _bidOut;
			}

		}

		public decimal GetAsk()
		{
			lock (_lckAskOut)
			{
				return _askOut;

			}

		}
		public string Instrument { get; set; }

		private object _lckBidOut = new object();
		private object _lckAskOut = new object();
		private decimal _bidOut = 0;
		private decimal _askOut = 0;
		protected int _stockDepth;
		/*CPlaza2Connector*/
		protected IClientStockConverter _client;
		private CLogger m_logger;

		protected decimal _bidInternal = 0;
		protected decimal _askInternal = 0;


		protected decimal _bidInternalOld = 0;
		protected decimal _askInternalOld = 0;




		

		public CBaseStockConverter(int stockDept, string isin, IClientStockConverter client)
		{
			_stockDepth = stockDept;
			Instrument = isin;
			_client = client;
			m_dirStock = new CSharedStocks(_client);
			m_logger = new CLogger("StockConverter_" + Instrument, flushMode:true  , subDir:"StockConverters", useMicroseconds:false);
		}

		protected void RaisePropertyChanged(string prop)
		{
			if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
		}


		public void UpdateBidAndAskGUI(decimal bid, decimal ask)
		{

			Ask = ask;
			Bid = bid;
		}

		public void UpdateBidAsk(decimal bid, decimal ask)
		{

			try
			{

				lock (_lckBidOut)
				{
					_bidOut = bid;

				}
				lock (_lckAskOut)
				{
					_askOut = ask;
				}

				//TODO separate GUI and recalc logics
				//TODO for calculation use another bid and ask
				//if need directly use:
				//UpdateBidAndAskGUI(decimal bid, decimal ask)

				_client.GUIBox.ExecuteWindowsStockUpdate(new Action(() => UpdateBidAndAskGUI(bid, ask)));

				Log("======== DEBUGING ======   Bid=" + Bid + " Ask=" + Ask + " =============================================");

			}
			catch (Exception e)
			{

				Error("UpdateBidAsk", e);
			}
		}


        protected int PartitionAsc(CStock[] array, int start, int end)
        {
            int marker = start;
            for (int i = start; i <= end; i++)
            {
                if (array[i].Price <= array[end].Price)
                {
                    CStock temp = array[marker]; // swap
                    array[marker] = array[i];
                    array[i] = temp;
                    marker += 1;
                }
            }
            return marker - 1;
        }


        //==============================================================================
        protected int PartitionDesc(CStock[] array, int start, int end)
        {
            int marker = start;
            for (int i = start; i <= end; i++)
            {
                if (array[i].Price >= array[end].Price)
                {
                    CStock temp = array[marker]; // swap
                    array[marker] = array[i];
                    array[i] = temp;
                    marker += 1;
                }
            }
            return marker - 1;
        }
        //from inet
        protected void QuicksortDesc(CStock[] array, int start, int end)
        {
            if (start >= end)
            {
                return;
            }
            int pivot = PartitionDesc(array, start, end);
            QuicksortDesc(array, start, pivot - 1);
            QuicksortDesc(array, pivot + 1, end);
        }




        //algo from inet
        protected void QuicksortAsc(CStock[] array, int start, int end)
        {
            if (start >= end)
            {
                return;
            }
            int pivot = PartitionAsc(array, start, end);
            QuicksortAsc(array, start, pivot - 1);
            QuicksortAsc(array, pivot + 1, end);
        }
        //==============================================================================




		public void Log(string msg, Stopwatch sw=null)
        {
            string outMsg = msg;
            if (sw != null)
                outMsg = " [" + sw.ElapsedTicks.ToString() + "] " + msg;

            m_logger.Log(outMsg);
        }

		public void Error(string description, Exception exception = null)
		{
            _client.Error(description, exception);

        }


	}








	
}
