using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using Terminal.DataBinding;






namespace Terminal.Conf
{
    [Serializable]
    public class CInstrumentConfig : IXMLSerializable
    {

        public string FileName { get; set; }


        public string Name { get; set; }
        public List <decimal> WorkAmounts { get; set; }
       // public CStockProperties StockProperties { get; set; }
        public CMarketProperties MarketProperties {get;set;}

           
        public bool NeedSelfInit { get; set; }


        public CInstrumentConfig()
        {

        }


        public CInstrumentConfig (string name,string filename, bool needSelfInit=false)
        {

          NeedSelfInit = needSelfInit;
           FileName = filename;

           WorkAmounts = new List<decimal>();


           Name = name;

           MarketProperties = new CMarketProperties
                                {
                                    StockProperties = new CStockProperties(),
                                    DealsProperties = new CDealsProperties(),
                                    ClusterProperties = new CClusterProperties()
                                };



                        
           
          




        //    Name = "";

       }


        public void SelfInit()
        {
           //  Instrument1 = 1;
            WorkAmounts.Add(1);
            WorkAmounts.Add(2);
            WorkAmounts.Add(3);
            WorkAmounts.Add(4);
            WorkAmounts.Add(5);


            MarketProperties.StockProperties.TickerName = Name;
            MarketProperties.StockProperties.StringHeight = "11";
            MarketProperties.StockProperties.Step = "1";
            MarketProperties.StockProperties.Decimals = "0";
           
            MarketProperties.DealsProperties.StringHeight = "11";          
           


            MarketProperties.ClusterProperties.StringHeight = "11";


        }




    }
}
