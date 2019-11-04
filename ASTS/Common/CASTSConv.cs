using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using TradingLib.Enums;

namespace ASTS.Common
{
    public static class CASTSConv
    {

        public static void ASTSTimeToHrMinSec(string asts,out int  hr, out int min, out int s)
        {
            hr = 0;
            min = 0;
            s = 0;

            int length = asts.Length;

            if (length > 4)
            {
                if (asts.Length == 6)                                  
                    hr = Convert.ToInt16(asts.Substring(length-6, 2));
                else // 5
                    hr = Convert.ToInt16(asts.Substring(length-5, 1));
                
            }
            if (length > 2)
            {
                if (asts.Length >= 4)
                    min = Convert.ToInt16(asts.Substring(length-4, 2));
                else //3
                    min = Convert.ToInt16(asts.Substring(length-3, 1));
            }


            if (length == 0)
                throw new ApplicationException("DateToHrMinSec invalid date format");

           
            if (length >=2)
                s = Convert.ToInt16(asts.Substring(length-2, 2));
            else
                s = Convert.ToInt16(asts.Substring(length-1, 1));
            

            
        }


        public static void ASTSDateToYearMonthDay(string asts, out int year, out int month, out int day)
        {
            year = 0;
            month = 0;
            day = 0;

            year = Convert.ToInt16(asts.Substring( 0, 4));
            month = Convert.ToInt16(asts.Substring(4, 2));
            day = Convert.ToInt16(asts.Substring(6, 2));



        }








        public static DateTime ASTSTimeToDateTime(string astsTime)
        {

            DateTime dt = DateTime.Now.Date;

            int hr = 0;
            int min = 0;
            int s = 0;

            ASTSTimeToHrMinSec(astsTime, out hr, out min, out s);

            dt = dt.AddHours(hr).AddMinutes(min).AddSeconds(s);


            return dt;
        }

        public static DateTime ASTSDateAndTimeToDateAndTime(string astsDate, string astsTime)
        {
            
            int year = 0;
            int month = 0;
            int day = 0;

            ASTSDateToYearMonthDay(astsDate, out year, out month, out day);
           

            //dt = dt.AddYears(year).AddMonths(month).AddDays(day);


            int hr = 0;
            int min = 0;
            int s = 0;

            ASTSTimeToHrMinSec(astsTime, out hr, out min, out s);

            //dt = dt.AddHours(hr).AddMinutes(min).AddSeconds(s);
            DateTime dt = new DateTime(year, month, day, hr, min, s);


            return dt;
        }

        public static EnmOrderAction ASTSActionToEnmOrderAction(char act)
        {
            if (act == 'O')
                return EnmOrderAction.Added;
            else if (act == 'W')
                return EnmOrderAction.Deleted;
            else if (act == 'M')
                return EnmOrderAction.Deal;



            return EnmOrderAction.Unknown;

        }

        public static char CancellOrderDirToASTS(int dir)
        {
            EnmCancelOrderDir cancellDir = (EnmCancelOrderDir) dir;
            if (cancellDir == EnmCancelOrderDir.BuyOnly)
                return 'B';
            else if (cancellDir == EnmCancelOrderDir.SellOnly)
                return 'S';




            return ' ';//both_dir
        }





    }
}
