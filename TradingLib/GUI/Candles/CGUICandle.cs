using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;
using System.ComponentModel;



namespace TradingLib.GUI.Candles
{
    public class CGUICandle : INotifyPropertyChanged
    {
       public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        //TO DO insert UpdateCandleMethod
        private DateTime _date;
        public DateTime Date 
        {
            get
            {
                return _date;

            }
            set
            {
                _date = value;
                RaisePropertyChanged("Date");
                 
            }
        }
        private double _open;
        public double Open 
        {
            get
            {
                return _open;

            }
            set
            {
                _open = value;
                RaisePropertyChanged("Open");

            }
        
        }
        private double _high;
        public double High
        {
            get
            {
                return _high;
            }
            set
            {

                _high = value;
                RaisePropertyChanged("High");
            }
        }
        private double _low;
        public double Low
        {
            get
            {
                return _low;
            }

            set
            {
                _low = value;
                RaisePropertyChanged("Low");

            }

        }
        private double _close;
        public double Close
        {
            get
            {
                return _close;
            }

            set
            {
                _close = value;
                RaisePropertyChanged("Close");
            }

        }

        private int _volume;
        public int Volume
        {
            get
            {
                return _volume;
            }

            set
            {
                _volume = value;
                RaisePropertyChanged("Volume");
            }
        }
    
    }
}
