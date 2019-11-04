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


using Terminal.Controls;


namespace Terminal.Views.ChildWindows
{
    /// <summary>
    /// Логика взаимодействия для DetailedViewModelWindow.xaml
    /// </summary>
    public partial class DetailedVMWindow : BaseTerminalWindow, IGUIDispatcherable
    {

    


        public Dispatcher GUIDispatcher { get; set; }

        public DetailedVMWindow()
        {
            InitializeComponent();

            GUIDispatcher = Dispatcher.CurrentDispatcher;

        }

        
      



    }
}
