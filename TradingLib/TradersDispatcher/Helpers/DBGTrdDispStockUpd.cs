using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Common.Interfaces;
using Common.Logger;

using TradingLib.Enums;
using TradingLib.ProtoTradingStructs;

using TradingLib.Data;


namespace TradingLib.TradersDispatcher.Helpers
{
    public class DBGTrdDispStockUpd
    {
        private Dictionary<string, ILogable> _loggers = new Dictionary<string, ILogable>();
        private bool _isOn = false;

     


        public DBGTrdDispStockUpd(bool isOn, Dictionary<string,long> dictInstr)
        {
            if (!isOn)
                return;

            _isOn = isOn;
        

          foreach (var kvp in dictInstr)
          {
                _loggers[kvp.Key] = new CLogger(String.Format("DBGTrdDispStockUpd_{0}", kvp.Key),
                                                  flushMode:true, subDir:"DBGTrdDispStockUpd");

                Log(kvp.Key, "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Log(kvp.Key, "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++  STARTED ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Log(kvp.Key, "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            }

         

        }


        public void PrintQueue(string instrument, int prec, Direction dir, List<CCmdStockChange> queue )
        {
            if (!_isOn)
                return;

            string st =  String.Format(String.Format("prec={0} dir={1} ==>", prec, dir));


          
            foreach (var el in queue)
                st += String.Format("{0} {1} {2} |", el.Code.ToString(), el.Price, el.Volume);

           Log(instrument, st);

          



        }



        public void PrintStock(string instrument, CStockClass stockClass)
        {

            if (!_isOn)
                return;

            
            
            foreach (var kvp in stockClass.StockListAsks)
            {
                Log(instrument, String.Format("prec={0}", kvp.Key));

                string st = "";
                foreach (var el in kvp.Value)                                                                   
                    st += String.Format("{0} {1}|", el.Price, el.Volume);

                 Log(instrument, st);
                
            }

            Log(instrument,"");
            Log(instrument, "");

            foreach (var kvp in stockClass.StockListBids)
            {
                Log(instrument, String.Format("prec={0}", kvp.Key));

                string st = "";
                foreach (var el in kvp.Value)
                    st += String.Format("{0} {1}|", el.Price, el.Volume);

                Log(instrument, st);

            }



            Log(instrument, "=======================================================================================================================================");


        }



        /*
        public void PrintStock(string instrument, CStockClass sharedStock)
        {
            if (!_isOn)
                return;



            string st = "";
            foreach (var kvp in sharedStock.QueueCmdStockExch)
            {
                st = String.Format("prec={0}",kvp.Key);
                foreach(var el in kvp.Value)                 
                    st += String.Format("{0} {1} {2}", el.Code.ToString(), el.Price, el.Volume);

                Log(instrument,st);
              
            }

            foreach (var kvp in sharedStock)
            {
                Log(instrument, String.Format("dir={0}", kvp.Key));
              
                foreach (var kvp2 in kvp.Value)
                {
                    Log(instrument,String.Format("prec={0}", kvp2.Key));
                    st = "";
                    
                    foreach (var el in kvp2.Value)
                        st += String.Format("{0} {1}|", el.Price, el.Volume);

                    Log(instrument, st);
                }

             
                Log(instrument, "");
            }

            Log(instrument, "=======================================================================================================================================");

        }
        */

        public void Log(string instrument, string msg)
        {
            _loggers[instrument].Log(msg);

        }


    }
}
