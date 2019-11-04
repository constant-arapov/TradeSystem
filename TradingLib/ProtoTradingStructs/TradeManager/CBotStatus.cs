using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using ProtoBuf;


using TradingLib.Interfaces.Keys;

namespace TradingLib.ProtoTradingStructs.TradeManager
{
    [ProtoContract]
    public class CBotStatus : CBaseTrdMgr_StockExch_BotId, IKey_TraderName
	{
    			

        [ProtoMember(1)]
        public bool IsDisabled { get; set; }


        [ProtoMember(2)]
        public decimal VMAllInstrOpenedAndClosed { get; set; }


        public string TraderName { get; set; }


        public decimal Limit { get; set; }


        public CBotStatus()
        {
            IsDisabled = false;
            VMAllInstrOpenedAndClosed = 0;
            TraderName = "";
        }
        /*
        public CBotStatus GetCopy()
        {
            lock (this)
            {
                return (CBotStatus)this.MemberwiseClone();
            }
        }
        */
    }
}
