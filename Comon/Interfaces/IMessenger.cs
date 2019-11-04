using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using TradingLib.Enums;

namespace Common.Interfaces
{
    public interface IMessenger
    {
         byte[] GetBinaryMessageHeaderAndBody(byte[] message, ref /*enmTradingEvent*/byte ev);
         byte[] GenBinaryMessageHeader(/*enmTradingEvent*/byte enmEvnt);
         

    }
}
