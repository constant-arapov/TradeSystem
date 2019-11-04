using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;

namespace Terminal.Controls.Market.Settings
{
    public class CSettingsControlStock
    {
        

        public int AmountFilter { get; set; }
        public string AmountStyle { get; set; }
        public Color AsksColor { get; set; }
     
        public Color BidsColor { get; set; }
        public int FilledAt { get; set; }
        public double PanelWidth { get; set; }
        public int RenewSpeed { get; set; }
        public int StringHeight { get; set; }
    }
}
