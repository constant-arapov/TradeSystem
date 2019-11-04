using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Threading;


using Common;
using Common.Interfaces;


using TradingLib.ProtoTradingStructs.TradeManager;


using TradeManager.Interfaces.Clients;
using TradeManager.DataSyncher;
using TradeManager.ViewModels;

using TradingLib.ProtoTradingStructs;

namespace TradeManager.DataSource
{
    public abstract class CBaseDataSource : CBaseFunctional, IClientDataSyncher
    {

        protected CDataSyncher_StockExch_Bot_InstrId <CBotPosTrdMgr, VMBotPosTrdMgr> _dataSynchBotPosTrdMgr;
        protected CDataSyncher_StockExch_BotId<CBotStatus, VMBotStatus> _dataSynchBotStatus;
        protected CDataSyncher_SockExch_Instr<CPositionInstrTotal, VMPosInstrTotal> _dataSynchPosInstrTotal;
        protected CDataSyncher_StockExch_BotId<CClientInfo, VMClientInfo> _dataSyncherClientInfo;

       
        public CBaseDataSource(IAlarmable client, Dispatcher guiDispatcher)
            : base(client)
        {


            _dataSynchBotPosTrdMgr =
            new CDataSyncher_StockExch_Bot_InstrId <CBotPosTrdMgr, VMBotPosTrdMgr>(this,                                                                          
                                                             VMBotPosTrdMgr.Create, guiDispatcher);

            _dataSynchBotStatus =
                new CDataSyncher_StockExch_BotId<CBotStatus, VMBotStatus>(this,
                                                                        VMBotStatus.Create, guiDispatcher);


            _dataSynchPosInstrTotal =
                new CDataSyncher_SockExch_Instr<CPositionInstrTotal, VMPosInstrTotal>(this,
                                                                                      VMPosInstrTotal.Create,
                                                                                      guiDispatcher);

            _dataSyncherClientInfo =
               new CDataSyncher_StockExch_BotId<CClientInfo, VMClientInfo>(this,
                                                                          VMClientInfo.Create,
                                                                          guiDispatcher);



        }



        public abstract bool IsStockExchSelected(int stockExhId);

    }
}
