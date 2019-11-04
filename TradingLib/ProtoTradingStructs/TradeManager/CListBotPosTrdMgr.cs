using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;


namespace TradingLib.ProtoTradingStructs.TradeManager
{
	/// <summary>
	///  List with opened pos of
	/// </summary>	
    [ProtoContract]
    public class CListBotPosTrdMgr : CBaseTrdMgr_StockExchId
    {


        //private int _stockExchId;

        [ProtoMember(1)]
        public List<CBotPosTrdMgr> Lst;

        //for PROTOBUF capability
        public CListBotPosTrdMgr()
        {
            Lst = new List<CBotPosTrdMgr>();
        }



        public CListBotPosTrdMgr(int stockExchid) : this()
        {
            //_stockExchId = stockExchid;
            StockExchId = stockExchid;
            
        }


        public void Update(int botId, CBotPos botPos)
        {

            lock (this)
            {
                bool bFound = false;
                for (int i = 0; i < Lst.Count; i++)
                {
                    if (Lst[i].BotId == botId
                        && Lst[i].Instrument == botPos.Instrument )
                    {
                        if (botPos.Amount == 0)
                        {
                            
                            Lst.RemoveAt(i);
                        }
                        else
                        {
                            //update
                            Lst[i].Amount = botPos.Amount;
                        }

                        bFound = true;
                    }                
                }

                if (!bFound)
                {
                    if (botPos.Amount != 0)
                        Lst.Add(new CBotPosTrdMgr
                        {
                            StockExchId = this.StockExchId,
                            BotId = botId,
                            Instrument = botPos.Instrument,
                            Amount = botPos.Amount
                        });



                }

                

            }
        }


        public CListBotPosTrdMgr GetCopy()
        {
            lock (this)
            {
                return new CListBotPosTrdMgr(this.StockExchId) { Lst = new List<CBotPosTrdMgr>(this.Lst) };
            }
        }





    }
}
