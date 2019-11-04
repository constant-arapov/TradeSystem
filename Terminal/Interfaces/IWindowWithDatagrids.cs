using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;



using Common.Interfaces;


namespace Terminal
{
    public interface IWindowWithDatagrids<TControl> : IGUIDispatcherable where TControl : UIElement, IDataControl
    {

        void AddNewDataFrame(string isin);

        void RemoveNewDataFrame(string isin);

        //void BindDataLog(string isin);

         ViewDataUpdater<TControl> ViewDataUpdater { get; }

    }
}
