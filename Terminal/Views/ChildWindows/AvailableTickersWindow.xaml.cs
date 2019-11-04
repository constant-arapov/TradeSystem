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

using Terminal.ViewModels;
using Terminal.Controls.Market;


namespace Terminal.Views.ChildWindows
{
    /// <summary>
    /// Логика взаимодействия для AvailableInstrumentsWindow.xaml
    /// </summary>
    public partial class AvailableTickersWindow : Window, IGUIDispatcherable
    {

        public Dispatcher GUIDispatcher { get; set; }

      //  public int CurrStockNum { get; set; }
       // public string CurrentTicker { get; set; }
      //  public int CurrentConId { get; set; }

        public MarketViewModel CurrentMarketViewModel { get; set; }
       // public ControlMarket ControlMarket { get; set; }

        



        //public int StockNum { get; set; }

        public AvailableTickersWindow()
        {
            InitializeComponent();
            this.WindowStyle = System.Windows.WindowStyle.ToolWindow;
            GUIDispatcher = Dispatcher.CurrentDispatcher;

        }
        /*
        public void SetStockNum(int stockNum)
        {

            StockNum = stockNum;
                 

        }
          */   


    }
}
