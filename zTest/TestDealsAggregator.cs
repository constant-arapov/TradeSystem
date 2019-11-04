using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib;
using TradingLib.Enums;
using TradingLib.ProtoTradingStructs;

using NUnit.Framework;

namespace zTest
{
   public class TestDealsAggregator
    {

        List<CDealClass> sourceLst = new List<CDealClass>();
        List<CDealClass> destLst = new List<CDealClass>();

        public TestDealsAggregator()
        {


            LoadData1();

            CDealsAgregator da = new CDealsAgregator(100, 1M);
            da.GenAggrPrice(sourceLst, destLst);


                     
            int i = 0;

            TestDealStruct(destLst[i++], 2,  EnmDealDir.Buy, 73000);
            TestDealStruct(destLst[i++], 1, EnmDealDir.Buy, 73000);
            TestDealStruct(destLst[i++], 1, EnmDealDir.Buy, 73000);
            TestDealStruct(destLst[i++], 1, EnmDealDir.Sell, 77571);
            TestDealStruct(destLst[i++], 2, EnmDealDir.Buy, 73040);
            TestDealStruct(destLst[i++], 4, EnmDealDir.Sell, 77544);
            TestDealStruct(destLst[i++], 1, EnmDealDir.Sell, 77557);
            TestDealStruct(destLst[i++], 2, EnmDealDir.Sell, 73020);

            LoadData2();
            da.GenAggrPrice(sourceLst, destLst);

            TestDealStruct(destLst[i++], 2, EnmDealDir.Sell, 73020);



            System.Threading.Thread.Sleep(0);
        }

        public void TestDealStruct(CDealClass dc, int amount, EnmDealDir dirDeal, decimal price)
        {
            Assert.AreEqual(amount,dc.Amount);
            Assert.AreEqual(dirDeal,dc.DirDeal);
            Assert.AreEqual(price,dc.Price);



        }
               
        public void LoadData1()
        {

            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 42, 602), DirDeal = EnmDealDir.Buy, Price = 73000, Amount = 1 });
            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 42, 602), DirDeal = EnmDealDir.Buy, Price = 73001, Amount = 1 });
            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 42, 873), DirDeal = EnmDealDir.Buy, Price = 73000, Amount = 1 });
            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 43, 082), DirDeal = EnmDealDir.Buy, Price = 73000, Amount = 1 });

            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 43, 203), DirDeal = EnmDealDir.Sell, Price = 77571, Amount = 1 });

            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 44, 107), DirDeal = EnmDealDir.Buy, Price = 73040, Amount = 2 });

            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 44, 600), DirDeal = EnmDealDir.Sell, Price = 77545, Amount = 2 });
            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 44, 600), DirDeal = EnmDealDir.Sell, Price = 77543, Amount = 2 });

            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 45, 749), DirDeal = EnmDealDir.Sell, Price = 77557, Amount = 1 });

            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 45, 826), DirDeal = EnmDealDir.Sell, Price = 73020, Amount = 2 });

        }
        public void LoadData2()
        {
            sourceLst.Clear();
            sourceLst.Add(new CDealClass { DtTm = new DateTime(2016, 02, 04, 14, 44, 46, 000), DirDeal = EnmDealDir.Sell, Price = 73020, Amount = 2 });



        }


    }





}
