using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Enums;
using TradingLib.BotEvents;

using Common.Interfaces;

using BitfinexCommon.Messages.Response;


namespace CryptoDealingServer.Interfaces
{
    public interface IClientUserOrderBoxCrypto : ILogable
    {
        void TriggerRecalculateBot(int botId, string isin, EnmBotEventCode code, object data);
        int GetDecimalVolume(string instrument);

        void UpdateDealsPos(string instrument, decimal price, EnmOrderDir dir,
                            int amount, long extId, 
                            DateTime moment, long mtsUpdate,  decimal fee);


		int StockExchId { get; set; }

        void CheckUnProcessedDeals();
        void CheckForDealsWithNoBotId(long orderId, int BotId);
    }
}
