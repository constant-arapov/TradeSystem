using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;


namespace TradingLib.ProtoTradingStructs
{
    [ProtoContract]
    public class CUserMoney
    {
        [ProtoMember(1)]
        public CAccountMoney AccountMoney { get; set; }


        [ProtoMember(2)]
        public List<CAccountTrade> ListAccountTrade { get; set; }

        public CUserMoney()
        {
            AccountMoney = new CAccountMoney();
            ListAccountTrade = new List<CAccountTrade>();

        }


    }
}
