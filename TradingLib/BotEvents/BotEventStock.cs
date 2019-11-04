using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib;
using TradingLib.Enums;
using TradingLib.ProtoTradingStructs;

namespace TradingLib.BotEvents
{
    public class BotEventStock
    {

        public BotEventStock (Dictionary<Direction, List<CStock>> stocks )
        {

            try
            {

             /*  Stocks[Direction.Up] = new List<CStock>();
                Stocks[Direction.Down] = new List<CStock>();

                Stocks[Direction.Up].AddRange(stocks[Direction.Up]);
                Stocks[Direction.Down].AddRange(stocks[Direction.Down]);
               */ 

               

            } 

            catch (Exception e)
            {
                string st = e.Message;
            }

        }


        public Dictionary<Direction, List<CStock>> Stocks = new Dictionary<Direction, List<CStock>>() { { Direction.Up, new List<CStock>() }, { Direction.Down, new List<CStock>() } };

    }
}
