using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Components;

using TradingLib.Enums;
using TradingLib.Bots;
using TradingLib.BotEvents;

namespace TradingLib.Bots
{
    public class CBotTesterCrossFirst : CBotSingleInstrument
    {


        public CBotTesterCrossFirst(int botId, string name, CSettingsBot settingsBot,
                           Dictionary<string, string> dictSettingsStrategy, /*CPlaza2Connector*/IDealingServer plaza2Connector) :
            base(botId, name, settingsBot, dictSettingsStrategy, plaza2Connector)
        {
            SetState(EnmStratStates._000_PreInitial);

        }


        protected override void RecalcBotLogics(CBotEventStruct botEvent)
        {

            if (IsState(EnmStratStates._000_PreInitial))
            {
               
                CloseAllPositions();
                CancellAllBotOrders();
              //  SetState(EnmStratStates._001_Initial);
                DisableBot();
            }
            if (IsState(EnmStratStates._001_Initial))
            {
             AddOrderNearSpread(m_isin, /*OrderDirection.Sell*/EnmOrderDir.Sell, m_parLot, 0);
               SetState(EnmStratStates._002_AfterLomitOrderAdded);
             
            }
           



        }


        enum EnmStratStates
        {
            _000_PreInitial,
            _001_Initial,
            _002_AfterLomitOrderAdded

        }



    }
    
}
