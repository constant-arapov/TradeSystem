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

using TradingLib;


namespace Terminal.Controls.ChildWindows
{
    /// <summary>
    /// Логика взаимодействия для ControlDealsLog.xaml
    /// </summary>
    public partial class ControlDealsLog : UserControl, IDataControl
    {


        public string TickerName
        {
            get { return (string)GetValue(TickerNameProperty); }
            set { SetValue(TickerNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TickerName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TickerNameProperty =
            DependencyProperty.Register("TickerName", typeof(string), typeof(ControlDealsLog), new UIPropertyMetadata(""));


        public DataGrid DatagridData
        {
            get
            {
                return DealsLog;

            }
            set
            {
                DealsLog = value;
            }



        }



        public ControlDealsLog()
        {
            InitializeComponent();

            this.DataContext = this;

        }

        private void DealsLog_LoadingRow(object sender, DataGridRowEventArgs e)
        {

            /*
            CUserDeal pl = (CUserDeal)e.Row.Item;


            if (pl.BuySell == EnmDealDir.Sell)
                e.Row.Background = Brushes.LightCoral; //Brushes.Red;
            else

                e.Row.Background = Brushes.LimeGreen;// Brushes.Green;
            */

        }
    }
}
