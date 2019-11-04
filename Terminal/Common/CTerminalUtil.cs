using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;

using Common.Utils;

namespace Terminal.Common
{
    public static class CTerminalUtil
    {
        public static string GetAmount(int amount,int decimalVolume)
        {
             string stAmount = "";
            //for Bitfinex
            if (decimalVolume != 0)
            {
                string formatAmount = "F" + decimalVolume;
                decimal dcmlAmount = amount * CUtil.GetDecimalMult(decimalVolume);
                stAmount = dcmlAmount.ToString(formatAmount);
            }
            else//for other
            {
                stAmount = amount.ToString();
            }
            //---
            
            return stAmount;
        }

        public static void UpdateLocalColors(Brush globalBrush, ref Color localColor)
        {
            if (globalBrush is SolidColorBrush)
            {

                SolidColorBrush scGlobBrush = (SolidColorBrush)globalBrush;

                if (localColor != scGlobBrush.Color)
                {


                    localColor.R = scGlobBrush.Color.R;
                    localColor.G = scGlobBrush.Color.G;
                    localColor.B = scGlobBrush.Color.B;
                    localColor.A = scGlobBrush.Color.A;

                }
            }

        }

        public static void UpdateBrush(Color color, ref Brush brush)
        {
            if (((SolidColorBrush)brush).Color != color)
                brush = new SolidColorBrush(color);



        }

        public static void UpdateBrushAndPen(Color color, ref Brush brush, ref Pen pen, double penThick)
        {
            if (((SolidColorBrush)brush).Color != color)
            {
                brush = new SolidColorBrush(color);
                pen = new Pen(brush, penThick);
            }

        }




    }
}
