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

using System.Windows.Controls.Primitives;


using Common;
using Common.Utils;


using TCPLib;

using Terminal.Conf;



namespace Terminal.Controls.ChildWindows
{
    /// <summary>
    /// Логика взаимодействия для ControlConnections.xaml
    /// </summary>
    public partial class ControlConnections : UserControl
    {


        public delegate  void delegOpenAuthWindow (int conNum,  string stockName);

        public delegOpenAuthWindow DelOpenAuthWindow;

        public ControlConnections()
        {
            InitializeComponent();

            this.DataContext = this;

        }

        private void ConnectionGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
            int rowNum = CUtilWPF.GetDataGridClickedRowNum(e);
            if (rowNum < 0)
                return;
            string name = ((CServer)this.ConnectionGrid.Items[rowNum]).Name;



            DelOpenAuthWindow(rowNum,name);
            


        }

        private int FindRowIndex(DataGridRow row)
        {
            DataGrid dataGrid =
                ItemsControl.ItemsControlFromItemContainer(row)
                as DataGrid;

            int index = dataGrid.ItemContainerGenerator.
                IndexFromContainer(row);

            return index;
        }



    }
}
