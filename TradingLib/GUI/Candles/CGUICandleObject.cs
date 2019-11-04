using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows;

namespace TradingLib.GUI.Candles
{
    public class GUICandleObject : CGUICandle
    {
        private bool _bNeedGUIUpdate = false;
        public void UpdateCandle(bool bNeedGUIUpdate)
        {

            UpdateFillColor();
            UpdateWickHeight();
            UpdateBodyHeight();

            UpdateValueOpenedOrClose();

             /*UpdateWickCenter();
             UpdateScaleY();
             UpdateOrigin();
            */

        }


        private double _wickHeight;
        public double WickHeight
        {
            get
            {

                //    return (High - Low);
                return _wickHeight;

            }

            set
            {
                _wickHeight = value;
               // if (_bNeedGUIUpdate)
                    RaisePropertyChanged("WickHeight");
            }

        }


        private void UpdateWickHeight()
        {
            WickHeight = High - Low;


        }

        private void UpdateValueOpenedOrClose()
        {
            ValueOpenOrClose = Math.Max(Open, Close);

        }



        private double _valueOpenOrClose;
        public double ValueOpenOrClose
        {

            get
            {
                return _valueOpenOrClose;
            }
            set
            {
                _valueOpenOrClose = value;

             //   if (_bNeedGUIUpdate)
                    RaisePropertyChanged("ValueOpenOrCLose");

            }






        }



        private double _wickCenter;
        public double WickCenter
        {
            get
            {

                return _wickCenter;

            }

            set
            {
                _wickCenter = value;
              //  if (_bNeedGUIUpdate)
                    RaisePropertyChanged("WickCenter");
            }

        }


        private void UpdateWickCenter()
        {

            WickCenter = (High + Low) / 2;
        }


        private double _scaleY;
        public double ScaleY
        {
            get
            {
                return _scaleY;
            }
            set
            {
                _scaleY = value;

          //    if (_bNeedGUIUpdate)
                RaisePropertyChanged("ScaleY");

            }
        }

        public void UpdateScaleY()
        {
            var scaleY = BodyHeight / WickHeight;
            if (scaleY == 0)
                scaleY = 0.01;
            ScaleY = scaleY;
            
            
        }

        private Point _origin;
        public Point Origin
        {
            get
            {
                return _origin;
            }
            set
            {
                _origin = value;
            //    if (_bNeedGUIUpdate)
                    RaisePropertyChanged("Origin");

            }

        }

        private void UpdateOrigin()
        {
            //center
            Point origin = new Point() { X = 0 };
            var top = Open > Close ? Open : Close;
            origin.Y = (High - top + (BodyHeight / 2)) / WickHeight;
            Origin = origin;

        }




        private double _bodyHeight;
        public double BodyHeight
        {
            get
            {
                return _bodyHeight;

            }
            set
            {
                _bodyHeight = value;
              //  if (_bNeedGUIUpdate)
                    RaisePropertyChanged("BodyHeight");
            }

        }

        private void UpdateBodyHeight()
        {
            BodyHeight = Math.Abs(Open - Close);
        }

       
        public Color CandleColor { get; set; }

        
        private Color _fillColor;
        public Color FillColor
        {
            get
            {
                return _fillColor;
            }
            set
            {
                _fillColor = value;
              //  if (_bNeedGUIUpdate)
                    RaisePropertyChanged("FillColor");
            }

        }

        public void UpdateFillColor()
        {

            if (Open > Close)
                FillColor = CandleColor;
            else
                FillColor = Colors.LightGray;
            


        }
    }
}
