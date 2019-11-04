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



using Common.Utils;

using ASTS.DealingServer;


namespace TradeSystemASTS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    ///        
    public partial class MainWindow : Window
    {

        CASTSDealingServer _dealingServer;
		System.Windows.Threading.Dispatcher _guiDisp;

        public MainWindow()
        {

           

           

            InitializeComponent();

			_guiDisp = System.Windows.Threading.Dispatcher.CurrentDispatcher;

     //      Closed += new EventHandler(MainWindow_Closed);


        //    CUtil.ThreadStart(ThreadFunc);

       //     _dealingServer = new CASTSDealingServer();


          


           // _dealingServer.GUIDispatcher  =  

            //_dealingServer.Process();

        }
        
        void MainWindow_Closed(object sender, EventArgs e)
        {
          //  _dealingServer.Disconnect();
            
        }

		private void MenuItem_ChangePassword(object sender, RoutedEventArgs e)
		{
			ChangePasswordWindow cwp = new ChangePasswordWindow();
			cwp.ShowDialog();
		}

        /*
        private void ThreadFunc()
        {

            _dealingServer = new CASTSDealingServer();

			_dealingServer.GUIBox.GUIDispatcher = _guiDisp;
           
            
            _dealingServer.Process();



        }
        */


    }
}
