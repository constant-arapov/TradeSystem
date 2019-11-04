using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Enums;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Components;
using TradingLib.BotEvents;

namespace TradingLib.Bots
{
    public class CBotTesterCrossSecond : CBotSingleInstrument
    {


        public CBotTesterCrossSecond(int botId, string name, CSettingsBot settingsBot,
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
                //SetState(EnmStratStates._001_Initial);
                DisableBot();
            }



            if (IsState(EnmStratStates._001_Initial))
            {
                if (IsPairedBotOrderAdded())
                {
                    AddMarketOrder(m_isin, /*OrderDirection.Buy*/EnmOrderDir.Buy, m_parLot);
                    SetState(EnmStratStates._002_AfterSentMarketOrder);
                }
            }
        }
        private bool IsPairedBotOrderAdded()
        {
            CBotBase bot =  _dealingServer.GetBotById(6);



            if (bot.MonitorOrdersAll != null)
            {
                lock (bot.MonitorOrdersAll)
                {
                    if (bot.MonitorOrdersAll.ContainsKey(m_isin) && bot.MonitorOrdersAll[m_isin].Count > 0 &&
                        bot.MonitorOrdersAll[m_isin].First().Value.Isin == m_isin &&
                        bot.MonitorOrdersAll[m_isin].First().Value.Amount == m_parLot)
                        return true;
                }
            }
              


            return false;
        }


        enum EnmStratStates
        {
            _000_PreInitial,
            _001_Initial,
            _002_AfterSentMarketOrder
           
        }



    }

}
