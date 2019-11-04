using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terminal.TradingStructs
{
    //TODO refact or simplifying
    public class PortfolioGraphicData 
    {
       // public PortfolioGraphicData();

        public int CurrentPositionAmount { get; set; }
        public double CurrentPositionPrice { get; set; }
        //public GraphicDataType DataType { get; }
        public double FixPrice { get; set; }
        //public ParameterAvailability FixPriceAvailability { get; }
        public bool IsEmpty { get; set; }
        public bool IsOrdersSorted { get; set; }
        public int MemorySize { get; set; }
        public int OwnedOrdersCount { get; set; }
        //public RecalckProfitState ProfitRecalculateState { get; }
        public double StopPrice { get; set; }
        //public ParameterAvailability StopPriceAvailability { get; }
        public double TargetPrice { get; set; }
        //public ParameterAvailability TargetPriceAvailability { get; }
        public DateTime UpdateDateTime { get; set; }

        //public void AddOwnedOrder(double _Price, int _Amount, OrderAction _Action, long _OrderID, PortfolioOwnedOrderState _State, bool _CanCancel);
        //public void ClearOwnedOrders();
        //public void CopyFrom(PortfolioGraphicData _PortfolioData);
        //public PortfolioOwnedOrder GetOwnedOrder(int Index);
        //public void Load(byte* Buffer);
        //public void ReinitializeWith(int _PositionAmount, double _PositionPrice);
        //public void ReinitializeWith(int _PositionAmount, double _PositionPrice, ParameterAvailability _TargetPriceAvailability, double _TargetPrice);
        //public void ReinitializeWith(int _PositionAmount, double _PositionPrice, ParameterAvailability _TargetPriceAvailability, double _TargetPrice, ParameterAvailability _StopPriceAvailability, double _StopPrice);
        //public void ReinitializeWith(int _PositionAmount, double _PositionPrice, ParameterAvailability _TargetPriceAvailability, double _TargetPrice, ParameterAvailability _StopPriceAvailability, double _StopPrice, ParameterAvailability _FixPriceAvailability, double _FixPrice);
        //public void ReinitializeWith(int _PositionAmount, double _PositionPrice, ParameterAvailability _TargetPriceAvailability, double _TargetPrice, ParameterAvailability _StopPriceAvailability, double _StopPrice, ParameterAvailability _FixPriceAvailability, double _FixPrice, RecalckProfitState _RecalckState);
        //protected void ResizeOwnedOrdersArray(int ByCapacity);
        //public short Save(byte* Buffer);
        //public void SetOwnedOrderTag(int Index, int _Tag);
        //public void SortOwnedOrdersByPriceUp(bool ClearTags);
    }
}
