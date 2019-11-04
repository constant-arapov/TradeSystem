using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Collections
{
    public class CDict_L2_List<L1, L2, TValue> : Dictionary<L1, Dictionary<L2, List<TValue>>>
                                                        where TValue : new()
    {


        public void CreateIfNotExists(L1 L1Key, L2 L2Key)
        {
            if (!this.ContainsKey(L1Key))
                this[L1Key] = new Dictionary<L2, List <TValue>>();

            if (!this[L1Key].ContainsKey(L2Key))
                this[L1Key][L2Key] = new List<TValue>();

        }

        //note ovverride new val 
        public void Update(L1 L1Key, L2 L2Key, TValue val)
        {
            //thrade safe ?
            lock (this)
            {
                CreateIfNotExists(L1Key, L2Key);

                this[L1Key][L2Key].Add(val);

            }
        }
        /*
        public TValue GetLast(L1 L1Key, L2 L2Key)
        {
            lock (this)
            {
                CreateIfNotExists(L1Key, L2Key);
                return this[L1Key][L2Key].Last();
            }
        }
        */

    }
}
