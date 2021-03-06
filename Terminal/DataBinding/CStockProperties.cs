﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terminal.DataBinding
{
    public class CStockProperties
    {
      //  public string UserPos { get; set; }
        public string Asks { get; set; }
        public string Bids { get; set; }
        public string Orders { get; set; }
        public string Step { get; set; }
        //public string Portfolio { get; set; }
        public string TickerName { get; set; }
        public string Decimals { get; set; }
        public string StringHeight { get; set; }
        public string FontSize { get; set; }
        public string VolumeFullBar { get; set; }
        public string BigVolume { get; set; }
        public string IsInControlDeals { get; set; }
        public string Level1Y { get; set; }
        public string Level2Y { get; set; }
        public string StockNum { get; set; }
		public string StopLossPrice { get; set; }
		public string TakeProfitPrice { get; set; }
        public string StopLossInvertPrice { get; set; }
        public string BuyStopPrice { get; set; }
        public string SellStopPrice { get; set; }
        public string BuyStopAmount { get; set; }
        public string SellStopAmount { get; set; }
        public string SelectionMode { get; set; }
		public string SelectedY { get; set; }
        public string SelectedPrice { get; set; }        
        public string IsNeedRepaintDeals { get; set; }
		public string UserLevels { get; set; }
		public string ThrowSteps { get; set; }
        public string CurrAmountNum { get; set; }
        public string Level1Mult { get; set; }
        public string Level2Mult { get; set; }
        public string IsConnectedToServer { get; set; }
        public string DecimalVolume { get; set; }
    }
}
