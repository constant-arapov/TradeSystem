using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib;



namespace Visualizer
{
    //TODO move to terminal
	public interface ITradeOperations
	{

        void AddOrder(string isin, int amount, EnmOrderDir dir, decimal price);
        void CancellOrdersWithPrice(double price);
        void CancellAllOrders();
        void CloseAllPositions();
        void CloseAllPositionsByIsin(string isin);

        void ChangeInstrument();
        void DeleteInstrument();


        //tempo TODO normal
       // string TickerName { get; }


	}
}
