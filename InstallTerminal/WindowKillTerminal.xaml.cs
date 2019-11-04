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

using System.Diagnostics;


using Common.Utils;


namespace InstallTerminal
{
    /// <summary>
    /// Логика взаимодействия для WindowKillTerminal.xaml
    /// </summary>
    public partial class WindowKillTerminal : Window
    {
        private Action _continueInstall;

        public WindowKillTerminal(Action continueInstall)
        {
            InitializeComponent();
            _continueInstall = continueInstall;
        }

        private void ButtonCancell_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonTerminate_Click(object sender, RoutedEventArgs e)
        {
            //TODO normal window close
            Process proc = CUtil.GetProcess("Terminal");
           if (proc !=null)
           {
               proc.Kill();
           }
           Close();
           _continueInstall();
          


        }


     



    }
}
