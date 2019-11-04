using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using TradingLib.Enums;

using TradingLib;


namespace Terminal.Events.Data
{
    public class CDataAddOrder
    {
        public string Instrument;
        public decimal Amount;
        public EnmOrderDir Dir;
        public decimal Price;


    }
}
