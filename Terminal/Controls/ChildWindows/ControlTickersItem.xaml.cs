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
    /// Логика взаимодействия для ControlTickersItem.xaml
    /// </summary>
    public partial class ControlTickersItem : UserControl
    {
        public ControlTickersItem()
        {
            InitializeComponent();
            this.DataContext = this;
            this.DatagridTickers.DataContext = this;
        }
    }
}
