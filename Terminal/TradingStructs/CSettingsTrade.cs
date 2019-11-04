using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terminal.TradingStructs
{
    public class CSettingsTrade
    {
       

        public string AveragingMethod { get; set; }
        public int BackAmount { get; set; }
        public int DecreaseAmount { get; set; }
        public int First_IncreaseAmount { get; set; }
        public int First_WorkAmount { get; set; }
        public int MaxPosition { get; set; }
        public int Second_IncreaseAmount { get; set; }
        public int Second_WorkAmount { get; set; }
        public int StopLoss_Steps { get; set; }
        public int TakeProfit_Steps { get; set; }
        public int Third_WorkAmount { get; set; }
        public int ThrowLimitTo { get; set; }
    }
}
