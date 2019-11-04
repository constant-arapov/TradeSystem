using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;


namespace Common
{
      
    public class CPerfAnlzr
    {

        private bool _bNotFirstScan = false;
        private DateTime _dtSinceFirstScan = new DateTime(0);

        int _parSinceStart = 3;
     


        IAlarmable _alarmer;



        public CPerfAnlzr(int parSinceStart, IAlarmable _alarmer) 
           : this (_alarmer)
        {
            _parSinceStart = parSinceStart;
           // _parMaxMsecScan = parMaxMsecScan;

        }

        public CPerfAnlzr(IAlarmable alarmer)
        {
            _alarmer = alarmer;

        }

        /*public void CheckLim(double val, double maxVal, string msg)
        {


        }*/
        public void CheckLim(long val, long maxVal, string msg)
        {
            if (IsWorkingMode())            
                if (val > maxVal)
                    if (_alarmer !=null)
                    _alarmer.Error(msg + " val =" + val.ToString() + " maxVal=" + maxVal.ToString() + " ");                          

        }

        public bool IsWorkingMode()
        {

            
            if (_bNotFirstScan && ((DateTime.Now - _dtSinceFirstScan).Seconds > _parSinceStart))
                return true;

            if (!_bNotFirstScan)
            {
                _bNotFirstScan = true;
                _dtSinceFirstScan = DateTime.Now;
            }

            return false;
        }



    }
}
