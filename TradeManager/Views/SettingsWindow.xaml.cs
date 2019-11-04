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

using TradingLib.ProtoTradingStructs;

using TradeManager.Commands;

namespace TradeManager.Views
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            CAuthRequest areq = 
                new CAuthRequest { User = TextBoxUser.Text,
                                   Password = PasswordBoxPasswod.Password };

            CCommands.CmdUpdateAccount.Execute(areq, this);
            Close();
        }

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Close();
		}
    }
}
