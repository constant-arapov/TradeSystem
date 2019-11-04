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
using System.Windows.Navigation;
using System.Windows.Shapes;


using Terminal.Interfaces;
using Terminal.Events;


namespace Terminal.Controls.Market.ChildElements
{
    /// <summary>
    /// Control which is  the bar of user's added order. 
    /// It's contain Amount TextBlock. And "X" textblock
    /// </summary>
    public partial class ControlUserOrders : UserControl, IStockNumerable
    {

        public double Price;

    
        public int StockNum { get; set; }


        public ControlUserOrders(double _Height, int stockNum)
        {
            InitializeComponent();
            base.Height = _Height;
            StockNum = stockNum;
        }


        // event handler for special textblock 
        //for remove order
        private void TxtBlck_RemoveOrder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((e.ChangedButton == MouseButton.Middle) || (sender is TextBlock))
            {
                //this.TryingTo_RemoveOrder(this);
                EventsViewModel.CmdCancellOrdersWithPrice.Execute(Price, this);


            }
        }



    }
}
