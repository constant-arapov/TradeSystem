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
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window, IGUIDispatcherable
    {
        public Dispatcher GUIDispatcher { set; get; }

        public AuthWindow()
        {
            InitializeComponent();
            GUIDispatcher = Dispatcher.CurrentDispatcher;
            this.WindowStyle = System.Windows.WindowStyle.ToolWindow;
        }

        public void SetParams(int connId, string stockName)
        {

            this.AuthForm.LabelStockName.Content = stockName;
            this.AuthForm.ConnNum = connId;    

        }



    }
}
