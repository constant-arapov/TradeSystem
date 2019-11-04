using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input;


namespace TradeSystemASTS
{
    public class CommandsMainWindowViewModel
    {

        private static readonly RoutedUICommand _cmdPasswordChange =
                         new RoutedUICommand("", "CmdPasswordChange", typeof(CommandsMainWindowViewModel));





        public static RoutedUICommand CmdPasswordChange
        {
            get
            {
                return _cmdPasswordChange;
            }
        }


    }
}
