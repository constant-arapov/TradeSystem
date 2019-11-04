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
using System.Windows.Shapes;


using System.Windows.Threading;

using Common;
using Common.Interfaces;

using Terminal;
using Terminal.ViewModels.ChildWindows;


namespace Terminal.Views.ChildWindows
{
    /// <summary>
    /// Логика взаимодействия для VMWindow.xaml
    /// </summary>
    public partial class VMWindow : BaseTerminalWindow, IGUIDispatcherable, IAlarmable
    {

        public Dispatcher GUIDispatcher { get; set; }

       // private CAlarmer _alarmer;

        public VMWindow()
        {
            InitializeComponent();


            GUIDispatcher = Dispatcher.CurrentDispatcher;
            
            this.VMControlDatagrid.DelOpenDetailedVM = OpenDetailedVM;
                                        
        }

        public void Error(string msg, Exception e)
        {
           // if (_alarmer !=null)
           // _alarmer.Error(msg, e);

            CKernelTerminal.ErrorStatic(msg, e);
        }



        public void OpenDetailedVM()
        {


            try
            {
                ViewModelDetailedVM vm = (ViewModelDetailedVM) 
                            CKernelTerminal.GetViewModelDispatcherInstance().GetViewModelChilInst <ViewModelDetailedVM>();

               // int ind = VMControlDatagrid.VM.SelectedIndex;


                CKernelTerminal.OpenChildWindow<DetailedVMWindow>();
                vm.BindGrid();

               /* CKernelTerminal kernTeminal = CKernelTerminal.GetKernelTerminalInstance();




                DetailedVMWindow win =
                   (DetailedVMWindow)kernTeminal.ViewModelDispatcher.OpenChildWindow<DetailedVMWindow>();

               
                win.Left = this.Left + 10;
                win.Top = this.Top + 10;
                */
              //  win.Left = this.Left - 100;

             //    win.Left = 2800;
              //  win.Top = 400;

                // win.Show();

            }

            catch (Exception e)
            {
                Error("VMWindow.OpenDetailedVM", e);

            }


        
        }
        public void Callback(Window win)
        {

            win.Left = this.Width + this.Left + 10;

        }


    

    }
}
