using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Collections
{
    public class CDictL2Simple<L1, L2, TValue> : Dictionary<L1, Dictionary<L2, TValue>>
                                                        where TValue : new()
    {

        protected void CreateIfNotExists(L1 L1Key, L2 L2Key)
        {
            if (!this.ContainsKey(L1Key))
                this[L1Key] = new Dictionary<L2, TValue>();

            if (!this[L1Key].ContainsKey(L2Key))
                this[L1Key][L2Key] = new TValue();

        }

        public bool IsExists(L1 L1key, L2 L2key)
        {
            if (this.ContainsKey(L1key))
                if (this[L1key].ContainsKey(L2key))
                    return true;


            return false;
        }



        public void Update(L1 L1Key, L2 L2Key, TValue val)
        {
                CreateIfNotExists(L1Key, L2Key);

                this[L1Key][L2Key] = val;

           
        }

        


    }
}
