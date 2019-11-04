using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


using Common;
using Common.Interfaces;
using Common.Collections;
using Common.Logger;
using Common.Utils;

using TradingLib;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Interaction;
using TradingLib.Abstract;
using TradingLib.Common.VMCalc;
using TradingLib.Enums;
using TradingLib.Data;
using TradingLib.Data.DB;
using TradingLib.ProtoTradingStructs;
using TradingLib.BotEvents;
using TradingLib.Bots;

//using Plaza2Connector.Interfaces;
using DBCommunicator;




namespace Plaza2Connector
{
    public class CUserDealsPosBoxP2 : CBaseUserDealsPosBox,  IUserdealsPosBoxForP2Connector, IAlarmable
    {
       

        List<CRawUserDeal> m_listRawUserDeal = new List<CRawUserDeal>();
        public List<CRawUserDeal> ListRawUserDeal { get { return m_listRawUserDeal; } }
    



        Dictionary<string,  List<CRawUserDeal>> m_dictUserDeals =new  Dictionary<string, List<CRawUserDeal>>();
        
  
     
        CLogger m_logger = new CLogger("UserDealsBox");
        Dictionary<string, CCommonPosition> m_dictCommonPositions = new Dictionary<string, CCommonPosition>();
       

        
        public CUserDealsPosBoxP2(IClientUserDealsPosBox userDealsPosBoxClient) :
            base(userDealsPosBoxClient, CBaseVMCalc.CreateFORTSVmCalc(),
                 bBuildNonSavedPositionsFromDealsLog:false)                                                                
        {      
            m_logger.Log("==== UserDealsBox created ====");
			
        }


     
        public decimal CalcTotalBotVMClosed(int BotID,string isin)
        {
            decimal v = 0;

            //LckDictPositionsOfBots.WaitOne();
            lock (LckDictPositionsOfBots)
            {
                if (DicBotPosLog.ContainsKey(BotID))
                    foreach (KeyValuePair<string, List<CBotPos>> kv in DicBotPosLog[BotID])
                        foreach (CBotPos bp in kv.Value)
                            v += bp.VMClosed_RUB;


            }
            //LckDictPositionsOfBots.ReleaseMutex();

            return v;

        }

        //Update Last Trade Data (pos or deals). Set last receieved data

        private void UpdateLatestTradeData(string instrument,int userID,  
                                    Dictionary<int, Dictionary<string, CLatestTradeData>> dataWithTime, CLatestTradeData latestTradeData )
        {

           // string instrument = _userDealsPosBoxClient.GetTicker(rd.Isin_Id);

            if (!dataWithTime .ContainsKey(userID))
                dataWithTime[userID] = new Dictionary<string, CLatestTradeData>();
            if (!dataWithTime[userID].ContainsKey(instrument))
                dataWithTime[userID][instrument] = new CLatestTradeData();

                //dataWithTime[userID][instrument].Dt_timestamp_ms = rdTimeMili;



          
        }

      


      



        /// <summary>
        /// Entry point of CUserDealsPosBox
        /// 
        /// Call from Plaza2Connector\Plaza2Listener.ProcessUserDeal
        /// </summary>
        /// <param name="deal"></param>
        public  void Update(USER_DEAL.user_deal deal)
        {


            try
            {
                if (deal.nosystem != 0)
                    Error("No system user deal.");

           


                //drop already-processed deals (with the same id)
                foreach (CRawUserDeal rud in m_listRawUserDeal)
                {

                    if (rud.ReplId == deal.replID)
                    {

                        //mxListRawUserDeal.ReleaseMutex();
                        return;
                        //note: could be duplicate. Nothing to do with it.

                    }


                }

                string instrument = UserDealsPosBoxClient.GetTicker(deal.isin_id);


                m_listRawUserDeal.Add(new CRawUserDeal(deal,instrument));
                CalculateBotsPos(new CRawUserDeal(deal,instrument));
                UserDealsPosBoxClient.UpdateGUIDealCollection(new CRawUserDeal(deal,instrument));
             


            }
            catch (Exception e)
            {

                m_logger.Log("Error update " + e.Message + " " + e.StackTrace);
                Error("CUserDealsBox.Update", e);

            }
            finally
            {

              

            }
                 
        }


     
      
    }
}
