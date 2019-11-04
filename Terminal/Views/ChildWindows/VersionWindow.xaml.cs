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

using Common;
using Common.Utils;


namespace Terminal.Views.ChildWindows
{
    /// <summary>
    /// Логика взаимодействия для VersionWindow.xaml
    /// </summary>
    public partial class VersionWindow : Window
    {
        public VersionWindow()
        {
            InitializeComponent();
            TextblockVersion.Text = "версия: " + CUtil.GetVersion();
            TextblockTime.Text = CUtil.GetBuildTime();
            this.KeyDown += VersionWindow_KeyDown;
        }

        private void VersionWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            Close();
        }
    }
}
