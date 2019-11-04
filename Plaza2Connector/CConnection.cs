using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ru.micexrts.cgate;
using System.Runtime.InteropServices;
using ru.micexrts.cgate.message;


using System.Threading;

using System.Diagnostics;




using Common;
using Common.Interfaces;
using Common.Logger;

using Plaza2Connector.Common;

namespace Plaza2Connector
{
    class CConnection : IAlarmable
    {

      
        Connection m_conn;

        System.Threading.Thread m_threadProc;

        string m_settings;

       
        DateTime m_dtLastOKConnection;
        int m_parSecsSinceConnOK = 5;
        public int BotId;

        public bool Exit { get; set; }
                
          List <CSettingsListener> m_listenersSettings = new List<CSettingsListener>();
          List<CSettingsPublisher> m_publisherSettings = new List<CSettingsPublisher>();

          List<CPlaza2Listener> m_listenersList = new List<CPlaza2Listener>();
          List<CPlaza2Publisher> m_publishersList = new List<CPlaza2Publisher>();

          CLogger m_logger;

          string Name { get; set; }


          CPlaza2Connector m_plaza2Connector;


          int m_sleepTime;

          private CSleeper _sleeper;

          public bool IsConnectionOrderControl { set; get; }
          public bool IsOrderControlReady { set; get; }

          public bool IsConnectionProblem { get; set; } 


          public System.Collections.Concurrent.BlockingCollection<CCommandStucture> CommandQueue {get;set;} //= new System.Collections.Concurrent.BlockingCollection<CCommandStucture> ();


           public CConnection(string name, string protocol, string ip, string port, string appName,  List<CSettingsListener> listenersSettings, List<CSettingsPublisher> publisherSettings, CPlaza2Connector p2con, int sleepTime)
        {
           
            Name = name;
            m_settings = string.Format(@"{0}://{1}:{2};app_name={3}_{4}", protocol, ip, port, appName,  name   );
            
            BotId = 0;
            IsConnectionOrderControl = CUtilConnection.IsConnectionOrderControl(Name, ref  BotId);

            m_listenersSettings = listenersSettings;
            m_publisherSettings = publisherSettings;
            Exit = false;
            m_plaza2Connector = p2con;
                
            
            m_threadProc = new System.Threading.Thread(ThreadProc);
            m_threadProc.Start();
            if (name == "ConnectionMarketData" || name == "ConnectionStockData" /*|| name == "ConnectionHeartBeat"*/)
                m_threadProc.Priority = ThreadPriority.Highest;


            m_sleepTime = sleepTime;

            IsConnectionProblem = false;
            _sleeper = new CSleeper();
         
        }
  



          private void CreateListeners()
          {
              //TO DO check if exists
              //m_listenersList.Clear();
              foreach (CSettingsListener SL in m_listenersSettings)              
                  m_listenersList.Add (new CPlaza2Listener( m_conn, SL, m_plaza2Connector,this));
                              
          }

          private void CreatePublishers()
          {

              //TO DO check if exists
              //m_publishersList.Clear();
              foreach (CSettingsPublisher sp in m_publisherSettings)
                  m_publishersList.Add(new CPlaza2Publisher(m_conn, sp,m_plaza2Connector));

             

          }



          private void AddOrder(CCommandStucture cs)
          {

          CCommandAddOrder ao = (CCommandAddOrder)cs.Command;

              Message sendMessage = m_publishersList[0].Publisher.NewMessage(MessageKeyType.KeyName, "FutAddOrder");
              DataMessage smsg = (DataMessage)sendMessage;

          

              DateTime dtExp = DateTime.Now.AddHours(48);
              try
              {
             

                  smsg.UserId = (uint)ao.UserId;

                  smsg["broker_code"].set(ao.Broker_code);
                  smsg["isin"].set(ao.Isin);
                  smsg["client_code"].set(ao.Client_code);
                  smsg["type"].set((int) ao.Type);
                  smsg["dir"].set((int)ao.Dir);  //1 -buy 2 -sell
                  smsg["amount"].set(ao.Amount);
                  smsg["price"].set(ao.Price);
                  smsg["ext_id"].set(ao.Ext_id);
                  smsg["date_exp"].set(dtExp.ToString("yyyyMMdd"));

                  
                  
                  m_publishersList[0].Publisher.Post(sendMessage, PublishFlag.NeedReply);
                // Log("Add order isin=" + smsg["isin"] + " type=" + smsg["type"] + " dir=" + smsg["dir"] + " price="  +  smsg["price"] +  " amount=" + smsg["amount"]);
                  Log("Add order isin=" + ao.Isin + " type=" + ao.Type + " dir=" + ao.Dir + " price=" + ao.Price + " amount=" + ao.Amount+ " ext_id="+ ao.Ext_id) ;

               

                  
              }
              catch (Exception e)
              {
                  if ((DateTime.Now - m_dtLastOKConnection).TotalSeconds < m_parSecsSinceConnOK)
                  {

                     Log("Error in  CConnection.AddOrder. " + e.Message);
                      Error("Error in  CConnection.AddOrder. ", e);
                  }

              }

         

             sendMessage.Dispose();

          }

          public void Log(string message)
          {
             m_logger.Log(message);
          }


         public void Error(string description, Exception exception = null)
          {

              m_plaza2Connector.Alarmer.Error(description,  exception );


          }

          private void DeleteOrder(CCommandStucture cs)

          {

              try
              {                                                                                         //TODO change to commandname
                  Message sendMessage = m_publishersList[0].Publisher.NewMessage(MessageKeyType.KeyName, "FutDelOrder");
                  DataMessage smsg = (DataMessage)sendMessage;

                  CCommandDelOrder delo = (CCommandDelOrder)cs.Command;

                  smsg["order_id"].set(delo.Order_id);


                  m_publishersList[0].Publisher.Post(sendMessage, PublishFlag.NeedReply);


                  smsg.Dispose();
              }catch (Exception e)
              {
                 Log("Error deleting order. "+ e.Message);
                 Error("Error deleting order", e);
                }



          }

          private void DeleteUserOrders(CCommandStucture cs)
          {

              try
              {
                  Message sendMessage = m_publishersList[0].Publisher.NewMessage(MessageKeyType.KeyName, cs.CommandName);
                  DataMessage smsg = (DataMessage)sendMessage;

                  CCommandDelUserOrders delu = (CCommandDelUserOrders)cs.Command;

                  smsg["buy_sell"].set(delu.Buy_sell);
                  smsg["non_system"].set(0);
                  smsg["code"].set(m_plaza2Connector.Client_code);
                  smsg["ext_id"].set(delu.Ext_id);
                  smsg["isin"].set(delu.Isin);
                  m_publishersList[0].Publisher.Post(sendMessage, PublishFlag.NeedReply);
                  Log("Cancel user orders Buy_Sell="+ delu.Buy_sell + " Ext_id"+ delu.Ext_id + " Isin=" + delu.Isin);

              }
              catch (Exception e)
              {
                  string err = "Error delete user orders";
                 Log(err);
                  Error(err,e);

              }

          }


          private void ProcessDummyStruct()
          {
              Log("Process Dummy structure");


          }
          private void ProcessCommandStructure(CCommandStucture cs)
          {

              switch (cs.CommandName)
              {
                  case "FutAddOrder": AddOrder(cs); break;
                  case "FutDelOrder": DeleteOrder(cs); break;
                  case "FutDelUserOrders": DeleteUserOrders(cs); break;
                  case "Dummy": ProcessDummyStruct(); break;
              }



          }

          private bool bEnableCommandQueue;
          private void ThreadProcessCommands()
          {

              CommandQueue = new System.Collections.Concurrent.BlockingCollection<CCommandStucture>();

              CCommandStucture comStruct = new CCommandStucture();
              comStruct.CommandName = "Dummy"; //to remove first iteration delay
              CommandQueue.Add(comStruct);

              foreach (CCommandStucture cs in CommandQueue.GetConsumingEnumerable())
              {
                  if (bEnableCommandQueue)
                  {
                      Log("Process command struct");
                      ProcessCommandStructure(cs);
                  }

              }


          }

         private void  ProcessCommandQueue()
         {


           

           /*  if (CommandQueue.Count > 0)
             {
                 try
                 {

                     for (int i = 0; i < 100; i++)
                     {
                         CCommandStucture cs = new CCommandStucture() ;
                         bool res = CommandQueue.TryDequeue(out cs);
                         ProcessCommandStructure(cs);
                      
                         

                         if (res) break;
                         else
                         {
                             System.Threading.Thread.Sleep(5);

                         }



                     }
                 }
                 catch (Exception e)
                 {
                     string err = "Error processing command queue. ";
                    Log(err+e.Message);
                     Error(err,e);
                 }
             }
             */

        }




          private void ProcessPublishers()
          {
              foreach (CPlaza2Publisher pbsh in m_publishersList)
              {



                  try
                  {

                      if (pbsh.StateIS == State.Closed)
                      {


                          pbsh.Open("");


                      }
                      else if (pbsh.StateIS == State.Error)
                      {
                            pbsh.Close();

                       

                          m_plaza2Connector.IsReadyForSendOrder = false;

                      }
                      else if (pbsh.StateIS == State.Active)
                      {
                  //        if (pbsh.Name == "ListenerStock" || pbsh.Name == "ListenerDeals")
                 //             Log("start process publisher " + pbsh.Name);

                          if (!m_plaza2Connector.IsReadyForSendOrder)
                          {
                            
                              pbsh.CreateStuctures();
                              m_plaza2Connector.IsReadyForSendOrder = true;
                          }
                         // ProcessCommandQueue();

                          bEnableCommandQueue = true;
                        
                      //    if (pbsh.Name == "ListenerStock" || pbsh.Name == "ListenerDeals")
                          //    Log("end process publisher " + pbsh.Name);

                      }
                  }
                  catch (Exception e)
                  {
                      string s = e.Message;
                    //2018-05-07
                    Error("Process publishers. ",e);
                  }
              }

          }

         
       
       
          private void ProcessListeners()
          {
             
              
              foreach (CPlaza2Listener lstn in m_listenersList)
              {

                  try
                  {     //tempo
                     // if (lstn.Name == "ListenerStock" || lstn.Name == "ListenerDeals")
                       //     Log("start process listener "+ lstn.Name);

                      lstn.CheckOnOff();

                      if (!lstn.Disabled &&  lstn.StateIS == State.Closed)
                      {

                        //  if (this.Name != "ConnectionOrderControl_1" ||
                          //    (this.Name == "ConnectionOrderControl_1"  && m_publishersList.Count!=0 &&  m_publishersList[0].StateIS == State.Active))//to remove exceptions
                          if (!IsConnectionOrderControl ||
                                (IsConnectionOrderControl && m_publishersList.Count != 0 && m_publishersList[0].StateIS == State.Active))//to remove exceptions

                          {

                             /* if (lstn.Name == "ListenerPosition")
                                  lstn.Open("mode=snapshot+online;lifenum=1;rev.position=8075354");
                              else if (lstn.Name == "ListenerUserDeals")
                                  lstn.Open("mode=snapshot+online;lifenum=402;rev.user_deal=1374328895");


                              else 
                        */
                              

                              if (lstn.Name == "ListenerHeartBeat")
                                  lstn.Open("mode=online");

                              else if (lstn.Name == "ListenerDeals")
                              {
                                  if (m_plaza2Connector.NeedHistoricalDeals)
                                       lstn.Open("rev.deal=" + m_plaza2Connector.LastDealRevId+";lifenum=" + 
                                            m_plaza2Connector.LastDealLifeNum);
                                      //lstn.Open("rev.deal=" + m_plaza2Connector.LastDealRevId);
                                  else
                                      lstn.Open("mode=online");
                              }
                              //2018-07-02
                              else if (lstn.Name == "ListenerASTSCurrALL_TRADES")
                              {
                                lstn.Open("rev.ALL_TRADES="+m_plaza2Connector.LastASTSCurrDealRevId+";lifenum="+
                                   m_plaza2Connector.LastASTSCurrDealLifeNum);

                               // lstn.Open(String.Format("rev.ALL_TRADES={0};lifenum=0", m_plaza2Connector.LastASTSCurrDealRevId ));// + m_plaza2Connector.LastASTSCurrDealRevId);
                               // lstn.Open("");
                              }
                              
                              else if (lstn.Name == "ListenerStock")
                                  lstn.Open("mode=snapshot+online"/*"mode=online"*/);
                            //  else if (lstn.Name == "ListenerUserOrderLog")
                              //    lstn.Open("rev.orders_log=" + m_plaza2Connector.LastUserOrderLogRevId);

                             // else if (lstn.Name == "ListenerUserDeals")
                               //   lstn.Open("rev.user_deal=" + m_plaza2Connector.LastUserDealRevId);
                              else
                                  lstn.Open("");
                              
                             

                          }

                      }
                      else if (lstn.StateIS == State.Opening)
                      {




                      }

                      else if (!lstn.Disabled && lstn.StateIS == State.Error)
                      {


                          lstn.Disabled = true;
                          lstn.TimerRetry.Set();

                          lstn.Close();
                          lstn.OnError();

                      }
                      else if (!lstn.Disabled && lstn.StateIS == State.Active)
                      {
                          lstn.TimerRetry.Reset();
                          lstn.Disabled = false;

                          // if (lstn.Name == "ListenerOrderControlReply")
                          //m_plaza2Connector.IsOrderControlAvailable = true;

                          if (CUtilConnection.IsConnectionOrderControl(this.Name, ref BotId)
                              && !this.IsOrderControlReady)
                          {


                              (new Thread(ThreadProcessCommands)).Start();
                              System.Threading.Thread.Sleep(100); //wait full init of blocking coll


                              this.IsOrderControlReady = true;




                              if (m_plaza2Connector.IsAllContolConnectionReady())
                                  m_plaza2Connector.IsOrderControlAvailable = true;

                             
                          }
                      }

                 //     if (lstn.Name == "ListenerStock" || lstn.Name == "ListenerDeals")
                    //      Log("end process listener " + lstn.Name);

                  }
                  catch (Exception e)
                  {
                      string err = "Error pocessing listener " + lstn.Name;
                     Log(lstn.Name + " "+ e.Message);
                      m_plaza2Connector.Alarmer.Error(err,e);

                    


                  }

              }

          }

          private void CloseListeners()
          {

              foreach (CPlaza2Listener lstn in m_listenersList)
              {
                  lstn.Close();
                  

              }

          }

          private void ClosePublishers()
          {
              foreach (CPlaza2Publisher pb in m_publishersList)
              {
                  pb.Close();

              }



          }

          private void OnError()
          {
              if (Name == "ConnectionHeartBeat")
                  m_plaza2Connector.IsServerTimeAvailable = false;
              else if (Name == "ConnectionInfoData")
              {} //m_plaza2Connector.IsSessionOnline = false;
              else if (Name == "ConnectionMarketData")
                  m_plaza2Connector.IsDealsOnline = false;
              else if (Name == "ConnectionOrderControl")
                  m_plaza2Connector.IsOrderControlAvailable = false;
              else if (Name == "ConnectionStocksData")
                  m_plaza2Connector.IsStockOnline = false;
              else if (Name == "ConnectionUserData")
              {
                  m_plaza2Connector.IsOnlineVM = false;
                  m_plaza2Connector.IsOnlineUserOrderLog = false;
                  m_plaza2Connector.IsOnlineUserDeals = false;
                  m_plaza2Connector.IsPositionOnline = false;
              }

              IsConnectionProblem = true;


          }

        private  CLogger CreateLogger(string name)
        {
            if (name.Contains("ConnectionOrderControl"))
            {

                int ind = Name.IndexOf("_");
                if (ind > 0)
                {
                    string subDirInp = Name.Substring(0, ind);
                    return new CLogger(name, flushMode: true, subDir: subDirInp, useMicroseconds: false);

                }


            }



            return new CLogger(name);
        }

       


        private void ThreadProc()
        {
            Stopwatch sw = new Stopwatch();
            //m_logger = new CLogger(Name);
            //2017-10-23
            m_logger = CreateLogger(Name);

           Log("Creating working thread for connecion" + Name);

            m_conn = new Connection(m_settings);
            
            bool bActive = false;

             CreateListeners();
             CreatePublishers();
            // if (Name == "ConnectionStocksData")
            //     Thread.BeginThreadAffinity();

                while (!Exit)
                {
                   
                    try
                    {

                    State state = m_conn.State;

                    if (state == State.Error)
                    {
                      //  CloseListeners();
                      //  ClosePublishers();
                        OnError();
                        m_conn.Close();
                       
                        //TO DO Error log
                    }
                    else if (state == State.Closed)
                    {
                       
                        m_conn.Open("");

                       // CreateListeners();
                       // CreatePublishers();

                    }
                    else if (state == State.Opening)
                    {
                        
                        ErrorCode result = m_conn.Process(0);
                       

                       



                        if (result != ErrorCode.Ok && result != ErrorCode.TimeOut)
                        {
                            //TO DO error to log
                           
                           Log(String.Format("Warning: connection state request failed: {0}", CGate.GetErrorDesc(result)));
                        }

                    }
                    else if (state == State.Active)
                    {
                                        

                        if (!bActive) { bActive = true;Log("Connection " + Name + " is active."); }

                      //  Log("before process");
                        sw.Reset();
                        sw.Start();

                     

                        ErrorCode result = m_conn.Process(1);
                      /*  if (m_sleepTime == 0)
                        {
                            if (result == ErrorCode.Ok)
                            {
                                _sleeper.OnDataRecieved();
                            }
                            else if (result == ErrorCode.TimeOut)
                            {

                                _sleeper.OnTimeOut();

                            }
                        }*/
                        sw.Stop();
                        //Log("after process");
                        int delta = m_sleepTime - (int)sw.ElapsedMilliseconds;
					

                        if (m_sleepTime > 0)
                            if (delta > 0)
								if (!(this.Name == "ConnectionMarketData" && result == ErrorCode.Ok))
                                 Thread.Sleep(delta);

                                           
                      
                        if (result != ErrorCode.Ok && result != ErrorCode.TimeOut)
                        {
                            //   CGate.LogError(String.Format("Warning: connection state request failed: {0}", CGate.GetErrorDesc(result)));
                            // TO DO to error log file
                           Log(String.Format("Warning: connection state request failed: {0}", CGate.GetErrorDesc(result)));
                        }
                      

                        //TODO if timeout
                        ProcessListeners();
                        ProcessPublishers();

                 
                        m_dtLastOKConnection = DateTime.Now;

                        if (IsConnectionProblem)
                        {
                            IsConnectionProblem = false;

                            if (Name == "ConnectionStocksData")
                                m_plaza2Connector.StockBoxInp.ReInitAllStocks();

                        }

                  

                    }


                

                }

                   catch (Exception e)
                {

                    if ((DateTime.Now - m_dtLastOKConnection).TotalSeconds < m_parSecsSinceConnOK)
                    {

                        string err = "ERROR CConnection !  " + this.Name;
                       Log(err + e.Message);
                        Error(err, e);

                    }

                  }


            }

               // if (Name == "ConnectionStocksData")
                  //  Thread.EndThreadAffinity();

            m_conn.Close();
            CloseListeners();
            ClosePublishers();
            m_conn.Dispose();

        }

       



      



    }
}
