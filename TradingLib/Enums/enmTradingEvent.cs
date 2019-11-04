using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Enums
{
    //NOTE ! Add only to the end of enum

    public enum enmTradingEvent : byte
    {
        StockUpadate =1,
        DealsUpdate,
        SynchroniseTime,
        AddOrder,
        AuthRequest,
        AuthResponse,
        UserOrdersUpdate,
        CancellOrderById,
        CancellOrdersByIsin,
        CancellAllOrders,
        CloseAllPositions,
        CloseAllPositionsByIsin,
        UserUpdatePositionMonitor,
        UserUpdatePosLog,
        UserUpdateDealsLog,
        UserUpdateVM,
        UserUpdateAvailableTickers,
        UserSubscribeTicker,
        UpdateMoneyData,       
        SetStoplossTakeProfit,
        StopLossTakeProfitAccepted,
		SendOrderThrow,
		InvertPosition,
        SendOrderRest,
        PositionInstrTotal,
        BotStatus,
        BotPosTrdMgr,
		EnableBot,
		DisableBot,
        CloseBotPosTrdMgr,
		UpdateInstrumentParams,
		UpdateUserPosLogLate,
        ClientInfo,
        SendReconnect


    }
}
