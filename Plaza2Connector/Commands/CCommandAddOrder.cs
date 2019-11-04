using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Enums;

namespace Plaza2Connector
{
    class CCommandAddOrder
    {

        public CCommandAddOrder(uint userId, string broker_code,  string isin, string client_code, OrderTypes type, /*OrderDirection*/EnmOrderDir dir, int amount, string price, int ext_id)
        {
            UserId = userId;
            Broker_code = broker_code;
            Isin = isin;             
            Client_code = client_code;
            Type = type;
            Dir =  dir;
            Amount = amount;
            Price = price;
            Ext_id = ext_id;
           


        }
        public uint UserId;
        public string Broker_code;
        public string Isin;
        public string Client_code;
        public OrderTypes Type;
        public /*OrderDirection*/EnmOrderDir Dir;
        public int Amount;
        public string Price;
        public int Ext_id;


    }
}
