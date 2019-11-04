using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Collections;
using Common.Logger;


using TradingLib;

using TradingLib.Enums;
using TradingLib.Data;
using TradingLib.Data.DB;
using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Clients;
using TradingLib.Abstract;
using TradingLib.Bots;
using TradingLib.BotEvents;

using TradingLib.Common.VMCalc;
using TradingLib.ProtoTradingStructs;


using CryptoDealingServer.Interfaces;


namespace CryptoDealingServer.Components
{
    public class CUserDealsPosBoxCrypto : CBaseUserDealsPosBox //IUserDealsPosBox
    {

        private Dictionary<long, long> _dictBotIdDealId = new Dictionary<long, long>();

		private CLogger _logger;
        


        private IClientUserDealsPosBoxCrypto _client;


        public CUserDealsPosBoxCrypto(IClientUserDealsPosBoxCrypto client, CBaseVMCalc vmCalc, bool bBuildNonSavedPositionsFromDealsLog) 
            : base (client, vmCalc, bBuildNonSavedPositionsFromDealsLog)
        {
            _client = client;
			_logger = new CLogger("CUserDealsPosBoxCrypto");
        }


		public void Log(string message)
		{
			_logger.Log(message);
		}
		
		//deprecated brunch remove if not need
        public void Update(string instrument, decimal price, EnmOrderDir dir,
                            int amount, long extId, 
                            DateTime moment, 
                            long mtsCreate,
                            decimal fee)
        {

            CRawUserDeal rd = new CRawUserDeal
            {
                Instrument = instrument,
                Amount = amount,
                Dir = (sbyte) dir,
                Ext_id_buy = dir == EnmOrderDir.Buy ? extId : 0,
                Ext_id_sell = dir == EnmOrderDir.Sell ? extId : 0,
                Price = price,
                ReplId = GetBotDealId(extId, mtsCreate),
                Moment = moment,
                Fee_buy =  0,
                Fee_sell = 0


            };

            //TODO chek if we already processed deal

            CalculateBotsPos(rd, isOnlineASTSCalc: true);

            UserDealsPosBoxClient.UpdateGUIDealCollection(rd);


        }

        /// <summary>
        /// On update deal.
        /// 
        /// Call from:
        /// 1) CCryptoDealingServer.UpdateUserDeals - when "te" recieved
        /// 2) CCryptoDealingServer.UpdateUserDeals - when "tu" recieved
        /// 
        /// </summary>

        public void Update(long dealId, long orderId, string instrument, EnmOrderDir dir,
							decimal amount, int botId, decimal price, DateTime moment,
							long mtsCreate, decimal fee)
		{
			
			Log(string.Format("[Update] botId={0} dealId={1} orderId={2} instrument={3} dir={4} amount={5} price={6} moment={7} mtsCreate={8} fee={9})",
				botId,//0
				dealId,//1
				orderId,//2
				instrument,//3
				dir,//4
				amount,//5
				price, //6
				moment, //7
				mtsCreate, //8
                fee //9
				));

			CRawUserDeal rd = new CRawUserDeal
			{
				Id_Deal = dealId,
				Instrument = instrument,
				Amount = Math.Abs(amount),
				Dir = (sbyte)dir,
				Ext_id_buy = dir == EnmOrderDir.Buy ? botId : 0,
				Ext_id_sell = dir == EnmOrderDir.Sell ? botId : 0,
				Price = price,
				ReplId = GetBotDealId(botId, mtsCreate),
				Moment = moment,
				Fee_buy = dir == EnmOrderDir.Buy ? Math.Abs(fee) : 0,
				Fee_sell = dir == EnmOrderDir.Sell ? Math.Abs(fee) : 0
                

			};
			

			CalculateBotsPos(rd, isOnlineASTSCalc: true);

            //2018-05-24
            //In case we recieve fee data in deal 
            //do trigger fee calculation. This is  
            //seldom case, when we where not able to process
            //"Update" deal from "te" (because orderId-botId recieved later).
            //and process deal from "tu"
            //Usually we recieve "te" with no fee, and then "tu" with fee
            if (fee != 0)
            {
                Log("Update fee from [Update]");

                //2018-06-16 TODO remove all orderdir to dealDir
                UpdateFee(botId, instrument, dealId, Math.Abs(fee), (EnmDealDir) dir );
            }
           

			UserDealsPosBoxClient.UpdateGUIDealCollection(rd);


		}



        public void CreateDictBotDealId(List<CBotBase> lstBots)
        {
            lstBots.ForEach(bot =>
             _dictBotIdDealId[bot.BotId] = 0);

        }


        public override void OnAddNewBot(int botId)
        {
            _dictBotIdDealId[botId] = 0;
        }






        /// <summary>
        /// Must be 
        /// 1) Unique
        /// 2) Grow
        /// May be do something more complex in the future
        /// </summary>
        /// <param name="botId"></param>
        /// <param name="mtsCreate"></param>
        /// <returns></returns>
        public long GetBotDealId(long botId, long mtsCreate)
        {

            int maxValue = 999999;

            _dictBotIdDealId[botId] = _dictBotIdDealId[botId] < maxValue ? _dictBotIdDealId[botId]+1 : 0;
            long res =  mtsCreate * 1000000 +  _dictBotIdDealId[botId];
            return res;

        }




        /*
        private decimal GetFee(decimal price, decimal amount, ref decimal fee, int accountId)
        {
            if (amount==0)
            {
                Log("GetDealingFee. amount is 0");
                return 0;
            }

            if (price == 0)
            {
                Log("GetDealingFee. price is 0");
                return 0;
            }



            decimal turnover = amount * price;
            decimal markerOrderProc = 0.002m;
            decimal tol = 0.00001m;
           

            decimal ratio = fee / turnover;



            if (ratio  >= markerOrderProc - tol)
            {
                Log(String.Format("GetDealingFee. Market order case ratio={0} turnover={1} price={2} amount={3} feeStock={4}", 
                                    ratio, //0                                  
                                    turnover, //1
                                    price, //2
                                    amount, //3
                                    fee));//4);
                return 0;


            }
            else
            {
                //decimal feeDealingProc = 0.0005m;
                decimal feeDealingProc = _client.GetFeeDealingPcnt(accountId);


                decimal feeDealing = turnover * feeDealingProc;
                Log(String.Format("GetDealingFee.Limit order case ratio={0} feeDeailng={1} turnover={2} price={3} amount={4} feeStock={5}",
                                    ratio, //0
                                    feeDealing, //1
                                    turnover, //2
                                    price, //3
                                    amount, //4
                                    fee));//5
                return feeDealing;
            }






            
        }
        */
        private void CalculateFees(int accountId,  decimal feeInp, ref decimal calcedfee,   
                                            ref decimal feeDealing, ref decimal feeStock, CPosChangeFrag posChangeFrag)
        {
  
            feeStock = feeInp;
           


            if (posChangeFrag.Amount == 0)
            {
                Log("GetDealingFee. amount is 0");
                return;
            }

            if (posChangeFrag.Price == 0)
            {
                Log("GetDealingFee. price is 0");
                return;
            }



            decimal turnover = posChangeFrag.Amount * posChangeFrag.Price;
            decimal markerOrderProc = 0.002m;
            decimal tol = 0.00001m;


            decimal ratio = feeStock / turnover;


            //market order case
            if (ratio >= markerOrderProc - tol)
            {
                //TODO depend of current market order comiss (depends of month turnover)
                //2018-09-11               
                //calcedfee = feeStock;
                decimal feeMarketProc = _client.GetFeePctMarket(accountId);
                calcedfee = feeMarketProc * turnover;

                Log(String.Format("GetDealingFee. Market order case ratio={0} turnover={1} price={2} amount={3} feeInp={4} calcedFee={5} feeStock={6} feeDealing={7}",
                                    ratio, //0                                  
                                    turnover, //1
                                    posChangeFrag.Price, //2
                                    posChangeFrag.Amount, //3
                                    feeInp,  //4
                                    calcedfee,//5
                                    feeStock, //6
                                    feeDealing //7

                                    ));

                feeDealing = calcedfee;
                
                
                //fee is equal Fee_Stock, FeeDealing is zero
              


            }
            else
            {
                //2018-09-11
                //decimal feeDealingProc = _client.GetFeeDealingPcnt(accountId);
                decimal feeDealingProc = _client.GetFeePcntLim(accountId);


                //percents where set so calc fee
                string eq;
                if (feeDealingProc != 0m)
                {

                    feeDealing = turnover * feeDealingProc;

                    calcedfee = feeDealing;

                    eq = "feeDealingProc != 0m";
                    //fee is equal Fee_dealing, Fee_stock is equal original value


                }
                else
                {
                    calcedfee = feeStock;
                    eq = "feeDealingProc == 0m";

                    //fee is equal Fee_Stock, FeeDealing is zero

                }

                Log(String.Format("GetDealingFee.Limit order case {0}  ratio={1}  turnover={2} price={3} amount={4} feeInp={5} calcedFee={6} feeStock={7} feeDealing={8}",
                                  eq, //0
                                  ratio, //1                                        
                                  turnover, //2
                                  posChangeFrag.Price, //3
                                  posChangeFrag.Amount, //4
                                  feeInp, //5
                                  calcedfee, //6
                                  feeStock, //7
                                  feeDealing //8
                                  ));



            }

            posChangeFrag.Fee = calcedfee;
            posChangeFrag.FeeDealing = feeDealing;
            posChangeFrag.Fee_Stock = feeStock; 







        }






        /// <summary>
        /// Late updates fee data of BotPos. Deals data (id, price etc)
        /// was already saved before to specific lists of CPosChangeFrag structs: 
        /// ListOpeningPosChanges and ListClosingPosChanges
        /// So we need to find CPosChangeFrag element with DealId and update 
        /// it's fee. 
        /// Could be three cases:
        /// 1) If BotPos is opened (is not fully closed yet) than update fee of DictPositionsOfBots
        ///     element.
        /// 2) If BotPot was fully closed than update fee of DicBotPosLog
        /// 3) If position "rotated". For that case, if botpos is opening,
        ///   we check last DicBotPosLog element and if IsFeeLateCalced is not set
        ///   we perform recalc fee for this element. Note: fee of rotating deal
        ///   adds to the new "opened" position.
        /// 
        /// If position was fully closed or rotated, saved to DicBotPosLog and all fees are
        /// not zeroes do update BotPos fee, and update other systems(Database, bots, etc)
        /// 
        ///     2018-06-16 - as could be two deals with the same DealId (cross situation)
        ///                  do check DealId and Dir now.
        /// 
        /// Call from:
        /// 1) CryptoDealingServer.UpdateUserDealsLateUpd
        /// 2) UserDealsPosBoxCrypto.Update
        /// </summary>
        /// <param name="botId"></param>
        /// <param name="instrument"></param>
        /// <param name="dealId"></param>
        /// <param name="fee"></param>
		public void UpdateFee(int botId, string instrument, long dealId, decimal fee, EnmDealDir dir)
		{
			bool bDealWithNoFee = false;
			bool bFoundInPosLog = false;
            bool bFoundInOpenedPos = false;
            bool bFoundHistNotFeeCalced = false;
            bool bNullAnomalyFound = false;

			CBotPos bpClosed = null;
            decimal calcedFee = 0;
            decimal feeDealing = 0;
            decimal feeStock = 0;
          

            decimal calcedFeeForPos = 0;
            decimal feeDealingForPos = 0;
            decimal feeStockForPos = 0;

            Log(String.Format("[UpdateFee] botId={0} instrument={1} dealId={2} fee={3}",
                                botId, instrument, dealId, fee) );


           


            //first find in opened positions

            lock (LckDictPositionsOfBots)
			{
				if (DictPositionsOfBots.ContainsKey(botId))				
					if (DictPositionsOfBots[botId].ContainsKey(instrument))
					{
						CBotPos bp = DictPositionsOfBots[botId][instrument];
						foreach (var el in bp.ListOpeningPosChanges)
							if (el.IdDeal == dealId && el.Dir == dir) //2018-06-16 check dir
							{
                               
                                //2018-06-07 protect against override fee with wrong "0" value
                                if (fee == 0 && el.Fee != 0)
                                {                                   
                                    Log("Protect to owerrite with zero. ListOpeningPosChanges");
                                }
                                else
                                {
                                    Log("Deals of DictPositionsOfBot's opening positions. Do Update fee.");
                                    //2018-07-19
                                    //2018-08-03 removed check
                                   // if (el.Fee == 0)                                                                          
                                        CalculateFees(botId,  fee, ref calcedFee,   ref feeDealing, ref feeStock, el);

                                    
                                                                                                        
                                    bFoundInOpenedPos = true;
                                }
                                //return; //2018-05-23 removed
                            }
                        
                        foreach (var el in bp.ListClosingPosChanges)
                            if (el.IdDeal == dealId && el.Dir == dir)  //2018-06-16 check dir
                            {
                                //2018-06-07 protect against override fee with wrong "0" value
                                if (fee == 0 && el.Fee != 0)
                                {
                                    Log("Protect to owerrite with zero. ListClosingPosChanges");
                                }
                                else
                                {
                                    Log("Deals of DictPositionsOfBot's closing positions. Do Update fee.");
                                    //2018-07-19
                                    //2018-08-03 removed check
                                    //if (el.FeeDealing == 0)
                                        CalculateFees(botId, fee, ref calcedFee, ref feeDealing, ref feeStock, el);


                                    bFoundInOpenedPos = true;
                                }
                                //return; //2018-05-23 removed

                            }



                    }			
			}

			//then look in historical positions	
			lock (DicBotPosLog)
			{
                //if not found in opened pos find in poslog
                if (!bFoundInOpenedPos)
				    if (DicBotPosLog.ContainsKey(botId))
					    if (DicBotPosLog[botId].ContainsKey(instrument))
                            if (DicBotPosLog[botId][instrument].Count>0)
					        {
    						    bpClosed = DicBotPosLog[botId][instrument].Last();
						        foreach (var el in bpClosed.ListClosingPosChanges)
						        {
                                    if (el.IdDeal == dealId && el.Dir == dir) //2018-06-16 check dir
                                    {
                                        //TODO normal loading  from DB after disconnect
                                        //2018-07-19
                                        //   //2018-08-03 removed check
                                        //if (el.FeeDealing == 0)
                                            CalculateFees(botId, fee, ref calcedFee, ref feeDealing, ref feeStock, el);





                                        bFoundInPosLog = true;
                                        Log("bFoundInPosLog");
                                    }
                                    else if (el.Fee == 0)
                                    {
                                        bDealWithNoFee = true;
                                        Log(String.Format("bDealWithNoFee DealId={0}",el.IdDeal));
                                    }							
						        }   
					        }
                
                //2018-05-23 check if last pos "fee processed".
                //This is for processing "rotate" position case
                 if (bFoundInOpenedPos)
                    if (DicBotPosLog.ContainsKey(botId))
                        if (DicBotPosLog[botId].ContainsKey(instrument))
                            if (DicBotPosLog[botId][instrument].Count > 0)
                            {
                                bpClosed = DicBotPosLog[botId][instrument].Last();
                                if (bpClosed!=null)
                                    if (bpClosed.IsFeeLateCalced == 0)
                                    {
                                        bFoundHistNotFeeCalced = true;
                                        Log("bFoundHistNotFeeCalced");
                                    }
                            }

                //2018-06-03 protect against  bpClosed==null "anlomaly"
                if (bFoundInPosLog && bpClosed == null)
                {
                    bpClosed = DicBotPosLog[botId][instrument].Last();
                    bNullAnomalyFound = true;
                    Log("Null anomaly found");
                }
                //end protect amomaly


                //TODO check if fee not recalc yet
                //if all deals of historical postions are closed, all fees are saved (no bDealWithNoFee)
                // perform recalc and set botpos as fee calculated.
                //2018-05-23 also perform calc when "rotate pos" situation (bFoundHistNotFeeCalced)
                //2018-06-20 try to remove bDealWithNoFee and IsFeeLateCalced condition becaused
                //fee COULD BE zero in a "very little" amount case. Note: as we check "bpClosed!=null"
                //we calcuylate and update total position fee after position close.
                if (bpClosed!=null)
                    if (/*bpClosed.IsFeeLateCalced==0 &&*/ (bFoundInPosLog || bFoundHistNotFeeCalced)
                        /*&& !bDealWithNoFee*/)
                    {

                        Log("Update fee of historical position");
                        if (bpClosed.ListOpeningPosChanges != null) //2018-07-19 protect against null data (when loaded from DB to poslog)
                            bpClosed.ListOpeningPosChanges.ForEach(el =>
                            {
                                calcedFeeForPos += el.Fee;
                                feeStockForPos += el.Fee_Stock;
                                feeDealingForPos += el.FeeDealing;

                            }
                        );

                        if (bpClosed.ListClosingPosChanges != null) //2018-07-19  protect against null data (when loaded from DB to poslog)
                            bpClosed.ListClosingPosChanges.ForEach(el =>
                            {
                                calcedFeeForPos += el.Fee;
                                feeStockForPos += el.Fee_Stock;
                                feeDealingForPos += el.FeeDealing;

                            }
                        );

                        if (calcedFeeForPos != 0) //2018-07-19 protect against owerride fee (if null we do not need)
                        {
                            bpClosed.Fee_Stock = feeStockForPos; ; //fee recieved from stock
                            bpClosed.FeeDealing = feeDealingForPos; //fee by dealing
                            bpClosed.Fee = calcedFeeForPos;//  2018-07-19                                         
                            bpClosed.Fee_Total = bpClosed.Fee; //is equal fee

                            bpClosed.VMClosed_RUB_stock = bpClosed.VMClosed_RUB_clean - bpClosed.Fee_Stock;//is equal VM on stock exchange (for checking), substract only stock fee
                            bpClosed.VMClosed_RUB = bpClosed.VMClosed_RUB_clean - bpClosed.Fee;//resulting VM substract full fee
                            bpClosed.VMClosed_RUB_user = bpClosed.VMClosed_RUB; //is equal VMClosed_RUB

                            bpClosed.IsFeeLateCalced = 1;

                            _client.UpdateUserPosLogLate(new CDBUpdateLate
                            {
                                Instrument = instrument,
                                ReplId = bpClosed.ReplIdClosed,
                                BotId = botId,
                                Fee_Total = bpClosed.Fee_Total,
                                Fee = bpClosed.Fee,
                                Fee_Stock = bpClosed.Fee_Stock,
                                FeeDealing = bpClosed.FeeDealing,
                                VMClosed_RUB = bpClosed.VMClosed_RUB,
                                VMClosed_RUB_user = bpClosed.VMClosed_RUB_user,
                                VMClosed_RUB_stock = bpClosed.VMClosed_RUB_stock,
                                IsFeeLateCalced = bpClosed.IsFeeLateCalced
                            });

                        }
                       


                    }

                //===================== DEBUGGING START
                //2018-06-03 this is for debugging
                //to catch bpClosed==null situation
                if (bFoundInPosLog)
                    if (bpClosed == null || bNullAnomalyFound)
                    {
                        Log("bpClosed == null catching situation");
                        if (DicBotPosLog.ContainsKey(botId))
                        {
                            if (DicBotPosLog[botId].ContainsKey(instrument))
                            {
                                if (DicBotPosLog[botId][instrument].Count != 0)
                                {
                                    var lastEl = DicBotPosLog[botId][instrument].Last();
                                    if (lastEl == null)
                                        Log("Last el is null");

                                    foreach (var elBp in DicBotPosLog[botId][instrument])
                                        Log(String.Format("DtOpen={0} DtClosed={1} BuySell={2} " +
                                                            "CloseAmount={3} Priceopen={4} Priceopen={4} PriceClose={5}",
                                                            elBp.DtOpen,
                                                            elBp.DtClose,
                                                            elBp.BuySell,
                                                            elBp.CloseAmount,
                                                            elBp.PriceOpen,
                                                            elBp.PriceClose));
                                       
                                    

                                }
                                else
                                    Log("Count is 0");
                                 
                            }
                            else
                            {
                                Log("Doesn't contain instrument");
                            }

                        }
                        else
                            Log("Doesn't contain botId");

                       


                    }
                //DEBUGGING END


            } //end lock (DicBotPosLog)


                                 

            _client.TriggerRecalculateBot(botId, "", EnmBotEventCode.OnForceUpdTotalVM, null);

            _client.UpdateFeeUserDealsLog(new CDBUpdateFeeUserDealsLog
            {
                DealId = dealId,
                Fee = calcedFee,                               
                FeeDealing = feeDealing,
                FeeStock = feeStock
                

            });

           //

        }



        /// <summary>
        /// Extends base Method case on Bfx could be wrong deals sequence
        /// </summary>
        /// <param name="rd"></param>
        /// <param name="dataWithTime"></param>
        /// <returns></returns>
        protected override bool IsNeedUpdateUserDealsLog(CRawUserDeal rd, Dictionary<int, Dictionary<string, CLatestTradeData>> dataWithTime)
        {

            //first check using "classical" StockExchAlgo - replId or time is  older than  last processed
            if (base.IsNeedUpdateUserDealsLog(rd, dataWithTime))
            {
                
                return true;
            }
            //else check if Deal with this Id was not processed
            else
            {

                Log("IsNeedUpdateUserDealsLog. Invalid sequence check.");

                int userId=0;

                int iExt_id_buy = (int)rd.Ext_id_buy;
                int iExt_id_sell = (int)rd.Ext_id_sell;

                if (dataWithTime.ContainsKey(iExt_id_buy) && iExt_id_buy != 0)
                    userId = iExt_id_buy;  //continue check

                else if (dataWithTime.ContainsKey(iExt_id_sell) && iExt_id_sell != 0)
                    userId = iExt_id_sell; //continue check

                if (userId > 0)
                {
                    Log("IsNeedUpdateUserDealsLog.Invalid sequence check. DealID="+rd.Id_Deal);

                    if (!IsContainDealId(userId, rd.Id_Deal))
                    {
                        Log("IsNeedUpdateUserDealsLog. Invalid sequence check. DealId not exist need update");
                        return true;
                    }
                    else
                    {
                        Log("IsNeedUpdateUserDealsLog. Invalid sequence check. DealId already exists");
                        return false;
                    }
                }
                else
                {
                    string msg = "IsNeedUpdateUserDealsLog. Invalid sequence check. No userDeald";
                    Log(msg);
                    Error(msg);
                    return false;
                }


            }




           
        }


     







    }
}
