using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Terminal.TradingStructs;





namespace Terminal.TradingStructs.Clusters
{
	public class  CCluster
	{
		public int AmountTotal;
		public int AmountBuy;
		public int AmountSell;
		public EnmDealDirection TotalDir;




        public void Update(CDeal deal)
        {
            AmountTotal += deal.Amount;

            if (deal.Direction == EnmDealDirection.Buy)
                AmountBuy += deal.Amount;
            else
                AmountSell += deal.Amount;


            if (AmountSell > AmountBuy)
                TotalDir = EnmDealDirection.Sell;
            else
                TotalDir = EnmDealDirection.Buy;

        }

        public void Update(CCluster cluster)
        {
            AmountTotal += cluster.AmountTotal;

         
            AmountBuy += cluster.AmountBuy;

            AmountSell += cluster.AmountSell;


            if (AmountSell > AmountBuy)
                TotalDir = EnmDealDirection.Sell;
            else
                TotalDir = EnmDealDirection.Buy;

          
        }

        public  CCluster Copy ()
        {
            return (CCluster)MemberwiseClone();

        }
     





	}
}
