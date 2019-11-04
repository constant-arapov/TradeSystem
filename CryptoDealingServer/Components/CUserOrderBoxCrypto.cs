using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading;
using Common.Utils;


using TradingLib.Data;
using TradingLib.Enums;


using TradingLib.Interfaces.Components;
using TradingLib.BotEvents;


using BitfinexCommon;
using BitfinexCommon.Enums;
using BitfinexCommon.Helpers;
using BitfinexCommon.Messages.Response;

using CryptoDealingServer.Interfaces;
using CryptoDealingServer.Helpers;


namespace CryptoDealingServer.Components
{
    public class CUserOrderBoxCrypto : IUserOrderBox
    {

        IClientUserOrderBoxCrypto _client;

		
		CUserOrderLog _userOrderLog = new CUserOrderLog();

        //do not use because of "-0.0" problem
        //COrderStatusTracker _userOrderTracker = new COrderStatusTracker();
        CDBGUserOrderBox _dbg;

        public CUserOrderBoxCrypto(IClientUserOrderBoxCrypto client)
        {
            _client = client;

            //added 2018-03-23 for capability
            mxListRawOrders = new Mutex();

            _dbg = new CDBGUserOrderBox(client);
        }

        //for capability
        private List<CRawOrdersLogStruct> _listRawOrdersStruct = new List<CRawOrdersLogStruct>();
        public List<CRawOrdersLogStruct> ListRawOrdersStruct
        {
            get
            {
                return _listRawOrdersStruct;
            }
        }



        public Mutex mxListRawOrders { get; set; }
        public Dictionary<long, CRawOrdersLogStruct> DictUsersOpenedOrders { set; get; }





        /// <summary>
        /// 
        /// 
        /// Algorithm is described here:      
        /// https://github.com/bitfinexcom/bitfinex-api-node/issues/153
        /// </summary>
        /// <param name="respOrders"></param>
        /// <param name="orderAction"></param>
        public void ProcessOrder(ResponseOrders respOrders, EnmOrderAction orderAction)
        {
            //TODO normal

			string instrument = respOrders.Symbol.Remove(0, 1);
			int decimalsOfVolume = _client.GetDecimalVolume(instrument);
			//int iAmount = (int)Math.Abs(CUtilConv.GetIntVolume((decimal)respOrders.Amount, decimalsOfVolume));
			//int iAmountOrig = (int)Math.Abs(CUtilConv.GetIntVolume((decimal)respOrders.AmountOrig, decimalsOfVolume));
            decimal amount =  Convert.ToDecimal(Math.Abs((double)respOrders.Amount));
            decimal amountOrig = Convert.ToDecimal(Math.Abs((double)respOrders.AmountOrig));

            //int iAmountRest = (int) CBfxUtils.GetIntVolume( Math.Abs((decimal)respOrders.AmountOrig) -  Math.Abs((decimal)respOrders.Amount),decimalsOfVolume);
            DateTime momentOrderCreate = CUtilTime.DateTimeFromUnixTimestampMillis((long)respOrders.MtsCreate);
            DateTime momentOrderUpdate = CUtilTime.DateTimeFromUnixTimestampMillis((long)respOrders.MtsUpdate);

			//check advance data in status field 
			//Could by like "Cancelled" or  more complex "EXECUTED @ 10908.0(-0.0)
            EnmBfxOrderStatus orderStatus = CBfxUtils.GetOrderStatus(respOrders.OrderStatus);

			//common parametters same for all order update type
            CRawOrdersLogStruct userOrdLogStruct = new CRawOrdersLogStruct
            {
                Id_ord = respOrders.Id,
                Ext_id = (int)respOrders.Gid,
            
                Price = Math.Abs((decimal)respOrders.Price),
			//	Amount = (int)iAmount,
            //    Amount_rest = iAmountRest,
				Instrument = instrument,
                Action = (sbyte) BfxOrdStatusToEnmOrderAction(orderStatus),
            //    Moment = momentOrderCreate           
            };


			

            //case of add order
            if (orderAction == EnmOrderAction.Added)
            {
                //2018-04-04 NOTE. Added order could be two cases:
                //1) Jast Added limit order - we do use amount
                //2) Put limit order by market and it is partialy filled - we alse do
                //   use amount as we initial order in bot with rest of order.  
                //   One important thing - we do override Action from "Partialy filled" to "OrderAccepted"


                userOrdLogStruct.Amount = amount;
                userOrdLogStruct.Dir = respOrders.Amount > 0 ? (sbyte)EnmOrderDir.Buy : (sbyte)EnmOrderDir.Sell;

                //Override to "OrderAccepted" for "partially filled" case
                if (orderStatus == EnmBfxOrderStatus.PartiallyFilled)
                    userOrdLogStruct.Action = (sbyte) EnmOrderAction.Added;

                
                 _client.TriggerRecalculateBot(userOrdLogStruct.Ext_id,
                                        userOrdLogStruct.Instrument,
                                        EnmBotEventCode.OnOrderAccepted, 
                                        userOrdLogStruct);
                //added 2018-06-06
                //first process deal if exist
                //old place of CheckForDealsWithNoBotId before 2018-08-03

                CUserOrder userOrder = new CUserOrder
				{
					Amount = amount,
					BotId = userOrdLogStruct.Ext_id,
					Instrument = userOrdLogStruct.Instrument,
					OrderActionLast = EnmOrderAction.Added,
					OrderId = userOrdLogStruct.Id_ord,
					Status = respOrders.OrderStatus

				};
				_userOrderLog.Update(userOrder);
                //2018-06-03 move AFTER adding order (as is _userOrderLog later)
                _client.CheckForDealsWithNoBotId(userOrdLogStruct.Id_ord, userOrdLogStruct.Ext_id);

                _dbg.DBGProcessOrderAdded(userOrder);
            }
            //case of fully deleted order
			else if (orderAction == EnmOrderAction.Deleted && orderStatus == EnmBfxOrderStatus.Canceled)
            {
                                                                        
                 //just tell bot to cancell order
                 _client.TriggerRecalculateBot(userOrdLogStruct.Ext_id,
                                        userOrdLogStruct.Instrument,
                                        EnmBotEventCode.OnOrderCancel,
                                        userOrdLogStruct);


                //added 2018-05-13
                //first process deal if exist
                //old place of CheckForDealsWithNoBotId before 2018-08-03


                //no order is not need even for "very late" update.
                //2018-05-31 removed (user cancel before "te" and "tu"- having a problem)                
                //_userOrderLog.Delete(userOrdLogStruct.Ext_id, 
                //				  userOrdLogStruct.Instrument, 
                //			  userOrdLogStruct.Id_ord);


                //2018-05-31 set order deleted
                CUserOrder userOrder = new CUserOrder
                {
                    Amount = amount,
                    BotId = userOrdLogStruct.Ext_id,
                    Instrument = userOrdLogStruct.Instrument,
                    OrderActionLast = EnmOrderAction.Deleted,
                    OrderId = userOrdLogStruct.Id_ord,
                    Status = respOrders.OrderStatus

                };

                _userOrderLog.Update(userOrder);
                //2018-06-03 move AFTER adding order (as is _userOrderLog later)
                _client.CheckForDealsWithNoBotId(userOrdLogStruct.Id_ord, userOrdLogStruct.Ext_id);

                _dbg.DBGDeleted(userOrdLogStruct);

               

                
                //TODO check not processed deals
            }  
            //case of full or partial executed order
			else if ((orderAction == EnmOrderAction.Deleted && orderStatus == EnmBfxOrderStatus.Executed) ||
                    //case of partial filled
					 (orderAction == EnmOrderAction.Update && orderStatus == EnmBfxOrderStatus.PartiallyFilled))                                                                                 
                       
            {

				decimal amountUse = amountOrig - amount;
                //check for partially filled
                userOrdLogStruct.Dir = respOrders.AmountOrig > 0 ? (sbyte)EnmOrderDir.Buy : (sbyte)EnmOrderDir.Sell;

                //2018-04-04 possible cases:
                //1) Deleted and executed order by market when limit order was not set yet, on that case amount=0
                //   but as no order exist in BotBase.MonitorOrders - nothing happens
                //2) Deal for order that exists, we do encrease amounts of orders on that case -NOT TESTED-
                userOrdLogStruct.Amount = amount;

                //first tell bot order deal 
                _client.TriggerRecalculateBot(userOrdLogStruct.Ext_id,
                                        userOrdLogStruct.Instrument,
                                        EnmBotEventCode.OnOrderDeal,
                                        userOrdLogStruct);


                //_userOrderTracker.Update(userOrdLogStruct.Ext_id, userOrdLogStruct.Id_ord, respOrders.OrderStatus);

                //2018-06-13 the same as in all two other conditions
                //old place of CheckForDealsWithNoBotId before 2018-08-03


                EnmOrderAction ordActUse = EnmOrderAction.Unknown;

				if (orderStatus == EnmBfxOrderStatus.Executed)
					ordActUse = EnmOrderAction.Deal;
				else if (orderStatus == EnmBfxOrderStatus.PartiallyFilled)
					ordActUse = EnmOrderAction.PartialFilled;

             

                CUserOrder userOrder = new CUserOrder
				{
					Amount = amount,
					BotId = userOrdLogStruct.Ext_id,
					Instrument = userOrdLogStruct.Instrument,
					OrderActionLast = ordActUse,
					OrderId = userOrdLogStruct.Id_ord,
					Status = respOrders.OrderStatus

				};

				_userOrderLog.Update(userOrder);

                _client.CheckForDealsWithNoBotId(userOrdLogStruct.Id_ord, userOrdLogStruct.Ext_id);


                _dbg.DBGExecPartFilled(ordActUse,userOrder);
             }
            
        }

        private void CheckUnProcessedDeals()
        {
            //_lstNotProcessedDeals


        }


       



        public static EnmOrderAction BfxOrdStatusToEnmOrderAction(EnmBfxOrderStatus bfxOrdStatus)
        {
            if (bfxOrdStatus == EnmBfxOrderStatus.Active)
                return EnmOrderAction.Added;
            else if (bfxOrdStatus == EnmBfxOrderStatus.Canceled)
                return EnmOrderAction.Deleted;
            else if (bfxOrdStatus == EnmBfxOrderStatus.Executed)
                return EnmOrderAction.Deal;
            //added 2018-04-04
            else if (bfxOrdStatus == EnmBfxOrderStatus.PartiallyFilled)
                return EnmOrderAction.PartialFilled;

            return EnmOrderAction.Unknown;

        }

		public int GetBotIdOfOrder(string instrument, long orderId)
		{
			return _userOrderLog.GetBotId(instrument, orderId);
		}

        public void CleanUserOrderLog()
        {
            _userOrderLog.Cleaning();

        }

        public void CleanFullUserOrderLog()
        {
            _userOrderLog.CleaningFull();
        
        }






    }
}
