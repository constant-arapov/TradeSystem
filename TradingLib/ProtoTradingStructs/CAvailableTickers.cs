using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using ProtoBuf;


namespace TradingLib.ProtoTradingStructs
{

    [ProtoContract]
    public class CAvailableTickers
    {
        [ProtoMember(1)]
        public List<CTIckerData> ListAvailableTickers { get; set; }

        public CAvailableTickers()
        {

            ListAvailableTickers = new List<CTIckerData>();

        }
    }

      [ProtoContract]
    public class CTIckerData
    {
          [ProtoMember(1)]
          public string TickerName { get; set; }

          [ProtoMember(2)]
          public decimal  Step { get; set; }


          [ProtoMember(3)]
          public int Decimals { get; set; }


          [ProtoMember(4)]
          public int DecimalVolume { get; set; }


          [ProtoMember(5)]
          public decimal minimum_order_size { get; set; }



    }


}
