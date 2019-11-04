using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Enums;
using TradingLib.BotEvents;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Components;


namespace TradingLib.Bots
{
    public class CBotTesterLimits : CBotBase
    {
        public CBotTesterLimits()
            : base()
        {


        }

          public CBotTesterLimits (int botId, string name, CSettingsBot settingsBot, Dictionary<string, string> dictSettingsStrategy,
                                                                                          /*CPlaza2Connector*/IDealingServer plaza2Connector) :
            base(botId, name, settingsBot, dictSettingsStrategy, plaza2Connector)
        {

            SetState(EnmStratStates._010_Initial);

        }


          protected override void RecalcBotLogics(CBotEventStruct botEvent)
          {
              base.RecalcBotLogics(botEvent);
             

              if (IsState(EnmStratStates._010_Initial))
              {
              //    CancellAllBotOrders();
              //    CloseAllBotPositions();
                 // for (int i=0; i<2; i++)
                   // AddOrderNearSpread("RTS-12.15", OrderDirection.Buy, 1, 50);

                  AddMarketOrder("RTS-12.15", /*OrderDirection.Buy*/EnmOrderDir.Buy, 2);
           
               //   ForceAddMarketOrder("RTS-12.15", OrderDirection.Buy, 1, 10, 4);

                 // AddMarketOrder("RTS-12.15", OrderDirection.Buy, 1);
                  //AddMarketOrder("GAZR-12.15", OrderDirection.Buy, 1);
                  //AddMarketOrder("Si-12.15", OrderDirection.Buy, 1);


                  SetState(EnmStratStates._011_AfterFirstOrderAdded);
              }
              else if (IsState(EnmStratStates._011_AfterFirstOrderAdded))
              {


              }

          }

          enum EnmStratStates
          {
              _000_PreInitial,
              _001_WaitAllOldOrdersAndPosClosed,
              _010_Initial,
              _011_AfterFirstOrderAdded
              


          }


    }

}
