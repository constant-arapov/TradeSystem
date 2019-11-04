using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Windows;
using System.Windows.Media;
using System.Globalization;

using Terminal.ViewModels;

namespace Terminal.Controls.Market.ChildElements
{
    public class CSelectionDrawerBg
    {
        private ControlDeals _controlDeals;
        private FontFamily _txtFontFam = new FontFamily("Verdana");
        private CultureInfo _cultureInfo = new CultureInfo("en-US");

        CSelectionMode _selectionMode;


        public CSelectionDrawerBg (ControlDeals controlDeals)
        {
            _controlDeals = controlDeals;
        }


        private bool IsNeedDrawSelection()
        {
            if (_controlDeals.SelectedYBg == 0)
                return false;


            if (_selectionMode == null)
                return false;




            if (_selectionMode.IsModeDrawLevel ||
               _selectionMode.IsModeStopLossInvert ||
               _selectionMode.IsModeStopLossTakeProfit ||
               _selectionMode.IsModeStopOrder ||
                _selectionMode.IsModeRestOrder)
                return true;

            return false;
        }



        private Brush GetBrushOfSelection()
        {

            if (_controlDeals.SelectionModeBg.IsModeDrawLevel)
                return new SolidColorBrush(Color.FromArgb(0x10, 0xFF, 0x00, 0x00));

            if (_controlDeals.SelectionModeBg.IsModeStopOrder)
                return new SolidColorBrush(Color.FromArgb(0x10, 0xFF, 0x00, 0x00));

            if (_controlDeals.SelectionModeBg.IsModeStopLossTakeProfit)
                return new SolidColorBrush(Color.FromArgb(0x10, 0xFF, 0x00, 0x00));


            if (_controlDeals.SelectionModeBg.IsModeStopLossInvert)
                return new SolidColorBrush(Color.FromArgb(0x10, 0xFF, 0x00, 0x00));

            if (_controlDeals.SelectionModeBg.IsModeRestOrder)
                return new SolidColorBrush(Color.FromArgb(0x10, 0xFF, 0x00, 0x00));

            if (_controlDeals.SelectionModeBg.IsModeRestOrder)
                return new SolidColorBrush(Color.FromArgb(0x10, 0xFF, 0x00, 0x00));

            //default transparent color
            return new SolidColorBrush(Color.FromArgb(0x00, 0x00, 00, 0x00));


        }

        private string GetText()
        {

            if (_selectionMode.IsModeDrawLevel)
            {
               
                List<CLevelEl> _lstLevels = _controlDeals.LstUserLevelsBg;
                var res = _lstLevels.Find(el => el.Price == _controlDeals.SelectedPriceBg);
                if (res != null)
                    return "";

                return "Уровень";
            }



            if (_selectionMode.IsModeStopLossTakeProfit)
            {
                if (_controlDeals.SelectedPriceBg != _controlDeals.StopLossPriceBg &&
                    _controlDeals.SelectedPriceBg != _controlDeals.TakeProfitPriceBg)
                    return "Стоплос/тейк";
                else
                    return "";



            }

            if (_selectionMode.IsModeStopOrder)
            {
                if (_controlDeals.SelectedPriceBg != _controlDeals.BuyStopPriceBg &&
                    _controlDeals.SelectedPriceBg != _controlDeals.SellStopPriceBg)
                    return "Стоп ордер";
                else
                    return "";


            }
            if (_selectionMode.IsModeStopLossInvert)
            {
                if (_controlDeals.SelectedPriceBg != _controlDeals.StopLossInvertPriceBg &&
                  _controlDeals.SelectedPriceBg != _controlDeals.StopLossInvertPriceBg)
                    return "Инверт. стопл";
                else
                    return "";
            }


            if (_selectionMode.IsModeRestOrder)
            {
                if (_controlDeals.SelectedPriceBg != _controlDeals.StopLossInvertPriceBg &&
                  _controlDeals.SelectedPriceBg != _controlDeals.StopLossInvertPriceBg)
                    return "Равн. поз.";
                else
                    return "";


            }
            return "error";
        }


        public void Draw(DrawingContext drwContxt)
        {
            if (_controlDeals.SelectionModeBg != null)
                _selectionMode = _controlDeals.SelectionModeBg;


            if (!IsNeedDrawSelection())
                return;

            Brush brush = GetBrushOfSelection();
            Pen pen = new Pen(brush, 1);

            drwContxt.DrawRectangle(brush, pen, new Rect(0, _controlDeals.SelectedYBg, _controlDeals.ActualWidth, _controlDeals.StringHeightBg));

            Typeface typeFace = new Typeface(_txtFontFam, FontStyles.Normal, FontWeights.Normal, new FontStretch());

            var text = new FormattedText(GetText(), _cultureInfo, FlowDirection.LeftToRight, typeFace, _controlDeals.FontSizeBg, Brushes.Black);

            double parXOffset = 5;
            drwContxt.DrawText(text, new Point(parXOffset, _controlDeals.SelectedYBg));



        }






    }
}
