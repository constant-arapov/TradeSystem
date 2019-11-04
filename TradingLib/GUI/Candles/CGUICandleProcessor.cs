using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using TradingLib.Data;

using Common;
using Common.Collections;


namespace TradingLib.GUI.Candles
{
    class CGUICandleProcessor
    {
        public string Isin { get; set; }

        private CBlockingQueue<CStructTF> _q = new CBlockingQueue<CStructTF>();
        private  CGUICandleBox  _guiCandleBox;
    

        public CGUICandleProcessor(string isin, CGUICandleBox guiCandleBox)
        {

            Isin = isin;
            _guiCandleBox = guiCandleBox;

            (new Thread(ThreadFunc)).Start();
            

        }


        long tmp = 0;
        long old = 0;
        private void ThreadFunc()
        {

            while (!_guiCandleBox.IsTFIsinLastDataAccepted(Isin))
                Thread.Sleep(5);



            while (true)
            {

                CStructTF sc = _q.GetElementBlocking();
             

                _guiCandleBox.UpdateCandle(sc.Isin, sc.TF, sc.Dt,  sc.TFinfo);

            }
                


        }




      

        public void Add(string isin,string tf, string dt, CTimeFrameInfo tfi)
        {
            
 
            _q.Add(new CStructTF(isin, tf, dt, (CTimeFrameInfo)tfi.Copy()));

     

        }




    }
}
