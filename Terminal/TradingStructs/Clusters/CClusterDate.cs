using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Terminal.TradingStructs;


using Common;
using Common.Collections;

namespace Terminal.TradingStructs.Clusters
{
    public class CClusterDate : CDictL1Simple<DateTime, CCluster>
    {

        /// <summary>
        /// Call from CClusterProcessor.UpdateNewestClusters
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="deal"></param>
        public void UpdateCluster(DateTime dt, CDeal deal)
        {

            CreateIfNotExists(dt);
            
            this[dt].Update(deal);
       
        }

        /// <summary>
        /// Call from CClusterProcessor.RebuildCurrentClusters
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="cluster"></param>
        public void UpdateCluster(DateTime dt, CCluster cluster)
        {
            CreateIfNotExists(dt);
            this[dt].Update(cluster);


        }



        public CCluster GetValue(DateTime dt)
        {
            if (this.ContainsKey(dt))
                return this[dt];

            return null;
        }


    }
}
