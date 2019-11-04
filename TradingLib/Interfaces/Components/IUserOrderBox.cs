using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using TradingLib.Data;

namespace TradingLib.Interfaces.Components
{
    public interface IUserOrderBox
    {
        //TODO refactor !
        List<CRawOrdersLogStruct> ListRawOrdersStruct { get; }
        Mutex mxListRawOrders { get; set; }
        Dictionary<long, CRawOrdersLogStruct> DictUsersOpenedOrders { set; get; }
		


    }
}
