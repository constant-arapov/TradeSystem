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
    public class CDataSyncher_SockExch_Instr<TModelElement, TVMElement> :
                    CBaseDataSyncher<TModelElement, TVMElement>
                    where TModelElement : CBaseTrdMgr_StockExch_InstrId
                    where TVMElement : IKey_StockExch_Inst
    {

        public CDataSyncher_SockExch_Instr(IClientDataSyncher client,
                                        Func<TModelElement, TVMElement> createVMElement,
                                        Dispatcher guiDisp)
            :base(client, EnmCodeKeys._03_StockExchId_Instrument, createVMElement, guiDisp)
        {
            

        }

    }





}
