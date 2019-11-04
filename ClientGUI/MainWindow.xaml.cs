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
using System.Threading;

using Common;


using TCPLib;

namespace ClientGUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //TODO model viewmodel etc

       
        CModelMarket modelMarket = new CModelMarket();

        CGlobalConfig GlobalConfig = new CGlobalConfig();


        private void ThreadTCPClient()
        {
           

            CTCPClient cl = new CTCPClient("127.0.0.1", 81, modelMarket);

         

            while (!cl.ConnectToServer())
                Thread.Sleep(1000);

        }


        public void OnWindowClosing(object sender, EventArgs e)
        {
            //TODO normal exit
            System.Diagnostics.Process.GetCurrentProcess().Kill();

        }


        
        public MainWindow()
        {
            InitializeComponent();
            Closing += OnWindowClosing;

            string pathToConfig = CUtil.GetConfigDir() + @"\globalSettings.xml";     //System.AppDomain.CurrentDomain.BaseDirectory + @"config\globalSettings.xml";
            CGlobalConfig GlobalConfig = new CGlobalConfig(pathToConfig);
            CSerializator.Read<CGlobalConfig>(ref GlobalConfig);


          
            (new Thread(ThreadTCPClient)).Start();


            Stock.ModelMarket = modelMarket;

            Stock.SetParameters(10);            
            Stock.CreateStockRecords();
        }





    }
}
