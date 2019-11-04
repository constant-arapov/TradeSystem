using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.BotEvents
{
    public class CBotEventStruct
    {


        public CBotEventStruct(string isin, EnmBotEventCode evCode, object data)
        {

            DtTmEvent = DateTime.Now;
            EventCode = evCode;
            Isin = isin;
            Data = data;
        }

        public DateTime DtTmEvent;
        public EnmBotEventCode EventCode;
        public String Isin;
        public object Data;

    }
}
