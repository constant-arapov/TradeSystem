using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib;
using TradingLib.ProtoTradingStructs;

using TradingLib.Enums;

using System.Diagnostics;
using Common;

using Common.Logger;

namespace zTestMem
{
    public class TestTicks
    {

        List<CDealClass> sourceLst = new List<CDealClass>();
        List<CDealClass> destLst = new List<CDealClass>();
        Stopwatch sw = new Stopwatch();
        List<long> lstMS = new List<long>();
        List<long> lstTicks = new List<long>();


        List<long> lstTotBeginMs = new List<long>();
        List<long> lstTotEndMs = new List<long>();

        List<long> lstTotBeginTicks = new List<long>();
        List<long> lstTotEndTicks = new List<long>();

        Stopwatch swTot = new Stopwatch();
        Stopwatch swFile = new Stopwatch();


        public TestTicks()
        {
            string d = DateTime.Now.ToString("yyy_MM_dd__hh_mm_ss");
            CLogger logger = new CLogger("TestTicks_" + d, false);


            swTot.Reset();
            swTot.Start();
            InitTicks();

            CDealsList dl = new CDealsList();
            dl.DealsList = new List<CDealClass>();
            List<long> lstDi = new List<long>();
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 900000; i++)
                {

                    lstTotBeginMs.Add(swTot.ElapsedMilliseconds);
                    lstTotBeginTicks.Add(swTot.ElapsedTicks);

                    sw.Reset();
                    sw.Start();
                    CDealsAgregator da = new CDealsAgregator(100, 1M);
                    da.GenAggrPrice(sourceLst, dl.DealsList);
                    //CDealsAgregator da = new CDealsAgregator(sourceLst, dl.DealsList, 100,1M);
                    sw.Stop();
                    lstMS.Add(sw.ElapsedMilliseconds);
                    //lstTicks.Add(sw.ElapsedTicks);

                    lstTotEndMs.Add(swTot.ElapsedMilliseconds);
                    lstTotEndTicks.Add(swTot.ElapsedTicks);
                    if (i > 1)
                    {
                        long di = lstTotBeginMs[i] - lstTotBeginMs[i - 1];
                        if (di > 10)
                            lstDi.Add(di);
                    }
                    // System.Threading.Thread.Sleep(1);
                }

                swTot.Stop();
                lstTotBeginMs.Clear();
                lstTotBeginTicks.Clear();
                lstTotEndMs.Clear();
                lstTotEndTicks.Clear();
                lstMS.Clear();

            }
            swFile.Reset();
            swFile.Start();
            for (int i = 0; i < lstTotBeginTicks.Count; i++)
            {
                logger.Log("__________________________________");
                logger.Log("i=" + i);
                logger.Log("iter begin " + lstTotBeginMs[i] + " | " + lstTotBeginTicks[i]);
                logger.Log("iter end " + lstTotEndMs[i] + " | " + lstTotEndTicks[i]);
                logger.Log("__________________________________");

            }
            swFile.Stop();

            for (int i = 0; i < lstDi.Count; i++)
            {

                logger.Log(lstDi[i].ToString());

            }


            logger.Flush();

            System.Threading.Thread.Sleep(1000);

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
}
