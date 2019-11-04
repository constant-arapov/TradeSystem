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
using Common.Utils;

using Terminal.Views.ChildWindows;

namespace Terminal.Views.ChildWindows
{
    /// <summary>
    /// Логика взаимодействия для ConnectionsWindow.xaml
    /// </summary>
    public partial class ConnectionsWindow : Window, IGUIDispatcherable
    {
        private CKernelTerminal _kernelTerminal;
        public Dispatcher GUIDispatcher { get; set; }


        


        public ConnectionsWindow()
        {
            InitializeComponent();

            GUIDispatcher = Dispatcher.CurrentDispatcher;

            ConnectionTable.DelOpenAuthWindow = OpenAuthWindow;
            this.WindowStyle = System.Windows.WindowStyle.ToolWindow;

            var win = CUtilWin.FindWindow<MainWindow>();

            if (win != null)
            {
                this.Left = win.Left + 20;
                this.Top = win.Top + 10;

            }
        }

        public void OpenAuthWindow(int connId, string  stockName)
        {
         
          
           _kernelTerminal = CKernelTerminal.GetKernelTerminalInstance();
           AuthWindow aw =  (AuthWindow) _kernelTerminal.ViewModelDispatcher.OpenChildWindow<AuthWindow>(false);
           aw.Left = this.Left+30;
           aw.Top = this.Top + 30;

           aw.SetParams(connId, stockName);
           Window win = (Window)aw;
           CUtilWin.ShowActivated(ref win);
         
            /*
            AuthWindow aw = (AuthWindow) _kernelTerminal.ViewModelDispatcher.GetViewChild<AuthWindow>();
            aw.SetParams(connId, stockName);
            aw.Closed += new EventHandler(aw_Closed);
            aw.ShowDialog();
         */
        }

     
    }
}
