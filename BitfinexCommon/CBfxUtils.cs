using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Utils;

using TradingLib.Enums;


using BitfinexCommon.Enums;


namespace BitfinexCommon
{
    public class CBfxUtils
    {
        public static EnmBfxOrderStatus GetOrderStatus(string inStatus)
        {

              string [] stArr  =   inStatus.Split(':');
              
            if (stArr == null || stArr.Count() == 0)
                  throw new ApplicationException("CBfxUtils.GetOrderStatus");


            string status = stArr[0];


            if (string.IsNullOrWhiteSpace(status))
                return EnmBfxOrderStatus.Undefined;
            var safe = status.ToLower().Trim();
            if (safe.StartsWith("active"))
                return EnmBfxOrderStatus.Active;
            else if (safe.StartsWith("executed"))
                return EnmBfxOrderStatus.Executed;
            else if (safe.StartsWith("partially filled"))
                return EnmBfxOrderStatus.PartiallyFilled;
            else if (safe.StartsWith("canceled"))
                return EnmBfxOrderStatus.Canceled;
            //why ?
            /*  switch (safe)
              {
                  case "active":
                  case var s when s.StartsWith("active"):
                      return Orde.Active;
                  case "executed":
                  case var s when s.StartsWith("executed"):
                      return OrderStatus.Executed;
                  case "partially filled":
                  case var s when s.StartsWith("partially filled"):
                      return OrderStatus.PartiallyFilled;
                  case "canceled":
                  case var s when s.StartsWith("canceled"):
                      return OrderStatus.Canceled;
              }
             **/
            //Log.Warning("Can't parse OrderStatus, input: " + safe);
            return EnmBfxOrderStatus.Undefined;
        }

        public static int GetPrecInt(string stPrec)
        {
            return Convert.ToInt32(stPrec.Remove(0, 1));


        }



        public static decimal GetDealPriceFromStatus(string inStatus)
        {
            string status = inStatus.Replace('.', ',');

            string[] stArr = status.Split(':');

            if (stArr == null || stArr.Count() == 0)
                throw new ApplicationException("CBfxUtils.GetDealPriceFromStatus");


            string [] stDataArr = stArr[0].Split('@');
            if (stDataArr == null || stDataArr.Count() != 2)
                throw new ApplicationException("CBfxUtils.GetDealPriceFromStatus 1");


            decimal price = 0m;
          
            string []stPriceData =  stDataArr[1].Split('(');
            price = Convert.ToDecimal(stPriceData[0]);
            if (stDataArr[1].Contains("("))
            {
                //don't now what is it may be we'll need in the future
                string secondParam = stPriceData[1].Trim(')');
                decimal dcmlSecParam = Convert.ToDecimal(secondParam);
              
                

            }
          



            return price;


            
        }



        public static EnmBfxOrderType GetOrderType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return EnmBfxOrderType.Undefined;
            var safe = type.ToLower().Trim();


            if (safe == "market")
                return EnmBfxOrderType.Market;

            else if (safe == "exchange market")
                return EnmBfxOrderType.ExchangeMarket;

            else if (safe == "limit")
                return EnmBfxOrderType.Limit;

            else if (safe == "exchange limit")
                return EnmBfxOrderType.ExchangeLimit;

            else if (safe == "trailing stop")
                return EnmBfxOrderType.ExchangeTrailingStop;

            else if (safe == "exchange trailing stop")
                return EnmBfxOrderType.ExchangeTrailingStop;

            else if (safe == "stop")
                return EnmBfxOrderType.Stop;

            else if (safe == "exchange stop")
                return EnmBfxOrderType.ExchangeStop;

            else if (safe == "stop limit")
                return EnmBfxOrderType.StopLimit;

            else if (safe == "exchange stop limit")
                return EnmBfxOrderType.ExchangeStopLimit;

            else if (safe == "exchange fok")
                return EnmBfxOrderType.ExchangeFok;

            else if (safe == "fok")
                return EnmBfxOrderType.Fok;

            /*
        case "market":
        case var s when s.StartsWith("market"):
            return OrderType.Market;
        case "exchange market":
        case var s when s.StartsWith("exchange market"):
            return OrderType.ExchangeMarket;
        case "limit":
        case var s when s.StartsWith("limit"):
            return OrderType.Limit;
        case "exchange limit":
        case var s when s.StartsWith("exchange limit"):
            return OrderType.ExchangeLimit;
        case "trailing stop":
        case var s when s.StartsWith("trailing stop"):
            return OrderType.TrailingStop;
        case "exchange trailing stop":
        case var s when s.StartsWith("exchange trailing stop"):
            return OrderType.ExchangeTrailingStop;
        case "stop":
        case var s when s.StartsWith("stop"):
            return OrderType.Stop;
        case "exchange stop":
        case var s when s.StartsWith("exchange stop"):
            return OrderType.ExchangeStop;
        case "stop limit":
        case var s when s.StartsWith("stop limit"):
            return OrderType.StopLimit;
        case "exchange stop limit":
        case var s when s.StartsWith("exchange stop limit"):
            return OrderType.ExchangeStopLimit;
        case "fok":
        case var s when s.StartsWith("fok"):
            return OrderType.Fok;
        case "exchange fok":
        case var s when s.StartsWith("exchange fok"):
            return OrderType.ExchangeFok;
             */

            //Log.Warning("Can't parse OrderStatus, input: " + safe);
            return EnmBfxOrderType.Undefined;
        }

        public static string GetOrderTypeString(EnmBfxOrderType type)
        {
            switch (type)
            {
                case EnmBfxOrderType.Market:
                    return "MARKET";
                case EnmBfxOrderType.ExchangeMarket:
                    return "EXCHANGE MARKET";
                case EnmBfxOrderType.Limit:
                    return "LIMIT";
                case EnmBfxOrderType.ExchangeLimit:
                    return "EXCHANGE LIMIT";
                case EnmBfxOrderType.TrailingStop:
                    return "TRAILING STOP";
                case EnmBfxOrderType.ExchangeTrailingStop:
                    return "EXCHANGE TRAILING STOP";
                case EnmBfxOrderType.Stop:
                    return "STOP";
                case EnmBfxOrderType.ExchangeStop:
                    return "EXCHANGE STOP";
                case EnmBfxOrderType.StopLimit:
                    return "STOP LIMIT";
                case EnmBfxOrderType.ExchangeStopLimit:
                    return "EXCHANGE STOP LIMIT";
                case EnmBfxOrderType.Fok:
                    return "FOK";
                case EnmBfxOrderType.ExchangeFok:
                    return "EXCHANGE FOK";
            }
            throw new ApplicationException("Not supported order type");
        }

       


    



		

        public static string RemoveFirstT(string instrument)
        {
            return instrument.Remove(0, 1);
        }

    




    }
}
