using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;




namespace Terminal.Conf
{
    public class CStocksVisualConf : IXMLSerializable
    {

        public string FileName { get; set; }

        public bool NeedSelfInit { get; set; }

        public List<CStocksVisual> ListStocksVisual { get; set; }


        public CStocksVisualConf()
        {


        }

        public CStocksVisualConf(string path, bool needSelfInit = false)
        {
            FileName = path;
            NeedSelfInit = needSelfInit;

            ListStocksVisual = new List<CStocksVisual>();


            if (NeedSelfInit)
                SelfInit();

        }
        public void SelfInit()
        {
            ListStocksVisual.Add(new CStocksVisual 
                                    { ConNumm = 0, Ticker = "RTS-6.16" });

            ListStocksVisual.Add(new CStocksVisual 
                                       { ConNumm = 0, Ticker = "Si-6.16" });
        }



    }

    public class CStocksVisual
    {
        public int ConNumm { get; set; }
        public string Ticker { get; set; }
        public double Step { get; set; }
        public int Decimals { get; set; }

       
        public double WidthStock { get; set; }
        public double WidthClusters { get; set; }

        public CStocksVisual()
        {
            Ticker = "";


        }
        


    

    }


}
