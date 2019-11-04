using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common;
using Common.Interfaces;



namespace TradingLib.Interfaces.Clients
{
    public interface IClientGUIBox : IClientGUICandleBox
    {
        bool IsAllInstrAllMarketsAvailable {get;}
        bool AnalzyeTimeFrames { get; set; }
        CAlarmer Alarmer { get; }
   
    }
}
