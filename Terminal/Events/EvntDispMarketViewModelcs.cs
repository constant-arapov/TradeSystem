using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Reflection;

using Common;

using Terminal.ViewModels;

namespace Terminal.Events
{
    public class EvntDispMarketViewModel
    {


        public EvntDispMarketViewModel(ExecutedRoutedEventHandler routeEvent)
        {


			Type type = Type.GetType("Terminal.Events.EventsViewModel");

			var v = this.GetType();
			foreach (var property in type.GetProperties())
			{

				RoutedUICommand evnt = (RoutedUICommand)property.GetValue(null, null);
				CreateCommandBinding(evnt, routeEvent, DefaultCanDo);

			}


        /*    CreateCommandBinding(EventsViewModel.CmdAddOrder, routeEvent, DefaultCanDo);
            CreateCommandBinding(EventsViewModel.CmdSizeChanged, routeEvent, DefaultCanDo);


            CreateCommandBinding(EventsViewModel.CmdCancellOrdersWithPrice, routeEvent, DefaultCanDo);
            CreateCommandBinding(EventsViewModel.CmdCancellAllOrders, routeEvent, DefaultCanDo);
            CreateCommandBinding(EventsViewModel.CmdCloseAllPositions, routeEvent, DefaultCanDo);
            CreateCommandBinding(EventsViewModel.CmdCloseAllPositionsByIsin, routeEvent, DefaultCanDo);
            CreateCommandBinding(EventsViewModel.CmdShowChangeInstrumentWindow, routeEvent, DefaultCanDo);
          */





        }


		


        public void CreateCommandBinding(RoutedUICommand command, ExecutedRoutedEventHandler execute,
                                           CanExecuteRoutedEventHandler canExecute)
        {
            
            var binding = new CommandBinding(command,
                                             execute,              
                                           canExecute
                                             );

             
            
            // Register CommandBinding for all windows.
            CommandManager.RegisterClassCommandBinding(/*typeof(UserControl)*//*typeof(Control)*/typeof(FrameworkElement), binding);


        }

        private static void DefaultCanDo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;



        }


    }
}
