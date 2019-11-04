using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Data
{
    public class CFutLims
    {
        public CFutLims(decimal cl_quote, decimal lim_up, decimal lim_down)
        {

            Min = cl_quote - lim_down;
            Max = cl_quote + lim_up;

        }


        public decimal Max;
        public decimal Min;
       

    }
}
