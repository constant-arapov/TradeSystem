using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;




namespace TradingLib.ProtoTradingStructs.TradeManager
{
    [ProtoContract]
    public class CListBotStatus : CBaseTrdMgr_StockExchId
    {
        [ProtoMember(1)]
        public List<CBotStatus> Lst { get; set; }


        public CListBotStatus()
        {
            Lst = new List<CBotStatus>();        
        }

        public CListBotStatus(int stockExchId) : this()
        {
            StockExchId = stockExchId;
        }



        public void Update (CBotStatus botStatus)
        {

            lock (this)
            {
                bool bFound = false;
                for (int i = 0; i < Lst.Count; i++)
                {
                    if (botStatus.BotId == Lst[i].BotId)
                    {
                        Lst[i] = botStatus;
                        bFound = true;
                    }

                }

                if (!bFound)
                    Lst.Add(botStatus);


            }

        }


        public CListBotStatus GetCopy()
        {
            lock (this)
            {
                return new CListBotStatus(this.StockExchId) { Lst = new List<CBotStatus>(this.Lst) };
            }
        }



    }
}
