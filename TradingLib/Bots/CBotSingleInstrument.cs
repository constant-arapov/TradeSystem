using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using TradingLib;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Components;
using TradingLib.Enums;
using TradingLib.ProtoTradingStructs;
using TradingLib.Bots;
using TradingLib.BotEvents;

namespace TradingLib.Bots
{
     [Serializable]
    public class CBotSingleInstrument : CBotBase
    {

        protected long m_parStopLoss = 0;
        protected long m_parTakeProfit = 0;
        protected int m_parLot = 0;
        protected string m_isin;

        protected CBotMarketData MonitorMarketData;
        protected CBotPos MonitorPositions;
        protected Dictionary<long, COrder> MonitorOrders = new Dictionary<long,COrder>();

       
        public CBotSingleInstrument()
            :base ()
        {



        }

        public CBotSingleInstrument(int botId, string name, CSettingsBot settingsBot, Dictionary<string, string> dictSettingsStrategy,
                                                                                          /*CPlaza2Connector*/IDealingServer plaza2Connector) :
            base(botId, name, settingsBot, dictSettingsStrategy, plaza2Connector)
        {

           // m_isin = settingsBot.ListIsins[0];
           // m_parLot = settingsBot.TradingSettings[m_isin].Lot;

        }

        public void AddMarketOrder(EnmOrderDir dir)
        {

            AddMarketOrder(m_isin, dir, m_parLot);

        }

        public void AddMarketOrder(EnmOrderDir dir, int lot)
        {

            AddMarketOrder(m_isin, dir, lot);
           
        }
        
        public void AddOrder(EnmOrderDir dir, decimal price, int lot)
        {
            AddOrder(m_isin, price, dir, lot);

        }

        public void AddOrder(EnmOrderDir dir, decimal price)
        {
            AddOrder(m_isin, price, dir, m_parLot);

        }


        

        protected decimal StepPrice
        {
            get
            {
                if (_dealingServer != null && _dealingServer.DictInstruments != null
                    && m_isin != "" && _dealingServer.DictInstruments.ContainsKey(m_isin))
                {
                    return _dealingServer.DictInstruments[m_isin].Min_step;
                }
                return 0;
            }

        }

        protected decimal PositionSign
        {
            get
            {
                if (MonitorPositions != null)
                {
                    if (MonitorPositions.Amount > 0)
                        return 1;
                    else if (MonitorPositions.Amount < 0)
                        return -1;

                }
                return 0;
            }

        }

        protected decimal StoplossPrice
        {
            get
            {
                if (MonitorPositions != null && MonitorPositions.PriceOpen != 0
                      && PositionSign != 0 && StepPrice != 0)

                    return MonitorPositions.PriceOpen - PositionSign * StepPrice * m_parStopLoss;


                return 0;
            }
        }


        protected decimal TakeProfitPrice
        {

            get
            {

                if (MonitorPositions != null && MonitorPositions.PriceOpen != 0
                     && PositionSign != 0 && StepPrice != 0)

                    return MonitorPositions.PriceOpen + PositionSign * StepPrice *  m_parTakeProfit;





                return 0; 
            }




        }




        protected bool IsStopLossWasBroken()
        {
            if ((PositionSign > 0 && MonitorMarketData.Bid < StoplossPrice) ||
                 (PositionSign < 0 && MonitorMarketData.Ask > StoplossPrice))
            {
                Log("IsStopLossWasBroken.true. PositionSign=" + PositionSign + 
                    " Bid=" + MonitorMarketData.Bid + " Ask=" + MonitorMarketData.Ask + " StoplossPrice=" + StoplossPrice);
                
                return true;
            }

            return false;
        }

        protected bool IsTakeProfitWasBroken()
        {
            if ((PositionSign > 0 && MonitorMarketData.Bid > TakeProfitPrice) ||
                 (PositionSign < 0 && MonitorMarketData.Ask < TakeProfitPrice))
            {
                Log("IsTakeProfitWasBroken.true. PositionSign=" + PositionSign +
                    " Bid=" + MonitorMarketData.Bid + " Ask=" + MonitorMarketData.Ask + " TakeProfitPrice=" + TakeProfitPrice);

                return true;
            }

            return false;
        }




        

        protected override void RecalcBotStructs(CBotEventStruct botEvent)
        {
            try
            {

                base.RecalcBotStructs(botEvent);

                if (MonitorMarketDataAll.ContainsKey(m_isin))
                    MonitorMarketData = MonitorMarketDataAll[m_isin];
                if (MonitorPositionsAll.ContainsKey(m_isin))
                    MonitorPositions = MonitorPositionsAll[m_isin];
                lock (MonitorOrdersAll)
                {
                    if (MonitorOrdersAll.ContainsKey(m_isin))
                        MonitorOrders = MonitorOrdersAll[m_isin];
                }
                mxCurrentStocks[m_isin].WaitOne();

                if (m_currentStocks.ContainsKey(m_isin) &&
                    m_currentStocks[m_isin].ContainsKey(Direction.Down) &&
                    m_currentStocks[m_isin].ContainsKey(Direction.Up)
                    )
                {
                    if (m_currentStocks[m_isin][Direction.Down].Count > 0
                        && m_currentStocks[m_isin][Direction.Down][0].Price > 0)
                        MonitorMarketData.Bid = m_currentStocks[m_isin][Direction.Down][0].Price;

                    if (m_currentStocks[m_isin][Direction.Up].Count > 0
                        && m_currentStocks[m_isin][Direction.Up][0].Price > 0)
                        MonitorMarketData.Ask = m_currentStocks[m_isin][Direction.Up][0].Price;

                }
            }
            catch (Exception e)
            {
                Error("RecalcBotStructs bot=" + Name, e);

            }
            finally
            {
                mxCurrentStocks[m_isin].ReleaseMutex();
            }
        }


        protected  override void LoadParameters()
        {

            base.LoadParameters();
            m_isin = SettingsBot.ListIsins[0]; //note trading instrument is the first

            m_parStopLoss = SettingsBot.TradingSettings[m_isin].StopLoss;
            m_parTakeProfit = SettingsBot.TradingSettings[m_isin].TakeProfit;
            m_parLot = SettingsBot.TradingSettings[m_isin].Lot;




        }

    }
}
