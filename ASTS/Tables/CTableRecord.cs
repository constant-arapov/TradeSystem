using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTS.Tables
{

    // Very simple record.
    public class CTableRecrd
    {
        internal int decimals;
        internal Dictionary<string, object> values = new Dictionary<string, object>();
    }
}
