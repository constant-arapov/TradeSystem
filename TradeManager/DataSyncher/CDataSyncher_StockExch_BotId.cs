using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Threading;


using Common.Interfaces;

using TradingLib.ProtoTradingStructs.TradeManager;

using TradeManager.Interfaces.Clients;
using TradeManager.Interfaces.Keys;

namespace TradeManager.DataSyncher
{
    public class CDataSyncher_StockExch_BotId<TModelElement, TVMElement> :
                    CBaseDataSyncher<TModelElement, TVMElement> 
                        where  TModelElement : CBaseTrdMgr_StockExch_BotId
                        where TVMElement : IKey_StockExch_Bot
                       
    {


        


        public CDataSyncher_StockExch_BotId(IClientDataSyncher client, 
                                        Func<TModelElement, TVMElement> createVMElement, Dispatcher guiDisp)
            : base(client, EnmCodeKeys._02_StockExchId_BotId, createVMElement, guiDisp)
        {


          
         

            
        }

      



    }
}
