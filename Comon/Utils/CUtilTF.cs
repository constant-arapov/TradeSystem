using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;

namespace Common.Utils
{
    public static class CUtilTF
    {

    


        public static bool IsOpened_M5_M15_M30_TF(DateTime dtPrev, DateTime dtCurr, List<int> lstScale, ref DateTime dtFrom, ref DateTime dtTo)
        {

            int i = -1;
            while (lstScale[i + 1] <= dtPrev.Minute)
                i++;

            dtFrom = CUtilTime.NormalizeMinutes(dtPrev, lstScale[i]);
            dtTo = CUtilTime.NormalizeMinutes(dtPrev, lstScale[i + 1]);


            if ((dtPrev.Date == dtCurr.Date && dtPrev.Hour == dtCurr.Hour && i + 1 < lstScale.Count && dtCurr.Minute >= lstScale[i + 1]) ||
                (dtPrev.Date == dtCurr.Date && dtCurr.Hour > dtPrev.Hour) ||
                (dtPrev.Date < dtCurr.Date))
                return true;


            return false;
        }

        public static bool WasClose_M1_TF(DateTime dtPrev, DateTime dtCurr)
        {

            if ((/*dtPrev.Date == dtCurr.Date && dtPrev.Hour == dtCurr.Hour && */Math.Abs(dtCurr.Minute - dtPrev.Minute) >= 1) ||
                 (dtPrev.Date == dtCurr.Date && dtCurr.Hour - dtPrev.Hour >= 1) ||
                 (dtPrev.Date < dtCurr.Date)
                )

                return true;


            return false;

        }

        public static bool WasClosed_M5_M15_M30_TF(DateTime dtPrev, DateTime dtCurr, List<int> lstScale, ref DateTime dtFrom, ref DateTime dtTo)
        {
            int i = -1;
            while (lstScale[i + 1] <= dtPrev.Minute)
                i++;

            int j = -1;
            while (lstScale[j + 1] <= dtCurr.Minute)
                j++;

            dtFrom = CUtilTime.NormalizeMinutes(/*dtCurr*/dtPrev, lstScale[i]);
            dtTo = CUtilTime.NormalizeMinutes(/*dtCurr*/dtCurr, lstScale[j]);


            if (dtFrom.Day == 23 && dtFrom.Hour == 18 && dtFrom.Minute == 55)
            {
                int tmp = 1;
            }


            if ((dtPrev.Date == dtCurr.Date && dtPrev.Hour == dtCurr.Hour && i + 1 < lstScale.Count && dtCurr.Minute >= lstScale[i + 1]) ||
                (dtPrev.Date == dtCurr.Date && dtCurr.Hour > dtPrev.Hour) ||
                (dtPrev.Date < dtCurr.Date))
                return true;


            return false;




        }

        public static bool IsClosed_M5_M15_M30_TF(DateTime dtPrev, DateTime dtCurr, List<int> lstScale,  ref DateTime dtFrom, ref DateTime dtTo)
        {

            int i = -1;
            while (lstScale[i + 1] <= dtPrev.Minute)
                i++;

             dtFrom = CUtilTime.NormalizeMinutes(dtPrev, lstScale[i]);
             dtTo = CUtilTime.NormalizeMinutes(dtPrev, lstScale[i + 1]);


            if ((dtPrev.Date == dtCurr.Date && dtPrev.Hour == dtCurr.Hour && i + 1 < lstScale.Count && dtCurr.Minute >= lstScale[i + 1]) ||
                (dtPrev.Date == dtCurr.Date && dtCurr.Hour > dtPrev.Hour) ||
                (dtPrev.Date < dtCurr.Date))
                return true;


            return false;
        }

        public static bool IsClosed_H1(DateTime dtPrev, DateTime dtCurr, List<int> lstScale, ref DateTime dtFrom, ref DateTime dtTo)
        {


             dtFrom = CUtilTime.NormalizeHour(dtPrev); 
             dtTo = CUtilTime.NormalizeHour(dtCurr);


            if (
               (dtPrev.Date == dtCurr.Date && dtCurr.Hour > dtPrev.Hour) ||
               (dtPrev.Date < dtCurr.Date))
                return true;


            return false;
        }



        public static bool WasClosed_H1(DateTime dtPrev, DateTime dtCurr, List<int> lstScale, ref DateTime dtFrom, ref DateTime dtTo)
        {


            dtFrom = CUtilTime.NormalizeHour(dtCurr);
            dtTo = CUtilTime.NormalizeHour(dtCurr.AddHours(1));


            if (
               (dtPrev.Date == dtCurr.Date && dtCurr.Hour > dtPrev.Hour) ||
               (dtPrev.Date < dtCurr.Date))
                return true;


            return false;
        }


        public static bool WasClosed_D1(DateTime dtPrev, DateTime dtCurr, List<int> lstScale, ref DateTime dtFrom, ref DateTime dtTo)
        {


            dtFrom = CUtilTime.NormalizeDay(dtCurr);
            dtTo = CUtilTime.NormalizeDay(dtCurr.AddDays(1));


            if (
               (dtPrev.Date < dtCurr.Date))
                return true;


            return false;
        }




        public static bool IsClosed_D1(DateTime dtPrev, DateTime dtCurr, List<int> lstScale, ref DateTime dtFrom, ref DateTime dtTo)
        {


            dtFrom = CUtilTime.NormalizeDay(dtPrev); 
            dtTo = CUtilTime.NormalizeDay(dtCurr);


            if (
               (dtPrev.Date < dtCurr.Date))
                return true;


            return false;
        }



        public static DateTime MaxDate(DateTime dt1, DateTime dt2)
        {
            if (dt1 > dt2)
                return dt1;
            else
                return dt2;

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



    }
}
