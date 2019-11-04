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

using System.Globalization;

using System.Data;
using TradingLib;


namespace Terminal.Controls.ChildWindows
{
    /// <summary>
    /// Логика взаимодействия для ControlPosLog.xaml
    /// </summary>
    /// 

    public partial class ControlPosLog : UserControl, IDataControl
    {



        public string TickerName
        {
            get { return (string)GetValue(TickerNameProperty); }
            set { SetValue(TickerNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TickerName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TickerNameProperty =
            DependencyProperty.Register("TickerName", typeof(string), typeof(ControlPosLog), new UIPropertyMetadata(""));

        

        public ControlPosLog()
        {
            InitializeComponent();
            this.DataContext = this;


        }

        public DataGrid DatagridData
        {
            get
            {
                return PosLog;

            }
            set
            {
                PosLog = value;
            }



        }


        private void PosLog_LoadingRow(object sender, DataGridRowEventArgs e)
        {

     
           /* CUserPosLog pl =  (CUserPosLog) e.Row.Item;


            if (pl.VMClosed_RUB <= 0)
                e.Row.Background = Brushes.Red;
            else

                e.Row.Background = Brushes.Green;
           */
            
        }

       


    }

    public class CutoffConverter : IValueConverter 
    {
   public  object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
   {
        return ((int)value) >= 0;
    }

   public  object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      //  throw new NotImplementedException();
       return null;

    }

    //public int Cutoff { get; set; }
}


}
