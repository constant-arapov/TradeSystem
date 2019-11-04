using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;


namespace TradingLib.Interfaces.Interaction
{
    public interface IMainWindowForInstrumentRec
    {

        Action<object, RoutedEventArgs> GetTFButtonEventHandler();
    }
}
