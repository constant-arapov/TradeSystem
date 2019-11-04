using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Interfaces.Interaction
{
    public interface IDataFormatForCntrlInstr
    {
        int GetPriceFormat(string instrument);

		int GetVolumeFormat(string instrument);

    }
}
