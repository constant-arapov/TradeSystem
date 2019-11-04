using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib;
using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Clients;
using TradingLib.Abstract;
using TradingLib.Common.VMCalc;
using TradingLib.Data;
using TradingLib.Enums;
using TradingLib.Data.DB;
using TradingLib.BotEvents;


using ASTS.Common;

namespace ASTS.DealingServer
{
    public class CUserDealsPosBoxASTS : CBaseUserDealsPosBox
    {

        private COnlineDetector _onlineDetector;


        public CUserDealsPosBoxASTS(IClientUserDealsPosBox client)
            : base(client, CBaseVMCalc.CreateMOEXVMCalc(),
                    bBuildNonSavedPositionsFromDealsLog: true)
        {

            _onlineDetector = new COnlineDetector(client.TriggerRecalcAllBots, EnmBotEventCode.OnUserDealOnline,
                                                    parTimeAfterUpdateMs: 500, parTimeAfterObjectCreated: 10000);

        }

        public void Update(string instrument, decimal price, char buySell, 
                            int amount, long extId, long tradeNo, string tradeDate,
                            string  tradeTime, long micosecs, decimal fee)
        {
           //CBotPos botPos = new CBotPos { Instrument = instrument,
            _onlineDetector.Update();

            DateTime moment = CASTSConv.ASTSDateAndTimeToDateAndTime(tradeDate, tradeTime);
            int milisec = Convert.ToInt16( (double) micosecs / 1000);
            moment =  moment.AddMilliseconds(milisec);

            CRawUserDeal rd = new CRawUserDeal
            {
                Instrument = instrument,
                Amount = amount,
                Dir = (sbyte)(buySell == 'B' ? OrderDirection.Buy : OrderDirection.Sell),
                Ext_id_buy = buySell == 'B' ? extId : 0,
                Ext_id_sell = buySell == 'S' ? extId : 0,
                Price = price,
                ReplId =  tradeNo,
                Moment = moment,
                Fee_buy = buySell == 'B' ? fee : 0,
                Fee_sell = buySell == 'S' ? fee : 0
                
                
            };
           
            //TODO chek if we already processed deal

            CalculateBotsPos(rd,isOnlineASTSCalc:true);

            UserDealsPosBoxClient.UpdateGUIDealCollection(rd);

          
           
        }



       



       

    }
}
