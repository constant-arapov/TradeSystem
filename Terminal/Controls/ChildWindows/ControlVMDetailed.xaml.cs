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

namespace Terminal.Controls.ChildWindows
{
    /// <summary>
    /// Логика взаимодействия для ControlVMDetailed.xaml
    /// </summary>
    public partial class ControlVMDetailed : UserControl//, IDataControl
    {
        /*
        public string TickerName
        {
            get { return (string)GetValue(TickerNameProperty); }
            set { SetValue(TickerNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TickerName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TickerNameProperty =
            DependencyProperty.Register("TickerName", typeof(string), typeof(ControlPosLog), new UIPropertyMetadata(""));


        public DataGrid DatagridData
        {
            get
            {
                return VMDetailed;

            }
            set
            {
                VMDetailed = value;
            }



        }

        */


        public ControlVMDetailed()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
