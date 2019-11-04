using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib.Data.DB;
using TradingLib.GUI;


using ASTS;

using ASTS.DealingServer;

using ASTS.DealingServer.Session;
using ASTS.DealingServer.Stocks;

namespace ASTS.Interfaces.Interactions
{
    public interface IDealingServerForASTSConnector : IAlarmable, ILogable, IDealingServerForASTSAllTrades, 
                                                      IDealingServerFotClientRM_Holds, IDealingServerForPositions, IDealingServerForTableOrders,
                                                      IDealingServerForTrades, IDealingServerForTableExt_OrderBook, IDealingServerForTableSysTime
                                                       
	{
        
        int StockExchId { get; set; }
      
		CStockBoxASTS StockBoxInp { get; }
        CSessionBoxASTS SessionBoxInp { get; }
        CGUIBox GUIBox { get; set; }
        CUserOrderBoxASTS UserOrderBoxInp {get;}
	    CPosistionsBoxASTS PosistionsBoxInp {get;}
        CUserDealsPosBoxASTS UserDealsPosBoxInp { get; }

        string LoadStockExchPassword(string login);
        string NewPassword { get; }

        void SaveNewPassword(string login);


	}
}
