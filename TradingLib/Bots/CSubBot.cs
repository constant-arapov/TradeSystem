using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Timers;


using Common;
using Common.Logger;


using TradingLib;
using TradingLib.Data;
using TradingLib.Enums;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Bots;
using TradingLib.Common;






namespace TradingLib.Bots
{
    public class CSubBot
    {

        public decimal StopLossPrice;
        public decimal StopLossInvertPrice;


        public decimal TakeProfitPrice;

        public decimal BuyStopPrice;
        public decimal SellStopPrice;

        public decimal BuyStopAmount;
        public decimal SellStopAmount;



        public bool IsStoplossActivated;
        public bool IsTakeProditActivated;


        public decimal Bid;
        public decimal Ask;

        private CLogger _loggerState;
        private CLogger _logger;


        private string _instrument;


        private CBotPos _positionPrev = new CBotPos();

        private CBotPos _positionCurr = new CBotPos();

        private CBotPos _posRembInvrt = new CBotPos();
		private CBotPos _posBeforeStopOrder = new CBotPos();


		private COrderThrowData _orderThrowData = new COrderThrowData();



        private CBotState<EnmTraderState> _state;

        private IBotTraderOperations _operations;

        /// <summary>
        /// Mointor of total amount of orders with price and direction
        /// </summary>
		private Dictionary<EnmOrderDir, Dictionary<decimal, decimal>> _userOrdersMon = new Dictionary<EnmOrderDir, Dictionary<decimal, decimal>>();

		private Timer _timerWaitThrowOrders;
        private Timer _timerWaitClosePosByMarket;
        

        private CDisableTradeData _disableTradeData;

		private IClientSubBot _client;

        /// <summary>
        /// Disable trading. Reason: time disable trading was expired
        /// </summary>
        private bool _modeDisableTraderTrading = false;
        
        /// <summary>
        /// Disable trading with short position.
        /// </summary>
        private bool _modeDisableTraderShortTrading = false;


        /// <summary>
        /// Time of short time trading expired. In check condition
        /// process.
        /// </summary>
        private bool _modeTraderCheckDisableShortConditions = false;

        private decimal _currMinOrderSize = 0;

        /// <summary>
        /// Disable trading. Reason: time disable trading short was expired 
        /// and on that time was short position opened
        /// </summary>
        private bool _modeDisableTraderByShortViol = false;

        private decimal _expAmntAftInc = 0;


        //===============================================================================================================================================================
        //==================================== PUBLIC METHODS ===========================================================================================================
        //===============================================================================================================================================================

        public CSubBot(int botId, string instrument, IBotTraderOperations operations, IClientSubBot client)
        {

            _instrument = instrument;
			_loggerState = new CLogger(botId.ToString("D4") + "_subBot__states_" + _instrument, flushMode: true, subDir: client.BotSubDir );
			_logger = new CLogger(botId.ToString("D4") + "_subBot_" + _instrument, flushMode: true, subDir: client.BotSubDir);

            _state = new CBotState<EnmTraderState>(EnmTraderState._0100_Initial,_loggerState);            
            _state.SetState(EnmTraderState._0110_Trading);
            _operations = operations;
			_timerWaitThrowOrders = new Timer(200);
			_timerWaitThrowOrders.Elapsed += new ElapsedEventHandler(TimerWaitThrowOrders_Elapsed);

            
            _timerWaitClosePosByMarket = new Timer(2000);
            _timerWaitClosePosByMarket.Elapsed += new ElapsedEventHandler(TimerWaitClosePosByMarket_Elapsed);


            _client = client;

           // _currMinOrderSize = _client.GetMinOrderSize(_instrument);
        }



        /// <summary>
        /// Allows or denies add order depend on disabling by instrument.
        /// </summary>
        /// <returns></returns>
        public bool IsAllowAddOrder()
        {
            if (_modeDisableTraderTrading ||
                _modeDisableTraderByShortViol)
                return false;

            return true;
        }




        public bool IsShortDisabled(EnmOrderDir dir, decimal amountOrdersToAdd)
        {

            if (!_modeDisableTraderShortTrading)
                return false;

            if (EnmOrderDir.Buy == dir)
                return false; //any buy orders are possible

            CBotPos bp = GetPositionCurr();
            decimal amountPos = bp.Amount;
            decimal amountExistingShortOrders = GetUserOrderAmountByDir(dir);

            if (amountPos >= amountExistingShortOrders + amountOrdersToAdd)
            {               
                return false;
            }


            Log("IsShortDisabled.");
            return true;
        }



        /// <summary>
        /// Update local copy of order's monitor
        /// 
        /// Call from BotTrader.UpdateSubBotOrdersMonitorInstr
        /// </summary>  
        public void UpdateOrdersMonitorInstr(Dictionary<EnmOrderDir, Dictionary<decimal, decimal>> userOrdersMonInstr)
        {

            lock (_userOrdersMon)
            {

                _userOrdersMon.Clear();
                foreach (var kvp in userOrdersMonInstr)
                {
                    var dir = kvp.Key;
                    if (!_userOrdersMon.ContainsKey(dir))
                        _userOrdersMon[dir] = new Dictionary<decimal, decimal>();

                    foreach (var kvp2 in kvp.Value)
                    {
                        var price = kvp2.Key;
                        _userOrdersMon[dir][price] = kvp2.Value;
                    }

                }
            }
        }



        public void OnUpdatePosition(CBotPos pos)
        {
            lock (_positionCurr)
            {
                _positionCurr.Amount = pos.Amount;
                _positionCurr.AvPos = pos.AvPos;

            }
            //changed 2017-03-08
            // CheckPositionClosedOrInvert();
            // CheckPositionJustOpened();

            CheckForStateChange();


            //prev value for detect closing/inversing position
            _positionPrev.Amount = pos.Amount;
            _positionPrev.AvPos = pos.Amount;


            // CheckOrderThrowApplied();


        }


        public void OnStockUpdateLogics()
        {
            CheckStopTakeInvertConditions();
            CheckStopOrdersConditions();
            CheckDisableTimeExpired();
            CheckDisableShortTimeExpired();


        }

        public bool IsPositionSmall()
        {
            if (Math.Abs(_positionCurr.Amount) >0 &&
                Math.Abs(_positionCurr.Amount) < _client.GetMinOrderSize(_instrument))
                return true;

            return false;
        }

        public void IncreasePosToMinPossible()
        {

            EnmOrderDir dir = _positionCurr.Amount > 0 ? EnmOrderDir.Buy : EnmOrderDir.Sell;
            decimal amount =  _client.GetMinOrderSize(_instrument);

            if (_positionCurr.Amount > 0)
                _expAmntAftInc = _positionCurr.Amount + amount;
            else
                _expAmntAftInc = _positionCurr.Amount - amount;

          
            _client.AddMarketOrder(_instrument, dir, amount);
            SetState(EnmTraderState._0119_WaitForPosInc);
            Log(String.Format("IncreasePosToMinPossible amount={0} _expectedAmount={1}",
                _positionCurr.Amount, _expAmntAftInc));
        }



        public void OnClosePositionsByMarketByTrader()
        {
            SetState(EnmTraderState._0111_WaitCloseByMarketByTrader);
            _timerWaitClosePosByMarket.Start();

          //  _currMinOrderSize = _client.GetMinOrderSize(_instrument);


            Log("OnClosePositionsByMarketByTrader");
        }


        public void OnSendOrderThrow(decimal priceThrow, decimal amountThrow, EnmOrderDir dir)
        {
            SetState(EnmTraderState._0117_WaitOrderThrow);
            _timerWaitThrowOrders.Start();
            Log("OnSendOrderThrow");
            _orderThrowData.PosPrev = GetPositionCurr();
            _orderThrowData.Price = priceThrow;
            _orderThrowData.Amount = amountThrow;

            _orderThrowData.Dir = dir;

            Log("Before send PosPrev.Amount.amount=" + _orderThrowData.PosPrev.Amount +
                " priceThrow=" + priceThrow + " dir=" + dir + " amountThrow=" + amountThrow);

        }



        public void Log(string message)
        {
            _logger.Log(message);
        }



        /// <summary>
        /// Call on user orders updates
        /// </summary>
        public void OnUserOrderUpdateLogics()
        {
            CheckCancellOrderAfterThrow();

        }


        public void UpdateBotsDisableTrading(CDisableTradeData disableTradeLoadedData)
        {

            _disableTradeData = disableTradeLoadedData;

        }

        public void AddOrderRest(decimal price, EnmOrderDir dir)
        {
                               
            CBotPos bp =  GetPositionCurr();
            decimal amountNeed = 0;
            if ( (dir == EnmOrderDir.Buy && bp.Amount < 0) ||
                 (dir == EnmOrderDir.Sell && bp.Amount > 0))

            {               
                amountNeed = Math.Abs(bp.Amount);
                if (amountNeed > 0)
                {
                    _operations.AddOrder(_instrument, price, dir, amountNeed);
                    Log(String.Format( "Order rest added price={0} amount={1} dir={2}",
                                         price, amountNeed, dir.ToString()));
                }  
            }
          
               
            
            



        }


        //===================================================================================================================================================================
        //===============================END PUBLIC METHODS =================================================================================================================
        //===================================================================================================================================================================


		private void TimerWaitThrowOrders_Elapsed(object sender, ElapsedEventArgs e)
		{
			_timerWaitThrowOrders.Stop();
			Log("TimerWaitThrowOrders_Elapsed");

			CheckOrderThrowApplied();
		}

        //2018-10-30
        private void TimerWaitClosePosByMarket_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (IS(EnmTraderState._0111_WaitCloseByMarketByTrader))
            {
                SetState(EnmTraderState._0110_Trading);
                _client.ResetIsClosingPos(_instrument);
                Log("Reset WaitCloseByMarketByTrader by timeout");
            }

            _timerWaitClosePosByMarket.Stop();
        }








        /// <summary>
        ///  Get total amount of orders by price and direction
        /// </summary>      
        private decimal GetUserOrderAmount(EnmOrderDir dir, decimal price)
		{
			lock (_userOrdersMon)
			{
				if (!_userOrdersMon.ContainsKey(dir))
					return 0;

				if (!_userOrdersMon[dir].ContainsKey(price))
					return 0;

				return _userOrdersMon[dir][price];
			}
		}

        private decimal GetUserOrderAmountByDir(EnmOrderDir dir)
        {
            decimal count = 0;
            lock (_userOrdersMon)
            {
                if (_userOrdersMon.ContainsKey(dir))
                    foreach (var kvp in _userOrdersMon[dir])
                        count += kvp.Value;
                
            }

            return count;
        }



        private int GetUserOrderAmountTotal()
        {
            int amount = 0;

            lock (_userOrdersMon)
            {
                if (_userOrdersMon.ContainsKey(EnmOrderDir.Buy))                
                    amount += _userOrdersMon[EnmOrderDir.Buy].Count;

                if (_userOrdersMon.ContainsKey(EnmOrderDir.Sell))
                    amount += _userOrdersMon[EnmOrderDir.Sell].Count;
                
            
            }
            return amount;

        }


        private CBotPos GetPositionCurr()
        {
            CBotPos pos = new CBotPos();
            lock (_positionCurr)
            {
                pos.Amount = _positionCurr.Amount;
                pos.AvPos = _positionCurr.AvPos;

            }

            return pos;
        }


        private bool IS(EnmTraderState state)
        {
            return _state.IS(state);
        }


        private void  SetState(EnmTraderState state)
        {
            _state.SetState(state);

        }
        


		private void ResetStopTakeInvert()
		{
			StopLossPrice = 0;
			TakeProfitPrice = 0;
			StopLossInvertPrice = 0;
			_operations.CallbackUpdateStopLossTakeProfit(_instrument);
		}

      



		private void ResetStopOrders()
		{
			BuyStopPrice = 0;
			SellStopPrice = 0;
			_operations.CallbackUpdateStopOrders(_instrument);
			
		}


        /// <summary>
        /// Check condition for apply stop, take, invert order.
        /// Send order and set waiting state
        /// </summary>
        private void CheckStopTakeInvertConditions()
        {

            CBotPos pos = GetPositionCurr();

            if (IS(EnmTraderState._0110_Trading))
            {

                if (TakeProfitPrice != 0)
                {
                    if (pos.Amount > 0) //buy
                    {
                        if (Bid >= TakeProfitPrice)
                        {
                            _operations.CloseByTakeProfit(_instrument);
                            SetState(EnmTraderState._0113_WaitTakeProfitApplyed);							
                            Log("TakeProfit sent. Long pos.");
                            return;
                        }
                    }
                    if (pos.Amount <= 0) //sell
                        if (Ask <= TakeProfitPrice)
                        {
                            _operations.CloseByTakeProfit(_instrument);
                            SetState(EnmTraderState._0113_WaitTakeProfitApplyed);							
							Log("TakeProfit sent. Short pos");
                            return;

                        }
                }

                if (StopLossPrice != 0)
                {
                    if (pos.Amount > 0) //buy
                        if (Bid <= StopLossPrice)
                        {
                            _operations.CloseByStopLoss(_instrument);
                            SetState(EnmTraderState._0112_WaitStoplossApplyed);							
							Log("StopLoss sent. Long pos.");
                            return ;

                        }

                    if (pos.Amount < 0) //sell
                        if (Ask >= StopLossPrice) //was >
                        {
                            _operations.CloseByStopLoss(_instrument);
                            SetState(EnmTraderState._0112_WaitStoplossApplyed);							
							Log("StopLoss sent. Short pos.");
                            return;

                        }
                }


                if (StopLossInvertPrice != 0)
                {

					//2017-03-13 only if pos is not equal zero
                    if (pos.Amount > 0) //buy
                        if (Bid <= StopLossInvertPrice)
                        {
                            _posRembInvrt.Amount = pos.Amount;
                            _posRembInvrt.AvPos = pos.AvPos;
                            _operations.SendStopLossInvert(_instrument, pos);
                            SetState(EnmTraderState._0114_WaitStopLossInvertApplyed);							
							Log("StopLossInvert sent. Long pos.");
                            return;

                        }


                    if (pos.Amount < 0) //sell
                        if (Ask >= StopLossInvertPrice)
                        {
                            _posRembInvrt.Amount = pos.Amount;
                            _posRembInvrt.AvPos = pos.AvPos;
                            _operations.SendStopLossInvert(_instrument, pos);							
                            SetState(EnmTraderState._0114_WaitStopLossInvertApplyed);							
							Log("StopLossInvert sent. Short pos.");
                            return;
                        }
                }

            }


        }


        /// <summary>
        /// If stoporder condition is satisfied
        /// send stop order and set wait state
        /// </summary>
        private void CheckStopOrdersConditions()
        {

            //if not trading state (wait etc..,) no need
            //to trigger stoporders
            if (!IS(EnmTraderState._0110_Trading))
                return;

			CBotPos currPos = GetPositionCurr();

            if (BuyStopPrice != 0)            
                if (Ask >= BuyStopPrice)                
                   if (BuyStopAmount != 0)
                   {                           
                        
                        _operations.TriggerStopOrder(_instrument, EnmOrderTypes.BuyStop, BuyStopAmount);
					   SetState(EnmTraderState._0115_WaitBuyStopApplyed);
					   _posBeforeStopOrder.Amount = currPos.Amount;					   
                        Log("BuyStop sent");
                        return;
                   }
                    
            


           if (SellStopPrice != 0)           
                if (Bid <= SellStopPrice)                
                    if (SellStopAmount != 0)
                    {
                        SetState(EnmTraderState._0116_WaitSellStopApplyed);
                        _operations.TriggerStopOrder(_instrument, EnmOrderTypes.SellStop, SellStopAmount);
						_posBeforeStopOrder.Amount = currPos.Amount;
                        Log("SellStopSent");
                        return;
                    }
                                    
        }


		/// <summary>
		/// Check transitional states and change them if need
        /// 
        /// Call from
        /// OnUpdatePositions
		/// </summary>
		private void CheckForStateChange()
		{
			CBotPos currPos = GetPositionCurr();

			if (IS(EnmTraderState._0111_WaitCloseByMarketByTrader))
			{
				//full position close
				if (currPos.Amount == 0)
				{
								
					SetState(EnmTraderState._0110_Trading);
					ResetStopTakeInvert();
					ResetStopOrders();
					Log("Position closed by trader");
				}

                //if (currPos.Amount < _client.)


			}

			if (IS(EnmTraderState._0115_WaitBuyStopApplyed) && currPos.Amount == _posBeforeStopOrder.Amount + BuyStopAmount ||
				IS(EnmTraderState._0116_WaitSellStopApplyed) && currPos.Amount == _posBeforeStopOrder.Amount - SellStopAmount)
			{
				
								
					SetState(EnmTraderState._0110_Trading);
					ResetStopOrders();
					Log("Stop order applyed");
				
			}

			
			if (IS(EnmTraderState._0112_WaitStoplossApplyed)||
				IS(EnmTraderState._0113_WaitTakeProfitApplyed))				
			{
				//if position closed than stoploss applyed
				if (currPos.Amount == 0)
				{					
					Log("StopLoss or takeprofit applyed");
					SetState(EnmTraderState._0110_Trading);
					ResetStopTakeInvert();
				}
			}

			if (IS(EnmTraderState._0114_WaitStopLossInvertApplyed))
			{
				// inverting pos changed direction and amount is the same that was before send
				if(currPos.Amount * _posRembInvrt.Amount <0 &&
					Math.Abs(_posRembInvrt.Amount) == Math.Abs(currPos.Amount))        
				{								
					Log("StopLossInvert applyed");
					SetState(EnmTraderState._0110_Trading);
					ResetStopTakeInvert();
				}

			}
            
            if (IS(EnmTraderState._0110_Trading))
            {
                //close by limit order
                if(currPos.Amount == 0 && _positionPrev.Amount!=0)
                {
                    Log("Position just closed ");
                    ResetStopTakeInvert();
                }
            }

            if (IS(EnmTraderState._0119_WaitForPosInc))
                if (currPos.Amount == _expAmntAftInc)
                {
                    _client.ClosePositionOfInstrument(_instrument);
                    SetState(EnmTraderState._0110_Trading);
                }

            




		}

		

		/// <summary>
		/// After throw order sent and if we requested to cancell orders 
		/// (which not applyed as market and position was not opened).
		/// Wait till all limit orders on the requested price deleted
		/// </summary>
		private void CheckCancellOrderAfterThrow()
		{
			if (IS(EnmTraderState._0118_WaitCancellOrderAfterThrow))
			{
				decimal amount = GetUserOrderAmount(_orderThrowData.Dir,  _orderThrowData.Price);
				if (amount == 0)
				{
					Log("CheckCancellOrderAfterThrow. amount=0");
					SetState(EnmTraderState._0110_Trading);

				}

			}

		}

		/// <summary>
		/// Check if throw order applied
		/// </summary>
		private void CheckOrderThrowApplied()
		{

			Log("CheckOrderThrowApplied");
            
			if (IS(EnmTraderState._0117_WaitOrderThrow))
			{

				var pos =  GetPositionCurr();
				decimal amountOrder = GetUserOrderAmount(_orderThrowData.Dir, _orderThrowData.Price);

				//All throw order volume became positions. Nothing to do.
                //Continue trading.
				if (amountOrder == 0)
				{
					Log("CheckOrderThrowApplied. amountOrder=0");
					SetState(EnmTraderState._0110_Trading);
				}
				//Part of the throw order became limit orders, cancell this part of orders 
                //and wait.
				else 
				{
					Log("CheckOrderThrowApplied. amountOrder != 0");
					_operations.CancellOrdersByPriceAndDir(_instrument, _orderThrowData.Dir, _orderThrowData.Price);
					SetState(EnmTraderState._0118_WaitCancellOrderAfterThrow);
				}


				
			}


		}


      




     

     



        private void CheckDisableTimeExpired()
        {
            if (_disableTradeData == null)
                return;

            //if time expired trigger diasble modes
            if (!_modeDisableTraderTrading &&
                _disableTradeData.TradeDisableCode == CodeDisableTradeByTime._03_DisableAll &&
                _client.ServerTime > _disableTradeData.DtDisable)
            {
                _modeDisableTraderTrading = true;
                Log("Mode disable trader trading was set");
            }  
      


            if (_modeDisableTraderTrading)
            {
                CBotPos bp = GetPositionCurr();
                decimal amountPos = bp.Amount;
                int orderAmount = GetUserOrderAmountTotal();

                if (orderAmount == 0)//No opedned orders
                {
                    if (amountPos == 0) //No opened position
                    {
                        if (!IS(EnmTraderState._0200_TradeDisableByTimeExpired))
                            //disable trading
                            SetState(EnmTraderState._0200_TradeDisableByTimeExpired);
                    }
                    else // opened position exists
                    {
                        if (!IS(EnmTraderState._0202_WaitClosePosOnTimeTradeExpired))                             
                          {
                              // DO Close all positions
                             CloseAllPositions();
                              SetState(EnmTraderState._0202_WaitClosePosOnTimeTradeExpired);                                                                     
                          }                                                   
                    }                   
                }
                else // opened order exist
                {
                    
                      if (!IS(EnmTraderState._0201_WaitCancellOrdersOnTimeTradeExpired))
                      {
                       //Do cancell all orders
                          _operations.CancellAllOrdersByInstrument(_instrument);
                          SetState(EnmTraderState._0201_WaitCancellOrdersOnTimeTradeExpired);                       
                       }                                           
                }               
            }


        }


        private void CheckDisableShortTimeExpired()
        {
            if (_disableTradeData == null)
                return;

            //if time expired trigger diasble modes
            if (!_modeDisableTraderShortTrading &&
                _disableTradeData.TradeDisableCode == CodeDisableTradeByTime._02_DisableShort &&
                _client.ServerTime > _disableTradeData.DtDisable)
            {
                _modeTraderCheckDisableShortConditions = true;
                Log("Mode check disable short conditions set");
            }

           CBotPos bp = GetPositionCurr();
           decimal amountPos = bp.Amount;
           int orderAmount = GetUserOrderAmountTotal();
            //check if condition break or not
            if (_modeTraderCheckDisableShortConditions)                
            {

            
                if (amountPos >= 0) // long pos or no opened - it's OK
                {
                    _modeTraderCheckDisableShortConditions = false;
                    _modeDisableTraderShortTrading = true;
                    Log("Mode disable trader short trading set");
                                       
                }
                else   // short postios is opened - condition braked
                {
                    if (!_modeDisableTraderByShortViol)
                    {
                      _modeDisableTraderByShortViol = true;
                      _modeTraderCheckDisableShortConditions = false;
                     Log("Mode disable trading short by short violated was set");
                       

                    }
                }
            }


            //condition brake close all and disable trading
            if (_modeDisableTraderByShortViol)
            {
                if (orderAmount == 0) //No orders.
                {
                    if (!IS(EnmTraderState._0250_TradeDisableByShortExpired))
                    {   //All positions are closed. Possible disable trading
                        if (amountPos == 0)                       
                            SetState(EnmTraderState._0250_TradeDisableByShortExpired);

                        
                    }
                    //Must be opened position at this point (from above logics).
                    if (!IS(EnmTraderState._0252_WaitClosePositionsByShortTimeExpired))
                    {
                        CloseAllPositions();
                        SetState(EnmTraderState._0252_WaitClosePositionsByShortTimeExpired);
                    }

                }
                else //Orders exist. First cancell orders.
                {
                    if (!IS(EnmTraderState._0251_WaitCancelOrdesrByShortTimeExpired))
                    {
                        _operations.CancellAllOrdersByInstrument(_instrument);
                        SetState(EnmTraderState._0251_WaitCancelOrdesrByShortTimeExpired);
                    }

                }
                                                                        
              }

            

        }


        private void CloseAllPositions()
        {
            _operations.ClosePositionOfInstrument(_instrument);

        }

		


    













    }
}
