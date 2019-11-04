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


using Common;
using Common.Utils;

using TradingLib.Interfaces.Interaction;


using TradingLib.Interfaces;

namespace TradeSystem.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindowForInstrumentRec
    {
        public MainWindowViewModel MWVM { get; set; }

        public void OnWindowClosing(object sender, EventArgs e)
        {
            //TODO normal exit
            System.Diagnostics.Process.GetCurrentProcess().Kill();

        }


        public MainWindow()
        {


            Closing += OnWindowClosing;

            // LoadDynamics();
            InitializeComponent();

            MWVM = (MainWindowViewModel)this.FindResource("MainWindowViewModel");

            this.Title += " "+ CUtil.GetVersion();


        }

        private void MenuItem_StartClearing(object sender, RoutedEventArgs e)
        {

            MWVM.Plaza2Connector.ClearingProcessor.ProcessManualClearing();


        }







        public Action<object, RoutedEventArgs> GetTFButtonEventHandler()
        {

             return MWVM.OnTFButtonClick;
        }

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{

		}
        
      
    }
}
