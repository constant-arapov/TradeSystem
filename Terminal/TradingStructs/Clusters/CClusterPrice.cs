using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Terminal.TradingStructs;

using Common.Collections;

namespace Terminal.TradingStructs.Clusters
{
    public class CClusterPrice : CDictL2Simple<double, DateTime, CCluster> 
    {

        public void UpdateCluster(DateTime dt, CDeal deal)
        {

            lock (this)
            {
                CreateIfNotExists(deal.Price, dt);
                this[deal.Price][dt].Update(deal);
            }

        }

        public void UpdateCluster(double price, DateTime dt, CCluster cluster)
        {
            lock (this)
            {
                CreateIfNotExists(price, dt);
                this[price][dt].Update(cluster);
            }

        }




		public CCluster GetClusterData(double price, DateTime dt)
        {
            CCluster value = null;

            lock (this)
            {
             
                if (IsExists(price, dt))
                    return this[price][dt];

            }
            return value;
        }

        public CCluster GetAmountInPriceArea(double priceFrom, double priceTo, DateTime dt)
        {

            CCluster cluster = null;
            lock (this)
            {
                foreach (var kvp in this)
                {
                    double price = kvp.Key;
                    if (price > priceFrom && price <= priceTo)
                    {


                        if (kvp.Value.ContainsKey(dt))
                        {

                            if (cluster == null)
                                cluster = new CCluster();

                            cluster.AmountTotal += kvp.Value[dt].AmountTotal;
                            cluster.AmountBuy += kvp.Value[dt].AmountBuy;
                            cluster.AmountSell += kvp.Value[dt].AmountSell;

                            cluster.TotalDir =
                                cluster.AmountBuy > cluster.AmountSell ? EnmDealDirection.Buy : EnmDealDirection.Sell;

                        }
                    }
                }

            }

            return cluster;

        }
       
        //not used check and remove
		public int GetAmountTotal(double price, DateTime dt)
		{			
			CCluster clusterData = GetClusterData(price, dt);
			if (clusterData != null)
				return clusterData.AmountTotal;

			return 0;

		}

		public List<CDeal> GetDealsList()
		{
			List<CDeal> lstDeals = new List<CDeal>();
            lock (this)
            {
                foreach (KeyValuePair<double, Dictionary<DateTime, CCluster>> kvp in this)
                {
                    double price = kvp.Key;

                    foreach (KeyValuePair<DateTime, CCluster> pair2 in kvp.Value)
                    {
                        CCluster cluster = pair2.Value;


                        if (cluster.AmountBuy > 0)
                        {
                            CDeal item = new CDeal
                            {
                                Price = kvp.Key,
                                DateTime = pair2.Key,
                                Direction = EnmDealDirection.Buy,
                                Amount = cluster.AmountBuy,
                            };
                            lstDeals.Add(item);
                        }


                        if (cluster.AmountSell > 0)
                        {

                            CDeal item = new CDeal
                            {
                                Price = kvp.Key,
                                DateTime = pair2.Key,
                                Direction = EnmDealDirection.Sell,
                                Amount = cluster.AmountSell,
                            };
                            lstDeals.Add(item);

                        }


                    }
                }
            }

			return lstDeals;
		}


    }
}
