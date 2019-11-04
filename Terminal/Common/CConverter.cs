using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Data;

using TradingLib.Enums;


namespace Terminal.Common
{
    public class CConverterDecimalToBool : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal dVal = (decimal)value;
            return dVal > 0 ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }



    }

    public class CConverterIntToBool : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal dVal = (int)value;
            return dVal > 0 ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }



    }





    public class CConverterBySellToBool : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            EnmDealDir dVal = (EnmDealDir)value;
            return dVal == EnmDealDir.Buy ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }



    }






}
