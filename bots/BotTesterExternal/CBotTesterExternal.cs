using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plaza2Connector
{
   

        [Serializable]
       public class CBotTesterExternal : CBotSingleInstrument  //CBotBase 
        {


            public void TmpMethod2()
            {
                Log("Say Hello !");


            }
            public CBotTesterExternal(int botId, string name, CSettingsBot settingsBot,
                               Dictionary<string, string> dictSettingsStrategy, CPlaza2Connector plaza2Connector) :
                base(botId, name, settingsBot, dictSettingsStrategy, plaza2Connector)
            {

               // SetState(EnmStratStates._000_PreInitial);

            } 


            public CBotTesterExternal() :
                base()
            {

              
            }

            public override void Init (int botId, string name, CSettingsBot settingsBot,
                               Dictionary<string, string> dictSettingsStrategy, CPlaza2Connector plaza2Connector)
            {

                base.Init  (botId,  name, settingsBot,
                                dictSettingsStrategy, plaza2Connector);

                SetState(EnmStratStates._000_PreInitial);
            }

            public override void Start()
            {

                base.Start();


            }




            bool bWas = false;


            protected override void CreateTimers()
            {
                base.CreateTimers();

                AddTimer("AfterFirstLimitOrderWasAccepted", 2000);
                AddTimer("BeforeAddSecondLimitOrder", 2000);
                AddTimer("BeforeSecondLimitCancell", 2000);


            }



            private int m_parOffset = 100;


            



            protected override void RecalcBotLogics(CBotEventStruct botEvent)
            {
                base.RecalcBotLogics(botEvent);
                // CloseAllBotPositions();
                // AddMarketOrder(m_isin, OrderDirection.Buy, m_parLot);
                TmpMethod2();

                if (IsState(EnmStratStates._000_PreInitial))
                {

                    if (MonitorOrders.Count == 0 && ((MonitorPositions == null || MonitorPositions.Amount == 0)))
                    {
                        SetState(EnmStratStates._010_Initial);
                    }
                    else
                    {
                        CancellAllBotOrders();
                        CloseAllBotPositions();

                        SetState(EnmStratStates._001_WaitAllOldOrdersClosed);
                    }

                }
                else if (IsState(EnmStratStates._001_WaitAllOldOrdersClosed))
                {
                    if (MonitorOrders.Count == 0 && (MonitorPositions == null || MonitorPositions.Amount == 0))
                        SetState(EnmStratStates._010_Initial);


                }
                else if (IsState(EnmStratStates._010_Initial))
                {
                    //tempo
                //    AddMarketOrder(OrderDirection.Buy);
                  //  SetState(EnmStratStates._100_NormalExit);
                    //


                    AddOrderNearSpread(m_isin, OrderDirection.Sell, m_parLot, m_parOffset);
                    SetState(EnmStratStates._020_WaitFirstLimitOrderAdded);

                }
                else if (IsState(EnmStratStates._020_WaitFirstLimitOrderAdded))
                {

                    if (MonitorOrders.Count == 1 && (
                        (MonitorOrders.First()).Value).Amount == m_parLot)
                    {
                        SetState(EnmStratStates._021_AfterFirstLimitOrderWasAccepted);
                        StartTimer("AfterFirstLimitOrderWasAccepted");
                    }

                }
                else if (IsState(EnmStratStates._021_AfterFirstLimitOrderWasAccepted))
                {

                    if (botEvent.EventCode == EnmBotEventCode.OnTimer
                   && (string)botEvent.Data == "AfterFirstLimitOrderWasAccepted")
                    {

                        if (MonitorOrders.Count == 1)
                        {
                            long id = (MonitorOrders.First()).Key;
                            CancelOrder(id);
                            SetState(EnmStratStates._022_WaitFirstLimitOrderCancell);
                        }

                    }

                }
                else if (IsState(EnmStratStates._022_WaitFirstLimitOrderCancell))
                {

                    if (MonitorOrders.Count == 0)
                        SetState(EnmStratStates._023_AfterFirstLimitOrderCancell);


                }
                else if (IsState(EnmStratStates._023_AfterFirstLimitOrderCancell))
                {

                    StartTimer("BeforeAddSecondLimitOrder");
                    SetState(EnmStratStates._030_BeforeSecondLimitOrderDelay);


                }
                else if (IsState(EnmStratStates._030_BeforeSecondLimitOrderDelay))
                {

                    if (botEvent.EventCode == EnmBotEventCode.OnTimer
                     && (string)botEvent.Data == "BeforeAddSecondLimitOrder")
                    {

                        AddOrderNearSpread(m_isin, OrderDirection.Sell, m_parLot, m_parOffset);
                        SetState(EnmStratStates._031_WaitSecondLimitOrderAdded);

                    }

                }
                else if (IsState(EnmStratStates._031_WaitSecondLimitOrderAdded))
                {
                    if (MonitorOrders.Count == 1 && (
                     (MonitorOrders.First()).Value).Amount == m_parLot)
                    {
                        SetState(EnmStratStates._032_AfterSecondLimitOrderAccepted);

                    }
                }
                else if (IsState(EnmStratStates._032_AfterSecondLimitOrderAccepted))
                {


                    StartTimer("BeforeSecondLimitCancell");
                    SetState(EnmStratStates._033_BeforeSecondLimitCancellDelay);




                }

                else if (IsState(EnmStratStates._033_BeforeSecondLimitCancellDelay))
                {


                    if (botEvent.EventCode == EnmBotEventCode.OnTimer
                  && (string)botEvent.Data == "BeforeSecondLimitCancell")
                    {

                        if (MonitorOrders.Count == 1 && (
                       (MonitorOrders.First()).Value).Amount == m_parLot)
                        {

                            CancellAllBotOrders();
                            SetState(EnmStratStates._034_WaitSecondLimitOrderCancell);
                        }
                    }
                }
                else if (IsState(EnmStratStates._034_WaitSecondLimitOrderCancell))
                {


                    if (MonitorOrders.Count == 0)
                        SetState(EnmStratStates._035_AfterSecondLimitOrderCancell);






                }



                /*
                Log("Recalc specific");
          
                int tmp_sleep = 10;

                if (m_plaza2Connector.IsOrderControlAvailable && m_plaza2Connector.IsStockOnline)
                {

                    if (!bWas)
                    {
                        bWas = true;

                        if (BotId == 1)
                        {
                          //  Alarm("test1");
                          //  Alarm("test2");
                           // System.Threading.Thread.Sleep(8000);
                        //   this.SettingsBot.Enabled = false;
                     
                        //     AddOrderNearSpread("RTS-9.15", OrderDirection.Buy, 1, 90); 
                        //      AddOrderNearSpread("Si-9.15", OrderDirection.Buy, 1, 110); 
                        //       AddOrderNearSpread("RTS-9.15", OrderDirection.Sell, 1, 80); 
                        //       AddOrderNearSpread("Si-9.15", OrderDirection.Sell, 1, 100); 
                        //       AddOrderNearSpread("RTS-9.15", OrderDirection.Buy, 1, 110); 
                   
                    
                        
                     //  CancellAllBotOrders();

                           // CloseAllBotPositions();


                        
                            //TO DO open by market
                    //  AddMarketOrder("RTS-9.15", OrderDirection.Buy, 1);

                     // AddOrder("RTS-6.15", 103840, OrderDirection.Sell, 1);
                         // CancellAllOrders();

                  


                           // CancelOrder(40133784905);
                             //    AddMarketOrder("RTS-6.15", OrderDirection.Buy, 1);

                           //    AddOrder("RTS-6.15", 99890, OrderDirection.Buy, 3);
                            //      AddOrder("RTS-6.15", 102000, OrderDirection.Buy, 1);
                            //   AddOrder("RTS-6.15", 101000, OrderDirection.Sell, 1);

                  //          if (!bWas)
               //             {
                                //    CancelOrder(40100161380);
                                //     CancellAllOrders();
                                //   CancellAllBotOrders();
                                //   AddOrder("RTS-6.15", 107770, OrderDirection.Buy, 1);
                   //             bWas = true;
                    //        }

                    //    }
                  //      else if (BotId == 2)
                        {

              
                       //     AddOrderNearSpread("RTS-9.15", OrderDirection.Buy, 1, 70); 
                        //    AddOrderNearSpread("Si-9.15", OrderDirection.Buy, 1, 70); 
                       //     AddOrderNearSpread("RTS-9.15", OrderDirection.Sell, 1, 50); 
                       //     AddOrderNearSpread("Si-9.15", OrderDirection.Sell, 1, 50); 
                         //   AddOrderNearSpread("RTS-9.15", OrderDirection.Buy, 1, 90); 
                        //    AddOrderNearSpread("Si-9.15", OrderDirection.Buy, 1, 90);
                    
            

                       //       CancellAllBotOrders();
                        //    CloseAllBotPositions();
                        
                          //  AddMarketOrder("RTS-6.15", OrderDirection.Sell, 1);
                            //  AddMarketOrder("RTS-6.15", OrderDirection.Buy, 1);

                            // AddOrder("RTS-6.15", 97000, OrderDirection.Buy, 1);
                            //  AddOrder("RTS-6.15", 102000, OrderDirection.Buy, 1);
                            //   AddOrder("RTS-6.15", 95000, OrderDirection.Sell, 1);
                            if (!bWas)
                            {
                                //   CancellAllBotOrders();
                                bWas = true;

                            }


                        }

                    }

                }

                */


            }

            enum EnmStratStates
            {
                _000_PreInitial,
                _001_WaitAllOldOrdersClosed,
                _010_Initial,
                _020_WaitFirstLimitOrderAdded,
                _021_AfterFirstLimitOrderWasAccepted,
                _022_WaitFirstLimitOrderCancell,
                _023_AfterFirstLimitOrderCancell,
                _030_BeforeSecondLimitOrderDelay,
                _031_WaitSecondLimitOrderAdded,
                _032_AfterSecondLimitOrderAccepted,
                _033_BeforeSecondLimitCancellDelay,
                _034_WaitSecondLimitOrderCancell,
                _035_AfterSecondLimitOrderCancell,
                _100_NormalExit






            }





        }
       [Serializable]
     public class DummyClass
       {

           public DummyClass()
           {


           }



       }




}
