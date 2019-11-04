using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;


using TradingLib;

namespace Terminal.ViewModels.ChildWindows
{
    public class ViewModelDealsLog<TDataLogUpdate, TDataLog, TControl> : BaseViewModelDataGridWin<TDataLogUpdate, TDataLog, TControl>
        where TControl : UIElement, IDataControl
        where TDataLogUpdate : IDataLogUpdateCommand<TDataLog>
    {
    
    }
}
