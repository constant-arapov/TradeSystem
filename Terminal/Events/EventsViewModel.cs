using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input;

namespace Terminal.Events
{
    public class EventsViewModel
    {
        private static readonly RoutedUICommand _cmdSizeChanged =
                         new RoutedUICommand("", "CmdSizeChanged", typeof(EventsViewModel));

        public static RoutedUICommand CmdSizeChanged
        {
            get
            {
                return _cmdSizeChanged;
            }
        }

        private static readonly RoutedUICommand _cmdStockWidthChanged =
                        new RoutedUICommand("", "CmdStockWidthChanged", typeof(EventsViewModel));

        public static RoutedUICommand CmdStockWidthChanged
        {
            get
            {
                return _cmdStockWidthChanged;
            }


        }

        private static readonly RoutedUICommand _cmdClusterWidthChanged =
                new RoutedUICommand("", "CmdClusterWidthChanged", typeof(EventsViewModel));


        public static RoutedUICommand CmdClusterWidthChanged
        {
            get
            {
                return _cmdClusterWidthChanged;

            }



        }




        private static readonly RoutedUICommand _cmdAddOrder =
                        new RoutedUICommand("", "CmdAddOrder", typeof(EventsViewModel));

        public static RoutedUICommand CmdAddOrder
        {
            get
            {
                return _cmdAddOrder;
            }
        }

        private static readonly RoutedUICommand _cmdCancellOrdersWithPrice =
                       new RoutedUICommand("", "CmdCancellOrdersWithPrice", typeof(EventsViewModel));

        public static RoutedUICommand CmdCancellOrdersWithPrice
        {
            get
            {
                return _cmdCancellOrdersWithPrice;
            }
        }

        private static readonly RoutedUICommand _cmdCancellAllOrders =
                    new RoutedUICommand("", "CmdCancellAllOrders", typeof(EventsViewModel));

        public static RoutedUICommand CmdCancellAllOrders
        {
            get
            {
                return _cmdCancellAllOrders;
            }
        }


        private static readonly RoutedUICommand _cmdCloseAllPositions =
                  new RoutedUICommand("", "CmdCloseAllPositions", typeof(EventsViewModel));

        public static RoutedUICommand CmdCloseAllPositions
        {
            get
            {
                return _cmdCloseAllPositions;
            }
        }


        private static readonly RoutedUICommand _cmdCloseAllPositionsByIsin =
                 new RoutedUICommand("", "CmdCloseAllPositionsByIsin", typeof(EventsViewModel));

        public static RoutedUICommand CmdCloseAllPositionsByIsin
        {
            get
            {
                return _cmdCloseAllPositionsByIsin;
            }
        }

        private static readonly RoutedUICommand _cmdShowChangeInstrumentWindow =
                     new RoutedUICommand("", "CmdShowChangeInstrumentWindow", typeof(EventsViewModel));

        public static RoutedUICommand CmdShowChangeInstrumentWindow
        {
            get
            {
                return _cmdShowChangeInstrumentWindow;
            }
        }


        private static readonly RoutedUICommand _cmdDeleteInstrument =
                  new RoutedUICommand("", "CmdDeleteInstrument", typeof(EventsViewModel));

        public static RoutedUICommand CmdDeleteInstrument
        {
            get
            {
                return _cmdDeleteInstrument;
            }
        }



        private static readonly RoutedUICommand _cmdChangeTimeFrame =
             new RoutedUICommand("", "CmdChangeTimeFrame", typeof(EventsViewModel));

        public static RoutedUICommand CmdChangeTimeFrame
        {
            get
            {
                return _cmdChangeTimeFrame;
            }
        }





		private static readonly RoutedUICommand _cmdSetStopLossTakeProfit =
			 new RoutedUICommand("", "CmdSetStopLossTakeProfit", typeof(EventsViewModel));



        public static RoutedUICommand CmdSetStopLossTakeProfit
        {
            get
            {
                return _cmdSetStopLossTakeProfit;
            }
        }


        private static readonly RoutedUICommand _cmdSetStopLossInvert =
         new RoutedUICommand("", "CmdSetStopLossInvert", typeof(EventsViewModel));



        public static RoutedUICommand CmdSetStopLossInvert
        {
            get
            {
                return _cmdSetStopLossInvert;
            }
        }



        private static readonly RoutedUICommand _cmdSetStopOrder =
         new RoutedUICommand("", "CmdSetStopOrder", typeof(EventsViewModel));



        public static RoutedUICommand CmdSetStopOrder
        {
            get
            {
                return _cmdSetStopOrder;
            }
        }


        private static readonly RoutedUICommand _cmdAddDelUserLevel =
         new RoutedUICommand("", "CmdAddDelUserLevel", typeof(EventsViewModel));


        public static RoutedUICommand CmdAddDelUserLevel
        {
            get
            {
				return _cmdAddDelUserLevel;
            }
        }


		private static readonly RoutedUICommand _cmdSetKeyboardTrading =
			new RoutedUICommand("", "CmdSetKeyboardTrading", typeof(EventsViewModel));


		public static RoutedUICommand CmdSetKeyboardTrading
		{
			get
			{
				return _cmdSetKeyboardTrading;
			}


		}

		private static readonly RoutedUICommand _cmdSendOrderThrow =
			new RoutedUICommand("", "CmdSendOrderThrow", typeof(EventsViewModel));



		public static RoutedUICommand CmdSendOrderThrow
		{
			get
			{
				return _cmdSendOrderThrow;
			}


		}

		private static readonly RoutedUICommand _cmdInvertPosition =
		new RoutedUICommand("", "CmdInvertPosition", typeof(EventsViewModel));



		public static RoutedUICommand CmdInvertPosition
		{
			get
			{
				return _cmdInvertPosition;
			}


		}

        private static readonly RoutedUICommand _cmdSaveInstrumentConfig =
            new RoutedUICommand("", "CmdSaveInstrumentConfig", typeof(EventsViewModel));


        public static  RoutedUICommand CmdSaveInstrumentConfig 
        {
            get
            {
                return _cmdSaveInstrumentConfig;
            }
        }


        private static readonly RoutedUICommand _cmdSendRestOrder =
           new RoutedUICommand("", "CmdSendRestOrder", typeof(EventsViewModel));


        public static RoutedUICommand CmdSendRestOrder
        {
            get
            {
                return _cmdSendRestOrder;
            }
        }


        private static readonly RoutedUICommand _cmdIncreaseMinStep =
            new RoutedUICommand("", "CmdIncreaseMinStep", typeof(EventsViewModel));



        public static RoutedUICommand CmdIncreaseMinStep
        {
            get
            {
                return _cmdIncreaseMinStep;
            }
        }


        private static readonly RoutedUICommand _cmdDecreaseMinStep =
          new RoutedUICommand("", "CmdDecreaseMinStep", typeof(EventsViewModel));



        public static RoutedUICommand CmdDecreaseMinStep
        {
            get
            {
                return _cmdDecreaseMinStep;
            }
        }



    }
}
