using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input;




namespace TradeManager.Commands
{
	public class CCommands
	{


	
        private static readonly RoutedUICommand _cmdUpadeteAccount =
                     new RoutedUICommand("", "CmdUpdateAccount", typeof(CCommands));

        public static RoutedUICommand CmdUpdateAccount
        {
            get
            {
                return _cmdUpadeteAccount;
            }


        }


		private static readonly RoutedUICommand _cmdDisableTrader =
						 new RoutedUICommand("", "CmdDisableTrader", typeof(CCommands));



		public static RoutedUICommand CmdDisableTrader
		{
			get
			{
				return _cmdDisableTrader;
			}


		}


		private static readonly RoutedUICommand _cmdEnableTrader =
					 new RoutedUICommand("", "CmdEnalbeTrader", typeof(CCommands));



		public static RoutedUICommand CmdEnalbeTrader
		{
			get
			{
				return _cmdEnableTrader;
			}


		}


		private static readonly RoutedUICommand _cmdSetMaxLossVM =
					 new RoutedUICommand("", "CmdSetMaxLossVM", typeof(CCommands));



		public static RoutedUICommand CmdSetMaxLossVM
		{
			get
			{
				return _cmdSetMaxLossVM;
			}


		}


		private static readonly RoutedUICommand _cmdAddWithdrawMoney =
						new RoutedUICommand("", "CmdAddWithdrawMoney", typeof(CCommands));



		public static RoutedUICommand CmdAddWithdrawMoney
		{
			get
			{
				return _cmdAddWithdrawMoney;
			}


		}

        private static readonly RoutedUICommand _cmdCloseTraderPos =
                        new RoutedUICommand("", "CmdCloseTraderPos", typeof(CCommands));



        public static RoutedUICommand CmdCloseTraderPos
        {
            get
            {
                return _cmdCloseTraderPos;
            }


        }

        private static readonly RoutedUICommand _cmdAddInstrument =
                       new RoutedUICommand("", "CmdAddInstrument", typeof(CCommands));



        public static RoutedUICommand CmdAddInstrument
        {
            get
            {
                return _cmdAddInstrument;
            }


        }


        private static readonly RoutedUICommand _cmdDeleteInstrument =
                     new RoutedUICommand("", "CmdDeleteInstrument", typeof(CCommands));



        public static RoutedUICommand CmdDeleteInstrument
        {
            get
            {
                return _cmdDeleteInstrument;
            }


        }


        private static readonly RoutedUICommand _cmdChangeProcProfit =
                new RoutedUICommand("", "CmdChangeProcProfit", typeof(CCommands));


        public static RoutedUICommand CmdChangeProcProfit
        {
            get
            {
                return _cmdChangeProcProfit;
            }

        }


        private static readonly RoutedUICommand _cmdChangeProcDealingFee =
               new RoutedUICommand("", "CmdChangeProcDealongFee", typeof(CCommands));


        public static RoutedUICommand CmdChangeProcDealingFee
        {
            get
            {
                return _cmdChangeProcDealingFee;
            }

        }

        private static readonly RoutedUICommand _cmdTraderAddFundsReq =
            new RoutedUICommand("", "CmdTraderAddFundsReq", typeof(CCommands));


        public static RoutedUICommand CmdTraderAddFundsReq
        {
            get
            {
                return _cmdTraderAddFundsReq;
            }

        }



        private static readonly RoutedUICommand _cmdTraderWithdrawReq =
        new RoutedUICommand("", "CmdTraderWithdrawReq", typeof(CCommands));


        public static RoutedUICommand CmdTraderWithdrawReq
        {
            get
            {
                return _cmdTraderWithdrawReq;
            }

        }

        private static readonly RoutedUICommand _cmdReconnect =
       new RoutedUICommand("", "CmdReconnect", typeof(CCommands));


        public static RoutedUICommand CmdReconnect
        {
            get
            {
                return _cmdReconnect;
            }

        }







    }
}
