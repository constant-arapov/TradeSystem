using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


using Common.Interfaces;

using TradingLib.Data;
using TradingLib.Data.DB;
using TradingLib.BotEvents;

using TradingLib.Interfaces.Components;
using TradingLib.Abstract;
using TradingLib.GUI;

using ASTS.Interfaces.Clients;
using ASTS.Common;

namespace ASTS.DealingServer
{
    public class CPosistionsBoxASTS  : CBasePosBox,  IPositionBox
    {

      
        private CListInstruments _instruments;
        private IAlarmable _alarmer;
        private CGUIBox _guiBox;

      
     
        private IClientPositionsBoxASTS _client;
        private COnlineDetector _onlineDetector;

       

        public CPosistionsBoxASTS(IClientPositionsBoxASTS client)
                :base(client)
        {
            _client = client;
            _instruments = client.Instruments;
            _alarmer = client;
            _guiBox = client.GUIBox;

            //TODO to base class
            DictPos = new Dictionary<string, CRawPosition>();
            foreach (var kvp in _instruments)
                DictPos[kvp.instrument] = new CRawPosition();

            _onlineDetector = new COnlineDetector(client.TriggerRecalcAllBots, EnmBotEventCode.OnPositionOnline,
                                               parTimeAfterUpdateMs: 500, parTimeAfterObjectCreated: 8000);



            CreateListPos(_client.StockExchId, _instruments);
           

        }



        public void Update(string instrument, long plannedCovered, decimal debit, decimal credit)
        {

            _onlineDetector.Update();
            //need only instruments, not money
            //TODO: maybe something inteli in the future
            if (instrument == "SUR" || instrument == "RUB")            
                return;
            

            long lotSize = _instruments.GetLotSize(instrument);
          
            if (lotSize == 0)
            {
                Error("CPosistionsBoxASTS.Update lotsize==0");
                return;
         
            }

          


            DictPos[instrument].Pos  = (int) (plannedCovered / lotSize) ;
            DictPos[instrument].PosGUI = DictPos[instrument].Pos;

            //need for Supervisor, so deprecated
            
            /*
            CRawPosition rawPos = new CRawPosition();

            rawPos.Pos = DictPos[instrument].Pos;
            rawPos.PosGUI = DictPos[instrument].Pos;

            
            _guiBox.ExecuteWindowsUpdate(new Action 
                                            (
                                            () =>
                                                _guiBox.UpdatePos(instrument,rawPos)
                                                )
                                          );
             */


            UpdateTrdMgrTotalPos();
         


        }

        public void UpdateCurrencyPos(string description, 
                                      string currCode,
                                      string bankId,
                                      object currPos)
        {

            _onlineDetector.Update();
          
            //   string st = "USD/RUB_TOD - USD";
            // string st = description;



            if (bankId == _client.Account)
            {
                string pat = @"([\w]*)/([\w]*) - ([\w]*)";

                Regex reg = new Regex(pat);
                Match m = reg.Match(description);



                if (m.Groups.Count == 4)
                    if (m.Groups[1].Value == m.Groups[3].Value)
                        if (currCode == m.Groups[1].Value)
                        {

                            string shortName = m.Groups[1].Value + m.Groups[2].Value;

                            string instrument = _instruments.GetInstumentByShortName(shortName);

                            if (instrument == "")
                            {
                               
                                return;
                            }



                            bool posWasClosed = currPos == null && DictPos.ContainsKey(instrument) && DictPos[instrument].Pos != 0;

                            if (currPos != null || posWasClosed)                                
                            {
                                long lotSize = _instruments.GetLotSize(instrument);




                                if (lotSize == 0)
                                {
                                    Error("CPosistionsBoxASTS.UpdateCurrencyPos lotsize==0");
                                    return;

                                }




                                decimal rawPos = 0;

                                if (currPos !=null)
                                    rawPos = (decimal)currPos;


                                DictPos[instrument].Pos = (int)(rawPos / lotSize);
                                DictPos[instrument].PosGUI = DictPos[instrument].Pos;



                            }


                        }

            }




         




        }






        public void Error(string msg, Exception e = null)
        {
            _alarmer.Error(msg,e);

        }



    }
}

