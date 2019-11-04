using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terminal.TradingStructs
{
    public struct OrderData
    {
        public EnmOrderAction Action;
        public decimal Amount;
        public bool CanCancel;
        public long OrderID;
        public double Price;
        public PortfolioOwnedOrderState State;
        public int Tag;
    }
}
