using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace TradingLib.ProtoTradingStructs.TradeManager
{
	[ProtoContract]      
	class CListTraderInfoSummary : CBaseTrdMgr_StockExchId
	{
		[ProtoMember(1)]
		public List<CTraderInfoSummary> Lst { get; set; }


		public CListTraderInfoSummary()
		{
			Lst = new List<CTraderInfoSummary>();
		}

        public CListTraderInfoSummary(int stockExchId)
            : this()
        {
            StockExchId = stockExchId;
        }



	}
}
