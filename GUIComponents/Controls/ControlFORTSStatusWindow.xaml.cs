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

namespace GUIComponents.Controls
{
    /// <summary>
    /// Логика взаимодействия для ControlFORTSStatusWindow.xaml
    /// </summary>
    public partial class ControlFORTSStatusWindow : UserControl
    {
        public ControlFORTSStatusWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {


        }

        public void BindButtons(object objBind)
        {


            int cnt = VisualTreeHelper.GetChildrenCount(this.GridRoot);

            for (int i = 0; i < cnt; i++)
            {
                DependencyObject dob = VisualTreeHelper.GetChild(this.GridRoot, i);
                ((ControlFORTSButton)dob).AutoBind(objBind);

            }
        }
    }
}
