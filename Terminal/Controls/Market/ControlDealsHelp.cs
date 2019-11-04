using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Diagnostics;



using Common;
using Common.Logger;


using Terminal.Common;

namespace Terminal.Controls.Market
{
    public partial  class ControlDeals
    {
        private Stopwatch swOnPnt1 = new Stopwatch();
        private Stopwatch swOnPnt2 = new Stopwatch();
        private Stopwatch swOnPnt3 = new Stopwatch();
        private Stopwatch swOnPnt4 = new Stopwatch();



        private Stopwatch swLvls1 = new Stopwatch();
        private Stopwatch swLvls2 = new Stopwatch();
        private Stopwatch swLvls3 = new Stopwatch();
        private Stopwatch swLvls4 = new Stopwatch();


        private CLogger _loggerOnPaint;
        private CLogger _loggerOnLevelsCondOrd;



        private void InitStopWatchOnPaint()
        {

            swOnPnt1.Restart();
            swOnPnt2.Restart();
            swOnPnt3.Restart();
            swOnPnt4.Restart();


        }



     
        private void InitStopWatchLvls()
        {
            swLvls1.Restart();
            swLvls2.Restart();
            swLvls3.Restart();
            swLvls4.Restart();


        }


     

        private void LogOnPaint(string message)
        {
            Log(ref _loggerOnPaint, "OnPaint", message);
        }

        private void LogLevelsCondOrd(string message)
        {

            Log(ref _loggerOnLevelsCondOrd, "LevelsCondOrder", message);

        }

        private void Log(ref CLogger  logger, string logname, string message)
        {
            

            if (logger == null)
            {
                if (TickerName != null && TickerName != Literals.Undefind)
                {
                    //2018-04-23 changed
                    //logger = new CLogger("ControlDeals_ " + logname + "____" + TickerName + "____" + DateTime.Now.ToString("yyy_MM_dd__hh_mm_ss"));
                    logger = new CLogger("ControlDeals_ " + logname + "____" + TickerName);
                    logger.Log(message);
                }
            }
            else
                logger.Log(message);
             
        }





    }
}
