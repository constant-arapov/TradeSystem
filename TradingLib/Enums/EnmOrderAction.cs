using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Enums
{
    public enum EnmOrderAction : sbyte
    {       
        Deleted = 0,
        Added =1,
        Deal = 2,
        Update =3, //2018-03-05
		PartialFilled =4,
        Unknown = 127   



    }
}
