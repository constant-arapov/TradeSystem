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


using TradingLib.Common;

using TradeManager.Commands;
using TradeManager.Commands.Data;
using TradeManager.ViewModels;


namespace TradeManager.Views
{
    /// <summary>
    /// Логика взаимодействия для AddInstrument.xaml
    /// </summary>
    public partial class AddInstrumentWindow : Window
    {
        public AddInstrumentWindow()
        {
            InitializeComponent();

             
        }

        private void ButtonSet_Click(object sender, RoutedEventArgs e)
        {

            if (ComboboxServerDB.SelectedIndex == -1 || 
                ComboboxStockExch.SelectedIndex == -1)
            {
                //TODO error message etc
                Close();
                return;
            }


          
            CCmdDataAddInstrument data = new CCmdDataAddInstrument
            {
                ServerId = ((VMDBCon)ComboboxServerDB.Items[ComboboxServerDB.SelectedIndex]).ServerId,
                StockExchId =  ((VMStockExch)ComboboxStockExch.Items[ComboboxStockExch.SelectedIndex]).StockExchId,                
                Instrument = TextBoxInstrument.Text
            };



            CCommands.CmdAddInstrument.Execute(data, this);
            Close();
        }

        private void ButtonCancell_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
