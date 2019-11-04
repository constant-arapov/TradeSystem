using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;

using TradingLib.Interfaces.Clients;
using TradingLib.Data;
using TradingLib.Data.DB;

using TradingLib.ProtoTradingStructs.TradeManager;



namespace TradingLib.Abstract
{
    public abstract class CBasePosBox : CBaseFunctional
    {

        public Dictionary<string, CRawPosition> DictPos { get; set; }

        public CListPositionInstrTotal ListPos { get; set; }
        
        public IClientPosBox _client;




        public CBasePosBox(IClientPosBox client) : base(client)
        {
            _client = client;

        }



        public void GenListPos(ref bool bNeedUpdate)
        {
         
            
            foreach (var kvp in DictPos)
            {
                string instrument = kvp.Key;

                for (int i = 0; i < ListPos.Lst.Count; i++)
                {
                    if (instrument ==  ListPos.Lst[i].Instrument &&
                        ListPos.Lst[i].Pos != kvp.Value.Pos 
                         /*&& kvp.Value.Pos!=0*/)//2018-05-24
                    {
                        ListPos.Lst[i].Pos = kvp.Value.Pos;
                        bNeedUpdate = true;
                        continue;
                    }
                }


            }
        }

        protected void UpdateTrdMgrTotalPos()
        {
            bool bNeedUpdate = false;
            GenListPos(ref bNeedUpdate);
            if (bNeedUpdate)
            {
                _client.UpdateTotalInstrumentPosition();
            }


        }





        public void CreateListPos(int stockExchId, CListInstruments listInstruments)
        {
            
            ListPos = new CListPositionInstrTotal(stockExchId);
            //changed 2018-05-22
            
            foreach (var inst in listInstruments)
            {

                if (inst.IsViewOnly == 0)
                {
                    ListPos.Lst.Add(new CPositionInstrTotal()
                    {
                        StockExchId = stockExchId,
                        Instrument = inst.instrument,
                        Pos = 0
                    });
                }
            }
            
        }

        public bool IsAllPosClosed()
        {
            CListPositionInstrTotal lstPos = ListPos.GetCopy();
           foreach (var el in lstPos.Lst)
            {
                if (el.Pos != 0)
                    return false;

            }

            return true;
        }






    }
}
