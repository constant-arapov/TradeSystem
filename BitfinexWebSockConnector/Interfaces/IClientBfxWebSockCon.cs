using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib;
using TradingLib.Interfaces.Clients;
using TradingLib.Enums;
using TradingLib.Data;

using BitfinexCommon.Messages.Response;


namespace BitfinexWebSockConnector.Interfaces
{
    public interface IClientBfxWebSockCon : IAlarmable,  IClientSharedStock
    {

        void UpdateStockBothDir(string instrument, int precision, CSharedStocks stock);
        void UpdateStockOneDir(string instrument, Direction dir, int precision, CSharedStocks stock);
        void UpdateDeal(string instrument, CRawDeal rd);

        void ProcessOrder(ResponseOrders respOrders, EnmOrderAction ordAction);

		void UpdateUserDeals(ResponseTrades rt);
		void UpdateUserDealsLateUpd(ResponseTrades rt);
        void UpdatePos(ResponsePositions respPos);

        void PeriodicActBfxAuth();

        void UpdateWallet(string walletType, string currency, decimal balance);
       
    }
}
