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


using System.Threading.Tasks;


using Common;

using Terminal.Views;
using Terminal.ViewModels;
using Terminal.DataBinding;

namespace Terminal.Views.ChildWindows
{
    /// <summary>
    /// Логика взаимодействия для ViewSettings.xaml
    /// </summary>
    public partial class SettingsTerminalWindow : Window
    {
        public SettingsTerminalWindow()
        {
            InitializeComponent();
        }


       


      

        private void Window_Closed(object sender, EventArgs e)
        {

            CKernelTerminal.SaveTerminalProperties();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ListBackGroundColors.ItemsSource = typeof(Colors).GetProperties();
        }

     

        private void ShowColorWindow(string bindColorPropertyName)
        {
            ColorSelectionWindow csw = new ColorSelectionWindow(bindColorPropertyName);
            csw.Left = this.Left - (csw.Width - this.Width);
            csw.Top = this.Top;
            csw.ShowDialog();


        }

        private void CallColorWindowClick(object sender, RoutedEventArgs e)
        {
            
            
           var prop =   BindingOperations.GetBinding((Button)sender, Button.BackgroundProperty);        
           ShowColorWindow(prop.Path.Path);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



    }
}
