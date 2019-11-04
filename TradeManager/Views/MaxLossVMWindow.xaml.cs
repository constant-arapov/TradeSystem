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


using TradeManager;
using TradeManager.Commands;
using TradeManager.Commands.Data;
using TradeManager.ViewModels;

namespace TradeManager.Views
{
	/// <summary>
	/// Логика взаимодействия для MaxLossVMWindow.xaml
	/// </summary>
	public partial class MaxLossVMWindow : Window
	{
		public MaxLossVMWindow()
		{
			InitializeComponent();
		}

		private void ButtonSet_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				//TODO validation etc
				decimal newLim = Convert.ToDecimal(TextBoxNewVMLim.Text.Replace('.',','));
				CCmdDataMaxLossVM cmd = new CCmdDataMaxLossVM() { TradersLims = (VMTradersLimits) this.DataContext, NewLim = newLim,
																  ServerId = ((VMTradersLimits)this.DataContext).ServerId
				};
				CCommands.CmdSetMaxLossVM.Execute(cmd, this);
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
