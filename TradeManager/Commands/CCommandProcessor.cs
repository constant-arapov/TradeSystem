using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

using TradeManager.Interfaces;


namespace TradeManager.Commands
{
	public class CCommandProcessor
	{

        ICommandToDataSource _commandToDataSource;

		public CCommandProcessor(ICommandToDataSource commandDataSource)
		{

            _commandToDataSource = commandDataSource;

			
            Add(CCommands.CmdUpdateAccount, _commandToDataSource.UpdateAccount);
			Add(CCommands.CmdDisableTrader, _commandToDataSource.DisableTrader);
			Add(CCommands.CmdEnalbeTrader, _commandToDataSource.EnableTrader);
			Add(CCommands.CmdSetMaxLossVM, _commandToDataSource.SetMaxLossVM);
			Add(CCommands.CmdAddWithdrawMoney, _commandToDataSource.AddWithDrawMoney);
            Add(CCommands.CmdCloseTraderPos, _commandToDataSource.CloseTraderPos);
            Add(CCommands.CmdAddInstrument, _commandToDataSource.AddInstrument);
            Add(CCommands.CmdDeleteInstrument, _commandToDataSource.DeleteInstrument);
            Add(CCommands.CmdChangeProcProfit, _commandToDataSource.ChangeProcProfit);
            Add(CCommands.CmdChangeProcDealingFee, _commandToDataSource.ChangeProfitDealingFee);
            Add(CCommands.CmdTraderAddFundsReq, _commandToDataSource.CompleteTrdAddFundsReq);
            Add(CCommands.CmdTraderWithdrawReq, _commandToDataSource.CompleteTrdWithdrawReq);
            Add(CCommands.CmdReconnect, _commandToDataSource.SendReconnect);
								
		}




		public void Add(RoutedUICommand cmd, ExecutedRoutedEventHandler handler)
		{
			//_dictCmdEventHandler[cmd] = handler;
			CreateCommandBinding(cmd, handler, DefaultCanDo);
		}

		private void CreateCommandBinding(RoutedUICommand command, ExecutedRoutedEventHandler execute,
										  CanExecuteRoutedEventHandler canExecute)
		{
			var binding = new CommandBinding(command,
											 execute,				
											 canExecute
											 );



			// Register CommandBinding for all windows.
			CommandManager.RegisterClassCommandBinding(typeof(Window), binding);


		}

		private static void DefaultCanDo(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;



		}

		public void OnCmdShowAddMoneyDialog(object sender, ExecutedRoutedEventArgs e)
		{
			System.Threading.Thread.Sleep(0);
		}

        




	}
}
