using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;


namespace Terminal.TradingStructs
{
     [Serializable]
     public class CDeal
     {
         public CDeal()
         {


         }



        public int Amount { get; set; }
        public DateTime DateTime { get; set; }
        public EnmDealDirection Direction { get; set; }
       // public Point Point { get; set; }
        public double Price { get; set; }

      }
}
