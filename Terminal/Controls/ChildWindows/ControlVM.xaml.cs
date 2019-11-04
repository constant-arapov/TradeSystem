using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using TradingLib;
using TradingLib.ProtoTradingStructs;

using Terminal.ViewModels.ChildWindows;


namespace Terminal.Controls.ChildWindows
{
    /// <summary>
    /// Логика взаимодействия для ControlVM.xaml
    /// </summary>
    public partial class ControlVM : UserControl
    {


        public   delegate void delegOpenDetailedVM();

        public delegOpenDetailedVM DelOpenDetailedVM ;

        private bool _bSelIndexChanged = false;


        public ControlVM()
        {
            InitializeComponent();
            DataContext = this;

            this.VM.PreviewMouseUp += VM_PreviewMouseUp;

        }

        private void VM_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (DelOpenDetailedVM != null)
                DelOpenDetailedVM();
        }

        private void VM_TargetUpdated(object sender, DataTransferEventArgs e)
        {
           /* DataGrid dg = (DataGrid)sender;

            if (dg != null)
            {
                CUserVMStockRecord sr = (CUserVMStockRecord)dg.Items[0];
                if (sr != null)
                {
                    if (sr.TotalVM < 0)
                    {
                       // dg.Items.
                        

          //              System.Threading.Thread.Sleep(0);
                    }

                }
            }
            */

        }

     
       



        private void VM_SourceUpdated(object sender, DataTransferEventArgs e)
        {
           
        }



        private void VM_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

          //  System.Threading.Thread.Sleep(0);

            //TODO activate !
        

        }

        private void VM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         //   System.Threading.Thread.Sleep(0);

            DataGrid dg = (DataGrid)sender;

            int ind = dg.SelectedIndex;

            if (ind >= 0)
            {
                var el =  (ModelStockRecord) dg.SelectedItem;

           
                ViewModelDetailedVM vm = (ViewModelDetailedVM)
                                CKernelTerminal.GetViewModelDispatcherInstance().GetViewModelChilInst<ViewModelDetailedVM>();

                vm.ConIdToShow = el.ConId;


             }



          
            

        }

      
    }


  

 


}
