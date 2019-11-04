using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Interfaces.Clients;


namespace Plaza2Connector.Interfaces
{
    public interface IClientSessionBoxP2 : IClientSessionBox
    {
        void OnEveningClearingBegin();
        void OnEveningClearingEnd();
        void OnIntradayClearingEnd();
    }
}
