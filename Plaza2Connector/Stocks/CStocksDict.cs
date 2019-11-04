using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common;
using Common.Interfaces;

using TradingLib;
using TradingLib.Enums;
using TradingLib.ProtoTradingStructs;

namespace Plaza2Connector
{
    public class CStocksDict : Dictionary<Direction, List<CStock>>
    {


       
        public string FileName {get;set;}

        SerializebleStock m_internalStock;



        public CStocksDict(string isin, string dataPath)
        {
            FileName = dataPath + "\\"+isin + "_stocks.xml";
            m_internalStock = new SerializebleStock(FileName);


        }


        public void Dump()
        {
           m_internalStock.Bids = ((CStocksDict)this.MemberwiseClone())[Direction.Down];
           m_internalStock.Asks = ((CStocksDict)this.MemberwiseClone())[Direction.Up];
            CSerializator.Write<SerializebleStock>(ref m_internalStock);
        }


        /*
        public void Add(Direction dir, List<CStock> lst)
        {

            base.Add(dir, lst);

        }


        public void Clear()
        {
            base.Clear();

        }*/




    }

    



  public  class SerializebleStock  :       IXMLSerializable
    {
        public void SelfInit() { }
        public bool NeedSelfInit { get; set; }
        public string FileName { get; set; }

        public SerializebleStock(string fileName, bool needSelfInit=false)
        {

            FileName = fileName;
            NeedSelfInit = needSelfInit;
      

        }


        public SerializebleStock() {}


        public List<CStock> Bids { get; set; }

        public  List<CStock> Asks {get;set;}



    }

}
