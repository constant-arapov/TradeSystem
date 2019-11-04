using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Interaction;
using TradingLib.Enums;

using TradingLib.TradersDispatcher;
using TradingLib.BotEvents;
using TradingLib.Data;



namespace TradingLib.Bots
{
    public class CBotTrader : CBotBase, IBotTraderOperations, IClientSubBot
    {


        CTradersDispatcher _tradersDispatcher;

        private Dictionary<string, CBotState<EnmTraderState>> _dictStates = new Dictionary<string, CBotState<EnmTraderState>>();
        //private Dictionary<string, CTraderInstrumentData> _traderInstrumentData = new Dictionary<string, CTraderInstrumentData>();
        private Dictionary<string, CSubBot> _subBots = new Dictionary<string, CSubBot>();

        private Dictionary<string, bool> _dictSubscribedInstruments = new Dictionary<string, bool>();




        public CBotTrader(int botId, string name, CSettingsBot settingsBot, Dictionary<string, string> settingsStrategy,
                            /*CPlaza2Connector*/IDealingServer plaza2Connector) :
            base(botId, name, settingsBot, settingsStrategy, plaza2Connector)
        {


            _tradersDispatcher = plaza2Connector.TradersDispatcher;

            InitData();
            

        }


        private void InitData()
        {
            foreach (var kvp in SettingsBot.TradingSettings)
            {
                _dictStates[kvp.Key] = new CBotState<EnmTraderState>(EnmTraderState._0100_Initial, m_logger);
                //_traderInstrumentData[kvp.Key] = new CTraderInstrumentData();
                _subBots[kvp.Key] = new CSubBot(BotId, kvp.Key,this,this);

                _dictSubscribedInstruments[kvp.Key] = false;

            }

        }

        public void OnSubscribeInstrument(string instrument)
        {
            lock (_dictSubscribedInstruments)
            {
                _dictSubscribedInstruments[instrument] = true;
            }

        }

        public  void OnUnSubscribeInstrument(string instrument)
        {
            lock (_dictSubscribedInstruments)
            {
                _dictSubscribedInstruments[instrument] = false;
            }



        }


        public  bool IsNeedUpdateStock(string instrument)
        {
            lock (_dictSubscribedInstruments)
            {
                return _dictSubscribedInstruments[instrument];
                   
            }

            return false;
            //TODO other cases: open positions etc...
        }


        /// <summary>
        /// Call from SubBot.CheckStopTakeInvertConditions
        /// </summary>
        /// <param name="instrument"></param>
        public void CloseByTakeProfit(string instrument)
        {

            ClosePositionOfInstrument(instrument);
        }

        /// <summary>
        /// Call from SubBot
        /// </summary>
        /// <param name="instrument"></param>
        public void CloseByStopLoss(string instrument)
        {
            ClosePositionOfInstrument(instrument);

        }

        /*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        
         *                                              INHERITED METHODS
         +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/


        /// <summary>
        /// Extends CBotBase Method.
        /// - Sends data to traders
        /// - Updates subbot's datra
        /// </summary>
		protected override void ProcessRawOrdLogStruct(CRawOrdersLogStruct rols)
		{

			base.ProcessRawOrdLogStruct(rols);

			BindTradersDispatcher();


			if (_tradersDispatcher != null)
			//if (m_plaza2Connector.IsOnlineUserOrderLog)
			{
				//TODO enqueue
				_tradersDispatcher.EnqueueUserOrdersUpdate(BotId, this);


			}

			UpdateSubBotOrdersMonitorInstr(rols);
		}

        
        /// <summary>
        /// Call from ProcessRawOrdLogStruct
        /// </summary>
        /// <param name="rols"></param>
		private void UpdateSubBotOrdersMonitorInstr(CRawOrdersLogStruct rols)
		{
			if (rols.Ext_id != BotId)
				return;
 
		
			string instrument = "unknown";
            
            //update 2017-08-10 for ASTS
            if (rols.Instrument != null && rols.Instrument != "") //for ASTS
            {
                instrument = rols.Instrument;
            }
            else //for Plaza 2
            {

                if (_dealingServer.Instruments.IsContainsIsinId(rols.Isin_Id))
                    instrument = _dealingServer.Instruments.DictIsinId_Instrument[rols.Isin_Id];

                else
                    Log("Error ! Unknown isin");

            }

			if (!_subBots.ContainsKey(instrument))
				return;

            //2017-07-18 check _monitorOrdersAllTotal
            if (_monitorOrdersAllTotal.ContainsKey(instrument))
			    _subBots[instrument].UpdateOrdersMonitorInstr(_monitorOrdersAllTotal[instrument]);
			//_monitorOrdersAllTotal[instrument]
			
		}

        /// <summary>
        /// Call from OnUserDealData
        /// </summary>
        /// <param name="isin"></param>
        /// <param name="bp"></param>
        protected override void UpdateMonitorPosisionsAll(string isin, CBotPos bp)
        {
            base.UpdateMonitorPosisionsAll(isin, bp);

            BindTradersDispatcher();

            if (_tradersDispatcher == null)
                _tradersDispatcher = _dealingServer.TradersDispatcher;
            if (_tradersDispatcher != null)
            {
                _tradersDispatcher.EnqueueUpdatUserPositionsMonitor(this, BotId);
                _tradersDispatcher.EnqueueUpdateVm(BotId);

            }

            lock (MonitorPositionsAll)
            {
                CBotPos pos = new CBotPos();
                if (MonitorPositionsAll.TryGetValue(isin, out pos))
                    _subBots[isin].OnUpdatePosition(pos);

            }

        }

        protected override void OnStockUpdateData(string instrument)
        {
			//2017-01-15 experimental
           // if (!IsNeedUpdateStock(instrument))
               // return;


            base.OnStockUpdateData(instrument);
          

            _subBots[instrument].Bid = _bid[instrument];
            _subBots[instrument].Ask = _ask[instrument];

			//tempo

			
	



        }


        protected override void OnDealsOnlineData()
        {
            base.OnUserDealOnlineData();


            if (_tradersDispatcher != null)
                _tradersDispatcher.EnqueueUpdateDealsLog(BotId);

        }

        protected override void OnUserDealData(string instrument, CBotEventStruct botEvent)
        {
            base.OnUserDealData(instrument, botEvent);

            if (_tradersDispatcher != null)
                _tradersDispatcher.EnqueueUpdateDealsLog(BotId);

        }


        protected override void RecalcBotStructs(CBotEventStruct botEvent)
        {
            base.RecalcBotStructs(botEvent);



            EnmBotEventCode evCode = botEvent.EventCode;
            if (EnmBotEventCode.OnTraderConnected == evCode)
            {
                OnTraderConnected();
                if (_tradersDispatcher != null)
                    _tradersDispatcher.EnqueueUpdateDealsLog(BotId);
            }
            else if (EnmBotEventCode.OnTimeDisableTradeLoaded == evCode)
            {
                CDisableTradeData dtl = (CDisableTradeData)botEvent.Data;
                string instrument = botEvent.Isin;
                if (_subBots.ContainsKey(instrument))
                {
                    _subBots[instrument].UpdateBotsDisableTrading(dtl);

                }
            }
            //2018-04-25
            else if (EnmBotEventCode.OnForceUpdTotalVM == evCode)
            {
                //note recalc of VM was already done in CBotBase
                if (_tradersDispatcher != null)
                    _tradersDispatcher.EnqueueUpdateVm(BotId);

            }
          


        }




        protected override void OnStockUpdateLogics(string instrument)
        {
            base.OnStockUpdateLogics(instrument);


            if (_subBots.ContainsKey(instrument))
                _subBots[instrument].OnStockUpdateLogics();
        }

        /*protected override void OnUserDealsLogics(string instrument)
        {
            base.OnUserDealsLogics(instrument);

            if (_subBots.ContainsKey(instrument))
                _subBots[instrument].OnUserDealsLogics();

        }
        */
        


        protected override void RecalcBotLogics(CBotEventStruct botEvent)
        {
            base.RecalcBotLogics(botEvent);

            //CheckNeedSendStopLossTakeProfit();

            EnmBotEventCode evCode = botEvent.EventCode;
            string instrument = botEvent.Isin;

			if (EnmBotEventCode.OnTraderDisconnected == evCode)
			{

				CancellAllBotOrders();

			}


            /*  if (EnmBotEventCode.OnStockUpdate == evCode)
              {
                  if (_subBots.ContainsKey(instrument))
                      _subBots[instrument].OnStockUpdate();

              }
              */



        }


       





        /// <summary>
        /// Specific  AddOrder Method - specialy when order added by Trader 
        /// (from TradersDispatcher). The main change comparing with 
        /// CBotBase.AddOrder method  is: if condition false 
        /// do not disable bot just do not pass the order to dealing server.
        /// </summary>
        /// <param name="instrument"></param>
        /// <param name="price"></param>
        /// <param name="dir"></param>
        /// <param name="amount"></param>
        /// <param name="botId"></param>
        /// <returns></returns>
        protected override bool AddOrder(string instrument, decimal price,  EnmOrderDir dir, decimal amount, int botId)
        {
            //TODO make logic together with CBotBase in common method 

            if (IsPossibleToAddOrder(instrument, price))
            {
                if (IsEnoughMoneyToAddOrder(price, amount, dir))
                {
                    _dealingServer.AddOrder(botId, instrument, price, dir, Math.Abs(amount));
                    LogAddOrder("CBotTrader.AddOrder", instrument, price, dir, amount);
                    m_currOrderSentCountTotal++;
                    if (!m_dictSentOrderCount.ContainsKey(instrument))
                        m_dictSentOrderCount[instrument] = 0;

                    m_dictSentOrderCount[instrument]++;
                    m_timers["WaitAddOrderReply"].Set();
                    return true;
                }
            }


            Error("CBotTrader not possible to AddOrder");


            return false;

        }


        /// <summary>
        /// Called from CTradersDispatcher when order command recieved
        ///  
        /// Contains extra check methods - specific for trader.
        /// </summary>
        /// <param name="isin"></param>
        /// <param name="price"></param>
        /// <param name="dir"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public   bool AddOrderByTrader(string isin, decimal price,  EnmOrderDir dir, decimal amount)
        {

            Log("Try add order by trader");
            if (IsAllowTraderActions(isin) && //denied for instrument (time disabled or short pos vialated disabled)
                !IsShortDisabled(isin, dir,amount) && //short is disable at the moment
                !DisabledBot && 
                //2017-11-28
                IsOrderDataCorrect(isin,price,dir,amount) //check if not null etc
                )
                {          
                    //2017-11-29 changed. When trader adding
                    // order. Use Method of CBotTrader class which is not disable order.
                    //return base.AddOrder(isin, price, dir, amount, BotId);
                    return AddOrder(isin, price, dir, amount, BotId);
                }
                else
                {
                    Log("Add order by trader is not allowed");
                    return false;
                }

            //return false;
        }


        private bool IsOrderDataCorrect(string instrument, decimal price, EnmOrderDir dir, decimal amount)
        {
            if (price <= 0)
            {
                Error("Incorrect price in trader order. BotId="+BotId);
                return false;
            }

            if(dir != EnmOrderDir.Buy && dir != EnmOrderDir.Sell)
            {
                Error("Incrorrect order dir from  trader.");
                return false;
            }

            if (amount <= 0)
            {
                Error("Incorrect amount in trader order. BotId=" + BotId);
                return false;
            }

            if (instrument == "")
            {
                Error("Null instrument in trader order. BotId=" + BotId);
                return false;
            }


            return true;
        }



        private bool IsShortDisabled(string instrument, EnmOrderDir dir, decimal amountOrdersToAdd)
        {

            if (!_subBots.ContainsKey(instrument))
                return false;
            else
                return _subBots[instrument].IsShortDisabled(dir, amountOrdersToAdd);
            


        }
	
     
        /*
		protected override void UpdateMonitorOrdersAll(string isin, CRawOrdersLogStruct rols)
		{
			base.UpdateMonitorOrdersAll(isin, rols);
		
		}
        */
	




		protected override void OnUserOrderUpdateLogics(string instrument)
		{

			base.OnUserOrderUpdateLogics(instrument);


			if (_subBots.ContainsKey(instrument))
				_subBots[instrument].OnUserOrderUpdateLogics();



		}


        //2018-06-13
        protected override bool NeedProcessStockUpdate(string instrument)
        {
            if (base.NeedProcessStockUpdate (instrument))
            {
                return true;
            }
            else
            {
                if (_subBots.ContainsKey(instrument))
                {
                    if (_subBots[instrument].StopLossPrice != 0)
                        return true;

                    if (_subBots[instrument].TakeProfitPrice != 0)
                        return true;

                    if (_subBots[instrument].StopLossInvertPrice != 0)
                        return true;

                    if (_subBots[instrument].BuyStopPrice != 0)
                        return true;


                }
                             
            }

            return false;
        }





        /*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        
               *                                          END INHERITED METHODS
       +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/


       


        /// <summary>
        /// Allows or denies instrument oriented actions.
        /// </summary>
        /// <param name="instrument"></param>
        /// <returns></returns>
        private bool IsAllowTraderActions(string instrument)
        {
            if (_subBots.ContainsKey(instrument))
            {
                if ( _subBots[instrument].IsAllowAddOrder())
                {
                    return true;
                }
                else 
                {
                    Log("Trader actions for instrument " + instrument + " not allowed");
                    return false;
                }


            }
               

            return true;
        }


     


		public void ClosePositionOfInstrumentByTrader(string instrument)
		{

            Log("Try close position by trader");
            if (IsAllowTraderActions(instrument))
            {
                if (_subBots.ContainsKey(instrument))
                {
                    //2018-10-30 if position is small need to increase position 
                    //to minimum allowed. Do start triggering here 
                    if (_subBots[instrument].IsPositionSmall())
                    {
                        Log("Position is too small !!!");
                        _subBots[instrument].IncreasePosToMinPossible();
                        return;
                    }
                    //end
                    _subBots[instrument].OnClosePositionsByMarketByTrader();
                  
                }

                ClosePositionOfInstrument(instrument);
            }
            else
            {
                Log("Cloes position is not allowed");
            }

		}

        /// <summary>
        /// Call from:
        ///     CTradersDispatcher.ProcessCancellOrdersByIsin
        /// </summary>
        /// <param name="instrument"></param>
        public void CancellOrdersWithInstrumenByTrader(string instrument)
        {

            Log("Try cancell order with instrument");

            if (IsAllowTraderActions(instrument))
            {
                base.CancellAllOrdersByInstrument(instrument);

            }
            else
            {
                Log("Cancell order with instrument not allowed ");
            }

        }



        private void OnTraderConnected()
        {
            BindTradersDispatcher();
            _tradersDispatcher.EnqueueSendAvailableTickers(BotId);
            _tradersDispatcher.EnqueueUpdatUserPositionsMonitor(this, BotId);
            _tradersDispatcher.EnqueueUserOrdersUpdate(BotId, this);
            _tradersDispatcher.EnqueueUpdateUserPosLog(BotId);
            _tradersDispatcher.EnqueueUpdateDealsLog(BotId);
            _tradersDispatcher.EnqueueUpdateVm(BotId);
            _tradersDispatcher.EnqueueUpdateMoneyData(BotId);

           /* foreach (var kvp in  _traderInstrumentData)
            {
                string instrument =  kvp.Key;
                CTraderInstrumentData data = kvp.Value;
                _tradersDispatcher.EnqueueAcceptStopLossTakeProfit(BotId, 
                                    instrument,EnmStopLossTakeProfit.TakePofit, data.TakeProfit);

            }
             */
         /*   foreach (var kvp in _subBots)
            {
                string instrument = kvp.Key;
                CSubBot subBot = kvp.Value;
                _tradersDispatcher.EnqueueAcceptStopLossTakeProfit(BotId, instrument, 
                                                                  EnmStopLossTakeProfit.TakePofit, subBot.TakeProfit);


            }
           */ 
      


        }

	

        private void BindTradersDispatcher()
        {

            if (_tradersDispatcher == null)
                _tradersDispatcher = _dealingServer.TradersDispatcher;



        }

        public void SendStopLossInvert(string instrument, CBotPos pos)
        {

			//2017-03-13 was checked recently
			//if (pos.Amount == 0)			
				//return;
			


            /*OrderDirection*/EnmOrderDir dir;

            if (pos.Amount > 0) //buy need sell
                dir = EnmOrderDir.Sell;//OrderDirection.Sell;
            else //sell need buy
                dir = EnmOrderDir.Buy;//OrderDirection.Buy;

           decimal amount = Math.Abs(pos.Amount *2);

           AddMarketOrder(instrument, dir, amount);


        }


		public void  SendOrderThrowByTrader(string instrument, EnmOrderDir dir, decimal amount, int throwSteps)
		{

            if (IsAllowTraderActions(instrument))
            {
                decimal offsetPrice = GetPriceWithOffset(instrument, dir, throwSteps);


                base.SendOrderThrow(instrument, dir, amount, offsetPrice);
                _subBots[instrument].OnSendOrderThrow(offsetPrice, amount, dir);

            }

		}


        public void InvertPositionByTrader(string instrument)
        {
            if (IsAllowTraderActions(instrument))
            {
                base.InvertPosition(instrument);
            }

        }


        /// <summary>
        /// Set TakePofit, StopLoss, StopLossInvert, BuyStop or SellStop
        /// 
        /// 
        /// 
        /// Call from  TradersDispatcher.ProcessTypeOrder
        /// </summary>
        /// <param name="instrument"></param>
        /// <param name="orderType"></param>
        /// <param name="price"></param>
        /// <param name="amount"></param>
        public void SetTypeOrderByTrader(string instrument,EnmOrderTypes orderType, decimal price, decimal amount=0)
        {

              if (!_subBots.ContainsKey(instrument))
                  return;


            if (!IsAllowTraderActions(instrument))
                 return;



            //TODO check bid ask here also
            //echoing respons for a while
            if (EnmOrderTypes.TakePofit == orderType)
            {
                //_traderInstrumentData[instrument].TakeProfit = price;

                _subBots[instrument].TakeProfitPrice = price;
				_subBots[instrument].Log("TakeProfitPrice="+price);
                _tradersDispatcher.EnqueueAcceptStopLossTakeProfit(BotId, instrument, orderType, price);

            }
            if ((EnmOrderTypes.StopLoss == orderType))
            {
                _subBots[instrument].StopLossPrice = price;
				_subBots[instrument].Log("StopLossPrice=" + price);
                _tradersDispatcher.EnqueueAcceptStopLossTakeProfit(BotId, instrument, orderType, price);

            }
            if ((EnmOrderTypes.StopLossInvert == orderType))
            {
                _subBots[instrument].StopLossInvertPrice = price;
				_subBots[instrument].Log("StopLossInvertPrice=" + price);
                _tradersDispatcher.EnqueueAcceptStopLossTakeProfit(BotId, instrument, orderType, price);

            }
            if (EnmOrderTypes.BuyStop == orderType)
            {
                _subBots[instrument].BuyStopPrice = price;
                _subBots[instrument].BuyStopAmount = amount;
				_subBots[instrument].Log("BuyStopPrice=" + price);
                _tradersDispatcher.EnqueueAcceptStopLossTakeProfit(BotId, instrument, orderType, price, amount);
            }
            if (EnmOrderTypes.SellStop == orderType)
            {
                _subBots[instrument].SellStopPrice = price;
                _subBots[instrument].SellStopAmount = amount;
				_subBots[instrument].Log("SellStopPrice=" + price);
                _tradersDispatcher.EnqueueAcceptStopLossTakeProfit(BotId, instrument, orderType, price, amount);

            }
        }







        public void TriggerStopOrder(string instrument,EnmOrderTypes ordType, decimal amount)
        {


            if (EnmOrderTypes.BuyStop == ordType)
            {
                AddMarketOrder(instrument, /*Plaza2Connector.OrderDirection.Buy*/EnmOrderDir.Buy, amount);
            }

            if (EnmOrderTypes.SellStop == ordType)
            {
                AddMarketOrder(instrument, /*Plaza2Connector.OrderDirection.Sell*/EnmOrderDir.Sell, amount);
            }

        }



        public void CallbackUpdateStopLossTakeProfit(string instrument)
        {
            if (_tradersDispatcher == null)
                return;

           _tradersDispatcher.EnqueueAcceptStopLossTakeProfit(BotId, instrument, EnmOrderTypes.StopLoss, _subBots[instrument].StopLossPrice);
          
           _tradersDispatcher.EnqueueAcceptStopLossTakeProfit(BotId, instrument, EnmOrderTypes.TakePofit, _subBots[instrument].TakeProfitPrice);

           _tradersDispatcher.EnqueueAcceptStopLossTakeProfit(BotId, instrument, EnmOrderTypes.StopLossInvert, _subBots[instrument].StopLossInvertPrice);

          

        }

        public void CallbackUpdateStopOrders(string instrument)
        {
            if (_tradersDispatcher == null)
                return;



            _tradersDispatcher.EnqueueAcceptStopLossTakeProfit(BotId, instrument, EnmOrderTypes.BuyStop, _subBots[instrument].BuyStopPrice);

            _tradersDispatcher.EnqueueAcceptStopLossTakeProfit(BotId, instrument, EnmOrderTypes.SellStop, _subBots[instrument].SellStopPrice);

        }

        public void AddRestOrder(string instrument, decimal price, EnmOrderDir dir)
        {

            _subBots[instrument].AddOrderRest(price, dir);




        }

    }
}
