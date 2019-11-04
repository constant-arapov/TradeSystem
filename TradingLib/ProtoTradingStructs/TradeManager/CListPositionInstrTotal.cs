using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;


namespace TradingLib.ProtoTradingStructs.TradeManager
{
    [ProtoContract]
    public class CListPositionInstrTotal : CBaseTrdMgr_StockExchId
    {

        [ProtoMember(1)]
        public List <CPositionInstrTotal> Lst {get;set;}



        public CListPositionInstrTotal()
        {
            Lst = new List<CPositionInstrTotal>();
        }

        public CListPositionInstrTotal(int stockExchId) : this ()
        {
           
            StockExchId = stockExchId;

        }




        public CListPositionInstrTotal GetCopy()
        {
            lock (this)
            {
                CListPositionInstrTotal copy = new CListPositionInstrTotal();
                //2018-05-24
                var lstNotNull = this.Lst.FindAll(el => el.Pos != 0);

                //copy.Lst = new List<CPositionInstrTotal>(this.Lst);
                copy.Lst = new List<CPositionInstrTotal>(lstNotNull);
                copy.StockExchId = this.StockExchId;
                return copy;
            }
        }


    }

  






}
