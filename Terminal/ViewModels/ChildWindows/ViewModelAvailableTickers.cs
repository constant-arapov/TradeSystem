using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;


using System.Collections.ObjectModel;


using TradingLib.ProtoTradingStructs;

using Common;
using Common.Utils;

using TCPLib;


using Terminal.Views;

using Terminal.Conf;
using Terminal.Views.ChildWindows;
using Terminal.Controls.ChildWindows;


namespace Terminal.ViewModels.ChildWindows
{
    public class ViewModelAvailableTickers : BaseViewModelChildWin
    {

        AvailableTickersWindow _winAvailTicks;
        //List<string> _lstAvailTicks;

        Dictionary <int, ObservableCollection<CTIckerData>>  _dictKConnVTickers = new Dictionary<int,ObservableCollection<CTIckerData>>();

        List<DataGrid> _lstDataGrids = new List<DataGrid>();
       // CKernelTerminal _kernelTerminal;

        public override void UpdateData(object data, int connId)
        {
           
            

            CAvailableTickers avt = (CAvailableTickers)data;
    
            _dictKConnVTickers[connId] =    new ObservableCollection<CTIckerData> (  avt.ListAvailableTickers);
            
            BindTabItem(connId);
        }


        protected override void CreateControls()
        {

            _winAvailTicks = (AvailableTickersWindow)_view;


           // CKernelTerminal kernelterminal =  CKernelTerminal.GetKernelTerminalInstance();

            lock (KernelTerminal.Communicator.ServersConf)
            {
                int conNum = 0;
                foreach (CServer s in KernelTerminal.Communicator.ServersConf.Servers)
                {
                    
                    TabItem ti = new TabItem();
                    ti.Header = s.Name;
                   
                    


                    ControlTickersItem cti = new ControlTickersItem();
                    cti.DatagridTickers.Tag = conNum;
                    _lstDataGrids.Add(cti.DatagridTickers);
                    ti.Content = cti;
                   

                  //  ListBox lb = new ListBox();

                    cti.DatagridTickers.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(DatagridTickers_PreviewMouseDown);
                   // lb.MouseDown += new System.Windows.Input.MouseButtonEventHandler(lb_PreviewMouseDown);

                   // _lstListbox.Add(lb);
                   // ti.Content = lb;
                    
                      


                    _winAvailTicks.AvailTicks.TabControlAvailTicks.Items.Add(ti);
                    conNum++;
                }

            }

            

            foreach (var kvp in _dictKConnVTickers)            
                BindTabItem(kvp.Key);


            base.CreateControls();
        }

        void DatagridTickers_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                var dg = (DataGrid)sender;




                int conNum = (int)dg.Tag;

                int rowNum = CUtilWPF.GetDataGridClickedRowNum(e);

                CTIckerData tickerData = (CTIckerData)dg.Items[rowNum];
                string ticker = tickerData.TickerName;



                if (KernelTerminal.ViewModelDispatcher.IsMarketViewModelExist(ticker))
                {
                    AllertWindow win = new AllertWindow("Невозможно выбрать инструмент. Стакан с таким инструментом уже существует.");


                    win.Topmost = true;

                    Window wn = (Window)win;


                   // CUtilWin.ShowActivated(ref wn);




                    Window windowRoot = (Window)CUtilWin.FindWindow<AvailableTickersWindow>();
                    CUtilWin.ShowDialogOnCenter(wn, windowRoot);
                       
                    

                    //CUtilWin.ShowDialogOnCenter(ref wn, _);

                    //win.Show();

                    return;

                }

               //note put to method marketViewModel as parameters
                MarketViewModel currMarketViewModel = _winAvailTicks.CurrentMarketViewModel;
                KernelTerminal.ChangeMarketInstrument(conNum, tickerData, currMarketViewModel);
           

               // (new Task(KernelTerminal.TaskSaveVisualConf)).Start();

                _winAvailTicks.Close();

            }
            catch (Exception exc)
            {

                Error("DatagridTickers_PreviewMouseDown", exc);

            }
        
        }

        private void BindTabItem(int conId)
        {
            if (_winAvailTicks != null)
            {
         
                _winAvailTicks.GUIDispatcher.BeginInvoke(

                    new Action(() =>
                   {
                       lock (_lstDataGrids)
                       {
                           try
                           {
                               _lstDataGrids[conId].ItemsSource = _dictKConnVTickers[conId];
                               _lstDataGrids[conId].Items.Refresh();
                           }
                           catch (Exception e)
                           {
                               Error("ViewModelAvailableTickers.BindTabItem", e);

                           }



                       }
               


                   }



                    ));
                  
                
            }



        }

        public override void  UnRegisterWindow()
        {
            lock (_lstDataGrids)
                _lstDataGrids.Clear();
 	        base.UnRegisterWindow();
        }
        


        






    }
}
