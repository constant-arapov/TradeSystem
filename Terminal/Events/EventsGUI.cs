using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input;


namespace Terminal.Events
{
    public class EventsGUI
    {


        private static readonly RoutedUICommand _connectToServer = 
                        new RoutedUICommand("", "ConnectToServer", typeof(EventsGUI));

        public static RoutedUICommand ConnectToServer
        {
            get
            {
                return _connectToServer;
            }
        }


        private static readonly RoutedUICommand _errorMessage =
                      new RoutedUICommand("", "ErrorMessage", typeof(EventsGUI));

        public static RoutedUICommand ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
        }

        private static readonly RoutedUICommand _closeInstrumentPositions =
                    new RoutedUICommand("", "CloseInstrumentPositions", typeof(EventsGUI));

        public static RoutedUICommand CloseInstrumentPositions
        {
            get
            {
                return _closeInstrumentPositions;
            }
        }

        private static readonly RoutedUICommand _cancellInstrumentOrders =
                  new RoutedUICommand("", "CancellInstrumentOrders", typeof(EventsGUI));

        public static RoutedUICommand CancellInstrumentOrders
        {
            get
            {
                return _cancellInstrumentOrders;
            }
        }


        private static readonly RoutedUICommand _closeAllPositions =
            new RoutedUICommand("", "CloseAllPositions", typeof(EventsGUI));


        public static  RoutedUICommand CloseAllPositions
        {
            get
            {
                return _closeAllPositions;
            }


        }

        private static readonly RoutedUICommand _cancellAllOrders =
            new RoutedUICommand("", "CancellAllOrders", typeof(EventsGUI));


        public static RoutedUICommand CancellAllOrders 
        {
            get
            {
                return _cancellAllOrders;
            }
        }

        /*
        private static readonly RoutedUICommand _commandSizeChanged =
                     new RoutedUICommand("", "ConnectToServer", typeof(EventsGUI));

        public static RoutedUICommand CommandSizeChanged
        {
            get
            {
                return _commandSizeChanged;
            }
        }
        */



    }
}
