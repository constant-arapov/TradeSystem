using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;
using TradingLib.ProtoTradingStructs;


using Plaza2Connector;
//using Plaza2Connector.Interfaces;

using System.Diagnostics;

using DBCommunicator;

//using DBCommunicator.DBData;
using TradingLib;
using TradingLib.Enums;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Data;
using TradingLib.Data.DB;
using TradingLib.BotEvents;
using TradingLib.Bots;

using NUnit.Framework;


namespace zTest
{
    
    public class MockUserDealsPosBoxClient : IClientUserDealsPosBox, IAlarmable
    {


        public decimal USDRate { set; get; }

        public string Ticker { get; set; }//  = "RTS-6.16";
        public int MinSteps { get; set; }
        public decimal StepPrice { get;set; }
        int _botId = 100;
        public decimal Tol{get;set;}
        public int StockExchId { get; set; }

        CUserDealsPosBoxP2 _userDealPosBox;

        public  decimal Bid { get; set; }
        public decimal Ask {get; set;}

        public decimal BrokerFeeCoef { get; set; }
        public decimal InternalFeeCoef { get; set; }

        public bool IsDealsPosLogLoadedFromDB { get; set; }

		public bool IsOnlineUserDeals { get; set; }
        public bool IsStockOnline { get; set; }

        public IDBCommunicator DBCommunicator { get; set; }

        public MockUserDealsPosBoxClient(decimal brokerFeeCoef, decimal internalFeeCoef)
        {
            BrokerFeeCoef = brokerFeeCoef /100;
            InternalFeeCoef = internalFeeCoef /100;

            _userDealPosBox = new CUserDealsPosBoxP2(this);
            _userDealPosBox.AcconuntsFeeProc = new Dictionary<int, decimal> { { 100, 20 } };
            IsStockOnline = true;

        }


        public void UpdateDBPosLog(long accountTradeId, int stockExchId, string Instrument, 
                                     CBotPos botPos)
        {


        }

        public void TriggerRecalcAllBots(EnmBotEventCode evnt, object data)
        {


        }


        public void UpdateDBUserDealsLog(CDBUserDeal userDeal)
        {


        }

        public   bool IsInstrumentExist(string instrument)
        {
            //assume exist
            return true;
        }


        public long GetLotSize(string instrument)
        {
            return 1;

        }


        public CBotBase GetBotById(long id)
        {
            return null;
        }

        /*
        public StubUserDealsPosBox(string ticker, int minSteps, decimal stepPrice, decimal tol)
        {


            _ticker = ticker;
            _minSteps = minSteps;
            StepPrice = stepPrice;

            _tol = tol;

            //DoTest();

           

             //botPos.Amount;
            //userDealPosBox.DicBotPosLog[100]["RTS-6.16"][0].VMClosed_Points;



         //   userDealPosBox.DicBotPosLog[

        }
         */
        private bool IsInRange(decimal idealValue, decimal comparedValue/*, decimal tol = 0.01M*/ )
        {
            if (comparedValue >=  idealValue - Tol &&
                comparedValue <= idealValue + Tol)

            return true;

            return false;
        }


        [Test]
        public void DoTest(List<CRawUserDeal> lstRawDeal,
                           List<Tuple<int, CBotPos>> lstCtrlPointsOpened,
                           List<Tuple<int, CBotPos>> lstCtrlPointsClosed   
                         )
        {

                  
            for (int i = 0; i < lstRawDeal.Count; i++)
            {
              
                _userDealPosBox.CalculateBotsPos(lstRawDeal[i]);
               


                var resOpened = lstCtrlPointsOpened.FirstOrDefault(a => a.Item1 == i + 1);
                if (resOpened != null)
                {
                    CBotPos bp = _userDealPosBox.DictPositionsOfBots[_botId][Ticker];   
                    Assert.AreEqual(bp.Amount, resOpened.Item2.Amount);
                    Assert.AreEqual(bp.AvPos, resOpened.Item2.AvPos);
                    Assert.AreEqual(bp.VMCurrent_Steps, resOpened.Item2.VMCurrent_Steps);
                    Assert.AreEqual(bp.VMCurrent_Points, resOpened.Item2.VMCurrent_Points);
                }


                var resClosed = lstCtrlPointsClosed.FirstOrDefault(a => a.Item1 == i + 1);
                if (resClosed != null) 
                {
                    int cnt = _userDealPosBox.DicBotPosLog[_botId][Ticker].Count - 1;
                    CBotPos botPosClosed = _userDealPosBox.DicBotPosLog[_botId][Ticker][cnt];


                    Assert.AreEqual      (botPosClosed.CloseAmount,        resClosed.Item2.CloseAmount);
                    Assert.AreEqual      (botPosClosed.VMClosed_Points,    resClosed.Item2.VMClosed_Points);
                    Assert.That(IsInRange(botPosClosed.VMClosed_Steps,     resClosed.Item2.VMClosed_Steps));
                    Assert.That(IsInRange(botPosClosed.VMClosed_RUB_clean, resClosed.Item2.VMClosed_RUB_clean));
                    Assert.That(IsInRange(botPosClosed.VMClosed_RUB,       resClosed.Item2.VMClosed_RUB));

                }

            }

            

        }



       
        public void Error (string msg,Exception e=null)
        {
            throw e;
        
        }

        public void UpdateGUIDealCollection(CRawUserDeal rd) { }

        public bool IsOrderFromPrevSession(CRawUserDeal rd) 
        {
            return false;
        }
        public bool IsPossibleCalculateBotPos(CRawUserDeal rd)
        {
            return true;
        }

        public bool IsReadyRefreshBotPos() 
        {
            return true;
        }

        public void UpdateTradersPosLog(int extId) 
        {                        
        }




        public decimal GetBid(string ticker) 
        {
            //return 0;
            return Bid;
        }
        public decimal GetAsk(string ticker)
        {
            //return 0;
            //return _ask;
            return Ask;

        }


        public decimal GetStepPrice(string ticker)
        {

            //return 13.16974M;
            return StepPrice;
        }

        public decimal GetMinStep(string ticker)
        {
            //return 10M;
            return MinSteps;

        }

        public void TriggerRecalculateBot(int botId, string isin, EnmBotEventCode code, object data)
        {


        }


        public string GetTicker(long id)
        {

           // return "RTS-6.16";
            return Ticker;
        }


        public void GetFeeParams(out decimal brokerFeeCoef, out decimal internalFeeCoef)
        {
            //TODO get from config

            brokerFeeCoef = BrokerFeeCoef;
            internalFeeCoef = InternalFeeCoef;

        }


        public void GUIBotUpdateMonitorPos(CBotPos bp, string isin, int botId) { }


        public void GUIBotUpdatePosLog(CBotPos BotPos, int extId) { }


		public int GetDecimalVolume(string instrument)
		{
			return 0;
		}

        public void UpdDBPosInstr(int botId, string instrument, decimal amount, decimal avPos)
        {

        }
    }
}
