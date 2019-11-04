using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib.ProtoTradingStructs;
using TradingLib.ProtoTradingStructs.TradeManager;


using TradeManager.Interfaces.Clients;

using TradeManager.ViewModels;



namespace zTest
{
    public class MockTradeMgr : IClientCommuTradeManager
    {

        public VMAccount VMAccount { get; set; }


        public void Error(string message, Exception exc = null)
        {
            new NotImplementedException();


        }

        public void UpdatePositionInstrTotal(CListPositionInstrTotal listPosInstrTotal)
        {


        }


        public  void UpdateDealingServersAuthStat(CAuthResponse aresp, int conId)
        {

            
        }

        public void UpdateListBotStatus(CListBotStatus listBotStatus)
        {
        
        }

        public void UpdateBotPosTrdMgr(CListBotPosTrdMgr listBotPosTrdInstrument)
        {

        }

        public void UpdateClientInfo(CListClientInfo listClietInfo)
        {


        }

        public  void OnConnectionDisconnect(int conId)
        {


        }


        public  bool IsStockExchSelected(int stockExhId)
        {
            return false;
        }


    }
}
