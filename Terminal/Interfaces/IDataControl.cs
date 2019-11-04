using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;

namespace Terminal
{
    public interface IDataControl
    {


        string TickerName { get; set; }
        DataGrid DatagridData { get; set; }

    }
}
