using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib
{
    public interface IDataLogUpdateCommand <TDataLog>
    {

        Dictionary<string, List<TDataLog>> DictLog { get; set; }
        void Sort();



    }
}
