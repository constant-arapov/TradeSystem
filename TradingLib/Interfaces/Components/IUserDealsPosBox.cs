using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Collections;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Enums;
using TradingLib.Common.VMCalc;
using TradingLib.Data;
using TradingLib.ProtoTradingStructs;



using System.Threading;

namespace TradingLib.Interfaces.Components
{
    public interface IUserDealsPosBox
    {
        decimal CalcBotVMOpenedAndClosed(int BotId, string isin, ref decimal vmClosed, ref bool bFoundClosed,
                                                    ref decimal vmOpenedPos);

        decimal CalcBotVMClosed(int BotId, string isin, ref decimal vmClosed, ref bool bFound);

       
		IClientUserDealsPosBox UserDealsPosBoxClient { get; }
		decimal BrokerFeeCoef { get; }
		Dictionary<int, decimal> AcconuntsFeeProc { get; set; }
		CDict_L2_List<int, string, CBotPos> DicBotPosLog { get; set; }
		CDict_L2_List<int, string, CUserDeal> DictUserDealsLog { get; set; }
		Dictionary<int, Dictionary<string, CBotPos>> DictPositionsOfBots { get; set; }

		void LoadTradeData();
		decimal USDRate { get; }
		decimal GetBid(string instrument);
		decimal GetAsk(string instrument);
		//void Update(USER_DEAL.user_deal deal);
		void CleanUserPosLog();
       
		void RefreshBotPos(string instrument);
        long GetLotSize(string instrument);
        CBaseVMCalc VMCalc { get; }

		int GetDecimalVolume(string instrument);

        bool IsAllPosClosed();
        decimal GetSessionProfit();
        void BindDealBotPos(long dealId, long bpDtOpenTimestampMs);
        void OnAddNewBot(int botId);
    }
}
