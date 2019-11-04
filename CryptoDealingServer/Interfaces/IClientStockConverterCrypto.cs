using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Interfaces.Clients;


namespace CryptoDealingServer.Interfaces
{
    public interface IClientStockConverterCrypto  : IClientStockConverter
    {

        int GetCurrentPriceDecimals(string instruments);
        void UpdatePriceDecimals(string instrument, int newPriceDecimals);

		decimal GetMinStep(string instruments);
		void UpdateCurrentMinSteps(string instrument, decimal newPriceDecimals);

		void TriggerUpdateInstrumentParams(string instrument);

        List<int> GetPricePrecisions();
    }
}
