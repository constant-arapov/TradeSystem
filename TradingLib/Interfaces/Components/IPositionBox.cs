using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using TradingLib.Data;

namespace TradingLib.Interfaces.Components
{
    public interface IPositionBox
    {
        //TODO refactor !
        //Mutex mxListRowsPositions {get;set;}
        //List<CRawPosition> ListRawPos { get; }
		Dictionary<string, CRawPosition> DictPos { get; set; }

		// void Update(POS.position pos);
    }
}
