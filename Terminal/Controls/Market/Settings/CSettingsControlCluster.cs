using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terminal.Controls.Market.Settings
{
    public class CSettingsControlCluster
    {
        

        public string ClusterStyleColor { get; set; }
  
        public int FilledAt { get; set; }
        public double GridWidth { get; set; }
        public double PanelWidth { get; set; }
        public int PercentsForColorGradient { get; set; }
        public bool SaveClusters { get; set; }
        public bool ShowClusterPanel { get; set; }
   
       
    }
}
