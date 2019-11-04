using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input;


namespace TradeManager.Interfaces
{
    public interface ICommandToDataSource
    {
        void UpdateAccount(object sender, ExecutedRoutedEventArgs e);
		void DisableTrader(object sender, ExecutedRoutedEventArgs e);
		void EnableTrader(object sender, ExecutedRoutedEventArgs e);
		void SetMaxLossVM(object sender, ExecutedRoutedEventArgs e);
		void AddWithDrawMoney(object sender, ExecutedRoutedEventArgs e);
        void CloseTraderPos(object sender, ExecutedRoutedEventArgs e);
        void AddInstrument(object sender, ExecutedRoutedEventArgs e);
        void DeleteInstrument(object sender, ExecutedRoutedEventArgs e);
        void ChangeProcProfit(object sender, ExecutedRoutedEventArgs e);
        void ChangeProfitDealingFee(object sender, ExecutedRoutedEventArgs e);
        void CompleteTrdAddFundsReq(object sender, ExecutedRoutedEventArgs e);
        void CompleteTrdWithdrawReq(object sender, ExecutedRoutedEventArgs e);
        void SendReconnect(object sender, ExecutedRoutedEventArgs e);
    }
}
