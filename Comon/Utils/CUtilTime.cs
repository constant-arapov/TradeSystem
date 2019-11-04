using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utils
{
    public static class CUtilTime
    {


        public static bool InTmInterval(DateTime Time, DateTime begin, DateTime end, int tol)
        {
            if (Time.AddMilliseconds(tol) > begin && Time.AddMilliseconds(-tol) < end)
                return true;

            return false;
        }

        public static bool IsEqual(DateTime dtToCompare, DateTime dtEtalon, int tolMS)
        {


            if (dtToCompare > dtEtalon.AddMilliseconds(-tolMS) &&
                dtToCompare < dtEtalon.AddMilliseconds(tolMS))
                    return true;




            return false;
        }




        public static string TmWithMs(DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy HH:mm:ss.fff");

        }

        public static string GeDateString(DateTime dt)
        {
            return String.Format("{0}_{1}_{2}", dt.Year.ToString("D4"), dt.Month.ToString("D2"), dt.Day.ToString("D2"));

        }


        public static string GetDateTimeString(DateTime dt)
        {

            return String.Format("{0}---{1}_{2}_{3}", GeDateString(dt),
                                 dt.Hour.ToString("D2"), dt.Minute.ToString("D2"), dt.Second.ToString("D2"));

        }

        public static string GetTimeString(DateTime dt)
        {

            return String.Format("{0}_{1}_{2}",
                                dt.Hour.ToString("D2"), dt.Minute.ToString("D2"), dt.Second.ToString("D2"));



        }

        public static DateTime GetDateFromString(string stDate)
        {

            int year = Convert.ToInt32(stDate.Substring(0, 4));
            int month = Convert.ToInt32(stDate.Substring(5, 2));
            int day = Convert.ToInt32(stDate.Substring(8, 2));

            DateTime dt = new DateTime(year, month, day);

            return dt;

        }

        public static DateTime NormalizeToM5_M15_M30(DateTime Date, int interval, int count)
        {
            for (int i = 0; i <= count; i++)
            {
                if (Date.Minute < i * interval)
                    return NormalizeMinutes(Date, (i - 1) * interval);
                else if (Date.Minute == interval * interval)
                    return NormalizeMinutes(Date, 0);

            }

            return Date;



        }


        public static DateTime NormalizeToM5(DateTime Date)
        {

            return NormalizeToM5_M15_M30(Date, 5, 12);

        }
        public static DateTime NormalizeToM15(DateTime Date)
        {

            return NormalizeToM5_M15_M30(Date, 15, 4);

        }

        public static DateTime NormalizeToM30(DateTime Date)
        {

            return NormalizeToM5_M15_M30(Date, 30, 2);

        }





        public static DateTime NormalizeMinutes(DateTime dtPrev, int minutesOffset)
        {

            return dtPrev.AddMinutes(-dtPrev.Minute).AddMinutes(minutesOffset).AddSeconds(-dtPrev.Second).AddMilliseconds(-dtPrev.Millisecond);

        }

        public static DateTime NormalizeSeconds(DateTime dt)
        {

            return dt.AddSeconds(-dt.Second).AddMilliseconds(-dt.Millisecond);

        }



        public static DateTime NormalizeHour(DateTime tm, int hourOffset = 0)
        {

            return tm.AddHours(-hourOffset).AddMinutes(-tm.Minute).AddSeconds(-tm.Second).AddMilliseconds(-tm.Millisecond);

        }


        public static DateTime NormalizeDay(DateTime tm)
        {

            return tm.AddHours(-tm.Hour).AddMinutes(-tm.Minute).AddSeconds(-tm.Second).AddMilliseconds(-tm.Millisecond);
        }

        public static DateTime SetTimeOfDay(DateTime tm, int hours, int minutes=0, int secs=0)
        {
            return NormalizeDay(tm).AddHours(hours).AddMinutes(minutes).AddSeconds(secs);

        }



        public static DateTime MaxDate(DateTime dt1, DateTime dt2)
        {
            if (dt1 > dt2)
                return dt1;
            else
                return dt2;

        }

        public static bool IsEqualTimesDay(DateTime tm1, DateTime tm2)
        {
            if (tm1.Year == tm2.Year && tm1.Month == tm2.Month && tm1.Day == tm2.Day)
                return true;

            return false;

        }

        public static bool IsEqualTimesSecondsAcc(DateTime tm1, DateTime tm2)
        {
            if (tm1.Year == tm2.Year && tm1.Month == tm2.Month && tm1.Day == tm2.Day &&
                tm1.Hour == tm2.Hour && tm1.Minute == tm2.Minute &&
                tm1.Second == tm2.Second)
                return true;

            return false;
        }

        public static bool IsEqualTimesMillisAcc(DateTime tm1, DateTime tm2)
        {
            if (tm1.Year == tm2.Year && tm1.Month == tm2.Month && tm1.Day == tm2.Day &&
                tm1.Hour == tm2.Hour && tm1.Minute == tm2.Minute &&
                tm1.Second == tm2.Second &&
                tm1.Millisecond == tm2.Millisecond)
                return true;

            return false;
        }


        //Note could be also holidays
        public static bool OlderThanTwoWorkDays(DateTime dt)
        {

            double deltaD = (DateTime.Now - dt).TotalDays;
            if ((DateTime.Now.DayOfWeek != DayOfWeek.Monday && deltaD > 3) ||
                 (DateTime.Now.DayOfWeek == DayOfWeek.Monday && deltaD > 4))

                return true;

            return false;
        }




        private static readonly DateTime UnixEpoch =
                 new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long GetUnixTimestampMillis(DateTime tm)
        {
            return (long)(tm - UnixEpoch).TotalMilliseconds;
        }




        public static DateTime DateTimeFromUnixTimestampMillis(long millis)
        {
            return UnixEpoch.AddMilliseconds(millis);
        }

        public static long GetCurrentUnixTimestampSeconds()
        {
            return (long)(DateTime.UtcNow - UnixEpoch).TotalSeconds;
        }

        public static DateTime DateTimeFromUnixTimestampSeconds(long seconds)
        {
            return UnixEpoch.AddSeconds(seconds);
        }

        public static DateTime MscToLocal(DateTime dt)
        {
            const int mscOffset = 3;

            int dh = (TimeZone.CurrentTimeZone.GetUtcOffset(dt)).Hours - mscOffset;

            return dt.AddHours(dh);          
        }

        public static DateTime LocalToMsc(DateTime dt)
        {
            const int mscOffset = 7;

            int dh = (TimeZone.CurrentTimeZone.GetUtcOffset(dt)).Hours - mscOffset;

            return dt.AddHours(dh);
        }
 

    }
}
