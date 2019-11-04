using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common.Interfaces;

namespace TradingLib.Interfaces.Clients 
{
    public interface IClientPosBox : IAlarmable
    {
        void UpdateTotalInstrumentPosition();
    }
}
