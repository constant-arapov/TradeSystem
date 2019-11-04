using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Text.RegularExpressions;

namespace Common.Utils
{
    public static class CUtilRegular
    {

        public static bool  Contains(string expr, string content)
        {
            Regex regex = new Regex("("+expr+")");
            Match m = regex.Match(content);

            if (m.Groups.Count > 1)
                return true;



            return false;
        }

    }
}
