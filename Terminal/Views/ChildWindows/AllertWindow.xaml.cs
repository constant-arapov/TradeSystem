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
    /// Логика взаимодействия для ErrorWindow.xaml
    /// </summary>
    public partial class AllertWindow : Window
    {
        public AllertWindow(string Message)
        {
            InitializeComponent();
            this.LabelError.Text = Message;
            //this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowStyle = System.Windows.WindowStyle.ToolWindow;
            this.ButtonOK.Click += new RoutedEventHandler(ButtonOK_Click);
        }

        public void ShowWindowOnCenter()
        {

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            this.ShowDialog();

        }



        void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
