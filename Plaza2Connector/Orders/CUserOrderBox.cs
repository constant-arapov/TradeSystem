using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Common;
using Common.Interfaces;
using Common.Logger;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Interaction;
using TradingLib.Interfaces.Components;
using TradingLib.Data;
using TradingLib.Bots;
using TradingLib.BotEvents;
using TradingLib.Enums;
using TradingLib.ProtoTradingStructs;



namespace Plaza2Connector
{
	public class CUserOrderBox : IUserOrderBox, IUserOrdersBoxForP2Connector,ILogable
    {
        CPlaza2Connector m_plaza2Connector;


        List<CRawOrdersLogStruct> m_listRawOrdersStruct = new List<CRawOrdersLogStruct>();

        public List<CRawOrdersLogStruct> ListRawOrdersStruct { get { return m_listRawOrdersStruct; } }

        public Mutex mxListRawOrders {get;set;}// =  new System.Threading.Mutex();


        public Dictionary<long, CRawOrdersLogStruct> DictUsersOpenedOrders { set; get; }


        private CLogger Logger { set; get; }


        public CUserOrderBox(CPlaza2Connector plaza2Connector)
        {
            m_plaza2Connector = plaza2Connector;
            mxListRawOrders = new Mutex();
            DictUsersOpenedOrders = new Dictionary<long, CRawOrdersLogStruct>();

            Logger = new CLogger("UserOrderBox");
           Log("UserOrderBox created +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ ");

        }
        public void Log(string message)
        {
            Logger.Log(message);

        }


        private void UpdateDictUserOpenedOrders(FUTTRADE.orders_log ol)
        {


            try
            {

                string isin = m_plaza2Connector.Instruments.GetInstrumentByIsinId(ol.isin_id);
                CRawOrdersLogStruct rawOls = new CRawOrdersLogStruct((FUTTRADE.orders_log)ol.Copy());

                if (ol.action == (sbyte)EnmOrderAction.Added)
                {
                   
                    DictUsersOpenedOrders[ol.id_ord] = rawOls;
                  //  m_plaza2Connector.GUIBox.UpdateOrders(isin, new CRawOrdersLogStruct(ol));
                   Log("Update "+ol.id_ord);
                    string dbgString = "Update "+ ol.id_ord;
                    m_plaza2Connector.GUIBox.ExecuteWindowsUpdate(new Action(() =>m_plaza2Connector.GUIBox.UpdateOrders(isin, rawOls)));
                   Log("End update " + ol.id_ord);


                }
                else if (ol.action == (sbyte)EnmOrderAction.Deleted || ol.action == (sbyte)EnmOrderAction.Deal)
                {
                    DictUsersOpenedOrders.Remove(ol.id_ord);
                    //m_plaza2Connector.GUIBox.RemoveOrder(ol.id_ord);


                   Log("Remove " + ol.id_ord);
                    string dbgString = "Remove " + ol.id_ord;
                    m_plaza2Connector.GUIBox.ExecuteWindowsUpdate(new Action(() => m_plaza2Connector.GUIBox.RemoveOrder(rawOls.Id_ord)));
                   Log("End remove " + ol.id_ord);

                }
            }
            catch (Exception e)
            {

           
                Error("UpdateDictUserOpenedOrders",e);

            }
        
            
        }

        public void Update(FUTTRADE.orders_log ol)
        {

            int i;

            UpdateDictUserOpenedOrders( (FUTTRADE.orders_log) ol.Copy());

           mxListRawOrders.WaitOne();

            {
                try
                {
                    //Here we fill "Raw Orders struct list based on replRev/replId
    
                    for (i = 0; i < m_listRawOrdersStruct.Count; i++)
                    {
                        if (m_listRawOrdersStruct[i].ReplId == ol.replID)
                        {
                            //Seems like this will never happened
                            m_listRawOrdersStruct[i] = new CRawOrdersLogStruct(ol);

                        }

                    }


                    if (i == m_listRawOrdersStruct.Count)
                    {
                        
                   
                        m_listRawOrdersStruct.Add(new CRawOrdersLogStruct(ol));

                        if (ol.action == (sbyte)EnmOrderAction.Deal)
                        {
                   
                        }



                        //TO DO put trigger new log here ?

                   
                        {
                           
                                foreach (CBotBase bt in m_plaza2Connector.ListBots)
                                {                                    
                                        if (bt.BotId == ol.ext_id)
                                        {

                                            EnmBotEventCode evCode = EnmBotEventCode.OnEmptyEvent;
                                            if (ol.action == (sbyte)EnmOrderAction.Added)
                                            {

                                                evCode = EnmBotEventCode.OnOrderAccepted;
                                               //TEMPO ! comment it
                                             /*   CRawOrdersLogStruct rlstr = new CRawOrdersLogStruct(ol);
                                                string instr = m_plaza2Connector.Instruments.GetInstrumentByIsinId(ol.isin_id);                                               
                                                bt.Recalc(instr, EnmBotEventCode.OnOrderAccepted, rlstr);
                                               */ 
                                            }
                                            else if (ol.action == (sbyte)EnmOrderAction.Deleted)
                                            {

                                              /*  CRawOrdersLogStruct rls = new CRawOrdersLogStruct(ol);
                                                string isin = m_plaza2Connector.Instruments.GetInstrumentByIsinId(ol.isin_id);
                                                bt.Recalc(isin, EnmBotEventCode.OnOrderCancel, rls);*/
                                                evCode = EnmBotEventCode.OnOrderCancel;

                                            }
                                            else if (ol.action == (sbyte)EnmOrderAction.Deal)
                                            {
                                              
                                              /*  CRawOrdersLogStruct rls = new CRawOrdersLogStruct(ol);
                                                string isin = m_plaza2Connector.Instruments.GetInstrumentByIsinId(ol.isin_id);
                                                bt.Recalc(isin, EnmBotEventCode.OnOrderDeal, rls);
                                               */
                                                evCode = EnmBotEventCode.OnOrderDeal;

                                            }

                                            
                                            CRawOrdersLogStruct rls = new CRawOrdersLogStruct(ol);

                                       

                                            
                                            string isin = m_plaza2Connector.Instruments.GetInstrumentByIsinId(ol.isin_id);
                                            bt.Recalc(isin, evCode, rls);
                                            


                                        }
                                    }
                                }

                            


                        }



                          

                     mxListRawOrders.ReleaseMutex();
                    
                }
                catch (Exception e)
                {
                    Error("CUserOrderBox.Update",e);

                }



            }
         


        //
        // TODO gen here orderslist for each obot
        //

        }


	

        public void Error(string description, Exception exception = null)
        {
            Error(description,  exception );

        }





    }
}
