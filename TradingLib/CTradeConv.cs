using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Enums;

namespace TradingLib
{
    public class CTradeConv
    {
        public static char OrderDirToChar(EnmOrderDir dir)
        {
            return dir == EnmOrderDir.Buy ? 'B' : 'S';



        }


    }
}
