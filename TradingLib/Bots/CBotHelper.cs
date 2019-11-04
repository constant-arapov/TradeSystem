using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Logger;

using TradingLib;
using TradingLib.Enums;
using TradingLib.ProtoTradingStructs;


namespace TradingLib.Bots
{
    public static class CBotHelper
    {
       public static void PrintBanner(CLogger log, string Text)
        {
            log.Log("                                                                                                                                                                                                                                  ");
            log.Log("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            log.Log("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            log.Log("+++++++++++++++++++++++++++++++++++++++++++++++++         "+ Text + "    +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            log.Log("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            log.Log("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            log.Log("                                                                                                                                                                                                                                  ");
            log.Log("                                                                                                                                                                                                                                  ");

        }

       public static void PrintOneStock(CLogger log, List<CStock> local_stock, string title)
       {

           string st = title;
           foreach (CStock stock in local_stock)
           {
               st += stock.Price.ToString("0") + " " + stock.Volume + " | ";


           }

           log.Log(st);


       }



       public static void PrintStocks(CLogger log, Dictionary<Direction, List<CStock>> stock)
       {
           PrintOneStock(log,stock[Direction.Up], "ASKS: ");
           log.Log("");
           PrintOneStock(log,stock[Direction.Down], "BIDS: ");

           string marker =
             "____________________________________________________________________________________________________________________________________________________________________________________________________________________________________";
           log.Log(marker);

       }




    }
}
