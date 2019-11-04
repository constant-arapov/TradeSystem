using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitfinexWebSockConnector.Data
{

    public class CBfxStorStorMsg
    {
        public EnmStockMsgUpd Event;
        public Object Data;

    }

    public enum EnmStockMsgUpd : sbyte
    {
        UpdateStock,
        UpdateSnapshot

    }

}
