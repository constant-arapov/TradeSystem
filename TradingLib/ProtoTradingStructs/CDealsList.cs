using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;


namespace TradingLib.ProtoTradingStructs
{

    [ProtoContract]
    public class CDealsList
    {

        /*
        [ProtoMember(1)]
        public CProtoMessageHeader MessageHeader { get; set; }
          */    
        [ProtoMember(1)]
        public string Isin { get; set; }


        [ProtoMember(2)]
        public List<CDealClass> DealsList { get; set; }


       
        
        [ProtoMember(3)]
        public  DateTime DtBeforePack { get; set; }


        public CDealsList(string isin) : this ()
        {


           // Isin = isin.ToCharArray();
            Isin = isin;
            //MessageHeader = new CProtoMessageHeader();
            DealsList = new List<CDealClass>();
        }
        public CDealsList()
        {
            
        }
        

    }
}
