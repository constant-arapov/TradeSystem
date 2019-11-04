using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLib.Interfaces.Clients
{
    public interface IClientSharedStock
    {

        List<int> GetPricePrecisions();

        int GetStockDepth(int precision);

    }
}
