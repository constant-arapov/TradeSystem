using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Enums;
using TradingLib.BotEvents;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Components;
using TradingLib.Data;

namespace TradingLib.Bots
{
   public class CBotHighLowContra :  CBotSingleInstrument  //CBotBase
    {


       // private EnmStrategyStates m_currState;
        private EnmBrkDir m_brkDir = EnmBrkDir.DOWN;

        private string m_isin;
        private decimal m_HighLast = 0;
        private decimal m_LowLast = 0;

        //private decimal m_ClosedTFPrice = 0;
        private decimal m_HighCurrent = 0;
        private decimal m_LowCurrent = 0;
        private decimal m_ClosedTFVolume = 0;

        CTimeFrameAnalyzer m_TimeframeAnalyzer = null;


        private string m_parTF;

        private int m_parLot = 1;
        long  m_parVolume;
        private double m_parDelayAfterTFBegin = 0;

     public CBotHighLowContra(int botId, string name, CSettingsBot settingsBot,  Dictionary<string, string> dictSettingsStrategy,
                                                                                        /*CPlaza2Connector*/IDealingServer plaza2Connector) :
            base(botId, name, settingsBot, dictSettingsStrategy, plaza2Connector)
        {

           

      
                        
        }

     protected override void LoadParameters()
     {

         base.LoadParameters();
         m_parTF = SettingsStrategy["TimeFrame"];
         m_parVolume = Convert.ToInt32(SettingsStrategy["VolumeOfCandle"]);
         m_parDelayAfterTFBegin = Convert.ToDouble(SettingsStrategy["DelayAfterTFBegin"]);

         m_isin = SettingsBot.ListIsins[0];
         SetState(EnmStrategyStates._010_Initial);

     }


     protected override void RecalcBotLogics(CBotEventStruct botEvent)
     {
         
         base.RecalcBotLogics(botEvent);
         //------------------------ INITIAL BLOCK ----------------------------------------------------------------------------
         if (IsState(EnmStrategyStates._010_Initial))
         {
             //tempo for debugging
            CloseAllPositions();
          //  DisableBot();

             if (m_TimeframeAnalyzer == null)
              m_TimeframeAnalyzer = _dealingServer.DealBox.DealsStruct[m_isin].TimeFrameAnalyzer;
             UpdateLastHighLow();
             if (m_HighLast!=0 && m_LowLast!=0)
                SetState(EnmStrategyStates._020_WaitingForExtremumBrake);

         }

         //-----------------------------------------------------------------------------------------------------------------------------------
         //--------------------- WAITING FOR EXTREMUM BLOCK-------------------------------------------------------------------------------------
         else if (IsState(EnmStrategyStates._020_WaitingForExtremumBrake))
         {

             if (botEvent.EventCode == EnmBotEventCode.OnTFUpdate)
             {
                 string stTF = (((BotEventTF)botEvent.Data).TFUpdate).ToString();
             }
             else if (botEvent.EventCode == EnmBotEventCode.OnTFChanged)
             {

                 string stTF = (((BotEventTF)botEvent.Data).TFUpdate).ToString();

                 if (stTF == m_parTF)
                 {
                     DateTime dtn = DateTime.Now;
                     DateTime dt = ((CTimeFrameInfo)((BotEventTF)botEvent.Data).TFI).Dt; ;
                     //m_ClosedTFPrice = ((CTimeFrameInfo)((BotEventTF)botEvent.Data).TFI).ClosePrice;
                     m_HighCurrent = ((CTimeFrameInfo)((BotEventTF)botEvent.Data).TFI).HighPrice;
                     m_LowCurrent = ((CTimeFrameInfo)((BotEventTF)botEvent.Data).TFI).LowPrice;
                     m_ClosedTFVolume = ((CTimeFrameInfo)((BotEventTF)botEvent.Data).TFI).Volume;

                      //m_TimeframeAnalyzer.GetTFIByDate(m_parTF, dt).ClosePrice;
                     if (WasBreakedExtermum())
                     {

                         if (m_ClosedTFVolume > m_parVolume)
                         {
                              Log("m_ClosedTFVolume="+ m_ClosedTFVolume);
                             SetState(EnmStrategyStates._030_ExtremumWasBrake);

                         }
                         else
                         {
                             //SetState(EnmStrategyStates._020_WaitingForExtremumBrake);
                             Log("m_ClosedTFVolume <= m_parVolume  Not enough volume. m_ClosedTFVolume=" + m_ClosedTFVolume + " m_parVolume=" + m_parVolume);
                             UpdateLastHighLow();
                         }

                     }
                     



                 }
             }


         }
         //--------------------- WAITING FOR EXTREMUM BLOCK-------------------------------------------------------------------------------------
         else if (IsState(EnmStrategyStates._030_ExtremumWasBrake))
         {
             StartTimer("DelayAfterExtremumBrake");
             SetState(EnmStrategyStates._040_DelayAfterExtremumBrake);

         }
         //--------------------- END WAITING FOR EXTREMUM BLOCK---------------------------------------------------------
        //--------------------- DELAY AFTER EXTREMUM BLOCK-----------------------------------------------------------------
         else if (IsState(EnmStrategyStates._040_DelayAfterExtremumBrake))
         {
            
             if (botEvent.EventCode == EnmBotEventCode.OnTimer 
                 && (string) botEvent.Data == "DelayAfterExtremumBrake")
             {
                 SetState(EnmStrategyStates._050_BeforeOpenPosition);

             }

         }
         //--------------------------------------------------------------------------------------------------------
         else if (IsState(EnmStrategyStates._050_BeforeOpenPosition))
         {
            
             if (m_brkDir == EnmBrkDir.DOWN)
                 AddMarketOrder(m_isin,  /*OrderDirection.Buy*/EnmOrderDir.Buy, m_parLot);
             else if (m_brkDir == EnmBrkDir.UP)
                 AddMarketOrder(m_isin, /*OrderDirection.Sell*/EnmOrderDir.Sell, m_parLot);
       
            
             SetState(EnmStrategyStates._060_WaitOpenedPos);
            // StartTimer(
         }
         else if (IsState(EnmStrategyStates._060_WaitOpenedPos))
         {
             //TO DO - move to base classes
             if (MonitorPositionsAll.ContainsKey(m_isin) &&
                 Math.Abs(MonitorPositionsAll[m_isin].Amount) == m_parLot)
             {

                 SetState(EnmStrategyStates._070_AfterOpenedPosition);

             }

         }
         else if (IsState(EnmStrategyStates._070_AfterOpenedPosition))
         {

             if (IsStopLossWasBroken())
             {
                 CloseAllPositions();
                 SetState(EnmStrategyStates._081_AfterStoplossWasBroken);
            }
             if (IsTakeProfitWasBroken()   )
             {
                 CloseAllPositions();
                 SetState(EnmStrategyStates._082_AfterTakeProfitWasBroken);

             }



         }
         else if (IsState(EnmStrategyStates._081_AfterStoplossWasBroken))
         {
             if (MonitorPositionsAll.ContainsKey(m_isin) &&
                  Math.Abs(MonitorPositionsAll[m_isin].Amount) == 0)
                 SetState(EnmStrategyStates._010_Initial);
         }
         else if (IsState(EnmStrategyStates._082_AfterTakeProfitWasBroken))
         {
             if (MonitorPositionsAll.ContainsKey(m_isin) &&
                 Math.Abs(MonitorPositionsAll[m_isin].Amount) == 0)
                 SetState(EnmStrategyStates._010_Initial);


         }



     }
     protected override void CreateTimers()        
     {
         AddTimer("DelayAfterExtremumBrake", m_parDelayAfterTFBegin);
         base.CreateTimers();
     }
     private bool WasBreakedExtermum()
     {
         decimal tmp_extrOffsetUp =   0;// 500;//for debugging, in production set to zero
         decimal tmp_extrOffsetDown = 0;// 500;

         if (/*m_ClosedTFPrice*/ m_HighCurrent > m_HighLast - tmp_extrOffsetUp)
         {
             m_brkDir = EnmBrkDir.UP;
             //Log("WasBreakedExtermum. true. Up m_HighLast =" + m_HighLast + " m_ClosedTFPrice=" + m_ClosedTFPrice + " tmp_extrOffsetUp=" + tmp_extrOffsetUp);
             Log("WasBreakedExtermum. true. Up m_HighLast =" + m_HighLast + " m_HighCurrent=" + m_HighCurrent + " tmp_extrOffsetUp=" + tmp_extrOffsetUp);
             return true;
         }
         else if (/*m_ClosedTFPrice*/ m_LowCurrent < m_LowLast + tmp_extrOffsetDown)
         {
             m_brkDir = EnmBrkDir.DOWN;
             //Log("WasBreakedExtermum. true. DOWN m_LowLast =" + m_LowLast + " m_ClosedTFPrice=" + m_ClosedTFPrice + " tmp_extrOffsetDown=" + tmp_extrOffsetUp);
            Log("WasBreakedExtermum. true. DOWN m_LowLast =" + m_LowLast + " m_LowCurrent=" + m_LowCurrent + " tmp_extrOffsetDown=" + tmp_extrOffsetUp);
             return true;
         }

         Log("WasBreakedExtermum. false.  m_HighLast =" + m_HighLast + " m_LowLast =" + m_LowLast + " m_HighCurrent=" + m_HighCurrent + m_LowCurrent + " " + m_LowCurrent);

         return false;
     }
     public void UpdateLastHighLow()
     {
         m_HighLast = _dealingServer.DealBox.DealsStruct[m_isin].TimeFrameAnalyzer.HighDayPrice;
         m_LowLast = _dealingServer.DealBox.DealsStruct[m_isin].TimeFrameAnalyzer.LowDayPrice;

     }

    




     private enum EnmStrategyStates
     {
         _010_Initial,
         _020_WaitingForExtremumBrake,
         _030_ExtremumWasBrake,
         _040_DelayAfterExtremumBrake,
         _050_BeforeOpenPosition,
         _060_WaitOpenedPos,
         _070_AfterOpenedPosition,
         _081_AfterStoplossWasBroken,
         _082_AfterTakeProfitWasBroken

     }
     private enum EnmBrkDir
     {
         UP, 
         DOWN
        

     }

    }
  


}
