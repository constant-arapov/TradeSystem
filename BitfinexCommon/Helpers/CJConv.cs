using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;


namespace BitfinexCommon.Helpers
{
    public static class CJConv
    {
        public static double ConvDbl(JToken jtoken)
        {
            return (jtoken == null || jtoken.Type == JTokenType.Null ) ?  (double) 0 : (double)jtoken;
        }


        public static int ConvInt(JToken jtoken)
        {
            return (jtoken == null || jtoken.Type == JTokenType.Null) ? (int)0 : (int)jtoken;
        }

        public static long ConvLong(JToken jtoken)
        {
            return (jtoken == null || jtoken.Type == JTokenType.Null) ? (long)0 : (long)jtoken;
        }

        public static long ConvLong(long? value)
        {
            return value == null ? 0 : (long)value;

        }



    }
}
