using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Data;

namespace Plaza2Connector
{
    public class CStockLogStruct
    {
        public CRawStock RawStock { get; set; }
        public long Ticks { get; set; }
        public long LogTick { get; set; }
        public long GetDataTick { get; set; }


    }
}
