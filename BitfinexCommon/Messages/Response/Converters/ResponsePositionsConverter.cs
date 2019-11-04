using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using BitfinexCommon.Helpers;

using BitfinexCommon.Messages.Response;


namespace BitfinexCommon.Messages.Response.Converters
{
   
    public class ResponsePositionsConverter : BaseResponseConverter<ResponsePositions>
    {

     
        

        protected override ResponsePositions JArrayToTradingTicker(JArray array)
        {
            ResponsePositions rp = new ResponsePositions();
            
            rp.Symbol =  (string) array[0];
            rp.Status =  (string) array[1];
            rp.Amount = ConvDbl(array[2]);
            rp.Base_price = ConvDbl(array[3]);
            rp.Margin_funding = ConvDbl(array[4]);
            rp.Margin_funding_type  = ConvInt(array[5]);
            rp.PL = ConvDbl(array[6]);
            rp.PL_perc  = ConvDbl(array[7]);
            rp.Price_liq = ConvDbl(array[8]);
            rp.Leverage = ConvDbl(array[9]);

            return rp;
        }

       


    }
}
