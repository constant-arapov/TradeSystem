using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using  TradeManager.Models;
using TradeManager.DataSource;

namespace TradeManager 
{
    public class CTradeManagerConfig : IXMLSerializable, IIsValidable
    {
        public string IP_DB { get; set; }

        //not using now - will be in the future
        public  long Port_DB { get; set; }


        public string FileName { get; set; }

        public bool NeedSelfInit { get; set; }

        public bool IsValid { get; set; }

        public List<int> LstStockExhId { get; set; }

        public List<ModelStockExchState> LstModelStockExchState { get; set; }
		public List<ModelDBCon> LstModelDBCon { get; set; }

        public CTradeManagerConfig()
        {
          


        }




        public void SelfInit()
        {
            IP_DB = "127.0.0.1";
            Port_DB = 3306;
            IsValid = true;
        }


    }
}
