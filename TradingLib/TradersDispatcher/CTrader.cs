using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.ProtoTradingStructs;



namespace TradingLib.TradersDispatcher
{
    class CTrader
    {
        public int ConnId { get; set; }
        public int BotId { get; set; }
        public List<string> SubscribedIsins { get; set; }

        public bool IsLoggedOn { get; set; }


        public CTrader()
        {

            SubscribedIsins = new List<string>();
            //SubscribedIsins.Add("RTS-6.16");
            //SubscribedIsins.Add("Si-6.16");

            IsLoggedOn = false;
        }


        public void SubscribeIsin(CSubscribeTicker subscrTick)
        {

            subscrTick.ListSubscribeCommands.ForEach(a =>
                                                       {
                                                           if (a.Action == EnmSubsrcibeActions.Subscribe)
                                                           {
                                                               if (!SubscribedIsins.Contains(a.Ticker))
                                                                   SubscribedIsins.Add(a.Ticker);
                                                           }
                                                           else if (a.Action == EnmSubsrcibeActions.UnSubscribe)
                                                           {
                                                               SubscribedIsins.RemoveAll(s => s == a.Ticker);
                                                           }

                                                       }
                                                     );
            //SubscribedIsins.Add(isin);


        }

      /*  public void UnSubscribeIsin(string isin)
        {

            SubscribedIsins.RemoveAll(a => a == isin);


        }
        */

    }
}
