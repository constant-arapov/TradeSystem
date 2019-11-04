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

namespace Terminal.Views.ChildWindows
{
    /// <summary>
    /// Логика взаимодействия для AllertWindowYesNo.xaml
    /// </summary>
    public partial class AllertWindowYesNo : Window
    {


        public bool YesClicked = false;
        public bool NoClicked = false;


        public AllertWindowYesNo(string message)
        {
            InitializeComponent();
            LabelError.Text = message;

        }

        public void  ShowWindowOnCenter()
        {

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            this.ShowDialog();

        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            YesClicked = true;
            Close();
        }

        private void ButtoNo_Click(object sender, RoutedEventArgs e)
        {
            NoClicked = true;
            Close();
        }

    }
}
