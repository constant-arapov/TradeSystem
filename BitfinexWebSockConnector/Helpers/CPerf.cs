using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Common.Interfaces;
using Common.Logger;

using System.Diagnostics;


namespace BitfinexWebSockConnector.Helpers
{
    class CPerfOrdBook
    {
        private ILogable _loggerOrdBookUpd;
        private ILogable _loggerOrdBookUpdSnapshot;


        private Stopwatch _swOrdBookUpd = new Stopwatch();
        private Stopwatch _swOrdBookSnapshot = new Stopwatch();

        private bool _isEnebled;

        public CPerfOrdBook(bool isEnabled)
        {
            _isEnebled = isEnabled;
            _loggerOrdBookUpd = CreateLogger("perfOrdBookUpd");
            _loggerOrdBookUpdSnapshot = CreateLogger("perfOrdBookSnapshot", flushMode:true);

        }

        private ILogable CreateLogger(string name, bool flushMode=false)
        {

            return new CLogger(name, flushMode: flushMode, subDir: "Perf");

        }



       
        public void StartOrdeBookUpd()
        {
            if (!_isEnebled)
                return;

            _swOrdBookUpd.Restart();


        }


        public void EndOrderBookUpd()
        {
            if (!_isEnebled)
                return;

          

            _swOrdBookUpd.Stop();
            _loggerOrdBookUpd.Log(GetPerfMsg("OrdBookUpd", _swOrdBookUpd));

        }

        public string GetPerfMsg(string msg,Stopwatch sw)
        {
            return String.Format("{0} ms={1} ticks={2}",msg, sw.ElapsedMilliseconds,sw.ElapsedTicks);
        }


        public void StartOrdeBookSnapshot()
        {
            if (!_isEnebled)
                return;

            _swOrdBookSnapshot.Restart();


        }


        public void EndOrderBookSnapshot()
        {
            if (!_isEnebled)
                return;



           _swOrdBookSnapshot.Stop();
           _loggerOrdBookUpdSnapshot.Log(GetPerfMsg("OrdBookSnapshot", _swOrdBookSnapshot));

        }


    }
}
