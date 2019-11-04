using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;


using Common.Interfaces;
using Common.Logger;

namespace BitfinexWebSockConnector.Helpers
{
    public class CPerfStockStor
    {

      

        private bool _isEnebled;
        Dictionary<int, ILogable> _dictLogger = new Dictionary<int, ILogable>();

        private Stopwatch _swThread;

        public CPerfStockStor(bool isEnabled, string instrument, List<int> lstPrecissions)
        {
            _isEnebled = isEnabled;

            lstPrecissions.ForEach(perc =>
            {
                _dictLogger[perc] = new CLogger(String.Format(@"perfStockStor_{0}_{1}", instrument, perc),
                                                 flushMode: true, subDir: "Perf");
                PrinstStartBanner(perc);

                                                }
           );


            _swThread = new Stopwatch();
        }

        private void Log(int perc, string msg)
        {
            _dictLogger[perc].Log(msg);
        }

        private void PrinstStartBanner(int prec)
        {
            Log(prec, "================================================ STARTED ===================================");


        }


        public  void UpdStart()
        {
            if (!_isEnebled)
                return;


            _swThread.Restart();
        }

        public void UpdEnd(int perc)
        {
            if (!_isEnebled)
                return;

            Log(perc, GetPerfMsg("updStart", _swThread));
            _swThread.Stop();
        }


        public string GetPerfMsg(string msg, Stopwatch sw)
        {
            return String.Format("{0} ms={1} ticks={2}", msg, sw.ElapsedMilliseconds, sw.ElapsedTicks);
        }

    }
}
