using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;

namespace MsgRetransmitter
{
    static class Helpers
    {
        public static string HtmlEncode(this string inputString)
        {
            return HttpUtility.HtmlEncode(inputString);
        }


    }
}
