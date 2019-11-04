using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terminal.ViewModels
{
    public class CSelectionMode
    {
        public bool IsModeDrawLevel = false;
        public bool IsModeStopLossTakeProfit = false;
        public bool IsModeStopLossInvert = false;
        public bool IsModeStopOrder = false;
        public bool IsModeRestOrder = false;


        private CSelectionMode Copy()
        {
            return new CSelectionMode()
            {
                IsModeDrawLevel = this.IsModeDrawLevel,
                IsModeRestOrder = this.IsModeRestOrder,
                IsModeStopLossInvert = this.IsModeStopLossInvert,
                IsModeStopLossTakeProfit = this.IsModeStopLossTakeProfit,
                IsModeStopOrder = this.IsModeStopOrder

            };
        }

        public void ResetAllModes()
        {
            IsModeDrawLevel = false;
            IsModeStopLossTakeProfit = false;
            IsModeStopLossInvert = false;
            IsModeStopOrder = false;
            IsModeRestOrder = false;
            
        }


    }
}
