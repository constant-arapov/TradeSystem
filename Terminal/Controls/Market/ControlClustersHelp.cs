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
    public partial class ControlClusters
    {
        private CLogger _logger;


        private void Log(string message)
        {
            if (_logger == null)
            {
                if (TickerName != null && TickerName != Literals.Undefind)
                {
                    _logger = new CLogger("ControlClusters_ " + TickerName);
                    _logger.Log(message);
                }
            }
            else
                _logger.Log(message);


        }

        private Stopwatch sw1 = new Stopwatch();
        private Stopwatch sw2 = new Stopwatch();
        private Stopwatch sw3 = new Stopwatch();
        private Stopwatch sw4 = new Stopwatch();

        private void InitStopWatch()
        {

            sw1.Restart();
            sw2.Restart();
            sw3.Restart();
            sw4.Restart();



        }
    }
}
