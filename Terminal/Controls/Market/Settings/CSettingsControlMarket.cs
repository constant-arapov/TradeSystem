using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Terminal.TradingStructs;

namespace Terminal.Controls.Market.Settings
{
    public class CSettingsControlMarket
    {
    
        public CSettingsControlCluster SetttingsControlCluster { get; set; }
        public CSettingsControlStock SettingsControlStock { get; set; }
        public CSettingsControlDeals SettingsControlDeals { get; set; }
        public CSettingsTrade SettingsTrade { get; set; }
    }
}
