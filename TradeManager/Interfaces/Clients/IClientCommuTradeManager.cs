using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib.ProtoTradingStructs;
using TradingLib.ProtoTradingStructs.TradeManager;

using TradeManager.ViewModels;

namespace TradeManager.Interfaces.Clients
{
	public interface IClientCommuTradeManager : IAlarmable		
	{
		void UpdateDealingServersAuthStat(CAuthResponse aresp, int connId);
        void UpdatePositionInstrTotal(CListPositionInstrTotal listPosInstrTotal);
        void UpdateListBotStatus(CListBotStatus listBotStatus);
        void UpdateBotPosTrdMgr(CListBotPosTrdMgr listBotPosTrdInstrument);
        void UpdateClientInfo(CListClientInfo listClietInfo);
        void OnConnectionDisconnect(int conId);


        VMAccount VMAccount { get; set; }

	}
}
