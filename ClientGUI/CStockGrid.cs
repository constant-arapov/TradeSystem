using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


using TradingLib;


namespace ClientGUI
{
    public class CStockGrid : Grid
    {
        decimal _price; //{ get; set; }
        decimal _volume;//{ get; set; }
        EnmDir _dir;


        private Label _labelPrice = new Label();
        private Label _labelVolume = new Label();



        public CStockGrid(decimal price)
        {

            _price = price;

            RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < 3; i++)
                ColumnDefinitions.Add(new ColumnDefinition());

            ///*Label*/  _labelPrice  lb = new Label();
            _labelPrice.Padding = new Thickness(0, 0, 0, 0);

            _labelPrice.Content = _price.ToString();
            Grid.SetRow(_labelPrice, 0);
            Grid.SetColumn(_labelPrice, 1);
            this.Children.Add(_labelPrice);

            _volume = 1;

            _labelVolume.Padding = new Thickness(0, 0, 0, 0);
            _labelVolume.Content = _volume.ToString();



            Grid.SetRow(_labelVolume, 0);
            Grid.SetColumn(_labelVolume, 0);
            this.Children.Add(_labelVolume);



        }

        public void UpdatePrice(decimal price)
        {


        }

        public void UpdateDir(EnmDir dir)
        {
            /*  if (_dir == dir)
                  return;
              */
            if (_volume == 0)
            {
                _labelVolume.Background = Brushes.White;
                _labelPrice.Background = Brushes.White;
                return;
            }


            if (EnmDir.Up == dir)
            {
                _labelVolume.Background = Brushes.Pink;
                _labelPrice.Background = Brushes.Pink;
            }
            else if (EnmDir.Down == dir)
            {
                _labelVolume.Background = Brushes.LightGreen;
                _labelPrice.Background = Brushes.LightGreen;
            }

        }

        public void UpdateVolume(long volume)
        {
            /* if (_volume == volume)
                 return;*/
            _volume = volume;
            if (_volume != 0)
                _labelVolume.Content = _volume.ToString();
            else
                _labelVolume.Content = "";
        }





    }
}
