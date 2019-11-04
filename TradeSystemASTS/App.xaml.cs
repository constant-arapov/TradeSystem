using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using Common.Utils;

namespace TradeSystemASTS
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        //public static string GParAppName;

        protected override void OnStartup(StartupEventArgs e)
        {
            CUtilGlobals.AppName = CUtil.GetExternalAppName();

            base.OnStartup(e);
          
        }

    }
}
