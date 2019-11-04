using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBCommunicator
{
    public static class CodesDBServers
    {
        public static sbyte _01_FORTSDealing = 1;
        public static sbyte _02_VDS_Crypto = 2;

        private static Dictionary<sbyte, string> _dictCodesNames = 
            new Dictionary<sbyte,string>
        {
            {_01_FORTSDealing, "ФОРТС дилинг"},
            {_02_VDS_Crypto, "VDS крипто"}


        };


       private static string GetName(sbyte code)
       {
           string outSt ="";
           if (_dictCodesNames.TryGetValue(code, out outSt))
               return outSt;
           else
               throw new ApplicationException("CodesDB unknown code");


           

       }

    }

   


}
