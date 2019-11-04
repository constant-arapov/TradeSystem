using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
     [Serializable]
    public class CClone
    {

        public object Copy()
        {
            return this.MemberwiseClone();
        }


    }
}
