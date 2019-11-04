using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Interfaces;

using Common.Logger;


using TradingLib.Enums;
using TradingLib.ProtoTradingStructs;


namespace Terminal.Communication.Helpers
{
    class DBGDataRcvr
    {

        private ILogable _logger;

        private bool _isOn;

        private Dictionary<string, ILogable> _dictLoggers = new Dictionary<string, ILogable>();


        public DBGDataRcvr(bool isOn)
        {
            _isOn = isOn;
            if (!_isOn)
                return;


         
        }



        private void Log(string instr, string msg)
        {

            if (!_dictLoggers.ContainsKey(instr))
            {
                _dictLoggers[instr] = new CLogger(String.Format("DBGDataRcvr_{0}", instr),
                                                        subDir: "DBGDataRcvr");
                _dictLoggers[instr].Log("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                _dictLoggers[instr].Log("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ STARTED ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++=++++++++++++++++++++++++++++++++++");
                _dictLoggers[instr].Log("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

            }
            _dictLoggers[instr].Log(msg);

        }



        public void PrintData(CStockClass source,  CStockClass dest)
        {
            if (!_isOn)
                return;

            string instr = source.Isin;

            Log(instr,"[SOURCESTOCK]");
            PrintOneStock(instr,source);

            Log(instr,"instr++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

            
            foreach (var kvp in source.QueueCMDStockChng[Direction.Down])
            {
                int prec = kvp.Key;
                string st = String.Format("prec={0} =>",prec);


                if (kvp.Value == null)
                    continue;


                foreach (var el in kvp.Value)
                   st += String.Format("{0} p={1} v={2}|",el.Code, el.Price, el.Volume);

                Log(instr,st);
              
            }
          


            Log(instr,"[DSTSTOCK]");
            PrintOneStock(instr,dest);
            Log(instr,"______________________________________________________________________________________________________________________________________________________________________________");


          
                foreach (var kvp in source.StockListBids)
                {
                    int prec = kvp.Key;

                    if (source.StockListBids[prec] == null)
                        continue;

                    int cntDest =   dest.StockListBids.Count ==0 || dest.StockListBids[prec] == null ? 0 : dest.StockListBids[prec].Count;

                    for (int i = 0; i <  Math.Min(source.StockListBids[prec].Count, cntDest); i++)
                    {
                        if (source.StockListBids[prec][i].Price != dest.StockListBids[prec][i].Price ||
                            source.StockListBids[prec][i].Volume != dest.StockListBids[prec][i].Volume)
                        {
                            Log(instr,String.Format("ERROR ! prec={0} i={1} Src.Price={2} Src.Volume={3} dstPrice={4} dstVolume={5} ",
                                            prec, //0
                                            i,//1
                                            source.StockListBids[prec][i].Price,//2
                                            source.StockListBids[prec][i].Volume,//3
                                            dest.StockListBids[prec][i].Price,//4
                                            dest.StockListBids[prec][i].Volume)//5
                                            );
                                            
                        }

                    }


                }

            //2018-07-09
            //check consitance
            foreach (var kvp in source.StockListBids)
            {

                int prec = kvp.Key;


                if (dest.StockListBids[prec] == null)
                    continue;

                //tempo for FORTS
                /*
                decimal maxBidPrice = dest.StockListBids[prec][0].Price;

                for (int i = 0; i < dest.StockListAsks[prec].Count; i++)
                    if (dest.StockListAsks[prec][i].Price <= maxBidPrice)
                        Log(instr, String.Format("ERROR ! ask<=maxBidPrice  prec={0} bid={1} ask={2}",
                                                           prec,
                                                           maxBidPrice,
                                                           dest.StockListAsks[prec][i].Price));

                decimal minAskPrice = dest.StockListAsks[prec][0].Price;


                for (int i = 0; i < dest.StockListBids[prec].Count; i++)
                    if (dest.StockListBids[prec][i].Price >= minAskPrice)
                        Log(instr, String.Format("ERROR ! bid >= minAskPrice prec={0} bid={1} ask={2}",
                                                           prec,
                                                           maxBidPrice,
                                                           dest.StockListAsks[prec][i].Price));
                  */                                         


            }

                Log(instr,"==============================================================================================================================================================================================");


        }

        private void PrintOneStock(string instr,CStockClass stock)
        {
            


            foreach (var kvp in stock.StockListBids)
            {
                int prec = kvp.Key;

                Log(instr,String.Format("prec={0}", prec));
                string st = "";


                if (kvp.Value == null)
                    continue;

                foreach (var el in kvp.Value)
                    st += String.Format("{0} {1}|", el.Price, el.Volume);

                Log(instr,st);

            }


        }







    }
}
