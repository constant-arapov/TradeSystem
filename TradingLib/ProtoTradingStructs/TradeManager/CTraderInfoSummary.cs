using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace TradingLib.ProtoTradingStructs.TradeManager
{
	[ProtoContract]
	public class CTraderInfoSummary : CBaseTrdMgr_StockExch_BotId
	{
		[ProtoMember(1)]
		public decimal VMAllInstrOpenedAndClosed { get; set; }


        //TODO: POS, LIMIT

		public CTraderInfoSummary()
		{

			VMAllInstrOpenedAndClosed = 0;

		}

        public CTraderInfoSummary GetCopy()
        {
            lock (this)
            {
                return (CTraderInfoSummary)this.MemberwiseClone();
            }
        }





	}
}
