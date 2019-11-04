using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using Terminal.TradingStructs.Clusters;

namespace Terminal.Graphics
{
    public class CDrawCluster
    {
        public CCluster Cluster;
        public double Y;
        public DateTime Dt;
        public double Width;       



        public CDrawCluster Copy()
        {       
            CDrawCluster copy = (CDrawCluster)this.MemberwiseClone();
            copy.Cluster = Cluster.Copy();//TODO check if we need to do it
            return copy;
        }


    }
}
