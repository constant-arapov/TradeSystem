using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Threading;


using Terminal.TradingStructs;
using Terminal.ViewModels;



namespace zTest
{
    class TestClusterProcessor
    {



		public void Test2()
		{

			CClusterProcessor clusterProcessor = new CClusterProcessor("test_instrument1", "M5");
			List <CDeal> _lstDeals = new List<CDeal>();
			_lstDeals.Add(new CDeal { Amount = 1, Price = 1000, Direction = EnmDealDirection.Sell });


			int count = 60;
			for (int i = 0; i < count; i++ )
				foreach (CDeal deal in _lstDeals)
				{
					clusterProcessor.Update(deal);
					Thread.Sleep(1000);

				}

			Thread.Sleep(20000);



		}


        public void Test1()
        {

            CClusterProcessor clusterProcessor = new CClusterProcessor("test_instrument1","M5");

            while (!clusterProcessor.IsDataLoaded)
                Thread.Sleep(10);

            //List <CDeal> _lstDeals = new List<CDeal>();
           // _lstDeals.Add(new CDeal { Amount = 1, Price = 1000, Direction = EnmDealDirection.Sell, DateTime = new DateTime(2017, 01, 27, 10, 25, 0) });

            //System.Threading.Thread.Sleep(10000);


            const int _parSecTime1 = 60;//6 * 60;
               const int _parSleepSec = 1;



               DateTime dtBegin = DateTime.Now;
               
               int priceInc = 1;

               double minCyclePrice = 11500;
               double price = minCyclePrice ;
               double maxCyclePrice = 11700; 


               while ((DateTime.Now - dtBegin).TotalSeconds < _parSecTime1)
               {
                   price += priceInc;
                   if (price > maxCyclePrice)
                       price = minCyclePrice;

                   CDeal deal = new CDeal
                   {
                       Amount = 1,
                       DateTime = DateTime.Now,
                       Direction = EnmDealDirection.Sell,
                       Price = price
                   }

                   ;
                   clusterProcessor.Update(deal);
                  //Thread.Sleep(_parSleepSec);


               }

          //     clusterProcessor.ChangeTimeFrame(5);



            Thread.Sleep(1000000);
           

        }



    }
}
