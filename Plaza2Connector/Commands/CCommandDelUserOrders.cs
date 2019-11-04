using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plaza2Connector
{
    class CCommandDelUserOrders
    {
        public CCommandDelUserOrders(int buy_sell, int ext_id, string isin)
        {
            Buy_sell = buy_sell;
            Ext_id = ext_id;
            Isin = isin;


        }

        public int Buy_sell;
        public int Ext_id;
        public string Isin;



    }
}
