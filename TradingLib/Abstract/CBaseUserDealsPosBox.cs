using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Common.Interfaces;
using Common.Collections;

using Common.Utils;

using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;

using TradingLib.Common.VMCalc;
using TradingLib.Data;
using TradingLib.Data.DB;
using TradingLib.BotEvents;
using TradingLib.ProtoTradingStructs;
using TradingLib.Enums;


namespace TradingLib.Abstract
{
    public abstract class CBaseUserDealsPosBox  : IUserDealsPosBox
    {
        //TODO incapsulate
        public Mutex mxDictPositionRecord = new Mutex();
       

        /// <summary>
        /// Storage with position of all bots of all instruments
        /// </summary>
        public Dictionary<int, Dictionary<string, CBotPos>> DictPositionsOfBots { get; set; }

		
  
        public object LckDictPositionsOfBots { get; set; }// = new object();



        public Dictionary<int, decimal> AcconuntsFeeProc { get; set; }

        private IClientUserDealsPosBox _client;
        public IClientUserDealsPosBox UserDealsPosBoxClient
        {
            get
            {
                return _client;
            }


        }

        // Log with closed positions of bots
        public CDict_L2_List<int, string, CBotPos> DicBotPosLog { get; set; }
        public CDict_L2_List<int, string, CUserDeal> DictUserDealsLog { get; set; }

        private decimal _brokerFeeCoef; 
        public decimal BrokerFeeCoef
        {
            get
            {
                return _brokerFeeCoef;
            }
        }

        private decimal _internalFeeCoef;
        public decimal InternalFeeCoef
        {
            get
            {
                return _internalFeeCoef;
            }
        }


        public decimal USDRate
        {
            get
            {
                return _client.USDRate;
            }

        }


        private IAlarmable _alarmer;

        public IAlarmable Alarmer
        {
            get
            {
                return _alarmer;
            }


        }

             

        public decimal GetBid(string instrument)
        {
            return _client.GetBid(instrument);
        }


        public decimal GetAsk(string instrument)
        {
            return _client.GetAsk(instrument);
        }

        public long GetLotSize(string instrument)
        {

            return _client.GetLotSize(instrument);
        }

        public CBaseVMCalc VMCalc
        {
            get
            {
                return _vmCalc;
            }

        }


        /// <summary>
        /// Latest BotPos of each bot on each instrument. BotId - Instrument - LatestTradeData (Dt_timestamp_ms, ReplId)
        /// </summary>     
        public CDict_L2<int, string, CLatestTradeData> LatestBotPosData = new CDict_L2<int, string, CLatestTradeData>();

       
        /// <summary>
        /// Latest deal of each bot on each instrument. BotId - Instrument - LatestTradeData (Dt_timestamp_ms, ReplId)
        /// </summary>
        public CDict_L2<int, string, CLatestTradeData> LatestDealsData = new CDict_L2<int, string, CLatestTradeData>();


		private bool _bWasBuildNonSavedPositionsFromDealsLog = false;



        protected IDBCommunicator _dbCommunicator;

        protected CBaseVMCalc _vmCalc;
        protected bool _bBuildNonSavedPositionsFromDealsLog;
        

        public CBaseUserDealsPosBox(IClientUserDealsPosBox client, CBaseVMCalc vmCalc, bool bBuildNonSavedPositionsFromDealsLog)
        {

            _client = client;
            _vmCalc = vmCalc;
            //_alarmer = alarmer;
           


            _brokerFeeCoef = UserDealsPosBoxClient.BrokerFeeCoef;
            _internalFeeCoef = UserDealsPosBoxClient.InternalFeeCoef;


                DicBotPosLog = new CDict_L2_List<int, string, CBotPos>();
             DictUserDealsLog = new CDict_L2_List<int, string, CUserDeal>();
             LckDictPositionsOfBots = new object();
             DictPositionsOfBots = new Dictionary<int, Dictionary<string, CBotPos>>();
             
            _dbCommunicator = UserDealsPosBoxClient.DBCommunicator;
            AcconuntsFeeProc = new Dictionary<int, decimal>();

            _bBuildNonSavedPositionsFromDealsLog = bBuildNonSavedPositionsFromDealsLog;
        }


		public int GetDecimalVolume(string instrument)
		{
			return _client.GetDecimalVolume(instrument);
		}
        /// <summary>
        /// Checks if RawDeal is later than Open posittion.
        /// Disabled 2017-10-19
        /// </summary>
        /// <param name="botID"></param>
        /// <param name="instrument"></param>
        /// <param name="rdTimeMili">Time in millisecs of raw deal</param>
        /// <returns></returns>
        private bool IsRawDealLaterThenOpenedPos(int botID, string instrument, long rdTimeMili)        
        {
            lock (LckDictPositionsOfBots)
            {
                CBotPos bp = GetBotPos(botID, instrument);

				

                //no opened pos rd is later
                if (bp.Amount == 0)
                    return true;


                long botposTimeMili = CUtilTime.GetUnixTimestampMillis(bp.DtOpen);

                if (rdTimeMili > botposTimeMili)
                    return true;



                return false;

            }
            
        }



        protected  bool IsNeedProcessPos(CRawUserDeal rd, 
										Dictionary<int, Dictionary<string, CLatestTradeData>> dictLatestBotPos,
										bool isASTSRecieveFromStock)
        {

            int iExt_id_buy = (int)rd.Ext_id_buy;
            int iExt_id_sell = (int)rd.Ext_id_sell;


            if (iExt_id_buy == 0 && iExt_id_sell == 0)
            {
                Error("Non user deal ! replId=" + rd.ReplId);
                return false; ;

            }



            long rdTimeMili = CUtilTime.GetUnixTimestampMillis(rd.Moment);

            int userId;

            if (dictLatestBotPos.ContainsKey(iExt_id_buy) && iExt_id_buy != 0)
                userId = iExt_id_buy;  //continue check

            else if (dictLatestBotPos.ContainsKey(iExt_id_sell) && iExt_id_sell != 0)
                userId = iExt_id_sell; //continue check

            else
                return true;   //out


            //string instrument = UserDealsPosBoxClient.GetTicker(rd.Isin_Id);
            string instrument = rd.Instrument;

            if (!dictLatestBotPos[userId].ContainsKey(instrument))
                return true;

            bool isNewerLastTimestamp = rdTimeMili > dictLatestBotPos[userId][instrument].Dt_timestamp_ms;
            bool equalTimeStampButGtaterId =  rdTimeMili == dictLatestBotPos[userId][instrument].Dt_timestamp_ms &&
                                               rd.ReplId > dictLatestBotPos[userId][instrument].ReplId &&
                                               dictLatestBotPos[userId][instrument].ReplId != 0;

            bool isLaterThanOpenedPos = IsRawDealLaterThenOpenedPos(userId, instrument, rdTimeMili);
			
			//bool isSpecASTSConditionPast = (isASTSRecieveFromStock && !isLaterThanOpenedPos && !_client.IsOnlineUserDeals) ? false : true;

            //Only if ASTS and if not online processing (not from DB) we NOT passed condition.
            //because for ASTS we loading data from Database
			bool isSpecASTSConditionPast=true;
			if (isASTSRecieveFromStock)
			{
				if (!_client.IsOnlineUserDeals)
					//if (isLaterThanOpenedPos)
					isSpecASTSConditionPast = false;
				

			}


            if ((isNewerLastTimestamp ||equalTimeStampButGtaterId)
				&& isSpecASTSConditionPast)                
                return true;

            //Modification of 2017-10-19. Remove isLaterTanOpenedPos condition.
            //Reason: when opening position and than adding position on exact have
            //time algorithm doesn't work
            

            return false;
        }      



        protected virtual bool IsNeedUpdateUserDealsLog(CRawUserDeal rd, Dictionary<int, Dictionary<string, CLatestTradeData>> dataWithTime)
        {

            int iExt_id_buy = (int)rd.Ext_id_buy;
            int iExt_id_sell = (int)rd.Ext_id_sell;

            if (iExt_id_buy == 0 && iExt_id_sell == 0)
            {
                Error("Non user deal ! replId=" + rd.ReplId);
                return false; ;

            }




            long rdTimeMili = CUtilTime.GetUnixTimestampMillis(rd.Moment);

            int userId;

            if (dataWithTime.ContainsKey(iExt_id_buy) && iExt_id_buy != 0)
                userId = iExt_id_buy;  //continue check

            else if (dataWithTime.ContainsKey(iExt_id_sell) && iExt_id_sell != 0)
                userId = iExt_id_sell; //continue check

            else


                return true;   //out


            string instrument = rd.Instrument;//UserDealsPosBoxClient.  GetTicker(rd.Isin_Id);

            if (!dataWithTime[userId].ContainsKey(instrument))
                return true;





            if ((rdTimeMili > dataWithTime[userId][instrument].Dt_timestamp_ms)
                 ||
                (rdTimeMili == dataWithTime[userId][instrument].Dt_timestamp_ms
                && rd.ReplId > dataWithTime[userId][instrument].ReplId)

                )


                return true;



            return false;
        }


        public bool IsContainDealId(int botId, long dealID)
        {

            lock(DictUserDealsLog)
            {

                if (!DictUserDealsLog.ContainsKey(botId))
                    return false;

                foreach (var kvp in DictUserDealsLog[botId])
                {
                   foreach (var deal in kvp.Value)
                    {
                        if (deal.DealId == dealID)
                            return true;
                    }

                }




            }



            return false;
        }







        /// <summary>
        /// Check if it's must be opened (or already closed) positions based on 
        /// deals. ASTS does not send user deals of previous session.
        /// So we get deals from database and calculate positions. 
        /// 
        /// </summary>
        private void BuildNonSavedPositionsFromDealsLog()
        {

            foreach (var kvp1 in DictUserDealsLog)
            {

                int botId = kvp1.Key;
                foreach (var kvp2 in kvp1.Value)
                {
                    string instrument = kvp2.Key;
                    foreach (var userDeal in kvp2.Value)
                    {

						




                        CRawUserDeal rd = new CRawUserDeal
                        {
                              Instrument = instrument,
                              Amount = userDeal.Amount,                             
                              Dir = (sbyte)(userDeal.BuySell == EnmDealDir.Buy ? OrderDirection.Buy : OrderDirection.Sell),
                              Ext_id_buy = userDeal.BuySell == EnmDealDir.Buy ? botId : 0,
                              Ext_id_sell = userDeal.BuySell == EnmDealDir.Sell ? botId : 0,
                              Price = userDeal.Price,
                              ReplId = userDeal.ReplId,
                              Moment = userDeal.Moment,
                              Fee_buy = userDeal.BuySell == EnmDealDir.Buy ? userDeal.Fee : 0,
                              Fee_sell = userDeal.BuySell == EnmDealDir.Sell ? userDeal.Fee : 0,
                              Id_Deal = userDeal.DealId, //2018-06-13
                              Fee_Dealing = userDeal.FeeDealing, 
                              FeeStock = userDeal.Fee_Stock //2018-08-05





                        };

                        CalculateBotsPos(rd);
						

                    }
                }
            }

        }


        /// <summary>
        /// Loads deals and poslog of current session which are not clearing processed
        /// yet. Also build (if need)  current positions from not clearing processed deals.
        /// 
        /// Call from CBaseDealingServer.LoadDataFromDB()
        /// </summary>
        public void LoadTradeData()
        {
            
            //Load Last trade data  - positions and deals  
            LoadLatestDataFromDB("get_latest_poslog_data", "DtClose_timestamp_ms", LatestBotPosData);
            LoadLatestDataFromDB("get_latest_userdealslog_data", "Moment_timestamp_ms", LatestDealsData);

         
            //load positions and deals of current session (not clearing processed yet)
            _dbCommunicator.LoadUserPosLogData(DicBotPosLog, _client.StockExchId);
            _dbCommunicator.LoadUserDealsLogData(DictUserDealsLog, _client.StockExchId);
            
            // added 2017-07-05
			//note: do ONLY for ASTS
			//update 2017-10-21
			//must be executed only one time
			if (_bBuildNonSavedPositionsFromDealsLog && !_bWasBuildNonSavedPositionsFromDealsLog)
			{
				_bWasBuildNonSavedPositionsFromDealsLog = true;

				while (!((CBaseDealingServer)_client).IsAllBotLoaded)
					Thread.Sleep(100);

				BuildNonSavedPositionsFromDealsLog();
				//wait till bot data recalculated
				
			}


            _client.IsDealsPosLogLoadedFromDB = true;
        }


        private void LoadLatestDataFromDB(string storedProcedure, string dtColumn, CDict_L2<int, string, CLatestTradeData> LatestData)
        {

            LoadLatestTradeData(_dbCommunicator.LoadLatestUserTradeData(storedProcedure, _client.StockExchId),
                                                   dtColumn, LatestData);
        }


     

        /// <summary>
        ///  Calculate bot position when new deal was recieved  from server.
        ///       
        ///  Call from:
        ///  UserDealsPosBox.Update
        /// 
        /// </summary>
        /// <param name="rd"></param>
		/// <param name="isOnlineASTSCalc">Special case - only for ASTS and for online</param>
        public virtual void CalculateBotsPos(CRawUserDeal rd, bool isOnlineASTSCalc = false)
        {



            lock (LckDictPositionsOfBots)
            {
                //TODO: check and remove mutex
              //  mxDictPositionRecord.WaitOne();
                try
                {

                    string instrument = rd.Instrument; //UserDealsPosBoxClient.GetTicker(rd.Isin_Id);

                
                    //if deals was already processed  (not seen yet) - move to Deals Log
                    if (IsNeedUpdateUserDealsLog(rd, LatestDealsData))
                        UpdateDealsData(instrument, rd);



                    //if data was already processed return
					if (!IsNeedProcessPos(rd, LatestBotPosData, isOnlineASTSCalc))
                        return;




                    int extID = (int)((rd.Ext_id_buy != 0) ? rd.Ext_id_buy : rd.Ext_id_sell);


                    CBotPos BotPos = GetBotPos(extID, instrument);


                    BotPos.OldAmount = BotPos.Amount;

                    //increase or decrease position
                    //amount depend on direction
                    //position amount has a sign
                    if (rd.Ext_id_buy != 0)
                        BotPos.Amount += rd.Amount;
                    else if (rd.Ext_id_sell != 0)
                        BotPos.Amount -= rd.Amount;



                    //  1)New position opened
                    //  2)Deal has the same dir as already opened pos - increase pos                
                    if ((BotPos.OldAmount >= 0 && rd.Ext_id_buy != 0) || (BotPos.OldAmount <= 0 && rd.Ext_id_sell != 0))
                    {
                     

                        //new position was opened
                        if (BotPos.OldAmount == 0)
                            BotPos.OnOpenNewPos(rd);
                        
                        
                        //2018-06-07 was before OnOpenNewPos
                        BotPos.AddOpenedCostHist(rd.Id_Deal, rd.Price, rd.Amount, 
                                                    rd.Fee_buy + rd.Fee_sell, rd.Fee_Dealing, rd.FeeStock,
                                                  (EnmDealDir) rd.Dir, rd.Moment, rd.ReplId);


                        BotPos.CalcCurrentPos();
                        
                        //UpdateLatestBotPos(extID, instrument, BotPos);

                    }
                    //deal has another dir than opened pos
                    else if ((BotPos.OldAmount >= 0 && rd.Ext_id_sell != 0) || (BotPos.OldAmount <= 0 && rd.Ext_id_buy != 0))
                    {
                        //new deal has another direction and amount less (or equal) than current position                     
                        //if we fully close or partial close no matter - 
                        //using Close method calculation of profit 
                        if (Math.Abs(rd.Amount) <= Math.Abs(BotPos.OldAmount))
                        {

                            BotPos.AddClosedCostHist(rd.Id_Deal,rd.Price, Math.Abs(rd.Amount), 
                                                        rd.Fee_buy + rd.Fee_sell, rd.Fee_Dealing, rd.FeeStock,
                                                        (EnmDealDir) rd.Dir, rd.Moment, rd.ReplId);


                            BotPos.Close(rd.Moment, rd.ReplId);



                            //Amount after a new deal is empty. Position was fully closed.
                            //The simpliest case
                            if (BotPos.Amount == 0)
                            {
                                //BotPos.CloseReduse();
                                UpdateLatestBotPos(extID, instrument, BotPos);
                                UpdateUserPosLog(extID, instrument, BotPos, rd);

                                BotPos = new CBotPos(instrument, this, AcconuntsFeeProc[extID]);


                            }

                        }
                        //new deal has another direction and amount more than current position
                        //so, we need revert position
                        //first close previous direction pos than open another dir pos
                        //with remain
                        else if ((Math.Abs(rd.Amount) > Math.Abs(BotPos.OldAmount)))
                        {
                            //note: using old amount for fully close
                            BotPos.AddClosedCostHist(rd.Id_Deal,rd.Price, Math.Abs(BotPos.OldAmount), 
                                                        rd.Fee_buy + rd.Fee_sell, rd.Fee_Dealing, rd.FeeStock,
                                                        (EnmDealDir) rd.Dir,
                                                       rd.Moment, rd.ReplId);

                            decimal amountRamained = BotPos.Mult * (Math.Abs(BotPos.OldAmount) - Math.Abs(rd.Amount));

                            BotPos.Close(rd.Moment, rd.ReplId);

                            UpdateLatestBotPos(extID, instrument, BotPos);
                            UpdateUserPosLog(extID, instrument, BotPos, rd);

                            BotPos = new CBotPos(instrument, this, AcconuntsFeeProc[extID]);
                            BotPos.Amount = amountRamained;
                            

                            BotPos.OnOpenNewPos(rd);

                            //2018-06-07 was before OnOpenNewPos
                            BotPos.AddOpenedCostHist(rd.Id_Deal, rd.Price, Math.Abs(amountRamained), 
                                                        rd.Fee_buy + rd.Fee_sell, rd.Fee_Dealing, rd.FeeStock,
                                                        (EnmDealDir)rd.Dir, rd.Moment,
                                                        rd.ReplId);

                            BotPos.CalcCurrentPos();

                        }


                    }
                    //Update positions of all bots of all instruments
                    DictPositionsOfBots[extID][instrument] = BotPos;

                    UserDealsPosBoxClient.TriggerRecalculateBot(extID, instrument, EnmBotEventCode.OnUserDeal, (CBotPos)BotPos.Copy());

                    _client.UpdDBPosInstr(extID, rd.Instrument, BotPos.Amount, BotPos.AvPos);
                }
                catch (Exception e)
                {

                    Error("CuserDealsBox.CalculateBotsPos", e);

                }

            //   mxDictPositionRecord.ReleaseMutex();
            }//end of  lock (LckDictPositionsOfBots)

            // LckDictPositionsOfBots.ReleaseMutex();

        }





        //Remembering last data that was retrieved from DB
        public void LoadLatestTradeData(List<Dictionary<string, object>> queryRes, string dtColName,
                                            CDict_L2<int, string, CLatestTradeData> data)
        {

            if (queryRes == null)
                return;

            foreach (var cols in queryRes)
            {
                int user_id = (int)cols["account_trade_Id"];
                string instrument = (string)cols["Instrument"];




                data.Update(user_id, instrument,
                new CLatestTradeData
                {
                    Dt_timestamp_ms = (long)cols["max_time"],
                    ReplId = (!(cols["max_repl"] is DBNull)) ? (long)cols["max_repl"] : 0
                  
                }

                              );

            }


        }


        /// <summary>
        /// 1)Triggers recalculation of bot positions
        /// 2)Trigger update bot positions on GUI
        /// 
        /// Call when bid and ask ins stock changed
        /// 
        /// Call from:
        /// 
        /// Plaza2Connector\CStockConverterP2.ThreadStockConverter
        /// ASTS\CStockCOnverterASTS.ProcessConvert
        /// </summary>
        /// <param name="instrument"></param>
        public void RefreshBotPos(string instrument)
        {
            if (!UserDealsPosBoxClient.IsReadyRefreshBotPos())
                return;

            lock (LckDictPositionsOfBots)
            {
                try
                {
             

                    foreach (KeyValuePair<int, Dictionary<string, CBotPos>> kvp in DictPositionsOfBots)
                    {
                        int botId = kvp.Key;

                        if (kvp.Value.ContainsKey(instrument))
                        {
                            CBotPos bp = kvp.Value[instrument];

                            bp.CalcCurrentPos();
                            UserDealsPosBoxClient.GUIBotUpdateMonitorPos(bp, instrument, botId);
                        }

                    }
                }

                catch (Exception e)
                {
                    Error("RefreshBotPos",e);

                }
            }


        }

        public void Error(string description, Exception exception = null)
        {

            //  Plaza2Connector.Error(description,  exception );
           // if (_alarmer != null)
             //   _alarmer.Error(description, exception);
            _client.Error(description, exception);

        }


        public void CleanUserPosLog()
        {


            foreach (var kvp in DicBotPosLog)
                kvp.Value.Clear();

            foreach (var kvp in DicBotPosLog)
                UserDealsPosBoxClient.UpdateTradersPosLog(kvp.Key);



        }



        /// <summary>  

        /// Calculate total VM. vm_total = vm_opened + vm_closed   
        /// </summary>
        /// <param name="BotId"></param>
        /// <param name="isin"></param>
        /// <param name="vmClosed">closed VM</param>
        /// <param name="bFound">was found in DicBotPosLog (closed posirions dictionary)</param>
        /// <returns> opened and closed VM</returns>
        /// 
        /// Was modified 2018.12.20 added vm opened and refactored
        /// 
        public decimal CalcBotVMOpenedAndClosed(int BotId, string isin, ref decimal vmClosed, ref bool bFoundClosed, 
                                                    ref decimal vmOpenedPos)
        {
            decimal vmTotal = 0;
            
           
            //  LckDictPositionsOfBots.WaitOne();
            lock (LckDictPositionsOfBots)
            {
                
                if (DicBotPosLog.ContainsKey(BotId) && DicBotPosLog[BotId].ContainsKey(isin))
                {
                    var ListBtorPos = DicBotPosLog[BotId][isin];
                    //foreach (KeyValuePair<string, List<CBotPos>> kv in DicBotPosLog[BotID])
                    foreach (CBotPos bp in ListBtorPos)
                    {
                        vmTotal += bp.VMClosed_RUB;
                        bFoundClosed = true;
                    }

                    vmClosed = vmTotal;
                   
                }
              
            }


            

            
            //2017-11-09
            //if no instrument in "Instruments"- that means must be no opened pos of this instrument
             if (_client.IsInstrumentExist(isin))
             {

                //2018-01-11 Now do not use getting full copy of BotPos which 
                // is significally reduce perfomance. Use speciel method GetVMCurrent_RUB
                // which gets just VMCurrent_RUB value from BotPos storage.

                // decimal openedVm =  (GetBotPos(BotId, isin)).VMCurrent_RUB;
                 decimal openedVm =  GetVMCurrent_RUB(BotId, isin);
                vmTotal += openedVm;
                vmOpenedPos += openedVm;




             }

         

                      
            // LckDictPositionsOfBots.ReleaseMutex();
            //tempo debug

          //  if (BotId == 101 && isin =="Si-3.18")
              //  Thread.Sleep(0);


            return vmTotal;


        }

        //May be no need an will remove. NOt used at the moment
        public decimal CalcBotVMClosed(int BotId, string isin, ref decimal vmClosed, ref bool bFound)
        {

            decimal v = 0;

            //  LckDictPositionsOfBots.WaitOne();
            lock (LckDictPositionsOfBots)
            {
                if (DicBotPosLog.ContainsKey(BotId) && DicBotPosLog[BotId].ContainsKey(isin))
                {
                    var ListBtorPos = DicBotPosLog[BotId][isin];
                    //foreach (KeyValuePair<string, List<CBotPos>> kv in DicBotPosLog[BotID])
                    foreach (CBotPos bp in ListBtorPos)
                    {
                        v += bp.VMClosed_RUB;
                        bFound = true;
                    }

                    vmClosed = v;

                




                }
            }

            return v;

        }



       /// <summary>
       /// Retrieve botpos from storage
       /// if need create it (lazy creation)
       /// 
       /// Call from 
	   /// 1) CalculateBotsPos
	   /// 2) IsRawDealLaterThenOpenedPos
       /// </summary>
       /// <param name="botID"></param>
       /// <param name="isin"></param>
       /// <returns></returns>
        public  CBotPos GetBotPos(int botID, string isin)
        {

           
            //LckDictPositionsOfBots.WaitOne();
            lock (LckDictPositionsOfBots)
            {
                if ((!DictPositionsOfBots.ContainsKey(botID)))
                    DictPositionsOfBots[botID] = new Dictionary<string, CBotPos>();

              //  try
                {
                    if (!DictPositionsOfBots[botID].ContainsKey(isin))
                        DictPositionsOfBots[botID][isin] = new CBotPos(isin, this,
                                                                       AcconuntsFeeProc[botID]);
                }
               // catch (Exception e)
                {
                 //   Thread.Sleep(0);
                }

               CBotPos bp = (CBotPos)DictPositionsOfBots[botID][isin].Copy();
               // CBotPos bp = DictPositionsOfBots[botID][isin];
                //tempo remove !!!!!
               
              

                return bp;
            }

        }

		/// <summary>
		


        public decimal GetVMCurrent_RUB(int botId, string instrument)
        {
            lock (LckDictPositionsOfBots)
            {
                if ((!DictPositionsOfBots.ContainsKey(botId)))
                    return 0;

                if (!DictPositionsOfBots[botId].ContainsKey(instrument))
                    return 0;

                return DictPositionsOfBots[botId][instrument].VMCurrent_RUB;   
            }

        }




        /// <summary>
        /// Updates historical log of user positions
        /// 
        /// Trigger update for GUI, Database and
        ///  client classB (which transmit to traderdipatcher, and
        ///  then to clients)
        /// Call from UserDealsPosBox
        /// 
        /// this.CalculateBotsPos
        /// 
        /// </summary>
        /// <param name="botId"></param>
        /// <param name="instrument"></param>
        /// <param name="bp"></param>
        /// <param name="rd"></param>
        public  void UpdateUserPosLog(int botId, string instrument, CBotPos bp, CRawUserDeal rd)
        {

            try
            {

                CBotPos botPos = (CBotPos)bp.Copy();

                DicBotPosLog.Update(botId, instrument, botPos);



                UserDealsPosBoxClient.GUIBotUpdatePosLog(botPos, botId);
                UserDealsPosBoxClient.UpdateTradersPosLog(botId);



              //  UpdateLatestBotPos(botId, instrument, bp);




                UserDealsPosBoxClient.UpdateDBPosLog(botId, 
                                                     UserDealsPosBoxClient.StockExchId, 
                                                     instrument, 
                                                     (CBotPos)bp.Copy());




            }
            catch (Exception e)
            {

                Error("UpdateUserPosLog(", e);

            }

        }


        /// <summary>
        /// Updates latest botpos
        /// </summary>
        /// <param name="botId"></param>
        /// <param name="instrument"></param>
        /// <param name="botPos"></param>
        private void UpdateLatestBotPos(int botId, string instrument,CBotPos botPos)
        {
            try
            {
                long timeMili = CUtilTime.GetUnixTimestampMillis(botPos.DtClose);


                LatestBotPosData.Update(botId, instrument,
                                                 new CLatestTradeData
                                                 {
                                                     Dt_timestamp_ms = timeMili,
                                                     ReplId = botPos.ReplIdClosed
                                                 }
                                          );


            }
            catch (Exception e)
            {

            }
        }



        /// <summary>
        /// Update deal's history log
        /// </summary>
        /// <param name="instrument"></param>
        /// <param name="rd"></param>                         
        private void UpdateDealsData(string instrument, CRawUserDeal rd)
        {

            CUserDeal userDeal = null;
            long timeMili = 0;

            CUserDeal dealWithSameId = null;

            int botId = (int)(rd.Ext_id_buy > 0 ? rd.Ext_id_buy : rd.Ext_id_sell);

            lock (DictUserDealsLog)
            {

                if (!DictUserDealsLog.ContainsKey(botId))
                    DictUserDealsLog[botId] = new Dictionary<string, List<CUserDeal>>();

                if (!DictUserDealsLog[botId].ContainsKey(instrument))
                    DictUserDealsLog[botId][instrument] = new List<CUserDeal>();

                dealWithSameId = DictUserDealsLog[botId][instrument].FirstOrDefault(a => a.ReplId == rd.ReplId);
                
                //deal doesn't have the same id
                if (dealWithSameId == null)
                {

                    EnmDealDir dd = rd.Ext_id_buy > 0 ? EnmDealDir.Buy : EnmDealDir.Sell;

                    userDeal = new CUserDeal
                    {
                        ReplId = rd.ReplId,
                        Amount = rd.Amount,
                        Moment = rd.Moment,
                        Price = rd.Price,
                        BuySell = dd,
                        Fee = rd.Fee_buy + rd.Fee_sell,
                        DealId = rd.Id_Deal //2018-06-13



                    };

                    DictUserDealsLog[botId][instrument].Add((CUserDeal)userDeal);
                    // Plaza2Connector.TriggerRecalculateBot(botId, EnmBotEventCode.OnUserDeal, 



                    timeMili = CUtilTime.GetUnixTimestampMillis(userDeal.Moment);

                    LatestDealsData.Update(botId, instrument,
                                                new CLatestTradeData
                                                {
                                                    Dt_timestamp_ms = timeMili,
                                                    ReplId = rd.ReplId,
                                                    Dt = rd.Moment

                                                }

                                                );


                    CDBUserDeal dbUSerDeal = new CDBUserDeal();
                    CUtil.CopyObjProperties(userDeal, dbUSerDeal);
                    dbUSerDeal.Instrument = instrument;
                    dbUSerDeal.account_trade_Id = (int)botId;
                    dbUSerDeal.stock_exch_id = UserDealsPosBoxClient.StockExchId; //StockExchangeId;
                    dbUSerDeal.ReplId = rd.ReplId;
                    dbUSerDeal.DealId = rd.Id_Deal; //2018-05-31
                    dbUSerDeal.Fee = rd.Fee_buy + rd.Fee_sell; //2018-05-31


                    UserDealsPosBoxClient.UpdateDBUserDealsLog(dbUSerDeal);



                }
            }

        
        }


        //added 2018-05-31
        public bool IsAllPosClosed()
        {

            if (DictPositionsOfBots == null)
                return false;

            lock(LckDictPositionsOfBots)
            {
                foreach (var kvp in DictPositionsOfBots)
                {
                    foreach(var kvp2 in kvp.Value)
                    {
                        if (kvp2.Value!=null)
                        {
                            CBotPos bp = kvp2.Value;
                            if (bp.Amount != 0)
                                return false;

                        }

                    }

                }



            }

            return true;

        }


        public decimal GetSessionProfit()
        {
            if (DicBotPosLog == null)
                return 0;


            decimal sum = 0;

            lock (DicBotPosLog)
            {
                foreach (var kvp in DicBotPosLog)
                {
                    foreach (var kvp2 in kvp.Value)
                    {
                        foreach (var kvp3 in kvp2.Value)
                        {
                            sum += kvp3.VMClosed_RUB_stock;


                        }


                    }

                }

            }

           return sum;
        }

        public void BindDealBotPos(long dealId, long bpDtOpenTimestampMs)
        {
            _dbCommunicator.QueueData(
                new CDBBindDealBotPos
                {
                    DealId = dealId,
                    BP_DtOpen_timestamp_ms = bpDtOpenTimestampMs
                }

                );


        }

        public virtual void OnAddNewBot(int botId)
        {


        }














    }
     
}
