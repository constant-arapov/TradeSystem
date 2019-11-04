using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Common.Utils;


namespace InstallTerminal
{
    /// <summary>
    /// Interaction logic for WindowUninstall.xaml
    /// </summary>
    public partial class WindowUninstall : Window, IClientUninstaller
    {

        CUnInstaller _uninstaller;

        System.Windows.Threading.Dispatcher _dispatcher;




        public WindowUninstall()
        {
            InitializeComponent();
            _dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
        }

        private void ButtonUninstall_Click(object sender, RoutedEventArgs e)
        {


            StackPanelProgressBar.Visibility = Visibility.Visible;
            StackPanelStartInfo.Visibility = Visibility.Hidden;
            ListBoxLog.Visibility = Visibility.Visible;


              _uninstaller = new CUnInstaller(this);

            _uninstaller.UnInstall();


        }

        public void OutMessage(string msg)
        {
            _dispatcher.BeginInvoke(
                    new Action(() =>
                        ListBoxLog.Items.Add(msg)));

        }

        public void UpdateProgressBar(int value)
        {
            _dispatcher.BeginInvoke(new Action(() =>
                  {
                      ProgressBarUninstall.Value = value;
                      TextBlockProgress.Text = value.ToString() + " %";
                    }
                    ));

        }


        public void CallBackErrorExit(string error)
        {
          _dispatcher.BeginInvoke(new Action(() =>
         {
             ShowErrorWin(error);
             Close();
         }));
        }

        private void ShowErrorWin(string error)
        {
            ErrorWin errWin = new ErrorWin();
            errWin.LabelError.Text = error;
            CUtilWin.ShowDialogOnCenter(errWin, this);

        }



        public void CallbackSuccess(string msg)
        {
            _dispatcher.BeginInvoke(new Action (() =>
                {
                SuccessWin succWin = new SuccessWin();
                succWin.TextBlockMessage.Text = msg;
                CUtilWin.ShowDialogOnCenter(succWin, this);
                succWin.Activate();
                Close();
            }));

        }

        public void OnStartUninstall()
        {
            _dispatcher.BeginInvoke(new Action(() =>
           {
               ButtonUninstall.IsEnabled = false;
           }
            ));

        }

        public void OnEndUninstall()
        {


        }



    }
}
