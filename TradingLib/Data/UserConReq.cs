using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib;

using TradingLib.ProtoTradingStructs;



namespace TradingLib.Data
{
    public class UserConReq
    {
        public int ConnNum { get; set; }
        public CAuthRequest AuthRequest { get; set; }
        public string ServerName { get; set; }



    }
}
