using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Newtonsoft.Json;

using BitfinexCommon.Messages.Response.Converters;

namespace BitfinexCommon.Messages.Response
{
    [JsonConverter(typeof(ResponsePositionsConverter))]
    public class ResponsePositions
    {
        public string Symbol {get;set;}
        public string Status {get;set;} //Status (ACTIVE, CLOSED).
        public double Amount {get;set;}  //Positive values means a long position, negative values means a short position.	
        public double Base_price {get;set;} //The price at which you entered your position.
        public double Margin_funding {get;set;}//The amount of funding being used for this position.
        public int    Margin_funding_type {get;set;}//0 for daily, 1 for term.
        public double PL {get;set;}		 //Profit & Loss
        public double PL_perc {get;set;} //Profit & Loss Percentage
        public double Price_liq	{get;set;}	//Liquidation price
        public double Leverage { get; set; }	//Beta value


        public override string ToString()
        {
            return String.Format
            ("Symbol={0} Status={1} Amount={2} BasePrice={3} Price_liq={4}  PL={5} Leverage={6}",
                                    Symbol, //0
                                    Status, //1
                                    Amount, //2
                                    Base_price,//3
                                    Price_liq,//4
                                    PL, //5
                                    Leverage//6
                                       );

                                       
        }


    }

  


}
