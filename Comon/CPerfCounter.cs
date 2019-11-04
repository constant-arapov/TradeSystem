using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;


namespace Common
{
    public  class CPerfCounter
    {

        PerformanceCounter _perfCntr;

        public  CPerfCounter(string cathegoryName, string counterName)
        {


            string procName = Process.GetCurrentProcess().ProcessName;

            _perfCntr = new PerformanceCounter(cathegoryName, counterName, procName);
            
        }

        public long GetValue()
        {

            return _perfCntr.RawValue;

        }

        public long GetMB()
        {
            return _perfCntr.RawValue / 1024 / 1024;

        }


        public long GetMillions()
        {
            return _perfCntr.RawValue / 1000000;

        }




        public  static List<string> GetCathegoriesList()
        {
            List<string> lst = new List<string>();


            foreach (var cat in PerformanceCounterCategory.GetCategories())
                lst.Add(cat.CategoryName);

            return lst;
        }

  



       public static PerformanceCounterCategory GetCathegoryByName(string cathegoryName)
        {

            foreach (var cat in PerformanceCounterCategory.GetCategories())
                if (cat.CategoryName == cathegoryName)
                    return cat;


            return null;

        }

      



        public static List<string> GetCountersList(string appName,  string cathegoryName)
        {
            List<string> lst = new List<string>();

            var cat = GetCathegoryByName(cathegoryName);
            if (cat != null)
                foreach (var cntr in cat.GetCounters(appName))
                    lst.Add(cntr.CounterName);
                
            return lst;
        }

       


    }
}
