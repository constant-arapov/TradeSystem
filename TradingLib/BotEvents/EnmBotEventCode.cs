using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.BotEvents
{
    public enum EnmBotEventCode
    {
        OnEmptyEvent,
        OnSessionUpdate,
        OnStockUpdate,
        OnPostionUpdate,
        OnDeals,
        OnOrderAccepted,
        OnOrderAdded,
        OnOrderCancel,
        OnOrderDeal,
        OnUserDeal,
        OnUserOrdersOnline,
        OnPositionOnline,
        OnUserDealOnline,
        OnSessionOnline,
        OnErrorAddOrder,
        OnErrorCancelOrder,
        OnTimer,
        OnTFUpdate,
        OnTFChanged,
        OnTraderConnected,
		OnTraderDisconnected,
        OnSetStopLossTakeProfit,
        OnTimeDisableTradeLoaded,
        OnCrossOrderReply,
        OnForceUpdTrdMgr,
        OnForceUpdTotalVM,
        OnSessionEnd

    }
}
