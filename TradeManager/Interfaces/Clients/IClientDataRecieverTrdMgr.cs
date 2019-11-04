using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib.ProtoTradingStructs;


namespace TradeManager.Interfaces.Clients
{
    public interface IClientDataRecieverTrdMgr  : IAlarmable
    {
		void AuthResponse(CAuthResponse aresp, int connId);
        IClientCommuTradeManager DataUser { get; }

        void OnDisconnect(int conId);
    }
}
