using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input;
using Terminal.ViewModels;


namespace Terminal.Events
{
    class CEventRouterViewModel
    {
       Dictionary<RoutedUICommand,ExecutedRoutedEventHandler> _dictCmdEventHandler = 
           new Dictionary<RoutedUICommand,ExecutedRoutedEventHandler>();




        public  CEventRouterViewModel(MarketViewModel marketViewModel)
        {
            Add(EventsViewModel.CmdSizeChanged, marketViewModel.OnSizeChanged);
            Add(EventsViewModel.CmdAddOrder, marketViewModel.OnAddOrder);
            Add(EventsViewModel.CmdCancellOrdersWithPrice, marketViewModel.OnCancellOrdersWithPrice);
            Add(EventsViewModel.CmdCancellAllOrders, marketViewModel.OnCancellAllOrders);
            Add(EventsViewModel.CmdCloseAllPositions, marketViewModel.OnCloseAllPositions);
            Add(EventsViewModel.CmdCloseAllPositionsByIsin, marketViewModel.OnCloseAllPositionsByIsin);
            Add(EventsViewModel.CmdShowChangeInstrumentWindow, marketViewModel.OnShowChangeInstrumentWindow);
            Add(EventsViewModel.CmdDeleteInstrument, marketViewModel.OnDeleteInstrument);
            Add(EventsViewModel.CmdChangeTimeFrame, marketViewModel.OnChangeTimeFrame);
            Add(EventsViewModel.CmdSetStopLossTakeProfit, marketViewModel.SetStopLossTakeProfit);
            Add(EventsViewModel.CmdSetStopLossInvert, marketViewModel.SetStopLossInvert);
            Add(EventsViewModel.CmdSetStopOrder, marketViewModel.SetStopOrder);
            Add(EventsViewModel.CmdAddDelUserLevel, marketViewModel.AddDelUserLevel);
			Add(EventsViewModel.CmdSetKeyboardTrading, marketViewModel.SetKeyboardTradingMode);
			Add(EventsViewModel.CmdSendOrderThrow, marketViewModel.SendOrderThrow);
			Add(EventsViewModel.CmdInvertPosition, marketViewModel.InvertPosition);
            Add(EventsViewModel.CmdSaveInstrumentConfig, marketViewModel.OnSaveInstrumentConfig);
            Add(EventsViewModel.CmdSendRestOrder, marketViewModel.SendRestOrder);
            Add(EventsViewModel.CmdStockWidthChanged, marketViewModel.StockWidthChanged);
            Add(EventsViewModel.CmdClusterWidthChanged, marketViewModel.ClusterWidthChanged);
            Add(EventsViewModel.CmdIncreaseMinStep, marketViewModel.IncreaseMinStep);
            Add(EventsViewModel.CmdDecreaseMinStep, marketViewModel.DecreaseMinStep);




        }

        private void Add(RoutedUICommand cmd,ExecutedRoutedEventHandler handler)
        {
            _dictCmdEventHandler[cmd] = handler;

        }

        public void RouteEvent(object sender, ExecutedRoutedEventArgs e)
        {
			try
			{

				RoutedUICommand cmd = (RoutedUICommand)e.Command;

				if (!_dictCmdEventHandler.ContainsKey(cmd))
				{
					CKernelTerminal.ErrorStatic("CEventRouterViewModel.RouteEvent command not found");
					return;
				}
				_dictCmdEventHandler[cmd].Invoke(sender, e);
			}
			catch (Exception excp)
			{
				CKernelTerminal.ErrorStatic("Error in RouteEvent", excp);

			}


        }
    }
}
