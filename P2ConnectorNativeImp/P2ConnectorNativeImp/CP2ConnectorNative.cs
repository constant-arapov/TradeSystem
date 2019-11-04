using System;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

using Common;
using Common.Interfaces;
using Common.Logger;
using Common.Utils;
using Common.IO;

using TradingLib;
using TradingLib.Enums;



using P2ConnectorNativeImp.Interfaces;

namespace P2ConnectorNativeImp 
{

	unsafe public class CP2ConnectorNative : IP2StockReaderNativeClient, IAlarmable, INamedPipeServerClient
	{


        //[DllImport("c:\\Dropbox\\proj\\MemMappedFiles\\RcvrDll\\x64\\Debug\\RcvrDll.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		[DllImport(@"..\..\..\..\..\P2ConnectorNative\RcvrDll\x64\Debug\RcvrDll.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int AddInstruments(IntPtr pInstruments, long num);

       




		private IP2ConnectorNativeClient _dealingServer;

		private List<CP2StockReaderNative> _listStocksReaders = 
			new List<CP2StockReaderNative>();

        private Dictionary<long, bool> _dictStockRecieved = new Dictionary<long, bool>();


        //private System.Threading.AutoResetEvent _ev = new System.Threading.AutoResetEvent(
        EventWaitHandle _evWaitNativeConStarted = new System.Threading.EventWaitHandle(false, EventResetMode.AutoReset, "ATFS_wait_P2NativeConnectorInitialized");


        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
        struct struct_instr
        {
            public byte isinId;
        }

        private CNamedPipeServer _namedPipeServer;


		public CP2ConnectorNative(IP2ConnectorNativeClient client)
		{

			_dealingServer = client;

          

		}


		public void Process()
		{

			try
			{

            
              _namedPipeServer = new CNamedPipeServer(@"\\.\pipe\myNamedPipe1", @"\\.\pipe\myNamedPipe2", this);


				StartExternalNativeConnector();

				_dealingServer.WaitInstrumentLoaded();

				var listInstrument = _dealingServer.GetInsruments();
              //  //tempo fofr debug
            //    List<string> listInstrument = new List<string> {"RTS-12.17",  "Si-12.17", "GAZR-12.17", "SBRF-12.17" };
                
                //tempo TODO normal
                if (listInstrument.Contains("USD000000TOD"))
                    listInstrument.Remove("USD000000TOD");

                if (listInstrument.Contains("USD000UTSTOM"))
                    listInstrument.Remove("USD000UTSTOM");


                InitInstruments(listInstrument);


				listInstrument.ForEach(instrument =>
										{
										 CP2StockReaderNative srn = new CP2StockReaderNative(_dealingServer, this);
                                         long isinId =  _dealingServer.GetIsinIdByInstrument(instrument);
										 srn.Process(instrument,
													 isinId
													 );
										 _listStocksReaders.Add(srn);
                                         _dictStockRecieved[isinId] = false;
                                        									
										});




				
				
			}
			catch (Exception e)
			{
				_dealingServer.Error("CP2ConnectorNative.Process", e);
			}


		}

        public void OnRecieveNamedPipeString(string message)
        {

            Error(message);
        }


		IntPtr pt;

        private void InitInstruments(List<string> listInstrument)
        {
             //pt = Marshal.AllocHGlobal(listInstrument.Count * sizeof(struct_instr)*10);
			pt = Marshal.AllocHGlobal(listInstrument.Count * sizeof(Int64));
            long[] arr = new long[listInstrument.Count];



            int i = 0;
            foreach (string instrument in listInstrument)
            {

                long isinId = _dealingServer.GetIsinIdByInstrument(instrument);
                *((long*)pt +i) = isinId;
                //arr[i] = isinId;
                //*((struct_instr*)pt + i) = new struct_instr { isinId = isinId };
                //*((byte*)pt + i) = 1;
                i++;


            }

            //Marshal.Copy(arr, 0, pt, listInstrument.Count);

			int res;

			do
			{
				res = AddInstruments(pt, i);
				Thread.Sleep(100);
			}
			while (res != 0);
        }


        public void UpdateStockReieved(long isinId)
        {

            lock (_dictStockRecieved)
            {
                _dictStockRecieved[isinId] = true;

                bool bAllOn = true;

                foreach (var kvp in _dictStockRecieved)
                    if (!kvp.Value)
                        bAllOn = false;


                if (bAllOn)
                    _dealingServer.IsStockOnline = true;
                    

            }



        }


        [DllImport("user32.dll")]
        static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);


		private void StartExternalNativeConnector()
		{


            string P2ConnectorNativeName = "P2ConnectorNative";

        
            if (CUtil.GetProcess(P2ConnectorNativeName) != null)
            {

                CUtil.GetProcess(P2ConnectorNativeName).Kill();

                while ( CUtil.GetProcess(P2ConnectorNativeName) != null)
                    Thread.Sleep(50);

            }
            //2018-01-09 start after kill to wait till OS close all MemMapped files
          

			//Thread.Sleep(5000);
           // _evWaitNativeConStarted = new EventWaitHandle(initialState: false, mode: EventResetMode.AutoReset);

            Process p2NativeProcess = new Process();

			//TODO normal, from env etc
			//= @"c:\Dropbox\proj\profinvest\plaza2\Plaza2Connector\P2ConnectorNative\P2ConnectorNative\x64\Release\";
            //string workDir = @"..\..\..\..\..\P2ConnectorNative\P2ConnectorNative\x64\Debug\";
            string workDir = @"..\..\..\..\..\P2ConnectorNative\P2ConnectorNative\x64\Release\";
 
			p2NativeProcess.StartInfo.WorkingDirectory = workDir;
			p2NativeProcess.StartInfo.FileName = workDir + P2ConnectorNativeName +".exe";
			//if not set hands application
			//p2RouterProcess.StartInfo.UseShellExecute = false;

			p2NativeProcess.Start();


         

            while (CUtil.GetProcess(P2ConnectorNativeName) == null)
                Thread.Sleep(50);

         //  ShowWindowAsync(p2NativeProcess.Handle, 9);


            //
           // Thread.Sleep(100);

            //_evWaitNativeConStarted = EventWaitHandle.OpenExisting("ATFS_wait_P2NativeConnectorInitialized");

            _evWaitNativeConStarted.WaitOne();
            //TODO normal
            //for now we accept L2Connector creates memmapped files dur
            Thread.Sleep(1000);


		}


        public void Error(string message, Exception e=null)
        {
            _dealingServer.Error(message, e);
            
        }



        



    

	}

}
