using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


using Common;
using Common.Collections;
using Common.Utils;

using TradingLib;
using TradingLib.Bots;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Data;
using TradingLib.GUI;


namespace TradingLib.GUI
{
    public class CGUIBot : CBaseProppertyChanged, IGUIBot, IDisposable
    {

        Dispatcher GUIDispatcher;
        /*CPlaza2Connector*/
        IClientGUIBot m_plaza2Connector;

        public CObservableIdCollection<MonitorPosInst, string> MonitorPos { get; set; }
        public CObservableIdCollection<OrderInst, long> Orders { set; get; }


        public ObservableCollection<CBotPos> PosLog { set; get; }


        public CUserDealsCollection UserDealsCollection { set; get; }
       
        public long BotId {get; set;}

        private string _botState;

      
        public event DGUIBotDelegate DisposeGUIBotEvent;


 
        public string BotState
        {

            get
            {
                return _botState;

            }
            set
            {
                _botState = value;
                RaisePropertyChanged("BotState");
            }

        }


        bool _readyForRecalcBotLogics = false;
        public bool IsReadyForRecalcBotLogics
        {
            get
            {
                return _readyForRecalcBotLogics; 
            }
            set 
            {
                _readyForRecalcBotLogics = value;
                RaisePropertyChanged("IsReadyForRecalcBotLogics");

            }




        }
  
        public CGUIBot(/*CPlaza2Connector*/IClientGUIBot plaza2Connector, long botId)
        {
            m_plaza2Connector = plaza2Connector;
            BotId = botId;

            (new System.Threading.Tasks.Task(TaskSetGUIDispatcher)).Start();


            MonitorPos = new CObservableIdCollection<MonitorPosInst, string>();

            Orders = new CObservableIdCollection<OrderInst, long>();

            PosLog = new ObservableCollection<CBotPos>();

                                 

            UserDealsCollection = new CUserDealsCollection(BotId,  (IClientUserDealsCollection) plaza2Connector);



        }

		public void UpdateDeal(CRawUserDeal rd, string instrument)
		{

			UserDealsCollection.Add(rd, instrument);


		}

        public void Dispose()
        {
            if (DisposeGUIBotEvent != null)
                DisposeGUIBotEvent();

            
        }

         ~CGUIBot()
        {
            Console.WriteLine("GUIBOT was terminated");


        }


        public void TaskSetGUIDispatcher()
        {

            const int NUM_CYCLES = 100;
            const int CYCLE_PERIOD = 100;
            int i = 0;

            for (i = 0; i < NUM_CYCLES; i++)
            {
                if (m_plaza2Connector.GUIBox != null && m_plaza2Connector.GUIBox.GUIDispatcher != null)
                {

                    GUIDispatcher = m_plaza2Connector.GUIBox.GUIDispatcher;
                    return;

                }

                Thread.Sleep(CYCLE_PERIOD);

            }

        }

        public void UpdateBotState(string st)
        {

            m_plaza2Connector.GUIBox.ExecuteWindowsUpdate(new Action ( ()=> BotState = st));

        }

        public void UpdatePosLog(CBotPos bp)
        {
              m_plaza2Connector.GUIBox.ExecuteWindowsUpdate(new Action(() =>
                PosLog.Add(bp)
                ));

           




        }



        private string GetPriceFormat(string isin)
        {

           
            if (m_plaza2Connector.DictInstruments!= null && m_plaza2Connector.DictInstruments.ContainsKey(isin))           
                 return   CUtil.GetStringFormatForStep(m_plaza2Connector.DictInstruments[isin].Min_step);
            

            return "";
        }
        public void UpdateMonitorPos(string isin,CBotPos pos)
        {
                 
            if (pos.Amount == 0)

                m_plaza2Connector.GUIBox.ExecuteWindowsUpdate(new Action(() =>
                  MonitorPos.RemoveWithId(isin)
                  ));
            else

                m_plaza2Connector.GUIBox.ExecuteWindowsUpdate(new Action(() =>
                MonitorPos.UpdateWithId(new MonitorPosInst(isin, (CBotPos)pos.Copy(), GetPriceFormat(isin)))
                ));



            if (m_plaza2Connector.IsOnlineUserDeals && !m_plaza2Connector.GUIBox.IsOnlineUserDealsDelayed)
                m_plaza2Connector.GUIBox.ExecuteWindowsUpdate(new Action(() =>
                   m_plaza2Connector.GUIBox.IsOnlineUserDealsDelayed = true));


            


        }

        public void UpdateOrders(string isin, CRawOrdersLogStruct rols)
        {


            if (m_plaza2Connector.IsOnlineUserOrderLog &&
                !m_plaza2Connector.GUIBox.IsOnlineUserOrderLogDelayed)

                m_plaza2Connector.GUIBox.ExecuteWindowsUpdate(new Action(() =>
                {
                    m_plaza2Connector.GUIBox.IsOnlineUserOrderLogDelayed = true;


                }

                 ));


             m_plaza2Connector.GUIBox.ExecuteWindowsUpdate(new Action(() =>
            Orders.UpdateWithId(new OrderInst(isin, rols))
            ));



        }


        public void RemoveOrders(string isin, CRawOrdersLogStruct rols)
        {
             m_plaza2Connector.GUIBox.ExecuteWindowsUpdate(new Action(() =>
            Orders.RemoveWithId(rols.Id_ord)
            ));


        }
    


    }
}
