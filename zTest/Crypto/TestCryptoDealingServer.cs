using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Globalization;



using NUnit.Framework;

using Common.Utils;

using BitfinexCommon;
using BitfinexCommon.Enums;



using CryptoDealingServer;



namespace zTest.Crypto
{
    public class TestCryptoDealingServer
    {
        public void Test()
        {

        //    Thread.CurrentThread.CurrentCulture = 
          //      new System.Globalization.CultureInfo("en-us");


            TestGetPriceFromVolume();
            TestGetOrderStatus();
            TestGetRoundTo();
            TestGetIntVolume();
            TestBfxTimes();
            TestPriceDecimals();
			TestStepPrice();
			TestGetDecimalVolume();

            CCryptoDealingServer dealingServer = new CCryptoDealingServer();
            dealingServer.Process();
            Console.ReadKey();
        }


   


        public void TestGetRoundTo()
        {


			Assert.AreEqual(CUtilConv.GetRoundTo(0.1m), 1);
			Assert.AreEqual(CUtilConv.GetRoundTo(0.2m), 1);
			Assert.AreEqual(CUtilConv.GetRoundTo(0.02m), 2);

			Assert.AreEqual(CUtilConv.GetRoundTo(0.002m), 3);
			Assert.AreEqual(CUtilConv.GetRoundTo(0.08m), 2);
			Assert.AreEqual(CUtilConv.GetRoundTo(0.02m), 2);
			Assert.AreEqual(CUtilConv.GetRoundTo(0.6m), 1);


			Assert.AreEqual(CUtilConv.GetRoundTo(190m), 0);
			Assert.AreEqual(CUtilConv.GetRoundTo(0.04m), 2);
			Assert.AreEqual(CUtilConv.GetRoundTo(0.06m), 2);
			Assert.AreEqual(CUtilConv.GetRoundTo(14m), 0);
			Assert.AreEqual(CUtilConv.GetRoundTo(6m), 0);


			Assert.AreEqual(CUtilConv.GetRoundTo(2m), 0);
			Assert.AreEqual(CUtilConv.GetRoundTo(8m), 0);
			Assert.AreEqual(CUtilConv.GetRoundTo(1m), 0);
			Assert.AreEqual(CUtilConv.GetRoundTo(0.4m), 1);
			Assert.AreEqual(CUtilConv.GetRoundTo(300), 0);


        }


        public void TestGetIntVolume()
        {
            //BTC
            //8432,4 0,01|8433,9 0,04404|8437 0,003|8438,6 0,1|8440,5 1|8440,7 1|8440,9 0,65399871|8441 9,11|8443 0,3|8443,
            Assert.AreEqual(760, CUtilConv .GetIntVolume(0.76016001m, 3));
			Assert.AreEqual(10, CUtilConv.GetIntVolume(0.01m, 3));
			Assert.AreEqual(44, CUtilConv.GetIntVolume(0.044m, 3));
			Assert.AreEqual(100, CUtilConv.GetIntVolume(0.1m, 3));
			Assert.AreEqual(654, CUtilConv.GetIntVolume(0.65399871m, 3));

            //TRXUSD 3546,8412
			Assert.AreEqual(3547, CUtilConv.GetIntVolume(3546.8412m, 0));

            //LTUSD  158,25 0,5853136|158,37 15|158,38 16,5|
			Assert.AreEqual(59, CUtilConv.GetIntVolume(0.5853136m, 2));
			Assert.AreEqual(1500, CUtilConv.GetIntVolume(15m, 2));
			Assert.AreEqual(1650, CUtilConv.GetIntVolume(16.5m, 2));

        }

		public void TestGetDecimalVolume()
		{

			//BTCUSD
			Assert.AreEqual(0.002m, CUtilConv.GetDecimalVolume(2, 3));

			//LTCUSD
			Assert.AreEqual(0.08m, CUtilConv.GetDecimalVolume(8, 2));

			//ETCUSD
			Assert.AreEqual(0.6m, CUtilConv.GetDecimalVolume(6, 1));

			//RRTUSD
			Assert.AreEqual(190m, CUtilConv.GetDecimalVolume(190, 0));
			

		}

        public void TestPriceDecimals()
        {
           //BTC
			Assert.AreEqual(1, CUtilConv.GetPriceDecimals(8894.9m));
            //AIDUSD
			Assert.AreEqual(5, CUtilConv.GetPriceDecimals(0.3204m));


        }
		public void TestStepPrice()
		{
			Assert.AreEqual(1, CUtilConv.GetMinStep(0));
			//BTC
			Assert.AreEqual(0.1, CUtilConv.GetMinStep(1));
            //AIDUSD
			Assert.AreEqual(0.00001, CUtilConv.GetMinStep(5));


		}



        public void TestBfxTimes()
        {

           DateTime dt = CUtilTime.DateTimeFromUnixTimestampMillis(1518516579415);

           if (dt != null)
               System.Threading.Thread.Sleep(0);

        }


        public void TestGetOrderStatus()
        {
           
            Assert.AreEqual(EnmBfxOrderStatus.Executed,
                            CBfxUtils.GetOrderStatus("EXECUTED @ 586.86(0.05): was PARTIALLY FILLED @ 586.06(0.85)"));
          
            Assert.AreEqual(EnmBfxOrderStatus.Executed,
                            CBfxUtils.GetOrderStatus("EXECUTED @ 10904.0"));


            Assert.AreEqual(EnmBfxOrderStatus.Canceled,
                          CBfxUtils.GetOrderStatus("CANCELED"));

       
        }


        public void TestGetPriceFromVolume()
        {

                 
             Assert.AreEqual(10908.0m,
                         CBfxUtils.GetDealPriceFromStatus("EXECUTED @ 10908.0(-0.0)"));


            Assert.AreEqual(10904.0m,
                         CBfxUtils.GetDealPriceFromStatus("EXECUTED @ 10904.0(0.0)"));


          Assert.AreEqual(586.86m,
                         CBfxUtils.GetDealPriceFromStatus("EXECUTED @ 586.86(0.05)"));




          Assert.AreEqual(586.86m,
                         CBfxUtils.GetDealPriceFromStatus("EXECUTED @ 586.86(0.05)"));

            
          Assert.AreEqual(586.06m, 
                            CBfxUtils.GetDealPriceFromStatus("PARTIALLY FILLED @ 586.06(0.85)"));

          Assert.AreEqual(586.86m,
                         CBfxUtils.GetDealPriceFromStatus("EXECUTED @ 586.86(0.05): was PARTIALLY FILLED @ 586.06(0.85)"));

         
        }

       





    }
}
