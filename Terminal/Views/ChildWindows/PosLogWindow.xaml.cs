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

using System.Windows.Threading;


using Common.Interfaces;

using Terminal.Controls.ChildWindows;


namespace Terminal.Views.ChildWindows
{
    /// <summary>
    /// Логика взаимодействия для PosLogWindow.xaml
    /// </summary>
    /// 



    public partial class PosLogWindow : BaseTerminalWindow, IGUIDispatcherable, IWindowWithDatagrids<ControlPosLog>
    {

       

        ViewDataUpdater<ControlPosLog> _viewDataUpdater;


        public ViewDataUpdater<ControlPosLog> ViewDataUpdater
        {
            get
            {
                return _viewDataUpdater;                                               
            } 


        }


      
        public Dispatcher GUIDispatcher { get; set; }


        

        public PosLogWindow()
        {
            InitializeComponent();

          //  SetWindowPosition();

            GUIDispatcher = Dispatcher.CurrentDispatcher;
          
            _viewDataUpdater = new ViewDataUpdater<ControlPosLog>(StackPanelMain);

        }





        public void AddNewDataFrame(string isin)
        {

            _viewDataUpdater.AddNewDataFrame(isin);
        
        }

      
        public void RemoveNewDataFrame(string isin)
        {
            _viewDataUpdater.RemoveDataFrame(isin);

        }



    }
}
