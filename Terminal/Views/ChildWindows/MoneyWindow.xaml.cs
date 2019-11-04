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


namespace Terminal.Views.ChildWindows
{
    /// <summary>
    /// Логика взаимодействия для MoneyWindow.xaml
    /// </summary>
    public partial class MoneyWindow : BaseTerminalWindow, IGUIDispatcherable
    {
        public Dispatcher GUIDispatcher { get; set; }

        public MoneyWindow()
        {
            InitializeComponent();
            GUIDispatcher = Dispatcher.CurrentDispatcher;
        }
    }
}
