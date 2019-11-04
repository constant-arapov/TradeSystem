using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Collections
{
    public class CDictL1Simple<L1, TValue> : Dictionary <L1, TValue>
                                            where TValue : new()
    {
        protected void CreateIfNotExists(L1 L1key)
        {         
            if (!this.ContainsKey(L1key))
                this[L1key] = new TValue();


        }

    }
}
