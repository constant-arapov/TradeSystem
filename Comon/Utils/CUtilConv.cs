using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;


namespace Common.Utils
{
	public static class CUtilConv
	{
		public static decimal GetDecimalVolume(int intVolume, int roundTo)
		{
			if (roundTo == 0)
				return (decimal)intVolume;

			decimal res = intVolume * CUtil.GetDecimalMult(roundTo);

			return res;

		}


		public static long GetIntVolume(decimal inDecVolume, int roundTo)
		{

			if (roundTo == 0)
				return (long)Math.Round(inDecVolume);

			for (int i = 0; i < roundTo; i++)
				inDecVolume *= 10;


			return (long)Math.Round(inDecVolume);

		}


		public static int GetRoundTo(decimal orderSize)
		{
			if (orderSize >= 1)
				return 0;

			double dres = Math.Log10((double)orderSize);
			int ires = (int)Math.Ceiling(Math.Abs(dres));

			return ires;
		}


		public static int GetPriceDecimals(decimal price)
		{
            
			if (price < 1)
				return 5; //2018-04-23 was 5, 2018-06-25 4->5

			double rowVal = Math.Log10((double)price);
            //2018-04-23 , 2018-06-25 4->5
            return Math.Max(5 - (int)Math.Ceiling(rowVal), 0);
		}


		public static decimal GetMinStep(int decimals)
		{


			decimal res = 1.0m;

			for (int i = 0; i < decimals; i++)
				res *= 0.1m;



			return res;

		}

        public static decimal ToDecimal(string inpString)
        {
            string outString = "";

            if (NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator == ",") //for RUS
                outString = inpString.Replace('.', ',');
            else
                outString = inpString;



           return Convert.ToDecimal(outString);

        }





	}
}
