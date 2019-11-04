using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common.Interfaces;


using TradingLib.GUI;




namespace ASTS.Tables
{
    public class CTableMarkets : CBaseTable
    {
      
        public CTableMarkets(string name, IAlarmable alarmer)
            : base(name, alarmer)
        {
          
        }

    }
}
