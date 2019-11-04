using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Threading;



using Common.Interfaces;
using TradingLib.ProtoTradingStructs.TradeManager;

using TradeManager.Interfaces.Clients;
using TradeManager.Interfaces.Keys;

namespace TradeManager.DataSyncher
{
    public class CDataSyncher_StockExch_Bot_InstrId<TModelElement, TVMElement> :
                    CBaseDataSyncher<TModelElement, TVMElement> 
                        where TModelElement : CBaseTrdMgr_StockExch_Bot_InstrId
                        where TVMElement : IKey_StockEch_Bot_Inst
    {
        public CDataSyncher_StockExch_Bot_InstrId(IClientDataSyncher client,
                                                    Func<TModelElement, TVMElement> createVMElement,
                                                    Dispatcher guiDispatcher    )

            : base(client, EnmCodeKeys._04_StockEchId_BotId_Instrument, createVMElement,
                    guiDispatcher)            
        {
            

        }

       


    }
}
