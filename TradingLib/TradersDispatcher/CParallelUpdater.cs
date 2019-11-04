using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;


namespace TradingLib.TradersDispatcher
{
    /// <summary>
    /// Class for parallel update by request.
    /// On update calls requested action
    /// 
    /// Using for update deals and stocks.
    /// 
    /// </summary>
    public class CParallelUpdater
    {
        private AutoResetEvent _evUpdate = new AutoResetEvent(false);

        public delegate void delegUpdate(string isin);
        private delegUpdate _calbackUpdate;
        private string _isin;

        public CParallelUpdater(string isin,delegUpdate requestedAction)
        {
            _calbackUpdate = requestedAction;
            _isin = isin;

            Thread th = new Thread(ThreadFunc);
            th.Start();
            th.Priority = ThreadPriority.Highest;
           
            //(new Thread(ThreadFunc)).Start();
        }

        public void Update()
        {
            _evUpdate.Set();
        }

        private void ThreadFunc()
        {
            while (true)
            {
                _evUpdate.WaitOne();
                _calbackUpdate(_isin);

            }

        }

    }
}
