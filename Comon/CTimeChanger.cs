using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;


namespace Common
{
   public  class CTimeChanger
    {
    //SetLocalTime C# Signature
            [DllImport("Kernel32.dll")]
            public static extern bool SetLocalTime(ref SYSTEMTIME Time);


            public struct SYSTEMTIME
            {
                public ushort wYear;
                public ushort wMonth;
                public ushort wDayOfWeek;
                public ushort wDay;
                public ushort wHour;
                public ushort wMinute;
                public ushort wSecond;
                public ushort wMilliseconds;


                public void FromDateTime(DateTime time)
                {
                    wYear = (ushort)time.Year;
                    wMonth = (ushort)time.Month;
                    wDayOfWeek = (ushort)time.DayOfWeek;
                    wDay = (ushort)time.Day;
                    wHour = (ushort)time.Hour;
                    wMinute = (ushort)time.Minute;
                    wSecond = (ushort)time.Second;
                    wMilliseconds = (ushort)time.Millisecond;
                }


                public DateTime ToDateTime()
                {
                    return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds);
                }

                public static DateTime ToDateTime(SYSTEMTIME time)
                {
                    return time.ToDateTime();
                }


            }
        
           public  CTimeChanger(int msOffset)
            {

                DateTime t = DateTime.Now.AddMilliseconds(msOffset);
                //Convert to SYSTEMTIME
                SYSTEMTIME st = new SYSTEMTIME();
                st.FromDateTime(t);
                //Call Win32 API to set time
                SetLocalTime(ref st);
            }


        }
}
