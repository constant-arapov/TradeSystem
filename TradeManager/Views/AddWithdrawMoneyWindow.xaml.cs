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


using TradingLib.Enums;

using TradeManager.Commands;
using TradeManager.Commands.Data;

using TradeManager.ViewModels;

namespace TradeManager.Views
{
	/// <summary>
	/// Логика взаимодействия для AddWithdrawMoney.xaml
	/// </summary>
	public partial class AddWithdrawMoneyWindow : Window
	{

		private EnmAccountOperations _opCode;

		public AddWithdrawMoneyWindow(EnmAccountOperations opCode) : this()
		{
			_opCode = opCode;
			
		}



		public AddWithdrawMoneyWindow()
		{
			InitializeComponent();
		}


		private void ButtonSet_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				//TODO validation etc
				/*decimal newLim = Convert.ToDecimal(TextBoxNewVMLim.Text);
				CCmdDataMaxLossVM cmd = new CCmdDataMaxLossVM() { TradersLims = (VMTradersLimits)this.DataContext, NewLim = newLim };
				CCommands.CmdSetMaxLossVM.Execute(cmd, this);
				Close();*/


				decimal newVal = Convert.ToDecimal(TextBoxInput.Text);
				VMAvailableMoney am = (VMAvailableMoney)  this.DataContext;

				CCmdDataAddWithdrawMoney cmdData = new CCmdDataAddWithdrawMoney
				{
					ServerId = am.ServerId,
					BotId = am.id,
					MoneyChanged = newVal,
					Operation_code = _opCode


				};

				CCommands.CmdAddWithdrawMoney.Execute(cmdData, this);
				Close();

			}
			catch (Exception exc)
			{
				Error("AddWithdrawMoney",exc);
			}
		}

		private void Error(string msg, Exception e = null)
		{
			App.ErrorStatic(msg, e);
		}



		private void ButtonCancell_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}



	}
}
