using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using TradingLib.Enums;
using TradingLib.Bots;

namespace TradingLib.Interfaces
{
    public interface IBotTraderOperations
    {

        void CloseByTakeProfit(string instrument);
        void CloseByStopLoss(string instrument);
        void CallbackUpdateStopLossTakeProfit(string instrument);
        void CallbackUpdateStopOrders(string instrument);        
        void SendStopLossInvert(string instrument, CBotPos pos);
        void TriggerStopOrder(string instrument, EnmOrderTypes ordType, decimal amount);
		void CancellOrdersByPriceAndDir(string instrument, EnmOrderDir dir, decimal price);

        void ClosePositionOfInstrument(string instrument);
        void CancellAllOrdersByInstrument(string instrument);
        bool AddOrder(string isin, decimal price, EnmOrderDir dir, decimal amount);
    }
}
