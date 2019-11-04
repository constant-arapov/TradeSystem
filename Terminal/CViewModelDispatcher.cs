using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Visualizer;
using TradingLib;

using System.Windows;
using System.Windows.Input;

using Common;
using Common.Interfaces;
using Common.Utils;

using TCPLib;

using Terminal.Common;
using Terminal.Events;
using Terminal.Interfaces;
using Terminal.Conf;
using Terminal.Controls.ChildWindows;
using Terminal.Controls.Market;

using TradingLib.ProtoTradingStructs;

using Terminal.ViewModels;
using Terminal.ViewModels.ChildWindows;

using Terminal.Views.ChildWindows;


namespace Terminal
{

	public class CViewModelDispatcher : IAlarmable, IChildWinDataUpdater
    {

        //private List<MarketViewModel> _listMarketViewModel = new List<MarketViewModel>();

        public TerminalViewModel TerminalViewModel { get; set; }


        private Dictionary<string, MarketViewModel> _dictKIsinVMarketViewModel = new Dictionary<string, MarketViewModel>();

        //private ViewModelPosLog _viewModelPosLog;

       // private List<MarketViewModel> ListMarketViewModel = new List<MarketViewModel>();
        private CKernelTerminal _kernelTerminal;

        private Dictionary<BaseViewModelChildWin,Window> _dictKViewModelVView = 
                                    new Dictionary<BaseViewModelChildWin, Window>();


        private Dictionary<Type, Type> _dictKTypeDataVTypeBaseViewModelChild =
                                    new Dictionary<Type, Type>();

        private Dictionary<Type, Type> _dictKTypeWindowVTypeViewModelChild =
                                   new Dictionary<Type, Type>();

        private CAlarmer _alarmer;




        EvntDispMarketViewModel _evntDispMarketViewModel;



        public CViewModelDispatcher(CKernelTerminal kernelTerminal)
        {
            _kernelTerminal = kernelTerminal;
            _alarmer = kernelTerminal.Alarmer;
            _evntDispMarketViewModel = new EvntDispMarketViewModel(RouteEvent);
          //  _evntDispMarketViewModel.BindCommands(RouteEvent);

            TerminalViewModel = new TerminalViewModel();


            //1. Register relation type_of_win - type_of_view_model type here
            _dictKTypeWindowVTypeViewModelChild[typeof(PosLogWindow)] =
                                                typeof(ViewModelPosLog<CUserPosLogUpdate, CUserPosLog, ControlPosLog>);

            _dictKTypeWindowVTypeViewModelChild[typeof(DealsLogWindow)] =
                                               typeof(ViewModelDealsLog<CUserDealsLogUpdate,CUserDeal,ControlDealsLog>);


            _dictKTypeWindowVTypeViewModelChild[typeof(VMWindow)] =
                                                    typeof(ViewModelVM);

            _dictKTypeWindowVTypeViewModelChild[typeof(VMWindow)] =
                                                    typeof(ViewModelVM);


            _dictKTypeWindowVTypeViewModelChild[typeof(DetailedVMWindow)] =
                                                   typeof(ViewModelDetailedVM);



            _dictKTypeWindowVTypeViewModelChild[typeof(ConnectionsWindow)] =
                                                   typeof(ViewModelConnection);


            _dictKTypeWindowVTypeViewModelChild[typeof(AuthWindow)] =
                                                   typeof(ViewModelAuth);


            _dictKTypeWindowVTypeViewModelChild[typeof(AvailableTickersWindow)] =
                                                   typeof(ViewModelAvailableTickers);


            //_dictKTypeWindowVTypeViewModelChild[typeof(SettingsTerminalWindow)] =
                               //                    typeof(ViewModelSettings);

            _dictKTypeWindowVTypeViewModelChild[typeof(MoneyWindow)] =
                                                    typeof(ViewModelMoney);
            
            //2. Register view models here and create it instance
            //   all VM must be child of BaseViewModelChildWindow
            _dictKViewModelVView[new ViewModelPosLog<CUserPosLogUpdate, CUserPosLog, ControlPosLog>()] = null;
            _dictKViewModelVView[new ViewModelDealsLog<CUserDealsLogUpdate,CUserDeal,ControlDealsLog>()] = null;
            _dictKViewModelVView[new ViewModelVM()] = null;
            _dictKViewModelVView[new ViewModelDetailedVM()] = null;
            _dictKViewModelVView[new ViewModelConnection(_kernelTerminal)] = null;
            _dictKViewModelVView[new ViewModelAuth()] = null;
            _dictKViewModelVView[new ViewModelAvailableTickers()] = null;
          //  _dictKViewModelVView[new ViewModelSettings()] = null;
            _dictKViewModelVView[new ViewModelMoney()] = null;


            //3. Register relation type_of_data - type_of_view model here
            _dictKTypeDataVTypeBaseViewModelChild[typeof(CUserPosLogUpdate)] =
                                            typeof(ViewModelPosLog<CUserPosLogUpdate, CUserPosLog, ControlPosLog>);
            
            //added 2018-04-16 note in reality we use CUserPosUpdLate here
            _dictKTypeDataVTypeBaseViewModelChild[typeof(CUserPosLogUpdLate)] =
                                           typeof(ViewModelPosLog<CUserPosLogUpdate, CUserPosLog, ControlPosLog>);


            _dictKTypeDataVTypeBaseViewModelChild[typeof(CUserDealsLogUpdate)] =
                                           typeof(ViewModelDealsLog<CUserDealsLogUpdate,CUserDeal,ControlDealsLog>);


            _dictKTypeDataVTypeBaseViewModelChild[typeof(CUserVMUpdate)] =
                                            typeof(ViewModelVM);


            _dictKTypeDataVTypeBaseViewModelChild[typeof(List<CUserVmInstrumentRecord>)] =
                                           typeof(ViewModelDetailedVM);


            _dictKTypeDataVTypeBaseViewModelChild[typeof(List<CServer>)] =
                                          typeof(ViewModelConnection);

            _dictKTypeDataVTypeBaseViewModelChild[typeof(CAuthResponse)] =
                                         typeof(ViewModelAuth);


            _dictKTypeDataVTypeBaseViewModelChild[typeof(CAvailableTickers)] =
                                         typeof(ViewModelAvailableTickers);

            //_dictKTypeDataVTypeBaseViewModelChild[typeof(CSettingsVisualTerminal)] =
              //                          typeof(ViewModelSettings);



            _dictKTypeDataVTypeBaseViewModelChild[typeof(CUserMoney)] =
                                                        typeof(ViewModelMoney);




            BindChildViewModelKernelTerminal();
        }

        public BaseViewModelChildWin GetViewModelChilInst<T>()
        {
            BaseViewModelChildWin res = null;
            foreach (var kvp in _dictKViewModelVView)
            {
                if(kvp.Key.GetType() ==  typeof (T))                
                    res = kvp.Key;
                
               
            }
            return res;

        }



        public  static TerminalViewModel GetTerminalViewModel()
        {
            return CKernelTerminal.GetViewModelDispatcherInstance().TerminalViewModel;


        }
        public static void BindTerminalViewModel(Window win)
        {

             win.DataContext =  GetTerminalViewModel();


        }

        public void BindChildViewModelKernelTerminal()
        {

            //foreach (var kvp in _dictKViewModelVView)
                //kvp.Key.KernelTerminal = _kernelTerminal;
              //  kvp.Key.BindToSystem(_kernelTerminal);

            _dictKViewModelVView.Keys.ToList().
                ForEach(a => a.BindToSystem(_kernelTerminal));



        }

        public void Error(string msg, Exception e=null)
        {

            CKernelTerminal.ErrorStatic(msg, e);
            //_alarmer.Error(msg, e);

        }



        public void DeleteMarketViewModel(string ticker)
        {
            lock (_dictKIsinVMarketViewModel)
                if (ticker != null && _dictKIsinVMarketViewModel.ContainsKey (ticker))
                    _dictKIsinVMarketViewModel.Remove(ticker);
           
        }


        public int GetConnectionIdOfMarketViewModel(string ticker)
        {
            lock (_dictKIsinVMarketViewModel)
                return _dictKIsinVMarketViewModel[ticker].ConnId;
           
        }

        /*
        public void UpdatePosLog(CUserPosLogUpdate userPosLogUpdate)
        {
            _viewModelPosLog.UpdatePosLog(userPosLogUpdate);
        }
        */


        public void StubSetViewModelConnectionId(int connId)
        {
            lock (_dictKIsinVMarketViewModel)
                 _dictKIsinVMarketViewModel.
                    ToList().ForEach(a => a.Value.ConnId = connId);

        }


        /// <summary>
        /// Recieve data from data reciever and route to specific
        /// view model
        /// 
        /// Call from CDataReciever
        /// </summary>
        /// <param name="data"></param>
        /// <param name="connId"></param>
        public void Update(object data, int connId)
        {
         


            Type viewModelType = _dictKTypeDataVTypeBaseViewModelChild[data.GetType()];

            var v = _dictKViewModelVView.FirstOrDefault(a => a.Key.GetType() == viewModelType);

           if (v.Key == null)
               throw new Exception("UpdateData not found !");

           
           v.Key.UpdateData(data, connId);

           


        }





        public void UpdateUserDealsLog(CUserDealsLogUpdate userDealsUpdate)
        {
            //_viewModelPosLog.UpdateDealsLog(userDealsUpdate);
            
        }



       public void OpenViewChildOnStart(string typeOfWin)
       {

          
        //find type of view model for this type of win
           Type typeViewModel = _dictKTypeWindowVTypeViewModelChild.
                    FirstOrDefault(a => a.Key.ToString() == typeOfWin).Value;

           if (typeViewModel == null)
               return;


           //than find view model instance
           BaseViewModelChildWin viewMod = _dictKViewModelVView.FirstOrDefault(a => a.Key.GetType() == typeViewModel).Key;
       
           Type type = Type.GetType(typeOfWin + ", Terminal");

           //than create win  
           _dictKViewModelVView[viewMod] = (Window)Activator.CreateInstance(type);
       

           //
           viewMod.RegisterWindow(_dictKViewModelVView[viewMod]);

           Window win = _dictKViewModelVView[viewMod];

           CUtilWin.ShowActivated(ref win);


           win.Closed += new EventHandler(OnChildWindowClosed);
         //  return win;

           
       }


        /// <summary>
        /// Get child window of given type. If such window is
        /// not exist create new one
        /// 
        /// Call from
        /// 
       /// OpenChildWindow
        /// </summary>       
        public Window GetViewChildInstance<TChildWindow>() where TChildWindow : Window
        {
            //if window is alredy exist just get it
            var res =_dictKViewModelVView.
                        FirstOrDefault(a =>  a.Value!=null && a.Value.GetType()== typeof(TChildWindow));

            //if exist no need to create it
            if (res.Key != null)
                return _dictKViewModelVView[res.Key];
           
            //if not need to create it

           
          //first find type of view model for this type of win
            Type typeViewModel = _dictKTypeWindowVTypeViewModelChild.
                    FirstOrDefault(a => a.Key == typeof(TChildWindow) ).Value;
            
            //than find view model instance
             BaseViewModelChildWin viewMod =   _dictKViewModelVView.FirstOrDefault(a => a.Key.GetType() == typeViewModel).Key;

            

            //than create win and return it 
             _dictKViewModelVView[viewMod] = (TChildWindow)Activator.CreateInstance(typeof(TChildWindow));


             viewMod.RegisterWindow(_dictKViewModelVView[viewMod]);

             return _dictKViewModelVView[viewMod];
                            
        }



        /// <summary>
        /// Get (creates if not exist) child window and 
        /// show it activated (if need)
        /// 
        /// 
        ///Call from  
        ///
        ///KernelTerminal.OpenChildWindow
        ///MarketViewModel.ShowChangeInstrumentWindow
        ///ConnectionWindow.OpenAuthWindow
        ///VmWindow.OpenDetailedVM        
        /// </summary>      
        public Window OpenChildWindow<TChildWindow>(bool showAtStartup=true) 
                                                where TChildWindow : Window 
        {
            try
            {
                Window win = GetViewChildInstance<TChildWindow>();

                if (showAtStartup)
                    CUtilWin.ShowActivated(ref win);


                win.Closed += new EventHandler(OnChildWindowClosed);
                return win;
            }
            catch (Exception e)
            {
                Error("OpenchildWindow", e);
                return null;
            }
        }

        public void OnChildWindowClosed(object sender, EventArgs e)
        {
            try
            {
                UnRegisterWindow((Window)sender);
            }

            catch (Exception exc)
            {

                Error("CViewModelDispatcher", exc);

            }



        }

        public void OnAuthoriseSuccessServer(int conId)
        {

            System.Threading.Tasks.Task task = 
                new System.Threading.Tasks.Task(() =>DelayedSetConnectionToServer(conId));
            task.Start();


        }


        private DateTime _dtConnected;
        private int _parSecCheckAfterConnection = 10;//2018-04-09 changed to 10 was 5;

        private void DelayedSetConnectionToServer(int conId)
        {

            _dtConnected = DateTime.Now;

            while ((DateTime.Now - _dtConnected).TotalSeconds < _parSecCheckAfterConnection)
            {
                lock (_dictKIsinVMarketViewModel)                                
                    foreach (var kvp in _dictKIsinVMarketViewModel)
                    {
                        MarketViewModel mvm = kvp.Value;
                        if (mvm.ConnId == conId)
                        {
                            if (!mvm.IsConnectedToServer)
                                mvm.IsConnectedToServer = true;

                        }


                    }
            System.Threading.Thread.Sleep(100);
           }
        }






		/// <summary>
		/// Reset KeyboardTradingMode for all stocks
		/// except off stockNumExc
		/// 
		/// Call from:
		/// MarkertViewModel.SetKeyboardTradingMode
		/// </summary>	
		public void ResetAllKeyBoardTradingExcpt(int stockNumExc)
		{
            lock (_dictKIsinVMarketViewModel)
			    foreach (var kvp in _dictKIsinVMarketViewModel)
    				if (kvp.Value.StockNum != stockNumExc)				
	    				kvp.Value.ResetKeyboardTradingMode();
							
		}


		/// <summary>
		/// Call when Connected stock need replace with the new one
		/// 1) Find VieModel with oldTicker, reset IsAlive (stops network processing 
		/// data thread)
		/// 2) Remove old ticker MarketViewModel from dictKIsinVMarketViewModel
		/// 3) Creates new ticker MarketViewModel and add it to _dictKIsinVMarketViewModel
		/// 
		/// Call from CKernelTerminal.EditConnectedStock
		/// </summary>	
        public void ReplaceMarketViewModel(int stockNum,  string oldTicker, CStocksVisual stockVisual, 
                                            ControlMarket controlMarket, CInstrumentConfig instrumentConfig)
        {
            lock (_dictKIsinVMarketViewModel)
            {
                var res = _dictKIsinVMarketViewModel.Values.FirstOrDefault(a => a.ControlMarket.StockNum == stockNum); //Where(key => key.StockNum == stockNum);

                if (res != null)
                {
                    res.IsAlive = false;
                    _dictKIsinVMarketViewModel.Remove(oldTicker);

                    _dictKIsinVMarketViewModel[stockVisual.Ticker] = new MarketViewModel(stockNum, controlMarket, _kernelTerminal,
                                                                                         stockVisual, instrumentConfig);
                    //upd 2017_08_27
                    // _dictKIsinVMarketViewModel[stockVisual.Ticker].IsConnectedToServer = true;

                }
            }
         
        }




      

        public void UnRegisterWindow(Window win)
            
        {
            var res = _dictKViewModelVView .FirstOrDefault(a => (a.Value == win));
            if (res.Key != null)
            //  throw 
            //    (new Exception("Unable unregister. Window not found"));
            {
                res.Key.UnRegisterWindow();
                _dictKViewModelVView[res.Key] = null;
            }
        }





        /// <summary>
        /// Creates new MarketViewModel instance and adds it to dictioanary with viewmodels
		/// 
        /// Call from  
		/// 1) CKernelTerminal.AddOneStockFromConfig
		/// 2) CKernelTerminal.AddEmptyStock
        /// </summary>           
        public void AddMarketViewModelNew(int stockNum, ControlMarket controlMarket,  CStocksVisual stockVisual, CInstrumentConfig instrumentConfig)
        {
            //TODO access using unique id of market
            try
            {
                lock(_dictKIsinVMarketViewModel)
                    _dictKIsinVMarketViewModel.Add(stockVisual.Ticker,
                                                new MarketViewModel(stockNum, controlMarket, _kernelTerminal, stockVisual, instrumentConfig));
            }
            catch (Exception e)

            {
                Error("AddMarketViewModelNew",e);

            }


        }




        /// <summary>
        /// 
        /// Called from CKernelTermina.EditNonConnectedStock
        /// </summary>       
        public void MakeEmptyViewModelNonEmpty(string ticker, MarketViewModel marketViewModel)
        {
            //upd 2016_12_23_01
            //_dictKIsinVMarketViewModel[ticker] = marketViewModel;
            lock (_dictKIsinVMarketViewModel)
            {
                if (!_dictKIsinVMarketViewModel.ContainsKey(Literals.Undefind))
                    throw (new ApplicationException("MakeEmptyViewModelNonEmpty must be undefined but not found"));

                _dictKIsinVMarketViewModel.Remove(Literals.Undefind);
                _dictKIsinVMarketViewModel[ticker] = marketViewModel;
            }

        }



        public MarketViewModel GetMarketViewModel(string ticker)
        {
            lock (_dictKIsinVMarketViewModel)
            {
                if (!_dictKIsinVMarketViewModel.ContainsKey(ticker))
                    return null;

                return _dictKIsinVMarketViewModel[ticker];
            }
        }

        public MarketViewModel GetMarketViewModel(int stockNum)
        {

            lock (_dictKIsinVMarketViewModel)
                foreach (var kvp in _dictKIsinVMarketViewModel)
                    if (kvp.Value.StockNum == stockNum)
                        return kvp.Value;


               return null;
        }

        public List<int> GetConIdLstWithOpenedPos()
        {
            List<int> _lst = new List<int>();
            lock (_dictKIsinVMarketViewModel)
                foreach (var kvp in _dictKIsinVMarketViewModel)
                {
                    MarketViewModel mvm = kvp.Value;
                    if (mvm.VMUserPos !=null && mvm.VMUserPos.Amount!=0)
                        if (!_lst.Contains(mvm.ConnId))
                            _lst.Add(mvm.ConnId);
                }

            return _lst;
        }


        public List <int> GetConIdWithOrders()
        {

            List<int> _lst = new List<int>();

            lock (_dictKIsinVMarketViewModel)
                foreach (var kvp in _dictKIsinVMarketViewModel)
                {
                    MarketViewModel mvm = kvp.Value;
                    for (int i = 0; i < mvm.Orders.Length; i++)

                    //non-zero order found
                        if (mvm.Orders[i].Amount != 0)
                        {
                       
                            if (!_lst.Contains(mvm.ConnId))
                            {
                                _lst.Add(mvm.ConnId);
                                break;
                            }
                    }
                    else if (mvm.Orders[i].Amount == 0) //no more non-zero orders in array[]
                        break;
                    

            }
                        

                return _lst;
        }





        public bool IsMarketViewModelExist(string isin)
        {
            lock (_dictKIsinVMarketViewModel)
            {
                if (_dictKIsinVMarketViewModel.ContainsKey(isin))
                    return true;

                return false;
            }
        }

        public void OnClose()
        {


            foreach (var kvp in _dictKIsinVMarketViewModel)
                kvp.Value.OnClose();



        }

   



        public void RouteEvent(object sender, ExecutedRoutedEventArgs e)
        {
            int stockNum = ((IStockNumerable)sender).StockNum;
            MarketViewModel mvm = GetMarketViewModel(stockNum);
            if (mvm == null)
            {
                Error("CViewModelDispatcher.RouteEvent. MarketViewModel not found");
                return;
            }

            mvm.RouteEvent(sender, e);
            /*string t = ((System.Windows.Input.RoutedUICommand)e.Command).Name;
            if (t == "CmdAddOrder")
                mvm.OnAddOrder(sender, e);
            */

               
            
        }

       



    }
}
