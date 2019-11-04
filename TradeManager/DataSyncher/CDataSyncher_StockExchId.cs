using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Windows.Data;

using Common.Interfaces;

using TradingLib.ProtoTradingStructs.TradeManager;

using TradeManager.Interfaces.Clients;
using TradeManager.Interfaces.Keys;

namespace TradeManager.DataSyncher
{
    public class CDataSyncher_StockExchId  <TModelElement, TVMElement>:
                    CBaseDataSyncher<TModelElement, TVMElement> 
                    where TModelElement : CBaseTrdMgr_StockExchId
                    where TVMElement : IKey_StockExch

            
                 
    {


        public CDataSyncher_StockExchId(IClientDataSyncher client,  
                                        Func<TModelElement, TVMElement> createVMElement,
                                        Dispatcher guiDisp)
            :base(client, EnmCodeKeys._01_StockExchId, createVMElement, guiDisp)
        {
         

        }

        public void UpdateFilter()
        {
            _collViewSourceVM.Filter += new FilterEventHandler(_collViewSourceVM_Filter);

        }

        void _collViewSourceVM_Filter(object sender, FilterEventArgs e)
        {
            IKey_StockExch keyStockExch = (IKey_StockExch)e.Item;
            if (keyStockExch.StockExchId == 4)
                e.Accepted = true;
            else
                e.Accepted = false;
        }
        



    }
}
