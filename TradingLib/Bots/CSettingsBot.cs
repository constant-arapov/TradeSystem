using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.ComponentModel;

using Common;



namespace TradingLib.Bots
{
    public class CSettingsBot : CBaseProppertyChanged
    {


        public CSettingsBot(enmStrategysCode stratCode, bool enabled, List <string> listIsins,  Dictionary<string, CBotLimits> dictBotIsinLimits, 
                            Dictionary <string, CTradingSettings> dictTradingSettings, bool isExternal, bool needTFAnalyzer, decimal maxLossVMClosedTotal)
        {
            ListIsins = listIsins;
            StrategyCode = stratCode;

            DictBotIsinLimits = dictBotIsinLimits;
            TradingSettings = dictTradingSettings;

            Enabled = enabled;
            IsExternal = isExternal;
            NeedTFAnalyzer = needTFAnalyzer;
            MaxLossVMClosedTotal = maxLossVMClosedTotal;

        }


        private bool _enabled;
        public bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                _enabled = value;
                RaisePropertyChanged("Enabled");

            }


        }

        public Dictionary<string, CTradingSettings> TradingSettings;
        public List<string> ListIsins;

        public enmStrategysCode StrategyCode;

        public object StrategySettings;
        public Dictionary<string, CBotLimits> DictBotIsinLimits;

        public bool IsExternal {get; set;}
        public bool NeedTFAnalyzer { get; set; }
        public decimal MaxLossVMClosedTotal { get; set; }

    }


    public enum enmStrategysCode
    {

        StrategyTest,
        StrategySupervisor,
        StrategyHighLowContra,
        StrategyTesterPos,
        StrategyTesterExternal,
        StrategyTesterCrossFirst,
        StrategyTesterCrossSecond,
        StrategyTesterLimits,
        StrategyTrader



    }

    public class CBotLimits
    {

        public CBotLimits(int maxSendOrderRuntime, int maxPosition, int maxAddedOrder, decimal maxLossVM)
        {

            MaxSendOrderRuntime =  Convert.ToInt32(maxSendOrderRuntime);
            MaxPosition = Convert.ToInt32(maxPosition);
            MaxAddedOrder = Convert.ToInt32(maxAddedOrder);
            MaxLossVM = Convert.ToDecimal(maxLossVM);


        }

      public int MaxSendOrderRuntime;
      public int MaxPosition;
      public int MaxAddedOrder;
      public decimal MaxLossVM;


    }
    public class CTradingSettings
    {
        public CTradingSettings(long stoploss, long takeProfit, int lot)
        {
            StopLoss = stoploss;
            TakeProfit = takeProfit;
            Lot = lot;
        }
        public long StopLoss;
        public long TakeProfit;
        public int Lot;

    }

}
