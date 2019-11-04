using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using System.Threading;


using Common;
using Common.Interfaces;
using Common.Logger;
using Common.Utils;


using TradingLib;
using TradingLib.Enums;


using P2ConnectorNativeImp.Interfaces;


namespace P2ConnectorNativeImp
{
	unsafe public class CP2StockReaderNative
	{
		const int BUFFER_SIZE = 3 * 4096;
		const int STOCK_DEPTH = 50; //TODO from global conf etc
		private long _isinId;
		private string _instrument;

	
        [DllImport(@"..\..\..\..\..\P2ConnectorNative\RcvrDll\x64\Debug\RcvrDll.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitStock(long isinId);


        [DllImport(@"..\..\..\..\..\P2ConnectorNative\RcvrDll\x64\Debug\RcvrDll.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReadStock(long isinId,out IntPtr outData);


		[DllImport(@"..\..\..\..\..\P2ConnectorNative\RcvrDll\x64\Debug\RcvrDll.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern void CloseStock(long isinId);


	

		private CSharedStocks _outStock;

		struct TStockElement
		{
			public Int64 volume;
			public byte dir;

		}

		[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
		struct cg_time_t
		{
			public UInt16 year; /// Year
			public byte month; /// Month of year (1-12)
			public byte day; /// Day of month (1-31)
			public byte hour; /// Hour (0-23)
			public byte minute; /// Minute (0-59)
			public byte second; /// Second (0-59)
			public UInt16 msec; /// Millisecond (0-999)
		};




		[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
		struct orders_aggr
		{
			public Int64 replID; // i8
			public Int64 replRev; // i8
			public Int64 replAct; // i8
			public Int32 isin_id; // i4
			public fixed byte price[11]; // d16.5
			public Int64 volume; // i8
			public cg_time_t moment; // t
			public Int64 moment_ns; // u8
			public sbyte dir; // i1
			public cg_time_t timestamp; // t
			public Int32 sess_id; // i4


		}

		private CSharedStocks sourceStock;

		private /*IP2StockReaderNative*/ IP2ConnectorNativeClient _dealingServer;
        private IP2StockReaderNativeClient _client;

        private CLogger _logger;

        public CP2StockReaderNative(IP2ConnectorNativeClient dealingServer, IP2StockReaderNativeClient client)
		{
			_dealingServer = dealingServer;
			_outStock = new CSharedStocks(2 * STOCK_DEPTH);
            _client = client;

           


		}

		private bool _bRun = true;
        private bool _bSentStockRecieved = false;


		IntPtr pt;
		IntPtr intPart;
		IntPtr scale;
        public void ThredProcess()
        {

             pt = Marshal.AllocHGlobal(BUFFER_SIZE);

			 intPart = Marshal.AllocHGlobal(8);
             scale = Marshal.AllocHGlobal(1);


            //Int64* intPart = ( Int64*) Marshal.AllocHGlobal(16);
            // byte* scale = (byte*) Marshal.AllocHGlobal(8);
            //TODO check initialization
            InitStock(_isinId);

            long lng;
            sbyte sb;



            _logger = new CLogger(String.Format("CP2StockReaderNative_{0}", _isinId),false, "P2StockReaderNative");
            int itmp = 0;

            while (_bRun)
            {
                //ReadData(out pt);
                try
                {
                    ReadStock(_isinId, out pt);

                    int iBuy = 0;
                    int iSell = 0;
                    string stDebug = "";
                    for (int cnt = 0; cnt < 2 * STOCK_DEPTH; cnt++)
                    {
                        orders_aggr* el = (orders_aggr*)pt + cnt;
                        //if empty element
                        if (el->replID == 0)
                            continue;

                        decimal price = GetDecimalFromBCD(el->price);
                        long volume = el->volume;
                        sbyte dir = el->dir;
                      
                        lock (_outStock.Lck)
                        {
                            //Direction 
                            int ind = 0;
                            if (dir == (sbyte)EnmOrderDir.Buy)
                                ind = iBuy++;
                            else
                                ind = iSell++;

                            _outStock[(Direction)dir][ind].Price = price;
                            _outStock[(Direction)dir][ind].Volume = volume;

                            stDebug += String.Format("d={0} p={1} v={2}",
                                                    (Direction)dir,
                                                     _outStock[(Direction)dir][ind].Price,
                                                     _outStock[(Direction)dir][ind].Volume);
                        }

                        //_outStock.

                        /*

                        //cg_bcd_get(el->price, &lng, &scale);

                        Int64* intPartVal = (Int64*)intPart;
                        sbyte* scaleVal = (sbyte*)scale;
                        s

                        if (intPartVal != null)
                            Thread.Sleep(0);

                        if (scaleVal != null)
                            Thread.Sleep(0);

                        // Console.WriteLine(el->volume.ToString());
                        */

                    }

                    _outStock.UpdateBidAsk();

                    _dealingServer.UpdateInpStocks(_isinId, ref _outStock);
                    if (!_bSentStockRecieved)                    
                        _client.UpdateStockReieved(_isinId);

                    Log(stDebug);
                    Log("==================================================================================================");

                 
                        Thread.Sleep(1);
                 


                }
                catch (Exception e)
                {
                    //TODO ERROR !
                    _dealingServer.Error("P2StockReaderNative.ThredProcess", e);
                }


            }

         



            try
            {
                CloseStock(_isinId);
            }
            catch (Exception e)
            {
                _dealingServer.Error("P2StockReaderNative error close stock", e);
            }

            //Close();


        }


        private void DbGOutStock()
        {
           


        }


        private void Log(string msg)
        {

            _logger.Log(msg);

        }



		public void Process(string instrument,long isinId)
		{

			_instrument = instrument;
			_isinId = isinId;


			//TODO check server process running wait etc


            CUtil.TaskStart(ThredProcess);

			


		}

		

		private decimal GetDecimalFromBCD(byte* priceBCD)
		{

			decimal price = 0;
			byte offsEndIntPart = 7;
			decimal mult = 1;
			for (byte i = 0; i <= 4; i++)
			{
				mult = 1;
				for (byte j = 0; j < i; j++)
					mult *= 100;

				price += mult * (*((byte*)(priceBCD + offsEndIntPart - i)));
			}


			for (byte i = 0; i <= 3; i++)
			{
				mult = 0.01M;
				for (int j = 0; j < i; j++)
					mult *= 0.01M;

				price += mult * (*((byte*)(priceBCD + offsEndIntPart + 1 + i)));
			}

			if (*(byte*)(priceBCD + 3) > 0)
				price *= -1;

			return price;
		}




	}
}
