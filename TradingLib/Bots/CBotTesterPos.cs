using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Components;
using TradingLib.Enums;
using TradingLib.BotEvents;


namespace TradingLib.Bots
{
    public class CBotTesterPos : CBotSingleInstrument
    {
        public CBotTesterPos(int botId, string name, CSettingsBot settingsBot,  
                        Dictionary <string, string> dictSettingsStrategy, /*CPlaza2Connector*/IDealingServer  plaza2Connector) :
            base(botId, name, settingsBot, dictSettingsStrategy, plaza2Connector)
        {

            SetState(EnmStratStates._000_PreInitial);

            

        }

        protected override void CreateTimers()
        {
            base.CreateTimers();

            AddTimer("DelayBeforeOpenFirstPos", 5000);
            AddTimer("HandleFirstPositionOpened",60000);



        }


        protected override void RecalcBotLogics(CBotEventStruct botEvent)
        {
            base.RecalcBotLogics(botEvent);


            if (IsState(EnmStratStates._000_PreInitial))
            {

                if (MonitorOrders.Count == 0 && ((MonitorPositions == null || MonitorPositions.Amount == 0)))
                {
                    SetState(EnmStratStates._010_Initial);
                }
                else
                {
                    CancellAllBotOrders();
                    CloseAllPositions();

                    SetState(EnmStratStates._001_WaitAllOldOrdersAndPosClosed);
                }

            }
            else if (IsState (EnmStratStates._001_WaitAllOldOrdersAndPosClosed))
            {
                
                if (IsNoOpenedPos() && IsNoOpenedOrders())
                    SetState(EnmStratStates._010_Initial);

                

            }
            else if (IsState (EnmStratStates._010_Initial))
            {
                StartTimer("DelayBeforeOpenFirstPos");
                SetState(EnmStratStates._011_DelayBeforeOpenFirstPos);

              

            }
            else if (IsState (EnmStratStates._011_DelayBeforeOpenFirstPos))
            {
                if (botEvent.EventCode == EnmBotEventCode.OnTimer 
                 && (string) botEvent.Data == "DelayBeforeOpenFirstPos")
                {

                AddMarketOrder(/*OrderDirection.Buy*/EnmOrderDir.Buy);
                SetState(EnmStratStates._012_WaitFirstPosOpened);

                }

            }
            else if (IsState (EnmStratStates._012_WaitFirstPosOpened))
            {
                if (MonitorPositions!=null && MonitorPositions.Amount == m_parLot)
                    SetState(EnmStratStates._013_FirstPositionOpened);


            }
            else if (IsState(EnmStratStates._013_FirstPositionOpened))
            {

                SetState(EnmStratStates._014_HandleFirstPositionOpened);
                StartTimer("HandleFirstPositionOpened");
                    

            }
            else if (IsState (EnmStratStates._014_HandleFirstPositionOpened))
            {
                if (botEvent.EventCode == EnmBotEventCode.OnTimer
                && (string)botEvent.Data == "HandleFirstPositionOpened")
                {



                    CloseAllPositions();
                    SetState(EnmStratStates._100_NormalExit);
                }
            }



        }




        enum EnmStratStates
        {
            _000_PreInitial,
            _001_WaitAllOldOrdersAndPosClosed,
            _010_Initial,
            _011_DelayBeforeOpenFirstPos,
            _012_WaitFirstPosOpened,
            _013_FirstPositionOpened,
            _014_HandleFirstPositionOpened,
            _100_NormalExit


        }

    }
}
