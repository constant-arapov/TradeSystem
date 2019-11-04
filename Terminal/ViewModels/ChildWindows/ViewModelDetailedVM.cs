using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;

using TradingLib;
using TradingLib.ProtoTradingStructs;

using Terminal.Views.ChildWindows;

namespace Terminal.ViewModels.ChildWindows
{
    public class ViewModelDetailedVM : BaseViewModelChildWin
    {
        //changed 2018-04-18
        public Dictionary<int, List<CUserVmInstrumentRecord>> DictStockRecords { get; set; }

        private DetailedVMWindow _winVM { get; set; }
        public int ConIdToShow = 0;

        public ViewModelDetailedVM()
        {

            DictStockRecords = new Dictionary<int, List<CUserVmInstrumentRecord>>();
        }

        public override void UpdateData(object data, int connId)
        {
            List<CUserVmInstrumentRecord> vm = (List<CUserVmInstrumentRecord>)data;

            if (!DictStockRecords.ContainsKey(connId))
                DictStockRecords[connId] = new List<CUserVmInstrumentRecord>();

            DictStockRecords[connId] = vm;
            BindGrid();
         
            /*
            if (DictStockRecords.Count == 0 || DictStockRecords[connId].Except(vm).Any()) //differs
            {
                DictStockRecords = vm;
                BindGrid();
            }
          */

            
           
            
           

            /*
            var res = ListStockRecords.FirstOrDefault(a => a.Isin == vm.Isin);
            if (res == null)
                ListStockRecords.Add(vm);
            else
            {

                res.VM = vm.VM;

            }
            */

        }


        protected override void CreateControls()
        {

            _winVM = (DetailedVMWindow)_view;
            BindGrid();


        }


        public void BindGrid()
        {

            if (_winVM != null)
            {
                _winVM.GUIDispatcher.BeginInvoke(
                     new Action(() =>
                     {
                         try
                         {  //changed 2018-03-28
                             if (DictStockRecords.ContainsKey(ConIdToShow))
                             {
                                 _winVM.VMControlDatagrid.VMDetailed.ItemsSource = DictStockRecords[ConIdToShow];
                                 _winVM.VMControlDatagrid.VMDetailed.Items.Refresh();
                             }
                         }
                         catch (Exception e)
                         {
                             Error("DetailedVMWindow.BindGrid", e);
                         }
                     }

                    ));
            }
        }
        





    }
}
