using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Globalization;



using Terminal.Controls;
using Terminal.ViewModels;




namespace Terminal.Controls.Market.ChildElements
{
    public class CSelectionDrawer
    {

        private ControlDeals _controlDeals;
        private FontFamily _txtFontFam = new FontFamily("Verdana");
        private CultureInfo _cultureInfo = new CultureInfo("en-US");

        CSelectionMode _selectionMode;


        public CSelectionDrawer(ControlDeals controlDeals)
        {

            _controlDeals = controlDeals;
         

        }

        private bool IsNeedDrawSelection()
        {
            if (_controlDeals.SelectedY == 0)
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

            if (_controlDeals.SelectionMode.IsModeDrawLevel)
                return new SolidColorBrush(Color.FromArgb(0x10, 0xFF, 0x00, 0x00));

            if (_controlDeals.SelectionMode.IsModeStopOrder)
                return new SolidColorBrush(Color.FromArgb(0x10, 0xFF, 0x00, 0x00));

            if (_controlDeals.SelectionMode.IsModeStopLossTakeProfit)
                return new SolidColorBrush(Color.FromArgb(0x10, 0xFF, 0x00, 0x00));


            if (_controlDeals.SelectionMode.IsModeStopLossInvert)
                return new SolidColorBrush(Color.FromArgb(0x10, 0xFF, 0x00, 0x00));

            if (_controlDeals.SelectionMode.IsModeRestOrder)
                return new SolidColorBrush(Color.FromArgb(0x10, 0xFF, 0x00, 0x00));

            if (_controlDeals.SelectionMode.IsModeRestOrder)
                return new SolidColorBrush(Color.FromArgb(0x10, 0xFF, 0x00, 0x00));

            //default transparent color
            return new SolidColorBrush(Color.FromArgb(0x00, 0x00, 00, 0x00));

            
        }

        private string GetText()
        {

		  if (_selectionMode.IsModeDrawLevel)
		  {
			 List<CLevelEl> _lstLevels =  _controlDeals.UserLevels.GetCopy();
			 var res = _lstLevels.Find(el => el.Price == _controlDeals.SelectedPrice);
		     if (res != null)
				return "";

            return "Уровень";
		  }



		  if (_selectionMode.IsModeStopLossTakeProfit)
		  {
			  if (_controlDeals.SelectedPrice != _controlDeals.StopLossPrice &&
				  _controlDeals.SelectedPrice != _controlDeals.TakeProfitPrice)
				return "Стоплос/тейк";
			  else
				return "";

			  

		  }

          if (_selectionMode.IsModeStopOrder)
          {
              if (_controlDeals.SelectedPrice != _controlDeals.BuyStopPrice &&
                  _controlDeals.SelectedPrice != _controlDeals.SellStopPrice)
                  return "Стоп ордер";
              else
                  return "";
                

          }
          if (_selectionMode.IsModeStopLossInvert)
          {
              if (_controlDeals.SelectedPrice != _controlDeals.StopLossInvertPrice &&
                _controlDeals.SelectedPrice != _controlDeals.StopLossInvertPrice)
                  return "Инверт. стопл";
              else
                  return "";
          }


	      if (_selectionMode.IsModeRestOrder)
		  {
			  if (_controlDeals.SelectedPrice != _controlDeals.StopLossInvertPrice &&
				_controlDeals.SelectedPrice != _controlDeals.StopLossInvertPrice)
				  return "Равн. поз.";
			  else
				  return "";


		  }
          return "error";
        }


        public void Draw(DrawingContext drwContxt)
        {
            if (_controlDeals.SelectionMode != null)
                _selectionMode = _controlDeals.SelectionMode;


            if (!IsNeedDrawSelection())
                return;

            Brush brush = GetBrushOfSelection();
            Pen pen = new Pen(brush, 1);

            drwContxt.DrawRectangle(brush, pen, new Rect(0, _controlDeals.SelectedY, _controlDeals.ActualWidth, _controlDeals.StringHeight));

            Typeface typeFace =  new Typeface(_txtFontFam, FontStyles.Normal, FontWeights.Normal, new FontStretch());

            var text = new FormattedText(GetText(), _cultureInfo, FlowDirection.LeftToRight, typeFace, _controlDeals.FontSize, Brushes.Black);

            double parXOffset = 5;
            drwContxt.DrawText(text, new Point(parXOffset, _controlDeals.SelectedY));

         

        }



       

    }
}
