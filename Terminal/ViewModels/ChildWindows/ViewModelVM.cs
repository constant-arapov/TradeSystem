using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using System.Collections.ObjectModel;


using TradingLib;
using TradingLib.ProtoTradingStructs;

using Terminal.Controls.ChildWindows;

using Terminal.Views.ChildWindows;


namespace Terminal.ViewModels.ChildWindows
{
    public class ViewModelVM : BaseViewModelChildWin
    {


        public List<ModelStockRecord> ListStockRecords { get; set; }
        private VMWindow _winVM { get; set; }

        public ViewModelVM()
        {
            ListStockRecords = new List<ModelStockRecord>();

            
         
        }

        public override void UpdateData(object data, int conId)
        {
            base.UpdateData(data, conId);


            CUserVMUpdate vm = (CUserVMUpdate)data;
            CUserVMStockRecord vmStockRec = vm.VMStockRecord;
            var res = ListStockRecords.FirstOrDefault (a => a.StockName == vmStockRec.StockName);
            
            if (res == null)
                ListStockRecords.Add(new ModelStockRecord
                                    {
                                        StockName = vmStockRec.StockName,
                                        TotalVM = vmStockRec.TotalVM,
                                        ConId = conId
                                     });
            else
            {
                res.TotalVM = vmStockRec.TotalVM;
              
            }
            BindGrid();

        }


        protected override void CreateControls()
        {

             _winVM = (VMWindow)_view;
             if (ListStockRecords.Count == 0)
                 _winVM.VMControlDatagrid.Visibility = Visibility.Hidden;
             else
                 _winVM.VMControlDatagrid.Visibility = Visibility.Visible;

             BindGrid();
           

        }

        private void BindGrid()
        {
            if (_winVM != null)
            {
                _winVM.GUIDispatcher.BeginInvoke(
                     new Action(() =>
                         {
                             try
                             {
                                 if (ListStockRecords.Count > 0)
                                     _winVM.VMControlDatagrid.Visibility = Visibility.Visible;

                                 _winVM.VMControlDatagrid.VM.ItemsSource = ListStockRecords;
                                 _winVM.VMControlDatagrid.VM.Items.Refresh();
                             }
                             catch (Exception e)
                             {
                                 Error("ViewModelVm.BindGrid", e);
                             }
                         }

                    ));

            }
        }






    }
}
