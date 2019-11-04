using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


using TradeManager.Commands;
using TradeManager.Commands.Data;
using TradeManager.ViewModels;


namespace TradeManager.Views
{
    /// <summary>
    /// Interaction logic for ProcProfitWindow.xaml
    /// </summary>
    public partial class ProcProfitWindow : Window
    {
        public ProcProfitWindow()
        {
            InitializeComponent();
        }


        private void ButtonSet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //TODO validation etc
                decimal newLim = Convert.ToDecimal(TextBoxNewVal.Text.Replace('.',','));
                CCmdDataProcProfit cmd = new CCmdDataProcProfit()
                {
                    TradersLims = (VMTradersLimits)this.DataContext,
                    NewLim = newLim,
                    ServerId = ((VMTradersLimits)this.DataContext).ServerId
                };
                 CCommands.CmdChangeProcProfit.Execute(cmd, this);
                Close();
            }
#pragma warning disable CS0168 // Переменная "exc" объявлена, но ни разу не использована.
            catch (Exception exc)
#pragma warning restore CS0168 // Переменная "exc" объявлена, но ни разу не использована.
            {
                //TODO error
            }
        }

        private void ButtonCancell_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
