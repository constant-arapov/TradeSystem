using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;
using TradingLib;


namespace ClientGUI
{
    public class CStockRecord : Grid
    {
        private decimal _price;

        private CStockGrid _stockGrid;
        public decimal Price { get; set; }
        public long Volume { get; set; }
        public EnmDir Dir { get; set; }

        public CStockRecord(decimal price)
            : base()
        {

            _price = price;


            RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < 3; i++)
                ColumnDefinitions.Add(new ColumnDefinition());



            _stockGrid = new CStockGrid(_price);


            Grid.SetRow(_stockGrid, 0);
            Grid.SetColumn(_stockGrid, 2);
            this.Children.Add(_stockGrid);

            //AddStockElement();

            //CWPFUtils.ConstructGrid((Grid) this, 1, 3);
        }

        public void UpdateStockRecordView()
        {

            UpdateVolume(this.Volume);
            UpdateDir();

        }
        private void UpdateDir()
        {
            _stockGrid.UpdateDir(Dir);

        }
        public void UpdateVolume(long volume)
        {
            _stockGrid.UpdateVolume(volume);
        }





    }
}
