using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Interfaces;
using Common.Logger;

using TradingLib.Enums;
using TradingLib.Data;

namespace Plaza2Connector
{
    class CCommonPosition
    {
        public void Recalc(CRawOrdersLogStruct ol, CLogger logger)
        {
            decimal  oldAmount =  Amount; 
            Amount= (ol.Dir == (sbyte)OrderDirection.Buy) ?   Amount + ol.Amount : Amount - ol.Amount;
            if (Amount != 0)
            {
                //new del has same dir as pos
                if ((oldAmount >= 0 && ol.Dir == (sbyte)OrderDirection.Buy) || (oldAmount <= 0 && ol.Dir == (sbyte)OrderDirection.Sell))
                    AvPos = (AvPos * Math.Abs(oldAmount) + ol.Deal_Price * ol.Amount) / (Math.Abs(oldAmount) + ol.Amount);
                else //new dir has another direction and more than pos
                if ( (Math.Abs(ol.Amount) > Math.Abs(oldAmount)   ))
                    AvPos = ol.Deal_Price;
            }
            else
            {
                AvPos = 0;


            }
            logger.Log("Amount="+Amount+" AvPos="+AvPos.ToString("0.0")+" | oldAmount=" + oldAmount + " | ol.Price="+ol.Deal_Price +" ol.Amount=" +ol.Amount+ " ol.Dir="+ol.Dir+" ol.Id_ord="+ol.Id_ord);
   
       
        }
       


       public decimal Amount =0;
       public decimal AvPos=0;

    

    }
}
