using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Interfaces.Clients;


namespace CryptoDealingServer.Interfaces
{
    public interface IClientSessionBoxCrypto : IClientSessionBox
    {
        void OnClearingProcessed();
        void UpdateTurnOver();
        void UpdateFeeTurnoverCoefs();
    }
}
