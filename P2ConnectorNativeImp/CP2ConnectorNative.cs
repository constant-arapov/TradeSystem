using System;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Linq;
using System.Text;

using Common;
using Common.Logger;
using Common.Interfaces;


namespace P2ConnectorNativeImp
{

    

    unsafe public class CP2ConnectorNative
    {

        /*public struct cg_msg_t
        {
            public UInt32 type;		/// Message type
            public UInt32 data_size;	/// Data size
            public void* data;			/// Data pointer
        };
        */

        public struct cg_msg_t
        {
            public UInt32 type;		/// Message type
            public Int64 data_size;	/// Data size
            public void* data;			/// Data pointer
            Int64 owner_id;                            /// 
        };



        /*
        struct cg_msg_streamdata_t
        {
            public UInt32 type;			/// Message type = CG_MSG_STREAM_DATA
            public UInt32 data_size;		/// Data size
            public void* data;				/// Data pointer

            public Int32 msg_index;		/// Message number in active scheme
            public UInt32 msg_id;		/// Unique message ID (if applicable)
            public char* msg_name;	/// Message name in active scheme

            public Int64 rev;			/// Message sequence number				
            public UInt32 num_nulls;		/// Size of presence map
            public byte* nulls;   /// Presence map. Contains 1 for NULL fields
        };
        */

        struct cg_msg_streamdata_t
        {
            public UInt32 type;			/// Message type = CG_MSG_STREAM_DATA
            public Int64 data_size;		/// Data size
            public void* data;				/// Data pointer
            Int64 owner_id;

            public Int64 msg_index;		/// Message number in active scheme
            public UInt32 msg_id;		/// Unique message ID (if applicable)
            public char* msg_name;	/// Message name in active scheme

            public Int64 rev;			/// Message sequence number				
            public Int64 num_nulls;		/// Size of presence map
            public UInt64 user_id;   /// 
        };




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


        struct Deal
        {
            public Int64 replID; // i8
            public Int64 replRev; // i8
            public Int64 replAct; // i8
            public Int32 sess_id; // i4
            public Int32 isin_id; // i4
            public Int64 id_deal; // i8
            public Int64 id_deal_multileg; // i8
            public Int64 id_repo; // i8
            public Int32 pos; // i4
            public Int32 amount; // i4
            public Int64 id_ord_buy; // i8
            public Int64 id_ord_sell; // i8
            public fixed byte price[11]; // d16.5
            public cg_time_t moment; // t
            public char nosystem; // i1

        };

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



        const UInt32 CG_STATE_CLOSED = 0;
        const UInt32 CG_STATE_ERROR = 1;
        const UInt32 CG_STATE_OPENING = 2;
        const UInt32 CG_STATE_ACTIVE = 3;

        const UInt32 CG_MSG_OPEN = 0x100;
        const UInt32 CG_MSG_CLOSE = 0x101;
        const UInt32 CG_MSG_TIME = 0x102;
        const UInt32 CG_MSG_DATA = 0x110;
        const UInt32 CG_MSG_STREAM_DATA = 0x120;


        const UInt32 CG_MSG_TN_BEGIN = 0x200;
        const UInt32 CG_MSG_TN_COMMIT = 0x210;

        const UInt32 CG_MSG_P2MQ_RANGE_START = 0x1000;
        const UInt32 CG_MSG_P2MQ_TIMEOUT = (0x1 + CG_MSG_P2MQ_RANGE_START);

        const UInt32 CG_MSG_P2REPL_RANGE_START = 0x1100;
        const UInt32 CG_MSG_P2REPL_LIFENUM = (0x10 + CG_MSG_P2REPL_RANGE_START);
        const UInt32 CG_MSG_P2REPL_CLEARDELETED = (0x11 + CG_MSG_P2REPL_RANGE_START);
        const UInt32 CG_MSG_P2REPL_ONLINE = (0x12 + CG_MSG_P2REPL_RANGE_START);
        const UInt32 CG_MSG_P2REPL_REPLSTATE = (0x15 + CG_MSG_P2REPL_RANGE_START);




        [DllImport("c:\\Program Files (x86)\\P2CGate\\bin\\cgate64.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 cg_env_open(string settings);


        [DllImport("c:\\Program Files (x86)\\P2CGate\\bin\\cgate64.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern UInt32 cg_conn_new(string conn_str, /*void** */int**  conn);


        [DllImport("c:\\Program Files (x86)\\P2CGate\\bin\\cgate64.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern UInt32 cg_conn_open(int* conn, string settings);

        [DllImport("c:\\Program Files (x86)\\P2CGate\\bin\\cgate64.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern UInt32 cg_conn_getstate(int* conn, UInt32* state);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public  delegate  UInt32 CG_LISTENER_CB(int* conn, int* listener, cg_msg_t* msg, void* data);

        [DllImport("c:\\Program Files (x86)\\P2CGate\\bin\\cgate64.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern UInt32 cg_lsn_new(void* conn, string settinf, CG_LISTENER_CB cb, void* data, int** lsnptr);

        [DllImport("c:\\Program Files (x86)\\P2CGate\\bin\\cgate64.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern UInt32 cg_conn_close(void* conn);

        [DllImport("c:\\Program Files (x86)\\P2CGate\\bin\\cgate64.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern UInt32 cg_conn_process(void* conn, UInt32 timeout, void* reserved);

        [DllImport("c:\\Program Files (x86)\\P2CGate\\bin\\cgate64.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern UInt32 cg_lsn_getstate(int* lsn, UInt32* state);

        [DllImport("c:\\Program Files (x86)\\P2CGate\\bin\\cgate64.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern UInt32 cg_lsn_open(int* lsn, string settings);

        [DllImport("c:\\Program Files (x86)\\P2CGate\\bin\\cgate64.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern UInt32 cg_lsn_close(int* lstn);

        [DllImport("c:\\Program Files (x86)\\P2CGate\\bin\\cgate64.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern UInt32 cg_bcd_get(void* bcd, Int64* intpart, byte* scale);



        private CG_LISTENER_CB CallbackDelegate;



       static CLogger _log;
      static IAlarmable _alarmer;

      static DateTime dtStarted;

      IntPtr protConn;
      //IntPtr conn;
        public CP2ConnectorNative(IAlarmable alarmer)
        {
            dtStarted = DateTime.Now;
            _alarmer = alarmer;
            //_alarmer.Error("Test");
            _log = new  CLogger("NatConnectionMarket");
            _log.Log("+++++++++++++++++++++++++++++++++ Connection started      +++++++++++++++++++++++++++++++++++++++++++++++");


            CallbackDelegate = Callback;


            UInt32 res = cg_env_open("ini=repl.ini;key=11111111");


            unsafe
            {


                int* conn = null;
                
                
                
                res = cg_conn_new("p2tcp://127.0.0.1:4001;app_name=test_nat", &conn);
                protConn = (IntPtr)conn;
                int* lstn = null;
                void* data = null;
                //res = cg_lsn_new(conn, "p2repl://FORTS_DEALS_REPL;tables=deal", CallbackDelegate, data, &lstn);
                res = cg_lsn_new(conn, "p2repl://FORTS_FUTAGGR50_REPL", CallbackDelegate, data, &lstn);

                UInt32 state;

                bool bExit = false;
                while (!bExit)
                {
                    res = cg_conn_getstate(conn, &state);
                    if (state == CG_STATE_ERROR)
                    {
                        res = cg_conn_close(conn);
                    }
                    else if (state == CG_STATE_CLOSED)
                    {
                        res = cg_conn_open(conn, "");

                    }

                    else if (state == CG_STATE_ACTIVE)
                    {

                        res = cg_conn_process(conn, 0, null);

                        res = cg_lsn_getstate(lstn, &state);

                        if (state == CG_STATE_CLOSED)
                        {
                           // res = cg_lsn_open(lstn, "mode=online");
							res = cg_lsn_open(lstn, "");

                        }
                        else if (state == CG_STATE_ERROR)
                        {

                            cg_lsn_close(lstn);
                        }
                    }
                }
               


            }
            //  res = cg_conn_new("p2tcp://127.0.0.1:4001;app_name=test_repl", pt);

        }
		/*
        private static CP2ConnectorNative _instance;

        private static CP2ConnectorNative Create (IAlarmable alarmable)
        {
            _instance = new CP2ConnectorNative(alarmable);
            return _instance;

        }
		*/


        public /*static*/ UInt32 Callback(int* conn, int* listener, cg_msg_t* msg, void* data)
        {
            try
            {
                switch (msg->type)
                {
                    case CG_MSG_STREAM_DATA:
                        {
                            cg_msg_streamdata_t* replmsg = (cg_msg_streamdata_t*)msg;
                            //string name = new string(replmsg->msg_name);
                            string name = Marshal.PtrToStringAnsi((IntPtr)replmsg->msg_name);


                            if (name == "deal")
                            {

                                Deal* deal = (Deal*)replmsg->data;

                                Int64 intpart = 0;
                                byte scale;

                                cg_bcd_get(deal->price, &intpart, &scale);
                                double price = intpart;

                                for (int i = 0; i < scale; i++)
                                    price /= 10;


                                DateTime dtServer = new DateTime(deal->moment.year, deal->moment.month, deal->moment.day,
                                                             deal->moment.hour, deal->moment.minute, deal->moment.second,
                                                             deal->moment.msec);


                                DateTime dtLocal = (DateTime.Now).AddHours(-2);

                                double delta = (dtLocal - dtServer).TotalMilliseconds;
                                string marker = "";
                                if ((DateTime.Now - dtStarted).TotalSeconds > 3)
                                    if (delta > 30)
                                    {

                                        marker = " <==";
                                       // _alarmer.Error("native deals dt=" + delta);
                                        Error("native deals dt=" + delta);
                                    }
                                _log.Log("replID=" + deal->replID + " replRev=" + deal->replRev + " id_deal" + deal->id_deal + " isin_id=" + deal->isin_id
                                    + " moment=" + dtServer.ToString("hh:mm:ss.fff") + " dt=" + delta + marker);

                            }

                            else if (name == "orders_aggr")
                            {

                                orders_aggr* oa = (orders_aggr*)replmsg->data;


                                if (oa->isin_id != 430354 && oa->isin_id != 422393 &&
                                    oa->isin_id != 457935 && oa->isin_id != 457974)
                                    break;

                                Int64 intpart = 0;
                                byte scale;

                                cg_bcd_get(oa->price, &intpart, &scale);
                                double price = intpart;

                                for (int i = 0; i < scale; i++)
                                    price /= 10;


                                string dt = CGTImeToString(oa->moment);


                                string msgToLog = String.Format("replId={0} replRev={1} isin_id={2} price={3} volume={4} moment={5}", 
                                                                oa->replID, oa->replRev, oa->isin_id, price, oa->volume, dt);
                                _log.Log(msgToLog);

                              
                            }



                            break;
                        }
                    case CG_MSG_P2REPL_ONLINE:
                        {
							_log.Log("======================================================================== ONLINE ============================================================================================================================");

                            break;
                        }
                    case CG_MSG_TN_BEGIN:
                        {
							_log.Log("==================================================== Begin ============================================================");
                            break;
                        }

                    case CG_MSG_TN_COMMIT:
                        {
							_log.Log("==================================================== Commit ============================================================");

                            break;
                        }

                    case CG_MSG_OPEN:
                        {



                            break;
                        }
                    case CG_MSG_CLOSE:
                        {



                            break;
                        }
                    case CG_MSG_P2REPL_LIFENUM:
                        {

                            break;
                        }

                    //   default:
                }
            }
            catch (Exception e)
            {
              //  _alarmer.Error("Error in native callback");
                Error("Error in native callback");
            }

            // System.Threading.Thread.Sleep(1000);
            return (UInt32)0;
        }


        //typedef CG_RESULT (CG_API *CG_LISTENER_CB)(cg_conn_t* conn, cg_listener_t* listener, struct cg_msg_t* msg, void* data);



       private static string CGTImeToString(cg_time_t cgtm)
       {
	   
	
            string st= 
	        String.Format ("{0}.{1}.{2} {3}:{4}:{5}",
		                cgtm.year, cgtm.month,cgtm.day, cgtm.hour, cgtm.minute, cgtm.second, cgtm.msec);

	

	return st;


}




        private static void Error(string msg, Exception e=null)
        {

            if (_alarmer != null)
            {
                _alarmer.Error(msg,e);

            }
            
        }


    }


}
