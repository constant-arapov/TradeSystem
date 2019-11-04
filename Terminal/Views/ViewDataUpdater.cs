using System;
using System.Collections.Generic;
using System.Collections;

using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Terminal
{
    /// <summary>
    /// "Helper" class that adds, removes frames (controls) to window (poslog or dealslog)"
    /// </summary>
    /// <typeparam name="TControl"></typeparam>
    public class  ViewDataUpdater <TControl> where TControl:  UIElement, IDataControl
    {
        public Dictionary<string, TControl> DictKStringVControl { get; set; }

        private StackPanel _stackPanel;


        public ViewDataUpdater(StackPanel stackPanel)
        {

            
            DictKStringVControl = new Dictionary<string, TControl>();
            _stackPanel = stackPanel;
        }



        public void AddNewDataFrame(string isin)
        {
            try
            {
                TControl control = (TControl)Activator.CreateInstance(typeof(TControl));
                control.TickerName = isin;
                _stackPanel.Children.Add(control);

                DictKStringVControl[isin] = control;
            }
            catch (Exception e)
            {
                Error("ViewDataUpdater.AddNewDataFrame", e);
            }

        }

        public void RemoveDataFrame(string isin)
        {
            //changed 2018-03-28 to protect against crash
            try
            {
                _stackPanel.Children.Remove(DictKStringVControl[isin]);
                DictKStringVControl.Remove(isin);
            }
            catch (Exception e)
            {
                Error("ViewDataUpdater.RemoveDataFrame", e);
            }
        }


        public void BindDataGrid(string instrument, IEnumerable dataSource)
        {
            //changed 2018-03-28 to protect against crash
                   
            try
            {
                if (DictKStringVControl.ContainsKey(instrument))
                {
                    DictKStringVControl[instrument].DatagridData.ItemsSource = dataSource;
                    DictKStringVControl[instrument].DatagridData.Items.Refresh();
                }
                else
                {
                    Error("ViewDataUpdater.BindDataGrid instrument not forund " + instrument);
                }
            }
            catch (Exception e)
            {
               
                Error("ViewDataUpdater.BindDataGrid", e);
            }
        }

        private void Error(string msg, Exception exc=null)
        {
            //added 2018-03-28 to protect against crash
            msg += " | Type=" + typeof(TControl).ToString();
            CKernelTerminal.ErrorStatic(msg,exc);
        }





    }
}
