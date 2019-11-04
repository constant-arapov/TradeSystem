using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib
{
	[Serializable]
    public class CLocalAccount
    {

        public long ConnectionNum { get; set; }
		public byte[] LoginHash { get; set; }
        public byte[] PwdHash { get; set; }



    }
}
