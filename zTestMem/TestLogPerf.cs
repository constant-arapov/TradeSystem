using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using System.Threading;

using Common;
using Common.Logger;
using Common.Collections;

using TradingLib;
using TradingLib.ProtoTradingStructs;
using TradingLib.Enums;

namespace zTestMem
{
    public class TestLogPerf
    {

        List<CDealClass> sourceLst = new List<CDealClass>();
        List<CDealClass> destLst = new List<CDealClass>();

        CLoggerBuffered LogBuff;
        public TestLogPerf()
        {

            //System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.LowLatency;

        
          //  CUtil.IncreaseProcessPriority();

            Thread.CurrentThread.Priority = ThreadPriority.Highest;


            InitTicks();
            CDealsList dl = new CDealsList();
            dl.DealsList = new List<CDealClass>();
                 

            CLogServer lgSrv = new CLogServer();
            List<long> lstHighTck = new List<long>();
            List<long> lstHighLogTck = new List<long>();

            LogBuff = new CLoggerBuffered("TestLogPerf", lgSrv);
            (new Thread(ThreadLogProducer)).Start();
            Stopwatch swTotal = new Stopwatch();
            LogBuff.Log("--------------started----------------------------");
            swTotal.Start();

            long beforeTck = 0;
            long afterTck = 0;
            long deltaTck = 0;
            long beforeLogTck = 0;
            long afterLogTck = 0;
            long deltalogTck = 0;
            long deltaCycle = 0;

            for (int i = 0; i < 10000; i++)
            {

                beforeTck = swTotal.ElapsedTicks;
                int fac = 1;
               // for (int j = 1; j < 20; j++)                
                 //   fac *= j;
                CDealsAgregator da = new CDealsAgregator(100, 1M);
                da.GenAggrPrice(sourceLst, dl.DealsList);


                //CDealsAgregator da = new CDealsAgregator(sourceLst, dl.DealsList, 100,1M);


                afterTck = swTotal.ElapsedTicks;
                deltaTck = afterTck - beforeTck;

                deltalogTck = afterLogTck - beforeLogTck;
                deltaCycle = beforeTck - afterLogTck;

                beforeLogTck = swTotal.ElapsedTicks;
               
                long d1 = 1;
                DateTime dt = DateTime.Now;
                string st1 = "sssssss";
                decimal dc1 = 32132.24343M;

                LogMsg(new CLogStruct
                {
                    I = i,
                    DeltaTck = deltaTck,
                    DeltaLogTck = deltalogTck,
                    DeltaCycle = deltaCycle,
                    D1 = d1,
                    Dt = dt,
                    St1 = st1,
                    dc1 = dc1,
                    LargeStr =
                        " 121323232434gdfgdgdsfgdfgsdgdfgsfdgsdgdsfgsdfgsdfgdfgsdfgsdfgfdsgsdg"
                }
                );
                if (deltaTck  > 100 && i>0)
                    lstHighTck.Add(deltaTck);
                if (deltalogTck > 100)
                    lstHighLogTck.Add(deltalogTck);


                    //LogBuff.Log("i= " + i + " deltaTck=" + deltaTck + " deltaLogTck=" + deltalogTck+"  "

                   //+ d1+dt+st1+dc1
                    // +" 121323232434gdfgdgdsfgdfgsdgdfgsfdgsdgdsfgsdfgsdfgdfgsdfgsdfgfdsgsdg"
                    //);
                afterLogTck = swTotal.ElapsedTicks;
                

            }


            swTotal.Stop();
            Thread.Sleep(2000);
            LogBuff.Log("eclapsed ms=" +  swTotal.ElapsedMilliseconds +" ticks="+swTotal.ElapsedTicks );
            LogBuff.Log("high tick ======");
            foreach(var v in lstHighTck)
            LogBuff.Log(" tick="+v);

            LogBuff.Log("high tick log======");
            foreach (var v in lstHighLogTck)
                LogBuff.Log(" log tick=" + v);

            LogBuff.Log("--------finished-------------------------------------------");
            
          
        }






        CBlockingQueue<CLogStruct> _bqLog = new CBlockingQueue<CLogStruct>();

        public void LogMsg(CLogStruct bqLog)
        {

            _bqLog.Add(bqLog);

        }
        private void ThreadLogProducer()
        {

            while (true)
            {
                CLogStruct strct = _bqLog.GetElementBlocking();
                LogBuff.Log("i= " + strct.I + " deltaTck=" + strct.DeltaTck + " deltaLogTck=" + strct.DeltaLogTck +"  "
                             +"deltaCycle="+strct.DeltaCycle
                             + " d1=" +strct.D1 +" dt=" + strct.Dt + " st1=" +strct.St1+ " d1="+strct.D1
                             + strct.LargeStr );

            }


        }


        public void InitTicks()
        {

            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 42, 602), DirDeal = EnmDealDir.Buy, Price = 73000, Amount = 1 });
            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 42, 602), DirDeal = EnmDealDir.Buy, Price = 73000, Amount = 2 });
            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 42, 873), DirDeal = EnmDealDir.Buy, Price = 73000, Amount = 1 });
            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 43, 082), DirDeal = EnmDealDir.Buy, Price = 73000, Amount = 1 });

            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 43, 203), DirDeal = EnmDealDir.Sell, Price = 77571, Amount = 1 });

            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 44, 107), DirDeal = EnmDealDir.Buy, Price = 73040, Amount = 2 });

            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 44, 600), DirDeal = EnmDealDir.Sell, Price = 77545, Amount = 2 });
            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 44, 600), DirDeal = EnmDealDir.Sell, Price = 77543, Amount = 2 });

            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 45, 749), DirDeal = EnmDealDir.Sell, Price = 77557, Amount = 1 });

            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 45, 826), DirDeal = EnmDealDir.Sell, Price = 73020, Amount = 2 });

        }



    }

     //log.Log("i= " + i + " deltaTck=" + deltaTck + " deltaLogTck=" + deltalogTck+"  "
    //+ d1+dt+st1+dc1




    public class CLogStruct
    {
        public long I { get; set; }
        public long DeltaTck { get; set; }
        public long DeltaLogTck { get; set; }
        public long DeltaCycle { get; set; }
        public long D1 { get; set; }
        public DateTime Dt { get; set; }
        public string St1 { get; set; }
        public decimal dc1 { get; set; }
        public string LargeStr { get; set; }


     }
}
