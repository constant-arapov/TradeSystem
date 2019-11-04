using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Text.RegularExpressions;

namespace Plaza2Connector
{
    public static class CUtilConnection
    {

        public static bool IsConnectionOrderControl(string st, ref int botId)
        {

            Regex newReg = new Regex(@"ConnectionOrderControl_([0-9]+)");
            Match m = newReg.Match(st);

            if (m.Groups.Count > 1)
            {
                botId = Convert.ToInt32(m.Groups[1].ToString());
                return true;
            }
            return false;
        }


    }
}
