using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;


using TradingLib.ProtoTradingStructs;





namespace TradingLib.ProtoTradingStructs.TradeManager
{
    [ProtoContract]
    public class CListClientInfo : CBaseTrdMgr_StockExchId
    {
        [ProtoMember (1)]
        public List<CClientInfo> Lst { get; set; }

        public CListClientInfo ()
        {
            Lst = new List<CClientInfo>();
        }

        public CListClientInfo(int stockExchId) : this()
        {
            StockExchId = stockExchId;        
        }

        public void Add(CClientInfo ci)
        {
            lock(this)
            {
                Lst.Add(ci);
            }

        }

        public void Delete(int conId)
        {
            lock(this)
            {
                Lst.RemoveAll(el => el.ConId == conId);
            }
        }



        public CListClientInfo GetCopy()
        {
            lock (this)
            {                             
                return new CListClientInfo(this.StockExchId) { Lst = new List<CClientInfo>(this.Lst) };
            }
        }




    }
}
