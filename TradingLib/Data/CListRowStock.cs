using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib.Data;

using Common;



namespace TradingLib
{
    public class CListRowStock : List<CRawStock>, IXMLSerializable
    {
        public string FileName { get; set; }
        public bool NeedSelfInit { get; set; }
        public void SelfInit() { }


        public CListRowStock(string isin,string dataPath)
        {
            FileName = dataPath+ "\\" + isin + "_rawstocks.xml";
          
        }

        public CListRowStock()
        {



        }


        public object Copy ()
        {
            return this.MemberwiseClone();


        }


       
    }
}
